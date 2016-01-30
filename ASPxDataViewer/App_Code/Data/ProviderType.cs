using System.ComponentModel;

namespace Data
{
    /// <summary>
    /// Тип провайдера
    /// </summary>
    public enum ProviderType
    {
        [Description("База данных")]
        DbProvider,
        [Description("Файловая система")]
        FileProvider
    }
}