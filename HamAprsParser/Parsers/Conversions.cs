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

namespace HamAprsParser.Parsers
{
    internal static class Conversions
    {
        #region Public Methods

        /// <summary>
        ///     Degresses to cardinal.
        /// </summary>
        /// <param name="degrees">The degrees.</param>
        /// <returns></returns>
        public static string DegressToCardinal(double degrees)
        {
            if (degrees >= 11.25 && degrees < 33.75)
                return "NNE";
            if (degrees >= 33.75 && degrees < 56.25)
                return "NE";
            if (degrees >= 56.25 && degrees < 78.25)
                return "ENE";
            if (degrees >= 78.25 && degrees < 101.25)
                return "E";
            if (degrees >= 101.25 && degrees < 123.75)
                return "ESE";
            if (degrees >= 123.75 && degrees < 146.25)
                return "SE";
            if (degrees >= 146.25 && degrees < 168.75)
                return "SSE";
            if (degrees >= 168.75 && degrees < 191.25)
                return "S";
            if (degrees >= 191.25 && degrees < 213.75)
                return "SSW";
            if (degrees >= 213.75 && degrees < 236.25)
                return "SW";
            if (degrees >= 236.25 && degrees < 258.75)
                return "WSW";
            if (degrees >= 258.75 && degrees < 281.25)
                return "W";
            if (degrees >= 281.25 && degrees < 303.75)
                return "WNW";
            if (degrees >= 303.75 && degrees < 326.25)
                return "NW";
            if (degrees >= 326.25 && degrees < 348.75)
                return "NNW";
            return "N";
        }

        /// <summary>
        ///     Knots to KMH.
        /// </summary>
        /// <param name="knots">The knots.</param>
        /// <returns></returns>
        public static int KnotsToKmh(int knots)
        {
            return (int)Math.Round(knots * 1.852);
        }

        /// <summary>
        ///     KTS to MPH.
        /// </summary>
        /// <param name="knots">The knots.</param>
        /// <returns></returns>
        public static int KtsToMph(int knots)
        {
            return (int)Math.Round(knots * 1.15077945);
        }

        /// <summary>
        ///     Meters to kilometers.
        /// </summary>
        /// <param name="meters">The meters.</param>
        /// <returns></returns>
        public static double MetersToKilometers(double meters)
        {
            return meters / 1000.0;
        }

        /// <summary>
        ///     Meters to miles.
        /// </summary>
        /// <param name="meters">The meters.</param>
        /// <returns></returns>
        public static double MetersToMiles(double meters)
        {
            return meters * 0.000621371192;
        }

        /// <summary>
        /// Parses the int.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>Nullable int.</returns>
        public static int? ParseInt(string s)
        {
            int i;
            return int.TryParse(s, out i) ? new int?(i) : null;
        }

        private static int ToInt(char[] charArray, int startIndex, int length)
        {
            var result = 0;
            var end = startIndex + length;
            for (var c = startIndex; c < end; c++)
                result = result * 10 + (charArray[c] - '0');

            return result;
        }


        #endregion Public Methods
    }
}