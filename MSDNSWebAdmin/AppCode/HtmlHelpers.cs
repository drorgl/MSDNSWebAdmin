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
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Html Helpers
    /// </summary>
    public static class HtmlHelpers
    {
        /// <summary>
        /// Get Localized string from resource, throws an exception if value is non-existant for easy development
        /// </summary>
        /// <param name="key">Key name</param>
        /// <returns>Localized string</returns>
        public static string GetLocalizedString(this HtmlHelper html, string key)
        {

            var value = html.ViewContext.HttpContext.GetGlobalResourceObject("Global", key);
#if DEBUG
            if ((value == null) || (string.IsNullOrEmpty(value.ToString())))
                throw new ArgumentNullException("No Value for key " + key);
#endif

            return value.ToString();
        }

        public static string GetLocalizedString(this HtmlHelper html, string key, object arg0)
        {
            return string.Format(html.GetLocalizedString(key), arg0);
        }

        public static string GetLocalizedString(this HtmlHelper html, string key, object arg0,object arg1)
        {
            return string.Format(html.GetLocalizedString(key), arg0,arg1);
        }

        public static string GetEnumTextLocalized<TEnum>(TEnum val, HtmlHelper html)
        {
            var key = val.GetType().Name + "_" + val.ToString();
            return html.GetLocalizedString(key);
        }

        public static TEnum ToValue<TEnum>(this TEnum enumObj, string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        /// <summary>
        /// Converts an enum to Select List
        /// </summary>
        /// <typeparam name="TEnum">enum type</typeparam>
        /// <param name="enumObj">enum value</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, HtmlHelper html) 
        {
            //Enum.GetValues(typeof(TEnum)).GetValue(0).GetType().Name

            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = GetEnumTextLocalized(e,html) };

            return new SelectList(values, "Id", "Name", enumObj);
        }

        /// <summary>
        /// Renders a textbox + label
        /// </summary>
        /// <param name="html"></param>
        /// <param name="label"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxForModel(this HtmlHelper html, string id, string value)
        {
            return new MvcHtmlString(string.Format("<label>{0}{1}</label>", html.GetLocalizedString(id), html.TextBox(id, value).ToHtmlString()));
        }

        public static MvcHtmlString TextBoxForModel(this HtmlHelper html, string id, string value, bool read_only = false)
        {
            object htmlAttributes = null;

            if (read_only)
            {
                htmlAttributes = new { @readonly = "readonly", @disabled = "disabled"};
            }


            return new MvcHtmlString(string.Format("<label>{0}{1}</label>", html.GetLocalizedString(id), html.TextBox(id, value,htmlAttributes).ToHtmlString()));
        }

        public static MvcHtmlString TextBoxForModel(this HtmlHelper html, string id, TimeSpan value)
        {
            return new MvcHtmlString(string.Format("<label>{0}{1}{2}</label>", html.GetLocalizedString(id), html.TextBox(id, value).ToHtmlString(), html.GetLocalizedString("TimeSpanFormat")));
        }

        public static MvcHtmlString TextBoxForModel(this HtmlHelper html, string id, TimeSpan value, bool read_only = false)
        {
            object htmlAttributes = null;

            if (read_only)
            {
                htmlAttributes = new { @readonly = "readonly", @disabled = "disabled" };
            }

            return new MvcHtmlString(string.Format("<label>{0}{1}{2}</label>", html.GetLocalizedString(id), html.TextBox(id, value,htmlAttributes).ToHtmlString(), html.GetLocalizedString("TimeSpanFormat")));
        }

        /// <summary>
        /// Renders a dropdownlist + label
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="html"></param>
        /// <param name="label"></param>
        /// <param name="enumval"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListForModel<TEnum>(this HtmlHelper html, string id, TEnum enumval, bool disabled = false)
        {
            object htmlAttributes = null;

            if (disabled)
            {
                htmlAttributes = new { @disabled = "true" };
            }

            return new MvcHtmlString(string.Format("<label>{0}{1}</label>", html.GetLocalizedString(id), html.DropDownList(id, enumval.ToSelectList(html),htmlAttributes)));
        }

        public static MvcHtmlString CheckboxForModel(this HtmlHelper html, string id, bool value)
        {
            return new MvcHtmlString(string.Format("<label>{1}{0}</label>", html.GetLocalizedString(id), html.CheckBox(id, value).ToHtmlString()));
        }

        public static MvcHtmlString DisplayLabelForModel(this HtmlHelper html, string id, string value)
        {
            return new MvcHtmlString(string.Format("<label>{0}{1}</label>", html.GetLocalizedString(id),value));
        }
    }
}