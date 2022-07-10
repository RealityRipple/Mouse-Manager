# ![](https://realityripple.com/Software/Applications/Mouse-Manager/favicon-32x32.png) Mouse Manager
Map keyboard commands to fourth and fifth buttons on a 4-button, 5-button, or Tilt-wheel mouse.

#### Version 2.2
> Author: Andrew Sachen  
> Created: June 21, 2010  
> Updated: July 9, 2022  

Language: Visual Basic.NET  
Compiler: Visual Studio 2010  
Framework: Version 4.0+

## Building
This application can be compiled using Visual Studio 2010 or newer, however an Authenticode-based Digital Signature check is integrated into the code to prevent incorrectly-signed or unsigned copies from running. Comment out lines 5-8 of `/MouseManager/ApplicationEvents.vb` to disable this signature check before compiling if you wish to build your own copy.

This application is *not* designed to support Mono/Xamarin compilation and may not work on Linux or OS X systems. In particular, there are multiple API calls used by this application: "WinVerifyTrust", "SetWindowsHookEx", "CallNextHookEx", "UnhookWindowsHookEx", and "keybd_event". The first call is used as part of the Authenticode Digital Signature check mentioned above, the others provide the core functionality of this application. There may also be internal code which supports Windows UI-drawing methods specifically and may perform poorly or incorrectly on alternate Operating Systems.

## Download
You can grab the latest release from the [Official Web Site](https://realityripple.com/Software/Applications/Mouse-Manager/).

## License
This is free and unencumbered software released into the public domain, supported by donations, not advertisements. If you find this software useful, [please support it](https://realityripple.com/donate.php?itm=Mouse+Manager)!
