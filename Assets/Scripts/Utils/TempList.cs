using System.Collections.Generic;

namespace Utils
{
    public static class TempList<T>
    {
        private static readonly List<T> s_Temp = new();

        public static IList<T> GetList()
        {
            s_Temp.Clear();
            return s_Temp;
        }
    }
}
