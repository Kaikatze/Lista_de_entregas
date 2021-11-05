using Lista_de_entregas.Models;
using Lista_de_entregas.ViewModel;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lista_de_entregas.DataBaseAcess
{
    public class PostgreSQL 
    {
        static string serverName = "localhost";
        static string porta = "5432";
        static string username = "postgres";
        static string password = "$321";
        static string database = "entregacargas";
        NpgsqlConnection pgsqlConnection;

        private string conexaoStr = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                                serverName, porta, username, password, database);
        public string ConectaString { get => conexaoStr;  }


        public int IdCarga { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public estados Estados { get; set; }
        public float Frete { get; set; }
        public float Peso { get; set; }
        public DateTime DataEntrega { get; set; }
        public DataSet DataSet { get; set; }

        public ObservableCollection<Entregas> Entregas { get; set; }

        private void CriaConexao()
        {
            if (this.pgsqlConnection == null)
            {
                this.pgsqlConnection = new NpgsqlConnection(ConectaString);
            }
        }

        
        public void InsertData()
        {

            //this.CriaCargaView();
            this.CriaConexao();
            using (this.pgsqlConnection)
            {
                pgsqlConnection.Open();

                string cmdInserir = String.Format("Insert Into Entregas(idcarga,endereco,cidade,estado,frete,toneladas,datacarga) values({0},'{1}','{2}','{3}',{4},{5},'{6}')", IdCarga.ToString(), Endereco,
                                                                                                                                    Cidade, Estados.ToString(), Frete.ToString(), Peso.ToString(), DataEntrega.ToString());

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }

                pgsqlConnection.Close();

            }
        }

       
        public void DeleteData()
        {
            this.CriaConexao();
            using (this.pgsqlConnection)
            {
                pgsqlConnection.Open();
                string cmdDeletar = String.Format("delete from entregas where idcarga = '{0}'", IdCarga.ToString());

                using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdDeletar, pgsqlConnection))
                {
                    pgsqlcommand.ExecuteNonQuery();
                }

                pgsqlConnection.Close();
            }

        }

        public ObservableCollection<Entregas> SelectByID()
        {
            CriaConexao();
            try
            {
                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConectaString))
                {
                    pgsqlConnection.Open();
                    string cmdSelect = "Select * from Entregas";

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmdSelect, npgsqlConnection))
                    {
                        DataSet = new DataSet();
                        adapter.Fill(DataSet, "Entregas");
                    }

                    if( Entregas == null)
                    {
                       Entregas = new ObservableCollection<Entregas>();
                    }

                    foreach (DataRow dataRow in DataSet.Tables[0].Rows)
                    {
                        Entregas entregas = new Entregas();
                        entregas.IdCarga = (int)dataRow[0];
                        entregas.Endereco = dataRow[1].ToString();
                        entregas.Cidade = dataRow[2].ToString();
                        entregas.Estados = (estados)Enum.Parse(typeof(estados), dataRow[3].ToString(), true);
                        entregas.Frete = (double)(decimal)dataRow[4];
                        entregas.Peso = (double)(decimal)dataRow[5];
                        entregas.DataEntrega = DateTime.Parse(dataRow[6].ToString());

                        Entregas.Add(entregas);
                    }
                }
            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message);
            }
            finally
            {
                pgsqlConnection.Close();
                DataSet = null;
            }

            return Entregas;
        }



    }
}
