namespace Data
{
    /// <summary>
    /// Summary description for ProviderFactory
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

        public IDataProvider GetProvider(ProviderType providerType)
        {
            IDataProvider dataProvider = null;

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