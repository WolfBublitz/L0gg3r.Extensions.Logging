using System.Linq;
using L0gg3r;
using L0gg3r.Builder;
using L0gg3r.Extensions.Logging;
using L0gg3r.LogSinks.Test;
using Microsoft.Extensions.Logging;

namespace LoggerFactoryTests.MethodTests.CreateMethodTests;

[TestClass]
public class TheCreateMethod
{
    [TestMethod]
    public void ShouldAddALoggerFactory()
    {
        // Arrange
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddL0gg3r());

        // Act
        ILogger extensionlogger = factory.CreateLogger("Program");

        // Assert
        extensionlogger.Should().NotBeNull();
    }

    [TestMethod]
    public void ShouldAddAnExistingLoggerInstance()
    {
        // Arrange
        Logger logger = new("Program");
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddL0gg3r(logger));

        // Act
        ILogger extensionlogger = factory.CreateLogger("Program");

        // Assert
        extensionlogger.Should().NotBeNull();
    }

    [TestMethod]
    public void ShouldAddAnLoggerInstanceFromABuilder()
    {
        // Arrange
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddL0gg3r(builder => { }));

        // Act
        ILogger extensionlogger = factory.CreateLogger("Program");

        // Assert
        extensionlogger.Should().NotBeNull();
    }
}
