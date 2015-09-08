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

namespace HamAprsParser
{
    /// <summary>
    ///     This class represents a callsign with (optional) ssid
    /// </summary>
    [DataContract]
    public class Callsign
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Callsign" /> class.
        /// </summary>
        /// <param name="call">The callsign and optional ssid.</param>
        public Callsign(string call)
        {
            var callssid = call.TrimEnd().Split('-');
            Base = callssid[0].ToUpperInvariant();
            Ssid = callssid.Length > 1 ? callssid[1] : null;
        }

        /// <summary>
        ///     Gets or sets the call.
        /// </summary>
        /// <value>
        ///     The call.
        /// </value>
        [DataMember]
        public string Base { get; set; }

        /// <summary>
        ///     Gets or sets the ssid.
        /// </summary>
        /// <value>
        ///     The ssid.
        /// </value>
        [DataMember]
        public string Ssid { get; set; }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Base + (string.IsNullOrEmpty(Ssid) ? string.Empty : "-" + Ssid);
    }
}