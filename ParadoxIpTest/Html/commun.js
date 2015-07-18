var curprg = -1;
var maxprg_s1 = 4;
var maxprg_s2 = 5;
var cptprg = 0;
var maxlength = 0;
var serror = 99;
var waitstart = 0;
var waitinitTimeout = 0;
var allarea_exp = 0;
var tblidmenu = [0, 0, 0, 0, 0, 0, 0, 0];
var tblpa = [0, 0, 0, 0, 0, 0, 0, 0];
var tbltog = [0, 0, 0, 0, 0, 0, 0, 0];
var tbl_presentinalarm = [0, 0, 0, 0, 0, 0, 0, 0];
var pos_scrolltop = 0;
var manyarea;
var tog_mnu = -1;
var live_tblu, live_tblz, live_stayd, live_option;
var tmp_trouble = "-1";
var tmp_alarme = "-1";
var subar, subval, subrequest = 0;
var email_currentitem = 0;
var lostcom;

function M$(d) {
    return document.getElementById(d);
}

function pcsMissingAlert() {
    top.customalert(parent.ln_pcs[11] + "<br><br>" + parent.ln_pcs[12], "", 1);
}

function customalert(text, cmd, frame, pcsMissing) {
    var sre = "",
        d, dtxt, title, ok, mObj, center;
    if (frame == 1) {
        d = top.mainframe.formframe.document;
        dtxt = "top.mainframe.formframe.document";
        title = top.mainframe.ln_system[5];
        ok = top.mainframe.ln_system[7];
        center = "100px";
    } else {
        d = top.document;
        dtxt = "top.document";
        title = top.ln_system[5];
        ok = top.ln_system[7];
        center = "41%";
    } if (d.getElementById("CUSTOMALERT")) {
        return;
    }
    if (cmd === null) {
        cmd = "";
    }
    mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
    mObj.id = "CUSTOMALERT";
    sre += "<div class='box-alert'><div class='box' style='width: 300px;left: " + center + ";top: 100px;'><div class='box-title strips'>" + title + "</div><div class='box-content'><p>";
    sre += text;
    sre += "</p><div class='box-action'><input type='submit' id='closeBtn' class='pfdButton' style='width:50px;' onclick='" + dtxt + ".getElementsByTagName(\"body\")[0].removeChild(" + dtxt + ".getElementById(\"CUSTOMALERT\")); " + cmd + "; return false;' value=" + ok + "></div></div></div></div>";
    mObj.innerHTML = sre;
}

function keeplowbyte(svalue) {
    var sre = "",
        short_val, ctemp, i;
    sre = "";
    for (i = 0; i < svalue.length; i++) {
        short_val = svalue.charCodeAt(i);
        short_val %= 256;
        ctemp = String.fromCharCode(short_val);
        sre += ctemp;
    }
    return sre;
}

function logininit(loginmode) {
    document.lf.loginsub.disabled = false;
    M$('user').focus();
}

function loginencrypt() {
    var val, temp, spass, s_low;
    val = true;
    if (document.lf.user.value == "" || document.lf.pass.value == "") {
        top.customalert(parent.ln_logpage[1], "top.document.lf.user.focus();", 0);
        document.lf.user.value = "";
        document.lf.pass.value = "";
        val = false;
    } else {
        s_low = top.keeplowbyte(document.lf.pass.value);
        document.lf.pass.value = s_low;
        temp = hex_md5(document.lf.pass.value);
        spass = temp + document.lf.ses.value;
        document.lf.p.value = hex_md5(spass);
        document.lf.u.value = rc4(spass, document.lf.user.value);
        document.lf.user.value = "";
        document.lf.pass.value = "";
    } if (val == true) {
        document.lf.loginsub.disabled = true;
    }
    return val;
}
var backup_sesv;
var backup_errn;
var backup_snme;
var backup_user;

function loginaff(sesv, errn, snme, user, loginmode, paneltype) {
    var sre = "";
    backup_sesv = sesv;
    backup_errn = errn;
    backup_snme = snme;
    backup_user = user;
    if (snme != "") {
        snme = snme + " - " + parent.ln_logpage[7];
    }
    sre += "<table><tr><td align='middle'><div class='box' style='width:460px;'><div class='box-title strips'>" + snme + "</div><div class='box-content'>";
    sre += logc(sesv, errn, user, loginmode, paneltype);
    sre += "</div></div></td></tr></table>";
    return sre;
}

function logc(sesv, errn, user, loginmode, paneltype) {
    var sre = "",
        mess, add;
    mess = "";
    add = "";
    if (user != "") {
        add = "<br />(" + parent.ln_logpage[12] + " " + user + ")";
    }
    switch (errn) {
    case 1:
        mess = "<div class='msg'>" + parent.ln_logpage[2] + "<br />" + parent.ln_logpage[13] + "</div>";
        break;
    case 2:
        mess = "<div class='msg'>" + parent.ln_logpage[3] + " " + parent.ln_logpage[14] + "</div>";
        break;
    case 3:
        mess = "<div class='msg'>" + parent.ln_logpage[4] + "</div>";
        break;
    case 4:
        mess = "<div class='msg'>" + parent.ln_logpage[5] + add + "</div>";
        break;
    case 5:
        mess = "<div class='msg'>" + parent.ln_logpage[6] + add + "</div>";
        break;
    default:
        break;
    }
    sre += "<form name='lf' action='default.html' method='get' onSubmit='return loginencrypt();'>";
    sre += mess + "<table class='form'><colgroup><col width='50%'/><col width='50%'/></colgroup>";
    sre += "<tr><td>" + parent.ln_logpage[8] + " </td><td><input style='width:100%' size=20 maxlength=6 type=password id=user></td></tr>";
    sre += "<tr><td>" + parent.ln_logpage[9] + " </td><td><input style='width:100%' size=20 maxlength=16 type=password id=pass></td></tr>";
    sre += "<td></td><td><div class='box-action'><input class='pfdButton' type=submit name='loginsub' value='" + parent.ln_logpage[0] + "'></div></td></tr>";
    sre += "</table>";
    sre += "<input type=hidden name='u' size=16>";
    sre += "<input type=hidden name='p' size=32>";
    sre += "<input type=hidden id='ses' size=32 value=" + sesv + "></form>";
    sre += "<div class='msg note'><b>" + parent.ln_logpage[10] + "</b>&nbsp;" + parent.ln_logpage[11] + "</div>";
    return sre;
}

function rc4(key, text) {
    var i, x, y, t, temp, x2, kl;
    kl = key.length;
    s = [];
    for (i = 0; i < 256; i++) {
        s[i] = i;
    }
    y = 0;
    x = kl;
    while (x--) {
        y = (key.charCodeAt(x) + s[x] + y) % 256;
        t = s[x];
        s[x] = s[y];
        s[y] = t;
    }
    x = 0;
    y = 0;
    var z = "";
    for (x = 0; x < text.length; x++) {
        x2 = x & 255;
        y = (s[x2] + y) & 255;
        t = s[x2];
        s[x2] = s[y];
        s[y] = t;
        temp = String.fromCharCode((text.charCodeAt(x) ^ s[(s[x2] + s[y]) % 256]));
        z += d2h(temp.charCodeAt(0));
    }
    return z;
}

