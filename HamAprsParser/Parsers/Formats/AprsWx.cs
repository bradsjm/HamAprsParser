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

namespace HamAprsParser.Parsers.Formats
{
    internal static class AprsWx
    {

        #region Public Methods

        /// <summary>
        /// Gets the comment from the weather payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns></returns>
        public static string GetComment(string payload) => Clean(payload).Trim();

        public static bool ParseHumidity(
                    string data,
            out int? humidity)
        {
            humidity = null;

            var match = Regex.Match(data, @"h(?<humidity>\d{1,3})", RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                humidity = int.Parse(match.Groups["humidity"].Value);
                if (humidity == 0) humidity = 100;
                if (humidity < 0 || humidity > 100) humidity = null;
                return true;
            }

            return false;
        }

        public static bool ParseLuminosity(
            string data,
            out int? luminosity)
        {
            luminosity = null;

            var match = Regex.Match(data, @"[lL](?<luminosity>\d{1,3})", RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                luminosity = int.Parse(match.Groups["luminosity"].Value);
                if (match.Value[0] == 'L') luminosity += 1000;
                return true;
            }

            return false;
        }

        public static bool ParsePressure(
            string data,
            out int? pressure)
        {
            pressure = null;

            var match = Regex.Match(data, @"b(?<pressure>\d{4,5})", RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                pressure = int.Parse(match.Groups["pressure"].Value);
                return true;
            }

            return false;
        }

        public static bool ParseRain(
            string data,
            out float? rainInch1H,
            out float? rainInch24H,
            out float? rainInchMidnight)
        {
            rainInch1H = rainInch24H = rainInchMidnight = null;

            var match = Regex.Match(data, @"r(?<rain>\d{1,3})", RegexOptions.ExplicitCapture);
            if (match.Success)
                rainInch1H = int.Parse(match.Groups["rain"].Value);

            match = Regex.Match(data, @"p(?<rain>\d{1,3})", RegexOptions.ExplicitCapture);
            if (match.Success)
                rainInch24H = int.Parse(match.Groups["rain"].Value);

            match = Regex.Match(data, @"P(?<rain>\d{1,3})", RegexOptions.ExplicitCapture);
            if (match.Success)
                rainInchMidnight = int.Parse(match.Groups["rain"].Value);

            return rainInch1H.HasValue || rainInch24H.HasValue || rainInchMidnight.HasValue;
        }

        public static bool ParseSnowfall(
            string data,
            out float? snowInches)
        {
            snowInches = null;

            var match = Regex.Match(data, @"[gt]\d{1,3}.*s(?<snowfall>\d{1,3})", RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                snowInches = int.Parse(match.Groups["snowfall"].Value);
                return true;
            }

            return false;
        }

        public static bool ParseWindAndTemperature(
            string data,
            out float? windSpeedMph,
            out int? windDirection,
            out float? temperatureF,
            out float? windGustMph)
        {
            windSpeedMph = temperatureF = windGustMph = null;
            windDirection = null;

            var match = Regex.Match(data, @"(?<course>[\d \.\-]{3})\/(?<speed>[\d \.]{3})g(?<gust>[\d \.]+)t(?<temp>-?[\d \.]+)", RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                windDirection = Conversions.ParseInt(match.Groups["course"].Value);
                windSpeedMph = Conversions.ParseInt(match.Groups["speed"].Value);
                windGustMph = Conversions.ParseInt(match.Groups["gust"].Value);
                temperatureF = Conversions.ParseInt(match.Groups["temp"].Value);
                if (windDirection > 360) windDirection = null;
                if (temperatureF > 150 || temperatureF < -150) temperatureF = null;
                if (windSpeedMph > 255) windSpeedMph = null;
                if (windGustMph > 255) windGustMph = null;
                return true;
            }

            match = Regex.Match(data, @"c(?<course>[\d \.\-]{3})s(?<speed>[\d \.]{3})g(?<gust>[\d \.]+)t(?<temp>-?[\d \.]+)", RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                windDirection = Conversions.ParseInt(match.Groups["course"].Value);
                windSpeedMph = Conversions.ParseInt(match.Groups["speed"].Value);
                windGustMph = Conversions.ParseInt(match.Groups["gust"].Value);
                temperatureF = Conversions.ParseInt(match.Groups["temp"].Value);
                if (windDirection > 360) windDirection = null;
                if (temperatureF > 150 || temperatureF < -150) temperatureF = null;
                if (windSpeedMph > 255) windSpeedMph = null;
                if (windGustMph > 255) windGustMph = null;
                return true;
            }

            match = Regex.Match(data, @"(?<course>[\d \.\-]{3})\/(?<speed>[\d \.]{3})t(?<temp>-?[\d \.]+)", RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                windDirection = Conversions.ParseInt(match.Groups["course"].Value);
                windSpeedMph = Conversions.ParseInt(match.Groups["speed"].Value);
                temperatureF = Conversions.ParseInt(match.Groups["temp"].Value);
                if (windDirection > 360) windDirection = null;
                if (temperatureF > 150 || temperatureF < -150) temperatureF = null;
                if (windSpeedMph > 255) windSpeedMph = null;
                return true;
            }

            match = Regex.Match(data, @"(?<course>[\d \.\-]{3})\/(?<speed>[\d \.]{3})g(?<gust>[\d \.]+)", RegexOptions.ExplicitCapture);
            if (match.Success)
            {
                windDirection = Conversions.ParseInt(match.Groups["course"].Value);
                windSpeedMph = Conversions.ParseInt(match.Groups["speed"].Value);
                windGustMph = Conversions.ParseInt(match.Groups["gust"].Value);
                if (windDirection > 360) windDirection = null;
                if (windSpeedMph > 255) windSpeedMph = null;
                return true;
            }

            return false;
        }

        #endregion Public Methods

        #region Private Methods

        private static string Clean(string data) => Regex.Replace(data, @".*[gtrPplLs#hbVv][0-9\+\-\._ ]{2,5}", string.Empty);

        #endregion Private Methods
    }
}
