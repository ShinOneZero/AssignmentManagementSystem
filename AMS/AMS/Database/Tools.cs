using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace AMS.Database
{
    public class Tools
    {
        public Tools()
        {

        }

        public static void CheckFileAndRemove(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
                fileInfo.Delete();
        }

        public static void CreateZIPFile(string backupFolder, string zipFilePath)
        {
            try
            {
                using (FileStream fileStream = new FileStream(zipFilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                    {
                        foreach (string filePath in Directory.EnumerateFiles(backupFolder, "*.*", SearchOption.AllDirectories))
                        {
                            string relativePath = filePath.Substring(backupFolder.Length + 1);

                            try
                            {
                                zipArchive.CreateEntryFromFile(filePath, relativePath);
                            }
                            catch (PathTooLongException)
                            {

                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("데이터베이스 파일을 찾을 수 없습니다.");
            }
        }

        public static void ExtractZIPFile(string zipFilePath, string backupFolder)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.OpenRead(zipFilePath))
                {
                    foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
                    {
                        try
                        {
                            string folderPath = Path.GetDirectoryName(Path.Combine(backupFolder, zipArchiveEntry.FullName));

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                            zipArchiveEntry.ExtractToFile(Path.Combine(backupFolder, zipArchiveEntry.FullName));
                        }
                        catch (PathTooLongException)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터베이스 파일을 찾을 수 없습니다.");
            }
        }



        public static void ExportToExcel(DataTable table)
        {
            try
            {
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Workbooks.Add();

                Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;

                // column headings
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    workSheet.Cells[1, i + 1] = table.Columns[i].ColumnName;
                }

                // rows
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (var j = 0; j < table.Columns.Count; j++)
                    {
                        workSheet.Cells[i + 2, j + 1] = table.Rows[i][j];
                    }
                }

                var excelFilePath = System.IO.Path.GetTempPath();
                if (!string.IsNullOrEmpty(excelFilePath))
                {
                    excelFilePath = excelFilePath + DateTime.Now.ToString("yyyy-MM-dd") +  "_품질업무관리_" + DateTime.Now.Ticks.ToString("x");
                    try
                    {
                        workSheet.SaveAs(excelFilePath);
                        excelApp.Quit();

                        excelApp = new Microsoft.Office.Interop.Excel.Application();
                        var workbook = excelApp.Workbooks.Open(excelFilePath);
                        var worksheet = workbook.Worksheets.get_Item(1) as Microsoft.Office.Interop.Excel.Worksheet;

                        worksheet.UsedRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        worksheet.UsedRange.Borders.Color = 0xFFC7C7C7;
                        worksheet.UsedRange.Columns.AutoFit();
                        workbook.Save();

                        excelApp.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n" + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel Error : " + ex.Message);
            }
        }
    }
}
