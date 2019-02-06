
using Newtonsoft.Json.Linq;
using server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace server
{
    public class Database
    {
        public String ConnStr { get; set; }
        public String DBName { get; set; }


        public Database()
        {
            ConnStr = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename =|DataDirectory|\\Database.mdf; Integrated Security = True";

            DBName = "dbo";
        }


        public void Start()
        {
			//DropTables();
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
                Debug.WriteLine(ex.ToString());
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
                    + "[id] VARCHAR(70) NULL,"
                    + "[idCharacter] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,"
                    + "[name] VARCHAR(70) NULL,"
                    + "[description] VARCHAR(1000) NULL,"
                    + "[class] VARCHAR(70) NULL)", DBName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
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
                //Console.WriteLine(ex.ToString());
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
                //Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        public JObject InsertCharacter(Character character)
        {
            bool add = true;
            int id = 0;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();

            SqlTransaction tran = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;

                cmd.CommandText = String.Format("INSERT INTO [{0}].[Character]"
                    + "(id, name, description, class) "
                    + "VALUES (@id, @name, @description, @class);"
                    + "SELECT SCOPE_IDENTITY();"
                    , DBName);
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = character.Id;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = character.Name;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = character.Description;
                cmd.Parameters.Add("@class", SqlDbType.VarChar).Value = character.Class;
                //cmd.ExecuteNonQuery();

                id = Convert.ToInt32(cmd.ExecuteScalar());
                character.CharacterId = id;

                foreach (Stat entry in character.Stats)
                {
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = String.Format("INSERT INTO [{0}].[Stats]"
                    + "(name, value, idCharacter) "
                    + "VALUES (@name, @value, @idCharacter)"
                    , DBName);
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = entry.Name;
                    cmd.Parameters.Add("@value", SqlDbType.Int).Value = entry.Value;
                    cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.CharacterId;
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
                    cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.CharacterId;
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                tran.Rollback();
                add = false;
            }
            conn.Close();

            JObject result = new JObject();
            result.Add("Status", add);
            result.Add("Id", id);

            return result;
        }

        public List<Character> GetCharacters()
        {
            List<Character> characters = new List<Character>();

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = String.Format("SELECT id, idCharacter, name, description, class FROM [{0}].[Character]", DBName);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    String id = reader.GetString(0);
                    int idCharacter = reader.GetInt32(1);
                    String characterName = reader.GetString(2);
                    String characterDescription = reader.GetString(3);
                    String characterClass = reader.GetString(4);

                    List<Stat> stats = new List<Stat>();

                    List<string> equipment = new List<string>();

                    characters.Add(new Character
                    {
                        Id = id,
                        CharacterId = idCharacter,
                        Name = characterName,
                        Description = characterDescription,
                        Class = characterClass,
                        Stats = stats,
                        Equipment = equipment
                    });
                }
            }

            foreach (Character character in characters)
            {

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("SELECT name, value FROM [{0}].[Stats] WHERE idCharacter = @idCharacter", DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.CharacterId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String statName = reader.GetString(0);
                        int statValue = reader.GetInt32(1);
                        Stat stat = new Stat { Name = statName, Value = statValue };
                        character.Stats.Add(stat);
                    }
                }

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("SELECT name FROM [{0}].[Equipment] WHERE idCharacter = @idCharacter", DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.CharacterId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String equipmentName = reader.GetString(0);
                        character.Equipment.Add(equipmentName);
                    }
                }
            }

            conn.Close();

            return characters;
        }

        public Character GetCharacter(int idCharacter)
        {
            Character character;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = String.Format("SELECT id, name, description, class FROM [{0}].[Character] WHERE idCharacter = @idCharacter", DBName);
            cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = idCharacter;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    String id = reader.GetString(0);
                    String characterName = reader.GetString(1);
                    String characterDescription = reader.GetString(2);
                    String characterClass = reader.GetString(3);

                    List<Stat> stats = new List<Stat>();

                    List<string> equipment = new List<string>();

                    character = new Character
                    {
                        Id = id,
                        CharacterId = idCharacter,
                        Name = characterName,
                        Description = characterDescription,
                        Class = characterClass,
                        Stats = stats,
                        Equipment = equipment
                    };
                }
                else
                    return null;
            }


            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = String.Format("SELECT name, value FROM [{0}].[Stats] WHERE idCharacter = @idCharacter", DBName);
            cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = idCharacter;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    String statName = reader.GetString(0);
                    int statValue = reader.GetInt32(1);
                    Stat stat = new Stat { Name = statName, Value = statValue };
                    character.Stats.Add(stat);
                }
            }

            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = String.Format("SELECT name FROM [{0}].[Equipment] WHERE idCharacter = @idCharacter", DBName);
            cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = idCharacter;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    String equipmentName = reader.GetString(0);
                    character.Equipment.Add(equipmentName);
                }
            }

            conn.Close();

            return character;
        }

        public bool DeleteCharacter(int id)
        {
            bool delete = true;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("DELETE FROM [{0}].[Equipment] WHERE idCharacter = @idCharacter", DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("DELETE FROM [{0}].[Stats] WHERE idCharacter = @idCharacter", DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("DELETE FROM [{0}].[Character] WHERE idCharacter = @idCharacter", DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = id;
                if (cmd.ExecuteNonQuery() == 0)
                    delete = false;
            }
            catch (Exception)
            {
                delete = false;
            }

            return delete;
        }

        public bool DeleteStat(int id, String statName)
        {
            bool delete = true;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("DELETE FROM [{0}].[Stats] WHERE idCharacter = @idCharacter AND name = @name", DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = statName;
                if (cmd.ExecuteNonQuery() == 0)
                    delete = false;
            }
            catch (Exception)
            {
                delete = false;
            }

            return delete;
        }

        public bool DeleteItem(int id, String itemName)
        {
            bool delete = true;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("DELETE FROM [{0}].[Equipment] WHERE idCharacter = @idCharacter AND name = @name", DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = itemName;
                if (cmd.ExecuteNonQuery() == 0)
                    delete = false;
            }
            catch (Exception)
            {
                delete = false;
            }

            return delete;
        }

        public bool AddStat(int id, Stat stat)
        {
            bool add = true;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("INSERT INTO [{0}].[Stats]"
                     + "(name, value, idCharacter) "
                     + "VALUES (@name, @value, @idCharacter)"
                     , DBName);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = stat.Name;
                cmd.Parameters.Add("@value", SqlDbType.Int).Value = stat.Value;
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                add = false;
            }

            return add;
        }

        public bool AddItem(int id, String itemName)
        {
            bool add = true;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("INSERT INTO [{0}].[Equipment]"
                + "(name, idCharacter) "
                + "VALUES (@name, @idCharacter)"
                , DBName);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = itemName;
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                add = false;
            }

            return add;
        }

        public bool UpdateStat(int id, Stat stat)
        {
            bool update = true;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = String.Format("UPDATE [{0}].[Stats]"
                     + "SET value = @value "
                     + "WHERE idCharacter = @idCharacter AND name = @name"
                     , DBName);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = stat.Name;
                cmd.Parameters.Add("@value", SqlDbType.Int).Value = stat.Value;
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = id;
                if (cmd.ExecuteNonQuery() == 0)
                    update = false;
            }
            catch (Exception)
            {
                update = false;
            }

            return update;
        }

        public bool UpdateCharacter(int id, String characterName, String characterDescription, String characterClass)
        {
            bool update = true;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                String command = "UPDATE [{0}].[Character] SET "
                    + "name = @name, "
                    + "description = @description, "
                    + "class = @class "
                    + "WHERE idCharacter = @idCharacter";

                cmd.CommandText = String.Format(command
                     , DBName);

                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = characterName;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = characterDescription;
                cmd.Parameters.Add("@class", SqlDbType.VarChar).Value = characterClass;

                if (cmd.ExecuteNonQuery() == 0)
                    update = false;
            }
            catch (Exception)
            {
                update = false;
            }

            return update;
        }

        public bool Update(Character character)
        {
            bool update = true;

            SqlConnection conn = new SqlConnection(ConnStr);

            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;

                String command = "UPDATE [{0}].[Character] SET "
                    + "name = @name, "
                    + "description = @description, "
                    + "class = @class "
                    + "WHERE idCharacter = @idCharacter";

                cmd.CommandText = String.Format(command
                     , DBName);

                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.CharacterId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = character.Name;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = character.Description;
                cmd.Parameters.Add("@class", SqlDbType.VarChar).Value = character.Class;

                if (cmd.ExecuteNonQuery() == 0)
                    update = false;

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;
                cmd.CommandText = String.Format("DELETE FROM [{0}].[Equipment] WHERE idCharacter = @idCharacter", DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.CharacterId;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;
                cmd.CommandText = String.Format("DELETE FROM [{0}].[Stats] WHERE idCharacter = @idCharacter", DBName);
                cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.CharacterId;
                cmd.ExecuteNonQuery();

                foreach (Stat stat in character.Stats)
                {
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = String.Format("INSERT INTO [{0}].[Stats]"
                         + "(name, value, idCharacter) "
                         + "VALUES (@name, @value, @idCharacter)"
                         , DBName);
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = stat.Name;
                    cmd.Parameters.Add("@value", SqlDbType.Int).Value = stat.Value;
                    cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.CharacterId;
                    cmd.ExecuteNonQuery();
                }

                foreach (String itemName in character.Equipment)
                {
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;
                    cmd.CommandText = String.Format("INSERT INTO [{0}].[Equipment]"
                    + "(name, idCharacter) "
                    + "VALUES (@name, @idCharacter)"
                    , DBName);
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = itemName;
                    cmd.Parameters.Add("@idCharacter", SqlDbType.Int).Value = character.CharacterId;
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception)
            {
                update = false;
                tran.Rollback();
            }
            tran.Commit();

            return update;
        }
    }
}