function hex_md5(s) {
    return binl2hex(core_md5(str2binl(s), s.length * 8));
}

function str_md5(s) {
    return binl2str(core_md5(str2binl(s), s.length * 8));
}

function core_md5(x, len) {
    x[len >> 5] |= 0x80 << ((len) % 32);
    x[(((len + 64) >>> 9) << 4) + 14] = len;
    var a = 1732584193;
    var b = -271733879;
    var c = -1732584194;
    var d = 271733878;
    for (var i = 0; i < x.length; i += 16) {
        var olda = a;
        var oldb = b;
        var oldc = c;
        var oldd = d;
        a = md5_ff(a, b, c, d, x[i + 0], 7, -680876936);
        d = md5_ff(d, a, b, c, x[i + 1], 12, -389564586);
        c = md5_ff(c, d, a, b, x[i + 2], 17, 606105819);
        b = md5_ff(b, c, d, a, x[i + 3], 22, -1044525330);
        a = md5_ff(a, b, c, d, x[i + 4], 7, -176418897);
        d = md5_ff(d, a, b, c, x[i + 5], 12, 1200080426);
        c = md5_ff(c, d, a, b, x[i + 6], 17, -1473231341);
        b = md5_ff(b, c, d, a, x[i + 7], 22, -45705983);
        a = md5_ff(a, b, c, d, x[i + 8], 7, 1770035416);
        d = md5_ff(d, a, b, c, x[i + 9], 12, -1958414417);
        c = md5_ff(c, d, a, b, x[i + 10], 17, -42063);
        b = md5_ff(b, c, d, a, x[i + 11], 22, -1990404162);
        a = md5_ff(a, b, c, d, x[i + 12], 7, 1804603682);
        d = md5_ff(d, a, b, c, x[i + 13], 12, -40341101);
        c = md5_ff(c, d, a, b, x[i + 14], 17, -1502002290);
        b = md5_ff(b, c, d, a, x[i + 15], 22, 1236535329);
        a = md5_gg(a, b, c, d, x[i + 1], 5, -165796510);
        d = md5_gg(d, a, b, c, x[i + 6], 9, -1069501632);
        c = md5_gg(c, d, a, b, x[i + 11], 14, 643717713);
        b = md5_gg(b, c, d, a, x[i + 0], 20, -373897302);
        a = md5_gg(a, b, c, d, x[i + 5], 5, -701558691);
        d = md5_gg(d, a, b, c, x[i + 10], 9, 38016083);
        c = md5_gg(c, d, a, b, x[i + 15], 14, -660478335);
        b = md5_gg(b, c, d, a, x[i + 4], 20, -405537848);
        a = md5_gg(a, b, c, d, x[i + 9], 5, 568446438);
        d = md5_gg(d, a, b, c, x[i + 14], 9, -1019803690);
        c = md5_gg(c, d, a, b, x[i + 3], 14, -187363961);
        b = md5_gg(b, c, d, a, x[i + 8], 20, 1163531501);
        a = md5_gg(a, b, c, d, x[i + 13], 5, -1444681467);
        d = md5_gg(d, a, b, c, x[i + 2], 9, -51403784);
        c = md5_gg(c, d, a, b, x[i + 7], 14, 1735328473);
        b = md5_gg(b, c, d, a, x[i + 12], 20, -1926607734);
        a = md5_hh(a, b, c, d, x[i + 5], 4, -378558);
        d = md5_hh(d, a, b, c, x[i + 8], 11, -2022574463);
        c = md5_hh(c, d, a, b, x[i + 11], 16, 1839030562);
        b = md5_hh(b, c, d, a, x[i + 14], 23, -35309556);
        a = md5_hh(a, b, c, d, x[i + 1], 4, -1530992060);
        d = md5_hh(d, a, b, c, x[i + 4], 11, 1272893353);
        c = md5_hh(c, d, a, b, x[i + 7], 16, -155497632);
        b = md5_hh(b, c, d, a, x[i + 10], 23, -1094730640);
        a = md5_hh(a, b, c, d, x[i + 13], 4, 681279174);
        d = md5_hh(d, a, b, c, x[i + 0], 11, -358537222);
        c = md5_hh(c, d, a, b, x[i + 3], 16, -722521979);
        b = md5_hh(b, c, d, a, x[i + 6], 23, 76029189);
        a = md5_hh(a, b, c, d, x[i + 9], 4, -640364487);
        d = md5_hh(d, a, b, c, x[i + 12], 11, -421815835);
        c = md5_hh(c, d, a, b, x[i + 15], 16, 530742520);
        b = md5_hh(b, c, d, a, x[i + 2], 23, -995338651);
        a = md5_ii(a, b, c, d, x[i + 0], 6, -198630844);
        d = md5_ii(d, a, b, c, x[i + 7], 10, 1126891415);
        c = md5_ii(c, d, a, b, x[i + 14], 15, -1416354905);
        b = md5_ii(b, c, d, a, x[i + 5], 21, -57434055);
        a = md5_ii(a, b, c, d, x[i + 12], 6, 1700485571);
        d = md5_ii(d, a, b, c, x[i + 3], 10, -1894986606);
        c = md5_ii(c, d, a, b, x[i + 10], 15, -1051523);
        b = md5_ii(b, c, d, a, x[i + 1], 21, -2054922799);
        a = md5_ii(a, b, c, d, x[i + 8], 6, 1873313359);
        d = md5_ii(d, a, b, c, x[i + 15], 10, -30611744);
        c = md5_ii(c, d, a, b, x[i + 6], 15, -1560198380);
        b = md5_ii(b, c, d, a, x[i + 13], 21, 1309151649);
        a = md5_ii(a, b, c, d, x[i + 4], 6, -145523070);
        d = md5_ii(d, a, b, c, x[i + 11], 10, -1120210379);
        c = md5_ii(c, d, a, b, x[i + 2], 15, 718787259);
        b = md5_ii(b, c, d, a, x[i + 9], 21, -343485551);
        a = safe_add(a, olda);
        b = safe_add(b, oldb);
        c = safe_add(c, oldc);
        d = safe_add(d, oldd);
    }
    return Array(a, b, c, d);
}

function md5_cmn(q, a, b, x, s, t) {
    return safe_add(bit_rol(safe_add(safe_add(a, q), safe_add(x, t)), s), b);
}

function md5_ff(a, b, c, d, x, s, t) {
    return md5_cmn((b & c) | ((~b) & d), a, b, x, s, t);
}

function md5_gg(a, b, c, d, x, s, t) {
    return md5_cmn((b & d) | (c & (~d)), a, b, x, s, t);
}

function md5_hh(a, b, c, d, x, s, t) {
    return md5_cmn(b ^ c ^ d, a, b, x, s, t);
}

