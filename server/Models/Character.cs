using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace server.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Class { get; set; }
        public Dictionary<string, int> Stats { get; set; }
        public List<string> Equipment { get; set; }

        public bool ToDatabase(){
                bool add = true;

                string connStr = "server=localhost;user=root;database=mydb;port=3306;";
                MySqlConnection conn = new MySqlConnection(connStr);

                conn.Open();

                MySqlTransaction tran = conn.BeginTransaction();

                try
                {
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = tran;

                    cmd.CommandText = "INSERT INTO `mydb`.`Character` (idCharacter, name, description, class)"
                        + "VALUES (?idCharacter, ?name, ?description, ?class)";
                    cmd.Parameters.Add("?idCharacter", MySqlDbType.Int16).Value = this.Id;
                    cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = this.Name;
                    cmd.Parameters.Add("?description", MySqlDbType.VarChar).Value = this.Description;
                    cmd.Parameters.Add("?class", MySqlDbType.VarChar).Value = this.Class;
                    cmd.ExecuteNonQuery();

                    foreach (KeyValuePair<string, int> entry in Stats)
                    {
                        cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = tran;
                        cmd.CommandText = "INSERT INTO `mydb`.`Stats` (Character_idCharacter, name, value)"
                        + "VALUES (?Character_idCharacter, ?name, ?value)";
                        cmd.Parameters.Add("?Character_idCharacter", MySqlDbType.Int16).Value = this.Id;
                        cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = entry.Key;
                        cmd.Parameters.Add("?value", MySqlDbType.Int16).Value = entry.Value;
                        cmd.ExecuteNonQuery();
                    }

                    foreach (string entry in Equipment)
                    {
                        cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = tran;
                        cmd.CommandText = "INSERT INTO `mydb`.`Equipment` (Character_idCharacter, name)"
                       + "VALUES (?Character_idCharacter, ?name)";
                        cmd.Parameters.Add("?Character_idCharacter", MySqlDbType.Int16).Value = this.Id;
                        cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = entry;
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    add = false;
                }
                conn.Close();
                return add;
        }
    }
}