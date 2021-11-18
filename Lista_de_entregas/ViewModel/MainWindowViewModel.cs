
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
        private ValidaEntrega ValidaEntrega;

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
        }
        
       
        private void AddButton()
        {
            ValidaEntrega = new ValidaEntrega();
            _entregas = new Entregas();
            int maxID = 0;
            if (ListaDeEntregas.Any())
            {
                maxID = ListaDeEntregas.Max(max => max.IdCarga);
            }
            _entregas.IdCarga = maxID + 1;
            WinRegister registerDialog = new WinRegister();
            registerDialog.DataContext = _entregas;
            registerDialog.ShowDialog();
            _validate = ValidaEntrega.Validate(_entregas);
            
            
            if (registerDialog.DialogResult.HasValue && registerDialog.DialogResult.Value)
            {
                
                if (_validate.IsValid)
                {
                    EntregasContexto.InsertData(_entregas);
                    EntregaSelecionada = _entregas;
                    ListaDeEntregas.Add(_entregas);
                }
                else
                {
                    
                    foreach (ValidationFailure failure in _validate.Errors)
                    {
                        MessageBox.Show(failure.ErrorMessage);
                    }
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
            ValidaEntrega = new ValidaEntrega();
            WinRegister updateDialog = new WinRegister();
            updateDialog.DataContext = EntregaSelecionada;
            updateDialog.ShowDialog();
            _validate = ValidaEntrega.Validate(_entregas);

              if (updateDialog.DialogResult.HasValue && updateDialog.DialogResult.Value)
              {
                if (_validate.IsValid)
                {
                    EntregasContexto.UpdateData(EntregaSelecionada);
                }
                else
                {
                    foreach (ValidationFailure failure in _validate.Errors)
                    {
                        MessageBox.Show(failure.ErrorMessage);
                    }
                }
              }
            
        }

    }
}
