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
    /// This class represents an Aprs item payload.
    /// </summary>
    [DataContract]
    public class ItemPayload : PositionPayload
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPayload" /> class.
        /// </summary>
        /// <param name="payloadBody">The payload.</param>
        public ItemPayload(string payloadBody) : base(payloadBody)
        {
            Type = PayloadType.Item;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="ObjectPayload" /> is live.
        /// </summary>
        /// <value>
        ///     <c>true</c> if live; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsLive { get; set; }

        /// <summary>
        ///     Gets or sets the name of the object.
        /// </summary>
        /// <value>
        ///     The name of the object.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"){Name}{(IsLive ? '!' : '_')}{Position}{Comment}";

        #endregion Public Methods

    }
}