using Lista_de_entregas.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_entregas.Models
{
    public class Entregas : NofityPropertyChanged, ICloneable
    {
        private int _idCarga;
        public int IdCarga 
        { get 
            { return _idCarga; } 
            set 
            { _idCarga = value; AvisaTela("IdCarga"); } 
        
        }

        private string _endereco;
        public string Endereco 
        { get 
            { return _endereco; }
            set
            { _endereco = value; AvisaTela("Endereco"); } 
        }

        private string _cidade;
        public string Cidade
        {
            get
            { return _cidade; }
            set
            { _cidade = value; AvisaTela("Cidade"); }
        }

        private estados _estados;
        public estados Estados 
        { get
            {return _estados; }
            set
            { _estados = value; AvisaTela("Estados"); }
        }

        private double _frete;
        public double Frete
        {
            get 
            { return _frete; }
            set
            { _frete = value; AvisaTela("Frete"); }
        }

        private double _peso;
        public double Peso 
        { get
            { return _peso; }
            set
            { _peso = value; AvisaTela("Peso"); } 
        }

        private DateTime _dataEntrega = DateTime.Now;
        public DateTime DataEntrega
        {
            get
            { return _dataEntrega; }
            set { _dataEntrega = value; AvisaTela("DataEntrega"); }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
