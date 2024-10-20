namespace APM.Core.ProviderInterfaces
{
    public interface IFileProvider
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public char[] Password { get; set; }

        //public void Open();
        //public void Save();
    }
}
