using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bool = System.Boolean;

namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : Button
    {
        public Cell ButtonCell;
        public Position ButtonPosition;

        public UserControl1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод для проверки списка на содержание определенного элемента.
        /// </summary>
        /// <param name="listToCheck">Список, который надо проверить.</param>
        /// <param name="obj">Элемент, присутствие которого необходимо найти.</param>
        /// <returns>Логическая переменная, содержащая значение вхождения данного элемента в список.</returns>
        public static Bool Contains (List<UserControl1> listToCheck, UserControl1 obj)
        {
            for (int i = 0; i < listToCheck.Count; i++)
            {
                if (listToCheck[i].ButtonCell.Occupied == obj.ButtonCell.Occupied &&
                listToCheck[i].ButtonPosition.Column == obj.ButtonPosition.Column &&
                listToCheck[i].ButtonPosition.Line == obj.ButtonPosition.Line && 
                listToCheck[i].ButtonCell.DarkColour == obj.ButtonCell.DarkColour)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
