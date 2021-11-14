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
        
        public PostgreSQL()  
        {
            pgsqlConnection = new NpgsqlConnection(ConectaString);

            comando = new NpgsqlCommand();
            comando.Connection = pgsqlConnection;
            ListaEntregas = new List<IEntregas>();

        }
        
        //private void CriaConexao()
        //{
        //    if (pgsqlConnection == null)
        //    {
        //        //pgsqlConnection = new NpgsqlConnection();
        //    }
        //}

        private void ExecutaCommando()
        {
            
            try

            {
                pgsqlConnection.Open();
                this.comando.ExecuteNonQuery();

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


        private void CriaComando(string comandoText, int comandoTimeOut = 15)
        {
            this.comando.CommandText = comandoText;
            this.comando.CommandTimeout = comandoTimeOut;
            this.comando.CommandType = CommandType.Text;
           
            
        }

        public void InsertData(IEntregas entregas)
        {
            string cmdInserir = String.Format("Insert Into Entregas(idcarga,endereco,cidade,estado,frete,toneladas,datacarga)" +
                        " values({0},'{1}','{2}','{3}','{4}','{5}','{6}')",
                          entregas.IdCarga.ToString(), entregas.Endereco,//Endereco & Cidade "String"
                          entregas.Cidade, entregas.Estados.ToString(),
                          entregas.Frete.ToString(), entregas.Peso.ToString(),
                          entregas.DataEntrega.ToString());
            CriaComando(cmdInserir);
            ExecutaCommando();
        }

        public void DeleteData(IEntregas entregas)
        {
            string cmdDeletar = String.Format("delete from entregas where idcarga = '{0}'", entregas.IdCarga.ToString());
            CriaComando(cmdDeletar);
            ExecutaCommando();
        }

        //Funcionando errado - Atualiza todas as Rows da tabela.
        public void UpdateData(IEntregas entregas)
        {
            string cmdAtualizar = String.Format("Update Entregas set Endereco = '{0}', Cidade = '{1}', Estado = '{2}', Frete = '{3}', Toneladas = '{4}', DataCarga = '{5}' where IdCarga = '{6}'",
                                                entregas.Endereco, entregas.Cidade, entregas.Estados.ToString(), entregas.Frete.ToString(), entregas.Peso.ToString(),
                                                entregas.DataEntrega.ToString(), entregas.IdCarga.ToString());
            CriaComando(cmdAtualizar);
            ExecutaCommando();
        }

        public void SelectOrderByID()
        {

            try
            {
                pgsqlConnection.Open();
                string select = "Select * from Entregas order by IdCarga";

               CriaComando(select);
                NpgsqlDataReader dataReader = this.comando.ExecuteReader();

                
                if (dataReader.HasRows)

                {

                    while (dataReader.Read())
                    {
                        IEntregas entregas = TratamentoDadosEntrega(dataReader);

                        ListaEntregas.Add(entregas);
                    }
                    dataReader.Close();
                }

            }
            catch (NpgsqlException error)
            {
                MessageBox.Show(error.ToString());
            }
            finally
            {
                pgsqlConnection.Close();
            }

        }

        private IEntregas TratamentoDadosEntrega(NpgsqlDataReader dataReader )
        {
            IEntregas entregas = new Entregas();
            try
            {
                entregas.IdCarga = (int)dataReader[0];
                entregas.Endereco = dataReader[1].ToString();
                entregas.Cidade = dataReader[2].ToString();
                entregas.Estados = (estados)Enum.Parse(typeof(estados), dataReader[3].ToString(), true);
                entregas.Frete = (double)(decimal)dataReader[4];
                entregas.Peso = (double)(decimal)dataReader[5];
                entregas.DataEntrega = DateTime.Parse(dataReader[6].ToString());
            }
            catch (Exception error)
            {

                MessageBox.Show(error.ToString());
            }
            return entregas;
        }

        public List<IEntregas> GetListaEntregas()
        {
            return this.ListaEntregas;
        }
    }
}
