using System.Windows;
using Bool = System.Boolean;

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        public DataWindow ()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие, вызываемое при нажатии на кнопку "GetExcelListButton". Выводит данные об активных аккаунтах в документ Excel.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GetExcelListButton_Click (object sender, RoutedEventArgs e)
        {
            if (UserProfile.CheckActiveAccounts())
            {
                StandartDialog excelDialog = new StandartDialog(DialogVariant.FileCreation);
                excelDialog.ShowDialog();

                if (!excelDialog.Error)
                {
                    Bool question = MessageBox.Show("Открыть документ после его создания?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.Yes;

                    UserProfile.CreateExcelUserList(excelDialog.FirstBoxText, excelDialog.SecondBoxText, question);
                }

                excelDialog.Dispose();
            }
        }

        /// <summary>
        /// Событие, вызываемое при нажатии на кнопку "GetWordListButton". Выводит данные об активных аккаунтах в документ Word.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GetWordListButton_Click (object sender, RoutedEventArgs e)
        {
            if (UserProfile.CheckActiveAccounts())
            {
                StandartDialog wordDialog = new StandartDialog(DialogVariant.FileCreation);
                wordDialog.ShowDialog();

                if (!wordDialog.Error)
                {
                    Bool question = MessageBox.Show("Открыть документ после его создания?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.Yes;

                    UserProfile.CreateWordUserList(wordDialog.FirstBoxText, wordDialog.SecondBoxText, question);
                }

                wordDialog.Dispose();
            }
        }

        /// <summary>
        /// Событие, вызываемое при нажатии на кнопку "DeleteAccountButton". Удаляет указанный аккаунт, если будет пройдена проверка безопасности.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void DeleteAccountButton_Click (object sender, RoutedEventArgs e)
        {
            if (UserProfile.CheckActiveAccounts())
            {
                StandartDialog deleteDialog = new StandartDialog(DialogVariant.AccountDeleting);
                deleteDialog.ShowDialog();

                UserProfile profileToDelete = UserProfile.CheckAccount(deleteDialog.FirstBoxText, deleteDialog.SecondBoxText);

                if (!deleteDialog.Error)
                {
                    if (MessageBox.Show("Вы ТОЧНО уверены?\nАккаунт будет удален безвозвратно!", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                    == MessageBoxResult.Yes)
                    {
                        profileToDelete.DeleteUser();
                    }
                }

                deleteDialog.Dispose();
            }
        }

        /// <summary>
        /// Событие, вызываемое при нажатии на кнопку "QuitButton". Закрывает данное окно и открывает окно Авторизации.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void QuitButton_Click (object sender, RoutedEventArgs e)
        {
            Autorization newWindow = new Autorization();
            newWindow.Show();

            Close();
        }
    }
}
