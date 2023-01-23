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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace toranim_code
{
    public partial class Form1 : Form
    {
        public static String[][] toranoot = new String[6][];
        public static String linkInput = @"קבצי אקסל\רשימת_תורנים.csv";
        public static String linkOutput = @"קבצי אקסל\לוח.csv";
        public static String[] days = { "ראשון", "שני", "שלישי", "רביעי", "חמישי", "שבת" };
        public static String[] OrigonDays = { "ראשון", "שני", "שלישי", "רביעי", "חמישי", "שבת" };
        public static Boolean[] hol = new Boolean[6];
        public static int indexOfDay = 0;
        public static int indexOfNames = 0;
        public static int indexOfHol = 2;
        public static int indexOfShabat = 3;
        public static int lineStart = 2;
        public static List<String> toranimList = new List<string>();
        public static Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void printToUser() // הדפסת הנתונים על המסך
        {
            TextBox t = this.massages;
            t.Text = "";
            FlowLayoutPanel[] p = new FlowLayoutPanel[6];
            p[0] = this.flowLayoutPanel1;
            p[1] = this.flowLayoutPanel2;
            p[2] = this.flowLayoutPanel3;
            p[3] = this.flowLayoutPanel4;
            p[4] = this.flowLayoutPanel5;
            p[5] = this.flowLayoutPanel6;
            for (int j = 0; j < 6; j++)
            {
                p[j].Controls.Clear();
                if (toranoot[j] != null)
                {
                    Label day = new Label();
                    day.Text = days[j];
                    day.BackColor = Color.LightGray;
                    day.TextAlign = ContentAlignment.MiddleCenter;
                    p[j].Controls.Add(day);
                    foreach (String i in toranoot[j])
                    {
                        if (i != null)
                        {
                            Label txt = new Label();
                            txt.Text = i;
                            txt.TextAlign = ContentAlignment.MiddleCenter;
                            p[j].Controls.Add(txt);
                        }
                    }
                }
            }  
        }

        private void button1_Click(object sender, EventArgs e) // הוספת שבוע אמצע סבב
        {
            if (indexOfDay > 0)
            {
                TextBox t = this.massages;
                t.Text = "אי אפשר, ניתן להוסיף עד שש תורנויות בפעם. נסה לבטל תורנות ולאחר מכן נסה שוב";
                return;
            }
            addToArray(toranoot, toran(true));
            updateHol(true);
            addToArray(toranoot, toran(true));
            updateHol(true);
            addToArray(toranoot, toran(true));
            updateHol(true);
            addToArray(toranoot, toran(true));
            updateHol(true);
            addToArray(toranoot, toran(true));
            updateHol(true);
            addToArray(toranoot, toran(false));
            updateHol(false);
            printToUser();
        }

        private void button2_Click(object sender, EventArgs e) // הוספת שבוע סוף סבב
        {
            if (indexOfDay > 1)
            {
                TextBox t = this.massages;
                t.Text = "אי אפשר, ניתן להוסיף עד שש תורנויות בפעם. נסה לבטל תורנות ולאחר מכן נסה שוב";
                return;
            }
            addToArray(toranoot, toran(true));
            updateHol(true);
            addToArray(toranoot, toran(true));
            updateHol(true);
            addToArray(toranoot, toran(true));
            updateHol(true);
            addToArray(toranoot, toran(true));
            updateHol(true);
            addToArray(toranoot, toran(true));
            updateHol(true);
            printToUser();
        }

        private void button3_Click(object sender, EventArgs e) // הוספת יום חול
        {
            if (indexOfDay > 5)
            {
                TextBox t = this.massages;
                t.Text = "אי אפשר, ניתן להוסיף עד שש תורנויות בפעם. נסה לבטל תורנות ולאחר מכן נסה שוב";
                return;
            }
            addToArray(toranoot, toran(true));
            updateHol(true);
            printToUser();
        }

        private void button4_Click(object sender, EventArgs e) // הוספת שבת
        {
            if (indexOfDay > 5)
            {
                TextBox t = this.massages;
                t.Text = "אי אפשר, ניתן להוסיף עד שש תורנויות בפעם. נסה לבטל תורנות ולאחר מכן נסה שוב";
                return;
            }
            addToArray(toranoot, toran(false));
            updateHol(false);
            printToUser();
        }

        private void button5_Click(object sender, EventArgs e) // הוספת יום עם מספר תורנים מיוחד
        {
            if (indexOfDay > 5)
            {
                TextBox t = this.massages;
                t.Text = "אי אפשר, ניתן להוסיף עד שש תורנויות בפעם. נסה לבטל תורנות ולאחר מכן נסה שוב";
                return;
            }
            int numToranim;
            if (Int32.TryParse(this.textBox1.Text, out numToranim))
            {
                addToArray(toranoot, toran(true, numToranim));
                days[indexOfDay] = "יום מיוחד";
                updateHol(true);
                printToUser();
            }
            this.textBox1.Text = "";
        }
        private void button6_Click(object sender, EventArgs e) // ביטול
        {
            if (indexOfDay>0)
            {
                toranoot[indexOfDay-1] = null;
                days[indexOfDay-1] = OrigonDays[indexOfDay - 1];
                hol[indexOfDay - 1] = true;
                indexOfDay--;
                printToUser();
            }
        }

        private void button7_Click(object sender, EventArgs e) // הוספת שבת מיוחדת
        {
            if (indexOfDay > 5)
            {
                TextBox t = this.massages;
                t.Text = "אי אפשר, ניתן להוסיף עד שש תורנויות בפעם. נסה לבטל תורנות ולאחר מכן נסה שוב";
                return;
            }
            int numToranim;
            if (Int32.TryParse(this.textBox1.Text, out numToranim))
            {
                addToArray(toranoot, toran(false, numToranim));
                days[indexOfDay] = "שבת מיוחדת";
                updateHol(false);
                printToUser();
            }
            this.textBox1.Text = "";
        }

        private void button8_Click(object sender, EventArgs e) // יצא לאקסל
        {
            int[] length = new int[toranoot.Length];
            for (int i = 0; i < toranoot.Length; i++)
            {
                if (toranoot[i] == null)
                {
                    length[i] = 0;
                }
                else
                {
                    length[i] = toranoot[i].Length;
                }
            }
            int maxLength = length.Max();
            if (maxLength == 0)
            {
                this.massages.Text= "לא נבחרו תורנים";
                return;
            }
            String[][] toranootWithDays = new String[turnArray(toranoot).Length + 1][];
            toranootWithDays[0] = days;
            for (int i = 0; i < turnArray(toranoot).Length; i++)
            {
                toranootWithDays[i + 1] = turnArray(toranoot)[i];
            }
            writerows(linkOutput, toranootWithDays, true);
            for (int i = 0; i < toranoot.Length; i++)
            {
                if (toranoot[i] != null)
                {
                    for (int j = 0; j < toranoot[i].Length; j++)
                    {
                        if (toranoot[i][j] != null)
                        {
                            update_name(toranoot[i][j], hol[i]);
                        }
                    }
                }
            }
            this.button9_Click(sender, e);
            this.massages.Text = "יוצא לאקסל בהצלחה";
        }

        public static void addToArray(String[][] s, String[] s2)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == null)
                {
                    s[i] = s2;
                    return;
                }
            }
        }

        public static void updateHol(Boolean b) // מעדכן את היום הנוכחי להיות שבת או חול
        {
            hol[indexOfDay] = b;
            if (indexOfDay < hol.Length)
            {
                indexOfDay++;
            }
        }

        public static String[][] turnArray(String[][] s)
        {
            if (s == null)
            {
                return null;
            }
            int[] length = new int[s.Length]; 
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == null)
                {
                    length[i] = 0;
                }
                else
                {
                    length[i] = s[i].Length;
                }
            }
            int maxLength = length.Max();
            String[][] s2 = new String[maxLength][];
            for (int i = 0; i < maxLength; i++)
            {
                String[] s3 = new String[s.Length];
                for (int j = 0; j < s.Length; j++)
                {
                    if (s[j] == null || s[j].Length <= i)
                    {
                        s3[j] = "";
                    }
                    else
                    {
                        s3[j] = s[j][i];
                    }
                }
                s2[i] = s3;
            }
            return s2;
        }

        public static String min(Dictionary<String, String> dict, String[][] myreader, Boolean toran_hol)
        { // מחזירה שם אקראי של אחד מהאנשים שהיו תורנים הכי מעט פעמים
            String[] list_min = new String[0];
            List<String> numList = new List<string>();
            numList.Add("-1");
            foreach (KeyValuePair<String, String> i in dict)
            {
                if ((!(numList.Contains(i.Value))) && i.Value != null && i.Value != "")
                {
                    numList.Add(i.Value);
                }
            }
            int numIndex = 0;
            Boolean flag = true;
            while (flag)
            {
                list_min = minArray(dict, myreader, toran_hol, Convert.ToInt32(numList[numIndex]));
                numIndex++;
                flag = list_min.Length == 0;
                if (numIndex>=numList.Count && flag)
                {
                    list_min = minArray(dict, myreader, toran_hol, -1, true);
                    flag = false;
                }
            }

            if (list_min.Length == 0)
            {
                list_min = new String[1];
                list_min[0] = "אין תורנים ברשימה";
                
            }
            String name = list_min[(int)(rnd.Next(1, list_min.Length) - 1)];
            return name;
        }

        public static String[] minArray(Dictionary<String, String> dict, String[][] myreader, Boolean toran_hol, int minNum, Boolean img = false)
        { // מחזירה מערך האנשים שהיו תורנים הכי מעט פעמים
            int num = 0, minT = -1;
            /*if (false)*/if (img)
            {
                int cnt = 0;
                foreach (KeyValuePair<String, String> i in dict)
                {
                    if (i.Key != null && i.Key != "" && i.Value != null && i.Value != "")
                    {
                        cnt++;
                    }
                }
                String[] list_min2 = new String[cnt];
                foreach (KeyValuePair<String, String> i in dict)
                {
                    if (i.Key != null && i.Key != "" && i.Value != null && i.Value != "")
                    {
                        list_min2[num] = (i.Key);
                        num++;
                    }
                }
                return list_min2;
            }
            else foreach (KeyValuePair<String, String> i in dict)
            {
                if (i.Value != null && i.Value != "" && i.Key != null && ((!toranimList.Contains(i.Key)) || img))
                {
                    if ((Convert.ToInt32(i.Value) < minT || minT == -1) && (Convert.ToInt32(i.Value) > minNum))
                    {
                        num = 1;
                        minT = Convert.ToInt32(i.Value);
                    }
                    else
                    {
                        if ((Convert.ToInt32(i.Value) == minT))
                        {
                            num++;
                        }
                    }
                }
            }
            String[] list_min = new String[num];
            foreach (KeyValuePair<String, String> i in dict)
            {
                if (i.Value != null && i.Key != null && i.Value != "" && i.Key != "" && i.Value == Convert.ToString(minT) && ((!toranimList.Contains(i.Key)) || img)) //&& !Contains(toranoot, i.Key))
                {
                    num--;
                    list_min[num] = (i.Key);
                }
            }
            return list_min;
        }

        public static void update_name(String name, Boolean toran_hol)
        { // מעדכנת את מספר התורנויות (חול או שבת) של התורן הנבחר בקובץ האקסל של רשימת התלמידים
            var myReader = GetCSVData(linkInput);
            int[] length = new int[myReader.Length];
            for (int i = 0; i < myReader.Length; i++)
            {
                if (myReader[i] == null)
                {
                    length[i] = 0;
                }
                else
                {
                    length[i] = myReader[i].Length;
                }
            }
            int maxLength = length.Max();
            if (myReader != null)
            {
                clear(linkInput, myReader.Length, maxLength);
            }
            foreach (String[] i in myReader)
            {
                if (i[indexOfNames] == name)
                {
                    if (toran_hol)
                    {
                        if (i[indexOfHol] == "")
                        {
                            i[indexOfHol] = "0";
                        }
                        i[indexOfHol] = (Convert.ToInt32(i[indexOfHol]) + 1) + "";
                    }
                    else
                    {
                        if (i[indexOfShabat] == "")
                        {
                            i[indexOfShabat] = "0";
                        }
                        i[indexOfShabat] = (Convert.ToInt32(i[indexOfShabat]) + 1) + "";
                    }
                }
            }
            writerows(linkInput, cutArray(myReader), true);
        }

        public static String[][] cutArray(String[][] s)
        {
            int num = 0;
            foreach (String[] s2 in s)
            {
                if (!allArrayIsNull(s2))
                {
                    num++;
                }
            }
            String[][] s3 = new String[num][];
            int i = 0;
            foreach (String[] s2 in s)
            {
                if (!allArrayIsNull(s2))
                {
                    s3[i] = s2;
                    i++;
                }
            }
            return s3;
        }

        public static Boolean allArrayIsNull(String[] s)
        {
            if (s == null || s.Length == 0)
            {
                return false;
            }
            foreach (String s2 in s)
            {
                if (s2 != null && s2 != "")
                {
                    return false;
                }
            }
            return true;
        }

        public static void clear(String filePath, int b, int a) // מוחק את כל המידע בקובץ מסוים לאורך נבחר
        {
            String[][] s = new String[a][];
            for (int j = 0; j < a; j++)
            {
                String[] s2 = new String[b];
                for (int i = 0; i < b; i++)
                {
                    s2[i] = null;
                }
                s[j] = s2;
            }
            writerows(filePath, s, false);
        }

        public static String[] toran(Boolean toran_hol, int num = -1)
        {
            int sum = num;
            if (sum == -1)
            {
                if (toran_hol)
                {
                    sum = 4;
                }
                else
                {
                    sum = 12;
                }
            }
            String[] list_names = new String[sum];
            
            for (int i = 0; i < sum; i++)
            {
                Dictionary<String, String> dict = new Dictionary<String, String>();
                var myReader = GetCSVData(linkInput);
                int x = 1;
                foreach (var m in myReader)
                {
                    if (x < lineStart)
                    {
                        x++;
                    }
                    else
                    {
                        if (toran_hol)
                        {
                            updateValue(dict, m[indexOfNames], m[indexOfHol]);
                        }
                        else
                        {
                            updateValue(dict, m[indexOfNames], m[indexOfShabat]);
                        }
                    }
                }
                var name = min(dict, myReader, toran_hol);
                toranimList.Add(name);
                list_names[i] = name;
            }
            return list_names;
        }

        public static void updateValue(Dictionary<String, String> dict, String dictKey, String dictValue) //מקבל שני ערכים ומעדכן אותם אם כבר יש מפתח זהה, או מוסיף אותם אם אין
        {
            if (!dict.ContainsKey(dictKey))
            {
                dict.Add(dictKey, dictValue);
            }
            else
            {
                dict[dictKey] = dictValue;
            }
        }

        public static String[][] addString(String[][] sa, String[] s)
        {
            if (sa == null)
            {
                String[][] sa3 = new String[1][];
                sa3[0] = s;
                return sa3;
            }
            String[][] sa2 = new String[sa.Length+1][];
            for (int i = 0; i < sa.Length; i++)
            {
                sa2[i] = sa[i];
            }
            sa2[sa.Length] = s;
            return sa2;
        }

        static string[][] GetCSVData(string filePath) // מחזיר מערך דו ממדי של קובץ CSV ממיקום מסוים
        {
            String[][] myReader = null;
            using (var reader = new StreamReader(filePath, Encoding.GetEncoding("Windows-1255")))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    myReader = addString(myReader, values);
                }
            }
            return myReader;
        }

        public static void writerows(String path, String[][] myReaderArr, bool b) //מעדכן קובץ CSV
        {            
            if (path != null && File.Exists(path));
            {
                String[] strings = new String[myReaderArr.Length];
                for (int i = 0; i < myReaderArr.Length; i++)
                {
                    strings[i] = arrayToString(myReaderArr[i]);
                }
                foreach (String s in strings)
                {
                    Boolean appendB = (b && true);
                    if (s == strings[0])
                    {
                        appendB = false;
                    }
                    using (StreamWriter writer = new StreamWriter(path, appendB, Encoding.GetEncoding("Windows-1255")))
                    {
                        writer.WriteLine(s);
                    }
                    
                }
            }
        }

        public static String arrayToString(String[] array) // ממיר מערך למחרוזת המופרדת באמצעות פסיקים
        {
            if (array == null)
                return "";
            String stringArray = "";
            if (array[0] != null)
            {
                stringArray = array[0];
            }
            for (int i = 1; i < array.Length; i++)
            {
                String s = array[i];
                if (array[i] == null)
                {
                    stringArray += ",";
                }
                else
                {
                    stringArray += "," + array[i];
                }
            }
            return stringArray;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e) // נקה הכל
        {
            while (indexOfDay > 0)
            {
                toranoot[indexOfDay - 1] = null;
                days[indexOfDay - 1] = OrigonDays[indexOfDay - 1];
                hol[indexOfDay - 1] = true;
                indexOfDay--;
            }
            printToUser();
        }
    }
}
