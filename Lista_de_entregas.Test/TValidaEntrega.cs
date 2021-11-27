using Lista_de_entregas.Models;
using NUnit.Framework;


namespace Lista_de_entregas.Test
{
    [TestFixture]
    public class TValidaEntrega
    {
        
        private ValidaEntrega resultadoEntrega = new ValidaEntrega();

        [Test]
        public void DeveRetornarFalseVeirificaValidacaoDaEntrega()
        {
            Entregas entrega = new Entregas() { IdCarga = -1, Endereco = "Rua Alabama", Cidade = "", Estados = estados.SP, Frete = 10, Peso = 1 };
            Assert.IsFalse(resultadoEntrega.IsValid(entrega), "Retorno tem que ser falso");


        }
        [Test]
        public void DeveRetornarTrueVeirificaValidacaoDaEntrega()
        {
            Entregas entrega = new Entregas() { IdCarga = 3, Endereco = "Rua Augusta", Cidade = "Alguma Cidade", Estados = estados.SP, Frete = 10, Peso = 1 };
            Assert.IsTrue(resultadoEntrega.IsValid(entrega), "Retorno tem que ser verdadeiro");

        }

        

    }
}
