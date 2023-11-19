using EasySave.Crypto;
using EasySave.Offline.JSON;
using System;
using System.IO;
using System.Security.Cryptography;
using EasySave.Offline;
using EasySave.Online.SQL.Connection;
using MySql.Data.MySqlClient;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Rijndael.Create().GetType().BaseType.Name);

            string saveFilePath = Environment.CurrentDirectory;

            File.Delete(saveFilePath + @"\test.bla");
            Stream stream = File.OpenWrite(saveFilePath + @"\test.bla");
            /*string conFilePath = saveFilePath + @"\connection.conf";

            MySQLServerConnection connection = new MySQLServerConnection();

            //DELETE LATER WHEN FILE EXISTS
            if (!File.Exists(conFilePath))
            {
                Stream conFile = File.OpenWrite(conFilePath);

                connection.Database = "website";
                connection.Password = "password";
                connection.Username = "program";
                connection.Servername = "localhost";

                Password conPass = Password.Create("123", 50000, 32, SHA256.Create());
                JSONDataObject conData = connection.CreateDataObject();
                conData.Compress();
                conData.Encrypt(conPass);

                JSONSerializable.Save(conData, Newtonsoft.Json.Formatting.Indented, conFile); 
                Console.WriteLine("Written File");
            }
            else
            {
                Stream conFile = File.OpenRead(conFilePath);
                JSONDataObject conData = JSONSerializable.Load(conFile);
                bool encrypted = true;
                while (encrypted)
                {
                    Console.Write("Enter Password for connection data:");
                    string pass = ReadPassword();
                    if (conData.CheckPassword(pass))
                    {
                        connection = JSONSerializable.Deserialize<MySQLServerConnection>(conData, pass);
                        encrypted = false;
                        Console.WriteLine();
                        Console.WriteLine("Success");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Wrong Password");
                    }
                    Console.WriteLine();
                } 
            }

            Console.WriteLine(connection.Servername);
            Console.WriteLine(connection.Database);
            Console.WriteLine(connection.Username);
            Console.WriteLine(connection.Password);

            Console.WriteLine(JSONSerializable.Serialize(connection.CreateDataObject()));

            SQLClient client = new SQLClient(connection.Servername, connection.Username, connection.Database);
            client.Connect(connection.Password);
            MySqlDataReader r = client.BeginExecute("SELECT Version();");
            while (r.Read())
            {
                Console.WriteLine(r.GetString(0));
            }
            r.Close();
            */

            JSONSerializationTest t = new JSONSerializationTest();
            JSONDataObject data = t.CreateDataObject();
            Console.WriteLine(JSONSerializable.Serialize(data));
            string password = "hello";
            Password p = Password.Create(password, 50000, 32, SHA256.Create());

            data.Compress();
            data.Encrypt(p);
            Console.WriteLine(JSONSerializable.Serialize(data));
            JSONSerializable.Save(data, stream);
            stream.Close();
            stream = File.OpenRead(saveFilePath + @"\test.bla");
            data = JSONSerializable.Load(stream);

            data.Decrypt(password);
            data.Decompress();

            Console.WriteLine(JSONSerializable.Deserialize<JSONSerializationTest>(data).hello);

            Console.WriteLine("\n\n");

            //SQLSerializationTest test = new SQLSerializationTest();
            
            Console.ReadKey();
        }
        /// <summary>
        /// Reads a password securely from the console
        /// </summary>
        /// <returns>The password</returns>
        public static string ReadPassword()
        {
            //from https://stackoverflow.com/questions/3404421/password-masking-console-application
            string password = "";
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password += keyInfo.KeyChar;
                }
            } while (keyInfo.Key != ConsoleKey.Enter);
            return password;
        }
    }
}
