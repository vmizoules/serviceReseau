﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesLayer;

namespace DataAccessLayer
{
    class SQLAccess : IBridge
    {
        public enum Jedi_enum
        {
            IDJEDI = 0,
            NAME = 1,
            ISSITH = 2,
            PIC = 3
        }
        public enum Carac_enum// Cours 
        {
            IDCARAC = 0,
            IDJEDI = 1,
            IDSTADE = 2,
            NOM = 3,
            VALEUR = 4
        }
        private string m_connectionString = "";

        public SQLAccess(string connectionString)
        {
            m_connectionString = connectionString;
            using (SqlConnection sqlConnection = new SqlConnection(m_connectionString))
            {
                //sqlConnection.Open();
              //  SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand("SELECT idJedi, Name, isSith, Pic FROM Jedis;", sqlConnection));

            }
        }


        public List<Jedi> GetAllJedis()
        {
            List<Jedi> _allJedis = new List<Jedi>();
            using (SqlConnection sqlConnection = new SqlConnection(m_connectionString))
            {
                string requete = "SELECT idJedi, Name, isSith, Pic FROM Jedis;";
                SqlCommand sqlCommand = new SqlCommand(requete, sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    List<Caracteristique> _carac = new List<Caracteristique>();
                    using (SqlConnection sqlConnection2 = new SqlConnection(m_connectionString))
                    {
                        String id = sqlDataReader.GetInt32((int)Jedi_enum.IDJEDI).ToString(); ;
                        String requete2 = "SELECT carac.idCarac, carac.idJedi, carac.idStade, carac.Nom ,carac.Valeur FROM Jedis jedi, Carac carac WHERE jedi.idJedi=" + id + " AND jedi.id=carac.idjedi;";
                        SqlCommand sqlCommand2 = new SqlCommand(requete2, sqlConnection2);
                        sqlConnection2.Open();

                        SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader();//creation d'une nouvelle sqlDataReader2
                        while (sqlDataReader2.Read())
                        {
                            _carac.Add(new Caracteristique(sqlDataReader2.GetInt32((int)Carac_enum.IDCARAC),
                                                          sqlDataReader2.GetString((int)Carac_enum.NOM),
                                                          sqlDataReader2.GetInt32((int)Carac_enum.VALEUR))
                              );
                        }
                        sqlConnection2.Close();
                        //jointure entre les 2 tables
                    }
                    // int id, string nom, bool isSith, List< Caracteristique > carac
                    _allJedis.Add(new Jedi(sqlDataReader.GetInt32((int)Jedi_enum.IDJEDI),
                                           sqlDataReader.GetString((int)Jedi_enum.NAME),
                                           sqlDataReader.GetBoolean((int)Jedi_enum.ISSITH),
                                           _carac,
                                           sqlDataReader.GetString((int)Jedi_enum.PIC)));
                }
                sqlConnection.Close();
            }
            return _allJedis;
        }

        public void AddJedis(Jedi _jedi)
        {
            using (SqlConnection sqlConnection = new SqlConnection(m_connectionString))
            {
                string query = "INSERT INTO Jedis (Name, IsSith, Pic) VALUES (@Name, @IsSith, @Pic)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Name", _jedi.Nom);
                sqlCommand.Parameters.AddWithValue("@IsSith", _jedi.IsSith);
                sqlCommand.Parameters.AddWithValue("@Pic", _jedi.Image);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();

               /* using (SqlConnection sqlConnection2 = new SqlConnection(m_connectionString))
                {
                    //string requete2 = "INSERT INTO Carac(idCarac, Nom, Valeur) .....";
                }*/
                sqlConnection.Close();
              }
        }

        public int RemoveJedi(Jedi _jedi)
        {
            int val = 0;
            using (SqlConnection sqlConnection = new SqlConnection(m_connectionString))
            {
                string query = "DELETE FROM Jedis WHERE idJedi='@idJedi'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", _jedi.ID);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                val = 1;
            }
            return val;
        }

        public List<Caracteristique> GetAllCaracteristique()
        {
            List<Caracteristique> _allCarac = new List<Caracteristique>();
            using (SqlConnection sqlConnection = new SqlConnection(m_connectionString))
            {
            }

            return _allCarac;
        }

        public int EditJedi(Jedi _jedi)
        {
            /*command.Text = "UPDATE Student 
            SET Address = @add, City = @cit Where FirstName = @fn and LastName = @add";*/
            int val = 0;
            using (SqlConnection sqlConnection = new SqlConnection(m_connectionString))
            {
                string query = "UPDATE Jedis SET Name=@name, IsSith=@Sith, Pic=@pic WHERE idJedi=@idjedi";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@idjedi", _jedi.ID);
                sqlCommand.Parameters.AddWithValue("@name", _jedi.Nom);
                sqlCommand.Parameters.AddWithValue("@Sith", _jedi.IsSith);
                sqlCommand.Parameters.AddWithValue("@pic", _jedi.Image);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                val = 1;
            }
            return val;
        }
    }

}