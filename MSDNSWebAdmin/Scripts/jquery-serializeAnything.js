/* @projectDescription jQuery Serialize Anything - Serialize anything (and not just forms!)
* @author Bramus! (Bram Van Damme)
* @version 1.0
* @website: http://www.bram.us/
* @license : BSD
* changedlog:
2012-03-09 - Dror Gluska - add disabled elements, removing them caused problems..
*/
(function ($)
{
    $.fn.serializeAnything = function ()
    {
        var toReturn = [];
        var els = $(this).find(':input').get();
        $.each(els, function ()
        {
            //if (this.name && !this.disabled && (this.checked || /select|textarea/i.test(this.nodeName) || /text|hidden|password/i.test(this.type)))
            if (this.name && (this.checked || /select|textarea/i.test(this.nodeName) || /text|hidden|password/i.test(this.type)))
            {
                var val = $(this).val();
                toReturn.push(encodeURIComponent(this.name) + "=" + encodeURIComponent(val));
            }
        });
        return toReturn.join("&").replace(/%20/g, "+");
    }
})(jQuery);