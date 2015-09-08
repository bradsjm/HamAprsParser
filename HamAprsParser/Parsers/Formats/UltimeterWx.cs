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

namespace HamAprsParser.Parsers.Formats
{
    internal static class UltimeterWx
    {
        private static readonly Regex DataLoggerMode = new Regex(
            "^!!(?<wind>[0-9A-F-]{4})([0F-]{2})(?<dir>[0-9A-F-]{2})(?<outtemp>[0-9A-F-]{4})(?<raintotal>[0-9A-F-]{4})(?<pressure>[0-9A-F-]{4})(?<intemp>[0-9A-F-]{4})(?<humidity>[0-9A-F-]{4})(?<inhumidity>[0-9A-F-]{4})(?<day>[0-9A-F-]{4})(?<time>[0-9A-F-]{4})(?<dayrain>[0-9A-F-]{4})?(?<windavg>[0-9A-F-]{4})?$",
            RegexOptions.ExplicitCapture);

        private static readonly Regex PacketMode = new Regex(
            @"^\$ULTW(?<wind>[0-9A-F-]{4})([0F-]{2})(?<dir>[0-9A-F-]{2})(?<outtemp>[0-9A-F-]{4})(?<raintotal>[0-9A-F-]{4})(?<pressure>[0-9A-F-]{4})([0-9A-F-]{4})([0-9A-F-]{4})([0-9A-F-]{4})(?<humidity>[0-9A-F-]{4})(?<day>[0-9A-F-]{4})(?<time>[0-9A-F-]{4})(?<dayrain>[0-9A-F-]{4})(?<windavg>[0-9A-F-]{4})$",
            RegexOptions.ExplicitCapture);

        /// <summary>
        /// Parses the datalogger packet from the Ultimeter 2000.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="windSpeedMph">The wind speed MPH.</param>
        /// <param name="windDirection">The wind direction.</param>
        /// <param name="temperatureF">The temperature f.</param>
        /// <param name="pressure">The pressure.</param>
        /// <param name="humidity">The humidity.</param>
        /// <param name="rainInchMidnight">The rain inch midnight.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Parse(
            string payload, 
            out float? windSpeedMph, 
            out int? windDirection,
            out float? temperatureF,
            out int? pressure,
            out int? humidity,
            out float? rainInchMidnight)
        {
            windSpeedMph = rainInchMidnight = temperatureF = pressure = null;
            humidity = windDirection = null;

            var result = (payload[0] == '$') ? PacketMode.Match(payload) : DataLoggerMode.Match(payload);
            if (!result.Success) return false;

            if (result.Groups["wind"].Value[0] != '-')
                windSpeedMph = (float) Math.Round(FromHex(result.Groups["wind"].Value)*0.1*0.621371F, 1); // mph

            if (result.Groups["dir"].Value[0] != '-')
                windDirection = (int) (FromHex(result.Groups["dir"].Value)*1.411764F); // degrees

            if (result.Groups["outtemp"].Value[0] != '-')
            {
                var temp = FromHex(result.Groups["outtemp"].Value);
                if (result.Groups["outtemp"].Value[0] > '7') // negative indicator two's compliment
                    temp = (temp & (temp - 0x7FFF)) - 0x8000;
                temperatureF = (float) Math.Round(temp*0.1F, 1); // degrees F
            }

            if (result.Groups["pressure"].Value[0] != '-')
                pressure = (int) (FromHex(result.Groups["pressure"].Value)*0.1F); // mbar

            if (result.Groups["humidity"].Value[0] != '-')
                humidity = (int) (FromHex(result.Groups["humidity"].Value)*0.1F);

            if (result.Groups["dayrain"].Success && result.Groups["dayrain"].Value[0] != '-')
                rainInchMidnight = (float) Math.Round(FromHex(result.Groups["dayrain"].Value)*0.01F, 2); // inch

            if (windDirection > 360) windDirection = null;
            if (humidity < 0 || humidity > 100) humidity = null;
            if (temperatureF > 150 || temperatureF < -150) temperatureF = null;
            if (windSpeedMph > 255) windSpeedMph = null;

            return true;
        }

        private static int FromHex(string hex) => Convert.ToInt32(hex, 16);
    }
}
