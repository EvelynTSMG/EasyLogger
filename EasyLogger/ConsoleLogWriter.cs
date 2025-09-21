namespace EasyLogger;

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
