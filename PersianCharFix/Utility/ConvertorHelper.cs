using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace PersianCharFix.Utility
{
    public static class ConvertorHelper
    {
        public static string ConvertToString(IEnumerable<ValueType> value)
        {
            return string.Join("-", value);
        }

        public static string ConvertToString(DateTime value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string ConvertToString(TimeSpan value, bool includeNow = false)
        {
            return value.ToString("c", CultureInfo.InvariantCulture);
        }

        public static T ConvertTo<T>(string value) where T : new()
        {
            var type = ReflectionHelper.GetBaseType<T>();
            if (ValidationHelper.IsNumeric<T>())
            {
                value.Split('-').Select(p => p.ConvertTo<T>());
            }
            return new T();
        }

        public static int TryToInt(this object obj)
        {
            var result = 0;
            try
            {
                result = Convert.ToInt32(obj);
            }
            catch
            {
            }
            return result;
        }

        public static object ToDBNull(this object obj)
        {
            return DBNull.Value.Equals(obj) ? null : obj;
        }

        public static byte ToByte(this object obj)
        {
            return Convert.ToByte(obj);
        }

        public static bool ToBoolean(this object obj)
        {
            return Convert.ToBoolean(obj);
        }

        public static decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj);
        }

        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static int? ToNullableInt(this object obj)
        {
            int? result = null;
            if (obj != null && obj.ToString() != "")
                result = Convert.ToInt32(obj);
            return result;
        }

        public static T CastTo<T>(this object obj)
        {
            return (T)obj;
        }

        public static T ConvertTo<T>(this object obj)
        {
            return Convert.ChangeType(obj, typeof(T)).CastTo<T>();
        }
        public static object ConvertTo(this object obj, Type type)
        {
            return Convert.ChangeType(obj, type);
        }

        public static char ToASCIIchar(this int code)
        {
            return Convert.ToChar(code);
        }

        public static int ToASCIIcode(this char ch)
        {
            return Convert.ToInt32(ch);
        }

        public static string ToText(this int digit)
        {
            var txt = digit.ToString();

            var length = txt.Length;

            var a1 = new string[10] { "-", "یک", "دو", "سه", "چهار", "پنح", "شش", "هفت", "هشت", "نه" };

            var a2 = new string[10] { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };

            var a3 = new string[10] { "-", "ده", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };

            var a4 = new string[10] { "-", "یک صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفصد", "هشصد", "نهصد" };

            var result = "";

            var isDahegan = false;

            for (var i = 0; i < length; i++)
            {
                var character = txt[i].ToString();

                switch (length - i)
                {
                    case 7: //میلیون
                        if (character != "0")
                        {
                            result += a1[Convert.ToInt32(character)] + " میلیون و ";
                        }
                        else
                        {
                            result = result.TrimEnd('و', ' ');
                        }
                        break;
                    case 6: //صدهزار
                        if (character != "0")
                        {
                            result += a4[Convert.ToInt32(character)] + " و ";
                        }
                        else
                        {
                            result = result.TrimEnd('و', ' ');
                        }
                        break;
                    case 5: //ده هزار
                        if (character == "1")
                        {
                            isDahegan = true;
                        }
                        else if (character != "0")
                        {
                            result += a3[Convert.ToInt32(character)] + " و ";
                        }
                        break;
                    case 4: //هزار
                        if (isDahegan)
                        {
                            result += a2[Convert.ToInt32(character)] + " هزار و ";
                            isDahegan = false;
                        }
                        else
                        {
                            if (character != "0")
                            {
                                result += a1[Convert.ToInt32(character)] + " هزار و ";
                            }
                            else
                            {
                                result = result.TrimEnd('و', ' ');
                            }
                        }
                        break;
                    case 3: //صد
                        if (character != "0")
                        {
                            result += a4[Convert.ToInt32(character)] + " و ";
                        }
                        break;
                    case 2: //ده
                        if (character == "1")
                        {
                            isDahegan = true;
                        }
                        else if (character != "0")
                        {
                            result += a3[Convert.ToInt32(character)] + " و ";
                        }
                        break;
                    case 1: //یک
                        if (isDahegan)
                        {
                            result += a2[Convert.ToInt32(character)];
                            isDahegan = false;
                        }
                        else
                        {
                            if (character != "0") result += a1[Convert.ToInt32(character)];
                            else result = result.TrimEnd('و', ' ');
                        }
                        break;
                }
            }
            return result;
        }

        public static string ToPrice(this object dec)
        {
            var Src = dec.ToString();
            Src = Src.Replace(".0000", "");
            if (!Src.Contains("."))
            {
                Src = Src + ".00";
            }
            var price = Src.Split('.');

            if (price[1].Length >= 2)
            {
                price[1] = price[1].Substring(0, 2);
                price[1] = price[1].Replace("00", "");
            }

            string Temp = null;

            var i = 0;

            if ((price[0].Length % 3) >= 1)
            {
                Temp = price[0].Substring(0, (price[0].Length % 3));
                for (i = 0; i <= (price[0].Length / 3) - 1; i++)
                {
                    Temp += "," + price[0].Substring((price[0].Length % 3) + (i * 3), 3);
                }
            }
            else
            {
                for (i = 0; i <= (price[0].Length / 3) - 1; i++)
                {
                    Temp += price[0].Substring((price[0].Length % 3) + (i * 3), 3) + ",";
                }
                Temp = Temp.Substring(0, Temp.Length - 1);
            }
            if (price[1].Length > 0)
            {
                return Temp + "." + price[1];
            }
            return Temp;
        }
        public static string ToPriceString(this object obj)
        {
            return obj.ToString().Trim('0').Trim('.');
        }

        /// <summary>
        /// Extend any collection implementing IList to return a DataView.
        /// </summary>
        /// <param name="list">IList (Could be List<Type>)</param>
        /// <returns>DataView</returns>
        public static DataView ToDataView(this IList list)
        {
            // Validate Source
            if (list.Count < 1)
                return null;

            // Initialize DataTable and get all properties from the first Item in the List.
            var table = new DataTable(list.GetType().Name);

            var properties = list[0].GetType().GetProperties();

            // Build all columns from properties found. (Custom attributes could be added later)
            foreach (var info in properties)
            {
                try
                {
                    table.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
                catch (NotSupportedException)
                {
                    // DataTable does not support Nullable types, we want to keep underlying type.
                    table.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType)));
                }
                catch (Exception)
                {
                    table.Columns.Add(new DataColumn(info.Name, typeof(object)));
                }
            }

            // Add all rows
            for (var index = 0; index < list.Count; index++)
            {
                var row = new object[properties.Length];

                for (var i = 0; i < row.Length; i++)
                {
                    row[i] = properties[i].GetValue(list[index], null); // Get the value for each items property
                }

                table.Rows.Add(row);
            }

            return new DataView(table);
        }

        public static DataTable ExcelToTable(this string filePath, bool hasHeader)
        {
            var dt = new DataTable();

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
            {
                //Read the first Sheet from Excel file.
                Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

                //Get the Worksheet instance.
                Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                //Fetch all the rows present in the Worksheet.
                IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                //Loop through the Worksheet rows.
                foreach (Row row in rows)
                {
                    //Use the first row to add columns to DataTable.
                    if (row.RowIndex.Value == 1)
                    {
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            dt.Columns.Add(GetValue(doc, cell));
                        }
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = GetValue(doc, cell);
                            i++;
                        }
                    }
                }
                return dt;
            }
        }

        private static string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue?.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

        public static string ToCSV<T>(this IEnumerable<T> instance)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.ForEach(v => csv.AppendFormat("{0},", v));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static string ToCSV<T>(this IEnumerable<T> instance, char separator)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.ForEach(value => csv.AppendFormat("{0}{1}", value, separator));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        {
            var dtReturn = new DataTable();
            // column names
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;
            foreach (var rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = rec.GetType().GetProperties();
                    foreach (var pi in oProps)
                    {
                        var colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                var dr = dtReturn.NewRow();

                foreach (var pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null
                        ? DBNull.Value
                        : pi.GetValue
                            (rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static string ConvertUrlsToLinks(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";

            var html = new Regex(regex, RegexOptions.IgnoreCase).Replace(str, "<a href=\"$1\" target=\"&#95;blank\">$1</a><br />").Replace("href=\"www", "href=\"http://www");

            return html;
        }

        public static string SerializeJson<T>(this T input) where T : class
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(ms, input);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public static string SerializeXml<T>(this T input) where T : class
        {
            using (var writer = new StringWriter())
            {
                new XmlSerializer(typeof(T)).Serialize(writer, input);
                return writer.ToString();
            }
        }

        public static T DeserializeJson<T>(this string json) where T : class
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            var obj = ser.ReadObject(ms) as T;
            ms.Close();
            return obj;
        }

        public static T DeserializeXml<T>(this string xml) where T : class
        {
            using (var reader = new StringReader(xml))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }
    }
}