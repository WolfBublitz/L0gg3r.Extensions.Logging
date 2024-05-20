// ----------------------------------------------------------------------------
// <copyright file="LogLevelExtensions.cs" company="L0gg3r">
// Copyright (c) L0gg3r Project
// </copyright>
// ----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace L0gg3r.Extensions.Logging;

/// <summary>
/// Provides extension methods for <see cref="LogLevel"/>.
/// </summary>
internal static class LogLevelExtensions
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Internal Methods                                                               │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Converts <see cref="LogLevel"/> to <see cref="Base.LogLevel"/>.
    /// </summary>
    /// <param name="logLevel">The extended <see cref="LogLevel"/>.</param>
    /// <returns>The converted <see cref="Base.LogLevel"/>.</returns>
    internal static Base.LogLevel FromExtensionsLogLevel(this LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => Base.LogLevel.Debug,
            LogLevel.Debug => Base.LogLevel.Debug,
            LogLevel.Information => Base.LogLevel.Info,
            LogLevel.Warning => Base.LogLevel.Warning,
            LogLevel.Error => Base.LogLevel.Error,
            LogLevel.Critical => Base.LogLevel.Fatal,
            _ => Base.LogLevel.Info,
        };
    }
}
