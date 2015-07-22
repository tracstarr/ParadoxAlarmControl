/*
 *  Paradox Alarm Panel integration via REST API callbacks
 *
 */

definition(
    name: "Paradox Connect",
    namespace: "bitbounce",
    author: "tracstarr",
    description: "Paradox Alarm Integration App",
    category: "My Apps",
    iconUrl: "http://pic.appvv.com/fb3361641c59ff32478ab1f0d12a9da7/175.png",
    iconX2Url: "http://pic.appvv.com/fb3361641c59ff32478ab1f0d12a9da7/175.png",
    oauth: true
)

import groovy.json.JsonBuilder

/************************************************************
*	Preferences
*/
preferences {

 	 page(name:"mainPage", title:"Paradox Connect", content: "mainPage") 
     page(name:"settingsPage", title:"Paradox Connect Settings", content: "serverSettingsPage") 
     page(name:"paradoxServiceConnectPage", title:"Paradox Service Connect", content:"paradoxServiceConnectPage", refreshTimeout:2, install:true)
     
     page(name:"deviceSelectPage", title:"Paradox Device Select", content:"deviceSelectPage")
     page(name:"partitionSelectPage", title:"Paradox Partition Select", content:"partitionSelectPage")
     
     page(name:"statusCheckPage", content:"statusCheckPage")
     page(name:"resetParadoxServerSettingsPage", content:"resetParadoxServerSettingsPage")
     page(name:"resetDeviceListPage", content:"resetDeviceListPage")
     page(name:"forceRefreshPage", content:"forceRefreshPage")
}

/************************************************************
*	REST Endpoint Definitions
*/
mappings {
  path("/zone/:id/:status") {
      action: [
          PUT: "updateZoneHandler"
      ]
  }
  path("/link") {
      action: [
          GET: "linkHandler",
      ]
   }
   path("/revoke") {
       action: [
           GET: "revokeHandler",
       ]
   }
   path("/partition/:id/:status") {
       action: [
           PUT: "partitionHandler",
       ]
   }
}

/************************************************************
*	Install/Uninstall/Updated
*/
def installed() {
	DEBUG( "Installed with settings: ${settings}")
	initialize()
}

def updated() {
	DEBUG("Updated with settings: ${settings}")

	unsubscribe()
	initialize()
}

def initialize() {
	 DEBUG("Initialize")
     
     subscribe(location, null, lanHandler, [filterEvents:false])        
}

def uninstalled() {
	api("reset",null)
    removeChildDevices(getChildDevices())
}

private removeChildDevices(delete) {
    delete.each {
        deleteChildDevice(it.deviceNetworkId)
    }
}
/************************************************************
*	Pages
*/

def mainPage()
{	
	return dynamicPage(name:"mainPage", title: "Paradox Connect", uninstall: state.isLinked, install: false) {
    			section() {
                	href ("settingsPage", title: state.isLinked? "Server Settings":"Connect", description:"Paradox server settings.")
                }
                
                if (state.isLinked)
                {                
                	UpdateSelectedDevices()
                 	
                	section() {
                		href ("partitionSelectPage", title: "Partitions", description: "Select the Partitions you wish to control.")                       
                        href ("deviceSelectPage", title: "Devices", description: "Select the devices you want to control.")
                    }
                }
                else
                {
                	section() {
                		paragraph ("No link has been established to your Paradox server. Please click Connect.")
                    }
                }
            }

}

def serverSettingsPage()
{
	return dynamicPage(name:"settingsPage", title: "Paradox Connect Settings", uninstall:false, install:false){
    		section() {
            	paragraph "Please enter your Paradox Service Endpoint details."
                input("ip", "string", title:"IP Address", description: "IP Address", required: true, defaultValue: "192.168.20.148")
                input("port", "string", title:"Port", description: "Port", defaultValue: 8876 , required: true)
                                
                if (state.isLinked)
                {                	          	
                    href ("statusCheckPage", title: "Status Check", description: "Check if your Paradox Controller is connected and running correctly.")
                    href ("resetParadoxServerSettingsPage", title: "Reset", description: "Reset your Paradox server settings and oAuth.")                        
                    href ("resetDeviceListPage", title: "Reset Device List", description: "Refetch devices.")                        
                    href ("forceRefreshPage", title: "Refresh all device statuses", description: "Refresh the status of all devices.")        
                }
                else
                {
                	href ("paradoxServiceConnectPage",name:"serviceConnect", title:"Link", description:"Try to link to your local Paradox server.")
                }
                 
            }            
            
        }
}

