using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
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
            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            IList<Order> orders = items as IList<Order> ?? items.ToList();
            if (orders.Any())
            {
                int i;
                Order order;

                StringBuilder sb = new StringBuilder();
                List<SQLiteParameter> dbParameters = new List<SQLiteParameter>();
                
                // удалить
                IList<Order> delOrders = orders.Where(o => o.State == ObjectState.Deleted).ToList();
                if (delOrders.Any())
                {
                    sb.AppendLine("DELETE FROM orders WHERE code IN (");
                    for(i = 0; i < delOrders.Count - 1; i++)
                    {
                        order = delOrders[i];
                        dbParameters.Add(new SQLiteParameter(DbType.Int32)
                        {
                            ParameterName = "@DelCode" + i,
                            Value = order.Code
                        });

                        sb.AppendFormat(" @DelCode{0},", i);
                        order.State = ObjectState.None;
                    }

                    // последняя запись --> добавляется [)] вместо [,]
                    order = delOrders[i];
                    dbParameters.Add(new SQLiteParameter(DbType.Int32)
                    {
                        ParameterName = "@DelCode" + i,
                        Value = order.Code
                    });

                    sb.AppendFormat(" @DelCode{0} );", i);
                    order.State = ObjectState.None;
                }

                // вставить & изменить
                IList<Order> mergedOrders = orders.Where(o => o.State == ObjectState.New || o.State == ObjectState.Updated).ToList();
                if (mergedOrders.Any())
                {
                    sb.AppendLine("INSERT OR REPLACE INTO orders(code, description, amount, price) VALUES ");   
                    for (i= 0; i < mergedOrders.Count() - 1; i++)
                    {
                        order = mergedOrders[i];
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
                        order.State = ObjectState.None;
                    }

                    // последняя запись --> добавляется [;] вместо [,]
                    order = mergedOrders[i];
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
                    order.State = ObjectState.None;
                }

                using (SQLiteCommand cmd = new SQLiteCommand(sb.ToString(), Connection))
                {
                    cmd.Parameters.AddRange(dbParameters.ToArray());
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Save(IEnumerable<Order> items)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            
            StringBuilder sb = new StringBuilder("DELETE FROM orders;");
            List<SQLiteParameter> dbParameters = new List<SQLiteParameter>();

            IList<Order> orders = items as IList<Order> ?? items.ToList();
            if (orders.Any())
            {
                int i;
                Order order;

                sb.AppendLine("INSERT INTO orders(code, description, amount, price) VALUES ");
                for (i= 0; i < orders.Count() - 1; i++)
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
                    order.State = ObjectState.None;
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
                order.State = ObjectState.None;
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

            return Connection.ExecutePocoReader("SELECT o.code, o.description, o.amount, o.price FROM orders o",
                x => new Order()
                {
                    Code = (int)x["code"],
                    Description = (string)x["description"],
                    Amount = (decimal)x["amount"],
                    Price = (decimal)x["price"]
                });
            
        }
    }
}