﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroScanner.Domain
{
    public class Token
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public Token(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
