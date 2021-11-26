using FluentValidation;
using FluentValidation.Results;
using Lista_de_entregas.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace Lista_de_entregas.Test
{
    [TestFixture]
    public class TValidaEntrega
    {
        
        private ValidaEntrega validaEntrega = new ValidaEntrega();

        [Test]
        public void TestaVeirificaValidacaoDaEntregaFalha()
        {
            Entregas entrega = new Entregas() { IdCarga = 1, Endereco = "Rua Alabama", Cidade = " ", Estados = estados.SP, Frete = 1, Peso = 1 };
            ValidationResult _validate = validaEntrega.Validate(entrega);
            Assert.IsTrue(_validate.IsValid);


        }
        [Test]
        public void TestaVeirificaValidacaoDaEntregaValido()
        {
            IEntregas entrega = new Entregas() { IdCarga = 3, Endereco = "Rua Augusta", Cidade = "Alguma Cidade", Estados = estados.SP, Frete = 1, Peso = 1 };
            ValidationResult _validate = validaEntrega.Validate(entrega);
            string resultado = _validate.ToString();

            Assert.IsEmpty(resultado);

        }

        

    }
}
