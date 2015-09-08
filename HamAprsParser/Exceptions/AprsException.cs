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
    /// Base Aprs Exception Class. All APRS exceptions inherit from this class.
    /// </summary>
    public abstract class AprsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AprsException" /> class.
        /// </summary>
        protected AprsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AprsException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected AprsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AprsException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        protected AprsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
