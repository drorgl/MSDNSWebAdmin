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

*/

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MSDNSWebAdmin.Models;
using MSDNSWebAdmin.AppCode;
using System.Web.Mvc.Html;
using System.Diagnostics;

namespace MSDNSWebAdmin.Controllers
{
    /// <summary>
    /// MVC Controller - Handles Login accounts
    /// </summary>
    public class AccountController : ControllerBase
    {

        // **************************************
        // URL: /Account/LogOn
        // **************************************
        public ActionResult LogOn()
        {
            return View();
        }

        /// <summary>
        /// Performs LogOns
        /// </summary>
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Performs Windows authentication
                    if (MSDNSWebAdmin.AppCode.WindowsAuthentication.Authenticate(model.UserName, model.Password, model.Domain))
                    {
                        Session["Username"] = model.UserName;
                        Session["Password"] = model.Password;
                        Session["Domain"] = model.Domain;

                        //Log successful login
                        Audit.Log(Audit.AuditTypeEnum.Login, System.Environment.MachineName, (model.Domain) + "\\" + (model.UserName), NetworkHelper.ClientIPAddress(HttpContext), "Logged on","","");

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        Audit.Log(Audit.AuditTypeEnum.Login, System.Environment.MachineName, (model.Domain) + "\\" + (model.UserName), NetworkHelper.ClientIPAddress(HttpContext), "Login failed","","");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    //Log failed Login
                    Audit.Log(Audit.AuditTypeEnum.Login, System.Environment.MachineName, (model.Domain) + "\\" + (model.UserName), NetworkHelper.ClientIPAddress(HttpContext), "Login failed","","");

                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************
        public ActionResult LogOff()
        {
            Audit.Log(Audit.AuditTypeEnum.Login, System.Environment.MachineName, (Session["Domain"] as string) + "\\" + (Session["Username"] as string), NetworkHelper.ClientIPAddress(HttpContext), "Logged off","","");

            Logger("Account","LogOff").InfoFormat("User {0} Logged off", Session["Username"]);

            Session.Abandon();
            
            return RedirectToAction("Index", "Home");
        }

        

    }
}
