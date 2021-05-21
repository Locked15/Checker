using System;
using System.IO;
using System.Windows;
using System.Text.Json;
using Bool = System.Boolean;
using System.Threading.Tasks;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace CheckerGame
{
    /// <summary>
    /// Класс для профилей пользователей.
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Поле, содержащее количество побед конкретного игрока.
        /// </summary>
        Int32 wins;

        /// <summary>
        /// Поле, содержащее количество побегов с матчей.
        /// </summary>
        Int32 leaves;

        /// <summary>
        /// Поле, содержащее имя конкретного игрока.
        /// </summary>
        String name;

        /// <summary>
        /// Поле, содержащее количество всех игр данного игрока.
        /// </summary>
        Int32 allGames;

        /// <summary>
        /// Поле, содержащее пароль данного игрока.
        /// </summary>
        String password;

        /// <summary>
        /// Поле, содержащее пол данного игрока. Для определения используется перечисление "Enum".
        /// </summary>
        UserGender gender;

        /// <summary>
        /// Поле, содержащее дату рождения данного игрока.
        /// </summary>
        DateTime birthTime;

        /// <summary>
        /// Статическое поле, отвечающее за разрешение на перезапись файла. Нужно для предохранения от записи в файл пустого списка пользователей. 
        /// </summary>
        static Bool ableToRefreshFile = false;

        /// <summary>
        /// Статическое поле, содержащее всех активных игроков на данный момент.
        /// </summary>
        static List<UserProfile> actualProfiles = new List<UserProfile>(1);

        /// <summary>
        /// Статическое поле, содержащее абсолютный путь к проекту.
        /// </summary>
        static String projectAbsPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("CheckerGame")
        + "CheckerGame".Length);

        /// <summary>
        /// Свойство, содержащее количество побед данного игрока.
        /// </summary>
        public Int32 Wins
        {
            get
            {
                return wins;
            }

            private set
            {
                wins = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее количество побегов игрока с матчей.
        /// </summary>
        public Int32 Leaves
        {
            get
            {
                return leaves;
            }

            private set
            {
                leaves = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее имя конкретного игрока.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее количество всех игр данного игрока.
        /// </summary>
        public Int32 AllGames
        {
            get
            {
                return allGames;
            }

            private set
            {
                allGames = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее пароль данного игрока.
        /// </summary>
        public String Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее пол данного игрока. Для определения используется перечисление "Enum".
        /// </summary>
        public UserGender Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее дату рождения данного игрока.
        /// </summary>
        public DateTime BirthTime
        {
            get
            {
                return birthTime;
            }

            set
            {
                birthTime = value;
            }
        }

        /// <summary>
        /// Статическое свойство, содержащее абсолютный путь к проекту.
        /// </summary>
        public static String ProjectAbsPath
        {
            get
            {
                return projectAbsPath;
            }
        }

        /// <summary>
        /// Статическое свойство, содержащее всех активных игроков на данный момент.
        /// </summary>
        public static List<UserProfile> ActualProfiles
        {
            get
            {
                return actualProfiles;
            }

            private set
            {
                actualProfiles = value;
            }
        }

        /// <summary>
        /// Статическое поле, содержащее параметры для сериализации.
        /// </summary>
        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        /// <summary>
        /// Конструктор класса "UserProfile".
        /// </summary>
        /// <param name="name">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="gender">Пол пользователя.</param>
        /// <param name="birthTime">Дата рождения пользователя.</param>
        /// <param name="wins">Количество побед данного пользователя. Данный аргумент конструктора нужен для корректной работы Сериализатора.</param>
        /// <param name="allGames">Количество игр у данного пользователя. Данный аргумент конструктора нужен для корректной работы Сериализатора.</param>
        /// <param name="leaves">Количество побегов у данного пользователя. Данный аргумент конструктора нужен для корректной работы Сериализатора.</param>>
        public UserProfile (String name, String password, UserGender gender, DateTime birthTime, Int32 wins, Int32 allGames, Int32 leaves)
        {
            this.name = name;
            this.password = password;
            this.gender = gender;
            this.birthTime = birthTime;
            this.wins = wins;
            this.allGames = allGames;
            this.leaves = leaves;
        }

        /// <summary>
        /// Статический метод для обновления списка аккаунтов.
        /// </summary>
        public static void RefreshAccounts ()
        {
            if (File.Exists(ProjectAbsPath + "\\Other\\Profiles.txt"))
            {
                using (StreamReader sr1 = new StreamReader(ProjectAbsPath + "\\Other\\Profiles.txt", System.Text.Encoding.Default))
                {
                    String allText = sr1.ReadToEnd();

                    ActualProfiles = JsonSerializer.Deserialize<List<UserProfile>>(allText, options);
                }
            }

            ableToRefreshFile = true;
        }

        /// <summary>
        /// Статический метод для обновления файла со всеми аккаунтами.
        /// </summary>
        public static void RefreshFile ()
        {
            if (ableToRefreshFile)
            {
                using (StreamWriter sw1 = new StreamWriter(ProjectAbsPath + "\\Other\\Profiles.txt", false, System.Text.Encoding.Default))
                {
                    sw1.Write(JsonSerializer.Serialize<List<UserProfile>>(ActualProfiles, options));
                }
            }
        }

        /// <summary>
        /// Статический метод для добавления нового аккаунта.
        /// </summary>
        /// <param name="account">Аккаунт пользователя, который надо добавить.</param>
        public static void AddAccount (UserProfile account)
        {
            ActualProfiles.Add(account);

            RefreshFile();
        }

        /// <summary>
        /// Метод для получения пользователя по имени его аккаунта.
        /// </summary>
        /// <param name="profileName">Имя аккаунта, по которому необходимо получить пользователя.</param>
        /// <returns>Аккаунт с указанным именем.</returns>
        public static UserProfile GetFromName (String profileName)
        {
            foreach (UserProfile profile in ActualProfiles)
            {
                if (profile.Name == profileName)
                {
                    return profile;
                }
            }

            return null;
        }

        /// <summary>
        /// Метод для проверки на то, есть ли сейчас в системе активные аккаунты.
        /// </summary>
        /// <returns>Логическое Значение, отвечающее за наличие активных аккаунтов.</returns>
        public static Bool CheckActiveAccounts ()
        {
            if (ActualProfiles.Count > 0)
            {
                return true;
            }

            MessageBox.Show("На данный момент активные аккаунты отсутствуют.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        /// <summary>
        /// Метод для проверки Имени и Пароля на существование в списке пользователей.
        /// </summary>
        /// <param name="name">Имя которое необходимо проверить.</param>
        /// <param name="password">Пароль, который необходимо проверить.</param>
        /// <returns>Аккаунт, который соответствует данному паролю и имени.</returns>
        public static UserProfile CheckAccount (String name, String password)
        {
            foreach (UserProfile profile in ActualProfiles)
            {
                if (profile.Name == name && profile.Password == password)
                {
                    return profile;
                }
            }

            return null;
        }

        /// <summary>
        /// Метод для проверки Имени на существование в системе аккаунта с таким же именем.
        /// </summary>
        /// <param name="account">Аккаунт, имя которого необходимо проверить.</param>
        /// <returns>Логическое значение, содержащее существование или нет аккаунта с таким именем.</returns>
        public static Bool CheckAccountName (UserProfile account)
        {
            foreach (UserProfile up in ActualProfiles)
            {
                if (up.Name == account.Name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Метод для добавления победы какому-либо пользователю.
        /// </summary>
        /// <param name="winner">Пользователь, которому надо добавить победу.</param>
        public void AddWin ()
        {
            ActualProfiles.Find(profile => profile == this).Wins += 1;
            ActualProfiles.Find(profile => profile == this).AllGames += 1;

            RefreshFile();
            RefreshAccounts();
        }

        /// <summary>
        /// Метод для увеличения счетчика игр у какого-либо пользователя.
        /// </summary>
        /// <param name="player">Пользователь, которому надо дополнить счетчик игр.</param>
        public void AddGame ()
        {
            ActualProfiles.Find(profile => profile == this).Leaves += 1;

            RefreshFile();
        }

        /// <summary>
        /// Метод для увеличения счетчика побегов с матчей у какого-либо пользователя.
        /// </summary>
        /// <param name="player">Пользователь, которому надо дополнить счетчик побегов.</param>
        public void AddLeave ()
        {
            ActualProfiles.Find(profile => profile == this).Leaves += 1;

            RefreshFile();
        }

        /// <summary>
        /// Метод, который создает документ Word, в котором содержится информация о текущих пользователях.
        /// </summary>
        /// <param name="path">Абсолютный путь к создаваемому файлу.</param>
        /// <param name="wordDocumentName">Название создаваемого файла.</param>
        /// <param name="openFileAfterCreate">Необязательный параметр. Отвечает за открытие файла после его создания.</param>
        public async static void CreateWordUserList (String path, String wordDocumentName, Bool openFileAfterCreate = false)
        {
            String fullPath;

            if (!wordDocumentName.EndsWith(".docx"))
            {
                wordDocumentName += ".docx";
            }

            //Проверка на окончание пути.
            _ = path.EndsWith("\\") ? fullPath = path + wordDocumentName :
            fullPath = path + "\\" + wordDocumentName;

            //Так как создание документа затрачивает много времени, этот алгоритм вынесен в отдельный поток.
            await Task.Run(() =>
            {
                //Создаем процесс Word.
                Word.Application appToDo = new Word.Application();

                //Создаем документ. Связываем его с созданным ранее процессом.
                appToDo.Documents.Add();
                Word.Document mainDocument = appToDo.Documents[1];

                //Производим запись в файл.
                for (int i = 0; i < ActualProfiles.Count; i++)
                {
                    /*
                    * <——————————————————————————————————————————————————————————————————————————————————————————————————>
                    * |                                        !Запись в Файл!                                           |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * | В данном регионе кода происходит наполнение Word-файла. Разберем этот алгоритм по пунктам:       |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * |                                    I. Создание Колонтитулов.                                     |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * | 1. Создаем экземпляр интерфейса "HeaderFooter". Связываем его с текущей секцией документа.       |
                    * | 2. При инициализации указываем, что это Верхний Колонтитул.                                      |
                    * | 3. Настраиваем его свойства: шрифт, стиль, выравнивание. Задаем текст с именем пользователя.     |
                    * | 4. Аналогично создаем еще один экземпляр интерфейса, но теперь прописываем номер страницы.       |
                    * | 5. У второго колонтитула также изменяем стиль чисел, отвечающих за нумерацию страниц.            |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * |                                     II. Создание Параграфов.                                     |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * | 1. Создаем экземпляр класса Word.Paragraph, связываем его с документом и добавляем сам параграф. |
                    * | 2. Далее используя "Range" мы задаем текст и его свойства.                                       |
                    * | 3. Далее используем функцию ".InsertParagraphAfter()", чтобы завершить параграф.                 |
                    * | 4. Если следующий параграф не последний, то используем метод "InsertParagraphBefore()", чтобы -> |
                    * | -> красиво расположить текст, создав пробел между строками.                                      |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * |                                      III. Завершение.                                            |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * | 1. Выполняем проверку на длину файла, чтобы не создавать лишние страницы.                        |
                    * | 2. Если проверка пройдена, используем метод "Sections.Add()", чтобы создать новую страницу.      |
                    * <——————————————————————————————————————————————————————————————————————————————————————————————————>
                    */

                    #region Область Кода: Заполнение Файла.

                    //Добавляем и настраиваем Верхний Колонтитул с именем текущего пользователя.
                    Word.HeaderFooter currentHeader = appToDo.ActiveDocument.Sections[i + 1].Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                    currentHeader.Range.Font.Bold = 0;
                    currentHeader.LinkToPrevious = false;
                    currentHeader.Range.Font.Name = "Georgia";
                    currentHeader.Range.Text = ActualProfiles[i].Name;
                    currentHeader.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    //Добавляем и настраиваем Нижний Колонтитул с номером текущей страницы.
                    Word.HeaderFooter currentFooter = appToDo.ActiveDocument.Sections[i + 1].Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                    currentFooter.Range.Font.Bold = 0;
                    currentFooter.LinkToPrevious = false;
                    currentFooter.Range.Font.Name = "Georgia";
                    currentFooter.Range.Text = (i + 1).ToString();
                    currentFooter.PageNumbers.NumberStyle = Word.WdPageNumberStyle.wdPageNumberStyleHanjaRead;
                    currentFooter.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    //Запись свойства: "Имя Пользователя.".
                    Word.Paragraph userName = mainDocument.Content.Paragraphs.Add();
                    userName.Range.InsertParagraphBefore();
                    userName.Range.Text = "Имя Пользователя: " + ActualProfiles[i].Name + ';';
                    userName.Range.Font.Name = "Georgia";
                    userName.Range.Font.Bold = 0;
                    userName.Range.InsertParagraphAfter();

                    //Запись свойства: "Количество побед.".
                    Word.Paragraph userWins = mainDocument.Content.Paragraphs.Add();
                    userWins.Range.InsertParagraphBefore();
                    userWins.Range.Text = "Количество побед: " + ActualProfiles[i].Wins + ';';
                    userWins.Range.Font.Name = "Georgia";
                    userName.Range.Font.Bold = 0;
                    userWins.Range.InsertParagraphAfter();

                    //Запись свойства: "Количество игр.".
                    Word.Paragraph userGames = mainDocument.Content.Paragraphs.Add();
                    userGames.Range.InsertParagraphBefore();
                    userGames.Range.Text = "Количество игр: " + ActualProfiles[i].AllGames + ';';
                    userGames.Range.Font.Name = "Georgia";
                    userName.Range.Font.Bold = 0;
                    userGames.Range.InsertParagraphAfter();

                    //Заголовок — "Количество побегов.".
                    Word.Paragraph userLeaves = mainDocument.Content.Paragraphs.Add();
                    userLeaves.Range.InsertParagraphBefore();
                    userLeaves.Range.Text = "Количество побегов: " + ActualProfiles[i].Leaves + ';';
                    userLeaves.Range.Font.Name = "Georgia";
                    userName.Range.Font.Bold = 0;
                    userLeaves.Range.InsertParagraphAfter();

                    //Заголовок — "Пол пользователя.".
                    Word.Paragraph userGender = mainDocument.Content.Paragraphs.Add();
                    userGender.Range.InsertParagraphBefore();
                    userGender.Range.Text = "Пол пользователя: " + ActualProfiles[i].GetStringGender() + ';';
                    userGender.Range.Font.Name = "Georgia";
                    userName.Range.Font.Bold = 0;
                    userGender.Range.InsertParagraphAfter();

                    //Заголовок — "Дата рождения.".
                    Word.Paragraph userBirthTime = mainDocument.Content.Paragraphs.Add();
                    userBirthTime.Range.InsertParagraphBefore();
                    userBirthTime.Range.Text = "Дата рождения: " + ActualProfiles[i].BirthTime.ToString("dd.MM.yyyy!");
                    userBirthTime.Range.Font.Name = "Georgia";
                    userName.Range.Font.Bold = 0;

                    //Проверка на длину файла. Создана для того, чтобы не создавать лишние страницы.
                    if ((i + 1) < ActualProfiles.Count)
                    {
                        appToDo.ActiveDocument.Sections.Add();
                    }

                    #endregion
                }

                //Сохраняем созданный файл по указанному адресу.
                mainDocument.SaveAs(fullPath);

                //Описание завершения работы:
                if (!openFileAfterCreate)
                {
                    /*
                    * <——————————————————————————————————————————————————————————————————————————————————————————————————>
                    * |                                     !ОЧЕНЬ ВАЖНЫЙ МОМЕНТ!                                        |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * | Здесь происходит завершение процесса "WORD.exe":                                                 |
                    * | 1. Сначала мы закрываем сам "Документ" (файл);                                                   |
                    * | 2. Затем, через метод Quit() мы завершаем работу процесса.                                       |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * | Если работу процесса не завершить, он останется работать на фоне, даже после закрытия программы. |
                    * <——————————————————————————————————————————————————————————————————————————————————————————————————>
                    */

                    mainDocument.Close();
                    appToDo.Quit();
                }

                //Однако если пользователь решил развернуть программу, то завершать процесс принудительно не нужно.
                else
                {
                    appToDo.Visible = true;
                }
            });
        }

        /// <summary>
        /// Метод, который создает таблицу Excel, в которой содержится информация о текущих пользователях.
        /// </summary>
        /// <param name="path">Абсолютный путь к создаваемому файлу.</param>
        /// <param name="excelSheetName">Название создаваемого файла.</param>
        /// <param name="openFileAfterCreate">Необязательный параметр. Отвечает за открытие файла после его создания.</param>
        public async static void CreateExcelUserList (String path, String excelSheetName, Bool openFileAfterCreate = false)
        {
            String fullPath;

            if (!excelSheetName.EndsWith(".xlsx"))
            {
                excelSheetName += ".xlsx";
            }

            //Проверка на окончание пути.
            _ = path.EndsWith("\\") ? fullPath = path + excelSheetName :
            fullPath = path + "\\" + excelSheetName;

            //Так как создание документа затрачивает много времени, этот алгоритм вынесен в отдельный поток.
            await Task.Run(() =>
            {
                //Создается "Процесс" Excel. Далее указывается количество страниц в будущем файле.
                Excel.Application appToWork = new Excel.Application();
                appToWork.SheetsInNewWorkbook = ActualProfiles.Count;

                //Создается "Книга" (файл), в него сразу добавляется страница. 
                //Далее эта страница записывается в отдельную переменную.
                //При работе с Excel индексация страниц начинается с 1.
                Excel.Workbook mainWorkbook = appToWork.Workbooks.Add(1);
                Excel.Worksheet currentWorksheet = mainWorkbook.Worksheets.Item[1];

                for (int i = 0; i < ActualProfiles.Count; i++)
                {
                    //Так как страницы добавляются Справа -> Налево, то используем нестандартный индекс.
                    currentWorksheet.Name = ActualProfiles[ActualProfiles.Count - (i + 1)].Name;

                    //Заполняем ячейки будущей книги по их индексам.
                    currentWorksheet.Cells[1, 1] = "Имя Пользователя:";
                    currentWorksheet.Cells[2, 1] = "Количество Побед:";
                    currentWorksheet.Cells[3, 1] = "Количество Игр:";
                    currentWorksheet.Cells[4, 1] = "Количество Побегов:";
                    currentWorksheet.Cells[5, 1] = "Пол:";
                    currentWorksheet.Cells[6, 1] = "Дата Рождения:";

                    //Добавляем в следующий столбец значения этих Свойств.
                    currentWorksheet.Cells[1, 2] = ActualProfiles[ActualProfiles.Count - (i + 1)].Name + '.';
                    currentWorksheet.Cells[2, 2] = ActualProfiles[ActualProfiles.Count - (i + 1)].Wins + '.';
                    currentWorksheet.Cells[3, 2] = ActualProfiles[ActualProfiles.Count - (i + 1)].AllGames + '.';
                    currentWorksheet.Cells[4, 2] = ActualProfiles[ActualProfiles.Count - (i + 1)].Leaves + '.';
                    currentWorksheet.Cells[5, 2] = ActualProfiles[ActualProfiles.Count - (i + 1)].GetStringGender() + '.';

                    //Последней ячейкой является Дата Рождения. Сюда также записывается значение, но перед этим оно форматируется.
                    currentWorksheet.Cells[6, 2] = ActualProfiles[ActualProfiles.Count - (i + 1)].BirthTime.ToString("dd.MM.yyyy!");

                    //Настраиваем выравнивание текста в ячейках:
                    currentWorksheet.Range["B1", "B6"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                    //Выполняем выравнивание столбцов по размеру.
                    Excel.Range columnsToFit = currentWorksheet.UsedRange;
                    columnsToFit.Columns.AutoFit();

                    //Задаем стиль для границ между ячейками:
                    currentWorksheet.Range["A1", "B6"].Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                    currentWorksheet.Range["A1", "B6"].Borders.Color = Excel.XlRgbColor.rgbBlack;

                    //Здесь выполняется проверка на существование следующей итерации.
                    //Это нужно для избегания создания лишних страниц в файле.
                    if (i + 1 < ActualProfiles.Count)
                    {
                        currentWorksheet = mainWorkbook.Worksheets.Add();
                    }
                }

                //Сохраняем созданный файл по указанному адресу.
                mainWorkbook.SaveAs(fullPath);

                //Описание завершения работы:
                if (!openFileAfterCreate)
                {
                    /*
                    * <——————————————————————————————————————————————————————————————————————————————————————————————————>
                    * |                                     !ОЧЕНЬ ВАЖНЫЙ МОМЕНТ!                                        |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * | Здесь происходит завершение процесса "EXCEL.exe":                                                |
                    * | 1. Сначала мы закрываем саму "Рабочую Книгу" (файл);                                             |
                    * | 2. Затем мы закрываем ВСЕ Рабочие Книги (файлы), с которыми работал созданный процесс;           |
                    * | 3. И после этого, через метод Quit() мы завершаем работу процесса.                               |
                    * |——————————————————————————————————————————————————————————————————————————————————————————————————|
                    * | Если работу процесса не завершить, он останется работать на фоне, даже после закрытия программы. |
                    * <——————————————————————————————————————————————————————————————————————————————————————————————————>
                    */

                    mainWorkbook.Close(false);
                    appToWork.Workbooks.Close();
                    appToWork.Quit();
                }

                //Однако если пользователь решит сразу открыть окно с Excel-файлом, то принудительно завершать процесс не придется.
                else
                {
                    //Достаточно сделать его видимым, изменив свойство "Visible".
                    appToWork.Visible = true;
                }
            });
        }

        /// <summary>
        /// Метод для получения строкового представления пола пользователя.
        /// </summary>
        /// <returns>Строковое представление пола.</returns>
        public String GetStringGender ()
        {
            switch (this.Gender)
            {
                case UserGender.Male:
                    return "Мужской";

                case UserGender.Female:
                    return "Женский";

                default:
                    return "Альтернативный";
            }
        }

        /// <summary>
        /// Метод для удаления аккаунта пользователя.
        /// </summary>
        public void DeleteUser ()
        {
            ActualProfiles.Remove(this);

            RefreshFile();
        }
    }

    /// <summary>
    /// Перечисление типа "Enum" для определения пола пользователя.
    /// </summary>
    public enum UserGender
    {
        /// <summary>
        /// Значение для мужского пола.
        /// </summary>
        Male,

        /// <summary>
        /// Значение для женского пола.
        /// </summary>
        Female,

        /// <summary>
        /// Значение для альтернативного пола.
        /// </summary>
        Alternative
    }
}
