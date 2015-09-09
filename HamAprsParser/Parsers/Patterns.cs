using System.Text.RegularExpressions;

namespace HamAprsParser.Parsers
{
    /// <summary>
    /// Regular Expression Patterns
    /// </summary>
    internal static class Patterns
    {
        // Envelope parser. E.g. JH6YLM>APRS,RELAY,TRACE5-5,qAo,JA6JMJ-3:!3210.70N/13132.15E#15 KAWA
        public static readonly Regex EnvelopeRegex = new Regex(
            "^(?<source>[A-Za-z0-9-]{1,9})>(?<dest>[A-Za-z0-9-,*]+?):(?<information>.+)$",
            RegexOptions.ExplicitCapture);

        // Day/Hours/Minutes zulu/local
        public static readonly Regex DayHourMinuteRegex =
            new Regex("^[/@]?(?<day>[0-3][0-9])(?<hour>[0-2][0-9])(?<minute>[0-5][0-9])(?<zone>[z/])", RegexOptions.ExplicitCapture);

        // Hour/Minute/Second zulu
        public static readonly Regex HourMinuteSecondRegex =
            new Regex("^[/@]?(?<hour>[0-2][0-9])(?<minute>[0-5][0-9])(?<second>[0-5][0-9])h", RegexOptions.ExplicitCapture);

        // Month/Day/Hour/Minute zulu
        public static readonly Regex MonthDayHourMinuteRegex =
            new Regex("^(?<month>[01][0-9])(?<day>[0-3][0-9])(?<hour>[0-2][0-9])(?<minute>[0-5][0-9])", RegexOptions.ExplicitCapture);

        /// <summary>
        ///  The comment may contain an altitude value, in the form /A=aaaaaa, where aaaaaa is the altitude
        ///  in feet. For example: /A=001234. The altitude may appear anywhere in the comment.
        /// </summary>
        public static readonly Regex AltitudeRegex = new Regex("/A=(?<alt>-[0-9]{5}|[0-9]{6})", RegexOptions.ExplicitCapture);

        public static readonly Regex CompressedPosition = new Regex(
            @"^[\/\\A-Za-j][\x21-\x7b]{8}[\x21-\x7b\x7d][\x20-\x7b]{3}");

        public static readonly Regex MicEPosition = new Regex(
            @"^[\x26-\x7f][\x26-\x61][\x1c-\x7f]{2}[\x1c-\x7d][\x1c-\x7f][\x21-\x7b\x7d][\/\\A-Z0-9]");

        public static readonly Regex UnCompressedPosition = new Regex(
            @"(?<latdeg>[0-9][0-9])(?<latmin>[0-7 ][0-9 ]\.[0-9 ][0-9 ])(?<ns>[nNSs])(?<symtable>[\/\\A-Z0-9])(?<longdeg>[0-1][0-9][0-9])(?<longmin>[0-7 ][0-9 ]\.[0-9 ][0-9 ])(?<ew>[eEwW])(?<symcode>[\x21-\x7b\x7d])",
            RegexOptions.ExplicitCapture);

        public static readonly Regex ObjectRegex = new Regex(
            @"^;(?<name>[\x20-\x7e]{9})\s*(?<state>[*_])(?<timestamp>[0-9]{6}[zh/])(?<position>.+)$",
            RegexOptions.ExplicitCapture);

        // :BLNA       : Mt Gambier Digi on 145.175Mhz
        public static readonly Regex AckRegex = new Regex(
            "^ack(?<ack>[A-Za-z0-9]{1,5})", RegexOptions.ExplicitCapture);

        public static readonly Regex MessageRegex = new Regex(
            @"^:(?<target>[A-Za-z0-9_ -]{9})\s*:(?<content>.*)$",
            RegexOptions.ExplicitCapture);

        public static readonly Regex MsgIdRegex = new Regex(
            "^(?<msg>[^{]*)\\{(?<msgid>[A-Za-z0-9]{1,5})", RegexOptions.ExplicitCapture);

        public static readonly Regex RejRegex = new Regex(
            "^rej(?<rej>[A-Za-z0-9]{1,5})", RegexOptions.ExplicitCapture);

        public static readonly Regex TelemetryRegex = new Regex(
            "^(BITS|PARM|UNIT|EQNS)\\.");

        public static readonly Regex ItemRegex = new Regex(
            @"^\)(?<name>[\x20-\x7e]{3,9})(?<state>[!_])(?<position>.+)$",
            RegexOptions.ExplicitCapture);

        // Destination callsign is six characters.
        // A-K characters are not used in the last 3 characters and MNO are never used
        public static readonly Regex MicEDestRegex = new Regex("^[0-9A-LP-Z]{3}[0-9LP-Z]{3}$", RegexOptions.IgnoreCase);

        public static readonly Regex DataLoggerMode = new Regex(
            "^!!(?<wind>[0-9A-F-]{4})([0F-]{2})(?<dir>[0-9A-F-]{2})(?<outtemp>[0-9A-F-]{4})(?<raintotal>[0-9A-F-]{4})(?<pressure>[0-9A-F-]{4})(?<intemp>[0-9A-F-]{4})(?<humidity>[0-9A-F-]{4})(?<inhumidity>[0-9A-F-]{4})(?<day>[0-9A-F-]{4})(?<time>[0-9A-F-]{4})(?<dayrain>[0-9A-F-]{4})?(?<windavg>[0-9A-F-]{4})?$",
            RegexOptions.ExplicitCapture);

        public static readonly Regex PacketMode = new Regex(
            @"^\$ULTW(?<wind>[0-9A-F-]{4})([0F-]{2})(?<dir>[0-9A-F-]{2})(?<outtemp>[0-9A-F-]{4})(?<raintotal>[0-9A-F-]{4})(?<pressure>[0-9A-F-]{4})([0-9A-F-]{4})([0-9A-F-]{4})([0-9A-F-]{4})(?<humidity>[0-9A-F-]{4})(?<day>[0-9A-F-]{4})(?<time>[0-9A-F-]{4})(?<dayrain>[0-9A-F-]{4})(?<windavg>[0-9A-F-]{4})$",
            RegexOptions.ExplicitCapture);

        /// <summary>
        /// The 7-byte CSE/SPD Data Extension can be used to represent the course and
        /// speed of a vehicle or APRS Object. The course is expressed in degrees (001-360),
        /// clockwise from due north. The speed is expressed in knots.
        /// </summary>
        public static readonly Regex CourseSpeedExtension = new Regex(@"^.{20}(?<course>[0-9. ]{3})/(?<speed>[0-9. ]{3})", RegexOptions.ExplicitCapture);

        /// <summary>
        /// The 7-byte PHGphgd Data Extension specifies the transmitter power,
        /// effective antenna height-above-average-terrain, antenna gain and antenna directivity.
        /// APRS uses this information to plot radio range circles around stations.
        /// </summary>
        public static readonly Regex PhgExtension = new Regex(@"^.{20}PHG(?<power>[0-9])(?<height>[\x30-\x7e])(?<gain>[0-9])(?<dir>[0-8])(?<rate>[0-9A-Z])?", RegexOptions.ExplicitCapture);
    }
}
