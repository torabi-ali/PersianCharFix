namespace PersianCharFix.Models
{
    public enum FunctionResult : byte
    {
        //انجام شده
        Done = 0,

        //قسمتی انجام شده
        PartialyDone = 2,

        //با شکست مواجه شده
        Failed = 4,

        //خطا
        Error = 8
    }
}
