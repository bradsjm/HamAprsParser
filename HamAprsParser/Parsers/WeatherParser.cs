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
using HamAprsParser.Parsers.Formats;

namespace HamAprsParser.Parsers
{
    /// <summary>
    /// WeatherParser parses various APRS weather formats from a payload string.
    /// </summary>
    public static class WeatherParser
    {

        #region Public Methods

        /// <summary>
        /// Creates weatherpayload object from string.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns></returns>
        public static WeatherData CreateFromString(string payload)
        {
            float? rainInch1H = null, rainInch24H = null, rainInchMidnight = null;
            float? windSpeedMph = null, temperatureF = null, windGustMph = null, snowInches = null;
            int? windDirection = null, humidity = null, pressure = null, luminosity = null;
            var isValid = false;

            if (string.IsNullOrEmpty(payload)) return null;

            switch (payload[0])
            {
                case '*': // Peet Bros U-II Weather Station (eg: #50B7500820082)
                case '#': // Peet Bros U-II Weather Station (eg: *7007600000000)
                    // Are any of these still in existance from the 1990s?
                    return null;

                case '$': // Peet Bros Ultimeter 2000 Station (eg: $ULTW0031003702CE0069----000086A00001----011901CC00000005)
                case '!': // Peet Bros Ultimeter 2000 Station (eg: !!006B005803500000----03E9--------002105140000005D)
                    isValid = UltimeterWx.Parse(payload, out windSpeedMph, out windDirection, out temperatureF,
                        out pressure, out humidity, out rainInchMidnight);
                    break;

                default:
                    if (AprsWx.ParseWindAndTemperature(payload, out windSpeedMph, out windDirection, out temperatureF,
                        out windGustMph))
                    {
                        AprsWx.ParseRain(payload, out rainInch1H, out rainInch24H, out rainInchMidnight);
                        AprsWx.ParseHumidity(payload, out humidity);
                        AprsWx.ParsePressure(payload, out pressure);
                        AprsWx.ParseLuminosity(payload, out luminosity);
                        AprsWx.ParseSnowfall(payload, out snowInches);
                        isValid = true;
                    }
                    break;
            }

            return isValid ? new WeatherData
            {
                RainInch1H = rainInch1H,
                RainInch24H = rainInch24H,
                RainInchMidnight = rainInchMidnight,
                WindSpeedMph = windSpeedMph,
                WindDirection = windDirection,
                TemperatureF = temperatureF,
                WindGustMph = windGustMph,
                Humidity = humidity,
                PressureMb = pressure,
                Luminosity = luminosity,
                SnowInches = snowInches
            } : null;
        }

        #endregion Private Methods
    }
}