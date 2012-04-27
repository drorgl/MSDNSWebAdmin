/*
DNS Web Admin - MS DNS Web Administration - Authorization filter class
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

Description:
This class makes sure the user is authenticated before allowing access to resources/actions

Change log:
2011-05-17 - Initial version

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSDNSWebAdmin.AppCode
{
    /// <summary>
    /// MVC Authentication Filter
    /// <para>Used to verify the user is logged in before allowing it to perform actions</para>
    /// </summary>
    public class AuthorizationFilter : FilterAttribute, IAuthorizationFilter
    {

        #region IAuthorizationFilter Members

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["Username"] == null)
            {
                if (!string.IsNullOrWhiteSpace(AppSettings.AlwaysLoggedOnUsername) &&
                    !string.IsNullOrWhiteSpace(AppSettings.AlwaysLoggedOnPassword) &&
                    !string.IsNullOrWhiteSpace(AppSettings.AlwaysLoggedOnDomain))
                {
                    filterContext.HttpContext.Session["Username"] = AppSettings.AlwaysLoggedOnUsername;
                    filterContext.HttpContext.Session["Password"] = AppSettings.AlwaysLoggedOnPassword;
                    filterContext.HttpContext.Session["Domain"] = AppSettings.AlwaysLoggedOnDomain;
                }
            }

            if (!string.IsNullOrWhiteSpace(AppSettings.AlwaysLoggedOnUsername) &&
                    !string.IsNullOrWhiteSpace(AppSettings.AlwaysLoggedOnPassword) &&
                    !string.IsNullOrWhiteSpace(AppSettings.AlwaysLoggedOnDomain))
            {
                filterContext.Controller.ViewData["ImportantMessages"] = "High Security risk!!! Always Logged On! check web.config";
            }

            //Check session Username exists
            if (filterContext.HttpContext.Session["Username"] == null)
                filterContext.HttpContext.Response.Redirect("/Account/LogOn?returnUrl=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.RawUrl));
        }

        #endregion
    }
}