function md5_ii(a, b, c, d, x, s, t) {
    return md5_cmn(c ^ (b | (~d)), a, b, x, s, t);
}

function safe_add(x, y) {
    var lsw = (x & 0xFFFF) + (y & 0xFFFF);
    var msw = (x >> 16) + (y >> 16) + (lsw >> 16);
    return (msw << 16) | (lsw & 0xFFFF);
}

function bit_rol(num, cnt) {
    return (num << cnt) | (num >>> (32 - cnt));
}

function binl2hex(binarray) {
    var hex_tab = "0123456789ABCDEF";
    var str = "";
    for (var i = 0; i < binarray.length * 4; i++) {
        str += hex_tab.charAt((binarray[i >> 2] >> ((i % 4) * 8 + 4)) & 0xF) + hex_tab.charAt((binarray[i >> 2] >> ((i % 4) * 8)) & 0xF);
    }
    return str;
}

function d2h(d) {
    var hD = "0123456789ABCDEF";
    var h = hD.substr(d & 15, 1);
    while (d > 15) {
        d >>= 4;
        h = hD.substr(d & 15, 1) + h;
    }
    if (h.length == 1) {
        h = "0" + h;
    }
    return h;
}

function str2binl(str) {
    var bin = Array();
    var mask = (1 << 8) - 1;
    for (var i = 0; i < str.length * 8; i += 8) {
        bin[i >> 5] |= (str.charCodeAt(i / 8) & mask) << (i % 32);
    }
    return bin;
}

function binl2str(bin) {
    var str = "";
    var mask = (1 << 8) - 1;
    for (var i = 0; i < bin.length * 32; i += 8) {
        str += String.fromCharCode((bin[i >> 5] >>> (i % 32)) & mask);
    }
    return str;
}

function waitaff() {
    var sre = "";
    document.title = mainframe.ln_wait[0];
    sre += "<table><tr><td align='middle'><div class='box' style='width:460px;'><div class='box-title strips'>" + mainframe.ln_wait[2] + "</div><div class='box-content'>";
    sre += waitconst();
    sre += "</div></div></td></tr></table>";
    setTimeout("waitinitTimeout = true;", 1000);
    return sre;
}

function waitconst() {
    var sre = "",
        i, cpt = 3;
    sre += "<table class='loading-steps'>";
    for (i = 0; i < 5; i++) {
        sre += "<tr><td style='width: 80px;'></td><td style='width: 16px;padding: 10px;'><div id='d" + i + "' class='checkbox checkbox-uncheck'></div></td><td id='bold" + i + "'><div style='margin-top: 3px;'>" + mainframe.ln_wait[cpt++] + "</div></td></tr>";
    }
    sre += "</table>";
    return sre;
}

function waitinit() {
    if (!waitinitTimeout) {
        setTimeout("waitinit();", 100);
        return;
    }
    cptprg = 0;
    waitstart = 1;
    top.refresh_step1();
    setTimeout("curprg = maxprg_s2;", 5000);
}

function refresh_step1() {
    if (top.mainframe.document.getElementById("d" + cptprg)) {
        while (cptprg < curprg && cptprg < maxprg_s1) {
            top.mainframe.document.getElementById("d" + cptprg).className = "checkbox checkbox-check";
            cptprg++;
        }
    }
    if (cptprg == maxprg_s1) {
        top.loadImages();
        top.refresh_step2();
    } else {
        setTimeout("top.refresh_step1();", 1000);
    }
}

function refresh_step2() {
    waitset_step2();
    if (curprg == maxprg_s2) {
        top.mainframe.document.getElementById("d" + cptprg).className = "checkbox checkbox-check";
        setTimeout("top.mainframe.location.href='index.html';", 250);
    } else {
        setTimeout("top.refresh_step2();", 1000);
    }
}

function waitset_step1(prg) {
    curprg = prg;
    if (prg == serror) {
        top.location.href = "login.html";
    }
}

function waitset_step2() {
    if (top.currCount == top.preload_image.length) {
        curprg = maxprg_s2;
    }
}

function IsNumeric(sTx) {
    var ValC = "0123456789";
    var IsN = true;
    var Ch;
    for (i = 0; i < sTx.length && IsN == true; i++) {
        Ch = sTx.charAt(i);
        if (ValC.indexOf(Ch) == -1) {
            IsN = false;
        }
    }
    return IsN;
}

function st_init() {
    tmp_trouble = "-1";
    tmp_alarme = "-1";
    setLayout("normal");
}

function st_affdata_al(data, hebrew) {
    var sre = "";
    sre += "<div class='box' style='width:220px;margin:20px;'><div class='box-title strips'><img src='l_trouble.png' class='box-icon' alt='trouble' height='24' width='24' />" + mainframe.ln_sts[2] + "</div><div class='box-content'>";
    sre += stal(mainframe.ln_sts[3], "alarm", data, hebrew);
    sre += "</div></div>";
    return sre;
}

function st_affdata_tr(data) {
    var sre = "";
    sre += "<div class='box' style='width:460px;margin:20px;'><div class='box-title strips'><img src='l_trouble.png' class='box-icon' alt='trouble' height='24' width='24' />" + mainframe.ln_sts[4] + "</div><div class='box-content'>";
    sre += sttr(mainframe.ln_sts[5], "l_trouble.png", data);
    sre += "</div></div>";
    return sre;
}

function sttr(tit, icon, data) {
    var sre = "";
    sre += "<ul class='troubles'>";
    for (i = 0; i < data.length; i++) {
        sre += "<li>" + mainframe.tbl_troublename[parseInt(data[i], 10)] + "</li>";
    }
    sre += "</ul>";
    return sre;
}

function stal(tit, icon, data, hebrew) {
    var sre = "";
    sre += "<table><tr><td width='30px'><div class='status status-" + icon + "'></div></td><td>";
    for (var i = 0; i < data.length; i++) {
        if (hebrew != 0) {
            sre += "<div><span class='bidi'>" + mainframe.tbl_areanam[parseInt(data[i], 10)] + "</span>" + " - " + mainframe.ln_system[1] + " " + (parseInt(data[i], 10) + 1) + "</div>";
        } else {
            sre += "<div>" + mainframe.ln_system[1] + " " + (parseInt(data[i], 10) + 1) + " - " + "<span class='bidi'>" + mainframe.tbl_areanam[parseInt(data[i], 10)] + "</span></div>";
        }
    }
    sre += "</td></tr></table>";
    return sre;
}

function st_affdata(snam, tblu, hebrew) {
    var sre = "",
        nbarea;
    nbarea = 0;
    for (i = 0; i < tblu.length; i++) {
        if (tblu[i] != 0) {
            nbarea++;
        }
    }
    if (nbarea > 1) {
        manyarea = 1;
    } else {
        manyarea = 0;
    }
    sre += "<div id='conteneurmenu' style='display:none;'>";
    sre += "<div class='box' style='width:460px;margin:20px;'><div class='box-title strips'>" + mainframe.ln_sts[6] + "</div><div class='box-content nomargin'>";
    sre += st_const(snam, tblu, hebrew);
    sre += "</div></div></div>";
    return sre;
}

