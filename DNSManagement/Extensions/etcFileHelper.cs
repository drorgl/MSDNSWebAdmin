/*
IPHelper - IP Helper methods
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
2012-04-09 - Initial Version
 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DNSManagement.Extensions
{
    /// <summary>
    /// system32\etc file reader
    /// </summary>
    public class etcFileHelper
    {
        public const string ServicesFilename = "services";

        public const string ProtocolFilename = "protocol";

        public class WKService
        {
            public string Name;
            public int Port;
            public string Protocol;
        }

        public class InternetProtocol
        {
            public string Name;
            public int ProtocolNumber;
        }

        public static string GetFullFilename(string filename)
        {
            //attempt to find in local directory
            if (File.Exists(filename))
                return filename;

            //attempt to find in system32/drivers/etc folder
            var systemfilename = Path.Combine(Environment.SystemDirectory, "drivers/etc", filename);
            if (File.Exists(systemfilename))
                return systemfilename;

            return filename;
        }

        public static List<InternetProtocol> ReadProtocols(string filename)
        {
            List<InternetProtocol> protocolslist = new List<InternetProtocol>();

            string line = "";
            using (StreamReader file = new StreamReader(filename))
            {
                while ((line = file.ReadLine()) != null)
                {
                    var cleanline = line.Replace("\t", " ").Replace("  ", " ");
                    if (cleanline.IndexOf('#') != -1)
                        cleanline = cleanline.Substring(0, cleanline.IndexOf('#'));

                    var parts = cleanline.Split(' ').Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
                    if (parts.Count < 2)
                        continue;

                    var name = parts[0];
                    var num = parts[1];

                    protocolslist.Add(new InternetProtocol
                    {
                        Name = name,
                        ProtocolNumber = Convert.ToInt32(num)
                    });

                    if (parts.Count > 2)
                    {
                        for (int i = 2; i < parts.Count; i++)
                            protocolslist.Add(new InternetProtocol
                            {
                                Name = parts[i],
                                ProtocolNumber = Convert.ToInt32(num)
                            });
                    }

                }
                file.Close();
            }

            return protocolslist;

        }

        public static List<WKService> ReadServices(string filename)
        {
            List<WKService> servicelist = new List<WKService>();

            string line = "";
            using (StreamReader file = new StreamReader(filename))
            {
                while ((line = file.ReadLine()) != null)
                {
                    var cleanline = line.Replace("\t", " ").Replace("  ", " ");
                    if (cleanline.IndexOf('#') != -1)
                        cleanline = cleanline.Substring(0, cleanline.IndexOf('#'));

                    var parts = cleanline.Split(' ').Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
                    if (parts.Count < 2)
                        continue;

                    var name = parts[0];
                    var portserv = parts[1].Split('/');
                    var port = portserv[0];
                    var service = portserv[1];

                    servicelist.Add(new WKService
                    {
                        Name = name,
                        Port = Convert.ToInt32(port),
                        Protocol = service
                    });

                    if (parts.Count > 2)
                    {
                        for (int i = 2; i < parts.Count;i++)
                            servicelist.Add(new WKService
                            {
                                Name = parts[i],
                                Port = Convert.ToInt32(port),
                                Protocol = service
                            });
                    }

                }
                file.Close();
            }

            return servicelist;

        }
    }
}
