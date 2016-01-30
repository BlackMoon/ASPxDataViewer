using System;
using System.Data;

namespace Extentions
{
    /// <summary>
    /// Расширение DB
    /// </summary>
    public static class DbExtentions
    {
        /// <summary>
        /// Расширение IDbConnection
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="connection">IDbConnection</param>
        /// <param name="commandText">текст</param>
        /// <param name="convertFunc">делегат преобразования</param>
        /// <returns></returns>
        public static PocoReader<T> ExecutePocoReader<T>(this IDbConnection connection, string commandText, Func<IDataRecord, T> convertFunc)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                IDataReader reader = command.ExecuteReader(CommandBehavior.Default);
                return new PocoReader<T>(reader, convertFunc, false);
            }
        }
    }
}