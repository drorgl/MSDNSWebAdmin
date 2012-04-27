/*
UnixDateTime - Unix datetime helper
Copyright (C) 2011 Dror Gluska
	
This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public License
(LGPL) as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.
The terms of redistributing and/or modifying this software also
include exceptions to the LGPL that facilitate static linking.
 	
This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
Lesser General Public License for more details.
 	
You should have received a copy of the GNU Lesser General Public License
along with this library; if not, write to Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA


Change log:
2012-03-01 - Initial version

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNSManagement.Extensions
{
    /// <summary>
    /// DateTime/Unix datetime converter
    /// </summary>
    public class UnixDateTime
    {
        /// <summary>
        /// Holds unix datetime
        /// </summary>
        private double _unixtimestamp;

        /// <summary>
        /// Holds C# DateTime
        /// </summary>
        private DateTime _timestamp;

        /// <summary>
        /// .ctor in unix datetime
        /// </summary>
        /// <param name="unixtime"></param>
        public UnixDateTime(double unixtime)
        {
            _unixtimestamp = unixtime;
            _timestamp = UnixTimeStampToDateTime(_unixtimestamp);
        }

        /// <summary>
        /// .ctor in C# DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        public UnixDateTime(DateTime timestamp)
        {
            _timestamp = timestamp;
            _unixtimestamp = DateTimeToUnixTimestamp(_timestamp);
        }

        /// <summary>
        /// Converts a unix datetime to C# DateTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Converts C# DateTime to unix datetime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToUniversalTime()).TotalSeconds;
        }

        /// <summary>
        /// DateTime format
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return _timestamp;
            }
        }

        /// <summary>
        /// Unix datetime format
        /// </summary>
        public double Unix
        {
            get
            {
                return _unixtimestamp;
            }
        }


        public override string ToString()
        {
            return _timestamp.ToString();
        }


    }
}
