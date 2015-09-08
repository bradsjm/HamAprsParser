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
    ///     This class represents an unsupported Aprs payload.
    /// </summary>
    [DataContract]
    public class UnsupportedPayload : Payload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedPayload"/> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="comment">The comment.</param>
        public UnsupportedPayload(string payload, string comment = null) : base(payload)
        {
            Type = PayloadType.Unspecified;
            Comment = comment;
        }

    }
}