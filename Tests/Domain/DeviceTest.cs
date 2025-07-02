using NUnit.Framework;
using Domain.Models;
using System;

namespace Tests.Domain
{
    [TestFixture]
    public class DeviceTest
    {
        [Test]
        public void Device_Constructor_SetsCorrectInitialValues()
        {
            // Arrange & Act
            var device = new Device("TEST001");

            // Assert
            Assert.AreEqual("TEST001", device.Id);
            Assert.AreEqual(20.0, device.Temperatura);
            Assert.IsFalse(device.JePecUkljucena);
            Assert.IsNotNull(device.PoslednjeOcitavanje);
        }

        [Test]
        public void Device_CanUpdateTemperature()
        {
            // Arrange
            var device = new Device("TEST001");

            // Act
            device.Temperatura = 25.5;

            // Assert
            Assert.AreEqual(25.5, device.Temperatura);
        }

        [Test]
        public void Device_CanUpdateHeaterState()
        {
            // Arrange
            var device = new Device("TEST001");

            // Act
            device.JePecUkljucena = true;

            // Assert
            Assert.IsTrue(device.JePecUkljucena);
        }
    }
}