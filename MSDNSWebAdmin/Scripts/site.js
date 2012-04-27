/*
DNS Web Admin - MS DNS Web Administration
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
2012-02-07 - Add zone type/ip version change ui handling.
2012-03-09 - Fix Tree Refresh / add notify
2012-03-17 - Fix search and double clicking a tree node to open its subnodes.

*/

/// <reference path="jquery-1.4.4-vsdoc.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="jquery.unobtrusive-ajax.min.js" />
/// <reference path="jquery.splitter.js" />

function notify(data)
{
    if (data == null)
        return;
    if ($('#notify').length == 0)
    {
        $('body').prepend($('<div id="notify" style="display:none;"></div>')).stop(true, true);
    }

    $('#notify').html(data).removeAttr("style").addClass('notifySuccess').click(function ()
  {
      $("#notify").stop(true, true).fadeOut();
  })
  .delay(2500).fadeOut();
}


function performPageFunctionality()
{

    $('input:radio,input:checkbox').each(function ()
    {
        var enableElements;
        var disableElements;
        var th = $(this);
        if (th.is(':checked'))
        {
            enableElements = th.attr('enableElements');
            if (enableElements != '')
                $(enableElements).attr('disabled', false);
            disableElements = th.attr('disableElements');
            if (disableElements != '')
                $(disableElements).attr('disabled', true);
        }
        else
        {
            enableElements = th.attr('enableElements');
            if (enableElements != '')
                $(enableElements).attr('disabled', true);
            disableElements = th.attr('disableElements');
            if (disableElements != '')
                $(disableElements).attr('disabled', false);
        }
    });
}

function serversUpdateStatus()
{
    
    $('#serverList tbody tr').each(function ()
    {
        var serverName = $(this).children("td:eq(0)").text();
        if (serverName != "")
        {
            $(this).children("td:eq(2)").load('/Home/ServersStatus?serverName=' + serverName, { preventCaching: Math.random() });
        }
    });

    if ($('#serverList tbody tr').length > 0)
        setTimeout(function () { serversUpdateStatus(); }, 5000);        
}

function reattachPageFunctionality()
{
    performPageFunctionality();

    //handle radio buttons
    $('input:radio,input:checkbox').change(function ()
    {
        performPageFunctionality();
    });

    //Handle Server statuses
    serversUpdateStatus();

    //handle file upload
    $('.fileupload').each(function ()
    {
        var uploader = new qq.FileUploader({
            // pass the dom node (ex. $(selector)[0] for jQuery users)
            element: $(this)[0],
            // path to server-side upload script
            action: $(this).attr('url')
        });   
    });
}


//Resizes the Split screen
function resizeElements()
{
    $('.page').css("height", $(window).height() - 70);
    var height = $('#main').height();//  - $('#main').offset().top;


    $("#splitScreen").css("height", height).trigger("resize");
}

//Resize Event Handler
$(window).resize(function ()
{
    resizeElements();
});

//On Ready event handler
$(document).ready(function ()
{
    setTimeout(function () { serversUpdateStatus(); }, 1000);


    resizeElements();
    $("#splitScreen").splitter({ cookie: "splitterCookie" });

    //handle search icon click
    $("#searchclick").click(function ()
    {
        $("#jsTree").jstree("search", $('#txtSearch').val());
    });

    //handle search enter
    $('#txtSearch').keypress(function (e)
    {
        if (e.which == 13)
        {
            $("#jsTree").jstree("search", $('#txtSearch').val());
        }
    });

    $("#jsTree")
	        .jstree({
	            "json_data": {
	                "data": [
	                    {
	                        "attr": { "href": "/Home/Servers", "id": "DNSServers" },
	                        "data":
                                {
                                    title: "DNS",
                                    icon: "/Content/icons/dns.png"
                                },

	                        "state": "closed"
	                    }
	                ],
	                "ajax": {
	                    "url": "/Home/Tree",
	                    "cache": false,
	                    "data": function (n)
	                    {
	                        return (
	                        {
	                            "id": n.attr("id"),
	                            "serverName": n.attr("serverName"),
	                            "zoneName": n.attr("zoneName"),
	                            "timestamp": Date.UTC.toString()
	                        });
	                    }
	                }
	            },
	            "search": {
	                "case_insensitive": true,
	                "ajax": {
	                    "url": "/Home/Tree"
	                }
	            },
	            "plugins": ["themes", "json_data", "search", "ui", "cookies"],
	            "themes": {
	                "url": "/Content/jstree.themes/default/style.css",
	                "dots": true,
	                "icons": true,
	                "theme": "default"
	            }
	        })
	        .bind("search.jstree", function (e, data)
	        {
	            notify("Found " + data.rslt.nodes.length + " nodes matching '" + data.rslt.str + "'.");
	        })
            .bind("select_node.jstree", function (event, data)
            {
                // `data.rslt.obj` is the jquery extended node that was clicked
                //alert(data.rslt.obj.attr("href"));
                $('#currentContentSRC').val(data.rslt.obj.attr("href"));
                RefreshContent();

            })
            .delegate("a", "dblclick", function (event, data)
            {
                //workaround because we're preventing click.
                var tree = jQuery.jstree._reference("#jsTree");
                tree.toggle_node();
            })
            .delegate("a", "click", function (event, data) { notify(data); event.preventDefault(); })

});

