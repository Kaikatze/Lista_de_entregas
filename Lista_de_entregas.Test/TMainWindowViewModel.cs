using Lista_de_entregas.Models;
using Lista_de_entregas.ViewModel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_entregas.Test
{
    [TestFixture]
    class TMainWindowViewModel
    {
        private MainWindowViewModel sutVM;
        private Mock<IEntregas> sutMock;
        

        [Test]
        public void TestaAdicionarEntregaNova()
        {
            sutMock = new Mock<IEntregas>();

            string algumaData = "12/11/2021";
            Entregas entregas = new Entregas() {IdCarga = 1, Endereco = "Rua Alabama", Cidade = " ", Estados = estados.SP, Frete = 1000, Peso = 10, DataEntrega = Convert.ToDateTime(algumaData)};

            Assert.That(sutVM.ListaDeEntregas, Has.Member(entregas));
        }

        [Test]
        public void TestAutoIncrementoId()
        {
            Entregas entragaUm = new Entregas() { IdCarga = 10 };

            Entregas entragaDois = new Entregas();

            sutVM.ListaDeEntregas.Add(entragaUm);

            entragaDois.IdCarga = sutVM.IncrementaMaxId();


            Assert.AreEqual(entragaDois.IdCarga, 11);
        }
    }
}
