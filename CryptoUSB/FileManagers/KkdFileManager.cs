using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.FileManagers
{
    public class KkdFileManager : IFileManager
    {
        //перенос сюда открытия и сохранения .kkd
        public string[] Extensions => throw new NotImplementedException();

        public string[] Filters => throw new NotImplementedException();

        public string Path => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string Password => throw new NotImplementedException();

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
