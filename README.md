# UltraLocalizer

Some issues and tips during the development of UltraLocalizer system.

# Globe.Localizer

TODO

# Globe.TranslationServer

Some notes about the porting of the old code in the new infrastructure

How to run with a specified profile
https://jporwol.wordpress.com/2017/10/23/asp-net-core-launch-profile-and-launchsettings-json/

How to publish
https://www.taithienbo.com/deploy-an-asp-net-core-application-to-iis-on-windows-server-2019/

How to manage certificates
https://devblogs.microsoft.com/aspnet/configuring-https-in-asp-net-core-across-different-platforms/
http://www.prowaretech.com/Computer/AspNetCore/ChangeKestrelServerPort

How to map View on EF Core
https://docs.microsoft.com/it-it/ef/core/what-is-new/ef-core-3.x/

How to install/update the client
https://github.com/Squirrel/Squirrel.Windows

## Notes on code under Porting folder

Each folder represents the porting of a single dll.

### UltraDBDLL

Some issues:

- In XmlManager there is the LocalizationResource defined in NewXmlFormat project. I defined LocalizationResource based on NewXmlFormat project
- What is NewXmlFormat? LocalizationResource is an auto generated file, but the project doesn't open for references problems

## Data from the database for testing purpose

All data for testing comes from the export of the original data in the database to csv files (each file has the name of the correspoding table).

### LOC_Languages Table

- LanguangeName values:
	* English
	* French
	* Italian
	* German
	* Spanish
	* Chinese
	* Russian
	* Portuguese

- ISOCoding values:

	* en
	* fr
	* it
	* de
	* es
	* zh
	* ru
	* pt

### LOC_ConceptsTable Table

|   ComponentNamespace   |   InternalNamespace   |   LocalizationID   |       Notes        |
|------------------------|-----------------------|--------------------|--------------------|
|    MeasureComponent    |       VASCULAR        |   L ICA (ICA/CCA)  |                    |
|    MeasureComponent    |       VASCULAR        |   R ICA (ICA/CCA)  |                    |
|    MeasureComponent    |       GENERIC         |   DISTANCE 1       | from 1 to 9        |
|    MeasureComponent    |       GENERIC         |   DISTANCE10       | from 10 to 18      |
|    MeasureComponent    |       VASCULAR        |   VELOCITY 1       | from 1 to 9        |
|    MeasureComponent    |       VASCULAR        |   VELOCITY10       | from 10 to 18      |

- ComponentNamespace values:
	* English
	* French
	* Italian
	* German
	* Spanish
	* Chinese
	* Russian
	* Portuguese

- ISOCoding values:

	* en
	* fr
	* it
	* de
	* es
	* zh
	* ru
	* pt

### LOC_JobList Table

Id values: 299 674 675 1233 1526 1539 1627 1653

## Questions

- In the grid of Job tab, I have Concept and Contexts columns. In xml files I have Concept tag that has tags with Context attributes.
Is Context that contains Concept or the contrary?
- If I read from xml, the filters must work? What are the job name and language?
- The Ultra Localizator makes a logout after a short period. Is there a configurable or fixed policy to handle this behaviour?

## References

### Blazor

- <https://bipinpaul.com/posts/display-spinner-on-each-api-call-automatically-in-blazor>

### Testing

- <https://asp.net-hacker.rocks/2019/01/15/unit-testing-data-access-dotnetcore.html>
- <https://www.easeus.com/sql-database-recovery/export-ms-sql-server-to-csv.html>

### Azure Sql Database

- <https://www.sqlshack.com/migrate-an-on-premises-sql-server-database-to-the-azure-sql-database/>
- <https://www.sqlcoffee.com/Troubleshooting137.htm>
- <https://www.mssqltips.com/sqlservertip/2624/script-to-find-and-drop-all-orphaned-users-in-all-sql-server-databases/>
- <https://docs.microsoft.com/en-us/sql/sql-server/failover-clusters/troubleshoot-orphaned-users-sql-server?redirectedfrom=MSDN&view=sql-server-ver15>
- <https://www.sqlshack.com/importing-a-bacpac-file-for-a-sql-database-using-ssms/>