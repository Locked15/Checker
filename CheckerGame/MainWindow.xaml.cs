using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Bool = System.Boolean;

/// <summary>
/// Основное Пространство Имен проекта.
/// </summary>
namespace CheckerGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Cell chosedCell;
        Int32 clickCounter;
        Cell[,] battleField = new Cell[8, 8];
        List<GameFigure> firstSide = new List<GameFigure>(1);
        List<GameFigure> secondSide = new List<GameFigure>(1);

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < Math.Pow(battleField.GetLength(1), 2); i++)
            {
                UserControl1 button = (UserControl1)PointsPanel.Children[i];
                String name = button.Name;

                Int32 row = Int32.Parse(name[1].ToString());
                Int32 column = Int32.Parse(name[2].ToString());

                button.ButtonPosition = new Position(row, column);
                button.ButtonCell = new Cell();

                Bool side = row <= 3;

                if (row % 2 == 0)
                {
                    if (column % 2 == 0)
                    {
                        button.ButtonCell.CurrentFigure = new GameFigure(button.ButtonPosition, FigureType.Common, side);
                        button.ButtonCell.Occupied = true;
                        button.ButtonCell.DarkColour = true;
                    }

                    else
                    {
                        button.ButtonCell.CurrentFigure = null;
                        button.ButtonCell.DarkColour = false;
                    }
                }

                else
                {
                    if (column % 2 != 0)
                    {
                        button.ButtonCell.CurrentFigure = new GameFigure(button.ButtonPosition, FigureType.Common, side);
                        button.ButtonCell.Occupied = true;
                        button.ButtonCell.DarkColour = true;
                    }


                    else
                    {
                        button.ButtonCell.CurrentFigure = null;
                        button.ButtonCell.DarkColour = false;
                    }
                }

                if (row == battleField.GetLength(1))
                {
                    button.ButtonCell.MainEdge = true;
                    button.ButtonCell.SecondEdge = false;
                }

                else if (row == 1)
                {
                    button.ButtonCell.SecondEdge = true;
                    button.ButtonCell.MainEdge = false;
                }
            }
        }

        private void UserControl1_Click(object sender, RoutedEventArgs e)
        {
            UserControl1 button = (UserControl1)sender;

            if (clickCounter == 0)
            {
                chosedCell = button.ButtonCell;
            }

            else
            {
                clickCounter = 0;
            }
        }
    }

    /// <summary>
    /// Класс для хранения информации об отдельной ячейке.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Логическое поле, отвечающее за то, является ли выбранная клетка 
        /// главной границей ("верхняя" граница поля).
        /// </summary>
        Bool mainEdge;

        /// <summary>
        /// Логическое поле, отвечающее за то, является ли выбранная клетка
        /// вторичной границей ("нижняя" граница поля).
        /// </summary>
        Bool secondEdge;

        /// <summary>
        /// Логическое поле, отвечающее за то, является ли выбранная клетка
        /// занятой (присутствует ли на ней фигура).
        /// </summary>
        Bool occupied;

        /// <summary>
        /// Логическое поле, отвечающее за то, является ли выбранная клетка
        /// "темной", то есть пригодной для прохождения.
        /// </summary>
        Bool darkColour;

        /// <summary>
        /// Поле, содержащее экземпляр класса "GameFigure", который характеризует
        /// фигуру, которая стоит в данной клетке. Если фигуры нет, принимает значение
        /// "null".
        /// </summary>
        GameFigure currentFigure;

        /// <summary>
        /// Свойство, отвечающее за то, является ли выбранная клетка 
        /// главной границей ("верхняя" граница поля).
        /// </summary>
        public Bool MainEdge
        {
            get
            {
                return MainEdge;
            }

            set
            {
                mainEdge = value;
            }
        }

        ///<summary>
        /// Свойство, отвечающее за то, является ли выбранная клетка
        /// вторичной границей ("нижняя" граница поля).
        ///</summary>>
        public Bool SecondEdge
        {
            get
            {
                return secondEdge;
            }

            set
            {
                secondEdge = value;
            }
        }

        /// <summary>
        /// Свойство, отвечающее за то, является ли выбранная клетка
        /// занятой (присутствует ли на ней фигура).
        /// </summary>
        public Bool Occupied
        {
            get
            {
                return occupied;
            }

            set
            {
                occupied = value;
            }
        }

        /// <summary>
        /// Свойство, отвечающее за то, является ли выбранная клетка
        /// "темной", то есть пригодной для прохождения.
        /// </summary>
        public Bool DarkColour
        {
            get
            {
                return darkColour;
            }

            set
            {
                darkColour = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее экземпляр класса "GameFigure", который характеризует
        /// фигуру, которая стоит в данной клетке. Если фигуры нет, принимает значение
        /// "null".
        /// </summary>
        public GameFigure CurrentFigure
        {
            get
            {
                return currentFigure;
            }

            set
            {
                currentFigure = value;
            }
        }

        /// <summary>
        /// Явно прописанный конструктор Класса "Cell". Нужен для создания 
        /// "пустых" экземпляров и их дальнейшего наполнения через Инициализатор.
        /// </summary>
        public Cell()
        {

        }

        /// <summary>
        /// Полноценный конструктор класса.
        /// </summary>
        /// <param name="occupied">Логическая переменная, отвечающая за занятость клетки.</param>
        /// <param name="edge">Логическая переменная, отвечающая за нахождение клетки у
        /// "верхней" границы игровой области.</param>
        /// <param name="currentFigure">Экземпляр класса "CurrentFigure". Отвечает за фигуру,
        ///  которая сейчас расположена в ячейке.</param>
        /// <param name="secondEdge">Логическая переменная, отвечающая за нахождение клетки
        /// у "нижней" границы игровой области.</param>
        /// <param name="darkColour">Логическая переменная, отвечающая за тип ячейки: 
        /// Темная или Светлая.</param>
        public Cell(Bool occupied, Bool edge, GameFigure currentFigure, Bool secondEdge, Bool darkColour)
        {
            Occupied = occupied;
            MainEdge = edge;
            CurrentFigure = currentFigure;
            SecondEdge = secondEdge;
            DarkColour = darkColour;
        }
    }

    /// <summary>
    /// Класс для хранения информации о местонахождении ячейки.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Поле, содержащее Строку, в которой находится ячейка.
        /// </summary>
        Int32 line;

        /// <summary>
        /// Поле, содержащее Колонну, в которой находится ячейка.
        /// </summary>
        Int32 column;

        /// <summary>
        /// Свойство, содержащее Строку, в которой находится ячейка.
        /// </summary>
        public Int32 Line
        {
            get
            {
                return line;
            }

            set
            {
                line = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее Колонну, в которой находится ячейка.
        /// </summary>
        public Int32 Column
        {
            get
            {
                return column;
            }

            set
            {
                column = value;
            }
        }

        /// <summary>
        /// Конструктор класса "Position".
        /// </summary>
        /// <param name="line">Строка, в которой находится ячейка.</param>
        /// <param name="column">Колонна, в которой находится ячейка.</param>
        public Position (Int32 line, Int32 column)
        {
            Line = line;
            Column = column;
        }
    }

    /// <summary>
    /// Класс для определения самих Шашек — "Игровых Фигур".
    /// </summary>
    public class GameFigure
    {
        /// <summary>
        /// Поле, содержащее экземпляр класса "Location", с местонахождением данной фигуры.
        /// </summary>
        Position location;

        /// <summary>
        /// Поле, содержащее экземпляр перечисления, определяющий Тип Фигуры (Пешка/Дамка).
        /// </summary>
        FigureType type;

        /// <summary>
        /// Поле, содержащее логическое значение, отвечающее за принадлежность фигуры к Главной ("Нижней")
        /// стороне.
        /// </summary>
        Bool mainSide;

        /// <summary>
        /// Свойство, содержащее экземпляр класса "Location", с местонахождением данной фигуры.
        /// </summary>
        public Position Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее экземпляр перечисления, определяющий Тип Фигуры (Пешка/Дамка).
        /// </summary>
        public FigureType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее логическое значение, отвечающее за принадлежность фигуры к Главной ("Нижней")
        /// стороне.
        /// </summary>
        public Bool MainSide
        {
            get
            {
                return mainSide;
            }

            set
            {
                mainSide = value;
            }
        }

        /// <summary>
        /// Конструктор класса "GameFigure".
        /// </summary>
        /// <param name="location">Переменная класса "Position", содержащая местонахождение фигуры.</param>
        /// <param name="type">Переменная перечисления Enum, содержащая Тип Фигуры.</param>
        /// <param name="side">Логическая переменная, отвечающая за принадлежность к "Главной"
        /// (Нижней) стороне.</param>
        public GameFigure (Position location, FigureType type, Bool side)
        {
            Location = location;
            Type = type;
            MainSide = side;
        }
    }

    /// <summary>
    /// Перечисление типа "Enum", содержащее два значения для определения фигуры: 
    /// Дамка и Пешка.
    /// </summary>
    public enum FigureType
    {
        /// <summary>
        /// Значение для Пешек.
        /// </summary>
        Common,

        /// <summary>
        /// Значение для Дамок.
        /// </summary>
        Special
    }
}
