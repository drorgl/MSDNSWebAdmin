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

namespace DNSManagement.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendLineFormat(this StringBuilder sb, string value, params object[] parameters)
        {
            if ((parameters == null) || (parameters.Length == 0))
                return sb.AppendLine(value);

            List<object> newparams = new List<object>();
            foreach (var p in parameters)
                if (p == null)
                    newparams.Add(string.Empty);
                else
                    newparams.Add(p);

            sb.AppendFormat(value, newparams.ToArray());
            return sb.AppendLine();
        }
    }
}