function st_affallzonealarm() {
    for (var i = 0; i < tblpa.length; i++) {
        tbltog[i] = !tbltog[i];
        st_affd(i + 1);
    }
    st_refreshexpend(0);
}

function st_refreshexpend(posi) {
    for (var i = 0; i < tbltog.length; i++) {
        if (tblpa[i] == 1) {
            if (tbltog[i] == 1) {
                mainframe.formframe.document.getElementById("EXP" + (i + 1)).innerHTML = st_br(i, mainframe.ln_sts[7], "fl fl-haut");
            } else {
                mainframe.formframe.document.getElementById("EXP" + (i + 1)).innerHTML = st_br(i, mainframe.ln_sts[8], "fl fl-bas");
            }
        }
    }
    if (manyarea) {
        if (allarea_exp == 1) {
            mainframe.formframe.document.getElementById("EXP0").innerHTML = st_bra(mainframe.ln_sts[9], "fl fl-haut");
        } else {
            mainframe.formframe.document.getElementById("EXP0").innerHTML = st_bra(mainframe.ln_sts[10], "fl fl-bas");
        }
    }
}

function st_affzonealarm(area) {
    if (tbl_presentinalarm[area - 1] == 1) {
        mainframe.formframe.document.getElementById("ZS" + area).innerHTML = st_affzonestatus(area, 1);
    }
}

function st_const(snam, tblu, hebrew) {
    var sre = "";
    var writehr = 0;
    var nbmnu = 1;
    subrequest = 0;
    sre += "<div class='areas-head'><table><colgroup><col width='50%' /><col width='25%' /><col width='25%' /></colgroup><tr><td>" + mainframe.ln_sts[11] + "</td><td></td><td align='right'>" + mainframe.ln_sts[12] + "</td></tr></table></div>";
    sre += "<div class='areas-separator'></div>";
    if (manyarea) {
        sre += "<div class='areas-area'><div class='areas-border'><table><colgroup><col width='50%' /><col width='25%' /><col width='25%' /></colgroup><tr><td><b>" + snam + " - " + mainframe.ln_system[0] + "</b></td><td><div id='EXP0'></div></td><td align='right'>" + st_addbtnmnu(99, nbmnu, 99, 0) + "</td></tr></table></div></div>";
        sre += "<div class='areas-separator'></div>";
        nbmnu++;
    }
    for (var i = 0; i < tblu.length; i++) {
        if (tblu[i] != 0) {
            if (writehr) {
                sre += "<div class='areas-separator'></div>";
            }
            writehr = 1;
            tblidmenu[i] = nbmnu;
            tblpa[i] = 1;
            sre += "<div class='areas-area'><div class='areas-border' id='border_" + i + "'><table><colgroup><col width='50%' /><col width='25%' /><col width='25%' /></colgroup>";
            sre += "<tr><td><div class='areas-stayd'><img src='stayd.png' alt='' id='st_stayd" + i + "' style='display:none' /></div>";
            if (hebrew != 0) {
                sre += "<div class='areas-label'><span class='bidi'>" + mainframe.tbl_areanam[i] + "</span>" + " - " + mainframe.ln_system[1] + " " + (i + 1) + "</div>";
            } else {
                sre += "<div class='areas-label'>" + mainframe.ln_system[1] + " " + (i + 1) + " - " + "<span class='bidi'>" + mainframe.tbl_areanam[i] + "</span></div>";
            }
            sre += "<div class='areas-status' id='st_st" + i + "'>" + st_affpartition(i, tblu[i], tblidmenu[i], 1) + "</div></td><td><div id='EXP" + (i + 1) + "'></div></td><td align='right' valign='middle'>";
            sre += st_addbtnmnu(tblu[i], nbmnu, i, 1) + "</td></tr>";
            sre += "<tr><td colspan='3'><div id='ZS" + (i + 1) + "'></div></tr>";
            sre += "</table></div></div>";
            nbmnu++;
        }
    }
    sre += "</div>";
    return sre;
}

function st_addbtnmnu(status, id, area, mode) {
    var sre = "";
    var MG = 0;
    var evo220 = 0;
    if ((mainframe.ProductID & 0xF0) != 0) {
        MG = 1;
    }
    sre += "<div class='ssmenubtn' id='st_bt" + area + "'>" + st_affpartition(area, status, id, 2);
    sre += "<ul id='ssmenu" + id + "' class='ssmenu' onmouseover='top.clearMenuTimeout();' onmouseout='top.setMenuTimeout();' onfocus='top.clearMenuTimeout();' onblur='top.setMenuTimeout();'>";
    if (mode == 0) {
        sre += st_li(area, 'r', mainframe.ln_sts[13]);
        if (!MG) {
            sre += st_li(area, 'f', mainframe.ln_sts[24]);
        }
        if (((live_option & 0x08) != 0) && (MG == 1)) {
            sre += st_li(area, 's', mainframe.ln_sts[25]);
        } else if (((live_option & 0x20) != 0) && (!MG)) {
            sre += st_li(area, 's', mainframe.ln_sts[25]);
        } else {
            sre += st_linone(mainframe.ln_sts[25]);
        } if (!MG && !evo220) {
            if ((live_option & 0x20) != 0) {
                sre += st_li(area, 'i', mainframe.ln_sts[26]);
            } else {
                sre += st_linone(mainframe.ln_sts[26]);
            }
        } else {
            if ((live_option & 0x08) != 0) {
                sre += st_li(area, 'p', mainframe.ln_sts[27]);
            } else {
                sre += st_linone(mainframe.ln_sts[27]);
            }
        } if (mainframe.ArmOnly != '1') {
            sre += "<li><div class='ssmenu-separator'></div></li>";
            sre += st_li(area, 'd', mainframe.ln_sts[16]);
        }
    }
    if (mode == 1) {
        sre += st_li(area, 'r', mainframe.ln_sts[23]);
        if (!MG) {
            sre += st_li(area, 'f', mainframe.ln_sts[24]);
        }
        if (((live_option & 0x08) != 0) && (MG == 1)) {
            sre += st_li(area, 's', mainframe.ln_sts[25]);
        } else if (((live_option & 0x20) != 0) && (!MG)) {
            sre += st_li(area, 's', mainframe.ln_sts[25]);
        } else {
            sre += st_linone(mainframe.ln_sts[25]);
        } if (!MG && !evo220) {
            if ((live_option & 0x20) != 0) {
                sre += st_li(area, 'i', mainframe.ln_sts[26]);
            } else {
                sre += st_linone(mainframe.ln_sts[26]);
            }
        } else {
            if ((live_option & 0x08) != 0) {
                sre += st_li(area, 'p', mainframe.ln_sts[27]);
            } else {
                sre += st_linone(mainframe.ln_sts[27]);
            }
        } if (mainframe.ArmOnly != '1') {
            sre += "<li><div class='ssmenu-separator'></div></li>";
            sre += st_li(area, 'd', mainframe.ln_sts[17]);
        }
    }
    sre += "</ul></div>";
    return sre;
}

