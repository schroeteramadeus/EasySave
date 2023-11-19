namespace EasySave.Online.SQL.Connection
{
    using System;
    using MySql.Data.MySqlClient;

    //TODO

    public class SQLClient
    {

        public string Host { get; private set; }
        public string Username { get; private set; }
        public string DataBase { get; private set; }

        private MySqlConnection _connection;

        private MySqlTransaction _currentTransaction;

        private MySqlDataReader _dataReader;

        public SQLClient(string host, string username, string database)
        {
            this.Host = host;
            this.Username = username;
            this.DataBase = database;
        }
        ~SQLClient()
        {
            this.Disconnect();
        }

        public bool Connect(string password)
        {
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = this.Host;
                builder.UserID = this.Username;
                builder.Password = password;
                builder.Database = this.DataBase;

                this._connection = new MySqlConnection(builder.GetConnectionString(true));
                this._connection.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public MySqlDataReader BeginExecute(string commandText)
        {
            if (this._dataReader != null)
                this.Rollback();
            if (this._currentTransaction == null)
                this._currentTransaction = this._connection.BeginTransaction();
            MySqlCommand command = this._connection.CreateCommand();
            command.Transaction = this._currentTransaction;
            command.CommandText = commandText;
            this._dataReader = command.ExecuteReader();
            return this._dataReader;
        }

        public void Rollback()
        {
            if(this._currentTransaction != null)
            {
                this._dataReader.Close();
                this._dataReader = null;
                this._currentTransaction.Rollback();
                this._currentTransaction = null;
            }
        }
        public void Commit()
        {
            if (this._currentTransaction != null)
            {
                this._dataReader.Close();
                this._dataReader = null;
                this._currentTransaction.Commit();
                this._currentTransaction = null;
            }
        }


        public void Disconnect()
        {
            if (this._connection.State != System.Data.ConnectionState.Closed)
            {
                if (this._currentTransaction != null)
                    this.Rollback();
                this._connection.Close();
                this._connection = null;
            }
        }
    }
}
