using NUnit.Framework;
using Domain.Models;
using System;
using System.Linq;

namespace Tests.Domain
{
    [TestFixture]
    public class HeaterTest
    {
        [Test]
        public void Heater_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var heater = new Heater();

            // Assert
            Assert.AreEqual("CentralnaHeater001", heater.Id);
            Assert.IsFalse(heater.JeUkljucen);
            Assert.IsNull(heater.VremePocetkaRada);
            Assert.IsNotNull(heater.RadniPeriodi);
            Assert.IsEmpty(heater.RadniPeriodi);
        }

        [Test]
        public void Heater_CanChangeState()
        {
            // Arrange
            var heater = new Heater();

            // Act
            heater.JeUkljucen = true;
            heater.VremePocetkaRada = DateTime.Now;

            // Assert
            Assert.IsTrue(heater.JeUkljucen);
            Assert.IsNotNull(heater.VremePocetkaRada);
        }

        [Test]
        public void Heater_CanAddWorkingPeriod()
        {
            // Arrange
            var heater = new Heater();
            var period = new RadniPeriod
            {
                VremePocetka = DateTime.Now.AddMinutes(-10),
                VremeKraja = DateTime.Now,
                TrajanjeSati = 10.0 / 60.0, // 10 minuta = 0.167 sati
                PotroseniResursi = 0.42
            };

            // Act
            heater.RadniPeriodi.Add(period);

            // Assert
            Assert.AreEqual(1, heater.RadniPeriodi.Count);
            Assert.AreEqual(period, heater.RadniPeriodi.First());
        }
    }
}