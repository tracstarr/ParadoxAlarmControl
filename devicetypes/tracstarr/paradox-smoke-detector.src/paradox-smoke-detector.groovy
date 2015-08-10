/**
 *  Paradox Smoke Detector
 *
 *  Copyright 2015 tracstarr
 *
 */
metadata {
	definition (name: "Paradox Smoke Detector", namespace: "tracstarr", author: "tracstarr") {
		capability "Smoke Detector"
        command "zone"
	}

	simulator {
		// TODO: define status and reply messages here
	}

	tiles {
    	standardTile("zone", "device.smoke", width: 2, height: 2, canChangeBackground: true, canChangeIcon: true) {
      	    state("clear", label:"Clear", icon:"st.alarm.smoke.clear", backgroundColor:"#ffffff")
			state("detected", label:"Smoke!", icon:"st.alarm.smoke.smoke", backgroundColor:"#e86d13")			
    }

    main "zone"
    details(["zone"])
  }
}

// Called from SmartApp
def zone(String state) {
  // state will be a valid state for a zone (1 - smoke,0- no smoke)
  // Since this is a motion sensor device we need to convert open to active and closed to inactive before sending the event
  def eventMap = [
   '1':"detected",
   '0':"clear"   
  ]
  
  def newState = eventMap."${state}"
  sendEvent (name: "smoke", value: "${newState}")
}