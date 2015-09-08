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
using System.Runtime.Serialization;

namespace HamAprsParser.Payloads
{
    /// <summary>
    ///     This class represents an Aprs position payload.
    /// </summary>
    [DataContract]
    public class PositionPayload : Payload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionPayload" /> class.
        /// </summary>
        /// <param name="payloadBody">The payload.</param>
        public PositionPayload(string payloadBody) : base(payloadBody)
        {
            Type = PayloadType.Position;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [DataMember]
        public PositionData Position { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        [DataMember]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => (CanMessage ? "=" : "!") + Position + Comment;
    }
}