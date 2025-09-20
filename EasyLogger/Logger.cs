using System.Runtime.CompilerServices;
using System.Text;

namespace EasyLogger;

[PublicAPI]
public class Logger {
    private const string LOG_SECTION_SEPARATOR = " | ";

    private readonly LoggerConfig _config;

    private FileStream _logStream = null!;
    private StreamWriter _writer = null!;

    /// <summary>
    /// Create a new Logger using a path and config.
    /// </summary>
    /// <param name="logPath">
    /// May be either a file or a directory.
    /// A filepath will create a log at the specified path.
    /// A path to a directory will automatically generate the log's name based on the logger's start time,
    ///   and then create a log file with that name in the provided directory.
    /// </param>
    /// <param name="config">The config the logger will use.</param>
    public Logger(string logPath, LoggerConfig config) {
        _config = config;

        if (Path.HasExtension(logPath)) {
            // Direct path to a file
            CreateLog(logPath);
        } else {
            // Path to directory, we need to generate a filename ourselves
            string name = $"{GetTimeStamp(_config.StartTime)}.log";
            string path = Path.Join(logPath, name);
            CreateLog(path);
        }
    }

    /// <summary>
    /// Create a new Logger using a path, id, and config.
    /// The name of the log file will be automatically generated from the logger's start time and id.
    /// </summary>
    /// <param name="logDirectory">The path to the directory the log should be created in.</param>
    /// <param name="id">The id of the log.</param>
    /// <param name="config">The config the logger will use.</param>
    public Logger(string logDirectory, string id, LoggerConfig config)
        : this(logDirectory, config) {
        string name = $"{GetTimeStamp(_config.StartTime)}_{id}.log";
        string path = Path.Join(logDirectory, name);
        CreateLog(path);
    }

    private void CreateLog(string path) {
        // File.Create throws insufficiently detailed exceptions to use a try-catch
        if (File.Exists(path)) throw new IOException($"Log file '{path}' already exists.");

        _logStream = File.Create(path);
        _writer = new StreamWriter(_logStream);
    }

    // Normally, we use local time to better match a user's expectation of what the latest log should be named.
    private string GetTimeStamp(DateTime time)
        => _config.UseUnixTime
            ? new DateTimeOffset(time)
                .ToUnixTimeMilliseconds()
                .ToString()
            : time.ToString("dd-MM-yyyyTHH:mm:ss.fff");

    private string GetTimeStampFromStart(DateTime time) {
        TimeSpan delta = time.Subtract(_config.StartTime);
        return _config.UseUnixTime
            ? ((int)delta.TotalMilliseconds).ToString()
            : $"{(int)delta.TotalHours:00}:{delta:mm}:{delta:ss}.{delta:fff}";
    }

    private static string BuildLogLine(params string[] sections)
        => string.Join(LOG_SECTION_SEPARATOR, sections);

    /// <summary>
    /// Log a message in the log file at the provided log level.
    /// </summary>
    /// <param name="level">The level to log the message at.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public void Log(LogLevel level, string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0) {
        string timestamp =
            _config.UseDeltaTime
                ? GetTimeStampFromStart(DateTime.Now)
                : GetTimeStamp(DateTime.Now);

        string logLine;

        if (_config.LogCaller) {
            logLine = BuildLogLine($"[{level}]", timestamp, $"{Path.GetFileName(caller)}:{callerLine}", message);
        } else {
            logLine = BuildLogLine($"[{level}]", timestamp, message);
        }

        _writer.WriteLine(logLine);
    }

    /* ========== Additional methods for ease of use ========== */
    // ReSharper disable ExplicitCallerInfoArgument

    /// <summary>
    /// Log a message in the log file at the Debug level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public void Debug(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Debug, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Verbose level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public void Verbose(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Verbose, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Info level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public void Info(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Info, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Warn level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public void Warn(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Warn, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Error level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public void Error(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Error, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Fatal level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public void Fatal(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Fatal, message, caller, callerLine);

    // ReSharper restore ExplicitCallerInfoArgument
}
