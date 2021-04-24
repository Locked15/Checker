using System;
using System.Windows;
using System.Windows.Controls;
using Bool = System.Boolean;

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        /// <summary>
        /// Внутреннее поле, содержащее подсказку, содержащую пароль пользователя.
        /// </summary>
        ToolTip passwordTool = new ToolTip();

        /// <summary>
        /// Статическое поле, отвечающее за то, что сейчас регистрируется второй пользователь.
        /// </summary>
        static Bool secondUser = false;

        /// <summary>
        /// Статическое свойство, отвечающее за то, что сейчас регистрируется второй пользователь.
        /// </summary>
        public static Bool SecondUser
        {
            get
            {
                return secondUser;
            }

            set
            {
                secondUser = value;
            }
        }

        /// <summary>
        /// Конструктор класса. Нужен для работы данного окна.
        /// </summary>
        public Registration ()
        {
            InitializeComponent();

            GenderChoseBox.Items.Add("Мужской");
            GenderChoseBox.Items.Add("Женский");
            GenderChoseBox.Items.Add("Альтернативный");
        }

        /// <summary>
        /// Событие, возникающее при нажатии кнопки "CreateNewUserButton".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewUserButton_Click (object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NameInputBox.Text) || !BirthChose.SelectedDate.HasValue ||
            GenderChoseBox.SelectedItem == null || String.IsNullOrEmpty(PasswordInputBox.Password))
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

                UserProfile newUser = new UserProfile(NameInputBox.Text, PasswordInputBox.Password, gender, (DateTime)BirthChose.SelectedDate, 0, 0);

                if (PasswordInputBox.Password.Length < 5)
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

                    if (SecondUser)
                    {
                        Hub.SecondUser = newUser;

                        Close();
                    }

                    else
                    {
                        Hub.FirstUser = newUser;

                        Hub hub = new Hub();
                        hub.Show();
                        Close();
                    }
                }
            }
        }

        /// <summary>
        /// Событие, возникающее при включении опции "Отобразить пароль".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void SeeTheUnseen_Checked(object sender, RoutedEventArgs e)
        {
            passwordTool.Content = PasswordInputBox.Password;

            PasswordInputBox.ToolTip = passwordTool;
        }

        /// <summary>
        /// Событие, возникающее при отключении опции "Отобразить пароль".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void SeeTheUnseen_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordInputBox.ToolTip = null;
        }
    }
}