def partitionSelectPage()
{
	if (state.gotPartitions)
    {  
        def partitionOptions = partitionsDiscovered() ?: []
        def numPartitions = partitionOptions.size() ?: 0

		return dynamicPage(name:"partitionSelectPage", title:"Partition Selection", nextPage:"") {
			section("Select your device below.") {
            	input "selectedPartitions", "enum", required:false, title: "Select Partitions (${numPartitions} found)", multiple: true, options: partitionOptions
			}		
		}   
    }
    
    if (!state.waitOnRestCall)
    {
		api("partitions",null)	
    }
    
	return dynamicPage(name:"partitionSelectPage", title:"Partition Discovery Started!", nextPage:"", refreshInterval:3) {
		section() {
				paragraph "Getting partitions..."
			}	
	}   
    
}

def deviceSelectPage()
{
	if (state.gotDevices)
    {
        def ocOptions = openCloseDiscovered() ?: []
        def numOcFound = ocOptions.size() ?: 0

        def motionOptions = motionDiscovered() ?: []
        def numMotionFound = motionOptions.size() ?: 0
        
        def smokeOptions = smokeDetectorDiscovered() ?: []
        def smokeFound = smokeOptions.size() ?: 0
     
     	return dynamicPage(name:"deviceSelectPage", title:"Device Selection", nextPage:"") {
			section("Select your device below.") {
        		input "selectedOpenClose", "enum", required:false, title:"Select Open/Close Sensors (${numOcFound} found)", multiple:true, options:ocOptions
				input "selectedMotion", "enum", required:false, title:"Select Motion Sensors (${numMotionFound} found)", multiple:true, options:motionOptions
                input "selectedSmoke", "enum", required:false, title:"Select Smoke Detectors (${smokeFound} found)", multiple:true, options:smokeOptions
			}		
		}   
    }
    
    if (!state.waitOnRestCall)
    {
		api("deviceList",null)	
    }
    
	return dynamicPage(name:"deviceSelectPage", title:"Device Discovery Started!", nextPage:"", refreshInterval:3) {
		section() {
				paragraph "Getting device lists..."
			}	
	}   
    
}

def paradoxServiceConnectPage()
{
	//TRACE("${state}")
	if (!state.isLinked)
	{    	
        if (!state.connectingToService)
     	{        	
            paradoxHubConnect()
    		TRACE("Waiting for response from Paradox local service.")
            state.connectingToService = true
    	}
        
        state.connectingToService = false
        return dynamicPage(name:"paradoxServiceConnectPage", title:"Connecting to Paradox Service",refreshInterval:3) {
                section() {
                    paragraph "Please Wait"
                }
            }
    }
    else
    {
    	TRACE("Connected to Paradox Service !")
        state.connectingToService = false
        return dynamicPage(name:"paradoxServiceConnectPage", title:"Connecting to Paradox Service", install:true, uninstall: false) {
                section() {
                    paragraph "Connected. Please click Done."
                }
            }
    }
}

def resetDeviceListPage ()
{
	state.gotDevices = false
    state.gotPartitions = false
    state.waitOnRestCall = false
    state.motionSensors = null
    state.partitions = null
    state.openCloseDevices = null
    state.smokeSensors = null
    
    return dynamicPage(name:"resetDeviceListPage", title:"Reset Devices") {
			section() {
				paragraph "Reset..."
			}
		}
}

def forceRefreshPage()
{
	api("fullrefresh",null)
	return dynamicPage(name:"forceRefreshPage", title:"Force Refresh") {
			section() {
				paragraph "Done"
			}
		}
}

def resetParadoxServerSettingsPage()
{
	return dynamicPage(name:"resetParadoxServerSettingsPage", title:"Reset All") {
			section() {
				paragraph "Not Implemented..."
			}
		}
}

