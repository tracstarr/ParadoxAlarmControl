/*
 *  Paradox Panel
 *
 */ 

metadata {
  
  definition (name: "Paradox Alarm Panel", author: "tracstarr <tracstarr@hotmail.com>", namespace:"bitbounce") {
    capability "Refresh"
    command "partition"
    command "arm"
    command "disarm"
    command "force"
    command "stay"
    command "instant"
    
    attribute "alarmMode", "enum", ["armed","exitdelay","entrydelay","notready","ready","alarm"]
  }

  simulator {    
  }

  tiles {
    standardTile("paradoxPartition", "device.alarmMode", width: 2, height: 2, canChangeBackground: false, canChangeIcon: false) {
      state "armed",     label: 'Armed',      backgroundColor: "#79b821", icon:"st.Home.home3"
      state "exitdelay", label: 'Exit Delay', backgroundColor: "#ff9900", icon:"st.Home.home3"
      state "entrydelay",label: 'EntryDelay', backgroundColor: "#ff9900", icon:"st.Home.home3"
      state "notready",  label: 'Open',       backgroundColor: "#ffcc00", icon:"st.Home.home2"
      state "ready",     label: 'Ready',      backgroundColor: "#79b821", icon:"st.Home.home2", defaultState: true
      state "alarm",     label: 'Alarm',      backgroundColor: "#ff0000", icon:"st.security.alarm.alarm"
    }
    
    standardTile("disarmTile", "disarm", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "default",     label: 'Disarm', action: "disarm",  backgroundColor: "#FF3300", icon:"st.security.alarm.off"       
    }
    
    standardTile("armTile", "arm", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "default",     label: 'Arm', action: "arm", backgroundColor: "#79b821", icon:"st.security.alarm.on"      
    }
    
    standardTile("stayTile", "stay", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "default",     label: 'Stay', action: "stay", backgroundColor: "#FF9900", icon:"st.Home.home4"      
    }
    
    standardTile("forceTile", "force", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "default",     label: 'Force', action: "force", backgroundColor: "#660033", icon:"st.security.alarm.on"      
    }
    
    standardTile("instantTile", "instant", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "default",     label: 'Instant', action: "instant", backgroundColor: "#996600", icon:"st.security.alarm.on"      
    }
    
    main "paradoxPartition"
	details(["paradoxPartition","disarmTile","armTile","forceTile", "instantTile", "stayTile"])
  }
}

def parse(String description) {
  def myValues = description.tokenize()

  log.debug "Event Parse function: ${description}"
 
}

def sendArmAction(action)
{
	log.debug action
   	def dni = device.deviceNetworkId
 	sendEvent(name: "setArmingMode", value: "${dni} ,${action}", descriptionText: "Set alarm mode to ${action}")
}

def arm()
{	
 	sendArmAction("arm")
}

def stay()
{
	sendArmAction("stay")
}

def disarm()
{
	sendArmAction("disarm")
}

def force()
{
	sendArmAction("force")
}

def instant()
{
	sendArmAction("instant")
}

def partition(String state) {
    log.debug "Partition: ${state} for partition: ${partition}"
    sendEvent (name: "alarmMode", value: "${state}")
}

def poll() {
  log.debug "Executing 'poll'" 
}

def refresh() {
  log.debug "Executing 'refresh' which is actually poll()"
  poll()  
}