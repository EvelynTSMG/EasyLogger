namespace EasyLogger.Tests;

public class FileLogWriterTests {
    [SetUp]
    public void LogDirSetUp() {
        Directory.CreateDirectory("./logs/");
    }

    [Test]
    public void LogTest() {
        const string LOG_PATH = "./logs/test.log";

        var logger =
            new Logger(
                new LoggerConfig {
                    LogTimeStamp = false,
                },
                new FileLogWriter(LOG_PATH)
            );
        logger.Info("This is a test message.");
        logger.Flush();

        Assert.That(File.Exists(LOG_PATH));

        using FileStream fs = File.OpenRead(LOG_PATH);
        using StreamReader sr = new(fs);

        string? line = sr.ReadLine();
        Assert.Multiple(
            ()
                => {
                Assert.That(line, Is.Not.EqualTo(null));
                Assert.That(line, Is.EqualTo("[INFO] | This is a test message."));
            }
        );
    }

    [TearDown]
    public void LogDirTearDown() {
        Directory.Delete("./logs/", true);
    }
}
