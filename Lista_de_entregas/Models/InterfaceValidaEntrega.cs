using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_entregas.Models
{
    public interface InterfaceValidaEntrega 
    {
        bool IsValid(IEntregas entregas);
        
    }
}
