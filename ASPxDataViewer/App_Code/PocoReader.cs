using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Summary description for Extentions
/// </summary>
/// <summary>
/// A lightweight SQL to LINQ style wrapper on <see cref="IDataReader"/>.
/// </summary>
/// <typeparam name="T">The type of the POCO.</typeparam>
public class PocoReader<T> : IEnumerable<T>
{
    /// <summary>
    /// Local storage of the data reader.
    /// </summary>
    private readonly IDataReader _dataReader;

    /// <summary>
    /// Local storage for the translation function between the SQL and the POCO.
    /// </summary>
    private readonly Func<IDataRecord, T> _convertFunc;

    /// <summary>
    /// Local storage for whether we should close the reader when fully enumerated.
    /// </summary>
    private readonly bool _closeReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="PocoReader{T}"/> class.
    /// </summary>
    /// <param name="dataReader">The data reader to retrieve the data from and iterate over.</param>
    /// <param name="convertFunc">The translation object to convert from the SQL to the POCO.</param>
    /// <param name="closeReader">A value indicating whether or not to close the reader when the enumeration is exhausted.</param>
    public PocoReader(IDataReader dataReader, Func<IDataRecord, T> convertFunc, bool closeReader)
    {
        if (dataReader == null)
            throw new ArgumentNullException("dataReader");

        if (convertFunc == null)
            throw new ArgumentNullException("convertFunc");

        
        _dataReader = dataReader;
        _convertFunc = convertFunc;
        _closeReader = closeReader;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the reader.
    /// </summary>
    /// <returns>An enumerator for the reader.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        while (_dataReader.Read())
        {
            yield return _convertFunc(_dataReader);
        }

        if (_closeReader)
            _dataReader.Close();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the reader.
    /// </summary>
    /// <returns>An enumerator for the reader.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
