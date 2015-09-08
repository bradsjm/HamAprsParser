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

using System.Runtime.Serialization;

namespace HamAprsParser.Payloads
{
    /// <summary>
    /// This class represents an Aprs object payload.
    /// </summary>
    [DataContract]
    public class ObjectPayload : ItemPayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionPayload" /> class.
        /// </summary>
        /// <param name="payloadBody">The payload.</param>
        public ObjectPayload(string payloadBody) : base(payloadBody)
        {
            Type = PayloadType.Object;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $";{Name.PadRight(9)}{(IsLive ? '*' : '_')}{Position}{Comment}";
    }
}