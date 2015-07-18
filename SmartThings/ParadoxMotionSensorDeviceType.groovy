/*
 * Paradox Motion Device
 */

metadata {
  definition (name: "Paradox Motion Sensor", author: "tracstarr <tracstarr@hotmail.com>", namespace:"bitbounce") {
    capability "Motion Sensor"
    capability "Sensor"
    command "zone"
  }

  simulator {
  }

  tiles {
    standardTile("zone", "device.motion", width: 2, height: 2, canChangeBackground: true, canChangeIcon: true) {
      state("active",   label:'motion',    icon:"st.motion.motion.active",   backgroundColor:"#53a7c0")
      state("inactive", label:'no motion', icon:"st.motion.motion.inactive", backgroundColor:"#ffffff")
      state("alarm",    label:'ALARM',     icon:"st.motion.motion.active",   backgroundColor:"#ff0000")
    }

    main "zone"
    details(["zone"])
  }
}

// Called from SmartApp
def zone(String state) {
  // state will be a valid state for a zone (1 - active,0- inactive)
  // Since this is a motion sensor device we need to convert open to active and closed to inactive before sending the event
  def eventMap = [
   '1':"active",
   '0':"inactive",
   'alarm':"alarm"
  ]
  
  def newState = eventMap."${state}"
  sendEvent (name: "motion", value: "${newState}")
}
