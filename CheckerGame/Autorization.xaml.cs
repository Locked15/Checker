﻿using System.Windows;

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        /// <summary>
        /// Конструктор класса. Необходим для работы окна.
        /// </summary>
        public Autorization ()
        {
            InitializeComponent();

            UserProfile.RefreshAccounts();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "NewUserButton". Открывает окно регистрации нового пользователя. 
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void NewUserButton_Click (object sender, RoutedEventArgs e)
        {
            Registration register = new Registration();

            register.Show();
            Close();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "EnterInExistingAccountButton". Открывает окно для входа в существующий аккаунт.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EnterInExistingAccountButton_Click (object sender, RoutedEventArgs e)
        {
            Entering enter = new Entering();

            enter.Show();
            Close();
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "EnterInInformationMenuButton". Открывает окно информации.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EnterInInformationMenuButton_Click (object sender, RoutedEventArgs e)
        {
            DataWindow informationWindow = new DataWindow();
            informationWindow.Show();

            Close();
        }
    }
}
