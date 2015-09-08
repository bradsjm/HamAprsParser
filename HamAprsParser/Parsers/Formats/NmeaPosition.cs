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
using System.Globalization;
using HamAprsParser.Exceptions;

namespace HamAprsParser.Parsers.Formats
{
    internal static class NmeaPosition
    {
        /// <summary>
        /// Parses the NMEA position.
        /// </summary>
        /// <param name="payloadBody">The payload body.</param>
        /// <returns>PositionData.</returns>
        /// <exception cref="HamAprsParser.Exceptions.AprsParserException">
        /// Too few parts in sentence (minimum 5)
        /// Not a valid NMEA position fix
        /// Not valid or not autonomous NMEA sentence
        /// Invalid NMEA sentence
        /// Abject failure parsing NMEA sentence
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Latitude too high
        /// Longitude too high
        /// Bad latitude sign
        /// Bad longitude sign
        /// </exception>
        public static PositionData ParsePosition(string payloadBody)
        {
            var nmea = payloadBody.Split(',');
            if (nmea.Length < 5)
                throw new AprsParserException(payloadBody, "Too few parts in sentence (minimum 5). Actual: " + nmea.Length);

            string latstr = null; // Latitude String
            string lngstr = null; // Longitude String
            string lathem = null; // Polarity of Latitude
            string lnghem = null; // Polarity of Longitude
            double lat, lng;

            // $GPGGA Global Positioning System Fix Data
            if (nmea[0] == "$GPGGA" && nmea.Length >= 15)
            {
                // $GPGGA,hhmmss.dd,xxmm.dddd,<N|S>,yyymm.dddd,<E|W>,v,ss,d.d,h.h,M,g.g,M,a.a,xxxx*hh
                if (nmea[6] != "1")
                    throw new AprsParserException(payloadBody, "Not a valid NMEA position fix");

                latstr = nmea[2];
                lathem = nmea[3];
                lngstr = nmea[4];
                lnghem = nmea[5];
            }

            // $GPGLL Geographic Position, Latitude/Longitude Data
            else if (nmea[0] == "$GPGLL" && nmea.Length > 7)
            {
                // $GPGLL,xxmm.dddd,<N|S>,yyymm.dddd,<E|W>,hhmmss.dd,S,M*hh
                // S = Status: 'A' = Valid, 'V' = Invalid
                if (nmea[6] != "A" || nmea[7][0] != 'A')
                    throw new AprsParserException(payloadBody, "Not valid or not autonomous NMEA sentence");

                latstr = nmea[1];
                lathem = nmea[2];
                lngstr = nmea[3];
                lnghem = nmea[4];
            }

            // $GPRMC Remommended Minimum Specific GPS/Transit Data
            else if (nmea[0] == "$GPRMC" && nmea.Length > 11)
            {
                // $GPRMC,hhmmss.dd,S,xxmm.dddd,<N|S>,yyymm.dddd,<E|W>,s.s,h.h,ddmmyy,d.d,<E|W>,M*hh
                // S = Status: 'A' = Valid, 'V' = Invalid
                if (nmea[2] != "A")
                    throw new AprsParserException(payloadBody, "Not valid or not autonomous NMEA sentence");

                latstr = nmea[3];
                lathem = nmea[4];
                lngstr = nmea[5];
                lnghem = nmea[6];
            }

            // $GPWPL Waypoint Load (not in APRS specs, but in NMEA specs)
            else if (nmea[0] == "$GPWPL" && nmea.Length > 5)
            {
                latstr = nmea[1];
                lathem = nmea[2];
                lngstr = nmea[3];
                lnghem = nmea[4];
            }

            // $PNTS Seen on APRS-IS, private sentense based on NMEA..
            else if (nmea.Length > 15 && nmea[0] == "$PNTS" && nmea[1] == "1")
            {
                latstr = nmea[7];
                lathem = nmea[8];
                lngstr = nmea[9];
                lnghem = nmea[10];
            }
            else if (nmea[0] == "$GPGSA" || nmea[0] == "$GPVTG" || nmea[0] == "$GPGSV")
            {
                // recognized but ignored
                return null;
            }

            // Parse lats,lath, lngs, lngh
            if (latstr == null || lngstr == null)
                throw new AprsParserException(payloadBody, "Invalid NMEA sentence");

            try
            {
                lat = StringToLatitude(latstr, lathem);
            }
            catch (Exception ex)
            {
                throw new AprsParserException(payloadBody, "Invalid NMEA position parsing latitude", ex);
            }

            try
            {
                lng = StringToLongitude(lngstr, lnghem);
            }
            catch (Exception ex)
            {
                throw new AprsParserException(payloadBody, "Invalid NMEA position parsing longitude", ex);
            }

            if (lat > 90.0F)
                throw new AprsParserException(payloadBody, "Invalid NMEA position. Latitude too high");

            if (lng > 180.0F)
                throw new AprsParserException(payloadBody, "Invalid NMEA position. Longitude too high");

            return new PositionData
            {
                Latitude = (float)lat,
                Longitude = (float)lng,
                PositionAmbiguity = 0,
                SymbolTable = '/',
                SymbolCode = '>',
                Source = PositionType.Nmea
            }; // FIXME: GPS symbols
        }

        private static double StringToLatitude(string value, string ns)
        {
            if (value == null || value.Length < 3)
                return double.NaN;

            var latitude = int.Parse(value.Substring(0, 2), CultureInfo.InvariantCulture) + double.Parse(value.Substring(2), CultureInfo.InvariantCulture) / 60;
            if (ns == "S")
                latitude *= -1;

            return latitude;
        }

        private static double StringToLongitude(string value, string ew)
        {
            if (value == null || value.Length < 4)
                return double.NaN;

            var longitude = int.Parse(value.Substring(0, 3), CultureInfo.InvariantCulture) + double.Parse(value.Substring(3), CultureInfo.InvariantCulture) / 60;
            if (ew == "W")
                longitude *= -1;

            return longitude;
        }
    }
}
