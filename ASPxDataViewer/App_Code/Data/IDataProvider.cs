using System.Collections.Generic;

namespace Data
{
    public interface IDataProvider<T> where T : class
    {
        void Add(IEnumerable<T> items);
        void Save(IEnumerable<T> items);

        IEnumerable<T> Read();
    }
}