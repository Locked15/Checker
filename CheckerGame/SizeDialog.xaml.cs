using System;
using System.Windows;

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для SizeDialog.xaml
    /// </summary>
    public partial class SizeDialog : Window, IDisposable
    {
        /// <summary>
        /// Статическое Поле, которое содержит размер игрового поля.
        /// </summary>
        Int32? size = null;

        /// <summary>
        /// Статическое Свойство, которое отвечает за размер игрового поля.
        /// </summary>
        public Int32? Size
        {
            get
            {
                return size;
            }

            private set
            {
                size = value;
            }
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public SizeDialog ()
        {
            InitializeComponent();

            SizeDes.Text = "Для того, чтобы начать игру Вам осталось сделать всего одну вещь.\n\nУкажите размер Доски:";
        }

        /// <summary>
        /// Событие, возникающее при нажатии на кнопку "ReadyToGameButton".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ReadyToGameButton_Click (object sender, RoutedEventArgs e)
        {
            Int32 currentSize;

            if (Int32.TryParse(SizeBox.Text, out currentSize))
            {
                if (currentSize < 4 || currentSize > 10)
                {
                    MessageBox.Show("Введен некорретный размер игрового поля.\n\nПоле должно быть не меньше 4 блоков и не более 10 блоков.",
                    "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    Size = currentSize;

                    Close();
                }
            }

            else
            {
                MessageBox.Show("Введено нераспознаное значение.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Реализация интерфейса IDisposable. Метод для уничтожения объекта.
        /// </summary>
        public void Dispose ()
        {
            //При вызове данного метода компилятор автоматически проставит код здесь. 
            //Так что здесь ничего писать не надо.
        }
    }
}
