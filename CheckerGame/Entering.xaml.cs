using System.Windows;
using Bool = System.Boolean;

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для Entering.xaml
    /// </summary>
    public partial class Entering : Window
    {
        /// <summary>
        /// Статическое поле, отвечающее за то, что сейчас входит второй пользователь.
        /// </summary>
        static Bool secondUser = false;

        /// <summary>
        /// Статическое свойство, отвечающее за то, что сейчас входит второй пользователь.
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
        /// Конструктор класса. Необходим для работы окна.
        /// </summary>
        public Entering ()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "BeginEnterButton". Входит в аккаунт пользователя.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginEnterButton_Click (object sender, RoutedEventArgs e)
        {
            UserProfile profile = UserProfile.CheckAccount(UserNameInputBox.Text, PasswordInputBox.Text);

            if (profile == null)
            {
                MessageBox.Show("Аккаунт не найден.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                if (SecondUser)
                {
                    if (profile != Hub.FirstUser)
                    {
                        Hub.SecondUser = profile;

                        Close();
                    }

                    else
                    {
                        MessageBox.Show("Этот пользователь сейчас в Сети.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                
                else
                {
                    Hub.FirstUser = profile;

                    Hub hub = new Hub();
                    hub.Show();

                    Close();
                }
            }
        }
    }
}
