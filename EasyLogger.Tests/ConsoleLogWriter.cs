namespace EasyLogger.Tests;

public class ConsoleLogWriterTests {
    [TestCase("This is a test message.")]
    public void LogTest(string message) {
        TextWriter stdout = Console.Out;
        using StringWriter writer = new();
        Console.SetOut(writer);

        var logger =
            new Logger(
                new LoggerConfig {
                    LogTimeStamp = false,
                },
                new ConsoleLogWriter(writer)
            );

        logger.Info(message);

        string loggedMessage = writer.ToString();
        Assert.That(loggedMessage, Is.EqualTo($"[INFO] | This is a test message.\n"));

        writer.Close();
        Console.SetOut(stdout);
    }
}
