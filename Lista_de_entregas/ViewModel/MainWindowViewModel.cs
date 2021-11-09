
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
        
        public MainWindowViewModel()
        {

            EntregasContexto = new PostgreSQL();
            Entregas = new ObservableCollection<IEntregas>(EntregasContexto.SelectByID());
            

        }
        
       
        //Botões e Comandos
        
        //Add commands
        ICommand adicionar;
        public ICommand AdicionaComando
        {
            get
            {
                return adicionar ?? (adicionar = new RelayCommand(x => { AddButton(); }));
                
            }
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

        //Delete commands
        public ICommand deletarcomando;
        public ICommand DeletarComando
        {
            get
            {
                return deletarcomando ?? (deletarcomando = new RelayCommand(x => { DeleteButton(); }));
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

        //Update commands
        ICommand atualizarcomando;
        public ICommand AtualizaComando
        {
            get
            {
                return atualizarcomando ?? (atualizarcomando = new RelayCommand(x => { EditButton(); }));
            }
        }

        private void EditButton()
        {
            if (EntregaSelecionada != null)
            {
                Entregas clonaEntrega = (Entregas)EntregaSelecionada.Clone();
                WinRegister updateDialog = new WinRegister();
                updateDialog.DataContext = clonaEntrega;
                updateDialog.ShowDialog();

                if (updateDialog.DialogResult.HasValue && updateDialog.DialogResult.Value)
                {
                    EntregaSelecionada.IdCarga = clonaEntrega.IdCarga;
                    EntregaSelecionada.Endereco = clonaEntrega.Endereco;
                    EntregaSelecionada.Cidade = clonaEntrega.Cidade;
                    EntregaSelecionada.Estados = clonaEntrega.Estados;
                    EntregaSelecionada.Frete = clonaEntrega.Frete;
                    EntregaSelecionada.Peso = clonaEntrega.Peso;
                    EntregaSelecionada.DataEntrega = clonaEntrega.DataEntrega;
                }
            }
        }

    }
}
