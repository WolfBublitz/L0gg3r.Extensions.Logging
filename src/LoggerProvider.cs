// ----------------------------------------------------------------------------
// <copyright file="LoggerProvider.cs" company="L0gg3r">
// Copyright (c) L0gg3r Project
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

namespace L0gg3r.Extensions.Logging;

/// <summary>
/// A logger provider that provides <see cref="LoggerAdapter"/> instances.
/// </summary>
public sealed class LoggerProvider : ILoggerProvider
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘
    private readonly Base.ILogger logger;

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Constructors                                                            │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerProvider"/> class.
    /// </summary>
    public LoggerProvider()
    {
        logger = new Logger();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerProvider"/> class.
    /// </summary>
    /// <param name="logger">The initial <see cref="Logger"/>.</param>
    public LoggerProvider(Base.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        this.logger = logger;
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName)
    {
        if (categoryName == "root")
        {
            return new LoggerAdapter(logger);
        }
        else
        {
            return new LoggerAdapter(logger.GetChildLogger(categoryName));
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        logger.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
