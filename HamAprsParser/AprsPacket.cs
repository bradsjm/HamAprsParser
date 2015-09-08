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
using HamAprsParser.Parsers;
using HamAprsParser.Payloads;

namespace HamAprsParser
{
    /// <summary>
    ///     This class represents an Aprs Packet.
    ///     To create instances, use the static Create method.
    /// </summary>
    [DataContract]
    public class AprsPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AprsPacket" /> class.
        /// </summary>
        public AprsPacket()
        {
            Created = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        [DataMember]
        public DateTimeOffset Created { get; set; }

        /// <summary>
        ///     Gets or sets the destination call.
        /// </summary>
        /// <value>
        ///     The destination call.
        /// </value>
        [DataMember]
        public Callsign DestinationCall { get; set; }

        /// <summary>
        ///     Gets or sets the aprs information payload.
        /// </summary>
        /// <value>
        ///     The aprs information details.
        /// </value>
        [DataMember]
        public Payload Payload { get; set; }

        /// <summary>
        ///     Gets or sets the digipeaters.
        /// </summary>
        /// <value>
        ///     The digipeaters.
        /// </value>
        [DataMember]
        public string[] Digipeaters { get; set; }

        /// <summary>
        ///     Gets or sets the original string.
        /// </summary>
        /// <value>
        ///     The original string.
        /// </value>
        [DataMember]
        public string Original { get; set; }

        /// <summary>
        ///     Gets or sets the source call.
        /// </summary>
        /// <value>
        ///     The source call.
        /// </value>
        [DataMember]
        public Callsign SourceCall { get; set; }

        /// <summary>
        /// Creates instance of AprsPacket from packet string.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <returns></returns>
        public static AprsPacket Create(string packet) => AprsPacketParser.CreateFromString(packet);

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return SourceCall + ">" + 
                DestinationCall + 
                (Digipeaters.Length > 0 ? "," : "") +
                string.Join(",", Digipeaters) + ":" + 
                Payload;
        }
    }
}