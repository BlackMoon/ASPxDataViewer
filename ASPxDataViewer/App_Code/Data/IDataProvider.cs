using System.Collections.Generic;

namespace Data
{
    public interface IDataProvider<out T> where T : class
    {
        void Add();
        void Save();

        IEnumerable<T> Read();
    }
}