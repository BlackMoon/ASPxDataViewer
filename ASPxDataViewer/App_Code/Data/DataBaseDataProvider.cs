using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    /// <summary>
    /// Summary description for DataBaseDataProvider
    /// </summary>
    public class DataBaseDataProvider : IDataProvider<Order>
    {
        public DataBaseDataProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        public void Add(IEnumerable<Order> items)
        {
            throw new NotImplementedException();
        }

        public void Save(IEnumerable<Order> items)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> Read()
        {
            return Enumerable.Empty<Order>();
        }
    }
}