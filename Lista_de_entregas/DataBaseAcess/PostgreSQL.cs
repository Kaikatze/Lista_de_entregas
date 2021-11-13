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
        private static string serverName = "localhost";
        private static string porta = "5432";
        private static string username = "postgres";
        private static string password = "$321";
        private static string dataBase = "entregacargas";
        
        private string ConectaString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                                                 serverName, porta, username, password, dataBase);

        private NpgsqlConnection pgsqlConnection;
        private NpgsqlCommand comando;
        private List<IEntregas> ListaEntregas;
        private DataSet DataSet;
        private NpgsqlDataReader DataReader;
        
        public PostgreSQL()  
        {
            pgsqlConnection = new NpgsqlConnection(ConectaString);
            comando = new NpgsqlCommand();
            comando.Connection = pgsqlConnection;
            DataSet = new DataSet();
            ListaEntregas = new List<IEntregas>();

        }
        
        private void CriaConexao()
        {
            if (pgsqlConnection == null)
            {
                pgsqlConnection = new NpgsqlConnection();
            }
        }

        private void ExecutaCommando(string comandoText)
        {
            try
            {   
                CriaConexao();
                pgsqlConnection.Open();
                NpgsqlCommand comando = CriaComando(comandoText);
                comando.ExecuteNonQuery();

            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message);
            }
            finally
            {
                pgsqlConnection.Close();
                if (DataSet != null)
                {
                    DataSet = null;
                }
                

            }

        }

        private NpgsqlCommand CriaComando(string comandoText, int comandoTimeOut = 15)
        {
            comando.CommandText = comandoText;
            comando.CommandTimeout = comandoTimeOut;
            comando.CommandType = CommandType.Text;
           
            return comando;
        }

        public void InsertData(IEntregas entregas)
        {
            string cmdInserir = String.Format("Insert Into Entregas(idcarga,endereco,cidade,estado,frete,toneladas,datacarga)" +
                        " values({0},'{1}','{2}','{3}','{4}','{5}','{6}')",
                          entregas.IdCarga.ToString(), entregas.Endereco,//Endereco & Cidade "String"
                          entregas.Cidade, entregas.Estados.ToString(),
                          entregas.Frete.ToString(), entregas.Peso.ToString(),
                          entregas.DataEntrega.ToString());
            ExecutaCommando(cmdInserir);
        }

        public void DeleteData(IEntregas entregas)
        {
            string cmdDeletar = String.Format("delete from entregas where idcarga = '{0}'", entregas.IdCarga.ToString());
            ExecutaCommando(cmdDeletar);
        }

        //Funcionando errado - Atualiza todas as Rows da tabela.
        public void UpdateData(IEntregas entregas)
        {
            string cmdAtualizar = String.Format("Update Entregas set Endereco = '{0}', Cidade = '{1}', Estado = '{2}', Frete = '{3}', Toneladas = '{4}', DataCarga = '{5}'",
                                                entregas.Endereco, entregas.Cidade, entregas.Estados.ToString(), entregas.Frete.ToString(), entregas.Peso.ToString(),
                                                entregas.DataEntrega.ToString());
            ExecutaCommando(cmdAtualizar);
        }

        public List<IEntregas> SelectOrderByID()
        {

            try
            {
                CriaConexao();
                pgsqlConnection.Open();
                comando.CommandText = "Select * from Entregas order by IdCarga";
                comando.CommandTimeout = 15;
                comando.Connection = comando.Connection;
                comando.CommandType = CommandType.Text;

                DataReader = comando.ExecuteReader();

                if (DataReader.HasRows)
                {
                    while (DataReader.Read())
                    {
                        IEntregas entregas = new Entregas();
                        entregas.IdCarga = (int)DataReader[0];
                        entregas.Endereco = DataReader[1].ToString();
                        entregas.Cidade = DataReader[2].ToString();
                        entregas.Estados = (estados)Enum.Parse(typeof(estados), DataReader[3].ToString(), true);
                        entregas.Frete = (double)(decimal)DataReader[4];
                        entregas.Peso = (double)(decimal)DataReader[5];
                        entregas.DataEntrega = DateTime.Parse(DataReader[6].ToString());

                        ListaEntregas.Add(entregas);
                    }
                    DataReader.Close();
                }

                return ListaEntregas;
            }
            catch (NpgsqlException error)
            {
                MessageBox.Show(error.ToString());
                return null;
            }

            finally
            {
                pgsqlConnection.Close();
            }
        }

      

    }
}