function st_affd(area) {
    var i;
    if (area == 0) {
        if (allarea_exp == 1) {
            for (i = 0; i < tblpa.length; i++) {
                tbltog[i] = 1;
            }
            allarea_exp = 0;
        } else {
            for (i = 0; i < tblpa.length; i++) {
                tbltog[i] = 0;
            }
            allarea_exp = 1;
        }
        for (i = 0; i < tblpa.length; i++) {
            st_affd(i + 1);
        }
    } else if (tblpa[area - 1] == 1) {
        if (tbltog[area - 1] == 0) {
            mainframe.formframe.document.getElementById("ZS" + area).innerHTML = st_affzonestatus(area, 0);
            tbltog[area - 1] = 1;
        } else {
            mainframe.formframe.document.getElementById("ZS" + area).innerHTML = "";
            st_affzonealarm(area);
            tbltog[area - 1] = 0;
        }
    }
}

function st_affzonestatus(area, onal) {
    var sre = "",
        numero, classe;
    var cpt = 0;
    var varea = (Math.pow(2, (area - 1)));
    sre += "<table class='areas-zones'>";
    for (var i = 0; i < live_tblz.length; i++) {
        if ((varea & mainframe.tbl_zone[i * 2]) == varea) {
            if (cpt == 0) {
                sre += "<tr>";
                cpt++;
            }
            if ((onal == 0) || (onal == 1 && live_tblz[i] == 2)) {
                switch (live_tblz[i]) {
                case 0:
                    classe = "sc";
                    break;
                case 1:
                    classe = "so";
                    break;
                case 2:
                    classe = "sb";
                    break;
                case 3:
                    classe = "sct";
                    break;
                case 4:
                    classe = "sot";
                    break;
                case 5:
                    classe = "scm";
                    break;
                case 6:
                    classe = "som";
                    break;
                case 7:
                    classe = "sby";
                    break;
                case 8:
                    classe = "sca";
                    break;
                case 9:
                    classe = "soa";
                    break;
                default:
                    break;
                }
                if (i < 9) {
                    numero = "0" + (i + 1);
                } else {
                    numero = (i + 1);
                }
                sre += "<td class='zone-num sz " + classe + "'>" + numero + "</td><td class='zone-label bidi'>" + mainframe.tbl_zone[(i * 2) + 1] + "</td>";
                cpt++;
                if (cpt > 3) {
                    sre += "</tr>";
                    cpt = 0;
                }
            }
        }
    }
    if (cpt > 0) {
        while (cpt < 4) {
            sre += "<td class='f8 far ac b sz sh'></td><td width='120px'></td>";
            cpt++;
        }
        sre += "</tr>";
    }
    sre += "</table>";
    return sre;
}

function st_affpartition(area, status, id, mode) {
    var re1, re2, re3, valstatus;
    buttype = "arm";
    addstyle = "style='color:#FF0000'";
    tbl_presentinalarm[area] = 0;
    switch (status) {
    case 99:
        buttype = "allarea";
        addstyle = "";
        break;
    case 1:
        valstatus = mainframe.ln_sts[18];
        buttype = "disarm";
        addstyle = "";
        break;
    case 2:
        valstatus = mainframe.ln_sts[15];
        break;
    case 3:
        valstatus = "<b>" + mainframe.ln_sts[2] + "</b>";
        buttype = "inalarm";
        tbl_presentinalarm[area] = 1;
        break;
    case 4:
        valstatus = mainframe.ln_sts[19];
        break;
    case 5:
        valstatus = mainframe.ln_sts[20];
        break;
    case 6:
        valstatus = mainframe.ln_sts[21];
        addstyle = "style='color:#CE7109'";
        break;
    case 7:
        valstatus = mainframe.ln_sts[22];
        addstyle = "style='color:#CE7109'";
        break;
    case 8:
        valstatus = mainframe.ln_sts[28];
        buttype = "disarm";
        addstyle = "";
        break;
    case 9:
        valstatus = mainframe.ln_sts[29];
        buttype = "disarm";
        addstyle = "";
        break;
    case 10:
        valstatus = mainframe.ln_sts[30];
        break;
    default:
        break;
    }
    re1 = "<span " + addstyle + ">" + valstatus + "</span>";
    re2 = "<div id='menu" + id + "' name='but" + id + "' class='buttons buttons-" + buttype + "2' style='border-width:0px;' onclick='if(top.tog_mnu==" + id + "){top.hideAllMenu(0);}else{top.showMenu(\"ssmenu" + id + "\");top.tog_mnu=" + id + ";}' onmousedown='top.roll(\"menu" + id + "\", \"buttons buttons-" + buttype + "1\");' onmouseup='top.roll(\"menu" + id + "\", \"buttons buttons-" + buttype + "2\");' onmouseout='top.setMenuTimeout();top.roll(\"menu" + id + "\", \"buttons buttons-" + buttype + "2\");' onmouseover='top.clearMenuTimeout();'></div>";
    if (live_stayd == '1') {
        re3 = "block";
    } else {
        re3 = "none";
    } if (mode == 0) {
        mainframe.formframe.document.getElementById('st_st' + area).innerHTML = re1;
        mainframe.formframe.document.getElementById('st_bt' + area).innerHTML = st_addbtnmnu(status, id, area, 1);
        mainframe.formframe.document.getElementById('st_stayd' + area).style.display = re3;
        top.hideAllMenu(0);
        return 0;
    } else if (mode == 1) {
        return re1;
    } else {
        return re2;
    }
}

function savedata(tblu, tblz, stayd, option) {
    live_tblu = tblu;
    live_tblz = tblz;
    live_stayd = stayd;
    live_option = option;
}

function updatedata(tblu, tblz, stayd, option) {
    var i, j;
    var tbl1 = new Array();
    var tbl2 = new Array();
    var tbl_ref = new Array();
    var varea;
    var refresh = 0;
    var optionlocal = parseInt(option, 10);
    if (stayd != live_stayd) {
        i = 0;
        j = 0;
        for (i = 0, j = 0; i < tblu.length; i++) {
            if (tblu[i] != 0) {
                tbl1[j++] = i;
            }
        }
    }
    i = 0;
    j = 0;
    for (i = 0, j = 0; i < tblu.length; i++) {
        if (tblu[i] != live_tblu[i]) {
            tbl1[j++] = i;
            tbl_ref[i] = 1;
        } else {
            tbl_ref[i] = 0;
        }
    }
    i = 0;
    j = 0;
    for (i = 0, j = 0; i < tblz.length; i++) {
        if (tblz[i] != live_tblz[i]) {
            tbl2[j++] = mainframe.tbl_zone[(i * 2)];
        }
    }
    i = 0;
    for (i = 0; i < tbl2.length; i++) {
        for (j = 0; j < tblu.length; j++) {
            if (tbltog[j] == 1 || tbl_presentinalarm[j] == 1) {
                varea = (Math.pow(2, (j)));
                if ((varea & tbl2[i]) == varea) {
                    tbl_ref[j] = 1;
                }
            }
        }
    }
    savedata(tblu, tblz, stayd, optionlocal);
    i = 0;
    for (i = 0; i < tbl1.length; i++) {
        mainframe.formframe.document.getElementById('border_' + tbl1[i]).style.border = "2px solid white";
        top.st_affpartition(tbl1[i], tblu[tbl1[i]], tblidmenu[tbl1[i]], 0);
    }
    i = 0;
    for (i = 0; i < tbl_ref.length; i++) {
        if (tbl_ref[i] == 1) {
            tbltog[i] = !tbltog[i];
            st_affd(i + 1);
            refresh = 1;
        }
    }
    if (refresh) {
        st_refreshexpend(0);
    }
}

