namespace EasyLogger;

/// <summary>
/// Interface for a log writer, which improves
/// </summary>
public interface ILogWriter {
    /// <summary>
    /// Start a log. May be called multiple times by different <see cref="Logger">Loggers</see>.
    /// It is up to the LogWriter to handle this correctly.
    /// </summary>
    /// <param name="logger">The logger that is writing to this writer.</param>
    public void StartLog(Logger logger);

    /// <summary>
    /// Log a message.
    /// </summary>
    /// <param name="message">The message to write.</param>
    public void Log(string message);

    /// <summary>
    /// Flush the LogWriter, if applicable.
    /// </summary>
    public void Flush() { }
}
