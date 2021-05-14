using System;
using System.Windows;

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для Hub.xaml
    /// </summary>
    public partial class Hub : Window
    {
        /// <summary>
        /// Поле, содержащее первого игрока.
        /// </summary>
        static UserProfile firstUser;
        /// <summary>
        /// Поле, содержащее второго игрока.
        /// </summary>
        static UserProfile secondUser;

        /// <summary>
        /// Свойство, содержащее первого игрока.
        /// </summary>
        public static UserProfile FirstUser
        {
            get
            {
                return firstUser;
            }

            set
            {
                firstUser = value;
            }
        }
        /// <summary>
        /// Свойство, содержащее второго игрока.
        /// </summary>
        public static UserProfile SecondUser
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
        public Hub()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие, возникающее при загрузке окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HubWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Double winRate;

            winRate = Math.Round(Convert.ToDouble(FirstUser.Wins * 1.0 / (FirstUser.AllGames * 1.0)), 2);

            //Проверка на NaN. По определению NaN он никогда не равен себе, поэтому здесь проставлено такое условие.
            //ЭТО НЕ ОШИБКА!
            if (winRate != winRate)
            {
                winRate = 0.0;
            }

            FirstUserDes.Text += '\n' + FirstUser.Name;

            FirstUserWins.Text = FirstUser.Wins.ToString() + ';';
            FirstUserAllGames.Text = FirstUser.AllGames.ToString() + ';';
            FirstUserWinsProcent.Text = (winRate * 100).ToString() + "%;";
            FirstUserLeaves.Text = FirstUser.Leaves.ToString() + '.';

            Entering.SecondUser = true;
            Registration.SecondUser = true;
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "DeleteUser". Удаляет аккаунт первого пользователя и закрывает текущую сессию игры.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы ТОЧНО уверены?\nАккаунт будет удален безвозвратно!", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                FirstUser.DeleteUser(FirstUser);

                if (SecondUser != null)
                {
                    QuitSecondButton_Click(sender, new RoutedEventArgs());
                }

                Autorization newWindow = new Autorization();

                newWindow.Show();
                Close();
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "EnterSecondButton". Раскрывает окно входа для Второго Пользователя.
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EnterSecondButton_Click(object sender, RoutedEventArgs e)
        {
            Entering secondEnter = new Entering();
            secondEnter.ShowDialog();

            if (SecondUser != null)
            {
                SecondUserDes.Text += '\n' + SecondUser.Name;

                SecondUserConnected();
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "RegisterSecondButton". Раскрывает окно Регистрации для Второго Пользователя.
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void RegisterSecondButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registerSecond = new Registration();
            registerSecond.ShowDialog();

            if (SecondUser != null)
            {
                SecondUserDes.Text += '\n' + SecondUser.Name;

                SecondUserConnected();
            }
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "QuitSecondButton". Выходит из аккаунта Второго Пользователя.
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void QuitSecondButton_Click(object sender, RoutedEventArgs e)
        {
            SecondUserDes.Text = "Второй Пользователь:";

            SecondUser = null;
            SecondUserDisconnected();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "StartCheckerMatch". Запускает игру в шашки и скрывает данное окно.
        /// </summary>
        /// <param name="sender">Объект, который вызвал событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void StartCheckerMatch_Click(object sender, RoutedEventArgs e)
        {
            if (SecondUser == null)
            {
                MessageBox.Show("Второй пользователь отсутствует.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                SizeDialog dialog = new SizeDialog();
                dialog.ShowDialog();

                if (dialog.Size.HasValue)
                {
                    MainWindow game = new MainWindow(this, dialog.Size.Value);
                    game.Show();

                    Hide();

                    dialog.Dispose();
                }
            }
        }

        /// <summary>
        /// Метод для обновления элементов управления при подключении Второго Пользователя.
        /// </summary>
        private void SecondUserConnected()
        {
            Double winRate;

            winRate = Math.Round(SecondUser.Wins * 1.0 / (SecondUser.AllGames * 1.0), 2);

            //Проверка на NaN. По определению NaN он никогда не равен себе, поэтому здесь проставлено такое условие.
            //ЭТО НЕ ОШИБКА!
            if (winRate != winRate)
            {
                winRate = 0.0;
            }

            SecondUserWinsDes.Visibility = Visibility.Visible;
            SecondUserAllGamesDes.Visibility = Visibility.Visible;
            SecondUserWinsProcentDes.Visibility = Visibility.Visible;
            SecondUserLeavesDes.Visibility = Visibility.Visible;

            SecondUserWins.Text = " " + SecondUser.Wins.ToString() + ';';
            SecondUserAllGames.Text = " " + SecondUser.AllGames.ToString() + ';';
            SecondUserWinsProcent.Text = " " + (winRate * 100).ToString() + '%' + ';';
            SecondUserLeaves.Text = SecondUser.Leaves.ToString() + '.';
        }

        /// <summary>
        /// Метод для обновления элементов управления при отключении Второго Пользователя.
        /// </summary>
        private void SecondUserDisconnected()
        {
            SecondUserWinsDes.Visibility = Visibility.Hidden;
            SecondUserAllGamesDes.Visibility = Visibility.Hidden;
            SecondUserWinsProcentDes.Visibility = Visibility.Hidden;
            SecondUserLeavesDes.Visibility = Visibility.Hidden;

            SecondUserWins.Text = "";
            SecondUserAllGames.Text = "";
            SecondUserWinsProcent.Text = "";
            SecondUserLeaves.Text = "";
        }

        /// <summary>
        /// Метод для обновления свойств с Пользователями при завершении игры в Шашки.
        /// </summary>
        public void RefreshInformation()
        {
            UserProfile.RefreshAccounts();

            FirstUser = UserProfile.GetFromName(FirstUser.Name);
            SecondUser = UserProfile.GetFromName(SecondUser.Name);

            FirstUserWins.Text = FirstUser.Wins.ToString();
            FirstUserAllGames.Text = FirstUser.AllGames.ToString();
            FirstUserWinsProcent.Text = ((FirstUser.Wins / FirstUser.AllGames) * 100).ToString() + '%';

            SecondUserConnected();
        }
    }
}
