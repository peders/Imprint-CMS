Imprint CMS
===========

Imprint CMS is a domain specific CMS for publishers, based on ASP.net MVC 4 and LinqToSQL. It does very little, but tries to do it well.

Tools
-----

Imprint was developed using free tools from Microsoft (Visual Studio Community 2013 and SQL Server 2012 Express).

Get running
-----------

There is an empty database backup in the repository. Restore to your server of choice. The code refers by default to the SQL Express instance on localhost using Windows authentication.
Then fire up the project and run it.
To install properly, use Web Deploy to transform the web.config, then replace the connection string to match your environment.
Other settings used to be in web.config, but are now stored as configuration profiles in the database.

License
-------

Copyright 2013- Peder Skou

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
