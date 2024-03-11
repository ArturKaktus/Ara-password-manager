﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models.Interfaces
{
    public interface IRecordModel : IObjectModel
    {
        public string Login { get; set; }
        public char[] Password { get; set; }
        public string Url { get; set; }
    }
}
