namespace EasyLogger;

/// <summary>
/// Levels supported by the <see cref="Logger"/>.
/// Generally used to indicate the type or severity of a logged message, but no convention is enforced.
/// Users are encouraged to create their own convention to suit their needs.
/// </summary>
public enum LogLevel {
    Trace,
    Debug,
    Verbose,
    Info,
    Warn,
    Error,
    Fatal,
}
