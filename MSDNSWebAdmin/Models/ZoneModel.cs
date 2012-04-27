///*
//DNS Web Admin - MS DNS Web Administration
//Copyright (C) 2011 Dror Gluska
	
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public License
//(LGPL) as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//The terms of redistributing and/or modifying this software also
//include exceptions to the LGPL that facilitate static linking.
 	
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//Lesser General Public License for more details.
 	
//You should have received a copy of the GNU Lesser General Public License
//along with this library; if not, write to Free Software Foundation, Inc.,
//51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA


//Change log:
//2011-12-03 - Initial version

//*/

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace MSDNSWebAdmin.Models
//{
//    public class ZoneModel
//    {
//        public string Name { get; set; }

//        public string DnsServerName { get; set; }

//        public string Status { get; set; }

//        public DNSManagement.Zone.ZoneTypeEnum Type { get; set; }

//        public string Filename { get; set; }

//        public DNSManagement.Zone.AllowUpdateEnum AllowUpdates { get; set; }

//        public ZoneModel(DNSManagement.Zone fromZone)
//        {
//            this.Name = fromZone.Name;
//            //Status
//            if (fromZone.Shutdown == true)
//                Status = "Zone not loaded";
//            else if (fromZone.Paused == true)
//                Status = "Paused";
//            else Status = "Running";

//            this.Type = fromZone.ZoneType;

//            this.Filename = fromZone.DataFile;
//            this.AllowUpdates = fromZone.AllowUpdate;

//            this.DnsServerName = fromZone.DnsServerName;
//            //fromZone.UseWins 

//        }
//    }
//}