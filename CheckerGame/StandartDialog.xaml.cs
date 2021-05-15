using System;
using System.IO;
using System.Windows;
using Bool = System.Boolean;
using Colour = System.Windows.Media.Color;
using SolidColourBrush = System.Windows.Media.SolidColorBrush;

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для StandartDialog.xaml
    /// </summary>
    public partial class StandartDialog : Window, IDisposable
    {
        /// <summary>
        /// Логическое Поле, содержащее значение, отвечающее за преждевременное закрытие окна.
        /// </summary>
        Bool error = false;

        /// <summary>
        /// Поле, содержащее значение первого поля для ввода.
        /// </summary>
        String firstBox;

        /// <summary>
        /// Поле, содержащее значение второго поля для ввода.
        /// </summary>
        String secondBox;

        /// <summary>
        /// Поле, содержащее элемент перечисления "Enum", определяющего тип диалогового окна.
        /// </summary>
        DialogVariant variant;

        /// <summary>
        /// Логическое Свойство, содержащее значение, отвечающее за преждевременное закрытие окна.
        /// </summary>
        public Bool Error
        {
            get
            {
                return error;
            }

            private set
            {
                error = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее значение первого поля для ввода.
        /// </summary>
        public String FirstBoxText
        {
            get
            {
                return firstBox;
            }

            private set
            {
                firstBox = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее значение второго поля для ввода.
        /// </summary>
        public String SecondBoxText
        {
            get
            {
                return secondBox;
            }

            private set
            {
                secondBox = value;
            }
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="variant">Вариант диалогового окна для отображения.</param>
        public StandartDialog (DialogVariant variant)
        {
            this.variant = variant;

            InitializeComponent();

            if (variant == DialogVariant.AccountDeleting)
            {
                DialogDes.Text = "Укажите логин и пароль аккаунта.";

                FirstDes.Text = "Логин аккаунта:";
                SecondDes.Text = "Пароль аккаунта:"; 
            }
        }
        
        /// <summary>
        /// Событие, возникающее при наведении мыши на кнопку "ReadyButton".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ReadyButton_MouseEnter (object sender, System.Windows.Input.MouseEventArgs e)
        {
            ReadyButton.Foreground = new SolidColourBrush(Colour.FromRgb(0, 0, 0));
        }

        /// <summary>
        /// Событие, возникающее при выведении мыши из области кнопки "ReadyButton".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ReadyButton_MouseLeave (object sender, System.Windows.Input.MouseEventArgs e)
        {
            //Это значение RGB для цвета "AntiqueWhite".
            ReadyButton.Foreground = new SolidColourBrush(Colour.FromRgb(250, 235, 215));
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "ReadyButton". Выполняет предназначенную для диалогового окна задачу.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ReadyButton_Click (object sender, RoutedEventArgs e)
        {
            if (variant == DialogVariant.FileCreation)
            {
                if (Directory.Exists(FirstBox.Text) && !String.IsNullOrEmpty(SecondBox.Text))
                {
                    FirstBoxText = FirstBox.Text;
                    SecondBoxText = SecondBox.Text;

                    Error = !Error;

                    Close();
                }

                else
                {
                    MessageBox.Show("Указан некорректный путь.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else
            {
                UserProfile profileToDelete = UserProfile.CheckAccount(FirstBox.Text, SecondBox.Text);

                if (profileToDelete != null)
                {
                    FirstBoxText = FirstBox.Text;
                    SecondBoxText = SecondBox.Text;

                    Error = !Error;

                    Close();
                }

                else
                {
                    MessageBox.Show("Аккаунт не найден.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Событие, возникающее при закрытии окна.
        /// </summary>
        /// <param name="sender">Объект, вызывавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void StandartDialog_Closed (object sender, EventArgs e)
        {
            Error = !Error;
        }

        /// <summary>
        /// Метод для уничтожения экзмепляра класса и освобождения ресурсов.
        /// </summary>
        public void Dispose ()
        {
            //Весь прочий код будет проставлен компилятором автоматически.
            //Здесь ничего писать не надо.
        }
    }

    /// <summary>
    /// Перечисление, определяющие тип диалогового окна.
    /// </summary>
    public enum DialogVariant
    {
        /// <summary>
        /// Значение для диалогового окна по созданию файла.
        /// </summary>
        FileCreation,

        /// <summary>
        /// Значение для диалогового окна по удалению аккаунта.
        /// </summary>
        AccountDeleting
    }
}
