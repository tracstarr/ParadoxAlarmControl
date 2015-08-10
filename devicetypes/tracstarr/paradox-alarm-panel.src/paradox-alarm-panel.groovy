/*
 *  Paradox Panel
 *
 */ 

metadata {
  
  definition (name: "Paradox Alarm Panel", author: "tracstarr <tracstarr@hotmail.com>", namespace:"tracstarr") {
    capability "Actuator"
    capability "Refresh"
    command "setStatus"
    command "arm"
    command "disarm"
    command "force"
    command "stay"
    command "instant"
    
    attribute "alarmMode", "enum", ["armed","delay","notready","ready","inalarm"]    
  }

  simulator {    
  }

  tiles {
    standardTile("paradoxPartition", "device.alarmMode", width: 2, height: 2, canChangeBackground: false, canChangeIcon: false) {
      state "armed",     label: 'Armed',      backgroundColor: "#79b821", icon:"st.Home.home3"
      state "delay",	 label: 'Delay', 	  backgroundColor: "#ff9900", icon:"st.Home.home3"
      state "notready",  label: 'Open',       backgroundColor: "#ffcc00", icon:"st.Home.home2"
      state "ready",     label: 'Ready',      backgroundColor: "#79b821", icon:"st.Home.home2", defaultState: true
      state "inalarm",   label: 'Alarm',      backgroundColor: "#ff0000", icon:"st.security.alarm.alarm"
    }
    
    standardTile("disarmTile", "device.alarmMode", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "armed",     label: 'Disarm', action: "disarm",  backgroundColor: "#FF3300", icon:"st.security.alarm.off"       
      state "default",     label: 'Disarm', action: "disarm",  backgroundColor: "#D0D0D0", icon:"st.security.alarm.off"       
    }
    
    standardTile("armTile", "device.alarmMode", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "ready",     label: 'Arm', action: "arm", backgroundColor: "#79b821", icon:"st.security.alarm.on"      
      state "default",     label: 'Arm', action: "arm", backgroundColor: "#D0D0D0", icon:"st.security.alarm.on"      
    }
    
    standardTile("stayTile", "device.alarmMode", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "ready",     label: 'Stay', action: "stay", backgroundColor: "#FF9900", icon:"st.Home.home4"      
      state "default",     label: 'Stay', action: "stay", backgroundColor: "#D0D0D0", icon:"st.Home.home4"      
    }
    
    standardTile("forceTile", "device.alarmMode", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "default",     label: 'Force', action: "force", backgroundColor: "#660033", icon:"st.security.alarm.on"      
      state "armed",     label: 'Force', action: "force", backgroundColor: "#D0D0D0", icon:"st.security.alarm.on"      
    }
    
    standardTile("instantTile", "device.alarmMode", width: 1, height: 1, canChangeBackground: false, canChangeIcon: false) {
      state "ready",     label: 'Instant', action: "instant", backgroundColor: "#996600", icon:"st.security.alarm.on"      
      state "default",     label: 'Instant', action: "instant", backgroundColor: "#D0D0D0", icon:"st.security.alarm.on"      
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
    def mode = device.currentState("alarmMode")?.value
    
    
    if (mode != "ready" && (action != "disarm" && action !="force"))
 	{
   		log.debug "Cannot arm. Not in Ready State"
        return
    }
    else if (mode == "ready" && action == "disarm")
    {
    	log.debug "Already disarmed"
        return
    }
    
    parent.setArmingMode(this, action)
    
    // do we need this just to include activity?
 	sendEvent(name: "setArmingMode", value: "${action}", descriptionText: "Set alarm mode to ${action}")    
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

def setStatus(String state) {
    log.debug "Partition Status: ${state} "
    
    def eventMap = [
       '9':"notready",
       '8':"ready",
       '7':"delay",
       '3':"inalarm",
       '2':"armed"
      ]
  
  	def newState = eventMap."${state}"
  	sendEvent (name: "alarmMode", value: "${newState}")    
}

def refresh() {
	parent.refreshPartition(this)
}