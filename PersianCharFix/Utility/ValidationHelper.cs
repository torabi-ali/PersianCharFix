using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PersianCharFix.Utility
{
    public static class ValidationHelper
    {
        public static bool IsNullable<T>()
        {
            Type type = typeof(T);
            if (!type.IsValueType) return true;
            return Nullable.GetUnderlyingType(type) != null;
        }
        public static bool IsNumeric<T>()
        {
            var type = ReflectionHelper.GetBaseType<T>();
            var list = new List<Type> {
                typeof(byte),
                typeof(int),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(decimal)
            };
            return list.Contains(type);
        }

        public static bool IsInteger<T>(T value)
        {
            return (value is sbyte || value is short || value is int
                    || value is long || value is byte || value is ushort
                    || value is uint || value is ulong);
        }

        public static bool IsFloat<T>(T value)
        {
            return (value is float | value is double | value is decimal);
        }

        public static bool IsNumeric<T>(T value)
        {
            return IsInteger(value) || IsFloat(value);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        public static bool IsInt(this string str)
        {
            try
            {
                Convert.ToInt32(str.Replace(",", ""));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsByte(this string str)
        {
            try
            {
                Convert.ToByte(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDecimal(this string str)
        {
            try
            {
                Convert.ToDecimal(str.Replace('/', '.'));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsEnglish(this string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z0-9]+$");
        }

        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        public static bool IsMobile(this string str)
        {
            // simple : ^09\d{9}$
            return Regex.IsMatch(str, @"^((0|\+98)?([ ]|,|-|[()]){0,2}9[0-9]([ ]|,|-|[()]){0,2}(?:[0-9]([ ]|,|-|[()]){0,2}){8})$");
        }

        public static bool IsTimeSpan12(this string str)
        {
            return Regex.IsMatch(str, @"^(1[012]|[1-9]):([0-5]?[0-9]) (AM|am|PM|pm)$");
        }

        public static bool IsTimeSpan12P(this string str)
        {
            return Regex.IsMatch(str, @"^(1[012]|[1-9]):([0-5]?[0-9]) (ق ظ|ق.ظ|ب ظ|ب.ظ)$");
        }

        public static bool IsTimeSpan24hhm(this string str)
        {
            return Regex.IsMatch(str, @"^([01][0-9]|2[0-3]):([0-5]?[0-9])$");
        }

        public static bool IsTimeSpan24hm(this string str)
        {
            return Regex.IsMatch(str, @"^(2[0-3]|[01]?\d):([0-5]?[0-9])$");
        }

        public static bool IsPersianDateTime(this string str)
        {
            return Regex.IsMatch(str, @"^(13\d{2}|[1-9]\d)/(1[012]|0?[1-9])/([12]\d|3[01]|0?[1-9]) ([01][0-9]|2[0-3]):([0-5]?[0-9])$");
        }

        public static bool IsTime(this string str)
        {
            return Regex.IsMatch(str, @"^([01][0-9]|2[0-3]):([0-5]?[0-9])$");
        }

        public static bool IsPersianDate(this string str)
        {
            return Regex.IsMatch(str, @"^(13\d{2}|[1-9]\d)/(1[012]|0?[1-9])/([12]\d|3[01]|0?[1-9])$");
        }

        public static bool IsPersianMonthDate(this string str)
        {
            return Regex.IsMatch(str, @"^(13\d{2}|[1-9]\d)/(1[012]|0?[1-9])$");
        }

        public static bool IsNationalCode(this string nationalcode)
        {
            if (string.IsNullOrEmpty(nationalcode)) return false;
            if (!new Regex(@"\d{10}").IsMatch(nationalcode)) return false;
            var array = nationalcode.ToCharArray();

            var allDigitEqual = new[]
                {
                "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666",
                "7777777777", "8888888888", "9999999999"
            };

            if (allDigitEqual.Contains(nationalcode)) return false;
            var j = 10;

            var sum = 0;

            for (var i = 0; i < array.Length - 1; i++)
            {
                sum += int.Parse(array[i].ToString(CultureInfo.InvariantCulture)) * j;
                j--;
            }
            var div = sum / 11;

            var r = div * 11;

            var diff = Math.Abs(sum - r);

            if (diff <= 2)
            {
                return diff == int.Parse(array[9].ToString(CultureInfo.InvariantCulture));
            }
            var temp = Math.Abs(diff - 11);
            return temp == int.Parse(array[9].ToString(CultureInfo.InvariantCulture));
        }

        public static bool IsNationalCode(this string nationalcode, out string msg)
        {
            try
            {
                var chArray = nationalcode.Trim().ToCharArray();

                var numArray = new int[chArray.Length];

                for (var i = 0; i < chArray.Length; i++)
                {
                    numArray[i] = (int)char.GetNumericValue(chArray[i]);
                }
                var num2 = numArray[9];

                switch (nationalcode.Trim())
                {
                    case "0000000000":
                    case "1111111111":
                    case "22222222222":
                    case "33333333333":
                    case "4444444444":
                    case "5555555555":
                    case "6666666666":
                    case "7777777777":
                    case "8888888888":
                    case "9999999999":
                        msg = "کد ملی وارد شده صحیح نمی باشد";
                        return false;
                }
                var num3 = (numArray[0] * 10) + (numArray[1] * 9) + (numArray[2] * 8) + (numArray[3] * 7) + (numArray[4] * 6) + (numArray[5] * 5) + (numArray[6] * 4) + (numArray[7] * 3) + (numArray[8] * 2);
                var num4 = num3 - ((num3 / 11) * 11);

                if (((num4 == 0) && (num2 == num4)) || ((num4 == 1) && (num2 == 1)) || ((num4 > 1) && (num2 == Math.Abs(num4 - 11))))
                {
                    msg = "کد ملی صحیح می باشد";
                    return true;
                }
                msg = "کد ملی نامعتبر است";
                return false;
            }
            catch (Exception)
            {
                msg = "لطفا یک عدد 10 رقمی وارد کنید";
                return false;
            }
        }

        public static bool IsUnicode(this string str)
        {
            foreach (var ch in str)
            {
                if (ch > 255) return true;
            }
            return false;
        }

        public static bool IsDigit(this string str)
        {
            return str.All(char.IsDigit);
        }

        public static bool IsTimeSpan(this string str)
        {
            var ts = new TimeSpan();
            return TimeSpan.TryParse(str, out ts);
        }

        public static bool IsUrl(this string str)
        {
            var rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(str);
        }

        public static bool IsFarsi(this string Str)
        {
            //char[] AllowChr = new char[47] { 'آ', 'ا', 'ب', 'پ', 'ت', 'ث', 'ج', 'چ', 'ح', 'خ', 'د', 'ذ', 'ر', 'ز', 'ژ', 'س', 'ش', 'ص', 'ض', 'ط', 'ظ', 'ع', 'غ', 'ف', 'ق', 'ک', 'گ', 'ل', 'ا', 'ن', 'و', 'ه', 'ی', 'ة', 'ي', 'ؤ', 'إ', 'أ', 'ء', 'ئ', 'ۀ', ' ', 'ك', 'ﮎ', 'ﮏ', 'ﮐ', 'ﮑ' };
            var AllowChar = new int[47]
                {
                1590, 1589, 1579, 1602, 1601, 1594, 1593, 1607, 1582, 1581, 1580, 1670, 1662, 1588, 1587, 1740, 1576, 1604,
                1575, 1578, 1606, 1605, 1705, 1711, 1592, 1591, 1586, 1585, 1584, 1583, 1574, 1608, 1577, 1610, 1688,
                1572, 1573, 1571, 1569, 1570, 1728, 32, 1603, 64398, 64399, 64400, 64401
                };

            foreach (var item in Str)
            {
                var ASCII = (int)item;

                if (!AllowChar.Contains(ASCII))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

    }
}