def statusCheckPage()
{   	
    if (!state.statusCheck)
    {
      api("status",null)
      state.statusCheck = true
      return dynamicPage(name:"statusCheckPage", title:"Status Check", refreshInterval:3) {
			section() {
				paragraph "Getting Status..."
			}
		}
	}
    
    state.statusCheck = false
    
    if (!state.isLinked)
    {
        return dynamicPage(name:"statusCheckPage", title:"Status"){
    		section() {
            	paragraph "Not linked"
            }
        }	
    }
    
    return dynamicPage(name:"statusCheckPage", title:"Status"){
    		section() {
            	paragraph "Connected"
            }
    }
}

/************************************************************
*	REST Handlers
*/
void updateZoneHandler() {
  updateZone()
}

def linkHandler()
{	
    if (state.isLinked)
    {
    	return [result: "already connected"]
    }
   
   	state.isLinked = true
    state.waitOnRestCall = false
   	return [result  : "ok"]
}

def revokeHandler()
{
    INFO("Paradox service requested revoking access")
    state.isLinked = false
    state.waitOnRestCall = false
    return [result  : "ok"]
}

def partitionHandler()
{
	updatePartition()
}

/************************************************************
*	ST subscription Handlers
*/

def lanHandler(evt) {
	def description = evt.description
    def hub = evt?.hubId

	def parsedEvent = parseEventMessage(description)
	parsedEvent << ["hub":hub]
    
    if (parsedEvent.body && parsedEvent.headers)
    {
    try
    {
    	def headerString = new String(parsedEvent.headers.decodeBase64())
        
        if (!headerString.contains("Paradox HttpListener"))
        {
            WARN("Not a message from Paradox Services")
            return
        }

		def bodyString = new String(parsedEvent.body.decodeBase64())
        
        def body = new groovy.json.JsonSlurper().parseText(bodyString)
        if (body?.errorCode)
        {
        	ERROR("[${body?.errorCode}] ${body?.message}")
		}
        else
        {
        	if (body?.action?.equalsIgnoreCase("status"))
            {               	
                state.isLinked = body?.isOk
            }
            else if (body?.action?.equalsIgnoreCase("devices"))
            {           
                body?.devices.each { 
                	
                    def d = null
                    
                	if (it?.deviceType?.equalsIgnoreCase("OpenCloseSensor"))
                    {    
                    	d = getOpenCloseDevices()                        
                    }
                    else if (it?.deviceType?.equalsIgnoreCase("MotionSensor"))
                    {       
                     	d = getMotionDevices()                		
                    }
                    else if (it?.deviceType?.equalsIgnoreCase("SmokeDetector"))
                    {
                    	d = getSmokeDetectorDevices()                		
                    }
                    else
                    {
                    	DEBUG("Ignoring current device type. " + it?.deviceType)
                    }
                                	
                	if (d != null)
                    {
                    	DEBUG("Adding to device list")
                    	d[it?.id] = [id: it.id, name: it.name, deviceType: it.deviceType, hub: parsedEvent.hub]   
                    }
                }
                
                state.gotDevices = true
            }           
            else if (body?.action?.equalsIgnoreCase("partitions"))
            {           
                body?.partitions.each { 
                	
                    def d = getPartitions()
                                	
                	if (d != null)
                    {
                    	DEBUG("Adding to partition list")
                    	d[it?.id] = [id: it.id, name: it.name, hub: parsedEvent.hub]   
                    }
                }
                
                state.gotPartitions = true
            }           
            
        }
      	
        } catch(Exception e)
        {
        	ERROR(e)
        } finally
        {
        	state.waitOnRestCall = false   
        }
    }
}

