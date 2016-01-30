using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
// ReSharper disable NotResolvedInText

namespace Data
{
    /// <summary>
    /// Summary description for FileSystemDataProvider
    /// </summary>
    public class FileSystemDataProvider : IDataProvider<Order>
    {
        public string CsvFileName { get; private set; }

        public string Separator { get; private set; }

        public FileSystemDataProvider()
        {
            Separator = ",";

            CsvFileName = ConfigurationManager.AppSettings["csvfilename"];

            CsvFileName = HttpContext.Current.Server.MapPath(CsvFileName);
        }

        public void Add(IEnumerable<Order> items)
        {
            throw new NotImplementedException();
        }

        public void Save(IEnumerable<Order> items)
        {
            if (string.IsNullOrEmpty(CsvFileName))
                throw new ArgumentNullException("csvFileName");

            using (StreamWriter sw = new StreamWriter(CsvFileName))
            {
                foreach (Order o in items)
                {
                    string line = string.Join(",", o.Code.ToString(), o.Description, o.Amount.ToString(CultureInfo.InvariantCulture), o.Price.ToString(CultureInfo.InvariantCulture));
                    sw.WriteLine(line);    
                }
            }
        }

        public IEnumerable<Order> Read()
        {
            if (string.IsNullOrEmpty(CsvFileName))
                throw new ArgumentNullException("csvFileName");

            IList<Order> orders = new List<Order>();

            foreach (string line in File.ReadAllLines(CsvFileName))
            {
                string[] x = line.Split(',');

                int code;
                if (int.TryParse(x[0], out code))
                {
                    decimal amount;
                    decimal.TryParse(x[2], out amount);

                    decimal price;
                    decimal.TryParse(x[3], out price);

                    orders.Add(new Order()
                    {
                        Code = code,
                        Description = x[1],
                        Amount = amount,
                        Price = price
                    });
                }
            }

            return orders;
        }
    }
}