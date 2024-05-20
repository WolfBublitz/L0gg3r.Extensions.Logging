// ----------------------------------------------------------------------------
// <copyright file="ILoggingBuilderExtensions.cs" company="L0gg3r">
// Copyright (c) L0gg3r Project
// </copyright>
// ----------------------------------------------------------------------------

using System;

using L0gg3r.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace L0gg3r.Extensions.Logging;

/// <summary>
/// Provides extension methods for <see cref="ILoggingBuilder"/>.
/// </summary>
public static class ILoggingBuilderExtensions
{
    // ┌────────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                                 │
    // └────────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Adds a new <see cref="Logger"/> to <paramref name="this"/> <see cref="ILoggingBuilder"/>.
    /// </summary>
    /// <param name="this">The extended <see cref="ILoggingBuilder"/>.</param>
    /// <returns>This <see cref="ILoggingBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="this"/> is <see langword="null"/>.</exception>
    public static ILoggingBuilder AddL0gg3r(this ILoggingBuilder @this)
    {
        ArgumentNullException.ThrowIfNull(@this);

        ServiceDescriptor serviceDescriptor = ServiceDescriptor.Singleton<ILoggerProvider, LoggerProvider>();

        @this.Services.TryAddEnumerable(serviceDescriptor);

        return @this;
    }

    /// <summary>
    /// Adds the <paramref name="logger"/> to <paramref name="this"/> <see cref="ILoggingBuilder"/>.
    /// </summary>
    /// <param name="this">The extended <see cref="ILoggingBuilder"/>.</param>
    /// <param name="logger">The <see cref="Logger"/> to add.</param>
    /// <returns>This <see cref="ILoggingBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="this"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is <see langword="null"/>.</exception>
    public static ILoggingBuilder AddL0gg3r(this ILoggingBuilder @this, Logger logger)
    {
        ArgumentNullException.ThrowIfNull(@this);
        ArgumentNullException.ThrowIfNull(logger);

        ServiceDescriptor serviceDescriptor = ServiceDescriptor.Singleton<ILoggerProvider, LoggerProvider>(_ => new LoggerProvider(logger));

        @this.Services.TryAddEnumerable(serviceDescriptor);
        @this.Services.AddSingleton<Base.ILogger>(logger);

        return @this;
    }

    /// <summary>
    /// Adds the <see cref="Logger"/> that is constructed by <paramref name="builder"/> to <paramref name="this"/> <see cref="ILoggingBuilder"/>.
    /// </summary>
    /// <param name="this">The extended <see cref="ILoggingBuilder"/>.</param>
    /// <param name="builder">The <see cref="LoggerBuilder"/> used for constructing the <see cref="Logger"/>.</param>
    /// <returns>This <see cref="ILoggingBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="this"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    public static ILoggingBuilder AddL0gg3r(this ILoggingBuilder @this, Action<LoggerBuilder> builder)
    {
        ArgumentNullException.ThrowIfNull(@this);
        ArgumentNullException.ThrowIfNull(builder);

        LoggerBuilder loggerBuilder = new();

        builder(loggerBuilder);

        return @this.AddL0gg3r(loggerBuilder.Build());
    }
}
