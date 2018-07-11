namespace PersianCharFix
{
    public static class Utility
    {
        public static string EmptyIfNull(this string str)
        {
            return (str == null ? "" : str);
        }

        public static string NullIfEmpty(this string str)
        {
            return (str == "" ? null : str);
        }

        public static string En2Fa(this string str)
        {
            return
                str.Replace('0', '۰')
                    .Replace('1', '۱')
                    .Replace('2', '۲')
                    .Replace('3', '۳')
                    .Replace('4', '۴')
                    .Replace('5', '۵')
                    .Replace('6', '۶')
                    .Replace('7', '۷')
                    .Replace('8', '۸')
                    .Replace('9', '۹');
        }

        public static string Fa2En(this string str)
        {
            return str
                    .Replace('۰', '0')
                    .Replace('۱', '1')
                    .Replace('۲', '2')
                    .Replace('۳', '3')
                    .Replace('۴', '4')
                    .Replace('۵', '5')
                    .Replace('۶', '6')
                    .Replace('۷', '7')
                    .Replace('۸', '8')
                    .Replace('۹', '9')

                    .Replace('٠', '0')
                    .Replace('١', '1')
                    .Replace('٢', '2')
                    .Replace('٣', '3')
                    .Replace('٤', '4')
                    .Replace('٥', '5')
                    .Replace('٦', '6')
                    .Replace('٧', '7')
                    .Replace('٨', '8')
                    .Replace('٩', '9');
        }

        public static string FixArabicChars(this string str)
        {
            return str.Replace('ﮎ', 'ک').Replace('ﮏ', 'ک').Replace('ﮐ', 'ک').Replace('ﮑ', 'ک').Replace('ك', 'ک').Replace('ي', 'ی').Replace('ئ', 'ی');
        }

        public static string FixPersianChars(this string str)
        {
            return str.Replace(" می ", " می‌")
                .Replace(" ای", "‌ای")
                .Replace(" ها ", "‌ها ")
                .Replace(" های ", "‌های ")
                .Replace(" تر ", "‌تر ")
                .Replace(" ترین ", "‌ترین ")
                .Replace(" . ", ".-.")
                .Replace(". ", ".-.")
                .Replace(" .", ".-.")
                .Replace(".-.", ". ")
                .Replace(" ، ", "،-،")
                .Replace("، ", "،-،")
                .Replace(" ،", "،-،")
                .Replace("،-،", "، ");
        }
    }
}
