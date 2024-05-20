using L0gg3r.Builder;
using L0gg3r.LogSinks.Test;
using L0gg3r.Extensions.Logging;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExtensionsLoggerTests.LogLevelMappingTests;

internal sealed class InformationLogLevelDataAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return new object?[]
        {
            LogLevel.Information,
            L0gg3r.Base.LogLevel.Info
        };
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data) => "Information log level";
}

internal sealed class WarningLogLevelDataAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return new object?[]
        {
            LogLevel.Warning,
            L0gg3r.Base.LogLevel.Warning
        };
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data) => "Warning log level";
}

internal sealed class ErrorLogLevelDataAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return new object?[]
        {
            LogLevel.Error,
            L0gg3r.Base.LogLevel.Error
        };
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data) => "Error log level";
}

internal sealed class CiriticalLogLevelDataAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return new object?[]
        {
            LogLevel.Critical,
            L0gg3r.Base.LogLevel.Error
        };
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data) => "Critical log level";
}

[TestClass]
public class TheExtensionsLogger
{
    [DataTestMethod]
    [InformationLogLevelData]
    [WarningLogLevelData]
    [ErrorLogLevelData]
    [CiriticalLogLevelData]
    public void ShouldMapTheLogLevelsCorrectly(LogLevel extensionsLogLevel, L0gg3r.Base.LogLevel expectedLogLevel)
    {
        // Arrange
        TestLogSink testLogSink = new();
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddL0gg3r(builder => builder.LogTo.LogSink(testLogSink)));
        ILogger extensionLogger = factory.CreateLogger("Program");

        // Act
        extensionLogger.Log(extensionsLogLevel, "Hello World!");
        factory.Dispose();

        // Assert
        testLogSink.LogMessages.Should().ContainSingle();
        testLogSink.LogMessages.First().LogLevel.Should().Be(expectedLogLevel);

        var x = testLogSink.LogMessages.First().Senders;
    }
}
