String.prototype.toHtml = function () {
    var s = this;
    if (s.indexOf("://") > 0) {
        //url
        s = s.replace(/(^|[^\"\'\]])(http|ftp|mms|rstp|news|https)\:\/\/([^\s\033\[\]\"\']+)/gi, "$1[url]$2://$3[/url]");
        //img
        s = s.replace(/\[url\](http\:\/\/\S+\.)(gif|jpg|jpeg|png)\[\/url\]/gi, "[img]$1$2[/img]");
    }
    //ubb: 匹配[UBB]...[/UBB]形式
    if (s.match(/\[(\w+)([^\[\]\s]*)\].*\[\/\1\]/)) {
        s = s.replace(/\[url\](.+?)\[\/url\]/gi, "<a href=$1 target=_blank>$1</a>");
        s = s.replace(/\[img\](.+?\.(?:gif|jpg|jpeg|png))\[\/img\]/gi, "<img src='$1' alt='$1'>");
        s = s.replace(/\[flash\](.+?\.swf)\[\/flash\]/gi, "<embed src='$1' quality=high wmode=transparent type='application/x-shockwave-flash' width=400 height=300></embed><br> FLASH: <a href='$1' target=_blank>$1</a><br>");
        s = s.replace(/\[wma\](.+?\.(?:wma|mp3))\[\/wma\]/gi, "<embed src='$1' height=40 AutoStart=0></embed><br> WMA: <a href='$1' target=_blank>$1</a><br>");
        s = s.replace(/\[color=([#0-9a-zA-Z]{1,10})\](.+?)\[\/color\]/gi, "<font color='$1'>$2</font>");
        s = s.replace(/\[size=([\d]+)\](.+?)\[\/size\]/gi, "<font size='$1'>$2</font>");
        s = s.replace(/\[b\](.+?)\[\/b\]/gi, "<b>$1</b>");
        s = s.replace(/\[i\](.+?)\[\/i\]/gi, "<i>$1</i>");
    }
    return s;
}