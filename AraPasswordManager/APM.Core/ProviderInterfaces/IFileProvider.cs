using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM.Core.ProviderInterfaces
{
    public interface IFileProvider
    {
        public string FilePath { get; set; }
        public char[] Password { get; set; }
        public void Open();
        public void Save();
    }
}
