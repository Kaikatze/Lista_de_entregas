
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

        public ObservableCollection<IEntregas> Entregas { get;  set; }
        public IEntregasContexto EntregasContexto { get; private set; }

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
           
            Entregas = new ObservableCollection<IEntregas>(EntregasContexto.SelectByID());
        }
        
       
        private void AddButton()
        {
            Entregas entregas = new();
            int maxID = 0;
            if (Entregas.Any())
            {
                maxID = Entregas.Max(max => max.IdCarga);
            }
            entregas.IdCarga = maxID + 1;
            WinRegister registerDialog = new WinRegister();
            registerDialog.DataContext = entregas;
            registerDialog.ShowDialog();
            
            
            if (registerDialog.DialogResult.HasValue && registerDialog.DialogResult.Value)
            {
                EntregasContexto.InsertData(entregas);
                EntregaSelecionada = entregas;
            }
            

            
        }

        private void DeleteButton()
        {
            if( EntregaSelecionada != null)
            {
                EntregasContexto.DeleteData(EntregaSelecionada);
                Entregas.Remove(EntregaSelecionada);
                EntregaSelecionada = (Entregas)Entregas.FirstOrDefault();
            }

        }


        private void EditButton()
        {
            
            WinRegister updateDialog = new WinRegister();
            updateDialog.DataContext = EntregaSelecionada;
            updateDialog.ShowDialog();

              if (updateDialog.DialogResult.HasValue && updateDialog.DialogResult.Value)
              {
                EntregasContexto.UpdateData(EntregaSelecionada);
              }
            
        }

    }
}
