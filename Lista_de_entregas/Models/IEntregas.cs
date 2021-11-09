using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_entregas.Models
{
    public interface IEntregas
    {
        int IdCarga { get; set; }
        string Endereco { get; set; }
        string Cidade { get; set; }
        estados Estados { get; set; }
        double Frete { get; set; }
        double Peso { get; set; }
        DateTime DataEntrega { get; set; }
    }
}