function updatedata_al(new_data, hebrew) {
    if (mainframe.infoframe != undefined) {
        if (mainframe.infoframe.document.getElementById('INALARM') != null) {
            if (new_data != tmp_alarme) {
                if (new_data != "") {
                    mainframe.infoframe.document.getElementById('INALARM').innerHTML = top.st_affdata_al(new_data, hebrew);
                    mainframe.infoframe.document.getElementById('INALARM').style.display = "block";
                } else {
                    mainframe.infoframe.document.getElementById('INALARM').style.display = "none";
                }
                tmp_alarme = new_data;
            }
        }
    }
}

function updatedata_tr(new_data) {
    if (new_data != tmp_trouble) {
        if (new_data != "") {
            mainframe.formframe.document.getElementById('DT').innerHTML = top.st_affdata_tr(new_data);
            mainframe.formframe.document.getElementById('DT').style.display = "block";
        } else {
            mainframe.formframe.document.getElementById('DT').style.display = "none";
        }
        tmp_trouble = new_data;
    }
}

function setsubmit(ar, val) {
    subrequest = 1;
    if (ar < 9) {
        ar = "0" + ar;
    }
    subar = ar;
    subval = val;
    if (liveframe.document.statuslive) {
        makesubmit();
    }
}

function makesubmit() {
    subrequest = 0;
    liveframe.document.statuslive.area.value = subar;
    liveframe.document.statuslive.value.value = subval;
    liveframe.document.statuslive.submit();
}

function afflostcom() {
    var sre = "";
    sre += "<html><head><title>" + top.mainframe.ln_system[3] + "</title></head><body><p style='font-size: 12pt; font-family:Arial; text-align: center; margin-top: 60px;'>" + top.mainframe.ln_system[4] + "<br />" + top.mainframe.ln_system[6] + "</p></body></html>";
    top.document.open();
    top.document.write(sre);
    top.document.close();
}

function st_br(i, nm, img) {
    return "<a href='javascript:top.st_affd(" + (i + 1) + "); top.st_refreshexpend(1);'>" + nm + "<span class='" + img + "'></span></a>";
}

function st_bra(nm, img) {
    return "<a href='javascript:top.st_affd(0); top.st_refreshexpend(1);'>" + nm + "<span class='" + img + "'></span></a>";
}

function st_li(area, id, name) {
    return "<li><a href='javascript:top.setsubmit(\"" + area + "\",\"" + id + "\");'>" + name + "</a></li>";
}

function st_linone(name) {
    return "<li><a href='#'>" + name + "</a></li>";
}
var menu_currentitem = 0;

function menuinit() {
    if (mainframe.contents.document.getElementById('FL0') != null) {
        afficherfl(menu_currentitem, "");
    }
}

function afficherfl(nit, lien) {
    var i;
    for (i = 0; i < 8; i++) {
        if (i == nit) {
            mainframe.contents.document.getElementById('FL' + i).innerHTML = "<img src='l_droite.png' alt=''/>";
        } else {
            mainframe.contents.document.getElementById('FL' + i).innerHTML = "";
        }
    }
    if (lien != "") {
        mainframe.formframe.location.href = lien;
    }
    menu_currentitem = nit;
}

function cant(user) {
    var sre = "";
    sre += "<span class='bidi'>" + user + "</span>" + " " + top.ln_cant[2];
    return sre;
}

function menuaff(user, mode, hebrew) {
    var sre = "",
        i, nom1, nom2;
    if (hebrew != 0) {
        sre += "<div class='box' style='width:220px;margin-top: 20px;'><div class='box-title strips'><span class='bidi'>" + user + "</span>" + " " + mainframe.ln_mnu[0] + "</div><div class='menu-actions'>";
    } else {
        sre += "<div class='box' style='width:220px;margin-top: 20px;'><div class='box-title strips'>" + mainframe.ln_mnu[0] + " " + "<span class='bidi'>" + user + "</span></div><div class='menu-actions'>";
    } if (mode != 2) {
        sre += "<span class='menu-item-disabled'>" + mainframe.ln_mnu[1] + "</span>";
    } else {
        sre += "<a class='password' href='javascript:top.afficherfl(8,\"password.html\");'>" + mainframe.ln_mnu[1] + "</a>";
    }
    sre += "<span style='display:inline-block;width:20px;text-align:center;'>|</span><a class='logout' href='logout.html'>" + mainframe.ln_mnu[2] + "</a></div><div class='menu-title'>" + mainframe.ln_mnu[3] + "</div>";
    sre += "<table class='menu-content'>";
    for (i = 0; i < 8; i++) {
        switch (i) {
        case 0:
            nom1 = "\"status.html\"";
            nom2 = mainframe.ln_mnu[4];
            if (mode == 0) {
                sre += menui(i);
                sre += "<td><span class='menu-item'>" + nom2 + "</span></td></tr>";
                menu_currentitem = 1;
                continue;
            }
            break;
        case 1:
            nom1 = "\"version.html\"";
            nom2 = mainframe.ln_mnu[8];
            break;
        case 2:
            nom1 = "\"config.html\"";
            nom2 = mainframe.ln_mnu[5];
            if (mode != 2) {
                sre += menui(i);
                sre += "<td><span class='menu-item'>" + nom2 + "</span></td></tr>";
                continue;
            }
            break;
        case 3:
            nom1 = "\"rcv.html\"";
            nom2 = mainframe.ln_mnu[21];
            if (mode != 2) {
                sre += menui(i);
                sre += "<td><span class='menu-item'>" + nom2 + "</span></td></tr>";
                continue;
            }
            break;
        case 4:
            nom1 = "\"email.html\"";
            nom2 = mainframe.ln_mnu[6];
            if (mode != 2) {
                sre += menui(i);
                sre += "<td><span class='menu-item'>" + nom2 + "</span></td></tr>";
                continue;
            }
            break;
        case 5:
            nom1 = "\"account.html\"";
            nom2 = mainframe.ln_mnu[7];
            if (mode != 2) {
                sre += menui(i);
                sre += "<td><span class='menu-item'>" + nom2 + "</span></td></tr>";
                continue;
            }
            break;
        case 6:
            nom1 = "\"io.html\"";
            nom2 = mainframe.ln_mnu[9];
            break;
        case 7:
            nom1 = "\"event.html\"";
            nom2 = mainframe.ln_mnu[10];
            break;
        default:
            break;
        }
        sre += menui(i);
        sre += "<td><a class='menu-item' href='javascript:top.afficherfl(" + i + "," + nom1 + ");'>" + nom2 + "</a></td></tr>";
    }
    sre += "</table><div class='legend-title'>" + mainframe.ln_mnu[11] + "</div>";
    sre += "<table class='legend-content'>";
    sre += "<tr><td colspan='4'><div class='legend-type'>" + mainframe.ln_system[1] + "</div></td></tr>";
    sre += "<tr><td><div class='legend-icon status status-arm'></div></td><td>" + mainframe.ln_mnu[12] + "</td><td><div class='legend-icon status status-alarm'></div></td><td>" + mainframe.ln_mnu[13] + "</td></tr>";
    sre += "<tr><td><div class='legend-icon status status-disarm'></div></td><td>" + mainframe.ln_mnu[14] + "</td></tr>";
    sre += "<tr><td colspan='4'><div class='legend-separator'></div></td></tr>";
    sre += "<tr><td colspan='4'><div class='legend-type'>" + mainframe.ln_system[2] + "</div></td></tr>";
    sre += "<tr><td><div class='legend-icon status status-open'></div></td><td>" + mainframe.ln_mnu[15] + "</td><td><div class='legend-icon status status-close'></div></td><td>" + mainframe.ln_mnu[16] + "</td></tr>";
    sre += "<tr><td><div class='legend-icon status status-bypass'></div></td><td>" + mainframe.ln_mnu[17] + "</td><td><div class='legend-icon status status-troubled'></div></td><td>" + mainframe.ln_mnu[18] + "</td></tr>";
    sre += "<tr><td><div class='legend-icon status status-breach'></div></td><td>" + mainframe.ln_mnu[19] + "</td><td><div class='legend-icon status status-memory'></div></td><td>" + mainframe.ln_mnu[20] + "</td></tr>";
    sre += "<tr><td colspan='4'><div class='legend-separator'></div></td></tr>";
    sre += "<tr><td colspan='4' class='legend-logo'><a href='http://www.paradox.com' target='_blank'><img src='paradox.png' alt='Paradox'/></a></td></tr>";
    sre += "</table>";
    sre += "</div>";
    return sre;
}

