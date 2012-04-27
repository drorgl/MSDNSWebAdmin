

Check MSDNSWebAdmin.tdl for task list.

uses the www.abstractspoon.com - ToDoList

Known Issues:
- SIG RR not implemented, suspected no implementation in WMI.
- SOA Modify is changing the record, but when the changes take effect is unpredictable/unknown to me. 
- WKS Modify is not working properly.
- Zone ChangeType is inconsistent, better to delete and create a new zone.
- Active Directory options and features were not tested, perhaps in the future.
- NXT RR not implemented, suspected no implementation in WMI.
- KEY RR not implemented, suspected no implementation in WMI.
- Many tests were implemented as "no exception is good", need to implement more thorough tests to actually verify functionality.