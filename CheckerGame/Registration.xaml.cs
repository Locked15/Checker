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

            BirthChose.DisplayDateEnd = DateTime.UtcNow;
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

                //Определение пола пользователя:
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

                //Создание экземпляра класса 'UserProfile':
                UserProfile newUser = new UserProfile(NameInputBox.Text, PasswordInputBox.Password, gender, (DateTime)BirthChose.SelectedDate, 0, 0, 0);

                //Проверка на длину пароля:
                if (PasswordInputBox.Password.Length < 5)
                {
                    MessageBox.Show("Пароль слишком короткий.", "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //Проверка на существование в системе пользователя с таким именем:
                else if (UserProfile.CheckAccountName(newUser))
                {
                    MessageBox.Show("Пользователь с таким именем уже определен.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //Проверка на возраст пользователя:
                else if (DateTime.UtcNow.Year - newUser.BirthTime.Year < 12)
                {
                    MessageBox.Show("Ваш возраст слишком мал, чтобы зарегистрироваться в данном приложении.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //Если аккаунт полностью соответствует условиям, он добавляется в систему:
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
        private void SeeTheUnseen_Checked (object sender, RoutedEventArgs e)
        {
            passwordTool.Content = PasswordInputBox.Password;

            PasswordInputBox.ToolTip = passwordTool;
        }

        /// <summary>
        /// Событие, возникающее при отключении опции "Отобразить пароль".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void SeeTheUnseen_Unchecked (object sender, RoutedEventArgs e)
        {
            PasswordInputBox.ToolTip = null;
        }

        /// <summary>
        /// Событие, возникающее при изменении введенного пароля.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void PasswordInputBox_PasswordChanged (object sender, RoutedEventArgs e)
        {
            if ((Bool)SeeTheUnseen.IsChecked)
            {
                passwordTool.Content = PasswordInputBox.Password;
            }
        }
    }
}
