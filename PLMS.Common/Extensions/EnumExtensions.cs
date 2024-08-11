namespace PLMS.Common.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<int> GetEnumValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<int>();
        }

        public static IEnumerable<T> GetEnumValuesAsEnum<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
