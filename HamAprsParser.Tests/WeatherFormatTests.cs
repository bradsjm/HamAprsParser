using HamAprsParser.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HamAprsParser.Tests
{
    [TestClass]
    public class WeatherFormatTests
    {

        [TestMethod]
        public void PositionlessWeatherTest()
        {
            var payload = "c220s004g005t077r000p002P001h50b09900wRSW";
            var actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.AreEqual(220, actual.WindDirection, "WindDirection");
            Assert.AreEqual(4, actual.WindSpeedMph, "WindSpeedMph");
            Assert.AreEqual(5, actual.WindGustMph, "WindGustMph");
            Assert.AreEqual(77, actual.TemperatureF, "TemperatureF");
            Assert.AreEqual(0, actual.RainInch1H, "RainInch1H");
            Assert.AreEqual(2, actual.RainInch24H, "RainInch24H");
            Assert.AreEqual(1, actual.RainInchMidnight, "RainInchMidnight");
            Assert.AreEqual(50, actual.Humidity, "Humidity");
            Assert.AreEqual(9900, actual.PressureMb, "Pressure");
            //Assert.AreEqual("wRSW", actual.Comment, "Software");

            payload = "c220s004g005t077r000p002l001h50b09900wRSW";
            actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.IsNull(actual.RainInchMidnight, "RainInchMidnight");
            Assert.AreEqual(1, actual.Luminosity, "Luminosity");

            payload = "c220s004g005t077r000p002L001h50b09900wRSW";
            actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.IsNull(actual.RainInchMidnight, "RainInchMidnight");
            Assert.AreEqual(1001, actual.Luminosity, "Luminosity");

            payload = "c220s004g005t077r000p002l001h50b09900s005wRSW";
            actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.AreEqual(5, actual.SnowInches, "SnowInches");
        }

        [TestMethod]
        public void PositionalWeatherTest()
        {
            var payload = "!4903.50N/07201.75W_220/004g005t077r000p002P001h50b.....wRSW";
            var actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.AreEqual(220, actual.WindDirection, "WindDirection");
            Assert.AreEqual(4, actual.WindSpeedMph, "WindSpeedMph");
            Assert.AreEqual(5, actual.WindGustMph, "WindGustMph");
            Assert.AreEqual(77, actual.TemperatureF, "TemperatureF");
            Assert.AreEqual(0, actual.RainInch1H, "RainInch1H");
            Assert.AreEqual(2, actual.RainInch24H, "RainInch24H");
            Assert.AreEqual(1, actual.RainInchMidnight, "RainInchMidnight");
            Assert.AreEqual(50, actual.Humidity, "Humidity");
            Assert.IsNull(actual.PressureMb, "Pressure");
            //Assert.AreEqual("wRSW", actual.Comment, "Software");
        }

        [TestMethod]
        public void PositionalTimeStampWeatherTest()
        {
            var payload = "@092345z4903.50N/07201.75W_220/004g005t-07r000p002P001h50b09900wRSW";
            var actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.AreEqual(220, actual.WindDirection, "WindDirection");
            Assert.AreEqual(4, actual.WindSpeedMph, "WindSpeedMph");
            Assert.AreEqual(5, actual.WindGustMph, "WindGustMph");
            Assert.AreEqual(-7, actual.TemperatureF, "TemperatureF");
            Assert.AreEqual(0, actual.RainInch1H, "RainInch1H");
            Assert.AreEqual(2, actual.RainInch24H, "RainInch24H");
            Assert.AreEqual(1, actual.RainInchMidnight, "RainInchMidnight");
            Assert.AreEqual(50, actual.Humidity, "Humidity");
            Assert.AreEqual(9900, actual.PressureMb, "Pressure");
            //Assert.AreEqual("wRSW", actual.Comment, "Software");
        }

        [TestMethod]
        public void CompressedPositionWeatherTest()
        {
            var payload = "=/5L!!<*e7> _7P[g005t077r000p002P001h50b09900wRSW";
            var actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.AreEqual(220, actual.WindDirection, "WindDirection");
            Assert.AreEqual(4, actual.WindSpeedMph, "WindSpeedMph");
            Assert.AreEqual(5, actual.WindGustMph, "WindGustMph");
            Assert.AreEqual(-7, actual.TemperatureF, "TemperatureF");
            Assert.AreEqual(0, actual.RainInch1H, "RainInch1H");
            Assert.AreEqual(2, actual.RainInch24H, "RainInch24H");
            Assert.AreEqual(1, actual.RainInchMidnight, "RainInchMidnight");
            Assert.AreEqual(50, actual.Humidity, "Humidity");
            Assert.AreEqual(9900, actual.PressureMb, "Pressure");
            //Assert.AreEqual("wRSW", actual.Comment, "Software");
        }

        [TestMethod]
        public void CompressedTimestampPositionWeatherTest()
        {
            var payload = "@092345z/5L!!<*e7 _7P[g005t077r000p002P001h50b09900wRSW";
            var actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.AreEqual(220, actual.WindDirection, "WindDirection");
            Assert.AreEqual(4, actual.WindSpeedMph, "WindSpeedMph");
            Assert.AreEqual(5, actual.WindGustMph, "WindGustMph");
            Assert.AreEqual(-7, actual.TemperatureF, "TemperatureF");
            Assert.AreEqual(0, actual.RainInch1H, "RainInch1H");
            Assert.AreEqual(2, actual.RainInch24H, "RainInch24H");
            Assert.AreEqual(1, actual.RainInchMidnight, "RainInchMidnight");
            Assert.AreEqual(50, actual.Humidity, "Humidity");
            Assert.AreEqual(9900, actual.PressureMb, "Pressure");
            //Assert.AreEqual("wRSW", actual.Comment, "Software");
        }

        [TestMethod]
        public void ObjectPositionWeatherTest()
        {
            var payload = ";BRENDAVVV*4903.50N/07201.75W_220/004g005t077r000p002P001h50b09900wRSW";
            var actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.AreEqual(220, actual.WindDirection, "WindDirection");
            Assert.AreEqual(4, actual.WindSpeedMph, "WindSpeedMph");
            Assert.AreEqual(5, actual.WindGustMph, "WindGustMph");
            Assert.AreEqual(77, actual.TemperatureF, "TemperatureF");
            Assert.AreEqual(0, actual.RainInch1H, "RainInch1H");
            Assert.AreEqual(2, actual.RainInch24H, "RainInch24H");
            Assert.AreEqual(1, actual.RainInchMidnight, "RainInchMidnight");
            Assert.AreEqual(50, actual.Humidity, "Humidity");
            Assert.AreEqual(9900, actual.PressureMb, "Pressure");
            //Assert.AreEqual("wRSW", actual.Comment, "Software");

            payload = ";BRENDAVVV*092345z4903.50N/07201.75W_220/004g005b0990";
            actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
            Assert.AreEqual(220, actual.WindDirection, "WindDirection");
            Assert.AreEqual(4, actual.WindSpeedMph, "WindSpeedMph");
            Assert.AreEqual(5, actual.WindGustMph, "WindGustMph");
            Assert.IsNull(actual.TemperatureF, "TemperatureF");
            Assert.IsNull(actual.RainInch1H, "RainInch1H");
            Assert.IsNull(actual.RainInch24H, "RainInch24H");
            Assert.IsNull(actual.RainInchMidnight, "RainInchMidnight");
            Assert.IsNull(actual.Humidity, "Humidity");
            Assert.AreEqual(990, actual.PressureMb, "Pressure");
            //Assert.IsNull(actual.Comment, "Software");
        }

        [TestMethod]
        public void MiscTest()
        {
            var payload = "@031954z3941.30N/09001.23W_235/009g...t092r...p...P000h47b10104L....DsIP-VP";
            var actual = WeatherParser.CreateFromString(payload);
            Assert.IsNotNull(actual);
        }
    }
}