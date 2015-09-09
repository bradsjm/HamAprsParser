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
using System.Text.RegularExpressions;
using HamAprsParser.Exceptions;

namespace HamAprsParser.Parsers
{
    /// <summary>
    /// APRS Time stamp format parsers
    /// </summary>
    internal static class TimestampParser
    {
        /// <summary>
        /// Parses the Day Hour Minute and Hour Minute Second formats and attempts to return a date time offset.
        /// </summary>
        /// <param name="payload">The aprs timestamp.</param>
        /// <param name="reference">The reference time (optional).</param>
        /// <returns></returns>
        public static DateTimeOffset? CreateFromString(string payload, DateTimeOffset? reference = null)
        {
            return ParseDayHourMinute(payload, reference)
                   ?? ParseHourMinuteSecond(payload, reference)
                   ?? ParseMonthDayHourMinute(payload, reference);
        }

        /// <summary>
        /// Parses the aprs day hour minute format.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="reference">The reference time (optional).</param>
        /// <returns></returns>
        public static DateTimeOffset? ParseDayHourMinute(string timestamp, DateTimeOffset? reference = null)
        {
            var result = Patterns.DayHourMinuteRegex.Match(timestamp);
            if (!result.Success) return null;

            var offset = result.Groups["zone"].Value == "z" ? DateTimeOffset.UtcNow.Offset : DateTimeOffset.Now.Offset;
            var current = reference ?? (result.Groups["zone"].Value == "z" ? DateTimeOffset.UtcNow : DateTimeOffset.Now);
            var day = int.Parse(result.Groups["day"].Value);
            var hour = int.Parse(result.Groups["hour"].Value);
            var minute = int.Parse(result.Groups["minute"].Value);

            try
            {
                var time = new DateTimeOffset(current.Year, current.Month, day, hour, minute, 0, offset);
                // If the time is more than twelve hours into the future, roll the timestamp one month backwards.
                if (time > current.AddHours(12))
                    return time.AddMonths(-1);

                return time;
            }
            catch (ArgumentException ex)
            {
                throw new AprsParserException(timestamp, "Unable to parse APRS Day Hour Minute", ex);
            }
        }

        /// <summary>
        /// Parses the aprs hour minute second format.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="reference">The reference time (optional).</param>
        /// <returns></returns>
        public static DateTimeOffset? ParseHourMinuteSecond(string timestamp, DateTimeOffset? reference = null)
        {
            var result = Patterns.HourMinuteSecondRegex.Match(timestamp);
            if (!result.Success) return null;

            var current = reference ?? DateTimeOffset.UtcNow;
            var hour = int.Parse(result.Groups["hour"].Value);
            var minute = int.Parse(result.Groups["minute"].Value);
            var second = int.Parse(result.Groups["second"].Value);

            try
            {
                var time = new DateTimeOffset(current.Year, current.Month, current.Day, hour, minute, second,
                    TimeSpan.Zero);

                // If the time is more than one hour into the future, roll the timestamp one day backwards.
                if (time > current.AddHours(1))
                    return time.AddDays(-1);

                // If the time is more than 23 hours into the past, roll the timestamp one day forwards.
                if (time < current.AddHours(-23))
                    return time.AddDays(1);

                return time;
            }
            catch (ArgumentException ex)
            {
                throw new AprsParserException("Unable to parse APRS Hour Minute Second: " + timestamp, ex);
            }
        }

        /// <summary>
        /// Parses the aprs month day hour minute format.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="reference">The reference time (optional).</param>
        /// <returns></returns>
        public static DateTimeOffset? ParseMonthDayHourMinute(string timestamp, DateTimeOffset? reference = null)
        {
            var result = Patterns.MonthDayHourMinuteRegex.Match(timestamp);
            if (!result.Success) return null;

            var current = reference ?? DateTimeOffset.UtcNow;
            var month = int.Parse(result.Groups["month"].Value);
            var day = int.Parse(result.Groups["day"].Value);
            var hour = int.Parse(result.Groups["hour"].Value);
            var minute = int.Parse(result.Groups["minute"].Value);

            try
            {
                var time = new DateTimeOffset(current.Year, month, day, hour, minute, 0, TimeSpan.Zero);

                // If the time is more than twelve hours into the future, roll the timestamp one month backwards.
                if (time > current.AddHours(12))
                    return time.AddMonths(-1);

                return time;
            }
            catch (ArgumentException ex)
            {
                throw new AprsParserException("Unable to parse APRS Month Day Hour Minute: " + timestamp, ex);
            }
        }
    }
}
