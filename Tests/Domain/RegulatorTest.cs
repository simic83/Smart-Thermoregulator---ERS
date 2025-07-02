using NUnit.Framework;
using Domain.Models;
using Domain.Enums;
using System;

namespace Tests.Domain
{
    [TestFixture]
    public class RegulatorTest
    {
        [Test]
        public void Regulator_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var regulator = new Regulator();

            // Assert
            Assert.AreEqual("MainRegulator001", regulator.Id);
            Assert.IsNotNull(regulator.PocitanjaTemperature);
            Assert.IsEmpty(regulator.PocitanjaTemperature);
        }

        [Test]
        public void Regulator_CanStoreTemperatureReadings()
        {
            // Arrange
            var regulator = new Regulator();
            var reading = new TemperaturnoPocitavanje
            {
                DeviceId = "TEST001",
                Temperatura = 22.5,
                Vreme = DateTime.Now
            };

            // Act
            regulator.PocitanjaTemperature.Add(reading);

            // Assert
            Assert.AreEqual(1, regulator.PocitanjaTemperature.Count);
            Assert.AreEqual("TEST001", regulator.PocitanjaTemperature[0].DeviceId);
            Assert.AreEqual(22.5, regulator.PocitanjaTemperature[0].Temperatura);
        }

        [Test]
        public void Regulator_CanSetWorkingParameters()
        {
            // Arrange
            var regulator = new Regulator();

            // Act
            regulator.PocetakDnevnogRezima = 7;
            regulator.KrajDnevnogRezima = 23;
            regulator.TemperaturaDnevna = 22.0;
            regulator.TemperaturaNocna = 18.0;
            regulator.TrenutniRezim = RezimRada.Dnevni;

            // Assert
            Assert.AreEqual(7, regulator.PocetakDnevnogRezima);
            Assert.AreEqual(23, regulator.KrajDnevnogRezima);
            Assert.AreEqual(22.0, regulator.TemperaturaDnevna);
            Assert.AreEqual(18.0, regulator.TemperaturaNocna);
            Assert.AreEqual(RezimRada.Dnevni, regulator.TrenutniRezim);
        }
    }
}