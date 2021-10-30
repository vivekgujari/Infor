using API;
using WinForms;
using System;
using System.Collections.Generic;

namespace Infor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            CRUD crud = new CRUD();
            Console.WriteLine("Do you want to upload commands in text file?\nType yes or no");
            string s = Console.ReadLine();
            if (s.Equals("yes"))
            {
                List<string> commands = ReadScript.function();
                crud.execute(commands);
            }
            else
            {
                while (true)
                {
                    string command = Console.ReadLine();
                    string[] data = command.Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                    string output = string.Empty;
                    if (data[0].ToLower().Equals("create"))
                    {
                        crud.create(data[1], data[2]);
                    }
                    else if (data[0].ToLower().Equals("update"))
                    {
                        crud.update(data[1], data[2]);
                    }
                    else if (data[0].ToLower().Equals("delete"))
                    {
                        crud.Delete(data[1]);
                    }
                    else if (data[0].ToLower().Equals("get"))
                    {
                        output = crud.get(data[1]);
                    }
                    else if (data[0].ToLower().Equals("getall"))
                    {
                        crud.GetAll();
                    }
                    else if (data[0].ToLower().Equals("quit"))
                    {
                        crud.quit();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("{0} command is not recognized", data[0]);
                    }
                 //   Console.WriteLine(output);
                }
            }
            Console.WriteLine("End of main program");
            Console.ReadKey();
        }
    }
}
