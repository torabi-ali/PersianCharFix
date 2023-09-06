using System.Text.RegularExpressions;

namespace App.Utility
{
    public static class StringHelper
    {
        public static string Fa2En(string str)
        {
            return str
                    .Replace("۰", "0")
                    .Replace("۱", "1")
                    .Replace("۲", "2")
                    .Replace("۳", "3")
                    .Replace("۴", "4")
                    .Replace("۵", "5")
                    .Replace("۶", "6")
                    .Replace("۷", "7")
                    .Replace("۸", "8")
                    .Replace("۹", "9")

                    .Replace("٠", "0")
                    .Replace("١", "1")
                    .Replace("٢", "2")
                    .Replace("٣", "3")
                    .Replace("٤", "4")
                    .Replace("٥", "5")
                    .Replace("٦", "6")
                    .Replace("٧", "7")
                    .Replace("٨", "8")
                    .Replace("٩", "9");
        }

        public static string FixPersianChars(string str)
        {
            var result = str.Replace('ﮎ', 'ک').Replace('ﮏ', 'ک').Replace('ﮐ', 'ک').Replace('ﮑ', 'ک').Replace('ك', 'ک'); // تصحیح ک
            result = result.Replace('ٱ', 'ا').Replace('ٵ', 'ا'); // تصحیح الف
            result = result.Replace('ي', 'ی').Replace('ئ', 'ی'); // تصحیح ی
            result = result.Replace('ة', 'ه'); // تصحیح ه
            result = result.Replace(' ', ' ');
            result = result.Replace(" می ", " می‌").Replace(" ی ", "‌ی ").Replace(" ای ", "‌ای ").Replace(" ها ", "‌ها ").Replace(" های ", "‌های ").Replace(" تر ", "‌تر ").Replace(" ترین ", "‌ترین "); // تصحیح نیم‌فاصله
            result = result.Replace(" . ", ". ").Replace(" .", ". "); // تصحیح نقطه
            result = result.Replace(" ، ", "، ").Replace(" ،", "، "); // تصحیح کاما

            return result;
        }

        public static string CleanString(this string str)
        {
            var result = str.Trim();
            result = FixPersianChars(result);
            result = Fa2En(result);
            result = RemoveBlankSpaces(result);

            return result;
        }

        public static string RemoveBlankSpaces(string text)
        {
            return Regex.Replace(text, @"\s\s+", " ");
        }
    }
}