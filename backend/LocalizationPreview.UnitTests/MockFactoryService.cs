using System.Data;
using LocalizationPreview.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace LocalizationPreview.UnitTests; 

public class MockFactoryService {
    public MockFactoryService()
    {
        LoggerFactory = new Mock<ILoggerFactory>();
        LoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => new Mock<ILogger>().Object);
    }

    public Mock<ILoggerFactory> LoggerFactory { get; private set; }

    public Mock<IConnectionFactory> GetConnectionFactory()
    {
        var (factory, _, _) = GetConnectionMocks();
        return factory;
    }

    public static (Mock<IConnectionFactory>, Mock<IDbConnection>, Mock<IDbTransaction>) GetConnectionMocks()
    {
        var factory = new Mock<IConnectionFactory>();
        var connection = new Mock<IDbConnection>();
        var transaction = new Mock<IDbTransaction>();

        connection.Setup(c => c.BeginTransaction()).Returns(transaction.Object);

        factory.Setup(cf => cf.CreateAsync()).ReturnsAsync(connection.Object);
        factory.Setup(cf => cf.Create()).Returns(connection.Object);

        return (factory, connection, transaction);
    }
}