using System.Runtime.CompilerServices;

namespace EasyLogger;

/// <summary>
/// A factory to create many <see cref="Logger">Loggers</see> with the same <see cref="LoggerConfig"/>
/// but different <see cref="FileLogWriter">FileLogWriters</see> provided with ids.
/// </summary>
/// <param name="logDirectory">The directory the logs should be created in.</param>
/// <param name="config">The config the loggers should use.</param>
/// <param name="extraWriters">
/// Extra writers the factory should outfit its loggers with.
/// All created loggers will use the same extra writers.
/// </param>
[PublicAPI]
public class FileLoggerFactory(string logDirectory, LoggerConfig config, params ILogWriter[] extraWriters) {
    /// <summary>
    /// Create a new <see cref="Logger"/> with a <see cref="FileLogWriter"/> with the provided id.
    /// </summary>
    /// <param name="id">The id the log file should use for its name. Must be unique. Defaults to the caller file's name without the extension.</param>
    /// <param name="callerPath">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <returns>A logger pre-configured with the factory's config and a <see cref="FileLogWriter"/> with the provided id.</returns>
    public Logger Create(string? id, [CallerFilePath] string callerPath = "") {
        id ??= Path.GetFileNameWithoutExtension(callerPath);

        return new Logger(config, [ new FileLogWriter(logDirectory, id), ..extraWriters ]);
    }

    // `logDirectory` is *intended* to replace the parameter passed in the primary constructor.
    // ReSharper disable once ParameterHidesPrimaryConstructorParameter
    /// <summary>
    /// Create a new <see cref="FileLoggerFactory"/> using the same <see cref="LoggerConfig"/> as was passed into this one,
    /// but with a different <paramref name="logDirectory"/>.
    /// </summary>
    /// <param name="logDirectory">The directory the logs should be created in.</param>
    /// <returns>A new <see cref="FileLoggerFactory"/> with a different <paramref name="logDirectory"/></returns>
    public FileLoggerFactory CloneWithDirectory(string logDirectory) => new(logDirectory, config);
}
