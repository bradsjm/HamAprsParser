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
using HamAprsParser.Exceptions;
using HamAprsParser.Extensions;

namespace HamAprsParser.Payloads
{
    /// <summary>
    ///     This class represents the "payload" of a packet, stripped of the source call,
    ///     destination call, and digi VIAs. Note this class is abstract:  only subclasses of it may be
    ///     instantiated.Per the APRS spec, these classes include Position, Direction Finding, Objects
    ///     and Items, Weather, Telemetry, Messages, Bulletins, Annoucements, Queries, Responses, Statuses,
    ///     and User-defined Others.
    /// </summary>
    [DataContract]
    public abstract class Payload
    {
        /// <summary>
        /// The raw payload content
        /// </summary>
        [DataMember]
        public string Body { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayloadBody"/> class.
        /// </summary>
        /// <param name="payloadBody">The payload.</param>
        protected Payload(string payloadBody)
        {
            if (string.IsNullOrWhiteSpace(payloadBody))
                throw new AprsParserException(payloadBody, "Payload is empty");

            Body = payloadBody;
            DataType = payloadBody[0];

            switch (DataType)
            {
                case '@':
                case '=':
                case '\'':
                case ':':
                    CanMessage = true;
                    break;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance can message.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can message; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool CanMessage { get; set; }

        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        /// <value>
        ///     The comment.
        /// </value>
        [DataMember]
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the data type identifier.
        /// </summary>
        /// <value>
        ///     The data type identifier.
        /// </value>
        [DataMember]
        public char DataType { get; set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        [DataMember]
        public IDataExtension Extension { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        [DataMember]
        public PayloadType Type { get; set; }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Body;
    }
}