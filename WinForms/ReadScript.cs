using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WinForms
{
    public static class ReadScript
    {
        public static List<string> function()
        {
            List<string> filecontent = new List<string>();

            using (OpenFileDialog filedialog = new OpenFileDialog())
            {
                filedialog.InitialDirectory = "c:\\";
                filedialog.Filter = "text files|*.txt";
                if (filedialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string line in File.ReadLines(filedialog.FileName))
                    {
                        filecontent.Add(line);
                    }
                }
            }
            return filecontent;
        }
    }
}
