using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace API
{
    public class CRUD
    {
        private string path = "c:\\Users\\vivekgujari\\Desktop\\sample.xlsx";
        private LRUCache cache = new LRUCache();
        public string key;
        public string value;
        private int row;
        public void create(string key, string value)
        {
            if (!File.Exists(path))
            {
                createfile();
            }
            string output = "";
            var file = new Application();
            Workbook wb = file.Workbooks.Open(path);
            Worksheet ws = wb.Worksheets[1];
            Range range = ws.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            int lastrow = range.Row;
            if (!keyexists(lastrow, file, wb, ws, key))
            {
                ws.Cells[lastrow + 1, 1] = key;
                ws.Cells[lastrow + 1, 2] = value;
                wb.Save();
                wb.Close(0);
                file.Quit();
                this.key = key;
                this.value = value;
            }
            else
            {
                Console.WriteLine("{0} already exists", key);
            }
        }

        private void createfile()
        {
            var file = new Application();
            Workbook wb = file.Workbooks.Add(Missing.Value);
            Worksheet ws = wb.Sheets[1];
            ws.Cells[1, 1] = "Key";
            ws.Cells[1, 2] = "Value";
            wb.SaveAs2(path);
            wb.Close(0);
            file.Quit();
        }

        private bool keyexists(int lastrow, Application file, Workbook wb,
            Worksheet ws, string key)
        {
            for (int i = 2; i <= lastrow; i++)
            {
                string t = ((Range)ws.Cells[i, 1]).Value2;
                if (key.ToLower().Equals(t.ToLower()))
                {
                    this.row = i;
                    wb.Save();
                    wb.Close(0);
                    file.Quit();
                    return true;
                }
            }
            return false;
        }
        public void update(string key, string value)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("First create the file by adding a key value pair");
            }
            string output = "";
            var file = new Application();
            Workbook wb = file.Workbooks.Open(path);
            Worksheet ws = wb.Worksheets[1];
            Range range = ws.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            int lastrow = range.Row;
            for (int i = 2; i <= lastrow; i++)
            {
                string t = ((Range)ws.Cells[i, 1]).Value2;
                if (key.ToLower().Equals(t.ToLower()))
                {
                    ws.Cells[i, 2] = value;
                    output = value;
                    this.key = key;
                    this.value = value;
                    break;
                }
            }

            wb.Save();
            wb.Close(0);
            file.Quit();
            if (output == "")
            {
                Console.WriteLine("No key found");
                return;
            }
            if (cache.containsKey(key))
            {
                cache.dictionary[key] = value;
            }
            Console.WriteLine(key + " " + output + " updated");
        }

        public string get(string key)
        {
            if (!File.Exists(path))
            {
                return "No file found";
            }
            if (cache.containsKey(key))
            {
                Console.WriteLine(cache.dictionary[key]);
                return cache.dictionary[key];
            }
            var file = new Application();
            Workbook wb = file.Workbooks.Open(path);
            Worksheet ws = wb.Worksheets[1];
            Range range = ws.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            int lastrow = range.Row;
            string output = "";
            for (int i = 2; i <= lastrow; i++)
            {
                string t = ((Range)ws.Cells[i, 1]).Value2;
                if (key.ToLower().Equals(t.ToLower()))
                {
                    output = ((Range)ws.Cells[i, 2]).Value2;
                    break;
                }
            }
            wb.Save();
            wb.Close(0);
            file.Quit();
            if (output != "")
            {
                cache.add(key, output);
                Console.WriteLine(output);
                return output;
            }
            Console.WriteLine("No key found");
            return "No key found";

        }

        public void Delete(string key)
        {
            var file = new Application();
            Workbook wb = file.Workbooks.Open(path);
            Worksheet ws = wb.Worksheets[1];
            Range range = ws.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            int lastrow = range.Row;
            string output = string.Empty;
            for (int i = 2; i <= lastrow; i++)
            {
                string t = ((Range)ws.Cells[i, 1]).Value2;
                if (key.ToLower().Equals(t.ToLower()))
                {
                    ((Range)ws.Rows[i]).Delete();
                    output = key;
                    this.key = key;
                    break;
                }
            }
            wb.Save();
            wb.Close(0);
            file.Quit();
            if (output != key)
            {
                Console.WriteLine("{0} not found", key);
                return;
            }
            Console.WriteLine($"{key} deleted successfully");
            cache.dictionary.Remove(key);
        }

        public void GetAll()
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("no file found");
            }
            var file = new Application();
            Workbook wb = file.Workbooks.Open(path);
            Worksheet ws = wb.Worksheets[1];
            Range range = ws.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            int lastrow = range.Row;
            for (int i = 2; i <= lastrow; i++)
            {
                Console.WriteLine(((Range)ws.Cells[i, 1]).Value2 + " " + ((Range)ws.Cells[i, 2]).Value2);
            }
            wb.Save();
            wb.Close(0);
            file.Quit();
        }

        public void quit()
        {
            Environment.Exit(0);
        }

        public void execute(List<string> commands)
        {
            foreach (var command in commands)
            {
                string[] data = command.Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                string c = data[0];
                string key = string.Empty;
                string value = string.Empty;
                if (data.Length > 1)
                {
                    key = data[1];
                    for (int i = 2; i < data.Length; i++)
                    {
                        value += data[i];
                    }
                }
                string output = string.Empty;
                if (c.ToLower().Equals("create"))
                {
                    create(key, value);
                }
                else if (c.ToLower().Equals("update"))
                {
                    update(key, value);
                }
                else if (c.ToLower().Equals("delete"))
                {
                    Delete(key);
                }
                else if (c.ToLower().Equals("get"))
                {
                    output = get(key);
                }
                else if (c.ToLower().Equals("getall"))
                {
                    GetAll();
                }
                else if (c.ToLower().Equals("quit"))
                {
                    quit();
                }
                else
                {
                    Console.WriteLine("{0} command is not recognized", data[0]);
                }
                // Console.WriteLine(output);
            }
        }

    }
}
