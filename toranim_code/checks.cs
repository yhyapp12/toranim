using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

public class Class1
{
    public static void main(string[] args)
    {
        print(GetCSVData(@"C:\Users\ישיבת ההסדר ירוחם\Downloads\faces.csv"));
    }

    static string[][] GetCSVData(string filePath)
    {
        string[][] data;
        using (var reader = new StreamReader(filePath))
        {
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                listA.Add(values[0]);
                listB.Add(values[1]);
            }
            List<List<string>> lists = new List<List<string>>()
                {
                    listA, listB
                };
            data = lists.Select(a => a.ToArray()).ToArray();
        }
        return data;
    }
}
