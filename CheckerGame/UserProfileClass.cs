using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace CheckerGame
{
    /// <summary>
    /// Класс для профилей пользователей.
    /// </summary>
    class UserProfile
    {
        String name;
        String password;
        UserGender gender;
        DateTime birthTime;
        static List<UserProfile> actualProfiles = new List<UserProfile>(1);

        public UserProfile()
        {
            //Я заболел. Надо закончить это... когда-нибудь.
            System.Windows.MessageBox.Show("Here?..");
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
