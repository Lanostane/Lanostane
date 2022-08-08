using System.Collections.Generic;

namespace Utils
{
    public static class TempList<T>
    {
        private readonly static List<T> _Temp = new();

        public static IList<T> GetList()
        {
            _Temp.Clear();
            return _Temp;
        }
    }
}
