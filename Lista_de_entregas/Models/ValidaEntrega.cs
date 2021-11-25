using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_entregas.Models
{
    public class ValidaEntrega : AbstractValidator<IEntregas>
    {
        public ValidaEntrega()
        {
            RuleFor(x => x.IdCarga)
                .NotEqual(0);


            RuleFor(x => x.Endereco)
                .NotEmpty()
                .Length(6, 50);


            RuleFor(x => x.Cidade)
                .NotEmpty()
                .Length(3, 50);


            RuleFor(x => x.Frete)
                .NotEqual(0);

            RuleFor(x => x.Peso)
                .NotEqual(0);

            RuleFor(x => x.DataEntrega)
                .NotEmpty();

        }

        //public void ValidaId(Entregas )
        //{
        //    RuleFor(x => x.IdCarga)
        //        .NotEqual(0);
        //}
    }
}
