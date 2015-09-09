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

using HamAprsParser.Exceptions;
using HamAprsParser.Payloads;

namespace HamAprsParser.Parsers
{
    internal static class ItemParser
    {
        public static ItemPayload CreateFromString(string packet)
        {
            var result = Patterns.ItemRegex.Match(packet);

            if (!result.Success)
                throw new AprsParserException(packet, "Unable to parse item payload in packet");

            var position = PositionParser.CreateFromString(result.Groups["position"].Value);
            if (position == null)
                throw new AprsParserException(packet, "Unable to parse object position");

            return new ItemPayload(packet)
            {
                Name = result.Groups["name"].Value.TrimEnd(),
                IsLive = result.Groups["state"].Value[0] == '*',
                Position = position
            };
        }
    }
}