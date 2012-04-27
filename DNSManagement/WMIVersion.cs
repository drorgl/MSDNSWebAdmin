/*
DNSManagement - Wrapper for WMI Management of MS DNS
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
2011-05-17 - Initial version

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNSManagement
{
    /// <summary>
    /// WMI Version
    /// </summary>
    public class WMIVersion
    {
        private UInt32 m_version;

        /// <summary>
        /// Initializes an instance from UInt32 version value
        /// </summary>
        /// <param name="version"></param>
        public WMIVersion(UInt32 version)
        {
            m_version = version;
        }


        /// <summary>
        /// Gets the value of the major component of the version number
        /// </summary>
        public int Major
        {
            get
            {
                var value = this.ToHex();
                value = value.Substring(value.Length - 2, 2);
                return int.Parse(value, System.Globalization.NumberStyles.HexNumber);
            }
        }

        /// <summary>
        /// Gets the value of the minor component of the version number
        /// </summary>
        public int Minor
        {
            get
            {
                var value = this.ToHex();
                value = value.Substring(value.Length - 4, 2);
                return int.Parse(value, System.Globalization.NumberStyles.HexNumber);
            }
        }

        /// <summary>
        /// Gets the value of the revision component of the version number
        /// </summary>
        public int Revision
        {
            get
            {
                var value = this.ToHex();
                value = value.Substring(0, 4);
                return int.Parse(value, System.Globalization.NumberStyles.HexNumber);
            }
        }

        /// <summary>
        /// Converts the version to HEX 
        /// </summary>
        /// <returns></returns>
        private string ToHex()
        {
            return Convert.ToString(m_version, 16);
        }

        /// <summary>
        /// Displays the version in Microsoft's way
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}.{1}.{2} (0x{3})", this.Major, this.Minor, this.Revision, this.Revision.ToString("X"));
        }


        
    }
}
