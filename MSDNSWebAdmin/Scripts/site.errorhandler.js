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

*/

/// <reference path="jquery-1.4.4-vsdoc.js" />


//Attach global juery ajaxerror
$(window).ready(function ()
{
    $(document).ajaxError(function (e, jqxhr, settings, exception)
    {
        var ex = exception;
        if (ex != null)
            ex = ex.toString();
        if (ex != "")
            showMessage("Ajax Error - " + ex, jqxhr.responseText);
    });
});

//Hides the error window
function hideMessage()
{
    $('#mask').hide();
    $('#messageWindow').hide();
}

//Shows the error window.
function showMessage(strTitle, strMessage)
{
    //debugger;
    var mask = $('#mask');

    if (mask.length == 0) {
        mask = $('<div id="mask" class="windowMask"></div>');
        $("body").prepend(mask);
    }

    //Get the screen height and width
    var maskHeight = $(document).height();
    var maskWidth = $(window).width();

    //Set height and width to mask to fill up the whole screen
    mask.css({ 'width': maskWidth, 'height': maskHeight });

    //transition effect		
    mask.fadeIn(1000);
    mask.fadeTo("slow", 0.8);

    //Get the window height and width
    var winH = $(window).height();
    var winW = $(window).width();

    var messageWindow = $('#messageWindow');
    if (messageWindow.length == 0) {
        messageWindow = $('<div id="messageWindow" class="windowError"></div>');
        $("body").prepend(messageWindow);
    }

    //<div class="windowHeader"></div>
    messageWindow.html('<div class="windowHeader">' + strTitle + '</div><div class="windowClose" onclick="hideMessage();">Close</div>' + '<div class="windowContent">' + strMessage + '</div>');

    //Set the popup window to center
    $(messageWindow).css('top', winH / 2 - $(messageWindow).height() / 2);
    $(messageWindow).css('left', winW / 2 - $(messageWindow).width() / 2);

    //transition effect
    $(messageWindow).fadeIn(2000);
}