//Refreshes the Tree
function RefreshTree(selectedNode)
{
    var tree = jQuery.jstree._reference("#jsTree");

    if (selectedNode != null)
        tree.select_node(selectedNode);

    tree.refresh();

    //$("#jsTree").jstree("refresh");
}


function RefreshContent(forceUrl)
{
    if (forceUrl != null)
    {
        $('#currentContentSRC').val(forceUrl)
    }
    $('#contentDiv').load($('#currentContentSRC').val(), { preventCaching: Math.random() }, function ()
    {
        reattachPageFunctionality();
    });
    
}      

function Servers_AddNew()
{
    $('#serveradd').toggle('fast');
}

function Servers_Cancel()
{
    $('#serveradd').hide('fast');
}


function Servers_Save()
{
    //debugger;
    jQuery.validator.unobtrusive.parse('#serveradd form');

    var isValid = $('#serveradd form').valid();
    if (isValid == false)
        return;
    //post data
    var formdata = $('#serveradd form').serialize();

    $.ajax(
        {
            type: "POST",
            url: '/Home/ServersSave',
            data: formdata,
            success: function (data)
            {
                if (data.toLowerCase() == "true")
                {
                    //reload content
                    //refresh servers
                    RefreshContent();
                    RefreshTree();
                }
            }
        });
    
}

function ServerServiceChange(serverName, action)
{
    $.ajax(
        {
            type: "POST",
            url: '/Home/ServerServiceAction',
            data: {
                serverName : serverName,
                serviceName : "DNS",
                action : action
            },
            success: function (data)
            {
                if (data.toLowerCase() == "true")
                {
                    
                    RefreshContent();
                    RefreshTree();
                }
            }
        });
}

function ZonePause(elem, serverName, zoneName)
{
alert('Not implemented');
return;
    $.ajax(
        {
            type: "POST",
            url: '/Home/ZonePause',
            data: {"pause":"true"}, //false?
            success: function (data)
            {
                //update element content, to pause/resume
            }
        });
}

function Servers_Delete(serverName)
{
    if (!confirm("Are you sure you want to delete " + serverName + "?"))
        return;
    //post data
    $.ajax(
        {
            type: "POST",
            url: '/Home/ServersDelete',
            data: { "serverName": serverName },
            success: function (data)
            {
                if (data.toLowerCase() == "true")
                {
                    //reload content
                    //refresh servers
                    RefreshContent();
                    RefreshTree();
                }
            }
        });

    
}

function add_new_row(table, rowcontent)
{
    var row = $(rowcontent);
    if ($(table).length > 0)
    {
        if ($(table + ' > tbody').length == 0) $(table).append('<tbody />');
        ($(table + ' > tr').length > 0) ? $(table).children('tbody:last').children('tr:last').append(row) : $(table).children('tbody:last').append(row);
    }

    return row;
}


