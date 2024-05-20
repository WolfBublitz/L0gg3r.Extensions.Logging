// ----------------------------------------------------------------------------
// <copyright file="LoggerAdapter.cs" company="L0gg3r">
// Copyright (c) L0gg3r Project
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace L0gg3r.Extensions.Logging;

/// <summary>
/// An adapter that maps the calls from <see cref="ILogger"/> to <see cref="Logger"/>.
/// </summary>
public sealed class LoggerAdapter : ILogger, IAsyncDisposable
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘
    private readonly Stack<Base.ILogger> loggerStack = new();

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Constructors                                                            │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerAdapter"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor will create a <see cref="LoggerAdapter"/> that uses the provided <see cref="Logger"/> instance.
    /// </remarks>
    /// <param name="logger">The <see cref="Logger"/> that shall be used.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is <see langword="null"/>.</exception>
    public LoggerAdapter(Base.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        loggerStack.Push(logger);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerAdapter"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor will create a <see cref="LoggerAdapter"/> that creates a new <see cref="Logger"/> instance with
    /// the provided <paramref name="categoryName"/>.
    /// </remarks>
    /// <param name="categoryName">The name of the logger.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Objekte verwerfen, bevor Bereich verloren geht", Justification = "Is disposed in DisposeAsync()")]
    public LoggerAdapter(string categoryName)
    {
        loggerStack.Push(new Logger(categoryName));
    }

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Private Properties                                                             │
    // └────────────────────────────────────────────────────────────────────────────────┘
    private Base.ILogger CurrentLogger => loggerStack.Peek();

    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        while (loggerStack.Count > 0)
        {
            await loggerStack.Pop().DisposeAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        string name = typeof(TState).Name;

        Base.ILogger childLogger = CurrentLogger.GetChildLogger(name);

        loggerStack.Push(childLogger);

        return new ActionDisposable(() =>
        {
            ValueTask task = loggerStack.Pop().DisposeAsync();

            if (task.IsCompleted)
            {
                return;
            }
            else
            {
                task.AsTask().GetAwaiter().GetResult();
            }
        });
    }

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (logLevel == LogLevel.None)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(formatter, nameof(formatter));

        object message = formatter(state, exception);

        CurrentLogger.Log(logLevel.FromExtensionsLogLevel(), message);
    }
}
