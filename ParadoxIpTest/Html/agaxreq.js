var tempFieldValue;
var keepAliveInt;
var keepAliveTimeout;
function M$(d) {
    return document.getElementById(d);
}
function N$(d) {
    return encodeURIComponentPlus(document.getElementById(d).value);
}
function ajaxRequestInit(url) {
    var xhr = ajaxRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if ((xhr.responseText[0] != '<') && (xhr.responseText[0] != null)) {
                var x = JSON.parse(xhr.responseText);
                ajaxExeAction(x.ack[0].action, x);
                ajaxExeAction(x.ack[0].action2, x);
                if (x.ack[0].disbtn) { setSaveBtn("disable", x.ack[0].disbtn); }
            } else { top.mainframe.location = "/login.html"; }
        }
    }; xhr.open("GET", url, true); xhr.send(null);
}
function ajaxExeAction(action, x) {
    if (action) {
        switch (action) {
            case "none":
                break;
            case "updateinfo": top.mainframe.infoframe.location.href = 'info.html';
                break;
            case "redir": window.location = "/redirect.html";
                break;
            case "reload": window.location = "/reload.html";
                break;
            case "keepalive": clearTimeout(keepAliveTimeout);
                break;
            case "saveEmailList": saveEmailList();
                break;
            case "pcsupdateinfo": RCV.updateInfo(x);
                break;
            case "pcsmissing": pcsMissingAlert();
                break;
            case "updatepcspres": verupdatepcspres(x);
                break;
            case "hidepcsinfo": hidepcsinfo();
                break;
            default:
                break;
        }
    }
}
function ajaxRequest() {
    var activexmodes = ["Msxml2.XMLHTTP", "Microsoft.XMLHTTP"];
    if (window.ActiveXObject) {
        for (var i = 0; i < activexmodes.length; i++) {
            try { return new ActiveXObject(activexmodes[i]); } catch (e) { }
        }
    } else if (window.XMLHttpRequest) {
        return new XMLHttpRequest();
    } else {
         return false;
    }
};

function initSaveBtn(nbButtons) {
    var i;
    if (nbButtons === 'all') {
        var i = 1;
        while (M$('saveBtn' + i) != null) { setSaveBtn("disable", i); i++; }
    } else { for (i = 1; i < (nbButtons + 1) ; i++) { setSaveBtn("disable", i); } }
}
function backupOnFocus(lastValue) { tempFieldValue = lastValue; }
function checkForChanges(currValue, btnId) {
     if ((currValue != tempFieldValue) && (M$('saveBtn' + btnId).disabled = true)) {
         setSaveBtn("enable", btnId);
         return true;
     } else { return false; }
}
function setSaveBtn(state, btnId) {
     if (btnId === 'all') {
         var i = 1;
         while (M$('saveBtn' + i) != null) {
             setSaveBtn(state, i);
             i++;
         }
     } else {
          if (state == "enable") {
               M$('saveBtn' + btnId).disabled = false; M$('saveBtn' + btnId).className = 'pfdButton';
          } else if (state == "disable") { M$('saveBtn' + btnId).disabled = true; M$('saveBtn' + btnId).className = 'pfdButtonDisabled'; }
     }
}
function clearKeepAliveInt() {
     clearInterval(keepAliveInt);
}
function setKeepAliveInt() {
     keepAliveInt = setInterval(function () { keep_alive() }, 5000);
}
function keep_alive() {
    var url = "keep_alive.html?msgid=1" + "&" + Math.random().toString().replace(",", ".").split(".")[1];
    keepAliveTimeout = setTimeout(clearKeepAliveInt, 20000);
    ajaxRequestInit(url);
}
function ClearPage(msg) {
    var d = top.document;
    clearKeepAliveInt();
    d.open();
    d.write("<html><body><br><br><p id='cfgSavedMsg' style='font-size: 12pt;font-family:Arial' align='center'>" + msg + "</p></body></html>"); d.close();
}
function encodeURIComponentPlus(s) {
    var s2 = encodeURIComponent(s);
    var s1 = "";
    while (s1 != s2) {
         s1 = s2; s2 = s1.replace("*", "%2A");
    } s1 = "";
    while (s1 != s2) {
         s1 = s2; s2 = s1.replace("\'", "%27");
    } s1 = "";
    while (s1 != s2) {
         s1 = s2; s2 = s1.replace("\"", "%22");
    } s1 = "";
    while (s1 != s2) {
         s1 = s2; s2 = s1.replace("(", "%28");
    } s1 = "";
    while (s1 != s2) {
         s1 = s2; s2 = s1.replace(")", "%29");
    } s1 = "";
    while (s1 != s2) {
         s1 = s2; s2 = s1.replace("%0A", " ");
    }
    if (s2.charAt(s2.length) == " ")
        s2.charAt(s2.length) = "%00"; return s2;
};
function forceFrameSetRefresh() {
    var fs = window.top.document.getElementsByTagName('frameset');
    var fcols = fs[0].cols;
    fs[0].removeAttribute('cols');
    fs[0].setAttribute('rows', '1500,*');
    fs[0].removeAttribute('rows');
    fs[0].setAttribute('cols', '1500,1');
    setTimeout("var fs = window.top.document.getElementsByTagName('frameset'); fs[0].removeAttribute('cols'); fs[0].setAttribute('rows', '1500,*'); fs[0].removeAttribute('rows'); fs[0].setAttribute('cols','" + fcols + "')", 1);
}