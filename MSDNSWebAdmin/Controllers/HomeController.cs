/*
DNS Web Admin - MS DNS Web Administration
Copyright (C) 2011 Dror Gluska
	
This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.



Change log:
2011-05-17 - Initial version
2012-03-09  ZoneSave, ZoneDelete, ZoneRRSave
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSDNSWebAdmin.AppCode;
using MSDNSWebAdmin.Models;
using DNSManagement;
using System.Web.Mvc.Html;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Text;
using DNSManagement.RR;

namespace MSDNSWebAdmin.Controllers
{
    /// <summary>
    /// Home/Main DNS Server controller
    /// </summary>
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Allowed service names to be controlled
        /// <para>This should only be DNS, there is no other implementation in the application</para>
        /// </summary>
        private string[] AllowedServiceControlNames = new string[] { "DNS" };


        /// <summary>
        /// Performs all the auditing required
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serverName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private ActionResult CheckAuthorizationAndLog(MSDNSWebAdmin.AppCode.Audit.AuditTypeEnum type, bool log, string serverName,string zoneName, string recordName, string text)
        {
            if ((!string.IsNullOrWhiteSpace(serverName)) && (!Authorization.IsAllowedServer(Session["Domain"] as string, Session["Username"] as string, serverName)))
            {
                Audit.Log(Audit.AuditTypeEnum.UnauthorizedAccess, serverName, (Session["Domain"] as string) + "\\" + (Session["Username"] as string), NetworkHelper.ClientIPAddress(HttpContext), "Attempted to " + text + "for an unauthorized server",zoneName,recordName);
                return new HttpUnauthorizedResult();
            }

            if (log == true)
                Audit.Log(type, serverName, (Session["Domain"] as string) + "\\" + (Session["Username"] as string), NetworkHelper.ClientIPAddress(HttpContext), text,zoneName,recordName);

            return null;
        }

        [AuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizationFilter]
        public ActionResult About()
        {
            return View();
        }

        [AuthorizationFilter]
        public ActionResult DNSServer(string serverName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.View, true, serverName,"","", "View Server Settings");
            if (authres != null)
                return authres;

            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            DNSServerModel model = DNSServerModel.FromServer(msserver);
            return PartialView(model);

        }

        [AuthorizationFilter]
        public ActionResult DNSServerSave(string serverName,
            string ListenAddressesAll, string[] ListenAddresses,
            string[] Forwarders,
            bool IsSlave,
            bool NoRecursion, bool BindSecondaries, bool StrictFileParsing, bool RoundRobin, bool LocalNetPriority, bool SecureResponses,
            DNSManagement.Server.NameCheckFlagEnum NameCheckFlag, DNSManagement.Server.BootMethodEnum BootMethod,
            int ScavengingInterval,
            string[] rootHintServer, string[] rootHintIPs,
            DNSManagement.Server.EventLogLevelEnum EventLogLevel
            )
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.Change, true, serverName,"","", "Save Server Settings");
            if (authres != null)
                return authres;



            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            //Save Interfaces
            if (ListenAddressesAll == "all")
            {
                msserver.ListenAddresses = null;//= msserver.ServerAddresses;
            }
            else if (ListenAddressesAll == "specific")
            {
                msserver.ListenAddresses = ListenAddresses;
            }

            //Save Forwarders
            msserver.Forwarders = Forwarders;

            //Save IsSlave/Root Hints
            msserver.IsSlave = IsSlave;

            //Disable Recursion
            msserver.NoRecursion = NoRecursion;

            //Bind Secondaries
            msserver.BindSecondaries = BindSecondaries;

            //Fail On Load if Bad zone data / Strict File Parsing
            msserver.StrictFileParsing = StrictFileParsing;

            //Enable Round Robin
            msserver.RoundRobin = RoundRobin;

            //Enable Netmask Ordering / Local Net Priority
            msserver.LocalNetPriority = LocalNetPriority;

            //Secure Cache against pollution / Secure Responses
            msserver.SecureResponses = SecureResponses;

            //Name Checking
            msserver.NameCheckFlag = NameCheckFlag;

            //Load Zone Data on startup / Boot Method
            msserver.BootMethod = BootMethod;

            //Scavenging Interval (HOURS)
            msserver.ScavengingInterval = TimeSpan.FromHours(ScavengingInterval);

            #region incomplete Root Hints servers save

            //try
            //{

            //List<ResolvedServer> roothintslist = new List<ResolvedServer>();
            //var rhints = msserver.GetRootHints();
            //var records = new List<DNSManagement.RR.ResourceRecord>();

            //foreach (var rh in rhints)
            //    records.AddRange(rh.GetRecords());

            ////save root hints
            //for (var i = 0; i < rootHintServer.Length;i++)
            //{
            //    if (string.IsNullOrEmpty(rootHintServer[i]))
            //        continue;

            //    var existing = (DNSManagement.RR.NSType)records.FirstOrDefault(r => r.RecordType == typeof(DNSManagement.RR.NSType) && r.OwnerName == "..RootHints" && r.ContainerName == "..RootHints" && ((DNSManagement.RR.NSType)r).NSHost == rootHintServer[i]);
            //    if (existing == null)
            //    {
            //        //CHECK !!! 
            //        existing = DNSManagement.RR.NSType.CreateInstanceFromPropertyData(msserver, serverName,"..RootHints" , "..RootHints", DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, rootHintServer[i]);
            //    }

            //    var existingaddresses = records.Where(e => e.OwnerName == existing.NSHost).ToList();

            //    var rootHintsForServer = rootHintIPs[i].Split(',').Select(rhp => rhp.Trim()).ToArray();
            //    for (var ia = 0; ia < rootHintsForServer.Length; ia++)
            //    {
            //        if (string.IsNullOrEmpty(rootHintsForServer[ia]))
            //            continue;

            //        var addressrecord = existingaddresses.FirstOrDefault(ead => (ead.RecordType == typeof(DNSManagement.RR.AType) && ((DNSManagement.RR.AType)ead).IPAddress == rootHintsForServer[ia]) ||
            //                                                                    (ead.RecordType == typeof(DNSManagement.RR.AAAAType) && ((DNSManagement.RR.AAAAType)ead).IPv6Address == rootHintsForServer[ia]));
            //        if (addressrecord == null)
            //        {
            //            var address = DNSManagementHelper.ParseIP(rootHintIPs[ia]);
            //            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //            {
            //                //create IPv4
            //                addressrecord = DNSManagement.RR.AType.CreateInstanceFromPropertyData(msserver, serverName, existing.NSHost, existing.NSHost, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, rootHintsForServer[ia]);
            //            }
            //            else if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            //            {
            //                //create IPv6
            //                addressrecord = DNSManagement.RR.AAAAType.CreateInstanceFromPropertyData(msserver, serverName, existing.NSHost, existing.NSHost, DNSManagement.RR.ResourceRecord.RecordClassEnum.IN, null, rootHintsForServer[ia]);
            //            }
            //        }
            //    }
            //    for (var ia = 0; ia < existingaddresses.Count; ia++)
            //    {
            //        if (!(existingaddresses[ia].RecordType == typeof(DNSManagement.RR.AType)
            //               && rootHintsForServer.Contains(((DNSManagement.RR.AType)existingaddresses[ia]).IPAddress))
            //            &&
            //            !(existingaddresses[ia].RecordType == typeof(DNSManagement.RR.AAAAType)
            //               && rootHintsForServer.Contains(((DNSManagement.RR.AAAAType)existingaddresses[ia]).IPv6Address))
            //            )
            //        {
            //            existingaddresses[ia].Delete();
            //        }
            //    }
            //}

            ////remove all non-existing roothints
            //var allroothints = records.Where(r => r.RecordType == typeof(DNSManagement.RR.NSType) && r.ContainerName == "..RootHints" && r.DomainName == "..RootHints" && r.OwnerName == "..RootHints").Select(r => (DNSManagement.RR.NSType)r).ToArray();
            //for (var arh = 0; arh < allroothints.Length;arh++)
            //{
            //    if (!rootHintServer.Contains(allroothints[arh].NSHost))
            //    {
            //        //delete their children first
            //        var children = records.Where(e => e.OwnerName == allroothints[arh].NSHost).ToList();
            //        for (var dc = 0; dc < children.Count(); dc++)
            //        {
            //            children[dc].Delete();
            //        }

            //        try
            //        {
            //            allroothints[arh].Delete();
            //        }
            //        catch { }
            //    }
            //}

            //foreach (var rh in rhints)
            //    rh.WriteBackRootHintDatafile(msserver);

            #endregion


            msserver.EventLogLevel = EventLogLevel;

            try
            {
                msserver.Save();
            }
            catch (System.Management.ManagementException me)
            {
                throw new Exception(me.ErrorInformation.GetText(System.Management.TextFormat.Mof), me);
            }

            return Content("true");
        }

        [AuthorizationFilter]
        public ActionResult Tree(string id, string serverName, string zoneName, string search_string)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.View, false, serverName, zoneName, "", "Show server in tree");
            if (authres != null)
                return authres;



            Response.Expires = -1;

            //search 
            if (!string.IsNullOrEmpty(search_string))
            {
                List<string> searchids = new List<string>();
                using (DNSAdminEntities db = new DNSAdminEntities())
                {
                    string user = Session["Domain"] + "\\" + Session["Username"];
                    var searchservers = db.DNSServers.Where(i => i.Username == user).ToList();
                    foreach (var server in searchservers)
                    {
                       
                        DNSManagement.Server msserver = new DNSManagement.Server(server.ServerName, Session["Username"] as string, Session["Password"] as string);
                        var zones = msserver.GetZones();
                        foreach (var zone in zones)
                        {
                            var resources = zone.GetRecords();

                            var resexist = resources.Where(i => i.OwnerName.ToLower().Contains(search_string.ToLower())).Any();



                            if ((zone.Name.ToLower().Contains(search_string.ToLower())) || (resexist))
                            {
                                searchids.Add("#Server_" + server.ServerName);
                                if (zone.Reverse)
                                {
                                    searchids.Add("#ReverseLookupZones_" + server.ServerName);
                                }
                                else if (zone.ZoneType == DNSManagement.Zone.ZoneTypeEnum.Forwarder)
                                {
                                    searchids.Add("#ConditionalForwarders_" + server.ServerName);
                                }
                                else
                                {
                                    searchids.Add("#ForwardLookupZones_" + server.ServerName);
                                }

                            }

                            if (resexist)
                            {
                                searchids.Add("#Zone_" + zone.DnsServerName + "_" + zone.Name);
                            }
                        }

                    }
                }

                return Json(searchids.ToArray(), JsonRequestBehavior.AllowGet);
            }



            var splits = id.Split('_');

            var key = (splits.Length > 0) ? splits[0] : "";

            //var value = (splits.Length > 1) ? splits[1] : "";
            //var value2 = (splits.Length > 2) ? splits[2] : "";

            


            switch (key)
            {
                case "DNSServers":
                    List<DNSServer> servers = null;
                    using (DNSAdminEntities db = new DNSAdminEntities())
                    {
                        string user = Session["Domain"] + "\\" + Session["Username"];
                        servers = db.DNSServers.Where(i => i.Username == user).ToList();
                    }
                    List<JsTreeNode> jsServers = new List<JsTreeNode>();
                    foreach (var server in servers)
                    {
                        jsServers.Add(new JsTreeNodeChildren
                        {
                            children = new List<JsTreeNode>(),
                            attr = new Attributes
                            {
                                id = "Server_" + server.ServerName,
                                serverName = server.ServerName,
                                href = "/Home/DNSServer?serverName=" + server.ServerName
                            },
                            data = new Data
                            {
                                title = server.ServerName,
                                icon = "/Content/icons/server.png"
                            },
                            state = "closed"
                        });
                    }
                    jsServers.Add(new JsTreeNode
                    {
                        attr = new Attributes
                        {
                            id = "AuditLogs",
                            href = "/Home/AuditLog"
                        },
                        data = new Data
                        {
                            title = "Audit Logs",
                            icon = "/Content/icons/log.png"
                        },
                    });
                    return Json(jsServers, JsonRequestBehavior.AllowGet);
                case "Server":
                    List<JsTreeNode> jsServerAreas = new List<JsTreeNode>();
                    jsServerAreas.Add(new JsTreeNode
                    {
                        attr = new Attributes
                        {
                            id = "Logs_" + serverName,
                            serverName = serverName,
                            href = "/Home/Logs?serverName=" + serverName
                        },
                        data = new Data
                        {
                            title = "Logs",
                            icon = "/Content/icons/log.png"
                        },
                    });
                    jsServerAreas.Add(new JsTreeNodeChildren
                    {
                        children = new List<JsTreeNode>(),
                        attr = new Attributes
                        {
                            id = "ForwardLookupZones_" + serverName,
                            serverName = serverName,
                            href = "/Home/ForwardLookupZones?serverName=" + serverName
                        },
                        data = new Data
                        {
                            title = "Forward Lookup Zones",
                            icon = "/Content/icons/folder.png"
                        },
                        state = "closed"
                    });
                    jsServerAreas.Add(new JsTreeNodeChildren
                    {
                        children = new List<JsTreeNode>(),
                        attr = new Attributes
                        {
                            id = "ReverseLookupZones_" + serverName,
                            serverName = serverName,
                            href = "/Home/ReverseLookupZones?serverName=" + serverName
                        },
                        data = new Data
                        {
                            title = "Reverse Lookup Zones",
                            icon = "/Content/icons/folder.png"
                        },
                        state = "closed"
                    });
                    jsServerAreas.Add(new JsTreeNodeChildren
                    {
                        children = new List<JsTreeNode>(),
                        attr = new Attributes
                        {
                            id = "ConditionalForwarders_" + serverName,
                            serverName = serverName,
                            href = "/Home/ConditionalForwarders?serverName=" + serverName
                        },
                        data = new Data
                        {
                            title = "Conditional Forwarders",
                            icon = "/Content/icons/folder.png"
                        },
                        state = "closed"
                    });
                    jsServerAreas.Add(new JsTreeNode
                    {
                        attr = new Attributes
                        {
                            id = "nslookup_" + serverName,
                            serverName = serverName,
                            href = "/Home/nslookup?serverName=" + serverName
                        },
                        data = new Data
                        {
                            title = "nslookup",
                            icon = "/Content/icons/kfind.png"
                        },
                    });
                    return Json(jsServerAreas, JsonRequestBehavior.AllowGet);
                //break;
                case "ForwardLookupZones":
                    List<JsTreeNode> jsServerForwardZones = new List<JsTreeNode>();

                    jsServerForwardZones.AddRange(GetForwardLookupZones(serverName).Select(i => new JsTreeNodeChildren
                    {
                        children = new List<JsTreeNode>(),
                        attr = new Attributes
                        {
                            id = "Zone_" + i.DnsServerName + "_" + i.Name,
                            serverName = serverName,
                            zoneName = i.Name,
                            href = "/Home/Zone?serverName=" + serverName + "&zoneName=" + i.Name
                        },
                        data = new Data
                        {
                            title = i.Name,
                            icon = "/Content/icons/folder.png"
                        },
                        state = "closed"
                    }));

                    return Json(jsServerForwardZones, JsonRequestBehavior.AllowGet);
                //break;
                case "ReverseLookupZones":
                    List<JsTreeNode> jsServerReverseZones = new List<JsTreeNode>();

                    jsServerReverseZones.AddRange(GetReverseLookupZones(serverName).Select(i => new JsTreeNodeChildren
                    {
                        children = new List<JsTreeNode>(),
                        attr = new Attributes
                        {
                            id = "Zone_" + i.DnsServerName + "_" + i.Name,
                            serverName = serverName,
                            zoneName = i.Name,
                            href = "/Home/Zone?serverName=" + serverName + "&zoneName=" + i.Name
                        },
                        data = new Data
                        {
                            title = i.Name,
                            icon = "/Content/icons/folder.png"
                        },
                        state = "closed"
                    }));

                    return Json(jsServerReverseZones, JsonRequestBehavior.AllowGet);
                //break;
                case "ConditionalForwarders":
                    List<JsTreeNode> jsServerConditionalForwardersZones = new List<JsTreeNode>();

                    jsServerConditionalForwardersZones.AddRange(GetConditionalForwardersZones(serverName).Select(i => new JsTreeNodeChildren
                    {
                        children = new List<JsTreeNode>(),
                        attr = new Attributes
                        {
                            id = "Zone_" + i.DnsServerName + "_" + i.Name,
                            serverName = serverName,
                            zoneName = i.Name,
                            href = "/Home/Zone?serverName=" + serverName + "&zoneName=" + i.Name
                        },
                        data = new Data
                        {
                            title = i.Name,
                            icon = "/Content/icons/folder.png"
                        },
                        state = "closed"
                    }));

                    return Json(jsServerConditionalForwardersZones, JsonRequestBehavior.AllowGet);
                //break;
                case "Zone":
                    List<JsTreeNode> jsZoneRRs = new List<JsTreeNode>();

                    DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

                    var zoneRRs = msserver.GetZones().FirstOrDefault(i => i.Name == zoneName).GetRecords();

                    jsZoneRRs.AddRange(zoneRRs.Select(i => new JsTreeNodeChildren
                            {
                                children = new List<JsTreeNode>(),
                                attr = new Attributes
                                {
                                    id = "ZoneRR_" + i.DnsServerName + "_" + i.ContainerName + "_" + HttpUtility.UrlEncode(i.TextRepresentation).Replace("%","_"),
                                    serverName = serverName,
                                    zoneName = zoneName,
                                    href = "/Home/ZoneRR?serverName=" + serverName + "&zoneName=" + zoneName + "&zoneTR=" + HttpUtility.UrlEncode(i.TextRepresentation)
                                },
                                data = new Data
                                {
                                    title = i.OwnerName,
                                    icon = "/Content/icons/folder.png"
                                },
                                state = "closed"
                            }));

                    return Json(jsZoneRRs, JsonRequestBehavior.AllowGet);
                //break;

            }
            return Content("Error");
        }

        [AuthorizationFilter]
        public ActionResult Servers()
        {
            List<DNSServer> servers = null;
            using (DNSAdminEntities db = new DNSAdminEntities())
            {
                string user = Session["Domain"] + "\\" + Session["Username"];
                servers = db.DNSServers.Where(i => i.Username == user).ToList();
            }

            ViewData["servers"] = servers;

            return PartialView();
        }



        [AuthorizationFilter]
        public ActionResult ServersStatus(string serverName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.View, false, serverName, "Get server service status","","");
            if (authres != null)
                return authres;


            ServiceManager sm = new ServiceManager(serverName, Session["Username"] as string, Session["Password"] as string);
            var services = sm.List();

            var service = services.FirstOrDefault(i => i.Name == "DNS");
            if (service == null)
                return Content("Not installed");
            return Content(service.State.ToString());

        }

        [AuthorizationFilter]
        public ActionResult ServersSave(ServersModel model)
        {


            Audit.Log(Audit.AuditTypeEnum.Change, model.Name, (Session["Domain"] as string) + "\\" + (Session["Username"] as string), NetworkHelper.ClientIPAddress(HttpContext), "Saved new server to list","","");



            using (DNSAdminEntities db = new DNSAdminEntities())
            {
                string user = Session["Domain"] + "\\" + Session["Username"];
                DNSServer server = new DNSServer();
                server.Id = Guid.NewGuid();
                server.Username = user;
                server.ServerName = model.Name;
                db.DNSServers.AddObject(server);
                db.SaveChanges();
            }

            return Content(true.ToString());
        }

        [AuthorizationFilter]
        public ActionResult ServersDelete(string serverName)
        {
            Audit.Log(Audit.AuditTypeEnum.Change, serverName, (Session["Domain"] as string) + "\\" + (Session["Username"] as string), NetworkHelper.ClientIPAddress(HttpContext), "Deleted server from list","","");



            using (DNSAdminEntities db = new DNSAdminEntities())
            {
                string user = Session["Domain"] + "\\" + Session["Username"];
                var servers = db.DNSServers.Where(i => i.Username == user && i.ServerName == serverName).ToList();
                foreach (var server in servers)
                    db.DNSServers.DeleteObject(server);

                db.SaveChanges();
            }

            return Content(true.ToString());
        }

        /// <summary>
        /// Service Control action, stop/start/resume/pause/restart
        /// </summary>
        /// <param name="serverName">Server Name</param>
        /// <param name="serviceName">Service Name</param>
        /// <param name="action">action, e.g. stop/start/resume/pause/restart</param>
        /// <returns></returns>
        [AuthorizationFilter]
        public ActionResult ServerServiceAction(string serverName, string serviceName, string action)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.Change, true, serverName,"","", "change service state");
            if (authres != null)
                return authres;


            //Check service name is in allowed list
            if (!AllowedServiceControlNames.Contains(serviceName))
            {
                Audit.Log(Audit.AuditTypeEnum.UnauthorizedAccess, serverName, (Session["Domain"] as string) + "\\" + (Session["Username"] as string), NetworkHelper.ClientIPAddress(HttpContext), "Attempted to change service state for an unauthorized service","","");
                return new HttpUnauthorizedResult();
            }

            ServiceManager sm = new ServiceManager(serverName, Session["Username"] as string, Session["Password"] as string);
            var service = sm.List().Where(i => i.Name == serviceName).FirstOrDefault();
            if (service != null)
            {
                switch (action.ToLower())
                {
                    case "stop": service.StopService();
                        break;
                    case "start": service.StartService();
                        break;
                    case "pause": service.PauseService();
                        break;
                    case "resume": service.ResumeService();
                        break;
                    case "restart":
                        service.StopService();
                        service.StartService();
                        break;
                }
            }


            return Content(true.ToString());
        }

        [AuthorizationFilter]
        public ActionResult ServerBackup(string serverName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.View, true, serverName,"","", "backup server");
            if (authres != null)
                return authres;

            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);
            BackupModel bm = new BackupModel();
            bm.PopulateFromServer(msserver);
            //var res = bm.ToXml();
            var files = bm.ToConfigurationFiles();

            MemoryStream backupFile = new MemoryStream();
            ZipOutputStream zipBackup = new ZipOutputStream(backupFile);
            foreach (var file in files)
            {
                var data = System.Text.Encoding.UTF8.GetBytes(file.Value);
                if ((data.Length == 0) || (string.IsNullOrEmpty(file.Key)))
                    continue;

                ZipEntry entry = new ZipEntry(ZipEntry.CleanName(file.Key));
                entry.DateTime = DateTime.Now;
                entry.Size = data.Length;
                zipBackup.PutNextEntry(entry);
                zipBackup.Write(data, 0, data.Length);
                zipBackup.CloseEntry();
            }
            zipBackup.Flush();
            zipBackup.IsStreamOwner = false;
            zipBackup.Close();


            Response.ContentType = "application/zip";
            Response.AppendHeader("content-disposition", string.Format("attachment; filename=\"Backup_{0}_{1}.zip\"", serverName, DateTime.Now.ToString("yyyyMMddHHmmss")));
            Response.CacheControl = "Private";
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(3)); // or put a timestamp in the filename in the content-disposition
            Response.OutputStream.Write(backupFile.ToArray(), 0, (int)backupFile.Length);
            Response.Flush();



            return new EmptyResult();

            //backup all settings to xml
            //backup all zones to bind files (?)
        }

        [AuthorizationFilter]
        public ActionResult ServerRestore(string serverName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.Change, true, serverName,"","", "restore server");
            if (authres != null)
                return authres;
            //get file from post
            //open and restore all settings from xml
            //restore all zones from bind files (?)
            return Content(true.ToString());
        }

        [AuthorizationFilter]
        public ActionResult ClearCache(string serverName)
        {
            throw new NotImplementedException();
        }


        [AuthorizationFilter]
        public ActionResult Zone(string serverName, string zoneName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.View, true, serverName,zoneName,"", "view zone");
            if (authres != null)
                return authres;

            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            var zone = msserver.GetZones().FirstOrDefault(i => i.Name == zoneName);
            return View(zone);
        }

        [AuthorizationFilter]
        public ActionResult ZonePause(string serverName, string zoneName, bool pause)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.Change, true, serverName,zoneName,"", "pause/resume zone");
            if (authres != null)
                return authres;

            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            var zone = msserver.GetZones().FirstOrDefault(i => i.Name == zoneName);
            zone.PauseZone();

            zone = msserver.GetZones().FirstOrDefault(i => i.Name == zoneName);

            return Content(zone.Paused.ToString());
        }

        //[AuthorizationFilter]
        //public ActionResult records()
        //{
        //    return View();
        //}

        /// <summary>
        /// Shows logs for server
        /// </summary>
        /// <param name="serverName">server name</param>
        /// <returns></returns>
        [AuthorizationFilter]
        public ActionResult Logs(string serverName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.ViewEventLog, true, serverName,"","", "view log");
            if (authres != null)
                return authres;

            var el = new EventLogging(serverName, Session["Username"] as string, Session["Password"] as string);
            {
                var start = 0;
                var length = (el.Count("DNS Server") < 100) ? el.Count("DNS Server") : 100;
                ViewData["start"] = start;
                ViewData["length"] = length;


                var entries = el.Get("DNS Server", start, length);

                ViewData["entries"] = entries;
            }



            return View();
        }

        [AuthorizationFilter]
        public ActionResult ZoneDelete(string serverName, string zoneName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.Change, true, serverName,zoneName,"", "delete zone");
            if (authres != null)
                return authres;

            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            var zones = msserver.GetZones();
            var zone = zones.FirstOrDefault(i => i.Name == zoneName);
            zone.Delete();

            return Content(true.ToString());
        }


        [AuthorizationFilter]
        public ActionResult ZoneSave(string serverName, string ZoneName, DNSManagement.Zone.ZoneTypeCreate ZoneTypeCreate, bool StoreZoneInAD, string[] MasterDNSServer
            , string ZoneFile, string ZoneFileNameNew, string ZoneFileNameExisting, DNSManagement.Zone.ZoneAllowUpdateEnum ZoneAllowUpdateEnum, string OptionalAdminEmail)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.Change, true, serverName,ZoneName,"", "save zone");
            if (authres != null)
                return authres;


            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            string zonefilename = null;
            //if (ZoneFile == "New")
            //    zonefilename = ZoneFileNameNew;
            //else 
            if (ZoneFile == "Existing")
                zonefilename = ZoneFileNameExisting;

            //if (string.IsNullOrWhiteSpace(zonefilename))
            //    zonefilename = ZoneName + ".dns";

            //var firstMaster = ((MasterDNSServer != null) && (MasterDNSServer.Length > 0)) ? MasterDNSServer.FirstOrDefault() : string.Empty;

            try
            {
                var zone = DNSManagement.Zone.CreateZone(msserver, ZoneName, ZoneTypeCreate, StoreZoneInAD, zonefilename, MasterDNSServer, OptionalAdminEmail);
                //zone.MasterServers = MasterDNSServer;
                zone.AllowUpdate = ZoneAllowUpdateEnum;
                zone.Save();

            }
            catch (System.Management.ManagementException me)
            {
                throw new Exception(me.ErrorInformation.GetText(System.Management.TextFormat.Mof), me);
            }


            return Content(true.ToString());

        }



        ///Home/ZoneRR?serverName=" + serverName + "&zoneName=" + zoneName + "&zoneTR=" + i.TextRepresentation
        [AuthorizationFilter]
        public ActionResult ZoneRR(string serverName, string zoneName, string zoneTR, ResourceRecord.ResourceRecordEnum? RecordType)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.View, true, serverName,zoneName,zoneTR, "view zone records");
            if (authres != null)
                return authres;

            //Handle new record
            if (RecordType != null)
            {
                return View(new ResourceRecordModel() { ResourceRecordType = RecordType.Value, ContainerName = zoneName, DnsServerName  = serverName});
            }

            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            var zone = msserver.GetZones().FirstOrDefault(i => i.Name == zoneName);
            var record = zone.GetRecords().FirstOrDefault(i => i.TextRepresentation == zoneTR);

            return View(new ResourceRecordModel( record));
        }

        [AuthorizationFilter]
        public ActionResult ZoneRRSave(string serverName, string zoneName, string TextRepresentation, string recordName,
            ResourceRecord.ResourceRecordEnum Type,
            DNSManagement.RR.ResourceRecord.RecordClassEnum RecordClass, TimeSpan TTL,
            string AAAA_IPv6Address,
            string AFSDB_ServerName, DNSManagement.RR.AFSDBType.SubtypeEnum? AFSDB_Subtype,
            //string ATMA_ATMAddress, DNSManagement.RR.ATMAType.AddressFormatEnum? ATMA_Format,
            string A_IPAddress,
            string CNAME_PrimaryName,
            string HINFO_CPU, string HINFO_OS,
            string ISDN_ISDNNumber, string ISDN_SubAddress,
            //DNSManagement.RR.KEYType.AlgorithmEnum? KEY_Algorithm, ushort? KEY_Flags, DNSManagement.RR.KEYType.ProtocolEnum? KEY_Protocol, string KEY_PublicKey,
            string MB_MBHost,
            string MD_MDHost,
            string MF_MFHost,
            string MG_MGMailbox,
            string MINFO_ErrorMailbox, string MINFO_ResponsibleMailbox,
            string MR_MRMailbox,
            ushort? MX_Preference, string MX_MailExchange,
            string NS_NSHost,
            //string NXT_NextDomainName, string NXT_Types,
            string PTR_PTRDomainName,
            string RP_RPMailbox, string RP_TXTDomainName,
            string RT_IntermediateHost, ushort? RT_Preference,
            DNSManagement.RR.SIGType.AlgorithmEnum? SIG_Algorithm, ushort? SIG_KeyTag, ushort? SIG_Labels, uint? SIG_OriginalTTL, string SIG_Signature, uint? SIG_SignatureExpiration,
            uint? SIG_SignatureInception, string SIG_SignerName, ushort? SIG_TypeCovered,
            TimeSpan? SOA_ExpireLimit, TimeSpan? SOA_MinimumTTL, string SOA_PrimaryServer, TimeSpan? SOA_RefreshInterval, string SOA_ResponsibleParty, TimeSpan? SOA_RetryDelay, uint? SOA_SerialNumber,
            ushort? SRV_Priority, ushort? SRV_Weight, ushort? SRV_Port, string SRV_SRVDomainName,
            string TXT_DescriptiveText,
            TimeSpan? WINSR_CacheTimeout, TimeSpan? WINSR_LookupTimeout, DNSManagement.RR.WINSRType.MappingFlagEnum? WINSR_MappingFlag, string WINSR_ResultDomain,
            TimeSpan? WINS_CacheTimeout, TimeSpan? WINS_LookupTimeout, DNSManagement.RR.WINSType.MappingFlagEnum? WINS_MappingFlag, string WINS_WinsServers,
            string WKS_InternetAddress, string WKS_IPProtocol, string WKS_Services,
            string X25_PSDNAddress
            )
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.Change, true, serverName, zoneName,recordName, "save zone records");
            if (authres != null)
                return authres;


            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            //get zone
            var zone = msserver.GetZones().FirstOrDefault(i => i.Name == zoneName);
            //get records
            //locate the right one, otherwise, create new
            var record = zone.GetRecords().FirstOrDefault(i => i.TextRepresentation == TextRepresentation);

            ResourceRecord newrecord = null;

            //check and add zone name to record name
            var checkedRecordName = recordName.Trim();
            if (!checkedRecordName.EndsWith(zoneName))
            {
                if (!checkedRecordName.EndsWith("."))
                    checkedRecordName = checkedRecordName + ".";
                checkedRecordName = checkedRecordName + zoneName;
            }

            try
            {

                switch (Type)
                {
                    case ResourceRecord.ResourceRecordEnum.AAAA:
                        if (record == null)
                            newrecord = AAAAType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, AAAA_IPv6Address);
                        else
                            newrecord = ((AAAAType)record).Modify(TTL, AAAA_IPv6Address);
                        break;
                    case ResourceRecord.ResourceRecordEnum.AFSDB:
                        if (record == null)
                            newrecord = AFSDBType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, AFSDB_Subtype.Value, AFSDB_ServerName);
                        else
                            newrecord = ((AFSDBType)record).Modify(TTL, AFSDB_Subtype, AFSDB_ServerName);
                        break;
                    //case ResourceRecord.ResourceRecordEnum.ATMA:
                    //    if (record == null)
                    //        newrecord = ATMAType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, recordName, RecordClass, TTL, ATMA_Format.Value, ATMA_ATMAddress);
                    //    else
                    //        newrecord = ((ATMAType)record).Modify(TTL, ATMA_Format, ATMA_ATMAddress);
                    //    break;
                    case ResourceRecord.ResourceRecordEnum.A:
                        if (record == null)
                            newrecord = AType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, A_IPAddress);
                        else
                            newrecord = ((AType)record).Modify(TTL, A_IPAddress);
                        break;
                    case ResourceRecord.ResourceRecordEnum.CNAME:
                        if (record == null)
                            newrecord = CNAMEType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, CNAME_PrimaryName);
                        else
                            newrecord = ((CNAMEType)record).Modify(TTL, CNAME_PrimaryName);
                        break;
                    case ResourceRecord.ResourceRecordEnum.HINFO:
                        if (record == null)
                            newrecord = HINFOType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, HINFO_CPU, HINFO_OS);
                        else
                            newrecord = ((HINFOType)record).Modify(TTL, HINFO_CPU, HINFO_OS);
                        break;
                    case ResourceRecord.ResourceRecordEnum.ISDN:
                        if (record == null)
                            newrecord = ISDNType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, ISDN_ISDNNumber, ISDN_SubAddress);
                        else
                            newrecord = ((ISDNType)record).Modify(TTL, ISDN_ISDNNumber, ISDN_SubAddress);
                        break;
                    //case ResourceRecord.ResourceRecordEnum.KEY:
                    //    if (record == null)
                    //        newrecord = KEYType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, KEY_Flags.Value, KEY_Protocol.Value, KEY_Algorithm.Value, KEY_PublicKey);
                    //    else
                    //        newrecord = ((KEYType)record).Modify(TTL, KEY_Flags, KEY_Protocol, KEY_Algorithm, KEY_PublicKey);
                    //    break;
                    case ResourceRecord.ResourceRecordEnum.MB:
                        if (record == null)
                            newrecord = MBType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, MB_MBHost);
                        else
                            newrecord = ((MBType)record).Modify(TTL, MB_MBHost);
                        break;
                    case ResourceRecord.ResourceRecordEnum.MD:
                        if (record == null)
                            newrecord = MDType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, MD_MDHost);
                        else
                            newrecord = ((MDType)record).Modify(TTL, MD_MDHost);
                        break;
                    case ResourceRecord.ResourceRecordEnum.MF:
                        if (record == null)
                            newrecord = MFType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, MF_MFHost);
                        else
                            newrecord = ((MFType)record).Modify(TTL, MF_MFHost);
                        break;
                    case ResourceRecord.ResourceRecordEnum.MG:
                        if (record == null)
                            newrecord = MGType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, MG_MGMailbox);
                        else
                            newrecord = ((MGType)record).Modify(TTL, MG_MGMailbox);
                        break;
                    case ResourceRecord.ResourceRecordEnum.MINFO:
                        if (record == null)
                            newrecord = MINFOType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, MINFO_ResponsibleMailbox, MINFO_ErrorMailbox);
                        else
                            newrecord = ((MINFOType)record).Modify(TTL, MINFO_ResponsibleMailbox, MINFO_ErrorMailbox);
                        break;
                    case ResourceRecord.ResourceRecordEnum.MR:
                        if (record == null)
                            newrecord = MRType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, MR_MRMailbox);
                        else
                            newrecord = ((MRType)record).Modify(TTL, MR_MRMailbox);
                        break;
                    case ResourceRecord.ResourceRecordEnum.MX:
                        if (record == null)
                            newrecord = MXType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, MX_Preference.Value, MX_MailExchange);
                        else
                            newrecord = ((MXType)record).Modify(TTL, MX_Preference, MX_MailExchange);
                        break;
                    case ResourceRecord.ResourceRecordEnum.NS:
                        if (record == null)
                            newrecord = NSType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, NS_NSHost);
                        else
                            newrecord = ((NSType)record).Modify(TTL, NS_NSHost);
                        break;
                    //case ResourceRecord.ResourceRecordEnum.NXT:
                    //    if (record == null)
                    //        newrecord = NXTType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, NXT_NextDomainName, NXT_Types);
                    //    else
                    //        newrecord = ((NXTType)record).Modify(TTL, NXT_NextDomainName, NXT_Types);
                    //    break;
                    case ResourceRecord.ResourceRecordEnum.PTR:
                        if (record == null)
                            newrecord = PTRType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, PTR_PTRDomainName);
                        else
                            newrecord = ((PTRType)record).Modify(TTL, PTR_PTRDomainName);
                        break;
                    case ResourceRecord.ResourceRecordEnum.RP:
                        if (record == null)
                            newrecord = RPType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, RP_RPMailbox, RP_TXTDomainName);
                        else
                            newrecord = ((RPType)record).Modify(TTL, RP_RPMailbox, RP_TXTDomainName);
                        break;
                    case ResourceRecord.ResourceRecordEnum.RT:
                        if (record == null)
                            newrecord = RTType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, RT_IntermediateHost, RT_Preference.Value);
                        else
                            newrecord = ((RTType)record).Modify(TTL, RT_IntermediateHost, RT_Preference);
                        break;
                    //case ResourceRecord.ResourceRecordEnum.SIG:
                    //    if (record == null)
                    //        newrecord = SIGType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, SIG_TypeCovered.Value, SIG_Algorithm.Value, SIG_Labels.Value, SIG_OriginalTTL.Value, SIG_SignatureExpiration.Value, SIG_SignatureInception.Value, SIG_KeyTag.Value, SIG_SignerName, SIG_Signature);
                    //    else
                    //        newrecord = ((SIGType)record).Modify(TTL, SIG_TypeCovered.Value, SIG_Algorithm.Value, SIG_Labels.Value, SIG_OriginalTTL.Value, SIG_SignatureExpiration.Value, SIG_SignatureInception.Value, SIG_KeyTag.Value, SIG_SignerName, SIG_Signature);
                    //    break;
                    case ResourceRecord.ResourceRecordEnum.SOA:
                        if (record == null)
                            throw new Exception("Logic error, SOA can't be created...");
                        else
                            newrecord = ((SOAType)record).Modify(TTL, SOA_SerialNumber, SOA_PrimaryServer, SOA_ResponsibleParty, SOA_RefreshInterval, SOA_RetryDelay, SOA_ExpireLimit, SOA_MinimumTTL);
                        break;
                    case ResourceRecord.ResourceRecordEnum.SRV:
                        if (record == null)
                            newrecord = SRVType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, SRV_Priority.Value, SRV_Weight.Value, SRV_Port.Value, SRV_SRVDomainName);
                        else
                            newrecord = ((SRVType)record).Modify(TTL, SRV_Priority, SRV_Weight, SRV_Port, SRV_SRVDomainName);
                        break;
                    case ResourceRecord.ResourceRecordEnum.TXT:
                        if (record == null)
                            newrecord = TXTType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, TXT_DescriptiveText);
                        else
                            newrecord = ((TXTType)record).Modify(TTL, TXT_DescriptiveText);
                        break;
                    case ResourceRecord.ResourceRecordEnum.WINSR:
                        if (record == null)
                            newrecord = WINSRType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, WINSR_MappingFlag.Value, WINSR_LookupTimeout.Value, WINSR_CacheTimeout.Value, WINSR_ResultDomain);
                        else
                            newrecord = ((WINSRType)record).Modify(TTL, WINSR_MappingFlag, WINSR_LookupTimeout, WINSR_CacheTimeout, WINSR_ResultDomain);
                        break;
                    case ResourceRecord.ResourceRecordEnum.WINS:
                        if (record == null)
                            newrecord = WINSType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, WINS_MappingFlag.Value, WINS_LookupTimeout.Value, WINS_CacheTimeout.Value, WINS_WinsServers);
                        else
                            newrecord = ((WINSType)record).Modify(TTL, WINS_MappingFlag, WINS_LookupTimeout, WINS_CacheTimeout, WINS_WinsServers);
                        break;
                    case ResourceRecord.ResourceRecordEnum.WKS:
                        if (record == null)
                            newrecord = WKSType.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, WKS_InternetAddress, WKS_IPProtocol, WKS_Services);
                        else
                            newrecord = ((WKSType)record).Modify(TTL, WKS_InternetAddress, WKS_IPProtocol, WKS_Services);
                        break;
                    case ResourceRecord.ResourceRecordEnum.X25:
                        if (record == null)
                            newrecord = X25Type.CreateInstanceFromPropertyData(msserver, msserver.Name, zoneName, checkedRecordName, RecordClass, TTL, X25_PSDNAddress);
                        else
                            newrecord = ((X25Type)record).Modify(TTL, X25_PSDNAddress);
                        break;
                    default:
                        throw new NotImplementedException("No implementation for record type " + record.RecordTypeText);
                }
            }catch (System.Management.ManagementException me)
            {
                throw new Exception(me.ErrorInformation.GetText(System.Management.TextFormat.Mof), me);
            }
            return Json(new { result = true.ToString(), serverName = serverName, zoneName = zoneName, record = HttpUtility.UrlEncode(newrecord.TextRepresentation) });

            //return Content(true.ToString());
        }


        private Zone[] GetForwardLookupZones(string serverName)
        {
            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            var zones = msserver.GetZones().Where(i => i.Reverse == false && i.ZoneType != DNSManagement.Zone.ZoneTypeEnum.Forwarder && i.Name != "TrustAnchors").ToArray();

            return zones;
        }

        private Zone[] GetConditionalForwardersZones(string serverName)
        {
            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            var zones = msserver.GetZones().Where(i => i.ZoneType == DNSManagement.Zone.ZoneTypeEnum.Forwarder).ToArray();

            return zones;
        }

        private Zone[] GetReverseLookupZones(string serverName)
        {
            DNSManagement.Server msserver = new DNSManagement.Server(serverName, Session["Username"] as string, Session["Password"] as string);

            var zones = msserver.GetZones().Where(i => i.Reverse == true).ToArray();
            return zones;
        }

        [AuthorizationFilter]
        public ActionResult ForwardLookupZones(string serverName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.View, true, serverName,"","", "view forward lookup zones");
            if (authres != null)
                return authres;


            var zones = GetForwardLookupZones(serverName);

            ViewData["zones"] = zones;
            ViewData["serverName"] = serverName;
            return View();
        }

        [AuthorizationFilter]
        public ActionResult ConditionalForwarders(string serverName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.View, true, serverName,"","", "view conditional lookup zones");
            if (authres != null)
                return authres;

            var zones = GetConditionalForwardersZones(serverName);

            ViewData["zones"] = zones;
            ViewData["serverName"] = serverName;
            return View();
        }

        [AuthorizationFilter]
        public ActionResult ReverseLookupZones(string serverName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.View, true, serverName,"","", "view reverse lookup zones");
            if (authres != null)
                return authres;

            var zones = GetReverseLookupZones(serverName);

            ViewData["zones"] = zones;
            return View();
        }

        [AuthorizationFilter]
        public ActionResult NSLookup(string serverName)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.NSLookup, true, serverName,"","", "nslookup access");
            if (authres != null)
                return authres;

            ViewData["Server"] = serverName;
            return View();
        }

        [AuthorizationFilter]
        public ActionResult NSLookupQuery(string serverName, string Type, string Query, Heijden.DNS.QType QueryType, Heijden.DNS.TransportType TransportType, Heijden.DNS.QClass Class)
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.NSLookup, true, serverName,"","", "nslookup query " + Query);
            if (authres != null)
                return authres;

            Heijden.DNS.Dig dig = new Heijden.DNS.Dig();
            dig.resolver.TransportType = TransportType;
            dig.resolver.DnsServer = serverName;

            StringBuilder sbresult = new StringBuilder();

            dig.MessageDelegate = delegate(string text)
            {
                sbresult.AppendLine(text);
            };

            switch (Type)
            {
                case "lookup": dig.DigIt(Query, QueryType, Class);
                    break;
                case "trace": dig.Trace(Query, QueryType, Class);
                    break;
            }


            return Content(sbresult.ToString());
        }

        [AuthorizationFilter]
        public ActionResult GetDNSHostnameByIP(string ipaddr)
        {
            var result = NetworkHelper.GetReverseDNS(ipaddr, 3000);
            if (!string.IsNullOrEmpty(result))
                return Content(result);


            return Content("unresolved");
        }


        [AuthorizationFilter]
        public ActionResult AuditLog()
        {
            var authres = CheckAuthorizationAndLog(Audit.AuditTypeEnum.ViewEventLog, true, null,"","", "Audit Log");
            if (authres != null)
                return authres;

            var records = Audit.GetAuditRecords();

            ViewData["records"] = records;

            return View();
        }
    }
}
