using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace APM.Core.ProviderInterfaces
{
    public interface IReadWriteFile
    {
        public void ReadFile(Window? owner, IStorageFile file);
        public void SaveFile(Window? owner, IStorageFile file);
    }
}
