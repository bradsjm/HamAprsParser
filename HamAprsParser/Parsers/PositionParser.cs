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
using HamAprsParser.Parsers.Formats;

namespace HamAprsParser.Parsers
{
    /// <summary>
    ///     This class with decode any of the Position formats specified in the APRS spec, including compressed,
    ///     uncompressed, and MicE encoded positions
    /// </summary>
    internal static class PositionParser
    {
        #region Private Fields

        /// <summary>
        ///  The comment may contain an altitude value, in the form /A=aaaaaa, where aaaaaa is the altitude
        ///  in feet. For example: /A=001234. The altitude may appear anywhere in the comment.
        /// </summary>
        private static readonly Regex AltitudeRegex = new Regex("/A=(?<alt>-[0-9]{5}|[0-9]{6})", RegexOptions.ExplicitCapture);

        private static readonly Regex CompressedPosition = new Regex(
            @"^[\/\\A-Za-j][\x21-\x7b]{8}[\x21-\x7b\x7d][\x20-\x7b]{3}");

        private static readonly Regex MicEPosition = new Regex(
            @"^[\x26-\x7f][\x26-\x61][\x1c-\x7f]{2}[\x1c-\x7d][\x1c-\x7f][\x21-\x7b\x7d][\/\\A-Z0-9]");

        private static readonly Regex UnCompressedPosition = new Regex(
            @"(?<latdeg>[0-9][0-9])(?<latmin>[0-7 ][0-9 ]\.[0-9 ][0-9 ])(?<ns>[nNSs])(?<symtable>[\/\\A-Z0-9])(?<longdeg>[0-1][0-9][0-9])(?<longmin>[0-7 ][0-9 ]\.[0-9 ][0-9 ])(?<ew>[eEwW])(?<symcode>[\x21-\x7b\x7d])",
            RegexOptions.ExplicitCapture);

        #endregion Private Fields

        #region Public Methods

        public static PositionData CreateFromString(string payload, Callsign destination = null)
        {
            switch (payload[0])
            {
                case '\'':
                case '`':
                    if (MicEPosition.IsMatch(payload) && destination != null)
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
                    var match = CompressedPosition.Match(payload);
                    if (match.Success)
                        return Formats.CompressedPosition.ParsePosition(payload);

                    // Check for uncompressed position
                    match = UnCompressedPosition.Match(payload);
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
            var match = AltitudeRegex.Match(comment);
            if (!match.Success || !int.TryParse(match.Groups["alt"].Value, out altitiude)) return null;
            return altitiude;
        }

        #endregion Public Methods

    }
}