# Web interface for Microsoft DNS

## Project needs a developer and hopefully a designer to take it under their wings, most features are not production ready and were barely tested ##

Web interface for managing Microsoft DNS, written in C#, MVC and WMI.

DNSManagement wrapper supports most DNS features, Web interface supports most of what is possible through the MMC snap-ins.

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
* HTTPs only (feature untested, configurable).

Project contents:

* DNSManagement - LGPL licensed - C# Wrapper for most MicrosoftDNS namespace objects.
* Heijden.DNS - CPOL licensed - full implementation of DNS lookup/DIG.
* MSDNSWebAdmin - GPL licensed - Web interface.

Credits:

* Heijden.DNS - by Alphons van der Heijden.
* Images/Icons - iconfinder.com/iconarchive.com

Known issues:

* SIG RR not implemented, suspected no implementation in WMI.
* SOA Modify is changing the record, but when the changes take effect is unpredictable/unknown to me. 
* WKS Modify is not working properly.
* Zone ChangeType is inconsistent, better to delete and create a new zone.
* Active Directory options and features were not tested, perhaps in the future.
* NXT RR not implemented, suspected no implementation in WMI.
* KEY RR not implemented, suspected no implementation in WMI.

Programmed by Dror Gluska. 
