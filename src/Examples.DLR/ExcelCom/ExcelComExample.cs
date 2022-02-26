using System;
using System.Runtime.InteropServices;

namespace Examples.DLR.ExcelCom
{
    internal class ExcelComExample
    {
        public static void DoCreateExcel(string filePath)
        {
            dynamic excelApp = null;
            dynamic workBooks = null;
            dynamic workBook = null;
            dynamic workSheets = null;
            dynamic workSheet = null;
            dynamic range = null;
            dynamic tableRange = null;

            dynamic cellTopLeft = null;
            dynamic cellButtomRight = null;
            dynamic range2 = null;

            try
            {
                Type excelAppType = Type.GetTypeFromProgID("Excel.Application");
                excelApp = Activator.CreateInstance(excelAppType);
                excelApp.DisplayAlerts = false;
                excelApp.Visible = true;

                workBooks = excelApp.WorkBooks;
                //workBook = workBooks.Open(filePath);
                workBook = workBooks.Add();

                workSheets = workBook.Sheets;
                workSheet = workSheets.Add();
                //workSheet = workSheets[1];
                range = workSheet.Cells;
                range[1, 1] = "Hello Excel!!";

                // 範囲一括設定
                object[,] data = CreateData();
                cellTopLeft = range[2, 1];
                cellButtomRight = range[11, 20];
                range2 = workSheet.Range[cellTopLeft, cellButtomRight];
                range2.value = data;

                // 選択した領域の値をメモリー上に格納（１セルずつ見ていくよりも早い）
                tableRange = workSheet.Range["A1", "B15"];
                object[,] values = tableRange.Value;
                DumpData(values);

                //workBook.Save();
                workBook.SaveAs(filePath);
                workBook.Close();
                excelApp.Quit();
            }
            finally
            {
                ReleaseComObject(ref range2);
                ReleaseComObject(ref cellButtomRight);
                ReleaseComObject(ref cellTopLeft);
                ReleaseComObject(ref tableRange);
                ReleaseComObject(ref range);
                ReleaseComObject(ref workSheet);
                ReleaseComObject(ref workSheets);
                ReleaseComObject(ref workBook);
                ReleaseComObject(ref workBooks);
                ReleaseComObject(ref excelApp);
            }

            Console.WriteLine($"{filePath} is created.");
            return;
        }

        private static object[,] CreateData()
        {
            object[,] data = new object[10, 20];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    data[i, j] = $"({i},{j})";
                }
            }

            return data;
        }

        private static void DumpData(object[,] data)
        {
            Console.WriteLine("--- dump ---");
            for (int i = 0; i < data.GetUpperBound(0); i++)
            {
                for (int j = 0; j < data.GetUpperBound(1); j++)
                {
                    Console.Write($"[{i},{j}] = {data[i, j]}");
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Release Com object wrapper (cast dynamic to object);
        /// </summary>
        /// <param name="com"></param>
        /// <seealso href="https://holiday-engineer.sakura.ne.jp/software/excel-error-disconnectedcontext2/"/>
        private static void ReleaseComObject(ref object com)
        {
            if (com != null) Marshal.ReleaseComObject(com);
            com = null;
        }

    }
}
