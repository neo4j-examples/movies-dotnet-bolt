# Neo4j Movies Example Application - Asp.net WebApi Version

### Requirements
* Have Neo4j running with the Movies Dataset
* Visual Studio 2015

### Stack
* Neo4j.Driver [GitHub](https://github.com/neo4j/neo4j-dotnet-driver)
* Asp.NET WebApi

### Run locally
* Start your local Neo4j Server [Download & Install Neo4j 3.2](https://neo4j.com/download/?ref=home) 
* Disable authentication by setting `dbms.security.auth_enabled=false` in config.log
```
Call `/seed` to populate the database
OR
Open the [Neo4j Browser](http://localhost:7474)
Install the Movies data-set with `:play movies`, click the statement, and hit the triangular "Run" button
```
* Open up the solution in Visual Studio and restore the Nuget Packages. 
* Press F5 to run the project, this will start your browser of choice and show you the interface.

