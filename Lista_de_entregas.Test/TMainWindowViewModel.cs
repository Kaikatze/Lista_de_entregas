using Lista_de_entregas.DataBaseAcess;
using Lista_de_entregas.Models;
using Lista_de_entregas.ViewModel;
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
        MainWindowViewModel sutVM = new MainWindowViewModel();

        [Test]
        public void TestaAdicionarEntregaNova()
        {
            string algumaData = "12/11/2021";
            Entregas entregas = new Entregas() {IdCarga = 1, Endereco = "Rua Alabama", Cidade = " ", Estados = estados.SP, Frete = 1000, Peso = 10, DataEntrega = Convert.ToDateTime(algumaData)};
            sutVM.AdcionaListaEntregas(entregas);

            Assert.That(sutVM.ListaDeEntregas, Has.Member(entregas));
        }
        [Test]
        public void TestAutoIncrementoId()
        {
            Entregas entregas = new Entregas() { IdCarga = 3 };
            sutVM.ListaDeEntregas.Add(entregas);

            sutVM.IncrementaMaxId();

            Assert.AreNotEqual(entregas.IdCarga = 3 , sutVM.ListaDeEntregas[0].IdCarga);
        }
    }
}