function addForwarders()
{
    var txt = 'Enter forwarder IP Address:<br />' + 
    '<input type="text" id="fwIPAddress" ' +
	    ' name="fwIPAddress" value="" />';
	
    function mycallbackform(v,m,f){
        if ((v != undefined) && (f.fwIPAddress != ""))
        {
            //$.prompt(v + ' ' + f.fwIPAddress);
            add_new_row('#tblForwarders', '<tr>' + 
            '<td><input type="hidden" id="Forwarders" name="Forwarders" value="' +  f.fwIPAddress + '" />' +  f.fwIPAddress + '</td>' + 
            '<td>unresolved yet</td>' +
            '<td><a onclick="$(this).closest(\'tr\').remove();"><img src="/Content/icons/delete.png" title="delete" /></a></td></tr>');

        }
    }

    $.prompt(txt,{
	    callback: mycallbackform,
	    buttons: { Add: 'Add', Cancel: 'Cancel' }
    });
}

function addEditRootHints(row, serverName, serverIPs)
{
    var txt = 'Enter Root Hint Hostname:<br />' +
    '<input type="text" id="rhServerName" ' +
	    ' name="rhServerName" value="' + serverName + '" /> <br />' + 
        'Enter Root Hint IPs: <br />' + 
    '<input type="text" id="rhServerIPs" ' +
        ' name="rhServerIPs" value="' + serverIPs + '" />';

    function mycallbackform(v, m, f)
    {
        var rhcontent = '<tr>' +
            '<td><input type="hidden" id="rootHintServer" name="rootHintServer" value="' + f.rhServerName + '" />' + f.rhServerName + '</td>' +
            '<td><input type="hidden" id="rootHintIPs" name="rootHintIPs" value="' + f.rhServerIPs + '" />[' + f.rhServerIPs + ']</td>' +
            '<td><a onclick="$(this).closest(\'tr\').remove();"><img src="/Content/icons/delete.png" title="delete" /></a>' +
            '<a onclick="addEditRootHints($(this).parents(\'tr\'),\'' + f.rhServerName + '\',\'' + f.rhServerIPs + '\');"><img src="/Content/icons/edit.png" title="Edit" alt="Edit" /></a>' +
            '</td></tr>';

        if (row != null)
        {
            $(row).replaceWith(rhcontent);
        }
        else if ((v != undefined) && (f.rhServerName != "") && (f.rhServerIPs != ""))
        {
            //$.prompt(v + ' ' + f.fwIPAddress);
            add_new_row('#tblRootHints', rhcontent);

        }
    }

    $.prompt(txt, {
        callback: mycallbackform,
        buttons: { Add: 'Add', Cancel: 'Cancel' }
    });
}


function showMessageAjax(url, title)
{
    $.ajax(
        {
            type: "POST",
            url: url,
            //data: { "serverName": serverName },
            success: function (data)
            {
                showMessage(title, data);
            }
        });
    }

function saveContent(url)
{
    $.ajax(
        {
            type: "POST",
            url: url,
            data: $('#contentDiv').serializeAnything(),
            success: function (data)
            {
                alert('saved');
                RefreshContent();
            }
        });
    }

//Executes on radio button change Zone type
function zoneTypeChange()
{
    switch ($('#ZoneTypeCreate:checked').val())
    {
        case "Primary":
            $('.showsecondary, .showstub').hide();
            $('.showprimary').show();
            break;
        case "Secondary":
            $('.showstub,.showprimary').hide();
            $('.showsecondary').show();
            break;
        case "Stub":
            $('.showprimary,.showsecondary').hide();
            $('.showstub').show();
            break;
    }
}

//Executes on radio button change ip version
function zoneVersionChange()
{
    switch ($('#IPv:checked').val())
    {
        case "4":
            $('.showipv6').hide();
            $('.showipv4').show();
            break;
        case "6":
            $('.showipv4').hide();
            $('.showipv6').show();
            break;
    }
}

//Executes an nslookup query
function executeNSlookup()
{
    $.ajax(
        {
            type: "POST",
            url: "/Home/NSLookupQuery",
            data: $('#nslookupContainer').serializeAnything(),
            success: function (data)
            {
                $('#nslookupResults').val(data);
            }
        });
    }

//Submit Zone to save
function zone_Save()
{
    $.ajax({
        type: "POST",
        url: "/Home/ZoneSave",
        data: $('#newZone').serializeAnything(),
        success: function (data)
        {
            RefreshTree();
            RefreshContent();
            notify('Zone saved!');
        }
    });
}

