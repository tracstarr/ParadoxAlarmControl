# ParadoxAlarmControl
Control your Paradox Alarm via IP150/IP100 modules via RESTful services. Includes SmartThings integration.

To compile and run you will need to download PhantomJS.exe as it's included in the project files and necessary for tests and the main Paradox IP assembly. 

Tests also require update to match your system (LiveTests.cs).

The app.config should be updated to match your system for login.

Must use PhantomJS 2.45 and related nuget packages for selenium. The nuget package for phantomJS isn't updated to latest 2.46. 

Swagger is used to allow manually testing endpoints. It can be reached at http://localhost:8876/swagger-ui (or whatever ip/port you setup).

The Linux build is untested. You will need a PhantomJS linux build.

