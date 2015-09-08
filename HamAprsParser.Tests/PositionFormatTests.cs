using System.Text;
using HamAprsParser.Extensions;
using HamAprsParser.Parsers;
using HamAprsParser.Parsers.Formats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HamAprsParser.Tests
{
    [TestClass]
    public class PositionFormatTests
    {
        [TestMethod]
        [TestCategory("MiCe")]
        public void MiCePositionTest()
        {
            const string destinationCall = "T4SQZZ";
            const string payload = "`(_fn\"Oj/";
            var actual = MicEPosition.ParsePosition(payload, new Callsign(destinationCall));
            Assert.AreEqual(3, actual.PositionAmbiguity, "Position ambiguity");
            Assert.AreEqual('j', actual.SymbolCode, "Symbol code");
            Assert.AreEqual('/', actual.SymbolTable, "Symbol table");
            Assert.AreEqual(44.52583, actual.Latitude, 1e-3, "Latitude");
            Assert.AreEqual(-112.08333, actual.Longitude, 1e-3, "Longitude");
        }

        [TestMethod]
        [TestCategory("MiCe")]
        public void MiCeCourseSpeedTest()
        {
            const string destinationCall = "T4SQZZ";
            const string payload = "`(_fn\"Oj/";
            var actual = MicEPosition.ParseCourseAndSpeed(Encoding.UTF8.GetBytes(payload), destinationCall);
            Assert.AreEqual(251, actual.Course, "Course");
            Assert.AreEqual(20, actual.Speed, "Speed");
        }

        [TestMethod]
        [TestCategory("NMEA")]
        public void NmeaPositionTest()
        {
            const string payload = "$GPRMC,184649,A,3832.7107,S,05844.1957,W,0.000,0.0,130909,4.5,W*62";
            var actual = NmeaPosition.ParsePosition(payload);
            Assert.AreEqual(0, actual.PositionAmbiguity, "Position ambiguity");
            Assert.AreEqual('>', actual.SymbolCode, "Symbol code");
            Assert.AreEqual('/', actual.SymbolTable, "Symbol table");
            Assert.AreEqual(-38.54518, actual.Latitude, 1e-3, "Latitude");
            Assert.AreEqual(-58.73658, actual.Longitude, 1e-3, "Longitude");
        }

        [TestMethod]
        [TestCategory("APRS")]
        public void CompressedPositionTest()
        {

        }

        [TestMethod]
        [TestCategory("APRS")]
        public void UnCompressedPositionTest()
        {
            const string payload = "!5057.18N/00549.40E>037/004/A=000353";
            var actual = PositionParser.CreateFromString(payload);
            Assert.AreEqual(0, actual.PositionAmbiguity, "Position ambiguity");
            Assert.AreEqual('>', actual.SymbolCode, "Symbol code");
            Assert.AreEqual('/', actual.SymbolTable, "Symbol table");
            Assert.AreEqual(50.953, actual.Latitude, 1e-3, "Latitude");
            Assert.AreEqual(5.82333, actual.Longitude, 1e-3, "Longitude");
        }

        [TestMethod]
        [TestCategory("APRS")]
        public void UnCompressedExtensionTest()
        {
            const string payload = "!5057.18N/00549.40E>037/004/A=000353";
            //var actual =
            //    UnCompressed.ParseUnCompressedExtension(Encoding.UTF8.GetBytes(payload), 1) as
            //        CourseAndSpeedExtension;
            //Assert.IsNotNull(actual);
            //Assert.AreEqual(37, actual.Course, "Course");
            //Assert.AreEqual(4, actual.Speed, "Speed");
        }
   }
}