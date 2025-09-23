namespace EasyLogger;

/// <summary>
/// Writer for logging to a new file.
/// Will automatically close the file when it is closed.
/// Files created can be read and written while the logger is open.
/// </summary>
[PublicAPI]
public class FileLogWriter : ILogWriter {
    private readonly string _logDirectory;
    private readonly string? _logName;
    private readonly string? _id;

    private FileStream _logStream = null!;
    private StreamWriter _writer = null!;

    /// <summary>
    /// Create a new FileLogWriter using a specific path.
    /// </summary>
    /// <param name="logPath">
    /// May be either a file or a directory.
    /// A filepath will create a log at the specified path.
    /// A path to a directory will automatically generate the log's name based on the logger's start time,
    ///   and then create a log file with that name in the provided directory.
    /// </param>
    public FileLogWriter(string logPath) {
        if (Path.HasExtension(logPath)) {
            // Direct path to a file
            _logName = Path.GetFileName(logPath);
            _logDirectory = logPath[..^_logName.Length];
        } else {
            // Path to directory, we need to generate a filename ourselves in StartLog
            _logDirectory = logPath;
            _logName = null;
        }

        _id = null;
    }

    /// <summary>
    /// Create a new FileLogWriter using a path and an id.
    /// The name of the log file will be automatically generated from the logger's start time and id.
    /// </summary>
    /// <param name="logDirectory">The path to the directory the log should be created in.</param>
    /// <param name="id">The id of the log.</param>
    public FileLogWriter(string logDirectory, string id) {
        _logDirectory = logDirectory;
        _logName = null;
        _id = id;
    }

    public void StartLog(Logger logger) {
        string name;

        if (_logName is null) {
            string timestamp = logger.GetTimeStamp(logger.Config.StartTime);
            name = _id is not null ? $"{timestamp}_{_id}.log" : $"{timestamp}.log";
        } else {
            name = _logName;
        }

        string path = Path.Join(_logDirectory, name);
        CreateLog(path);
    }

    private void CreateLog(string path) {
        if (File.Exists(path)) throw new IOException($"Log file '{path}' already exists.");

        _logStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
        _writer = new StreamWriter(_logStream);
    }

    public void Log(string message) {
        _writer.WriteLine(message);
    }

    public void Flush() {
        _writer.Flush();
    }
}
