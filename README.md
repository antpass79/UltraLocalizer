# UltraLocalizer

Some issues and tips during the development of UltraLocalizer system.

# Globe.Localizer

TODO

# Globe.TranslationServer

Some notes about the porting of the old code in the new infrastructure

## Notes on code under Porting folder

Each folder represents the porting of a single dll.

### UltraDBDLL

Some issues:

- In XmlManager there is the LocalizationResource defined in NewXmlFormat project. I defined LocalizationResource based on NewXmlFormat project
- What is NewXmlFormat? LocalizationResource is an auto generated file, but the project doesn't open for references problems

## Data from the database for testing purpose

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