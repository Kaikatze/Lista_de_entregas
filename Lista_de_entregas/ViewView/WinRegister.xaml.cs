using Lista_de_entregas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lista_de_entregas.ViewView
{
    /// <summary>
    /// Lógica interna para WinRegister.xaml
    /// </summary>
    public partial class WinRegister : Window
    {
        public WinRegister()
        {
            InitializeComponent();
            EstadoComboBox.ItemsSource = Enum.GetValues(typeof(estados)).Cast<estados>();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
