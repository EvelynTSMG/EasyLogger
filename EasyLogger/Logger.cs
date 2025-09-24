using System.Runtime.CompilerServices;

namespace EasyLogger;

/// <summary>
/// Logger that manages logging message to provided <see cref="ILogWriter">ILogWriters</see>.
/// When <see cref="IDisposable">Disposed</see>, flushes and closes all writers.
/// <seealso cref="LoggerConfig"/>
/// </summary>
[PublicAPI]
public class Logger : IDisposable {
    private const string LOG_SECTION_SEPARATOR = " | ";

    public readonly LoggerConfig Config;
    private readonly List<ILogWriter> _writers;

    /// <summary>
    /// Create a new Logger using a config.
    /// </summary>
    /// <param name="config">The config the logger will use.</param>
    /// <param name="writers">The writers the logger will call with message to log.</param>
    public Logger(LoggerConfig config, params ILogWriter[] writers) {
        Config = config;
        _writers = [ ..writers ];

        foreach (ILogWriter writer in _writers) {
            writer.StartLog(this);
        }
    }

    ~Logger() {
        foreach (ILogWriter writer in _writers) {
            writer.Flush();
            writer.Close();
        }
    }

    public void Dispose() {
        foreach (ILogWriter writer in _writers) {
            writer.Flush();
            writer.Close();
        }

        GC.SuppressFinalize(this);
    }

    internal string GetTimeStamp(DateTime time)
        => Config.UseUnixTime
            ? new DateTimeOffset(time)
                .ToUnixTimeMilliseconds()
                .ToString()
            : time.ToString("dd-MM-yyyyTHH:mm:ss.fff");

    private string GetTimeStampFromStart(DateTime time) {
        TimeSpan delta = time.Subtract(Config.StartTime);
        return Config.UseUnixTime
            ? ((int)delta.TotalMilliseconds).ToString()
            : $"{(int)delta.TotalHours:00}:{delta:mm}:{delta:ss}.{delta:fff}";
    }

    /// <summary>
    /// Flush all writers, if applicable.
    /// </summary>
    public void Flush() {
        foreach (ILogWriter writer in _writers) {
            writer.Flush();
        }
    }

    /// <summary>
    /// Log a message in the log file at the provided log level.
    /// </summary>
    /// <param name="level">The level to log the message at.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    /// <returns>Whether the logging succeeded. The logging has failed if <i>any</i> of the writers failed to log.</returns>
    public bool Log(LogLevel level, string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0) {
        List<string> sections = [ ];

        if (Config.LogLevel) {
            sections.Add($"[{level.ToString().ToUpperInvariant()}]");
        }

        if (Config.LogTimeStamp) {
            string timestamp =
                Config.UseDeltaTime
                    ? GetTimeStampFromStart(DateTime.Now)
                    : GetTimeStamp(DateTime.Now);

            sections.Add(timestamp);
        }

        if (Config.LogCaller) {
            sections.Add($"{Path.GetFileName(caller)}:{callerLine}");
        }

        sections.Add(message);

        string logLine = string.Join(LOG_SECTION_SEPARATOR, sections);

        bool successful = true;
        foreach (ILogWriter writer in _writers) {
            successful &= writer.Log(logLine);
        }

        return successful;
    }

    /* ========== Shorthand methods for ease of use ========== */
    // ReSharper disable ExplicitCallerInfoArgument

    /// <summary>
    /// Log a message in the log file at the Trace level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public bool Trace(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Trace, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Debug level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public bool Debug(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Debug, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Verbose level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public bool Verbose(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Verbose, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Info level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public bool Info(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Info, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Warn level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public bool Warn(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Warn, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Error level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public bool Error(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Error, message, caller, callerLine);

    /// <summary>
    /// Log a message in the log file at the Fatal level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <param name="callerLine">Optional. The line number of the call. Automatically supplied if not provided.</param>
    public bool Fatal(string message, [CallerFilePath] string caller = "", [CallerLineNumber] int callerLine = 0)
        => Log(LogLevel.Fatal, message, caller, callerLine);

    // ReSharper restore ExplicitCallerInfoArgument
}
