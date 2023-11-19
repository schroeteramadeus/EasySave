using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Online.SQL.DataManagement
{
    //TODO

    public class SQLTable
    {
        private string _name;
        private List<SQLAttribute> _key = new List<SQLAttribute>();
        private List<SQLAttribute> _attributes = new List<SQLAttribute>();

        public SQLTable(string name, List<SQLAttribute> key, List<SQLAttribute> attributes)
        {
            if (attributes.Count + key.Count > 0)
            {
                List<string> names = new List<string>();
                foreach (SQLAttribute attr in key)
                {
                    if (!names.Contains(attr.Name))
                    {
                        names.Add(attr.Name);
                        this._key.Add(attr);
                    }
                    else
                        throw new ArgumentException("Attributes can't have the same names");
                }
                foreach (SQLAttribute attr in attributes)
                {
                    if (!names.Contains(attr.Name))
                    {
                        names.Add(attr.Name);
                        this._attributes.Add(attr);
                    }
                    else
                        throw new ArgumentException("Attributes can't have the same names");
                }
                this._name = name;
            }
            else
                throw new ArgumentException("There were no attributes or keys given");
        }

    }
}
