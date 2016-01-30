using System.Configuration;

namespace Configuration
{
    /// <summary>
    /// Секция (.config) настроек csv-файла 
    /// </summary>
    public class CsvFileProviderSection : ConfigurationSection
    {
        /// <summary>
        /// Относительный путь к csv-файлу.
        /// </summary>
        [ConfigurationProperty("filename", DefaultValue = "orders.csv", IsRequired = true)]
        public string FileName
        {
            get { return (string)this["filename"]; }
            set { this["filename"] = value; }
        }

        /// <summary>
        /// Разделитель колонок в csv-файле
        /// </summary>
        [ConfigurationProperty("separator", DefaultValue = ",", IsRequired = true)]
        public string Separator
        {
            get { return (string)this["separator"]; }
            set { this["separator"] = value; }
        }

    }
}