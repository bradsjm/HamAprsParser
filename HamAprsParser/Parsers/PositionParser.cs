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
using HamAprsParser.Parsers.Formats;

namespace HamAprsParser.Parsers
{
    /// <summary>
    ///     This class with decode any of the Position formats specified in the APRS spec, including compressed,
    ///     uncompressed, and MicE encoded positions
    /// </summary>
    internal static class PositionParser
    {
        #region Public Methods

        public static PositionData CreateFromString(string payload, Callsign destination = null)
        {
            switch (payload[0])
            {
                case '\'':
                case '`':
                    if (Patterns.MicEPosition.IsMatch(payload) && destination != null)
                    {
                        var position = Formats.MicEPosition.ParsePosition(payload, destination);
                        //extension = Formats.MicEPosition.ParseCourseAndSpeed(payload, destination);
                        return position;
                    }
                    break;

                case '$':
                    if (!payload.StartsWith("$ULT", StringComparison.Ordinal))
                        return NmeaPosition.ParsePosition(payload);
                    break;

                case '!':
                    if (payload.StartsWith("!!", StringComparison.Ordinal))
                        return null;
                    goto default;

                default:
                    // Remove timestamp from payload
                    if ((payload[0] == '/' || payload[0] == '@') && payload.Length > 8)
                        payload = payload.Substring(8);

                    // Check for compressed position
                    var match = Patterns.CompressedPosition.Match(payload);
                    if (match.Success)
                        return Formats.CompressedPosition.ParsePosition(payload);

                    // Check for uncompressed position
                    match = Patterns.UnCompressedPosition.Match(payload);
                    if (match.Success)
                    {
                        var position = Formats.UnCompressedPosition.Convert(
                            match.Groups["latdeg"].Value,
                            match.Groups["latmin"].Value,
                            match.Groups["ns"].Value[0],
                            match.Groups["longdeg"].Value,
                            match.Groups["longmin"].Value,
                            match.Groups["ew"].Value[0]
                            );

                        // TODO: Does this really belong here?
                        position.SymbolTable = match.Groups["symtable"].Value[0];
                        position.SymbolCode = match.Groups["symcode"].Value[0];
                        position.AltitudeFt = ParseAltitude(payload);
                        return position;
                    }
                    break;

            }

            return null;
        }

        private static int? ParseAltitude(string comment)
        {
            int altitiude;
            var match = Patterns.AltitudeRegex.Match(comment);
            if (!match.Success || !int.TryParse(match.Groups["alt"].Value, out altitiude)) return null;
            return altitiude;
        }

        #endregion Public Methods

    }
}