using NUnit.Framework;
using Moq;
using Domain.Models;
using Domain.Services;
using Domain.Repositories.DevicesRepositories;
using Services.DeviceServices;
using System.Collections.Generic;

namespace Tests.Services.DevicesServiceTest
{
    [TestFixture]
    public class DevicesServiceTest
    {
        private Mock<IDeviceRepository> _mockRepository;
        private Mock<ILoggerService> _mockLogger;
        private DevicesService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IDeviceRepository>();
            _mockLogger = new Mock<ILoggerService>();
            _service = new DevicesService(_mockRepository.Object, _mockLogger.Object);
        }

        [Test]
        public void RegisterDevice_CallsRepositoryAdd()
        {
            // Arrange
            var device = new Device("TEST001");

            // Act
            _service.RegisterDevice(device);

            // Assert
            _mockRepository.Verify(r => r.Add(device), Times.Once);
            _mockLogger.Verify(l => l.LogEvent(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetAllDevices_ReturnsDevicesFromRepository()
        {
            // Arrange
            var devices = new List<Device>
            {
                new Device("TEST001"),
                new Device("TEST002")
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(devices);

            // Act
            var result = _service.GetAllDevices();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("TEST001", result[0].Id);
            Assert.AreEqual("TEST002", result[1].Id);
        }

        [Test]
        public void UpdateDeviceTemperature_UpdatesDeviceAndLogs()
        {
            // Arrange
            var device = new Device("TEST001") { Temperatura = 20.0 };
            _mockRepository.Setup(r => r.GetById("TEST001")).Returns(device);

            // Act
            _service.UpdateDeviceTemperature("TEST001", 22.5);

            // Assert
            Assert.AreEqual(22.5, device.Temperatura);
            _mockRepository.Verify(r => r.Update(device), Times.Once);
            _mockLogger.Verify(l => l.LogTemperatureReading("TEST001", 22.5), Times.Once);
        }

        [Test]
        public void UpdateDeviceTemperature_DoesNothingForNonExistentDevice()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetById("INVALID")).Returns((Device)null);

            // Act
            _service.UpdateDeviceTemperature("INVALID", 22.5);

            // Assert
            _mockRepository.Verify(r => r.Update(It.IsAny<Device>()), Times.Never);
            _mockLogger.Verify(l => l.LogTemperatureReading(It.IsAny<string>(), It.IsAny<double>()), Times.Never);
        }

        [Test]
        public void NotifyHeaterStateChange_UpdatesAllDevices()
        {
            // Arrange
            var devices = new List<Device>
            {
                new Device("TEST001"),
                new Device("TEST002"),
                new Device("TEST003")
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(devices);

            // Act
            _service.NotifyHeaterStateChange(true);

            // Assert
            foreach (var device in devices)
            {
                Assert.IsTrue(device.JePecUkljucena);
                _mockRepository.Verify(r => r.Update(device), Times.Once);
            }
            _mockLogger.Verify(l => l.LogEvent(It.IsAny<string>()), Times.Once);
        }
    }
}