function menui(i) {
    return "<tr><td width='10px'></td><td width='15px'><div id='FL" + i + "'></div></td>";
}
var cpt_inf;

function infoaff(tbl, hebrew) {
    var sre = "";
    var sitename;
    sitename = HTMLtoStr(tbl[0]);
    top.document.title = sitename;
    cpt_inf = 0;
    sre = "";
    sre += "<div class='box' style='width:220px;margin-top:20px;'><div class='box-title strips'>" + mainframe.ln_inf[cpt_inf++] + "</div><div class='box-content'>";
    sre += "<div class='sitename'>" + tbl[0] + "</div>";
    sre += "</div></div>";
    sre += "<div class='box' style='width:220px;margin-top:20px;'><div class='box-title strips'>" + mainframe.ln_inf[cpt_inf++] + "</div><div class='box-content'>";
    sre += infc(tbl, hebrew);
    sre += "</div></div>";
    return sre;
}

function infc(tbl, hebrew) {
    var sre = "",
        i, cpt_prpt = 6;
    sre += "<table><tr><td><span class='info-type'>" + mainframe.ln_inf[cpt_inf++] + "<span></td></tr>";
    for (i = 1; i < tbl.length; i++) {
        if (i > 2) {
            if (hebrew != 0) {
                sre += "<tr><td><dl><dt><span class='info-value'>" + (tbl[i]) + "</span></dt><dd><span class='info-label'>:" + top.mainframe.ln_inf[cpt_prpt++] + "</span></dd></dl></td></tr>";
            } else {
                sre += "<tr><td><dl><dt><span class='info-label'>" + top.mainframe.ln_inf[cpt_prpt++] + ":</span></dt><dd><span class='info-value'>" + (tbl[i]) + "</span></dd></dl></td></tr>";
            }
        } else {
            sre += "<tr><td><span class='info-value'>" + (tbl[i]) + "</span></td></tr>";
        }
        switch (i) {
        case 1:
            sre += infopt();
            break;
        case 2:
            sre += infopt();
            break;
        case 8:
            sre += infopt();
            break;
        default:
            break;
        }
    }
    sre += "</table>";
    return sre;
}

function infoinit(hebrew) {
    if (top.tmp_alarme && top.tmp_alarme != "" && top.tmp_alarme != "-1") {
        mainframe.infoframe.document.getElementById('INALARM').innerHTML = top.st_affdata_al(top.tmp_alarme, hebrew);
        mainframe.infoframe.document.getElementById('INALARM').style.display = "block";
    } else {
        mainframe.infoframe.document.getElementById('INALARM').style.display = "none";
    }
}

function infopt() {
    return "<tr><td><br /></td></tr><tr><td><span class='info-type'>" + mainframe.ln_inf[cpt_inf++] + "</span></td></tr>";
}
var bypassbody = false;
var vertical = false;
var centrer_menu = false;
var largeur_menu = 95;
var hauteur_menu = 25;
var largeur_sous_menu = 110;
var largeur_auto_ssmenu = true;
var espace_entre_menus = 5;
var top_menu = 0;
var top_ssmenu = top_menu + 28;
var left_menu = 0;
var left_ssmenu = largeur_menu + 2;
var delai = 650;
var marge_en_haut_de_page = top_menu + 40;
var marge_a_gauche_de_la_page = largeur_menu + 10;
var suivre_le_scroll = true;
var cacher_les_select = true;
var nbmenu = 0;
var menuTimeout;
var timeout_roll;
var agt = navigator.userAgent.toLowerCase();
var isMac = (agt.indexOf('mac') != -1);
var isOpera = (agt.indexOf('opera') != -1);
var ieVer = parseInt(agt.substring(agt.indexOf('msie') + 5), 10);
var isIE = ((agt.indexOf('msie') != -1 && !isOpera && (agt.indexOf('webtv') == -1)) && !isMac);
var isIE5win = (isIE && ieVer >= 5);
var isIE5mac = ((agt.indexOf('msie') != -1) && isMac);
var isSafari = (agt.indexOf('safari') != -1);
var reg = new RegExp("px", "g");

function preChargement() {
    if (mainframe.formframe.document.getElementById("conteneurmenu")) {
        mainframe.formframe.document.getElementById("conteneurmenu").style.display = "none";
    }
}

function Chargement() {
    if (mainframe.formframe.document.getElementById("conteneurmenu")) {
        nbmenu = 0;
        while (mainframe.formframe.document.getElementById("menu" + (nbmenu + 1))) {
            nbmenu++;
        }
        mainframe.formframe.document.getElementById("conteneurmenu").style.display = "none";
        trimespaces();
        hideAllMenu(0);
        mainframe.formframe.document.getElementById("conteneurmenu").style.display = 'block';
    }
}

