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
    public class PostgreSQL : IEntregasContexto
    {
        static string serverName = "localhost";
        static string porta = "5432";
        static string username = "postgres";
        static string password = "$321";
        static string database = "entregacargas";
        private string _conectaString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                                serverName, porta, username, password, database);
        NpgsqlConnection pgsqlConnection;
        public string ConectaString { get => _conectaString;  }
        public IEntregas Entrega { get; set; }
        public List<IEntregas> ListaEntregas { get; set; }

        public PostgreSQL() { }

        public PostgreSQL(IEntregas entrega)
        {
            Entrega = entrega;
        }

        public DataSet DataSet { get; set; }

        public ObservableCollection<Entregas> Entregas { get; set; }
        IEntregas IEntregasContexto.Entregas { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private void CriaConexao()
        {
            if (pgsqlConnection == null)
            {
                pgsqlConnection = new NpgsqlConnection(ConectaString);
            }
        }

        
        public void InsertData(IEntregas entregas)
        {

            try
            {
                CriaConexao();
                pgsqlConnection.Open();
                string cmdInserir = String.Format("Insert Into Entregas(idcarga,endereco,cidade,estado,frete,toneladas,datacarga)" +
                        " values({0},'{1}','{2}','{3}',{4},{5},'{6}')",
                          entregas.IdCarga.ToString(), entregas.Endereco,//Endereco & Cidade "String"
                          entregas.Cidade, entregas.Estados.ToString(),
                          entregas.Frete.ToString(), entregas.Peso.ToString(),
                          entregas.DataEntrega.ToString());
                NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdInserir, pgsqlConnection);
                pgsqlcommand.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            finally
            {
                pgsqlConnection.Close();
            }
        }


        public void DeleteData(IEntregas entregas)
        {
            try
            {
                CriaConexao();
                pgsqlConnection.Open();
                string cmdDeletar = String.Format("delete from entregas where idcarga = '{0}'", entregas.IdCarga.ToString());
                NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdDeletar, pgsqlConnection);
                pgsqlcommand.ExecuteNonQuery();

            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message);
            }
            finally
            {
                pgsqlConnection.Close();
            }

        }

        public List<IEntregas> SelectByID()
        {
            try
            {
                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConectaString))
                {
                    CriaConexao();
                    pgsqlConnection.Open();
                    string cmdSelect = "Select * from Entregas order by IdCarga";

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmdSelect, npgsqlConnection))
                    {
                        DataSet = new DataSet();
                        adapter.Fill(DataSet, "Entregas");
                    }

                    if( ListaEntregas == null)
                    {
                       ListaEntregas = new List<IEntregas>();
                    }

                    foreach (DataRow dataRow in DataSet.Tables[0].Rows)
                    {
                        IEntregas entregas = new Entregas();
                        entregas.IdCarga = (int)dataRow[0];
                        entregas.Endereco = dataRow[1].ToString();
                        entregas.Cidade = dataRow[2].ToString();
                        entregas.Estados = (estados)Enum.Parse(typeof(estados), dataRow[3].ToString(), true);
                        entregas.Frete = (double)(decimal)dataRow[4];
                        entregas.Peso = (double)(decimal)dataRow[5];
                        entregas.DataEntrega = DateTime.Parse(dataRow[6].ToString());

                        ListaEntregas.Add(entregas);
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

            return ListaEntregas;
        }

        public void UpdateData(IEntregas entregas)
        {
            throw new NotImplementedException();
        }
    }
}
