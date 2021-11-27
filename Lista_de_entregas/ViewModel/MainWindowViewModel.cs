
using FluentValidation.Results;
using Lista_de_entregas.DataBaseAcess;
using Lista_de_entregas.Models;
using Lista_de_entregas.ViewView;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lista_de_entregas.ViewModel
{
    public class MainWindowViewModel : NofityPropertyChanged

    {

        public ObservableCollection<IEntregas> ListaDeEntregas { get;  set; }
        public IEntregasContexto EntregasContexto { get; private set; }
        private ValidaEntrega Validador { get;  set; }

        private Entregas _entregas;

        private ValidationResult _validate;

        private Entregas _entregaSelecionada;
        public Entregas EntregaSelecionada
        {
            get { return _entregaSelecionada; }
            set { _entregaSelecionada = value; OnPropertyChanged("Entrega Selecionada"); }
        }

        public ICommand AdicionaComando { get;  set; }
        public ICommand DeletarComando { get; private set; }
        public ICommand AtualizaComando { get; private set; }


        public MainWindowViewModel()
        {
            AdicionaComando = new RelayCommand((param) => { AddButton(); });
            DeletarComando = new RelayCommand((param) => { DeleteButton(); });
            AtualizaComando = new RelayCommand((param) => { EditButton(); });

            EntregasContexto = new PostgreSQL();
            EntregasContexto.SelectOrderByID();
            ListaDeEntregas = new ObservableCollection<IEntregas>(EntregasContexto.GetListaEntregas());
            Validador = new ValidaEntrega();

        }

        public int IncrementaMaxId()
        {
            int maxID = 0;
            if (ListaDeEntregas.Any())
            {
                maxID = ListaDeEntregas.Max(max => max.IdCarga);
            }
            return maxID + 1;
        }

        private bool Confirmacao(Entregas entregas)
        {
            WinRegister registerDialog = new WinRegister();
            registerDialog.DataContext = entregas;
            registerDialog.ShowDialog();
            return (registerDialog.DialogResult.HasValue && registerDialog.DialogResult.Value);

        }

        public void AdcionaListaEntregas(Entregas entregas)
        {
            try
            {
                EntregasContexto.InsertData(entregas);
                EntregaSelecionada = entregas;
                ListaDeEntregas.Add(entregas);
            }
            catch (Exception err)
            {

                MessageBox.Show("Falha ao salvar a nova informação", err.Message);
            }
            finally
            {
                
            }
            

        }

        private void GeraErroInput(ValidationResult validate)
        {
            foreach (ValidationFailure failure in validate.Errors)
            {
                MessageBox.Show(failure.ErrorMessage);
            }
        }
        private void AddButton()
        {
            _entregas = new Entregas();

            _entregas.IdCarga = IncrementaMaxId();
            
            
            if (Confirmacao(_entregas))
            {
                _validate = Validador.Validate(_entregas);
                if (_validate.IsValid)
                {
                    AdcionaListaEntregas(_entregas);
                }
                else
                {
                    GeraErroInput(_validate);
                }
            }
            

            
        }

        private void DeleteButton()
        {
            if( EntregaSelecionada != null)
            {
                EntregasContexto.DeleteData(EntregaSelecionada);
                ListaDeEntregas.Remove(EntregaSelecionada);
            }

        }


        private void EditButton()
        {

              if (Confirmacao(EntregaSelecionada))
              {
                _validate = Validador.Validate(_entregas);

                if (_validate.IsValid)
                {
                    EntregasContexto.UpdateData(EntregaSelecionada);
                }
                else
                {
                    GeraErroInput(_validate);
                }
              }
            
        }

    }
}
