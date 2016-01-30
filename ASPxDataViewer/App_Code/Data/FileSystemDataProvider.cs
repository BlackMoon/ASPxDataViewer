using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Configuration;

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
            
            CsvFileProviderSection section = (CsvFileProviderSection)ConfigurationManager.GetSection("csvfileprovidersection");
            if (section != null)
            {
                CsvFileName = HttpContext.Current.Server.MapPath(section.FileName);
                Separator = section.Separator;
            }
        }

        public void Add(IEnumerable<Order> items)
        {
            IList<Order> orders = items as IList<Order> ?? items.ToList();
            if (orders.Any())
            {
                List<Order> baseItems = Read().ToList();
                
                // удалить
                foreach (Order o in orders.Where(o => o.State == ObjectState.Deleted))
                {
                    baseItems.RemoveAll(i => i.Code == o.Code);
                    o.State = ObjectState.None;
                }

                // изменить
                foreach (Order o in orders.Where(o => o.State == ObjectState.Updated))
                {
                    Order baseItem = baseItems.Find(i => i.Code == o.Code);
                    if (baseItem != null)
                    {
                        baseItem.Description = o.Description;
                        baseItem.Amount = o.Amount;
                        baseItem.Price = o.Price;
                    }

                    o.State = ObjectState.None;
                }

                // добавить
                foreach (Order o in orders.Where(o => o.State == ObjectState.New))
                {
                    baseItems.Add(new Order()
                    {
                        Code = o.Code,
                        Description = o.Description,
                        Amount = o.Amount,
                        Price = o.Price
                    });

                    o.State = ObjectState.None;
                }

                Save(baseItems);
            }
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

                    o.State = ObjectState.None;
                }
            }
        }

        public IEnumerable<Order> Read()
        {
            if (string.IsNullOrEmpty(CsvFileName))
                throw new ArgumentNullException("csvFileName");
            
            foreach (string line in File.ReadLines(CsvFileName))
            {
                string[] x = line.Split(',');

                int code;
                if (int.TryParse(x[0], out code))
                {
                    decimal amount;
                    decimal.TryParse(x[2], out amount);

                    decimal price;
                    decimal.TryParse(x[3], out price);

                    yield return new Order()
                        {
                            Code = code,
                            Description = x[1],
                            Amount = amount,
                            Price = price
                        };
                }
            }
        }
    }
}