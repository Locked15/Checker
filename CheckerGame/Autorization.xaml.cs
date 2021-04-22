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

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        public Autorization()
        {
            InitializeComponent();
        }

        private void NewUserButton_Click(object sender, RoutedEventArgs e)
        {
            Registration register = new Registration();

            register.Show();
            Close();
        }

        private void EnterInExistingAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Entering enter = new Entering();

            enter.Show();
            Close();
        }
    }
}
