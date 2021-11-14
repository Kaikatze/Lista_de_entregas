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
        void InsertData(IEntregas entregas);
        void DeleteData(IEntregas entregas);
        void UpdateData(IEntregas entregas);
        void SelectOrderByID();
        List<IEntregas> GetListaEntregas();
    }
}