//Saves a resourcerecord
function resourceRecord_Save()
{
    $('#resourceRecord input').attr('disabled', null);
    $.ajax({
        type: "POST",
        url: "/Home/ZoneRRSave",
        data: $('#resourceRecord').serializeAnything(),
        success: function (data)
        {
            if (data.result.toLowerCase() == "true")
            {
                resource_Load(data.serverName, data.zoneName, data.record);
                RefreshTree('#ZoneRR_' + data.serverName + "_" + data.zoneName + "_" + data.record.replace(/%/gi,'_'));
                notify("Zone " + data.serverName + "." + data.zoneName + "." + data.record + " saved!")
            }
        }
    });
}

//Deletes a zone
function zone_Delete(serverName, zoneName)
{
    if (!confirm("Are you sure you want to delete " + serverName + "." + zoneName + "?"))
        return;
    //post data
    $.ajax(
        {
            type: "POST",
            url: '/Home/ZoneDelete',
            data: { "serverName": serverName, "zoneName": zoneName },
            success: function (data)
            {
                RefreshContent();
                RefreshTree();
                notify("Zone " + serverName + "." + zoneName + " deleted!")
            }
        });


}

//Loads a zone into the Content area
function zone_Load(serverName, zoneName)
{
    var href = "/Home/Zone?serverName=" + serverName + "&zoneName=" + zoneName;
    RefreshContent(href);
}

//Loads a resource record into the content area
function resource_Load(serverName, zoneName, textRepresentation)
{
    var href = "/Home/ZoneRR?serverName=" + serverName + "&zoneName=" + zoneName + "&zoneTR=" + textRepresentation;
    RefreshContent(href);
}

function resource_New(serverName, zoneName, recordType)
{
    var href = "/Home/ZoneRR?serverName=" + serverName + "&zoneName=" + zoneName + "&RecordType=" + recordType;
    RefreshContent(href);
}

//Start reverse lookup on table row
//elem - tr element
//colnumberIp/Name/Status are the column numbers (td) holding the data needed.
function rowReverseLookup(elem, colnumberIp, colnumberName, colNumberStatus)
{
    var tr = $(elem);
    var children = tr.children('td');

    var ipaddr = $(children[colnumberIp]).text();

    $.ajax({
        type: "POST",
        url: "/Home/GetDNSHostnameByIP",
        data: { ipaddr: ipaddr },
        success: function (data)
        {
            $(children[colnumberName]).html(data);
            $(children[colNumberStatus]).html("OK");
        }
    });
}

//prompts for new Master DNS Server
function addMasterDNSServers()
{
    var txt = 'Add an IP Address or DNS Name:<br />' +
    '<input type="text" id="fwIPAddress" ' +
	    ' name="fwIPAddress" value="" />';

    function mycallbackform(v, m, f)
    {
        if ((v != undefined) && (f.fwIPAddress != ""))
        {
            //$.prompt(v + ' ' + f.fwIPAddress);
            var row = add_new_row('#tblMasterDNSServers', 
            
            '<tr>' +
            '    <td><input type="hidden" name="MasterDNSServer" value="' + f.fwIPAddress + '" />' + f.fwIPAddress + '</td>' +
            '    <td>Resolving...</td>' +
            '    <td>Validating...</td>' +
            '    <td>' +
            '        <a onclick="$(this).closest(\'tr\').remove();"><img src="/Content/icons/delete.png" alt="Delete" title="Delete" /></a>' +
            '        <a onclick="$(this).closest(\'tr\').insertBefore($(this).closest(\'tr\').prev());"><img src="/Content/icons/up.png" alt="Up" title="Up" /></a>' +
            '        <a onclick="$(this).closest(\'tr\').insertAfter($(this).closest(\'tr\').next());"><img src="/Content/icons/down.png" alt="Down" title="Down" /></a>' +
            '    </td>' +
            '</tr>'
            );

            rowReverseLookup(row, 0,1,2);

        }
    }

    $.prompt(txt, {
        callback: mycallbackform,
        buttons: { Add: 'Add', Cancel: 'Cancel' }
    });
}