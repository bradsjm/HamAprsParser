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
    internal static class MicEPosition
    {
        // Destination callsign is six characters.
        // A-K characters are not used in the last 3 characters and MNO are never used
        private static readonly Regex ValidDestRegex = new Regex("^[0-9A-LP-Z]{3}[0-9LP-Z]{3}$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Parses the specified MiCe payload bytes and destination call as position.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="destinationCall">The destination call.</param>
        /// <returns>PositionData.</returns>
        public static PositionData ParsePosition(string payload, Callsign destinationCall)
        {
            int posAmbiguity;
            float lat, lng;

            // Get latitude and position ambiguity followed by longitude
            GetLatitude(destinationCall.Base, out lat, out posAmbiguity);
            GetLongitude(destinationCall.Base, posAmbiguity, payload, out lng);

            // Return Position object
            return new PositionData
            {
                Latitude = lat,
                Longitude = lng,
                PositionAmbiguity = posAmbiguity,
                SymbolTable = payload[7],
                SymbolCode = payload[8],
                Source = PositionType.MicE
            };
        }

        /// <summary>
        /// Parses the MiCe course and speed extension.
        /// </summary>
        /// <param name="payloadBytes">The MSG payload.</param>
        /// <param name="destinationField">The destination field.</param>
        /// <returns></returns>
        public static CourseAndSpeedExtension ParseCourseAndSpeed(byte[] payloadBytes, string destinationField)
        {
            var sp = payloadBytes[4] - 28;
            var dc = payloadBytes[5] - 28;
            var se = payloadBytes[6] - 28;

            // Decoded according to Chap 10, p 52 of APRS Spec 1.0
            var speed = sp * 10;
            var q = dc / 10;
            speed += q;
            var r = dc % 10 * 100;
            var course = r + se;

            if (course >= 400)
                course -= 400;
            if (speed >= 800)
                speed -= 800;

            return new CourseAndSpeedExtension
            {
                Speed = speed,
                Course = course
            };
        }


        private static void GetLatitude(string destinationCall, out float latitude, out int posAmbiguity)
        {
            // Translate the characters to get the latitude
            var destcall2 = new char[6];
            for (var i = 0; i < 6; ++i)
            {
                var c = destinationCall[i];
                if ('A' <= c && c <= 'J')
                {
                    destcall2[i] = (char)(c - ('A' - '0'));
                }
                else if ('P' <= c && c <= 'Y')
                {
                    destcall2[i] = (char)(c - ('P' - '0'));
                }
                else if ('K' == c || 'L' == c || 'Z' == c)
                {
                    destcall2[i] = '_';
                }
                else
                    destcall2[i] = c;
            }

            // Determine latitude position ambiguity
            posAmbiguity = 0;
            if (destcall2[5] == '_')
            {
                destcall2[5] = '5';
                posAmbiguity = 4;
            }
            if (destcall2[4] == '_')
            {
                destcall2[4] = '5';
                posAmbiguity = 3;
            }
            if (destcall2[3] == '_')
            {
                destcall2[3] = '5';
                posAmbiguity = 2;
            }
            if (destcall2[2] == '_')
            {
                destcall2[2] = '3';
                posAmbiguity = 1;
            }
            if (destcall2[1] == '_' || destcall2[0] == '_')
                throw new AprsParserException(destinationCall, "MicE bad pos-ambiguity on destcall");

            latitude = int.Parse(new string(destcall2, 0,2));
            var latminutes = float.Parse(new string(destcall2, 2, 2) + "." + new string(destcall2, 4, 2));
            latitude += (latminutes / 60);

            // Check north/south direction, and correct the latitude sign if necessary
            if (destinationCall[3] <= 'L')
                latitude = 0.0F - latitude;
        }

        private static void GetLongitude(string destinationCall, int posAmbiguity, string payload, out float longitude)
        {
            var longDeg = payload[1] - 28;
            if (destinationCall[4] >= 'P')
                longDeg += 100;
            if (longDeg >= 180 && longDeg <= 189)
                longDeg -= 80;
            else if (longDeg >= 190 && longDeg <= 199)
                longDeg -= 190;
            var longMin = payload[2] - 28;
            if (longMin >= 60)
                longMin -= 60;
            var longMinFract = payload[3] - 28;

            switch (posAmbiguity)
            {
                // degree of positional ambiguity
                case 0:
                    longitude = (longDeg + longMin / 60.0F + (longMinFract / 6000.0F));
                    break;

                case 1:
                    longitude = (longDeg + longMin / 60.0F + ((longMinFract - longMinFract % 10 + 5) / 6000.0F));
                    break;

                case 2:
                    longitude = (longDeg + longMin / 60.0F);
                    break;

                case 3:
                    longitude = (longDeg + (longMin - longMin % 10 + 5) / 60.0F);
                    break;

                case 4:
                    longitude = (longDeg + 0.5F);
                    break;

                default:
                    throw new AprsParserException(payload, "Unable to extract longitude from MicE");
            }

            if (destinationCall[5] >= 'P')
            {
                // Longitude east/west sign
                longitude = 0.0F - longitude; // east positive, west negative
            }
        }
    }
}
