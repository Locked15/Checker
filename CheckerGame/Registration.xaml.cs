using System;
using System.Windows;
using System.Windows.Controls;

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();

            GenderChoseBox.Items.Add("Мужской");
            GenderChoseBox.Items.Add("Женский");
            GenderChoseBox.Items.Add("Альтернативный");
        }

        private void CreateNewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NameInputBox.Text) || BirthChose.SelectedDate == null ||
            GenderChoseBox.SelectedItem == null || String.IsNullOrEmpty(PasswordInputBox.Text))
            {
                MessageBox.Show("Какое-либо поле не было заполнено.", "Ошибка!", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                if (PasswordInputBox.Text.Length < 5)
                {
                    MessageBox.Show("Пароль слишком короткий.", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {

                }
            }
        }
    }
}
