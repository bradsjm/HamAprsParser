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
using HamAprsParser.Extensions;

namespace HamAprsParser.Parsers.Formats
{
    internal static class UnCompressedPosition
    {
        #region Public Methods

        /// <summary>
        /// Converts the parts of an uncompressed position packet into PostionData object.
        /// </summary>
        /// <param name="latdegstr">The latitude degree string.</param>
        /// <param name="latminstr">The latitude minutes string.</param>
        /// <param name="ns">The north/south indicatator.</param>
        /// <param name="longdegstr">The longitude degree string.</param>
        /// <param name="longminstr">The longitude minute string.</param>
        /// <param name="ew">The east/west indicator.</param>
        /// <returns>PositionData.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Latitude degree not an integer or out of range (>90).
        /// Latitude minute not a floating point number.
        /// Longitude degree not an integer or out of range (>180).
        /// Longitude minute  not a floating point number.
        /// </exception>
        public static PositionData Convert(
            string latdegstr, 
            string latminstr, 
            char ns, 
            string longdegstr, 
            string longminstr, 
            char ew)
        {
            int latdeg, longdeg;
            float latmin, longmin;

            var ambiguity = GetAmbiguity(latminstr);

            // Enforce ambiguity on both latitude and longitude
            switch (ambiguity)
            {
                case 1: // the last digit is not used (e.g. 4903.5_N)
                    latminstr = latminstr.Substring(0, 4);
                    longminstr = longminstr.Substring(0, 4);
                    break;

                case 2: // the minute decimals are not used (e.g. 4903.__N)
                    latminstr = latminstr.Substring(0, 2);
                    longminstr = longminstr.Substring(0, 2);
                    break;

                case 3: // the single minutes are not used (e.g. 490_.__N)
                    latminstr = latminstr.Substring(0, 1);
                    longminstr = longminstr.Substring(0, 1);
                    break;

                case 4: // disregard the minutes (49__.__N)
                    latminstr = "0";
                    longminstr = "0";
                    break;
            }

            if (!int.TryParse(latdegstr, out latdeg) || latdeg > 90)
                throw new ArgumentOutOfRangeException(nameof(latdegstr), "Latitude degree (" + latdegstr + ") not an integer or out of range (>90).");

            if (!float.TryParse(latminstr, out latmin) || latmin < 0)
                throw new ArgumentOutOfRangeException(nameof(latdegstr), "Latitude minute (" + latminstr + ") not a floating point number.");

            if (!int.TryParse(longdegstr, out longdeg) || longdeg > 180)
                throw new ArgumentOutOfRangeException(nameof(longminstr), "Longitude degree (" + longdegstr + ") not an integer or out of range (>180).");

            if (!float.TryParse(longminstr, out longmin) || longmin < 0)
                throw new ArgumentOutOfRangeException(nameof(longminstr), "Longitude minute (" + longminstr + ") not a floating point number.");

            var lat = latdeg + (latmin/60);
            var lng = longdeg + (longmin/60);

            if (ns == 's' || ns == 'S')
                lat = 0.0F - lat;

            if (ew == 'w' || ew == 'W')
                lng = 0.0F - lng;

            return new PositionData
            {
                Latitude = lat,
                Longitude = lng,
                PositionAmbiguity = ambiguity,
                Source = PositionType.UnCompressed
            };
        }

        /// <summary>
        /// Parses the compressed extension.
        /// </summary>
        /// <param name="payloadData">The payload data.</param>
        /// <returns>IDataExtension.</returns>
        public static IDataExtension ParseUnCompressedExtension(string payloadData)
        {
            var match = Patterns.PhgExtension.Match(payloadData);
            if (match.Success)
            {
                int? rate = null;
                if (match.Groups["rate"].Success)
                {
                    var ch = match.Groups["rate"].Value[0];
                    if (ch >= 'A' && ch <= 'Z') rate = ch - 'A' + 10;
                    if (ch >= '0' && ch <= '9') rate = ch - '0';
                }

                var power = (int)Math.Pow(match.Groups["power"].Value[0] - 48, 2);
                var height = (int)Math.Pow(2, match.Groups["height"].Value[0] - 48)*10;
                var gain = (int)Math.Pow(10, (match.Groups["height"].Value[0] - 48) / 10.0);
                var dir = match.Groups["height"].Value[0] - '0';

                return new PhgExtension
                {
                    Power = power,
                    Height = height,
                    Gain = gain,
                    Directivity = dir,
                    Rate = rate
                };
            }

            match = Patterns.CourseSpeedExtension.Match(payloadData);
            if (match.Success)
            {
                var course = Conversions.ParseInt(match.Groups["course"].Value);
                var speed = Conversions.ParseInt(match.Groups["speed"].Value);

                if (course.HasValue && course.Value > 360)
                    course = null;

                return new CourseAndSpeedExtension
                {
                    Course = course,
                    Speed = speed
                };
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets the ambiguity value (0-4).
        /// </summary>
        /// <param name="minstr">The minute string.</param>
        /// <returns>System.Int32.</returns>
        private static int GetAmbiguity(string minstr)
        {
            var match = Regex.Match(minstr.Replace(".", ""), @"^[0-9]{0,4}(?<ambiguity>[ ]{0,4})$");
            return (match.Success ? match.Groups["ambiguity"].Length : 0);
        }

        #endregion Private Methods

    }
}
