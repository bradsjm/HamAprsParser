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
    /// This class represents a Weather APRS payload.
    /// </summary>
    [DataContract]
    public class WeatherData
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the humidity.
        /// </summary>
        /// <value>
        /// The humidity.
        /// </value>
        [DataMember]
        public int? Humidity { get; set; }

        /// <summary>
        /// Gets or sets the luminosity.
        /// </summary>
        /// <value>
        /// The luminosity.
        /// </value>
        [DataMember]
        public int? Luminosity { get; set; }

        /// <summary>
        /// Gets or sets the pressure.
        /// </summary>
        /// <value>
        /// The pressure.
        /// </value>
        [DataMember]
        public int? PressureMb { get; set; }

        /// <summary>
        /// Gets or sets the rain inch1 h.
        /// </summary>
        /// <value>
        /// The rain inch1 h.
        /// </value>
        [DataMember]
        public float? RainInch1H { get; set; }

        /// <summary>
        /// Gets or sets the rain inch24 h.
        /// </summary>
        /// <value>
        /// The rain inch24 h.
        /// </value>
        [DataMember]
        public float? RainInch24H { get; set; }

        /// <summary>
        /// Gets or sets the rain inch midnight.
        /// </summary>
        /// <value>
        /// The rain inch midnight.
        /// </value>
        [DataMember]
        public float? RainInchMidnight { get; set; }

        /// <summary>
        /// Gets or sets the snow inches.
        /// </summary>
        /// <value>
        /// The snow inches.
        /// </value>
        [DataMember]
        public float? SnowInches { get; set; }

        /// <summary>
        /// Gets or sets the temperature f.
        /// </summary>
        /// <value>
        /// The temperature f.
        /// </value>
        [DataMember]
        public float? TemperatureF { get; set; }

        /// <summary>
        /// Gets or sets the wind course.
        /// </summary>
        /// <value>
        /// The wind course.
        /// </value>
        [DataMember]
        public int? WindDirection { get; set; }

        /// <summary>
        /// Gets or sets the wind gust MPH.
        /// </summary>
        /// <value>
        /// The wind gust MPH.
        /// </value>
        [DataMember]
        public float? WindGustMph { get; set; }

        /// <summary>
        /// Gets or sets the wind speed MPH.
        /// </summary>
        /// <value>
        /// The wind speed MPH.
        /// </value>
        [DataMember]
        public float? WindSpeedMph { get; set; }

        #endregion Public Properties
    }
}