/*
DNS Admin GUI - A test program for testing various DNSManagement library methods
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DNSManagement.Extensions;

namespace DNSAdminGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DNSManagement.Server server = null;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Log("Connecting to server..");
            server = new DNSManagement.Server("DNSSRV", "Administrator", "Pass@word1");
            Log("Connected");


        }

        private void Log(string message)
        {
            textBox1.Text += message + Environment.NewLine;
        }

        private void btnDumpServer_Click(object sender, EventArgs e)
        {
            Log("Dumping Server...");
            Log(server.ToString());
            Log("Done dumping server");
        }

        private void btnDomains_Click(object sender, EventArgs e)
        {
            Log("Dumping domains...");
            var domains = server.GetDomains();
            foreach (var domain in domains) 
                Log(domain.ToString());
            Log("Done dumping domains");
        }

        private void btnZones_Click(object sender, EventArgs e)
        {
            Log("Dumping zones...");
            var zones = server.GetZones();
            foreach (var zone in zones)
            {
                Log(zone.ToString());
                var records = zone.GetRecords();
                foreach (var r in records)
                    Log(r.ToString());
            }
            Log("done dumping zones");
        }

        private void btnCache_Click(object sender, EventArgs e)
        {
            Log("Dumping caches...");
            var caches = server.GetCache();
            foreach (var cache in caches)
            {
                Log(cache.ToString());
                var cacherecords = cache.GetRecords();
                foreach (var cr in cacherecords)
                    Log(cr.ToString());
            }
            Log("Done dumping caches.");
        }

        private void btnRootHints_Click(object sender, EventArgs e)
        {
            Log("Dumping root hints...");
            var rhs = server.GetRootHints();
            foreach (var rh in rhs)
            {
                Log(rh.ToString());
                var rhsrecords = rh.GetRecords();
                foreach (var rhr in rhsrecords)
                    Log(rhr.ToString());
            }

            Log("Dumping root hints...Container");
            //var rhsrecordss = server.GetRecords();//.Where(i => i.ContainerName == "..RootHints").ToList();
            //foreach (var rh in rhsrecordss)
            //    Log(rh.ToString());

            Log("Done dumping root hints");
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            Log("Dumping statistics...");
            var statistics = server.GetStatistics();
            foreach (var stat in statistics)
                Log(stat.ToString());
            Log("Done dumping statistics...");
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            Log("Dumping Services...");
            var servicesmanager = new DNSManagement.ServiceManager("10.0.0.2", "Administrator", "Pass@word1");
            foreach (var service in servicesmanager.List())
            {
                Log(service.ToString());
            }
            Log("Done dumping services...");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnProtoServ_Click(object sender, EventArgs e)
        {
            Log("tcp = " + IPHelper.GetProtoByName("tcp").ToString());
            Log("udp  = " + IPHelper.GetProtoByName("udp").ToString());
            Log("ipv6 = " + IPHelper.GetProtoByName("ipv6").ToString());
            Log("icmp = " + IPHelper.GetProtoByName("icmp").ToString());

            Log("ftp = " + IPHelper.GetServByName("ftp", "tcp").ToString());
            Log("smtp = " + IPHelper.GetServByName("smtp", "tcp").ToString());
            Log("domain = " + IPHelper.GetServByName("domain", "tcp").ToString());
            Log("https = " + IPHelper.GetServByName("https", "tcp").ToString());

        }

        private void btnSigZone_Click(object sender, EventArgs e)
        {
            Log("Dumping zones...");
            var zones = server.GetZones().Where(i => i.Name.Contains("sig"));
            foreach (var zone in zones)
            {
                Log(zone.ToString());
                var records = zone.GetRecords();
                foreach (var r in records)
                    Log(r.ToString());
            }
            Log("done dumping zones");
        }
    }
}
