NPersistence configuration
--------------------------

In order to configure NPersistence, a file called 'persistence.xml' needs to be placed
in the class path and marked as 'embedded resource'.

Web site:
For web sites, all configuration needs to be placed in the 'web.config' file. In this case, the content of 
the 'persistence.xml' file needs to be added to the 'web.config' file - see http://www.npersistence.org/web-site-tutorial/step7 
for more information.

Web application:
A web application allows both configuration methods (adding a 'persistence.xml' file or embeddeing configuration within 'web.config').
You can choose which configuration method to use.

Samples:
This Folder contains 2 samples of a 'persistence.xml' file:
persistence_MYSQL_DB.xml - configuration sample with MySQL db.
persistence_SQLITE_DB.xml - configuration sample with SQLite db.

If you choose to work with one of these files, change it's name to 'persistence.xml'.