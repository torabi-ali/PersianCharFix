namespace App.Dtos
{
    public enum FunctionResult : byte
    {
        Done = 1,

        PartialyDone = 2,

        Failed = 4,

        Error = 8
    }
}
