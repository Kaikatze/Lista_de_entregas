using Lista_de_entregas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_entregas.DataBaseAcess
{
    public interface IEntregasContexto
    {
        IEntregas Entregas { get; set; }
        List<IEntregas> ListaEntregas { get; set; }
        void InsertData(IEntregas entregas);
        void DeleteData(IEntregas entregas);
        void UpdateData(IEntregas entregas);
        List<IEntregas> SelectByID();
    }
}
