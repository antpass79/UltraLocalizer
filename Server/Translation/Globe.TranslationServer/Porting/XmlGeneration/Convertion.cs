using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBGlobal.Models;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBStrings;
using Globe.TranslationServer.Porting.UltraDBDLL.XmlManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Porting.XmlGeneration
{
    public class Convertion
    {
        private StreamWriter _sw;
        private string _DirPath;
        private readonly LocalizationContext context;

        public Convertion(string DirPath, LocalizationContext context)
        {
            _DirPath = DirPath;
            this.context = context;
        }

        public void EraseOldFiles()
        {
            string[] oldfiles = System.IO.Directory.GetFiles(_DirPath);
            foreach (string oldfile in oldfiles)
                System.IO.File.Delete(oldfile);
        }

        public bool LocalizeDB(bool DebugMode)
        {
            _sw = new StreamWriter(_DirPath + "\\log.txt");
            bool retVal = true;
            string CurrComponent = "";
            string CurrLang = "";
            try
            {
                // legge il database: per ogni concept cerca tutti i context collegati e genera le stringhe in formato xml
                UltraDBConcept concept = new UltraDBConcept(context);
                UltraDBGlobal globaldata = new UltraDBGlobal(context);
                List<DBComponent> compList = concept.GetAllComponent();
                foreach (DBComponent component in compList)
                {
                    if (component.ComponentNamespace == "all") continue;
                    if (component.ComponentNamespace == "OLD") continue;
                    foreach (int value in System.Enum.GetValues(typeof(UltraDBStrings.Languages)))
                    {
                        LocalizationResource res = new LocalizationResource();
                        res.ComponentNamespace = component.ComponentNamespace;
                        res.Language = ((UltraDBStrings.Languages)value).ToString();
                        res.Version = (decimal)1.0;

                        CurrComponent = res.ComponentNamespace;
                        CurrLang = res.Language;
                        if (CurrComponent == "RT" && CurrLang == "it")
                            CurrLang = CurrComponent;
                        List<DBGlobal> xmlFile = globaldata.GetDataByComponentISO(component.ComponentNamespace, ((UltraDBStrings.Languages)value).ToString());
                        var sections = from p in xmlFile
                                       group new DBGlobal { LocalizationID = p.LocalizationID, ContextName = p.ContextName, DataString = p.DataString, DatabaseID = p.DatabaseID, IsAcceptable = p.IsAcceptable }
                                    by p.InternalNamespace;

                        foreach (IGrouping<string, DBGlobal> secList in sections)
                        {
                            LocalizationSection sec = new LocalizationSection();
                            sec.InternalNamespace = secList.Key == "null" ? null : secList.Key;
                            res.LocalizationSection.Add(sec);
                            List<DBGlobal> cL = secList.ToList();

                            var concepts = from p in cL
                                           group new DBGlobal { LocalizationID = p.LocalizationID, ContextName = p.ContextName, DataString = p.DataString, DatabaseID = p.DatabaseID, IsAcceptable = p.IsAcceptable }
                                           by p.LocalizationID;

                            foreach (IGrouping<string, DBGlobal> sList in concepts)
                            {
                                Concept con = new Concept();
                                con.Id = sList.Key;
                                sec.Concept.Add(con);
                                List<DBGlobal> mL = sList.ToList();
                                foreach (DBGlobal d in mL)
                                {
                                    TagString s = new TagString();
                                    s.Context = d.ContextName;
                                    s.TypedValue = d.DataString;
                                    if (DebugMode)
                                    {
                                        s.DatabaseID = d.DatabaseID;
                                        s.IsAcceptable = d.IsAcceptable;
                                    }
                                    con.String.Add(s);
                                }
                            }
                        }
                        res.Save(_DirPath + "\\" + res.ComponentNamespace + "." + res.Language + ".xml");
                    }
                }
            }
            catch (System.Exception ex)
            {
                retVal = false;
                AppendNewLog(ex.Message, "Exception:" + CurrComponent + "," + CurrLang);
            }
            finally
            {
                _sw.Close();
            }
            return retVal;
        }
        public bool ExportDBExcel()
        {
            //DataClassesGroupledEntityDataContext DBLinq = new DataClassesGroupledEntityDataContext(ConfigurationManager.ConnectionStrings["UltraDBWrapper.Properties.Settings.LocalizationConnectionString"].ConnectionString);
            _sw = new StreamWriter(_DirPath + "\\log.txt");
            bool retVal = true;
            string CurrComponent = "";
            string CurrLang = "";
            try
            {
                // legge il database: per ogni concept cerca tutti i context collegati e genera le stringhe in formato xml
                UltraDBConcept concept = new UltraDBConcept(context);
                UltraDBGlobal globaldata = new UltraDBGlobal(context);
                List<DBComponent> compList = concept.GetAllComponent();
                foreach (DBComponent component in compList)
                {
                    if (component.ComponentNamespace == "all") continue;
                    if (component.ComponentNamespace == "OLD") continue;
                    foreach (int value in System.Enum.GetValues(typeof(UltraDBStrings.Languages)))
                    {
                        LocalizationResource res = new LocalizationResource();
                        res.ComponentNamespace = component.ComponentNamespace;
                        res.Language = ((UltraDBStrings.Languages)value).ToString();
                        res.Version = (decimal)1.0;

                        CurrComponent = res.ComponentNamespace;
                        CurrLang = res.Language;
                        if (!(
                             (CurrLang == "de" ||
                              CurrLang == "es" ||
                              CurrLang == "fr" ||
                              CurrLang == "pt"
                             )
                             ))
                        {
                            continue;
                            CurrLang = CurrComponent;
                        }
                        List<DBGlobal> xmlFile = globaldata.GetDataByComponentISO(component.ComponentNamespace, ((UltraDBStrings.Languages)value).ToString());
                        var EditStringCollection = (from p in xmlFile
                                                    where !p.IsToIgnore
                                                    select new
                                                    {
                                                        Context = p.ContextName,
                                                        InternalNamespace = p.InternalNamespace,
                                                        LocalizationID = p.LocalizationID,
                                                        EngString = (from c in context.LocStrings2Context
                                                                     where c.IdstringNavigation.Idlanguage == 1 && c.Idconcept2Context == p.Concept2ContextID
                                                                     select c.IdstringNavigation.String).FirstOrDefault(),
                                                        edtStringID = p.DatabaseID,
                                                        edtString = p.DataString
                                                    }
                                                       ).OrderBy(z => z.InternalNamespace).ThenBy(z => z.LocalizationID);

                        string fileName = _DirPath + "\\" + res.ComponentNamespace + "." + res.Language + ".xlsx";
                        using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
                        {
                            // Add a WorkbookPart to the document.
                            WorkbookPart workbookPart = document.AddWorkbookPart();
                            workbookPart.Workbook = new Workbook();
                            // Add a WorksheetPart to the WorkbookPart.
                            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                            // Adding style
                            WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                            stylePart.Stylesheet = GenerateStylesheet();
                            Worksheet worksheet = new Worksheet();
                            worksheetPart.Worksheet = worksheet;
                            // Adding columns
                            Columns columns = new Columns();
                            Column A = new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 10.0D, BestFit = true, CustomWidth = true };
                            Column B = new Column() { Min = (UInt32Value)2U, Max = (UInt32Value)2U, Width = 20.0D, BestFit = true, CustomWidth = true };
                            Column C = new Column() { Min = (UInt32Value)3U, Max = (UInt32Value)3U, Width = 50.0D, BestFit = true, CustomWidth = true };
                            Column D = new Column() { Min = (UInt32Value)4U, Max = (UInt32Value)4U, Width = 50.0D, BestFit = true, CustomWidth = true };
                            Column E = new Column() { Min = (UInt32Value)5U, Max = (UInt32Value)5U, Width = 50.0D, BestFit = true, CustomWidth = true };
                            columns.Append(A);
                            columns.Append(B);
                            columns.Append(C);
                            columns.Append(D);
                            columns.Append(E);
                            worksheet.Append(columns);
                            //Append sheetdata
                            SheetData sheetData = new SheetData();
                            worksheet.AppendChild(sheetData);
                            Sheets sheets = workbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = res.ComponentNamespace + "_" + res.Language };
                            sheets.Append(sheet);
                            // Constructing header
                            UInt32 rowIndex = 0;
                            Row row = new Row { RowIndex = ++rowIndex };

                            row.Append(
                                ConstructCell("DatabaseID", CellValues.String, "A", rowIndex, 2),
                                ConstructCell("Context", CellValues.String, "B", rowIndex, 2),
                                ConstructCell("English translation", CellValues.String, "C", rowIndex, 2),
                                ConstructCell("Current translation", CellValues.String, "D", rowIndex, 2),
                                ConstructCell("New translation", CellValues.String, "E", rowIndex, 2));

                            // Insert the header row to the Sheet Data
                            sheetData.AppendChild(row);

                            // Inserting each employee
                            foreach (var DBLocalizedString in EditStringCollection)
                            {
                                if (string.IsNullOrEmpty(DBLocalizedString.edtString))
                                    continue;
                                row = new Row { RowIndex = ++rowIndex };

                                string lowercase = DBLocalizedString.edtString.ToLower();
                                char c = lowercase[0];
                                lowercase = char.ToUpper(c) + lowercase.Substring(1);
                                row.Append(
                                    ConstructCell(DBLocalizedString.edtStringID.ToString(), CellValues.Number, "A", rowIndex, 1),
                                    ConstructCell(DBLocalizedString.Context, CellValues.String, "B", rowIndex, 1),
                                    ConstructCell(DBLocalizedString.EngString, CellValues.String, "C", rowIndex, 1),
                                    ConstructCell(DBLocalizedString.edtString, CellValues.String, "D", rowIndex, 1),
                                    ConstructCell(lowercase, CellValues.String, "E", rowIndex, 1));
                                sheetData.AppendChild(row);
                            }

                            workbookPart.Workbook.Save();
                            document.Close();
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {
                retVal = false;
                AppendNewLog(ex.Message, "Exception:" + CurrComponent + "," + CurrLang);
            }
            finally
            {
                _sw.Close();
            }
            return retVal;
        }

        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }

                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }

                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = new Alignment { WrapText = true } }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true } // header
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

        private Cell ConstructCell(string value, CellValues dataType, string CellColumn, UInt32 index, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                CellReference = CellColumn + index,
                StyleIndex = styleIndex,
            };
        }
        private void AppendNewLog(string mess, string ID)
        {
            _sw.WriteLine(mess + " " + ID);
            _sw.Flush();
        }

        public void ReadDB()
        {
            string x = "E:\\ESAOTE\\UltraLocalizator\\DB2ULTRADB\\bin\\Debug\\Measure.en.xml";
            LocalizationResource r = LocalizationResource.Load(x);

        }
    }
}
