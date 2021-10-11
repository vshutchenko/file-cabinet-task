using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Provides output in table format.
    /// </summary>
    public class TablePrinter
    {
        private const int Margin = 2;
        private List<int> widths = new List<int>();
        private List<Alignment> alignments = new List<Alignment>();
        private List<PropertyInfo> recordProperties = typeof(FileCabinetRecord).GetProperties().ToList();
        private List<PropertyInfo> propertiesToprint = new List<PropertyInfo>();

        private IEnumerable<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="TablePrinter"/> class.
        /// </summary>
        /// <param name="records">Records to print.</param>
        /// <param name="propertiesToprintNames">Properties to print.</param>
        public TablePrinter(IEnumerable<FileCabinetRecord> records, string[] propertiesToprintNames)
        {
            this.records = records;
            this.SetTableParameters(propertiesToprintNames);
        }

        private enum Alignment
        {
            Left,
            Right,
        }

        private int Width
        {
            get
            {
                int width = 0;
                for (int i = 0; i < this.widths.Count; i++)
                {
                    width += this.widths[i] + Margin;
                }

                width += this.widths.Count + 1;
                return width;
            }
        }

        /// <summary>
        /// Prints table with records.
        /// </summary>
        public void PrintTable()
        {
            string value = string.Empty;
            List<List<string>> rows = new List<List<string>>();
            List<string> currentRow = new List<string>();

            foreach (var record in this.records)
            {
                for (int i = 0; i < this.propertiesToprint.Count; i++)
                {
                    if (this.propertiesToprint[i].PropertyType == typeof(DateTime))
                    {
                        var date = DateTime.Parse(this.propertiesToprint[i].GetValue(record).ToString());
                        value = date.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        value = this.propertiesToprint[i].GetValue(record).ToString();
                    }

                    if (value.Length > this.widths[i])
                    {
                        this.widths[i] = value.Length;
                    }

                    currentRow.Add(value);
                }

                rows.Add(new List<string>(currentRow));
                currentRow.Clear();
            }

            this.PrintHeader();

            for (int i = 0; i < rows.Count; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < rows[i].Count; j++)
                {
                    int length = 0;
                    if (rows[i][j].Length < this.widths[j])
                    {
                        length = this.widths[j] - rows[i][j].Length;
                    }

                    if (this.alignments[j] == Alignment.Left)
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

            this.PrintBorder();
        }

        private void SetTableParameters(string[] propertiesToprintNames)
        {
            for (int i = 0; i < propertiesToprintNames.Length; i++)
            {
                var prop = this.recordProperties.Find(p => p.Name.Equals(propertiesToprintNames[i], StringComparison.InvariantCultureIgnoreCase));
                if (prop != null)
                {
                    this.propertiesToprint.Add(prop);
                    this.widths.Add(prop.Name.Length);
                    if (this.propertiesToprint[i].PropertyType == typeof(string))
                    {
                        this.alignments.Add(Alignment.Left);
                    }
                    else
                    {
                        this.alignments.Add(Alignment.Right);
                    }
                }
            }
        }

        private void PrintHeader()
        {
            string line = string.Empty;
            for (int i = 0; i < this.propertiesToprint.Count; i++)
            {
                line += "| " + this.propertiesToprint[i].Name;
                if (this.propertiesToprint[i].Name.Length < this.widths[i])
                {
                    line += new string(' ', this.widths[i] - this.propertiesToprint[i].Name.Length);
                }

                line += " ";
            }

            line += "|";
            this.PrintBorder();
            Console.WriteLine(line);
            this.PrintBorder();
        }

        private void PrintBorder()
        {
            string border = "+" + new string('-', this.Width - Margin) + "+";
            Console.WriteLine(border);
        }
    }
}
