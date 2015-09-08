using System.Diagnostics;
using HamAprsParser.Payloads;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HamAprsParser.Tests
{
    [TestClass]
    public class TestPackets
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", 
            @"Data\position.csv", "position#csv", DataAccessMethod.Sequential)]
        public void PositionTests()
        {
            var data = TestContext.DataRow;
            var packet = data["Packet"].ToString();

            Trace.WriteLine(packet);
            var actual = AprsPacket.Create(packet);

            Assert.AreEqual(actual.Original, packet, "Original string value does not match input string");
            Assert.AreEqual(actual.ToString(), packet, "Packet ToString() does not match input string");
            Assert.AreEqual(actual.SourceCall.ToString(), data["From"].ToString(), "Source call does not match");
            Assert.AreEqual(actual.DestinationCall.ToString(), data["Dest"].ToString(), "Destination call does not match");
            Assert.IsNotNull(actual.Payload, "Payload is null");
            Assert.IsInstanceOfType(actual.Payload, typeof(PositionPayload), "Payload is not an APRS Position");

            var payload = (PositionPayload)actual.Payload;
            Assert.IsNotNull(payload.Position, "Payload position is null");
            Assert.AreEqual((float)payload.Position.Latitude, float.Parse(data["Latitude"].ToString()));
            Assert.AreEqual((float)payload.Position.Longitude, float.Parse(data["Longitude"].ToString()));
            Assert.AreEqual(payload.Comment, data["Comment"].ToString());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"Data\object.csv", "object#csv", DataAccessMethod.Sequential)]
        public void ObjectTests()
        {
            var data = TestContext.DataRow;
            var packet = data["Packet"].ToString();
            Trace.WriteLine(packet);
            var actual = AprsPacket.Create(packet);

            Assert.AreEqual(actual.Original, packet, "Original string value does not match input string");
            Assert.AreEqual(actual.ToString(), packet, "Packet ToString() does not match input string");
            Assert.AreEqual(actual.SourceCall.ToString(), data["From"].ToString(), "Source call does not match");
            Assert.AreEqual(actual.DestinationCall.ToString(), data["Dest"].ToString(), "Destination call does not match");
            Assert.IsNotNull(actual.Payload, "Payload is null");
            Assert.IsInstanceOfType(actual.Payload, typeof(ObjectPayload), "Payload is not an APRS Object");

            var payload = (ObjectPayload)actual.Payload;
            Assert.IsNotNull(payload.Position, "Payload position is null");
            Assert.AreEqual((float)payload.Position.Latitude, float.Parse(data["Latitude"].ToString()));
            Assert.AreEqual((float)payload.Position.Longitude, float.Parse(data["Longitude"].ToString()));
            Assert.AreEqual(payload.Comment, data["Comment"].ToString());
            Assert.AreEqual(payload.Name, data["ObjectName"].ToString());
        }

    }
}
