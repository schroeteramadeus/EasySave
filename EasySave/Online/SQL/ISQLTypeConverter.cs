using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Online.SQL.DataManagement;

namespace EasySave.Online.SQL
{
    public interface ISQLTypeConverter
    {
        //TODO
        public SQLTable ConvertToSQL(object obj);
        public object ConvertFromSQL(SQLTable data);
    }
}
