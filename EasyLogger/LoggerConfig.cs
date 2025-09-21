namespace EasyLogger;

/// <summary>
/// A configuration for <see cref="Logger">Loggers</see> to use. The config is logged as a preamble on log file creation.
/// </summary>
[PublicAPI]
public struct LoggerConfig {
    /// <summary>
    /// The time at the start of logging.
    /// It is encouraged to use a <see cref="FileLoggerFactory"/> to create many <see cref="Logger">Loggers</see> with the same start time.
    /// <br/>Defaults to <c>DateTime.Now</c>.
    /// </summary>
    public DateTime StartTime = DateTime.Now;

    /// <summary>
    /// Whether the logger should use <see href="https://wikipedia.org/wiki/Unix_time">Unix Time</see>
    /// instead of an <see href="https://wikipedia.org/wiki/ISO_8601">ISO 8601</see> compliant date and time format.
    /// <br/>Defaults to <c>false</c>.
    /// </summary>
    public bool UseUnixTime = false;

    /// <summary>
    /// Whether the logger should use the difference between its creation and log time when logging,
    /// such that logging 12 seconds and 54 milliseconds after start time will log <c>00:00:12.054</c>.
    /// Does not log days. Logging past 24 hours will instead log the total amount of hours that have passed,
    /// such that logging 4 days after start time will log <c>96:00:00.000</c>.
    /// <br/>Defaults to <c>false</c>.
    /// </summary>
    public bool UseDeltaTime = false;

    /// <summary>
    /// Whether to also log the caller when logging.
    /// For example, when logging from line 36 of <c>LoggerConfig.cs</c>, the logger would log <c>LoggerConfig.cs:36</c>
    /// <br/>Defaults to <c>false</c>.
    /// </summary>
    public bool LogCaller = false;

    /// <summary>
    /// Whether to also log the current time when logging.
    /// <br/>Defaults to <c>true</c>.
    /// </summary>
    public bool LogTimeStamp = true;

    /// <summary>
    /// Whether to also log the level of the log.
    /// <br/>Defaults to <c>true</c>.
    /// </summary>
    public bool LogLevel = true;

    public LoggerConfig() { }
}
