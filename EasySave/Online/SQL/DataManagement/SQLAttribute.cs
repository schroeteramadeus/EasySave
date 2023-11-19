using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Online.SQL.DataManagement
{
    //TODO

    public class SQLAttribute
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public bool Nullable { get; private set; }

        public SQLAttribute(string name, Type type) : this(name, type, true) { }
        public SQLAttribute(string name, Type type, bool nullable)
        {
            this.Name = name;
            this.Type = type;
            this.Nullable = nullable;
        }
    }
}
