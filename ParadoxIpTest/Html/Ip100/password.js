var cpt_pass;var pass_mode;function passinit(){if(pass_mode!=1){top.mainframe.formframe.document.pasf.oldp.focus();}else{setTimeout("top.mainframe.formframe.location.href='status.html';",3000);}}function pasen(){var val,temp,spass,s_low;val=true;if(top.mainframe.formframe.document.pasf.oldp.value==""||top.mainframe.formframe.document.pasf.newp.value==""||top.mainframe.formframe.document.pasf.newp2.value==""){top.customalert(top.mainframe.ln_pass[1],"top.mainframe.formframe.document.pasf.oldp.focus();",1);val=false;}else if(top.mainframe.formframe.document.pasf.newp.value!=top.mainframe.formframe.document.pasf.newp2.value){top.customalert(top.mainframe.ln_pass[2],"top.mainframe.formframe.document.pasf.oldp.focus();",1);val=false;}else if(!valide_char(top.mainframe.formframe.document.pasf.newp.value)||!valide_char(top.mainframe.formframe.document.pasf.newp2.value)){top.customalert(top.mainframe.ln_pass[10],"top.mainframe.formframe.document.pasf.oldp.focus();",1);val=false;}else{s_low=top.keeplowbyte(top.mainframe.formframe.document.pasf.oldp.value);top.mainframe.formframe.document.pasf.oldp.value=s_low;s_low=top.keeplowbyte(top.mainframe.formframe.document.pasf.newp.value);top.mainframe.formframe.document.pasf.newp.value=s_low;s_low=top.keeplowbyte(top.mainframe.formframe.document.pasf.newp2.value);top.mainframe.formframe.document.pasf.newp2.value=s_low;temp=top.mainframe.formframe.hex_md5(top.mainframe.formframe.document.pasf.oldp.value);spass=temp+top.mainframe.formframe.document.pasf.ses.value;top.mainframe.formframe.document.pasf.old.value=top.mainframe.formframe.hex_md5(spass);top.mainframe.formframe.document.pasf.new1.value=top.mainframe.formframe.rc4(spass,top.mainframe.formframe.document.pasf.newp.value);top.mainframe.formframe.document.pasf.new2.value=top.mainframe.formframe.rc4(spass,top.mainframe.formframe.document.pasf.newp2.value);}top.mainframe.formframe.document.pasf.oldp.value="";top.mainframe.formframe.document.pasf.newp.value="";top.mainframe.formframe.document.pasf.newp2.value="";return val;}function passaff(sesv,inf){var sre;cpt_pass=6;sre="<center><form name='pasf' action='password.html' method='get' onSubmit='return top.pasen();'>";sre+=top.cmtop("gray",top.mainframe.ln_pass[cpt_pass++],"#000000","100%");sre+=passconst(sesv,inf);sre+=top.cmbot("100%");sre+="</center></form>";return sre;}function passconst(sesv,inf){var sre;var mess="";switch(inf){case 1:mess=passm(top.mainframe.ln_pass[3],"c1");break;case 2:mess=passm(top.mainframe.ln_pass[4],"c4");break;case 3:mess=passm(top.mainframe.ln_pass[5],"c4");break;default:break;}sre="<table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><TD width='1' bgcolor='#C8CFD2'></TD><td><center><table width='100%'><tr class='he4'><td></td></tr>"+mess;if(inf!=1){sre+="<tr class='he4'><td></td></tr><tr class='f10 far'><td width='15px'></td><td width='200px'>"+top.mainframe.ln_pass[cpt_pass++]+"</td><td width='150px' align='right'><input id='oldp' type='password' size='20' maxlength='16'></td><td width='20px'></td></tr><tr class='f10 far'><td></td><td>"+top.mainframe.ln_pass[cpt_pass++]+"</td><td align='right' width='150px'><input id='newp' type='password' size='20' maxlength='16'></td></tr><tr class='f10 far'><td></td><td>"+top.mainframe.ln_pass[cpt_pass++]+"</td><td align='right' width='150px'><input id='newp2' type='password' size='20' maxlength='16'></td></tr><tr><td colspan='3' align='right'><input type='submit' name='submit' value='"+top.mainframe.ln_pass[0]+"'></td></tr>";}sre+="<tr><td><input type=hidden name='old' size=32><input type=hidden name='new1' size=32><input type=hidden name='new2' size=32><input type=hidden id='ses' value="+sesv+" size=16></td></tr></table></center></TD><TD width='1' bgcolor='#C8CFD2'></TD></tr></table>";pass_mode=inf;return sre;}function passm(mess,col){return"<tr><td colspan=4 class='f10 far "+col+"' align='center'>"+mess+"</td></tr>";}function valide_char(svalue){var val=true;var i,short_val;for(i=0;i<svalue.length;i++){short_val=svalue.charCodeAt(i);if((short_val>=48&&short_val<=57)||(short_val>=65&&short_val<=90)||(short_val>=97&&short_val<=122)){val=true;}else{val=false;}}return val;}