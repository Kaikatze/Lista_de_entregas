using Lista_de_entregas.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lista_de_entregas.DataBaseAcess
{
    class SQLServer : IEntregasContexto
    {
        private static string serverName = "VITORIA";
        private static string userID = "sa";
        private static string password = "$321";
        private static string dataBase = "entregas";

        private string ConectaString = String.Format("Server={0};User Id={1};Password={2};Database={3};",
                                                                         serverName,  userID, password, dataBase);

        private SqlConnection  sqlConnection;
        private SqlCommand comando;
        private List<IEntregas> ListaEntregas;
        private SqlDataReader dataReader;

        public SQLServer()
        {
            sqlConnection = new SqlConnection(ConectaString);
            comando = new SqlCommand();
            comando.Connection = sqlConnection;
            ListaEntregas = new List<IEntregas>();
        }

        private void ExecutaCommando()
        {

            try

            {
                sqlConnection.Open();
                this.comando.ExecuteNonQuery();

            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message, " A operação não foi realizada com êxito!");
            }
            finally
            {
                sqlConnection.Close();
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
            string cmdInserir = String.Format("Insert Into Entregas(IdCarga, Endereco, Cidade, Estados, Frete, Peso, DataEntrega)" +
                        " values({0},'{1}','{2}','{3}','{4}','{5}','{6}')",
                          entregas.IdCarga.ToString(), entregas.Endereco,
                          entregas.Cidade, entregas.Estados.ToString(),
                          entregas.Frete.ToString(), entregas.Peso.ToString(),
                          entregas.DataEntrega.Date.ToString("mm/dd/yyy"));
            CriaComando(cmdInserir);
            ExecutaCommando();
        }

        public void DeleteData(IEntregas entregas)
        {
            string cmdDeletar = String.Format("delete from entregas where idcarga = '{0}'", entregas.IdCarga.ToString());
            CriaComando(cmdDeletar);
            ExecutaCommando();
        }

        public void UpdateData(IEntregas entregas)
        {
            string cmdAtualizar = String.Format("Update Entregas set Endereco = '{0}', Cidade = '{1}', Estados = '{2}', Frete = '{3}', Peso = '{4}', DataEntrega = '{5}' where IdCarga = '{6}'",
                                                entregas.Endereco, entregas.Cidade, entregas.Estados.ToString(), entregas.Frete.ToString(), entregas.Peso.ToString(),
                                                entregas.DataEntrega.ToString(), entregas.IdCarga.ToString());
            CriaComando(cmdAtualizar);
            ExecutaCommando();
        }

        public void SelectOrderByID()
        {

            try
            {
                string select = "Select * from Entregas order by IdCarga";
                sqlConnection.Open();
                CriaComando(select);
                
                dataReader = this.comando.ExecuteReader();


                if (dataReader.HasRows)

                {

                    while (dataReader.Read())
                    {
                        IEntregas entregas = TratamentoDadosEntrega(dataReader);

                        ListaEntregas.Add(entregas);
                    }

                }

            }
            catch (SqlException error)
            {
                MessageBox.Show(error.ToString(), " Operação não realizada! ");
            }
            finally
            {
                sqlConnection.Close();
                dataReader.Close();
            }

        }

        private IEntregas TratamentoDadosEntrega(SqlDataReader dataReader)
        {
            IEntregas entregas = new Entregas();
            try
            {
                entregas.IdCarga = dataReader.GetInt32(0);
                entregas.Endereco = dataReader.GetString(1);
                entregas.Cidade = dataReader.GetString(2);
                entregas.Estados = Enum.Parse<estados>(dataReader.GetString(3));
                entregas.Frete = (double)dataReader.GetDecimal(4);
                entregas.Peso = (double)dataReader.GetDecimal(5);
                entregas.DataEntrega = dataReader.GetDateTime(6);
            }
            catch (Exception error)
            {

                MessageBox.Show(error.ToString(), " Operação não realizada");
            }
            return entregas;
        }

        public List<IEntregas> GetListaEntregas()
        {
            return this.ListaEntregas;
        }
    }
}

