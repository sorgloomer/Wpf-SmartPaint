SmartPaint application in .NET
==============================

Development
-----------

### Useful resources

#### Plugin framework
http://msdn.microsoft.com/en-us/library/ms972962.aspx
I highly suggest to leave the app.config section, and just enumerate all the DLL-s in a folder, and load the classes implementing IPlugin. This seems more flexible to me.

#### Localisation using a resource file
http://www.codeproject.com/Articles/5447/NET-Localization-using-Resource-file
