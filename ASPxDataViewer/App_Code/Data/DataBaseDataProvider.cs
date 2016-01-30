using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Extentions;

namespace Data
{
    /// <summary>
    /// Summary description for DataBaseDataProvider
    /// </summary>
    public class DataBaseDataProvider : IDataProvider<Order>
    {
        public string ConnectionString { get; private set; }
        public SQLiteConnection Connection { get; private set; }

        public DataBaseDataProvider()
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["ordersDB"];
            if (connectionStringSettings != null)
                ConnectionString = connectionStringSettings.ConnectionString;

            Connection = new SQLiteConnection(ConnectionString);
        }
        
        public void Add(IEnumerable<Order> items)
        {
            throw new NotImplementedException();
        }

        public void Save(IEnumerable<Order> items)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            int i = 0;
            StringBuilder sb = new StringBuilder("DELETE FROM orders;");

            List<SQLiteParameter> dbParameters = new List<SQLiteParameter>();

            IList<Order> orders = items as IList<Order> ?? items.ToList();
            if (orders.Any())
            {
                Order order = null;
                sb.AppendLine("INSERT INTO orders(code, description, amount, price) VALUES ");
                for (; i < orders.Count() - 1; i++)
                {
                    order = orders[i];
                    dbParameters.Add(new SQLiteParameter(DbType.Int32)
                    {
                        ParameterName = "@Code" + i,
                        Value = order.Code
                    });

                    dbParameters.Add(new SQLiteParameter(DbType.String)
                    {
                        ParameterName = "@Description" + i,
                        Value = order.Description
                    });

                    dbParameters.Add(new SQLiteParameter(DbType.Decimal)
                    {
                        ParameterName = "@Amount" + i,
                        Value = order.Amount
                    });

                    dbParameters.Add(new SQLiteParameter(DbType.Decimal)
                    {
                        ParameterName = "@Price" + i,
                        Value = order.Price
                    });

                    sb.AppendFormat("( @Code{0}, @Description{0}, @Amount{0}, @Price{0}),", i);
                }

                // последняя запись --> добавляется [;] вместо [,]
                order = orders[i];
                dbParameters.Add(new SQLiteParameter(DbType.Int32)
                {
                    ParameterName = "@Code" + i,
                    Value = order.Code
                });

                dbParameters.Add(new SQLiteParameter(DbType.String)
                {
                    ParameterName = "@Description" + i,
                    Value = order.Description
                });

                dbParameters.Add(new SQLiteParameter(DbType.Decimal)
                {
                    ParameterName = "@Amount" + i,
                    Value = order.Amount
                });

                dbParameters.Add(new SQLiteParameter(DbType.Decimal)
                {
                    ParameterName = "@Price" + i,
                    Value = order.Price
                });

                sb.AppendFormat("( @Code{0}, @Description{0}, @Amount{0}, @Price{0});", i);
            }

            using (SQLiteCommand cmd = new SQLiteCommand(sb.ToString(), Connection))
            {
                cmd.Parameters.AddRange(dbParameters.ToArray());
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Order> Read()
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            return Connection.ExecutePocoReader("SELECT * FROM orders", 
                x => new Order()
                {
                    Code = x.GetInt32(0),
                    Description = x.GetString(1),
                    Amount = x.GetDecimal(2),
                    Price = x.GetDecimal(3)
                });
            
        }
    }
}