private def parseEventMessage(String description) {
	def event = [:]
	def parts = description.split(',')
	parts.each { part ->
		part = part.trim()
		if (part.startsWith('devicetype:')) {
			def valueString = part.split(":")[1].trim()
			event.devicetype = valueString
		}
		else if (part.startsWith('mac:')) {
			def valueString = part.split(":")[1].trim()
			if (valueString) {
				event.mac = valueString
			}
		}
		else if (part.startsWith('networkAddress:')) {
			def valueString = part.split(":")[1].trim()
			if (valueString) {
				event.ip = valueString
			}
		}
		else if (part.startsWith('deviceAddress:')) {
			def valueString = part.split(":")[1].trim()
			if (valueString) {
				event.port = valueString
			}
		}		
		else if (part.startsWith('headers')) {
			part -= "headers:"
			def valueString = part.trim()
			if (valueString) {
				event.headers = valueString
			}
		}
		else if (part.startsWith('body')) {
			part -= "body:"
			def valueString = part.trim()
			if (valueString) {
				event.body = valueString
			}
		}
        else if (part.startsWith('requestId')) {
			part -= "requestId:"
			def valueString = part.trim()
			if (valueString) {
				event.requestId = valueString
			}
		}
	}

	event
}
/************************************************************
*	API and http methods
*/

private def getHostAddress() {
    return "${ip}:${port}"
}

private String convertIPtoHex(ipAddress) { 
    String hex = ipAddress.tokenize( '.' ).collect {  String.format( '%02x', it.toInteger() ) }.join()
    return hex
}

private String convertPortToHex(port) {
    String hexport = port.toString().format( '%04x', port.toInteger() )
    return hexport
}

def toJson(Map m)
{
	return new org.codehaus.groovy.grails.web.json.JSONObject(m).toString()
}

def toQueryString(Map m)
{
	return m.collect { k, v -> "${k}=${URLEncoder.encode(v.toString())}" }.sort().join("&")
}

private def api(method, args) {
		
    def methods = [
		'status': 
			[uri:"/smartthings/status", 
          		type: 'get'],
		'configure': 
			[uri:"/configure", 
          		type: 'put'],
		'reset': 
			[uri:"/configure/reset", 
          		type: 'get'],
        'deviceList': 
			[uri:"/devices", 
          		type: 'get'],
        'partitions': 
			[uri:"/partitions", 
          		type: 'get'],
        'set':
        	[uri:"/setpartitionmode",
            	type: 'put'],
        'refreshPartition':
        	[uri:"/refresh/partition?" + toQueryString(args),
            	type: 'get'],
        'fullrefresh':
        	[uri:"/forcestatusrefresh",
            	type: 'get'],
		]
        
	def request = methods.getAt(method)
 	
    state.waitOnRestCall = true
    
    try {
		
		if (request.type == 'put') {
			putapi(args,request.uri)
		} else if (request.type == 'get') {
			getapi(request.uri)
		} 
	} catch (Exception e) {
		ERROR("doRequest> " + e)		
	}    	
}

private getapi(uri) {
  
  def hubAction = new physicalgraph.device.HubAction(
    method: "GET",
    path: uri,
    headers: [	HOST:getHostAddress(),
    			"Accept":"application/json"
                ]
  )
  
  sendHubCommand(hubAction)  
}

private putapi(params, uri) {
		
	def hubAction = new physicalgraph.device.HubAction(
		method: "PUT",
		path: uri,
		body: toJson(params),
		headers: [Host:getHostAddress(), "Content-Type":"application/json" ]
		)
	sendHubCommand(hubAction)    
}
/************************************************************
*	Functions
*/
def setArmingMode(partitionDevice, mode)
{	
	def pId = partitionDevice?.device?.deviceNetworkId[-1..-1]
	def params = ["PartitionId": "${pId}","Mode" : "${mode}"]
    api("set", params)    
}

def refreshPartition(partitionDevice)
{	
    def pId = partitionDevice?.device?.deviceNetworkId[-1..-1]
	def params = ["PartitionId": "${pId}"]
    api("refreshPartition", params)
}

def deleteChildren(selected, existing, dniPostfix)
{
	// given all known devices, search the list of selected ones, if the device isn't selected, see if it exists as a device, if it does, remove it.
    existing.each { device ->
    	def dni = app.id + dniPostfix + device.value.id
        def sel
        
        if (selected)
        {
        	sel = selected.find { dni == it }            
        }
        
        if (!sel)
        {
        	def d = getChildDevice( dni )
            if (d)
            {
            	DEBUG("Deleting device " + dni )
           		deleteChildDevice( dni )
            }        	
        }
    }
}

