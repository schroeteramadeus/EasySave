using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Online.SQL.DataManagement;

namespace EasySave.Online.SQL
{
    //TODO

    public interface ISQLTypeConverter
    {
        public SQLTable ConvertToSQL(object obj);
        public object ConvertFromSQL(SQLTable data);
    }
}
