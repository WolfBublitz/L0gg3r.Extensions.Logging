using L0gg3r.LogSinks.Test;
using L0gg3r.Extensions.Logging;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ExtensionsLoggerTests.LogLevelMappingTests;

internal sealed class TraceLogLevelDataAttribute : SimpleTestDataAttribute
{
    public override string DisplayName => $"Extensions log level: {LogLevel.Trace} should map to L0gg3r log level: {L0gg3r.Base.LogLevel.Debug}";

    public override object[] Data =>
    [
        LogLevel.Trace,
        L0gg3r.Base.LogLevel.Debug
    ];
}

internal sealed class DebugLogLevelDataAttribute : SimpleTestDataAttribute
{
    public override string DisplayName => $"Extensions log level: {LogLevel.Debug} should map to L0gg3r log level: {L0gg3r.Base.LogLevel.Debug}";

    public override object[] Data =>
    [
        LogLevel.Debug,
        L0gg3r.Base.LogLevel.Debug,
    ];
}

internal sealed class InformationLogLevelDataAttribute : SimpleTestDataAttribute
{
    public override string DisplayName => $"Extensions log level: {LogLevel.Information} should map to L0gg3r log level: {L0gg3r.Base.LogLevel.Info}";

    public override object[] Data =>
    [
        LogLevel.Information,
        L0gg3r.Base.LogLevel.Info
    ];
}

internal sealed class WarningLogLevelDataAttribute : SimpleTestDataAttribute
{
    public override string DisplayName => $"Extensions log level: {LogLevel.Warning} should map to L0gg3r log level: {L0gg3r.Base.LogLevel.Warning}";

    public override object[] Data =>
    [
        LogLevel.Warning,
        L0gg3r.Base.LogLevel.Warning
    ];
}

internal sealed class ErrorLogLevelDataAttribute : SimpleTestDataAttribute
{
    public override string DisplayName => $"Extensions log level: {LogLevel.Error} should map to L0gg3r log level: {L0gg3r.Base.LogLevel.Error}";

    public override object[] Data =>
    [
        LogLevel.Error,
        L0gg3r.Base.LogLevel.Error
    ];
}

internal sealed class CriticalLogLevelDataAttribute : SimpleTestDataAttribute
{
    public override string DisplayName => $"Extensions log level: {LogLevel.Critical} should map to L0gg3r log level: {L0gg3r.Base.LogLevel.Fatal}";

    public override object[] Data =>
    [
        LogLevel.Critical,
        L0gg3r.Base.LogLevel.Fatal
    ];
}

[TestClass]
public class TheExtensionsLogger
{
    [DataTestMethod]
    [TraceLogLevelData]
    [DebugLogLevelData]
    [InformationLogLevelData]
    [WarningLogLevelData]
    [ErrorLogLevelData]
    [CriticalLogLevelData]
    public void ShouldMapTheLogLevelsCorrectly(LogLevel extensionsLogLevel, L0gg3r.Base.LogLevel expectedLogLevel)
    {
        // Arrange
        TestLogSink testLogSink = new();
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.SetMinimumLevel(LogLevel.Trace).AddL0gg3r(builder => builder.WithMinimumLogLevel(L0gg3r.Base.LogLevel.Debug).LogTo.LogSink(testLogSink)));
        ILogger extensionLogger = factory.CreateLogger("Program");

        // Act
        extensionLogger.Log(extensionsLogLevel, "Hello World!");
        factory.Dispose();

        // Assert
        testLogSink.LogMessages.Should().ContainSingle();
        testLogSink.LogMessages.First().LogLevel.Should().Be(expectedLogLevel);
    }
}
