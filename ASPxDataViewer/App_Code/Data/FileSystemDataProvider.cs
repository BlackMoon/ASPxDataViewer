using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Summary description for FileSystemDataProvider
    /// </summary>
    public class FileSystemDataProvider : IDataProvider<Order>
    {
        public FileSystemDataProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> Read()
        {
            throw new NotImplementedException();
        }
    }
}