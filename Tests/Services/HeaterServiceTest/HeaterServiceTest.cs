using Domain.Services;
using Moq;
using NUnit.Framework;
using Services.HeaterServices;
using System.Threading;
using System.Timers;

namespace Tests.Services.HeaterServiceTest
{
    [TestFixture]
    public class HeaterServiceTest
    {
        private Mock<ILoggerService> _mockLogger;
        private HeaterService _service;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILoggerService>();
            _service = new HeaterService(_mockLogger.Object);
        }

        [Test]
        public void TurnOn_ChangesStateAndLogs()
        {
            // Act
            _service.TurnOn();

            // Assert
            Assert.IsTrue(_service.IsOn());
            _mockLogger.Verify(l => l.LogHeaterStateChange(true), Times.Once);
            _mockLogger.Verify(l => l.LogHeaterCycle(true, It.IsAny<System.DateTime>()), Times.Once);
        }

        [Test]
        public void TurnOn_DoesNothingIfAlreadyOn()
        {
            // Arrange
            _service.TurnOn();
            _mockLogger.Reset();

            // Act
            _service.TurnOn();

            // Assert
            _mockLogger.Verify(l => l.LogHeaterStateChange(It.IsAny<bool>()), Times.Never);
            _mockLogger.Verify(l => l.LogHeaterCycle(It.IsAny<bool>(), It.IsAny<System.DateTime>()), Times.Never);
        }

        [Test]
        public void TurnOff_AfterTurnOn_UpdatesWorkingPeriod()
        {
            // Arrange
            _service.TurnOn();
            Thread.Sleep(100); // Malo vremena za rad

            // Act
            _service.TurnOff();

            // Assert
            Assert.IsFalse(_service.IsOn());
            Assert.Greater(_service.GetTotalWorkingHours(), 0);
            Assert.Greater(_service.GetResourceConsumption(), 0);
            _mockLogger.Verify(l => l.LogHeaterStateChange(false), Times.Once);
            _mockLogger.Verify(l => l.LogHeaterCycle(false, It.IsAny<System.DateTime>()), Times.Once);
            _mockLogger.Verify(l => l.LogEvent(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void TurnOff_DoesNothingIfAlreadyOff()
        {
            // Arrange - peć je već isključena
            _mockLogger.Reset();

            // Act
            _service.TurnOff();

            // Assert
            _mockLogger.Verify(l => l.LogHeaterStateChange(It.IsAny<bool>()), Times.Never);
            _mockLogger.Verify(l => l.LogHeaterCycle(It.IsAny<bool>(), It.IsAny<System.DateTime>()), Times.Never);
        }

        [Test]
        public void GetResourceConsumption_CalculatesCorrectly()
        {
            // Arrange
            _service.TurnOn();
            Thread.Sleep(1000); // 1 sekunda rada
            _service.TurnOff();

            // Act
            var consumption = _service.GetResourceConsumption();

            // Assert
            Assert.Greater(consumption, 0);
            Assert.AreEqual(0.01, consumption, 0.005); // 0.01 kWh po sekundi
        }
    }
}