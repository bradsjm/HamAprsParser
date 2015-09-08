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

namespace HamAprsParser.Exceptions
{
    /// <summary>
    /// Base Aprs Exception Class
    /// </summary>
    public class AprsPacketException : AprsException
    {
        /// <summary>
        /// Gets the packet string.
        /// </summary>
        /// <value>
        /// The packet.
        /// </value>
        public string Packet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AprsPacketException" /> class.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="message">The message that describes the error.</param>
        public AprsPacketException(string packet, string message)
            : this(packet, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AprsPacketException" /> class.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public AprsPacketException(string packet, string message, Exception inner)
            : base(message + "[[" + packet + "]]", inner)
        {
            Packet = packet;
        }
    }
}
