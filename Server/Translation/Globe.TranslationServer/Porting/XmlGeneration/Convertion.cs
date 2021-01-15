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
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Globe.TranslationServer.Porting.XmlGeneration
{
    public class Convertion
    {
        //private StreamWriter _sw;
        private string _DirPath;
        private readonly LocalizationContext context;
        private readonly IConfiguration configuration;

        public Convertion(string DirPath, LocalizationContext context, IConfiguration configuration)
        {
            _DirPath = DirPath;
            this.context = context;
            this.configuration = configuration;
        }

        public void EraseOldFiles()
        {
            string[] oldfiles = System.IO.Directory.GetFiles(_DirPath);
            foreach (string oldfile in oldfiles)
                System.IO.File.Delete(oldfile);
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
            //_sw.WriteLine(mess + " " + ID);
            //_sw.Flush();
        }

        public void ReadDB()
        {
            string x = "E:\\ESAOTE\\UltraLocalizator\\DB2ULTRADB\\bin\\Debug\\Measure.en.xml";
            LocalizationResource r = LocalizationResource.Load(x);

        }
    }
}
