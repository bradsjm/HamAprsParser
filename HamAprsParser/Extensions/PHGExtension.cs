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

namespace HamAprsParser.Extensions
{
    public class PhgExtension : IDataExtension
    {
        /// <summary>
        ///     Gets or sets the power.
        /// </summary>
        /// <value>
        ///     The power.
        /// </value>
        public int Power { get; set; }

        /// <summary>
        ///     Gets or sets the height.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        ///     Gets or sets the gain.
        /// </summary>
        /// <value>
        ///     The gain.
        /// </value>
        public int Gain { get; set; }

        /// <summary>
        ///     Gets or sets the directivity.
        /// </summary>
        /// <value>
        ///     The directivity.
        /// </value>
        public int Directivity { get; set; }

        /// <summary>
        /// Gets or sets the rate.
        /// </summary>
        /// <value>The rate.</value>
        public int? Rate { get; set; }
    }
}