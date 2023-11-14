namespace EasySave.Offline.JSON.FileManagement
{
    using System;
    using EasySave.Crypto;
    using System.IO;
    using System.Text.Json.Serialization;
    using System.Text;

    public class DataFile<T> where T : JSONSerializable
    {
        public string Path { get; private set; }
        public T Data { get; private set; }

        private Password _password;

        private bool _deleted = false;
        public DataFile(string path, T initialData)
        {
            this.Path = path;

            //TODO if logfile has entry load from there with password
            if (!this.TryCreateNew(this.Path, initialData))
            {
                using (Stream conFile = File.OpenRead(this.Path))
                {
                    JSONDataObject conData = JSONSerializable.Load(conFile);
                    if (!conData.Encrypted)
                    {
                        this.Data = JSONSerializable.Deserialize<T>(conData);
                    }
                    else
                        throw new ArgumentException("Data was encrypted");
                }
            }
        }
        public DataFile(string path, Password password, T initialData)
        {
            this.Path = path;
            this._password = password;

            //TODO if logfile has entry load from there with password
            if (!this.TryCreateNew(this.Path, initialData))
            {
                using (Stream conFile = File.OpenRead(this.Path))
                {
                    JSONDataObject conData = JSONSerializable.Load(conFile);
                    if (conData.Encrypted)
                    {
                        string pass = Encoding.Default.GetString(password.PasswordHash);
                        if (conData.CheckPassword(pass))
                        {
                            this.Data = JSONSerializable.Deserialize<T>(conData, pass);
                        }
                        else
                            throw new ArgumentException("The given password was invalid");
                    }
                }
            }
        }

        ~DataFile()
        {
            if(!this._deleted)
                Save();
        }
        public void Delete()
        {
            this._deleted = true;
            File.Delete(this.Path);
        }
        public void Save()
        {
            //create failsafe
            Guid guid = System.Guid.NewGuid();
            using (Stream conFile = File.OpenWrite(guid.ToString()))
            {
                JSONDataObject data = this.Data.CreateDataObject();
                data.Compress();
                if(this._password != null)
                    data.Encrypt(this._password);
                JSONSerializable.Save(data, Newtonsoft.Json.Formatting.Indented, conFile);
                conFile.Flush();
                conFile.Close();
            }
            //TODO if program crashes put guid in logfile and load it next time

            //overwrite original
            using (Stream conFile = File.OpenWrite(this.Path))
            {
                conFile.SetLength(0);
                JSONDataObject data = this.Data.CreateDataObject();
                data.Compress();
                if (this._password != null)
                    data.Encrypt(this._password);
                JSONSerializable.Save(data, Newtonsoft.Json.Formatting.Indented, conFile);
                conFile.Flush();
                conFile.Close();
            }
            //delete failsafe
            File.Delete(guid.ToString());

            this._deleted = false;
        }

        private bool TryCreateNew(string path, T data)
        {
            if (!File.Exists(this.Path))
            {
                Stream conFile = File.Create(this.Path);
                this.Data = data;
                return false;
            }
            return true;
        }
    }
}
