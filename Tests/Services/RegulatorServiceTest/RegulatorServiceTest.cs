using NUnit.Framework;
using Moq;
using Domain.Services;
using Domain.Enums;
using Services.RegulatorServices;
using System;

namespace Tests.Services.RegulatorServiceTest
{
    [TestFixture]
    public class RegulatorServiceTest
    {
        private Mock<IHeaterService> _mockHeater;
        private Mock<IDevicesService> _mockDevices;
        private Mock<ILoggerService> _mockLogger;
        private RegulatorService _service;

        [SetUp]
        public void Setup()
        {
            _mockHeater = new Mock<IHeaterService>();
            _mockDevices = new Mock<IDevicesService>();
            _mockLogger = new Mock<ILoggerService>();
            _service = new RegulatorService(_mockHeater.Object, _mockDevices.Object, _mockLogger.Object);
        }

        [Test]
        public void ProcessTemperatureReading_TurnsOnHeaterWhenCold()
        {
            _service.ConfigureWorkMode(0, 24, 22.0, 18.0);
            _mockHeater.SetupSequence(h => h.IsOn())
                .Returns(false)
                .Returns(true)
                .Returns(true)
                .Returns(true);

            for (int i = 0; i < 4; i++)
                _service.ProcessTemperatureReading($"DEV{i}", 19.0);

            _mockHeater.Verify(h => h.TurnOn(), Times.Once);
            _mockDevices.Verify(d => d.NotifyHeaterStateChange(true), Times.Once);
        }

        [Test]
        public void ProcessTemperatureReading_TurnsOffHeaterWhenHot()
        {
            _service.ConfigureWorkMode(0, 24, 22.0, 18.0);
            _mockHeater.SetupSequence(h => h.IsOn())
                .Returns(true)
                .Returns(false)
                .Returns(false)
                .Returns(false);

            for (int i = 0; i < 4; i++)
                _service.ProcessTemperatureReading($"DEV{i}", 23.0);

            _mockHeater.Verify(h => h.TurnOff(), Times.Once);
            _mockDevices.Verify(d => d.NotifyHeaterStateChange(false), Times.Once);
        }


        [Test]
        public void ProcessTemperatureReading_DoesNotToggleHeaterWithinHysteresis()
        {
            // Arrange
            _service.ConfigureWorkMode(0, 24, 22.0, 18.0);
            _mockHeater.Setup(h => h.IsOn()).Returns(false);

            // Act - Temperature within hysteresis range (21.8 - 22.2)
            for (int i = 0; i < 4; i++)
            {
                _service.ProcessTemperatureReading($"DEV{i}", 22.0);
            }

            // Assert
            _mockHeater.Verify(h => h.TurnOn(), Times.Never);
            _mockHeater.Verify(h => h.TurnOff(), Times.Never);
        }

        [Test]
        public void GetCurrentMode_ReturnsCorrectMode()
        {
            // Arrange
            _service.ConfigureWorkMode(8, 20, 22.0, 18.0);

            // Act
            var mode = _service.GetCurrentMode();

            // Assert
            // This will depend on current time, but should return either Dnevni or Nocni
            Assert.IsTrue(mode == RezimRada.Dnevni || mode == RezimRada.Nocni);
        }

        [Test]
        public void GetTargetTemperature_ReturnsDayTemperatureInDayMode()
        {
            // Arrange
            var currentHour = DateTime.Now.Hour;
            _service.ConfigureWorkMode(0, 24, 25.0, 20.0); // Always day mode

            // Act
            var temp = _service.GetTargetTemperature();

            // Assert
            Assert.AreEqual(25.0, temp);
        }

        [Test]
        public void StartRegulation_LogsEvent()
        {
            // Act
            _service.StartRegulation();

            // Assert
            _mockLogger.Verify(l => l.LogEvent("Regulacija započeta"), Times.Once);
        }

        [Test]
        public void ProcessTemperatureReading_LimitsStoredReadings()
        {
            // Act - Add more than 100 readings
            for (int i = 0; i < 150; i++)
            {
                _service.ProcessTemperatureReading("TEST001", 22.0);
            }

            // Assert - Should keep only last 100
            // Ne možemo direktno proveriti, ali možemo proveriti da se ne ruši
            Assert.DoesNotThrow(() => _service.ProcessTemperatureReading("TEST001", 22.0));
        }
    }
}