
using server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace server
{
    public class Database
    {
        public String ConnStr { get; set; }
        public String DBName { get; set; }

        private static Database instance;

        private Database()
        {     

            ConnStr = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename =|DataDirectory|\\Database.mdf; Integrated Security = True";

            DBName = "dbo";
        }

        public static Database Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Database();
                }
                return instance;
            }
        }

        public void Start()
        {
            DropTables();            
            CreateCharacterTable();
            CreateEquipmentTable();
            CreateStatsTable();
        }

        private void DropTables()
        {
            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;

                cmd.CommandText = String.Format("DROP TABLE [{0}].[Stats]", DBName);
                cmd.ExecuteNonQuery();

                cmd.CommandText = String.Format("DROP TABLE [{0}].[Equipment]", DBName);
                cmd.ExecuteNonQuery();

                cmd.CommandText = String.Format("DROP TABLE [{0}].[Character]", DBName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }      

        private void CreateCharacterTable()
        {
            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;

                cmd.CommandText = String.Format("CREATE TABLE [{0}].[Character]("
                    + "[idCharacter] INT NOT NULL PRIMARY KEY,"
                    + "[name] VARCHAR(45) NULL,"
                    + "[description] VARCHAR(300) NULL,"
                    + "[class] VARCHAR(45) NULL)", DBName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        private void CreateStatsTable()
        {
            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;

                cmd.CommandText = String.Format("CREATE TABLE [{0}].[Stats]("
                    + "[idStats] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,"
                    + "[name] VARCHAR(50) NOT NULL,"
                    + "[value] INT NULL,"
                    + "[idCharacter] INT NOT NULL FOREIGN KEY REFERENCES [{0}].[Character](idCharacter))"
                    , DBName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        private void CreateEquipmentTable()
        {
            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;

                cmd.CommandText = String.Format("CREATE TABLE [{0}].[Equipment]("
                    + "[idEquipment] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,"
                    + "[name] VARCHAR(50) NOT NULL,"
                    + "[idCharacter] INT NOT NULL FOREIGN KEY REFERENCES [{0}].[Character](idCharacter))"
                    , DBName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        public bool InsertCharacter(Character character)
        {
            bool add = true;
           
            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();

            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;

                cmd.CommandText = String.Format("INSERT INTO [{0}].[Character]"
                    + "(idCharacter, name, description, class) "
                    + "VALUES (@idCharacter, @name, @description, @class)"                   
                    , DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.Id;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = character.Name;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = character.Description;
                cmd.Parameters.Add("@class", SqlDbType.VarChar).Value = character.Class;               
                cmd.ExecuteNonQuery();

                foreach (KeyValuePair<string, int> entry in character.Stats)
                {
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = String.Format("INSERT INTO [{0}].[Stats]"
                    + "(name, value, idCharacter) "
                    + "VALUES (@name, @value, @idCharacter)"
                    , DBName);                    
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = entry.Key;
                    cmd.Parameters.Add("@value", SqlDbType.Int).Value = entry.Value;
                    cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.Id;
                    cmd.ExecuteNonQuery();
                }

                foreach (string entry in character.Equipment)
                {
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = String.Format("INSERT INTO [{0}].[Equipment]"
                    + "(name, idCharacter) "
                    + "VALUES (@name, @idCharacter)"
                    , DBName);
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = entry;                    
                    cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.Id;
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();

            }
            catch (Exception)
            {
                tran.Rollback();
                add = false;
            }
            conn.Close();
            return add;
        }

    }
}