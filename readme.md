# Neo4j Movies Example Application - Asp.net WebApi Version

#PRE-RELEASE

This uses the **alpha** version of the database, it is in no way stable!

### Requirements
* Have Neo4j running with the Movies Dataset [Download & Install 3.0 ALPHA](http://alpha.neohq.net/)
* Visual Studio 2015

### Stack
* Neo4j.Driver [GitHub](https://github.com/neo4j/neo4j-dotnet-driver), [Nuget - Milestone](http://nuget.org/packages/Neo4j.Driver), [MyGet - Snapshot]
* Asp.NET WebApi

### Run locally
* Start your local Neo4j Server  [Download & Install 3.0 ALPHA](http://alpha.neohq.net/), open the [Neo4j Browser](http://localhost:7474). Then install the Movies data-set with `:play movies`, click the statement, and hit the triangular "Run" button
* Open up the solution in Visual Studio and restore the Nuget Packages. 

**WARNING** This version relies on the SnapShot version of the driver (until **M02** is available). You will need to add the snapshot nuget feed (https://www.myget.org/F/neo4j-driver-snapshots/api/v3/index.json) to your package feeds.

* Press F5 to run the project, this will start your browser of choice and show you the interface.

