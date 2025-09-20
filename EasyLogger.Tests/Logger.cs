namespace EasyLogger.Tests;

[TestFixture]
public class LoggerTests {
    // There are test cases missing here, specifically ones testing if timezones are accounted for
    // However, those are difficult(?) to test and have little to no impact on user experience
    // So I think it's fair to say we can forgo them for now

    [TestCase(621_355_968_000_000_000L, "01-01-1970T00:00:00.000")]   // Unix Epoch
    [TestCase(0, "01-01-0001T00:00:00.000")]                          // 0
    [TestCase(1_242_711_936_987_654_321L, "01-01-3939T00:01:38.765")] // Really far future
    public void NameTest_NoId(long time, string name) {
        _ =
            new Logger(
                "./logs/",
                new LoggerConfig {
                    StartTime = new DateTime(time, DateTimeKind.Utc),
                }
            );
        Assert.That(File.Exists($"./logs/{name}.log"));
    }

    [TestCase(621_355_968_000_000_000L, "testid", "01-01-1970T00:00:00.000_testid")]
    [TestCase(0, "ExtendedChars_ßðśə", "01-01-0001T00:00:00.000_ExtendedChars_ßðśə")]
    [TestCase(0, "UnicodeChars_😊🇫🇷", "01-01-0001T00:00:00.000_UnicodeChars_😊🇫🇷")]
    [TestCase(1_242_711_936_987_654_321L, "Id_With_Underscores", "01-01-3939T00:01:38.765_Id_With_Underscores")]
    public void NameTest_Id(long time, string id, string name) {
        _ =
            new Logger(
                "./logs/",
                id,
                new LoggerConfig {
                    StartTime = new DateTime(time, DateTimeKind.Utc),
                }
            );
        Assert.That(File.Exists($"./logs/{name}.log"));
    }

    [TestCase(621_355_968_000_000_000L, "0")]                // Unix Epoch
    [TestCase(0, "-62135596800000")]                         // Negative!
    [TestCase(1_242_711_936_987_654_321L, "62135596898765")] // Far future
    public void NameTest_UnixTime(long time, string name) {
        _ =
            new Logger(
                "./logs/",
                new LoggerConfig {
                    StartTime = new DateTime(time, DateTimeKind.Utc),
                    UseUnixTime = true,
                }
            );
        Assert.That(File.Exists($"./logs/{name}.log"));
    }

    [TearDown]
    public void LogFileTearDown() {
        foreach (string filepath in Directory.EnumerateFiles("./logs/", "*.log")) {
            Console.Write($"Removing log `{Path.GetFileName(filepath)}`");
            File.Delete(filepath);
        }
    }
}
