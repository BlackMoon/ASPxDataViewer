namespace Data
{
    /// <summary>
    /// ProviderFactory - фабрика провайдеров
    /// </summary>
    public sealed class ProviderFactory
    {
        private static ProviderFactory _instance;

        public static ProviderFactory Instance
        {
            get
            {
                return _instance ?? (_instance = new ProviderFactory());
            }
        }

        private ProviderFactory()
        {
        }

        /// <summary>
        /// Получить провайдер по типу
        /// </summary>
        /// <param name="providerType">Тип провайдера</param>
        /// <returns>Провайдер</returns>
        public IDataProvider<Order> GetProvider(ProviderType providerType)
        {
            IDataProvider<Order> dataProvider = null;

            switch (providerType)
            {
                case ProviderType.DbProvider:
                    dataProvider = new DataBaseDataProvider();
                    break;

                case ProviderType.FileProvider:
                    dataProvider = new FileSystemDataProvider();
                    break;
            }

            return dataProvider;
        }
    }
}