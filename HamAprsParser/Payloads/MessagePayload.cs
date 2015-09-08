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
using HamAprsParser.Parsers;

namespace HamAprsParser.Payloads
{
    /// <summary>
    /// This class represents an Aprs message payload.
    /// </summary>
    [DataContract]
    public class MessagePayload : Payload
    {
        //
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePayload" /> class.
        /// </summary>
        /// <param name="body">The body.</param>
        public MessagePayload(string body) : base(body)
        {
            Type = PayloadType.Message;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is ack.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is ack; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsAck { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is rej.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is rej; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsRej { get; set; }

        /// <summary>
        ///     Gets the message body.
        /// </summary>
        /// <value>
        ///     The message body.
        /// </value>
        [DataMember]
        public string MessageBody { get; set; }

        /// <summary>
        ///     Gets the message number.
        /// </summary>
        /// <value>
        ///     The message number.
        /// </value>
        [DataMember]
        public string MessageId { get; set; }

        /// <summary>
        ///     Gets the target callsign.
        /// </summary>
        /// <value>
        ///     The target callsign.
        /// </value>
        [DataMember]
        public Callsign TargetCall { get; set; }

        /// <summary>
        /// Creates the specified body.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="destCall">The dest call.</param>
        /// <returns></returns>
        public static MessagePayload Create(string body, Callsign destCall) => MessageParser.CreateFromString(body, destCall);

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (MessageBody.Equals("ack") || MessageBody.Equals("rej"))
            {
                return $":{TargetCall.ToString().PadRight(9)}:{MessageBody}{MessageId}";
            }

            if (MessageId.Length > 0)
            {
                return $":{TargetCall.ToString().PadRight(9)}:{MessageBody}{{{MessageId}";
            }

            return $":{TargetCall.ToString().PadRight(9)}:{MessageBody}";
        }
    }
}