# Web interface for Microsoft DNS

## Project needs a developer and hopefully a designer to take it under their wings, most features are not production ready and were barely tested ##

Web interface for managing Microsoft DNS, written in C#, MVC and WMI.

DNSManagement wrapper supports all DNS features, Web interface supports most of what is possible through the MMC snap-ins.

Major features:

* Show EventLog
* Audit Log
* New/Edit/Delete forward lookup zones.
* New/Edit/Delete resource records.
* NSLookup/DIG
* View reverse lookup zones.
* Able to limit access only to specific servers via configuration.
* Log event types (Unauthorized access/Login/View/Change/Show logs)
* Log media types (DB/File), only DB shows in the interface.
* Log records limit.

Project contents:

* DNSManagement - LGPL licensed - C# Wrapper for all MicrosoftDNS namespace objects, full implementation.
* Heijden.DNS - CPOL licensed - full implementation of DNS lookup/DIG.
* MSDNSWebAdmin - GPL licensed - Web interface.

Credits:

* Heijden.DNS - by Alphons van der Heijden.
* Images/Icons - iconfinder.com/iconarchive.com

Programmed by Dror Gluska. 
