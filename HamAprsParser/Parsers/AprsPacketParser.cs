/* 
 * HAM APRS Parser - https://github.com/bradsjm/HamAprsParser
 * Copyright (c) 2015 Jonathan Bradshaw N9OXE, All rights reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library.
 */

using System.Text.RegularExpressions;
using HamAprsParser.Exceptions;
using HamAprsParser.Parsers.Formats;
using HamAprsParser.Payloads;

namespace HamAprsParser.Parsers
{
    /// <summary>
    /// Class AprsPacketParser provides the internal methods for parsing an APRS packet contents.
    /// The static AprsPacket.Create() factory method is used to invoke parsing.
    /// </summary>
    internal static class AprsPacketParser
    {

        #region Private Fields

        // Envelope parser. E.g. JH6YLM>APRS,RELAY,TRACE5-5,qAo,JA6JMJ-3:!3210.70N/13132.15E#15 KAWA
        private static readonly Regex EnvelopeRegex = new Regex(
            "^(?<source>[A-Za-z0-9-]{1,9})>(?<dest>[A-Za-z0-9-,*]+?):(?<information>.+)$",
            RegexOptions.ExplicitCapture);

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Creates from string.
        /// </summary>
        /// <param name="packet">The packet string.</param>
        /// <returns>AprsPacket</returns>
        /// <exception cref="AprsPacketException">Invalid packet format (regex failed)</exception>
        public static AprsPacket CreateFromString(string packet)
        {
            var result = EnvelopeRegex.Match(packet);
            if (!result.Success)
                throw new AprsPacketException(packet, "Unable to parse APRS packet envelope");

            var source = new Callsign(result.Groups["source"].Value);
            var digis = result.Groups["dest"].Value.Split(',');
            var information = result.Groups["information"].Value;
            var dest = new Callsign(digis[0]);
            var payload = ParsePacketPayload(dest, information);

            return new AprsPacket
            {
                SourceCall = source,
                DestinationCall = dest,
                Digipeaters = digis,
                Payload = payload,
                Original = packet
            };
        }

        #endregion Public Methods

        #region Private Methods

        private static Payload ParsePacketPayload(Callsign dest, string payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
                throw new AprsParserException(payload, "Packet payload is empty");

            switch (payload[0])
            {
                case '`':  // Current Mic-E Data (not used in TM-D700)
                case '\'': // Old Mic-E Data (but current data for TM-D700)
                case '!':
                case '=':
                case '/':
                case '@':
                case '$':
                    var timestamp = (payload[0] == '/' || payload[0] == '@') ? TimestampParser.CreateFromString(payload) : null;
                    var position = PositionParser.CreateFromString(payload, dest);
                    var weather = WeatherParser.CreateFromString(payload);
                    // TODO: This should support compressed too! Move into PositionParser
                    var extension = UnCompressedPosition.ParseUnCompressedExtension(payload);

                    if (weather != null)
                    {
                        return new WeatherPayload(payload)
                        {
                            Timestamp = timestamp,
                            Position = position,
                            Weather = weather,
                            Extension = extension,
                            Comment = null // TODO
                        };
                    }

                    return new PositionPayload(payload)
                    {
                        Timestamp = timestamp,
                        Position = position,
                        Extension = extension,
                        Comment = null // TODO: Get comment from position packet
                    };

                case ':': // Message
                    return MessagePayload.Create(payload, dest);

                case ';': // Object
                    return ObjectParser.CreateFromString(payload);

                case '>': // Status
                    return new UnsupportedPayload(payload) { Type = PayloadType.Status };

                case '<': // Station Capabilities
                    return new UnsupportedPayload(payload) { Type = PayloadType.StationCapabilities };

                case '?': // Query
                    return new UnsupportedPayload(payload) { Type = PayloadType.Query };

                case ')': // Item
                    return ItemParser.CreateFromString(payload);

                //case 'T': // Telemetry
                //    return new UnsupportedPayload(payload) { Type = PayloadType.Telemetry };

                case '#': // Peet Bros U-II Weather Station
                case '*':
                    return new UnsupportedPayload(payload)
                    {
                        Type = PayloadType.Wx,
                        Comment = "Peet Bros U-II Weather Station"
                    };

                case '_': // Weather report without position
                    return WeatherPayload.Create(payload);

                case '{': // User Defined
                    return new UnsupportedPayload(payload) { Type = PayloadType.UserDefined };

                case '}': // 3rd-party
                    return new UnsupportedPayload(payload) { Type = PayloadType.Thirdparty };
            }

            return new UnsupportedPayload(payload) { Type = PayloadType.Unspecified };
        }

        #endregion Private Methods

    }
}