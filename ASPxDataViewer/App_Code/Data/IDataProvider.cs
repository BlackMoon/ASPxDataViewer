using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Интерфейс провайдера данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataProvider<T> where T : class
    {
        /// <summary>
        /// Обновить записи
        /// </summary>
        /// <param name="items"></param>
        void Add(IEnumerable<T> items);

        /// <summary>
        /// Сохранить записи, старые данные удаляются
        /// </summary>
        /// <param name="items"></param>
        void Save(IEnumerable<T> items);

        /// <summary>
        /// Чтение записей
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Read();
    }
}