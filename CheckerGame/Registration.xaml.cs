using System;
using System.Windows;

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
            if (String.IsNullOrEmpty(NameInputBox.Text) || !BirthChose.SelectedDate.HasValue ||
            GenderChoseBox.SelectedItem == null || String.IsNullOrEmpty(PasswordInputBox.Text))
            {
                MessageBox.Show("Какое-либо поле не было заполнено.", "Ошибка!", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                UserGender gender;

                if (GenderChoseBox.SelectedItem.ToString() == "Мужской")
                {
                    gender = UserGender.Male;
                }

                else if (GenderChoseBox.SelectedItem.ToString() == "Женский")
                {
                    gender = UserGender.Female;
                }

                else
                {
                    gender = UserGender.Alternative;
                }

                UserProfile newUser = new UserProfile(NameInputBox.Text, PasswordInputBox.Text, gender, (DateTime)BirthChose.SelectedDate);

                if (PasswordInputBox.Text.Length < 5)
                {
                    MessageBox.Show("Пароль слишком короткий.", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else if (UserProfile.CheckAccountName(newUser))
                {
                    MessageBox.Show("Пользователь с таким именем уже определен.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    UserProfile.AddAccount(newUser);

                    Close();
                }
            }
        }
    }
}
