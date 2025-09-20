using System.Runtime.CompilerServices;

namespace EasyLogger;

/// <summary>
/// A factory to create many <see cref="Logger">Loggers</see> with the same <see cref="LoggerConfig"/> but different ids.
/// </summary>
/// <param name="logDirectory">The directory the logs should be created in.</param>
/// <param name="config">The config the loggers should use.</param>
[PublicAPI]
public class LoggerFactory(string logDirectory, LoggerConfig config) {
    /// <summary>
    /// Create a new <see cref="Logger"/> with the provided id.
    /// </summary>
    /// <param name="id">The id the logger should use for its name. Must be unique. Defaults to the caller file's name without the extension.</param>
    /// <param name="callerPath">Optional. The filepath to the caller. Automatically supplied if not provided.</param>
    /// <returns>A logger pre-configured with the factory's config and the provided id.</returns>
    public Logger Create(string? id, [CallerFilePath] string callerPath = "") {
        id ??= Path.GetFileNameWithoutExtension(callerPath);

        return new Logger(logDirectory, id, config);
    }

    // `logDirectory` is *intended* to replace the parameter passed in the primary constructor.
    // ReSharper disable once ParameterHidesPrimaryConstructorParameter
    /// <summary>
    /// Create a new <see cref="LoggerFactory"/> using the same <see cref="LoggerConfig"/> as was passed into this one,
    /// but with a different <paramref name="logDirectory"/>.
    /// </summary>
    /// <param name="logDirectory">The directory the logs should be created in.</param>
    /// <returns>A new <see cref="LoggerFactory"/> with a different <paramref name="logDirectory"/></returns>
    public LoggerFactory CloneWithDirectory(string logDirectory) => new(logDirectory, config);
}
