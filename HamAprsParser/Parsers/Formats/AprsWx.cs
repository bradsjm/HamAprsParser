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

        #region Private Fields

        private static readonly Regex CleanupRegex = new Regex(@".*[gtrPplLs#hbVv][0-9\+\-\._ ]{2,5}");
        private static readonly Regex HumidityRegex = new Regex(@"h(?<humidity>[0-9]{1,3})", RegexOptions.ExplicitCapture);
        private static readonly Regex LuminosityRegex = new Regex(@"h(?<luminosity>[0-9]{1,3})", RegexOptions.ExplicitCapture);
        private static readonly Regex PressureRegex = new Regex(@"b(?<pressure>\d{4,5})", RegexOptions.ExplicitCapture);
        private static readonly Regex Rain1HRegex = new Regex(@"r(?<rain>[0-9]{1,3})", RegexOptions.ExplicitCapture);
        private static readonly Regex Rain24HRegex = new Regex(@"p(?<rain>[0-9]{1,3})", RegexOptions.ExplicitCapture);
        private static readonly Regex RainTodayRegex = new Regex(@"P(?<rain>[0-9]{1,3})", RegexOptions.ExplicitCapture);
        private static readonly Regex SnowfallRegex = new Regex(@"[gt][0-9]{1,3}.*s(?<snowfall>[0-9]{1,3})", RegexOptions.ExplicitCapture);

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Gets the comment from the weather payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns></returns>
        public static string GetComment(string payload) => Clean(payload).Trim();

        /// <summary>
        /// Parses the humidity.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="humidity">The humidity.</param>
        /// <returns>System.Boolean.</returns>
        public static bool ParseHumidity(
                            string data,
                    out int? humidity)
        {
            humidity = null;

            var match = HumidityRegex.Match(data);
            if (match.Success)
            {
                humidity = int.Parse(match.Groups["humidity"].Value);
                if (humidity == 0) humidity = 100;
                if (humidity < 0 || humidity > 100) humidity = null;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parses the luminosity.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="luminosity">The luminosity.</param>
        /// <returns>System.Boolean.</returns>
        public static bool ParseLuminosity(
                    string data,
                    out int? luminosity)
        {
            luminosity = null;

            var match = LuminosityRegex.Match(data);
            if (match.Success)
            {
                luminosity = int.Parse(match.Groups["luminosity"].Value);
                if (match.Value[0] == 'L') luminosity += 1000;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parses the pressure.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="pressure">The pressure.</param>
        /// <returns>System.Boolean.</returns>
        public static bool ParsePressure(
                    string data,
                    out int? pressure)
        {
            pressure = null;

            var match = PressureRegex.Match(data);
            if (match.Success)
            {
                pressure = int.Parse(match.Groups["pressure"].Value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parses the rain.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="rainInch1H">The rain inch1 h.</param>
        /// <param name="rainInch24H">The rain inch24 h.</param>
        /// <param name="rainInchMidnight">The rain inch midnight.</param>
        /// <returns>System.Boolean.</returns>
        public static bool ParseRain(
                    string data,
                    out float? rainInch1H,
                    out float? rainInch24H,
                    out float? rainInchMidnight)
        {
            rainInch1H = rainInch24H = rainInchMidnight = null;

            var match = Rain1HRegex.Match(data);
            if (match.Success)
                rainInch1H = int.Parse(match.Groups["rain"].Value);

            match = Rain24HRegex.Match(data);
            if (match.Success)
                rainInch24H = int.Parse(match.Groups["rain"].Value);

            match = RainTodayRegex.Match(data);
            if (match.Success)
                rainInchMidnight = int.Parse(match.Groups["rain"].Value);

            return rainInch1H.HasValue || rainInch24H.HasValue || rainInchMidnight.HasValue;
        }

        /// <summary>
        /// Parses the snowfall.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="snowInches">The snow inches.</param>
        /// <returns>System.Boolean.</returns>
        public static bool ParseSnowfall(
                    string data,
                    out float? snowInches)
        {
            snowInches = null;

            var match = SnowfallRegex.Match(data);
            if (match.Success)
            {
                snowInches = int.Parse(match.Groups["snowfall"].Value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parses the wind and temperature.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="windSpeedMph">The wind speed MPH.</param>
        /// <param name="windDirection">The wind direction.</param>
        /// <param name="temperatureF">The temperature f.</param>
        /// <param name="windGustMph">The wind gust MPH.</param>
        /// <returns>System.Boolean.</returns>
        public static bool ParseWindAndTemperature(
                    string data,
                    out float? windSpeedMph,
                    out int? windDirection,
                    out float? temperatureF,
                    out float? windGustMph)
        {
            windSpeedMph = temperatureF = windGustMph = null;
            windDirection = null;

            var match = Regex.Match(data, @"(?<course>[0-9 \.\-]{3})\/(?<speed>[0-9 \.]{3})g(?<gust>[0-9 \.]+)t(?<temp>-?[0-9 \.]+)", RegexOptions.ExplicitCapture);
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

            match = Regex.Match(data, @"c(?<course>[0-9 \.\-]{3})s(?<speed>[0-9 \.]{3})g(?<gust>[0-9 \.]+)t(?<temp>-?[0-9 \.]+)", RegexOptions.ExplicitCapture);
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

            match = Regex.Match(data, @"(?<course>[0-9 \.\-]{3})\/(?<speed>[0-9 \.]{3})t(?<temp>-?[0-9 \.]+)", RegexOptions.ExplicitCapture);
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

            match = Regex.Match(data, @"(?<course>[0-9 \.\-]{3})\/(?<speed>[0-9 \.]{3})g(?<gust>[0-9 \.]+)", RegexOptions.ExplicitCapture);
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

        private static string Clean(string data) => CleanupRegex.Replace(data, string.Empty);

        #endregion Private Methods

    }
}