def DeleteChildDevicesNotSelected()
{
	deleteChildren(selectedOpenClose, getOpenCloseDevices(), "/zone")
    deleteChildren(selectedMotion, getMotionDevices(), "/zone")
	deleteChildren(selectedSmoke, getSmokeDetectorDevices(), "/zone")
    deleteChildren(selectedPartitions, getPartitions(), "/partition")
}

def UpdateSelectedDevices()
{
	DeleteChildDevicesNotSelected()    
    	
    createNewDevices(selectedOpenClose, getOpenCloseDevices(), "Paradox Open/Close Sensor", "/zone")
    createNewDevices(selectedMotion, getMotionDevices(), "Paradox Motion Sensor", "/zone")
    createNewDevices(selectedSmoke, getSmokeDetectorDevices(), "Paradox Smoke Detector", "/zone") 
    createNewDevices(selectedPartitions, getPartitions(), "Paradox Alarm Panel", "/partition")
}

private def createNewDevices(selected, existing, deviceType, dniPostfix)
{
	if (selected)
    {    	
     	selected.each { dni ->
        	def d = getChildDevice(dni)
            if (!d)
            {
            	def newDevice
            	newDevice = existing.find { (app.id + dniPostfix + it.value.id) == dni}
                d = addChildDevice("bitbounce",deviceType, dni, newDevice?.value.hub, [name: newDevice?.value.name])
                DEBUG("Created new " + deviceType)
            }           
        }      
    }
}

def generateAccessToken() {
    
    if (resetOauth) {
    	DEBUG( "Reseting Access Token")
    	state.accessToken = null
        resetOauth = false
    }
    
	if (!state.accessToken) {
    	createAccessToken()
        TRACE( "Creating new Access Token: $state.accessToken")
    }
  
}

private paradoxHubConnect()
{
	DEBUG("Connecting to Paradox Local Hub")
    
    generateAccessToken()
      
    def params = ["Location": "Home","AppId" : "${app.id}", "AccessToken" : "${state.accessToken}"]
    api("configure", params)
 
}

Map openCloseDiscovered() {
	def sensors =  getOpenCloseDevices()
	def map = [:]
	
    sensors.each {
        def value = "${it?.value?.name}"
        def key = app.id +"/zone"+ it?.value?.id
        map["${key}"] = value
    }
	
	map
}

Map motionDiscovered() {
	def motion =  getMotionDevices()
	def map = [:]
	
    motion.each {
        def value = "${it?.value?.name}"
        def key = app.id +"/zone"+ it?.value?.id 
        map["${key}"] = value
    }

	map
}

Map smokeDetectorDiscovered() {
	def smoke =  getSmokeDetectorDevices()
	def map = [:]
	
    smoke.each {
        def value = "${it?.value?.name}"
        def key = app.id +"/zone"+ it?.value?.id 
        map["${key}"] = value
    }

	map
}

Map partitionsDiscovered() {
	def smoke =  getPartitions()
	def map = [:]
	
    smoke.each {
        def value = "${it?.value?.name}"
        def key = app.id +"/partition"+ it?.value?.id 
        map["${key}"] = value
    }

	map
}

def getPartitions() {
	state.partitions = state.partitions ?: [:]
}

def getOpenCloseDevices() {
	state.openCloseDevices = state.openCloseDevices ?: [:]
}

def getMotionDevices() {
	state.motionSensors = state.motionSensors ?: [:]
}

def getSmokeDetectorDevices() {
	state.smokeSensors = state.smokeSensors ?: [:]
}

/************************************************************
*	Alarm Functions
*/
private updateZone() 
{	
	def zoneId = params.id
    def zoneStatus = params.status

	def childDevice = getChildDevice((app.id + "/zone" + zoneId))
    
    if (childDevice)
    {
    	childDevice.zone("${zoneStatus}")                
    }
    
}

private updatePartition()
{
	def partitionId = params.id
    def partitionStatus = params.status
    
    def childDevice = getChildDevice((app.id + "/partition" + partitionId))
    
    if (childDevice)
    {
    	childDevice.setStatus("${partitionStatus}")                
    }
}

/************************* DEBUGGING **********************/

private TRACE(message)
{
	log.trace(message)
}

private DEBUG(message)
{
	log.debug(message)
}

private WARN(message)
{
	log.warn(message)
}

private ERROR(message)

{
	log.error(message)
}