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
using HamAprsParser.Exceptions;
using HamAprsParser.Payloads;

namespace HamAprsParser.Parsers
{
    internal static class ObjectParser
    {
        private static readonly Regex ObjectRegex = new Regex(
            @"^;(?<name>[\x20-\x7e]{9})\s*(?<state>[*_])(?<timestamp>[0-9]{6}[zh/])(?<position>.+)$",
            RegexOptions.ExplicitCapture);

        public static ObjectPayload CreateFromString(string packet)
        {
            var result = ObjectRegex.Match(packet);

            if (!result.Success)
                throw new AprsParserException(packet, "Unable to parse object payload in packet");

            var position = PositionParser.CreateFromString(result.Groups["position"].Value);
            if (position == null)
                throw new AprsParserException(packet, "Unable to parse object position");

            var timestamp = TimestampParser.CreateFromString(result.Groups["timestamp"].Value);
            if (timestamp == null)
                throw new AprsParserException(packet, "Unable to parse object timestamp");

            return new ObjectPayload(packet)
            {
                Name = result.Groups["name"].Value.TrimEnd(),
                IsLive = result.Groups["state"].Value[0] == '*',
                Timestamp = timestamp,
                Position = position
            };
        }
    }
}