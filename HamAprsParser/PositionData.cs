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

namespace HamAprsParser
{
    /// <summary>
    ///     This object represents an APRS position / location.
    /// </summary>
    [DataContract]
    public class PositionData
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionData"/> class.
        /// </summary>
        public PositionData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionData" /> class.
        /// </summary>
        /// <param name="lat">The lat.</param>
        /// <param name="lon">The lon.</param>
        /// <param name="posAmbiguity">The position ambiguity.</param>
        /// <param name="symbolTable">The symbol table.</param>
        /// <param name="symbolCode">The symbol code.</param>
        /// <param name="source">The source.</param>
        [Obsolete]
        public PositionData(float lat, float lon, int posAmbiguity, char symbolTable, char symbolCode, PositionType source = PositionType.Unspecified)
        {
            Latitude = lat;
            Longitude = lon;
            PositionAmbiguity = posAmbiguity;
            SymbolTable = symbolTable;
            SymbolCode = symbolCode;
            Source = source;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the altitude.
        /// </summary>
        /// <value>The height.</value>
        [DataMember]
        public int? AltitudeFt { get; set; }

        /// <summary>
        ///     Gets or sets the latitude.
        /// </summary>
        /// <value>
        ///     The latitude.
        /// </value>
        [DataMember]
        public float Latitude { get; set; }

        /// <summary>
        ///     Gets or sets the longitude.
        /// </summary>
        /// <value>
        ///     The longitude.
        /// </value>
        [DataMember]
        public float Longitude { get; set; }

        /// <summary>
        ///     Gets or sets the position ambiguity.
        /// </summary>
        /// <value>
        ///     The position ambiguity.
        /// </value>
        [DataMember]
        public int PositionAmbiguity { get; set; }

        /// <summary>
        /// Gets or sets the position source.
        /// </summary>
        /// <value>
        /// The position source.
        /// </value>
        [DataMember]
        public PositionType Source { get; set; }

        /// <summary>
        ///     Gets or sets the symbol code.
        /// </summary>
        /// <value>
        ///     The symbol code.
        /// </value>
        [DataMember]
        public char? SymbolCode { get; set; }

        /// <summary>
        ///     Gets or sets the symbol table.
        /// </summary>
        /// <value>
        ///     The symbol table.
        /// </value>
        [DataMember]
        public char? SymbolTable { get; set; }

        #endregion Public Properties
    }
}