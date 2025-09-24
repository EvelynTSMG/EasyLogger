### What is EasyLogger?
EasyLogger is a C# logging library intended to be simple and easy to use.\
It is NOT intended to:
- have every feature under the sun,
- maintain backwards compatibility across many versions of C#,
- be the end all be all of C# logger libraries.

### How to use EasyLogger?
Loggers need a `LoggerConfig`, as well as any amount of `ILogWriter`s.\
For documentation on the configuration, see <a href="https://github.com/EvelynTSMG/EasyLogger/blob/main/EasyLogger/LoggerConfig.cs">EasyLogger/LoggerConfig.cs</a>\
\
For example, you can create a new Logger to write to the console with the default configuration like so:
```csharp
Logger logger = new(new LoggerConfig(), new ConsoleLogWriter());
```
If you want to create multiple log files with the same configuration, a `FileLoggerFactory` is provided.\
The factory will automatically add a `FileLogWriter` to created loggers.\
Additional writers may be passed in the factory constructor.
They will be directly used by created loggers, no copies will be made.\
All loggers created by the factory will log to the `logDirectory` provided to the factory.
```csharp
FileLoggerFactory loggerFactory = new("./logs/", new LoggerConfig(), new ConsoleLogWriter());
Logger appleLogger = loggerFactory.Create("apple");
Logger bananaLogger = loggerFactory.Create("banana");
Logger cherryLogger = loggerFactory.Create("cherry");
```
**Remember to flush or close loggers before the application exits.**
Otherwise, there is no guarantee the loggers will properly flush.
