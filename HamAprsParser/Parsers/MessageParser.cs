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
    internal static class MessageParser
    {
        /// <summary>
        /// Creates from string.
        /// </summary>
        /// <param name="packet">The packet string.</param>
        /// <param name="destCall">The dest call.</param>
        /// <returns>AprsPacket</returns>
        /// <exception cref="HamAprsParser.Exceptions.AprsParserException">Unable to parse message payload in packet</exception>
        public static MessagePayload CreateFromString(string packet, Callsign destCall)
        {
            var result = Patterns.MessageRegex.Match(packet);

            if (!result.Success)
                throw new AprsParserException(packet, "Unable to parse message payload in packet");

            var payload = new MessagePayload(packet)
            {
                Type = PayloadType.Message,
                TargetCall =  new Callsign(result.Groups["target"].Value),
                MessageBody = result.Groups["content"].Value.TrimEnd(),
                MessageId = null
            };

            if (destCall.Base == "BEACON")
                payload.TargetCall = destCall;

            // Check for Ack type message
            result = Patterns.AckRegex.Match(payload.MessageBody);
            if (result.Success)
            {
                payload.IsAck = true;
                payload.MessageId = result.Groups["ack"].Value;
                payload.MessageBody = null;
                return payload;
            }

            // Check for Rej type message
            result = Patterns.RejRegex.Match(payload.MessageBody);
            if (result.Success)
            {
                payload.IsRej = true;
                payload.MessageId = result.Groups["rej"].Value;
                payload.MessageBody = null;
                return payload;
            }

            // Attempt to seperate id from message if present
            result = Patterns.MsgIdRegex.Match(payload.MessageBody);
            if (result.Success)
            {
                payload.MessageId = result.Groups["msgid"].Value;
                payload.MessageBody = result.Groups["msg"].Value.TrimEnd();
                return payload;
            }

            // Check for telemetry indicator
            result = Patterns.TelemetryRegex.Match(payload.MessageBody);
            if (result.Success)
                payload.Type = PayloadType.Telemetry;

            return payload;
        }
    }
}