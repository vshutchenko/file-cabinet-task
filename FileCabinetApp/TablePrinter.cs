using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp
{
    public class TablePrinter
    {
        private const int Margin = 2;
        private List<int> widths = new List<int>();
        private List<Alignment> alignments = new List<Alignment>();
        private List<PropertyInfo> recordProperties = typeof(FileCabinetRecord).GetProperties().ToList();
        private List<PropertyInfo> propertiesToprint = new List<PropertyInfo>();

        private IEnumerable<FileCabinetRecord> records;

        private int Width
        {
            get
            {
                int width = 0;
                for (int i = 0; i < widths.Count; i++)
                {
                    width += widths[i] + Margin;
                }

                width += widths.Count + 1;
                return width;
            }
        }

        public TablePrinter(IEnumerable<FileCabinetRecord> records, string[] propertiesToprintNames)
        {
            this.records = records;
            SetTableParameters(propertiesToprintNames);
        }

        private enum Alignment
        {
            Left,
            Right,
        }

        private void SetTableParameters(string[] propertiesToprintNames)
        {
            for (int i = 0; i < propertiesToprintNames.Length; i++)
            {
                var prop = recordProperties.Find(p => p.Name.Equals(propertiesToprintNames[i], StringComparison.InvariantCultureIgnoreCase));
                if (prop != null)
                {
                    propertiesToprint.Add(prop);
                    widths.Add(prop.Name.Length);
                    if (propertiesToprint[i].PropertyType == typeof(string))
                    {
                        alignments.Add(Alignment.Left);
                    }
                    else
                    {
                        alignments.Add(Alignment.Right);
                    }
                }
            }
        }

        public void PrintTable()
        {
            string value = string.Empty;
            List<List<string>> rows = new List<List<string>>();
            List<string> currentRow = new List<string>();

            foreach (var record in records)
            {
                for (int i = 0; i < propertiesToprint.Count; i++)
                {
                    if (propertiesToprint[i].PropertyType == typeof(DateTime))
                    {
                        var date = DateTime.Parse(propertiesToprint[i].GetValue(record).ToString());
                        value = date.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        value = propertiesToprint[i].GetValue(record).ToString();
                    }

                    if (value.Length > widths[i])
                    {
                        widths[i] = value.Length;
                    }

                    currentRow.Add(value);
                }

                rows.Add(new List<string>(currentRow));
                currentRow.Clear();
            }

            PrintHeader();

            for (int i = 0; i < rows.Count; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < rows[i].Count; j++)
                {
                    int length = 0;
                    if (rows[i][j].Length < widths[j])
                    {
                        length = widths[j] - rows[i][j].Length;
                    }

                    if (alignments[j] == Alignment.Left)
                    {
                        line += "| " + rows[i][j] + new string(' ', length) + " ";
                    }
                    else
                    {
                        line += "| " + new string(' ', length) + rows[i][j] + " ";
                    }
                }

                line += "|";
                Console.WriteLine(line);
            }

            PrintBorder();
        }

        private void PrintHeader()
        {
            string line = string.Empty;
            for (int i = 0; i < propertiesToprint.Count; i++)
            {
                line += "| " + propertiesToprint[i].Name;
                if (propertiesToprint[i].Name.Length < widths[i])
                {
                    line += new string(' ', widths[i] - propertiesToprint[i].Name.Length);
                }

                line += " ";
            }

            line += "|";
            PrintBorder();
            Console.WriteLine(line);
            PrintBorder();
        }

        private void PrintBorder()
        {
            string border = "+" + new string('-', Width - Margin) + "+";
            Console.WriteLine(border);
        }
    }
}
