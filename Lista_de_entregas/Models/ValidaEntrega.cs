using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_entregas.Models
{
    public class ValidaEntrega : AbstractValidator<IEntregas>, InterfaceValidaEntrega
    {
        public ValidaEntrega()
        {
            RuleFor(x => x.IdCarga)
                .GreaterThan(0);


            RuleFor(x => x.Endereco)
                .NotEmpty()
                .Length(6, 50);


            RuleFor(x => x.Cidade)
                .NotEmpty()
                .Length(3, 50);


            RuleFor(x => x.Frete)
                .GreaterThan(10);

            RuleFor(x => x.Peso)
                .NotEqual(0);

            //RuleFor(x => x.DataEntrega)
            //    .NotEmpty();
        }

        public bool IsValid(IEntregas entrega)
        {
            ValidationResult result = Validate(entrega);
            if (result.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
