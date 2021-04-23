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
    /// Логика взаимодействия для Entering.xaml
    /// </summary>
    public partial class Entering : Window
    {
        public Entering()
        {
            InitializeComponent();
        }

        private void BeginEnterButton_Click(object sender, RoutedEventArgs e)
        {
            UserProfile profile = UserProfile.CheckAccount(UserNameInputBox.Text, PasswordInputBox.Text);

            if (profile == null)
            {
                MessageBox.Show("Аккаунт не найден.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                
            }
        }
    }
}
