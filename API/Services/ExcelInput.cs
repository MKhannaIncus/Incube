using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;

public class ExcelReader
{
    private string _filePath;

    // Constructor to initialize the file path
    public ExcelReader()
    {
    }

    // Method to read and display the Excel file content
    public List<string> DisplayExcelContent(string _filePath)
    {
        List<string> cellValues = new List<string>();

        // Ensure the file exists
        if (!File.Exists(_filePath))
        {
            Console.WriteLine("File not found: " + _filePath);
            //return null;
        }

        // Set the license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        FileInfo fileInfo = new FileInfo(_filePath);

        // Open the file in read-only mode
        //FileStream stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        // Load the Excel file using EPPlus
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            // Access the first worksheet in the workbook
            ExcelWorksheet worksheet = null;
            ExcelWorksheets worksheets = package.Workbook.Worksheets;
            //Projections worksheet
            ExcelWorksheet worksheetProjection = package.Workbook.Worksheets["Projections"];


            for (int i = 0; i < worksheets.Count; i++)
            {
                if (worksheets[i].Name == "Projections")
                {
                    worksheet = worksheets[i];
                }
            }


            // Get the number of rows and columns in the worksheet
            var rowCount = worksheet.Dimension.Rows;
            var colCount = worksheet.Dimension.Columns;
            ExcelAddress dimension = worksheet.Dimension;

            string address = dimension.Address;

            if (dimension != null)
            {
                int startRow = dimension.Start.Row;
                int endRow = dimension.End.Row;
                int startColumn = dimension.Start.Column;
                int endColumn = dimension.End.Column;

                for (int row = startRow; row <= endRow; row++)
                {
                    for (int col = startColumn; col <= endColumn; col++)
                    {
                        var cell = worksheet.Cells[row, col];
                        cellValues.Add(cell.Text);
                        Console.WriteLine($"Cell {cell.Address} has value: {cell.Text}");
                    }
                }
            }
            else
            {
                Console.WriteLine("The worksheet is empty.");
            }



            //worksheet.Dimension;

            // Iterate through each row and column, printing the cell content
            //for (int row = 1; row <= rowCount; row++)
            //{
            //    for (int col = 1; col <= colCount; col++)
            //    {
            //        // Print the cell value, followed by a tab character
            //        //result = worksheet.Cells[row, col].Text;
            //        //object cellValue = worksheet.Cells[row, col].Value;
            //        //string cellText = worksheet.Cells[row, col].Text;
            //        //string cellFormula = worksheet.Cells[row, col].Formula;
            //        Console.Write(worksheet.Cells[row, col].Text + "\t");
            //    }
            //    // Move to the next line after each row
            //    Console.WriteLine();
            //}

        }




        return cellValues;

    }

    public List<List<string>> ReadExcel(string _filePath, string worksheet)
    {

        List<List<string>> columns = new List<List<string>> ();
        List<List<string>> collectedInformation = new List<List<string>>();


        //Set the license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        FileInfo fileInfo = new FileInfo( _filePath );
        
        //Open the file in read-only mode
        //FileStream stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        FileStream stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);



        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {

            ExcelWorksheet worksheetProjection = package.Workbook.Worksheets[worksheet];
            var rowCount = worksheetProjection.Dimension.Rows;
            var colCount = worksheetProjection.Dimension.Columns;

            //get the dimension that holds values
            ExcelAddress dimension = worksheetProjection.Dimension;
            int startRow = dimension.Start.Row;
            int endRow = dimension.End.Row;
            int startColumn = dimension.Start.Column;
            int endColumn = dimension.End.Column;

            if (dimension != null )
            {
                for( int row = startRow; row <= endRow; ++row )
                {
                    List<string> rowTraversed = new List<string>();

                    for(int column = startColumn; column <= endColumn; column++)
                    {
                        var cell = worksheetProjection.Cells[row, column];
                        var cellValue = cell.Value;

                        if (cellValue is DateTime dateValue)
                        {

                            // Format DateTime as per your requirement, e.g., "yyyy-MM-dd"
                            rowTraversed.Add(dateValue.ToString("yyyy-MM-dd"));
                        }
                        else if (cellValue is double || cell.Value is int)
                        {
                            double numericValue = Convert.ToDouble(cell.Value);

                            DateTime possibleDate = DateTime.MinValue;

                            if (numericValue >= 1 && numericValue <= 2958465) // This range filters out non-date numbers
                            {
                                possibleDate = DateTime.FromOADate(Convert.ToDouble(cell.Value));

                            }
                            //// If the value is numeric
                            //rowTraversed.Add(cell.Value.ToString());
                            //The cash rec only contains investments from the year 2017 to 2024
                            //if (possibleDate.Year >= 2017 && possibleDate.Year <= 2024)
                            //Column 17 is transaction ammount and cannot be date
                            if(column != 17 && (possibleDate.Year >= 2017 && possibleDate.Year <= 2024))
                            {
                                rowTraversed.Add(possibleDate.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                // If the numeric value isn't a valid date, treat it as a number
                                rowTraversed.Add(cell.Value.ToString());
                            }

                        }
                        else
                        {
                            // Default case for text or other types
                            rowTraversed.Add(cell.Text);
                        }
                    }

                    collectedInformation.Add(rowTraversed);

                }
            }
        }

        return collectedInformation;
    }

    public List<List<string>> DealExcel(string _filePath)
    {
        List<List<string>> collectedInformation = new List<List<string>>();

        //Set the license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        FileInfo fileInfo = new FileInfo( _filePath );

        //Open the file in read-only mode
        FileStream stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        using (ExcelPackage package = new ExcelPackage( fileInfo ) )
        {
            //ExcelWorksheet worksheetproject = package.

        }

        return collectedInformation;
    }

}
