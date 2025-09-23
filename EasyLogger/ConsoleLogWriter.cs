namespace EasyLogger;

/// <summary>
/// Writer for logging to console.
/// </summary>
/// <example>
///   <code>
///     // Create a ConsoleLogWriter that writes to the standard output stream (default)
///     _ = new ConsoleLogWriter();
///     <br/>
///     // Create a ConsoleLogWriter that writes to the error output stream
///     _ = new ConsoleLogWriter(Console.Error);
///     <br/>
///     // Create a ConsoleLogWriter that will close the outputStream upon the writer being closed
///     _ = new ConsoleLogWriter(Console.Out, true);
///   </code>
/// </example>
/// <param name="outputStream">
/// TextWriter to use for logging.
/// <br/>Defaults to <c>Console.Out</c>.
/// </param>
/// <param name="closeStream">
/// Whether to close the <paramref name="outputStream"/> upon closing this writer.
/// <br/>Defaults to <c>false</c>.
/// </param>
[PublicAPI]
public class ConsoleLogWriter(TextWriter outputStream) : ILogWriter {
    public ConsoleLogWriter() : this(Console.Out) { }

    public void StartLog(Logger logger) { }

    public void Log(string message) {
        outputStream.WriteLine(message);
    }

    public void Flush() {
        outputStream.Flush();
    }
}
