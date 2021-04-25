using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Bool = System.Boolean;

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
        public UserProfile (String name, String password, UserGender gender, DateTime birthTime, Int32 wins, Int32 allGames)
        {
            this.name = name;
            this.password = password;
            this.gender = gender;
            this.birthTime = birthTime;
            this.wins = wins;
            this.allGames = allGames;
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

                    ableToRefreshFile = true;
                }
            }
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
        public void AddWin (UserProfile winner)
        {
            Int32 winnerIndex = ActualProfiles.IndexOf(winner);

            ActualProfiles[winnerIndex].Wins += 1;
            ActualProfiles[winnerIndex].AllGames += 1;

            RefreshFile();
            RefreshAccounts();
        }

        /// <summary>
        /// Метод для увеличения счетчика игр у какого-либо пользователя.
        /// </summary>
        /// <param name="player">Пользователь, которому надо дополнить счетчик игр.</param>
        public void AddGame (UserProfile player)
        {
            Int32 playerIndex = ActualProfiles.IndexOf(player);

            ActualProfiles[playerIndex].AllGames += 1;

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
