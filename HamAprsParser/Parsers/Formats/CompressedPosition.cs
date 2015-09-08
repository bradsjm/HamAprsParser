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
using System;
using System.Text.RegularExpressions;
using HamAprsParser.Exceptions;
using HamAprsParser.Extensions;

namespace HamAprsParser.Parsers.Formats
{
    internal static class CompressedPosition
    {
        /// <summary>
        /// Parses the position.
        /// </summary>
        /// <param name="payloadBody">The payload body.</param>
        /// <returns>PositionData.</returns>
        /// <exception cref="HamAprsParser.Exceptions.AprsParserException">
        /// Compressed position too short
        /// Compressed position invalid
        /// </exception>
        public static PositionData ParsePosition(string payloadBody)
        {
            // Extract Latitude bytes
            var lat1 = payloadBody[1] - 33;
            var lat2 = payloadBody[2] - 33;
            var lat3 = payloadBody[3] - 33;
            var lat4 = payloadBody[4] - 33;

            // Extract Longitude bytes
            var lng1 = payloadBody[5] - 33;
            var lng2 = payloadBody[6] - 33;
            var lng3 = payloadBody[7] - 33;
            var lng4 = payloadBody[8] - 33;

            // Fixups
            var lat = 90.0F - ((lat1 * (91 * 91 * 91) + lat2 * (91 * 91) + lat3 * 91 + lat4) / 380926.0F);
            var lng = -180.0F + ((lng1 * (91 * 91 * 91) + lng2 * (91 * 91) + lng3 * 91 + lng4) / 190463.0F);

            // Course/Speed, Altitude
            var c1 = payloadBody[10] - 33;
            var s1 = payloadBody[11] - 33;
            var comptype = payloadBody[12] - 33;
            int? altitude = null;

            if (c1 >= 0 && s1 >= 0 && (comptype & 0x18) == 0x10)
                altitude = (int)Math.Pow(1.002, c1 * 91 + s1);

            return new PositionData
            {
                Latitude = lat,
                Longitude = lng,
                PositionAmbiguity = 0,
                SymbolTable = payloadBody[0],
                SymbolCode = payloadBody[9],
                AltitudeFt = altitude,
                Source = PositionType.Compressed
            };
        }

        /// <summary>
        /// Parses the compressed extension.
        /// </summary>
        /// <param name="msgPayload">The MSG payload.</param>
        /// <param name="cursor">The cursor.</param>
        /// <returns></returns>
        public static IDataExtension ParseCompressedExtension(byte[] msgPayload, int cursor)
        {
            if (msgPayload[cursor + 9] == '_')
            {
                // this is a weather report packet, and thus has no extension
                return null;
            }

            var t = msgPayload[cursor + 12] - 33;
            var nmeaSource = (t & 0x18) >> 3;
            if (nmeaSource == 2)
            {
                // this message came from a GPGGA sentance, and therefore has altitude
                return null;
            }

            var c = msgPayload[cursor + 10] - 33;
            if (c + 33 == ' ')
            {
                // another special case, where csT is ignored
                return null;
            }

            if (c < 90)
            {
                // this is a compressed course/speed value
                var s = msgPayload[cursor + 11] - 33;
                return new CourseAndSpeedExtension
                {
                    Course = (c * 4),
                    Speed = ((int)Math.Round(Math.Pow(1.08, s) - 1))
                };
            }

            if (c == '{')
            {
                var s = msgPayload[cursor + 11] - 33;
                s = (int)Math.Round(2 * Math.Pow(1.08, s));
                return new RangeExtension(s);
            }

            return null;
        }
    }
}
