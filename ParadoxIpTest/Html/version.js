var v;
var mf = top.mainframe;
var g_verpollInterVal;
var g_presence;
function veraff(tblp, tblip, tblpcs, pcsdetected, mode) {
    var sre = "";
    v = 0;
    sre = "";
    sre += "<div class='box' style='width:460px;margin:20px;'><div class='box-title strips'>" + mf.ln_ver[v++] + "</div><div class='box-content'>";
    sre += verp(tblp);
    sre += "</div></div>";
    sre += "<div class='box' style='width:460px;margin:20px;'><div class='box-title strips'>" + mf.ln_ver[v++] + "</div><div class='box-content'>";
    sre += verm(tblip);
    sre += "</div></div>";
    if (pcsdetected != 'undetected') {
        sre += "<div class='box' style='width:460px;margin:20px;'  id='verpcs'><div class='box-title strips'>" + mf.ln_ver[v++] + "</div><div class='box-content'>";
        sre += verpcs(tblpcs, mode);
    }
    sre += "</div></div>";
    top.setLayout("normal");
    return sre;
}
function verstartpolling() {
    g_verpollInterVal = setInterval(pollPCSpresence, 5000);
    if (g_presence != 'Missing') {
        if (M$("ejectBtn")) {
            M$("ejectBtn").style.display = 'none';
        }
    }
}
function pollPCSpresence() {
    var url;
    url = "version_sync.html?msgid=1&" + Math.random().toString().replace(",", ".").split(".")[1];
    ajaxRequestInit(url);
}
function verupdatepcspres(x) {
    g_presence = pcsPresenceGet(x.verpcs[0].pcspresence);
    if (M$("presence")) {
        if (mf.ln_ver[17] == g_presence) {
            hidepcsinfo();
        } else if (mf.ln_ver[16] == g_presence) {
            if (M$("ejectBtn")) {
                M$("ejectBtn").style.display = 'inline';
            }
        } else {
            if (M$("ejectBtn")) {
                M$("ejectBtn").style.display = 'none';
            }
        }
        M$("presence").innerHTML = pcsPresenceGet(x.verpcs[0].pcspresence);
    } else if (mf.ln_ver[17] != g_presence) {
        window.location.reload(true);
    }
}
function verp(tblp) {
    var sre = "";
    var i = 0;
    sre += "<table class='form'><colgroup><col width='50%'/><col width='50%'/></colgroup>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td>" + tblp[i++] + "</td></tr>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td>" + tblp[i++] + "</td></tr>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td>" + tblp[i++] + "</td></tr>";
    sre += "</table>";
    return sre;
}
function verm(tblm) {
    var sre = "";
    var i = 0;
    sre += "<table class='form'><colgroup><col width='50%'/><col width='50%'/></colgroup>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td>" + tblm[i++] + "</td></tr>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td>" + tblm[i++] + "</td></tr>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td>" + tblm[i++] + "</td></tr>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td>" + tblm[i++] + "</td></tr>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td>" + tblm[i++] + "</td></tr>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td>" + tblm[i++] + "</td></tr>";
    sre += "</table>";
    return sre;
}
function verpcs(tblpcs, mode) {
    var sre = "";
    var i = 0;
    g_presence = pcsPresenceGet(tblpcs[i++]);
    sre += "<table class='form'><colgroup><col width='50%'/><col width='50%'/></colgroup>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td id='presence'>" + g_presence + "</td></tr>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td id='fw'>" + tblpcs[i++] + "</td></tr>";
    sre += "<tr><td>" + mf.ln_ver[v++] + "</td><td id='ser'>" + tblpcs[i++] + "</td></tr>";
    sre += "</table>";
    if (2 == mode) {
        sre += "<div class='box-action'><input type='button' name='ejectBtn' id='ejectBtn' value='" + top.mainframe.ln_pcs[1] + "' class='pfdButton' onclick='ejectPcs();'></div>";
    }
    return sre;
}
function ejectPcs() {
    var url;
    url = "version_sync.html?msgid=2&" + Math.random().toString().replace(",", ".").split(".")[1];
    ajaxRequestInit(url);
    M$('ejectBtn').disabled = true;
    M$('ejectBtn').className = "pfdButtonDisabled";
}
function hidepcsinfo() {
    if (M$("verpcs")) {
        M$("verpcs").style.display = 'none';
    }
}
function pcsPresenceGet(idx) {
    var pres;
    switch (idx) {
        case 0:
            pres = mf.ln_ver[15];
            break;
        case 1:
            pres = mf.ln_ver[16];
            break;
        case 2:
            pres = mf.ln_ver[17];
            break;
        default:
            pres = "---";
            break;
    }
    return pres;
}
