/*
 *  Paradox Open/Close Sensor
 *
 */

metadata {
  definition (name: "Paradox Open/Close Sensor", author: "tracstarr@hotmail.com", namespace: "bitbounce") {
    capability "Contact Sensor"
    command "zone"
  }

  simulator {}

  tiles {
    standardTile("zone", "device.contact", width: 2, height: 2, canChangeBackground: true, canChangeIcon: true) {
      state "open",   label: '${name}', icon: "st.contact.contact.open",   backgroundColor: "#ffa81e"
      state "closed", label: '${name}', icon: "st.contact.contact.closed", backgroundColor: "#79b821"
      state "alarm",  label: '${name}', icon: "st.contact.contact.open",   backgroundColor: "#ff0000"
    }
    
    main "zone"
	details(["zone"])
  }
}


def zone(String state) {
  // state will be a valid state for a zone (1 - open, 0-closed)
  // Since this is a motion sensor device we need to convert open to active and closed to inactive before sending the event
  def eventMap = [
   '1':"open",
   '0':"closed",
   'alarm':"alarm"
  ]
  def newState = eventMap."${state}"
  sendEvent (name: "contact", value: "${newState}")
}