function showMenu(strMenu) {
    bypassbody = true;
    clearMenuTimeout();
    hideAllMenu(0);
    if (mainframe.formframe.document.getElementById(strMenu)) {
        mainframe.formframe.document.getElementById(strMenu).style.display = "block";
    }
}

function setMenuTimeout() {
    menuTimeout = setTimeout('hideAllMenu(0)', delai);
}

function clearMenuTimeout() {
    if (menuTimeout) {
        clearTimeout(menuTimeout);
    }
}

function hideAllMenu(frombody) {
    if (frombody) {
        if (bypassbody) {
            bypassbody = false;
            return;
        }
    }
    for (i = 1; i <= nbmenu; i++) {
        if (mainframe.formframe.document.getElementById("ssmenu" + i)) {
            mainframe.formframe.document.getElementById("ssmenu" + i).style.display = "none";
        }
        top.tog_mnu = -1;
    }
}

function trimespaces() {
    if (isIE5win) {
        for (i = 1; i <= nbmenu; i++) {
            if (mainframe.formframe.document.getElementById("ssmenu" + i)) {
                with(mainframe.formframe.document.getElementById("ssmenu" + i)) {
                    innerHTML = innerHTML.replace(/<LI>|<\/LI>/g, "");
                }
            }
        }
    }
}

function SelectVisible(v, elem) {
    var i;
    if (cacher_les_select && (isIE || isIE5win)) {
        for (i = 0; i < elem.length; i++) {
            elem[i].style.visibility = v;
        }
    }
}

function roll(menu_id, menu_class) {
    mainframe.formframe.document.getElementById(menu_id).className = menu_class;
}

function eventaff(events, opt) {
    var sre = "";
    var lbl = top.mainframe.ln_evt;
    if (top.mainframe.document.getElementById("fsIndex").cols == "*,240," + layoutLargeWidth + ",260,*") {
        layoutWidth = layoutLargeWidth;
    }
    sre += "<form name='events' action='event.html' method='get'>";
    sre += "<div class='box' style='width:auto;margin:20px;'><div class='box-title strips'>" + lbl[0] + "<div id='eventExpand'";
    if (layoutWidth == layoutLargeWidth) {
        sre += " class='event-collapse'";
    } else {
        sre += " class='event-expand'";
    }
    sre += " onclick='adjustLayout(this);'></div></div>";
    sre += "<div class='box-content'><table class='legend-event'>";
    sre += "<tr><td colspan='4'><div class='legend-type-event'>" + lbl[5] + "</div></td>";
    sre += "<td><input type='submit' class='pfdButton' style='float:right;' name='refresh' value='" + lbl[4] + "' /></td></tr>";
    sre += "<tr><td><div class='legend-icon-event status status-open'></div></td><td style='width:65px'>" + lbl[6] + "</td>";
    sre += "<td><div class='legend-icon-event status status-breach'></div></td><td style='width:65px'>" + lbl[7] + "</td></tr>";
    sre += "<tr><td><div class='legend-icon-event status status-bypass'></div></td><td style='width:65px'>" + lbl[8] + "</td>";
    sre += "<td><div class='legend-icon-event status status-cancel'></div></td><td style='width:65px'>" + lbl[9] + "</td>";
    sre += "<td style='float:right'><input type='checkbox' id='opt' onclick='refresheventopt();'  ";
    if (opt == 1) {
        sre += "checked";
    }
    sre += "><label for='opt' style='font-size:12px'>" + lbl[10] + "</label></td></tr>";
    sre += "</table></div>";
    sre += "<table id='eventLog'";
    if (layoutWidth == layoutLargeWidth) {
        sre += " class='event-log large'>";
    } else {
        sre += " class='event-log'>";
    }
    sre += "<tr><th>" + lbl[1] + "</th><th>" + lbl[2] + "</th><th>" + lbl[3] + "</th></tr>";
    for (var i in events) {
        var event = events[i];
        sre += "<tr class='" + event.status + "'><td class='event-date'>" + event.time + "</td><td class='event-name'>" + event.name + "</td><td class='event-params'><ol>";
        for (var j in event.params) {
            var param = event.params[j];
            sre += "<li><dl><dt>" + j + "</dt><dd>" + param + "</dd></dl></li>";
        }
        sre += "</ol></td></tr>";
    }
    sre += "</table>";
    sre += "</div></div></form>";
    setLayout("event");
    return sre;
}

function refresheventopt() {
    var opt;
    (M$("opt").checked == true) ? opt = 1 : opt = 0;
    url = "event_sync.html?msgid=" + 1 + "&opt=" + opt;
    ajaxRequestInit(url);
}
var layoutLargeWidth = 1000;
var layoutSmallWidth = 500;
var layoutWidth = 500;

function adjustLayout() {
    var fs = top.mainframe.document.getElementById("fsIndex");
    var btn = document.getElementById("eventExpand");
    var tbl = document.getElementById("eventLog");
    if (layoutWidth == layoutSmallWidth) {
        layoutWidth = layoutLargeWidth;
        btn.className = 'event-collapse';
        tbl.className = 'event-log large';
    } else {
        layoutWidth = layoutSmallWidth;
        btn.className = 'event-expand';
        tbl.className = 'event-log';
    }
    fs.cols = "*,240," + layoutWidth + ",260,*";
}

function setLayout(mode) {
    var fs = top.mainframe.document.getElementById("fsIndex");
    switch (mode) {
        default:
    case "normal":
        fs.cols = "*,240,500,260,*";
        break;
    case "event":
        break;
    }
}
var Mask = function () {
    var b, m, t;
    return {
        show: function (id) {
            b = M$(id);
            m = M$(id + "_mask");
            if (!b) {
                return;
            }
            if (!m) {
                t = document.createElement('div');
                t.id = id + '_wrap';
                t.style.position = 'relative';
                b.parentNode.insertBefore(t, b);
                t.appendChild(b);
                m = document.createElement('div');
                m.id = id + '_mask';
                m.className = "addon_msk";
                t.appendChild(m);
                this.setpos(m, b);
            } else {
                this.setpos(m, b);
                m.style.visibility = "visible";
            }
        },
        hide: function (id) {
            m = M$(id + "_mask");
            if (m) {
                m.style.visibility = "hidden";
            }
        },a
        setpos: function (m, b) {
            m.style.height = b.offsetHeight + "px";
            m.style.left = b.offsetLeft + "px";
            m.style.top = "0px";
            m.style.width = b.offsetWidth + "px";
        }
    };
}();

function HTMLtoStr(s) {
    s = s.replace(/&#38;/g, "&");
    s = s.replace(/&#34;/g, "\"");
    s = s.replace(/&#60;/g, "<");
    s = s.replace(/&#62;/g, ">");
    s = s.replace(/&#160;/g, " ");
    s = s.replace(/\\r\\n/g, "\r\n");
    s = s.replace(/&#39;/g, "\'");
    s = s.replace(/&#92;/g, "\\");
    return s;
}