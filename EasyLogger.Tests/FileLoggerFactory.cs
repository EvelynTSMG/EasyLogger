namespace EasyLogger.Tests;

public class FileLoggerFactoryTests {
    // There is a test case missing here, specifically if the id is missing.
    // However, such a test would need to be spread across multiple files, making it unreasonable.

    [SetUp]
    public void LogDirSetUp() {
        Directory.CreateDirectory("./logs/");
    }

    [Test]
    public void NameTest() {
        string[] ids = [ "apple", "banana", "cherry" ];

        FileLoggerFactory factory =
            new(
                "./logs/",
                new LoggerConfig {
                    StartTime = new DateTime(0L, DateTimeKind.Utc), // Unix Epoch
                }
            );

        foreach (string id in ids) {
            _ = factory.Create(id);
        }

        foreach (string id in ids) {
            Assert.That(File.Exists($"./logs/01-01-0001T00:00:00.000_{id}.log"));
        }
    }

    [TearDown]
    public void LogDirTearDown() {
        Directory.Delete("./logs/", true);
    }
}
