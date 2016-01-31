using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// IDataReader --> IEnumerable конвертор
/// </summary>
public class PocoReader<T> : IEnumerable<T>
{
    /// <summary>
    /// DataReader
    /// </summary>
    private readonly IDataReader _dataReader;

    /// <summary>
    /// Функция конвертации
    /// </summary>
    private readonly Func<IDataRecord, T> _convertFunc;

    /// <summary>
    /// Закрывать DataReader?
    /// </summary>
    private readonly bool _closeReader;
   
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
   
    public IEnumerator<T> GetEnumerator()
    {
        while (_dataReader.Read())
        {
            yield return _convertFunc(_dataReader);
        }

        if (_closeReader)
            _dataReader.Close();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
