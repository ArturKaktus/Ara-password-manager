using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM.Desktop.Features.CatalogTreeView.Controls.NewGroup
{
    public class NewGroupViewModel
    {
        public NewGroupViewModel()
        {
            FolderName = "New Folder";
        }
        public NewGroupViewModel(string folderName)
        {
            FolderName = folderName;
        }
        public string FolderName { get; set; }
    }
}
