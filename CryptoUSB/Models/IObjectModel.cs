using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models
{
    public interface IObjectModel
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string Name { get; set; }
    }
}
