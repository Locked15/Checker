using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using Bool = System.Boolean;
using Colour = System.Windows.Media.Color;
using SolidColourBrush = System.Windows.Media.SolidColorBrush;

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
        /// <summary>
        /// Поле, содержащее экземпляр класса "Hub", необходимого для возвращения на предыдущую страницу.
        /// </summary>
        Hub hub;

        /// <summary>
        /// Логическое Поле, отвечающее за то, в "боевом" ли режиме сейчас Дамка или нет. 
        /// (Подробнее в UserControl1_Click -> 2 Блок if...else if...else).
        /// </summary>
        Bool battleMode;

        /// <summary>
        /// Логическое Поле, отвечающее за то, ходят ли сейчас Белая Сторона 
        /// ("Нижние").
        /// </summary>
        Bool whiteTurn;

        /// <summary>
        /// Поле с целочисленным значением, отвечающее за произведенное количество кликов.
        /// </summary>
        Int32 clickCounter;

        /// <summary>
        /// Поле, содержащее все ячейки игрового поля.
        /// </summary>
        Cell[,] battleField;

        /// <summary>
        /// Поле, содержащее выбранный при первом клике элемент 
        /// ("Шашка").
        /// </summary>
        UserControl1 chosedCell;

        /// <summary>
        /// Поле, содержащее все фигуры Главной Стороны
        /// ("Белые", "Нижние").
        /// </summary>
        static List<GameFigure> firstSide = new List<GameFigure>(1);

        /// <summary>
        /// Поле, содержащее все фигуры Вторичной Стороны ("Темные", "Верхние").
        /// </summary>
        static List<GameFigure> secondSide = new List<GameFigure>(1);

        /// <summary>
        /// Поле, содержащее все "Средние" линие, находящиеся между 
        /// "Линиями Фронта".
        /// </summary>
        List<Int32> battleFieldMiddleCellsRows = new List<Int32>(1);

        /// <summary>
        /// Список, содержащий кнопки ("Шашки"), которые должен уничтожить игрок в своем ходу.
        /// </summary>
        List<UserControl1> enemyTerminateList = new List<UserControl1>(1);

        /// <summary>
        /// Список, содержащий поля, куда может прыгнуть игрок, уничтожив фигуру.
        /// </summary>
        List<UserControl1> jumpTroughEnemyPlaces = new List<UserControl1>(1);

        /// <summary>
        /// Список, содержащий поля, куда в принципе может переместиться Дамка.
        /// </summary>
        List<Position> positionsToEnableIfCellIsKing = new List<Position>(1);

        /// <summary>
        /// Список, содержащий клетки, которые может уничтожить Дамка. Содержит значения для направления: Влево-Вниз. 
        /// Последний элемент, если список не пуст, содержит ячейку, куда Дамка может (и должна) переместиться.
        /// </summary>
        List<UserControl1> figuresToDestroyForTheKingDL = new List<UserControl1>(1);

        /// <summary>
        /// Список, содержащий клетки, которые может уничтожить Дамка. Содержит значения для направления: Вправо-Вниз. 
        /// Последний элемент, если список не пуст, содержит ячейку, куда Дамка может (и должна) переместиться.
        /// </summary>
        List<UserControl1> figuresToDestroyForTheKingDR = new List<UserControl1>(1);

        /// <summary>
        /// Список, содержащий клетки, которые может уничтожить Дамка. Содержит значения для направления: Вверх-Влево. 
        /// Последний элемент, если список не пуст, содержит ячейку, куда Дамка может (и должна) переместиться.
        /// </summary>
        List<UserControl1> figuresToDestroyForTheKingUL = new List<UserControl1>(1);

        /// <summary>
        /// Список, содержащий клетки, которые может уничтожить Дамка. Содержит значения для направления: Вправо-Вверх. 
        /// Последний элемент, если список не пуст, содержит ячейку, куда Дамка может (и должна) переместиться.
        /// </summary>
        List<UserControl1> figuresToDestroyForTheKingUR = new List<UserControl1>(1);

        /// <summary>
        /// Свойство, содержащее все фигуры Главной Стороны
        /// ("Белые", "Нижние").
        /// </summary>
        public static List<GameFigure> FirstSide
        {
            get
            {
                return firstSide;
            }

            set
            {
                firstSide = value;
            }

        }

        /// <summary>
        /// Свойство, содержащее все фигуры Вторичной Стороны
        /// ("Темные", "Верхние").
        /// </summary>
        public static List<GameFigure> SecondSide
        {
            get
            {
                return secondSide;
            }

            set
            {
                secondSide = value;
            }
        }

        /// <summary>
        /// Точка входа в приложение с интерфейсом.
        /// </summary>
        /// <param name="hub">Экземпляр класса "Hub".</param>
        /// <param name="size">Размер будущего игрового поля.</param>
        public MainWindow (Hub hub, Int32 size)
        {
            this.hub = hub;

            battleField = new Cell[size, size];

            Int32 j = 0;
            Int32 line = 0;
            String latestButton = "10";
            Int32 limit = battleField.GetLength(0) / 2;

            //Проверка на четность размерности игрового поля.
            if (size % 2 == 0)
            {
                //Если игровое поле имеет четную размерность, то между фигурами...
                //... будет отступ в 2 ряда. 

                battleFieldMiddleCellsRows.Add(limit);
                battleFieldMiddleCellsRows.Add(limit + 1);
            }

            else
            {
                //В ином случае отступ будет уже 3 ряда.

                battleFieldMiddleCellsRows.Add(limit);
                battleFieldMiddleCellsRows.Add(limit + 1);
                battleFieldMiddleCellsRows.Add(limit + 2);
            }

            InitializeComponent();

            PointsPanel.Rows = size;
            PointsPanel.Columns = size;

            whiteTurn = true;

            for (int i = battleField.GetLength(0); i > 0; i--)
            {
                for (int z = 1; z <= battleField.GetLength(1); z++)
                {
                    UserControl1 newButton = new UserControl1();

                    if (i % 2 != 0)
                    {
                        if (z % 2 == 0)
                        {
                            newButton.Background = new SolidColourBrush(Colour.FromRgb(255, 255, 255));
                        }

                        else
                        {
                            newButton.Background = new SolidColourBrush(Colour.FromRgb(90, 44, 0));
                        }
                    }

                    else
                    {
                        if (z % 2 != 0)
                        {
                            newButton.Background = new SolidColourBrush(Colour.FromRgb(255, 255, 255));
                        }

                        else
                        {
                            newButton.Background = new SolidColourBrush(Colour.FromRgb(90, 44, 0));
                        }
                    }

                    newButton.Name = $"_{i}{z}";
                    newButton.FontSize = 60;
                    newButton.ButtonPosition = new Position(i, z);

                    //Так как мы создаем элементы прямо через код, то нам необходимо зарегистрировать этот элемент в XAML.
                    //Только в таком случае все методы будут работать корректно.
                    NameScope.SetNameScope(newButton, NameScope.GetNameScope(PointsPanel));
                    RegisterName(newButton.Name, newButton);

                    PointsPanel.Children.Add(newButton);
                }
            }

            for (int i = 0; i < Math.Pow(size, 2); i++)
            {
                UserControl1 button = (UserControl1)PointsPanel.Children[i];
                String name = button.Name;

                Int32 row = button.ButtonPosition.Line;
                Int32 column = button.ButtonPosition.Column;

                button.ButtonCell = new Cell();

                //Проверка на сторону, к которой принадлежит фигура, находящаяся на данной клетке:
                //Если она находится на линии меньшей или равной (limit - 1), то эта фигура принадлежит основной стороне.
                Bool side = row <= limit - 1;

                if (row % 2 == 0 && !battleFieldMiddleCellsRows.Contains(row))
                {
                    if (column % 2 == 0)
                    {
                        button.ButtonCell.CurrentFigure = new GameFigure(button.ButtonPosition, FigureType.Common, side);
                        button.ButtonCell.Occupied = true;
                        button.ButtonCell.DarkColour = true;

                        if (side)
                        {
                            firstSide.Add(button.ButtonCell.CurrentFigure);

                            button.Foreground = new SolidColourBrush(Colour.FromRgb(255, 255, 255));
                        }

                        else
                        {
                            secondSide.Add(button.ButtonCell.CurrentFigure);

                            button.Foreground = new SolidColourBrush(Colour.FromRgb(0, 0, 0));
                        }

                        button.Click += UserControl1_Click;

                        button.Content = GameFigure.CommonFigure;
                    }

                    else
                    {
                        button.ButtonCell.CurrentFigure = null;
                        button.ButtonCell.DarkColour = false;
                    }
                }

                else if (!battleFieldMiddleCellsRows.Contains(row))
                {
                    if (column % 2 != 0)
                    {
                        button.ButtonCell.CurrentFigure = new GameFigure(button.ButtonPosition, FigureType.Common, side);
                        button.ButtonCell.Occupied = true;
                        button.ButtonCell.DarkColour = true;

                        if (side)
                        {
                            firstSide.Add(button.ButtonCell.CurrentFigure);

                            button.Foreground = new SolidColourBrush(Colour.FromRgb(255, 255, 255));
                        }

                        else
                        {
                            secondSide.Add(button.ButtonCell.CurrentFigure);

                            button.Foreground = new SolidColourBrush(Colour.FromRgb(0, 0, 0));
                        }

                        button.Click += UserControl1_Click;

                        button.Content = GameFigure.CommonFigure;
                    }

                    else
                    {
                        button.ButtonCell.CurrentFigure = null;
                        button.ButtonCell.DarkColour = false;
                    }
                }

                else
                {
                    if (row % 2 == 0)
                    {
                        if (column % 2 == 0)
                        {
                            button.Click += UserControl1_Click;

                            button.ButtonCell.Occupied = false;
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
                            button.Click += UserControl1_Click;

                            button.ButtonCell.Occupied = false;
                            button.ButtonCell.DarkColour = true;
                        }

                        else
                        {
                            button.ButtonCell.CurrentFigure = null;
                            button.ButtonCell.DarkColour = false;
                        }
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

            for (int i = 0; i < Math.Pow(battleField.GetLength(1), 2); i++)
            {
                if (latestButton.EndsWith("8"))
                {
                    line++;
                    j = 0;
                    latestButton = Convert.ToString(Convert.ToInt32(latestButton) + 2);
                }

                foreach (UserControl1 control in PointsPanel.Children)
                {
                    if (Convert.ToInt32(control.Name.Substring(1)) == Convert.ToInt32(latestButton) + 1)
                    {
                        battleField[line, j] = control.ButtonCell;

                        latestButton = control.Name.Substring(1);
                        j++;

                        break;
                    }
                }
            }

            TurnChange(whiteTurn);
        }

        /// <summary>
        /// Событие, возникающее при нажатии на какую-либо фигуру.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Прочие сведения...</param>
        void UserControl1_Click (object sender, RoutedEventArgs e)
        {
            UserControl1 button = (UserControl1)sender;

            if (clickCounter == 0 && button.ButtonCell.Occupied &&
            button.ButtonCell.CurrentFigure.MainSide == whiteTurn)
            {
                battleMode = false;
                chosedCell = button;

                if (chosedCell.ButtonCell.CurrentFigure.Type == FigureType.Special)
                {
                    figuresToDestroyForTheKingUL = BattleFieldKingCheck(chosedCell.ButtonPosition, Direction.LeftUp);
                    figuresToDestroyForTheKingUR = BattleFieldKingCheck(chosedCell.ButtonPosition, Direction.RightUp);
                    figuresToDestroyForTheKingDL = BattleFieldKingCheck(chosedCell.ButtonPosition, Direction.LeftDown);
                    figuresToDestroyForTheKingDR = BattleFieldKingCheck(chosedCell.ButtonPosition, Direction.RightDown);

                    battleMode = figuresToDestroyForTheKingDL.Count + figuresToDestroyForTheKingDR.Count +
                    figuresToDestroyForTheKingUL.Count + figuresToDestroyForTheKingUR.Count != 0;

                    positionsToEnableIfCellIsKing.AddRange(KingFigureScanPositions(chosedCell.ButtonPosition, Direction.RightUp));
                    positionsToEnableIfCellIsKing.AddRange(KingFigureScanPositions(chosedCell.ButtonPosition, Direction.LeftUp));
                    positionsToEnableIfCellIsKing.AddRange(KingFigureScanPositions(chosedCell.ButtonPosition, Direction.LeftDown));
                    positionsToEnableIfCellIsKing.AddRange(KingFigureScanPositions(chosedCell.ButtonPosition, Direction.RightDown));
                }

                foreach (UserControl1 cell in PointsPanel.Children)
                {
                    if (chosedCell.ButtonCell.CurrentFigure.Type == FigureType.Common)
                    {
                        if ((!cell.ButtonCell.DarkColour ||
                        cell.ButtonPosition.Column == chosedCell.ButtonPosition.Column ||
                        Math.Abs(cell.ButtonPosition.Column - chosedCell.ButtonPosition.Column) > 1 ||
                        Math.Abs(cell.ButtonPosition.Line - chosedCell.ButtonPosition.Line) > 1 ||
                        cell.ButtonCell.Occupied) && cell != chosedCell)
                        {
                            cell.IsEnabled = false;
                        }
                    }

                    else
                    {
                        if (!battleMode)
                        {
                            if ((!cell.ButtonCell.DarkColour ||
                            cell.ButtonPosition.Column == chosedCell.ButtonPosition.Column ||
                            Math.Abs(cell.ButtonPosition.Column - chosedCell.ButtonPosition.Column) > 1 ||
                            Math.Abs(cell.ButtonPosition.Line - chosedCell.ButtonPosition.Line) > 1 ||
                            cell.ButtonCell.Occupied) && cell != chosedCell)
                            {
                                cell.IsEnabled = false;
                            }

                            if (!cell.ButtonCell.Occupied && Position.Contains(positionsToEnableIfCellIsKing, cell.ButtonPosition))
                            {
                                cell.IsEnabled = true;
                            }
                        }

                        else
                        {
                            if (cell.ButtonCell.DarkColour && cell != chosedCell)
                            {
                                //Проверка на ячейку по направлению: Вправо-Вверх:
                                try
                                {
                                    Int32 index = figuresToDestroyForTheKingUR.Count - 1;

                                    if (cell != figuresToDestroyForTheKingUR[index])
                                    {
                                        cell.IsEnabled = false;
                                    }

                                    else
                                    {
                                        cell.IsEnabled = true;

                                        continue;
                                    }
                                }

                                catch
                                {

                                }

                                //Проверка на ячейку по направлению: Влево-Вверх:
                                try
                                {
                                    Int32 index = figuresToDestroyForTheKingUL.Count - 1;

                                    if (cell != figuresToDestroyForTheKingUL[index])
                                    {
                                        cell.IsEnabled = false;
                                    }

                                    else
                                    {
                                        cell.IsEnabled = true;

                                        continue;
                                    }
                                }

                                catch
                                {

                                }

                                //Проверка на ячейку по направлению: Влево-Вниз:
                                try
                                {
                                    Int32 index = figuresToDestroyForTheKingDL.Count - 1;

                                    if (cell != figuresToDestroyForTheKingDL[index])
                                    {
                                        cell.IsEnabled = false;
                                    }

                                    else
                                    {
                                        cell.IsEnabled = true;

                                        continue;
                                    }
                                }

                                catch
                                {

                                }

                                //Проверка на ячейку по направлению: Вправо-Вниз:
                                try
                                {
                                    Int32 index = figuresToDestroyForTheKingDR.Count - 1;

                                    if (cell != figuresToDestroyForTheKingDR[index])
                                    {
                                        cell.IsEnabled = false;
                                    }

                                    else
                                    {
                                        cell.IsEnabled = true;

                                        continue;
                                    }
                                }

                                catch
                                {

                                }
                            }
                        }
                    }

                    if (chosedCell.ButtonCell.CurrentFigure.Type == FigureType.Common)
                    {
                        if (chosedCell.ButtonCell.CurrentFigure.MainSide)
                        {
                            if (cell.ButtonPosition.Line < chosedCell.ButtonPosition.Line)
                            {
                                cell.IsEnabled = false;
                            }
                        }

                        else
                        {
                            if (cell.ButtonPosition.Line > chosedCell.ButtonPosition.Line)
                            {
                                cell.IsEnabled = false;
                            }
                        }
                    }
                }

                if (chosedCell.ButtonCell.CurrentFigure.Type == FigureType.Common)
                {
                    BattleFieldCheck(button, 1, 1);
                    BattleFieldCheck(button, 1, -1);
                    BattleFieldCheck(button, -1, 1);
                    BattleFieldCheck(button, -1, -1);
                }

                if (enemyTerminateList.Count > 0)
                {
                    foreach (UserControl1 uc1 in PointsPanel.Children)
                    {
                        if (!enemyTerminateList.Contains(uc1) && uc1 != button &&
                        !jumpTroughEnemyPlaces.Contains(uc1))
                        {
                            uc1.IsEnabled = false;
                        }
                    }
                }

                clickCounter++;
            }

            else if (!button.ButtonCell.Occupied && clickCounter > 0)
            {
                try
                {
                    /*Поле battleMode нужно для переопределения логики, согласно которой производится ...
                    ... отбор ячеек, на которые может прыгнуть фигура, ставшая Дамкой.*/

                    Int32 rowPos = 0;
                    Int32 colPos = 0;
                    clickCounter = 0;
                    Cell tmpCell = chosedCell.ButtonCell;
                    Object tmpContent = chosedCell.Content;

                    if (enemyTerminateList.Count == 0 && !battleMode)
                    {
                        whiteTurn = !whiteTurn;
                    }

                    else
                    {
                        if (chosedCell.ButtonCell.CurrentFigure.Type == FigureType.Common)
                        {
                            for (int i = 0; i < battleField.GetLength(0); i++)
                            {
                                for (int j = 0; j < battleField.GetLength(1); j++)
                                {
                                    if (button.ButtonCell == battleField[i, j])
                                    {
                                        rowPos = button.ButtonPosition.Line;
                                        colPos = button.ButtonPosition.Column;

                                        break;
                                    }
                                }
                            }

                            foreach (UserControl1 uc1 in enemyTerminateList)
                            {
                                try
                                {
                                    Position checkPos = new Position(rowPos + 1, colPos + 1);

                                    if (uc1.ButtonPosition.Line == checkPos.Line && uc1.ButtonPosition.Column == checkPos.Column)
                                    {
                                        GameFigureClear(uc1);

                                        battleField[rowPos - 1, colPos - 1].CurrentFigure = null;
                                        battleField[rowPos - 1, colPos - 1].Occupied = false;
                                    }
                                }

                                catch
                                { }

                                try
                                {
                                    Position checkPos = new Position(rowPos + 1, colPos - 1);

                                    if (uc1.ButtonPosition.Line == checkPos.Line && uc1.ButtonPosition.Column == checkPos.Column)
                                    {
                                        GameFigureClear(uc1);

                                        battleField[rowPos - 1, colPos - 1].CurrentFigure = null;
                                        battleField[rowPos - 1, colPos - 1].Occupied = false;
                                    }
                                }

                                catch
                                { }

                                try
                                {
                                    Position checkPos = new Position(rowPos - 1, colPos - 1);

                                    if (uc1.ButtonPosition.Line == checkPos.Line && uc1.ButtonPosition.Column == checkPos.Column)
                                    {
                                        GameFigureClear(uc1);

                                        battleField[rowPos - 1, colPos - 1].CurrentFigure = null;
                                        battleField[rowPos - 1, colPos - 1].Occupied = false;
                                    }
                                }

                                catch
                                { }

                                try
                                {
                                    Position checkPos = new Position(rowPos - 1, colPos + 1);

                                    if (uc1.ButtonPosition.Line == checkPos.Line && uc1.ButtonPosition.Column == checkPos.Column)
                                    {
                                        GameFigureClear(uc1);

                                        battleField[rowPos - 1, colPos - 1].CurrentFigure = null;
                                        battleField[rowPos - 1, colPos - 1].Occupied = false;
                                    }
                                }

                                catch
                                { }
                            }

                            enemyTerminateList.Clear();
                            jumpTroughEnemyPlaces.Clear();
                        }

                        else
                        {
                            Direction attackDirection = DirectionDefinition(chosedCell.ButtonPosition, button.ButtonPosition);

                            if (attackDirection == Direction.RightUp)
                            {
                                for (int i = 0; i < figuresToDestroyForTheKingUR.Count - 1; i++)
                                {
                                    UserControl1 cellToInvoke = (UserControl1)FindName(figuresToDestroyForTheKingUR[i].Name);

                                    GameFigureClear(cellToInvoke);
                                }
                            }

                            else if (attackDirection == Direction.LeftUp)
                            {
                                for (int i = 0; i < figuresToDestroyForTheKingUL.Count - 1; i++)
                                {
                                    UserControl1 cellToInvoke = (UserControl1)FindName(figuresToDestroyForTheKingUL[i].Name);

                                    GameFigureClear(cellToInvoke);
                                }
                            }

                            else if (attackDirection == Direction.LeftDown)
                            {
                                for (int i = 0; i < figuresToDestroyForTheKingDL.Count - 1; i++)
                                {
                                    UserControl1 cellToInvoke = (UserControl1)FindName(figuresToDestroyForTheKingDL[i].Name);

                                    GameFigureClear(cellToInvoke);
                                }
                            }

                            else //Right Down
                            {
                                for (int i = 0; i < figuresToDestroyForTheKingDR.Count - 1; i++)
                                {
                                    UserControl1 cellToInvoke = (UserControl1)FindName(figuresToDestroyForTheKingDR[i].Name);

                                    GameFigureClear(cellToInvoke);
                                }
                            }
                        }
                    }

                    foreach (UserControl1 swapLocation in PointsPanel.Children)
                    {
                        //Зачистка старой ячейки.

                        if (swapLocation.Name == chosedCell.Name)
                        {
                            Position position = swapLocation.ButtonPosition;

                            if (swapLocation.ButtonCell.CurrentFigure.MainSide)
                            {
                                if (button.ButtonPosition.Line == 8)
                                {
                                    swapLocation.ButtonCell.CurrentFigure.Type = FigureType.Special;
                                }
                            }

                            else
                            {
                                if (button.ButtonPosition.Line == 1)
                                {
                                    swapLocation.ButtonCell.CurrentFigure.Type = FigureType.Special;
                                }
                            }

                            swapLocation.Content = button.Content;
                            swapLocation.ButtonCell = button.ButtonCell;
                            swapLocation.ButtonCell.Occupied = false;

                            battleField[position.Line - 1, position.Column - 1] = button.ButtonCell;
                        }
                    }

                    foreach (UserControl1 swapLocation in PointsPanel.Children)
                    {
                        //Перемещение в новую ячейку.

                        if (swapLocation.Name == button.Name)
                        {
                            Position position = swapLocation.ButtonPosition;

                            swapLocation.Content = tmpContent;
                            swapLocation.ButtonCell = tmpCell;
                            swapLocation.ButtonCell.Occupied = true;

                            battleField[position.Line - 1, position.Column - 1] = tmpCell;
                        }
                    }

                    foreach (UserControl1 cell in PointsPanel.Children)
                    {
                        if (cell.ButtonCell.DarkColour)
                        {
                            cell.IsEnabled = true;
                        }
                    }

                    positionsToEnableIfCellIsKing.Clear();
                    TurnChange(whiteTurn);
                }

                catch (NullReferenceException)
                {
                    MessageBox.Show($"Обнаружен баг.\nОдна из ссылок дала Null.",
                    "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                catch (ArgumentNullException)
                {
                    MessageBox.Show($"Обнаружен баг.\nПри вызове какого-то метода в качестве аргумента был передан Null.",
                    "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else if (button.ButtonCell.Occupied)
            {
                if (clickCounter == 1 && button == chosedCell)
                {
                    clickCounter = 0;
                    positionsToEnableIfCellIsKing.Clear();

                    foreach (UserControl1 selectedButton in PointsPanel.Children)
                    {
                        selectedButton.IsEnabled = true;
                    }

                    TurnChange(whiteTurn);
                }
            }

            RefreshColours();
            RefreshSymbols();
        }

        /// <summary>
        /// Метод для проверки клеток на возможность "перескочить". Код, вынесенный в отдельный метод.
        /// </summary>
        /// <param name="button">Клетка, которая была выбрана.</param>
        /// <param name="line">Линия. Первое число для прохода по массиву.</param>
        /// <param name="column">Столбец. Второе число для прохода по массиву.</param>
        void BattleFieldCheck (UserControl1 button, Int32 line, Int32 column)
        {
            try
            {
                Position centerPosition = new Position(Int32.Parse(button.Name[1].ToString()) - 1,
                Int32.Parse(button.Name[2].ToString()) - 1);

                if (battleField[centerPosition.Line + line, centerPosition.Column + column].Occupied
                && battleField[centerPosition.Line + line, centerPosition.Column + column].CurrentFigure.MainSide != whiteTurn)
                {
                    UserControl1 terminate;

                    //Произведена замена метода по поиску по имени. 
                    //Необходима проверка функциональности.
                    terminate = (UserControl1)FindName($"_{button.ButtonPosition.Line + line}" +
                    $"{button.ButtonPosition.Column + column}");

                    line *= 2;
                    column *= 2;

                    foreach (UserControl1 uc1 in PointsPanel.Children)
                    {
                        if (uc1.ButtonCell == battleField[centerPosition.Line + line, centerPosition.Column + column]
                        && !battleField[centerPosition.Line + line, centerPosition.Column + column].Occupied)
                        {
                            enemyTerminateList.Add(terminate);

                            uc1.IsEnabled = true;
                            jumpTroughEnemyPlaces.Add(uc1);
                        }
                    }
                }
            }

            catch
            {
                return;
            }
        }

        /// <summary>
        /// Метод для сканирования клеток на возможность "перескочить", если ходит Дамка. 
        /// </summary>
        /// <param name="startToScanPosition">Стартовая позиция сканирования. Именно здесь находится Дамка, которая ходит.</param>
        /// <param name="dir">Направление, в котором необходимо провести сканирование.</param>
        /// <returns>Список типа "UserControl1", содержащий элементы, которые Дамка уничтожит. 
        /// Последний элемент списка указывает на свободную позицию, куда может прыгнуть Дамка.</returns>
        List<UserControl1> BattleFieldKingCheck (Position startToScanPosition, Direction dir)
        {
            Bool enemyFinded = false;
            List<UserControl1> terminateList = new List<UserControl1>(1);

            if (dir == Direction.RightUp)
            {
                List<Position> ablePoses = KingFigureScanPositions(startToScanPosition, Direction.RightUp);
                UserControl1 uc1;

                foreach (Position able in ablePoses)
                {
                    uc1 = (UserControl1)FindName($"_{able.Line}{able.Column}");

                    if (uc1.ButtonCell.Occupied && uc1.ButtonCell.CurrentFigure.MainSide != whiteTurn)
                    {
                        terminateList.Add(uc1);

                        enemyFinded = true;
                    }

                    else if (!uc1.ButtonCell.Occupied && enemyFinded)
                    {
                        UserControl1 toEnable = (UserControl1)FindName(uc1.Name);
                        toEnable.IsEnabled = true;

                        terminateList.Add(uc1);

                        return terminateList;
                    }
                }
            }

            else if (dir == Direction.RightDown)
            {
                List<Position> ablePoses = KingFigureScanPositions(startToScanPosition, Direction.RightDown);
                UserControl1 uc1;

                foreach (Position able in ablePoses)
                {
                    uc1 = (UserControl1)FindName($"_{able.Line}{able.Column}");

                    if (uc1.ButtonCell.Occupied && uc1.ButtonCell.CurrentFigure.MainSide != whiteTurn)
                    {
                        terminateList.Add(uc1);

                        enemyFinded = true;
                    }

                    else if (!uc1.ButtonCell.Occupied && enemyFinded)
                    {
                        UserControl1 toEnable = (UserControl1)FindName(uc1.Name);
                        toEnable.IsEnabled = true;

                        terminateList.Add(uc1);

                        return terminateList;
                    }
                }
            }

            else if (dir == Direction.LeftUp)
            {
                List<Position> ablePoses = KingFigureScanPositions(startToScanPosition, Direction.LeftUp);
                UserControl1 uc1;

                foreach (Position able in ablePoses)
                {
                    uc1 = (UserControl1)FindName($"_{able.Line}{able.Column}");

                    if (uc1.ButtonCell.Occupied && uc1.ButtonCell.CurrentFigure.MainSide != whiteTurn)
                    {
                        terminateList.Add(uc1);

                        enemyFinded = true;
                    }

                    else if (!uc1.ButtonCell.Occupied && enemyFinded)
                    {
                        UserControl1 toEnable = (UserControl1)FindName(uc1.Name);
                        toEnable.IsEnabled = true;

                        terminateList.Add(uc1);

                        return terminateList;
                    }
                }
            }

            else //LeftDown
            {
                List<Position> ablePoses = KingFigureScanPositions(startToScanPosition, Direction.LeftDown);
                UserControl1 uc1;

                foreach (Position able in ablePoses)
                {
                    uc1 = (UserControl1)FindName($"_{able.Line}{able.Column}");

                    if (uc1.ButtonCell.Occupied && uc1.ButtonCell.CurrentFigure.MainSide != whiteTurn)
                    {
                        terminateList.Add(uc1);

                        enemyFinded = true;
                    }

                    else if (!uc1.ButtonCell.Occupied && enemyFinded)
                    {
                        UserControl1 toEnable = (UserControl1)FindName(uc1.Name);
                        toEnable.IsEnabled = true;

                        terminateList.Add(uc1);

                        return terminateList;
                    }

                    uc1 = (UserControl1)FindName($"_{able.Line}{able.Column}");
                }
            }

            terminateList.Clear();
            return terminateList;
        }

        /// <summary>
        /// Метод для комплексного выполнения последовательности методов по очистке ячейки. Сокращает код.
        /// </summary>
        /// <param name="uc1">Ячейка, которую необходимо очистить.</param>
        void GameFigureClear (UserControl1 uc1)
        {
            GameFigure.FigureDestroyed(uc1.ButtonCell.CurrentFigure, this, hub);

            uc1.ButtonCell = Cell.Destroy(uc1.ButtonCell);

            uc1.Content = "";
        }

        /// <summary>
        /// Метод для сканирования ячеек, в которые может переместиться Дамка.
        /// </summary>
        /// <param name="startPosition">Стартовая позиция, с которой надо начать сканирование.</param>
        /// <param name="dir">Направление, по которому необходимо провести сканирование.</param>
        /// <returns>Список с доступными позициями.</returns>
        List<Position> KingFigureScanPositions (Position startPosition, Direction dir)
        {
            Int32 startLine = startPosition.Line;
            Int32 startColumn = startPosition.Column;
            List<Position> enablePositions = new List<Position>(1);

            if (dir == Direction.RightUp)
            {
                while (startLine <= battleField.GetLength(0) && startColumn <= battleField.GetLength(1))
                {
                    enablePositions.Add(new Position(startLine, startColumn));

                    startLine++;
                    startColumn++;
                }
            }

            else if (dir == Direction.LeftUp)
            {
                while (startLine <= battleField.GetLength(0) && startColumn > 0)
                {
                    enablePositions.Add(new Position(startLine, startColumn));

                    startLine++;
                    startColumn--;
                }
            }

            else if (dir == Direction.LeftDown)
            {
                while (startLine > 0 && startColumn > 0)
                {
                    enablePositions.Add(new Position(startLine, startColumn));

                    startLine--;
                    startColumn--;
                }
            }

            else
            {
                while (startLine > 0 && startColumn <= battleField.GetLength(1))
                {
                    enablePositions.Add(new Position(startLine, startColumn));

                    startLine--;
                    startColumn++;
                }
            }

            return enablePositions;
        }

        /// <summary>
        /// Метод для определения направления по двум заданным координатам.
        /// </summary>
        /// <param name="buttonPosition">Стартовая позиция.</param>
        /// <param name="pos">Конечная позиция.</param>
        /// <returns>Направление, в котором расположены точки.</returns>
        public static Direction DirectionDefinition (Position buttonPosition, Position pos)
        {
            Position toDefDirection = buttonPosition - pos;

            if (toDefDirection.Line > 0 && toDefDirection.Column > 0)
            {
                return Direction.LeftDown;
            }

            else if (toDefDirection.Line < 0 && toDefDirection.Column > 0)
            {
                return Direction.LeftUp;
            }

            else if (toDefDirection.Line < 0 && toDefDirection.Column < 0)
            {
                return Direction.RightUp;
            }

            else
            {
                return Direction.RightDown;
            }
        }

        /// <summary>
        /// Метод для смены активных фигур ("Шашек").
        /// </summary>
        /// <param name="turn">Сторона, которая сейчас будет ходить.</param>
        public void TurnChange (Bool turn)
        {
            foreach (UserControl1 button in PointsPanel.Children)
            {
                if (button.ButtonCell.CurrentFigure != null &&
                button.ButtonCell.CurrentFigure.MainSide != turn)
                {
                    button.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Метод для обновления цветов у игровых фигур.
        /// </summary>
        public void RefreshColours ()
        {
            foreach (UserControl1 uc1 in PointsPanel.Children)
            {
                if (uc1.ButtonCell.Occupied)
                {
                    if (uc1.ButtonCell.CurrentFigure.MainSide)
                    {
                        uc1.Foreground = new SolidColorBrush(Colour.FromRgb(255, 255, 255));
                    }

                    else
                    {
                        uc1.Foreground = new SolidColorBrush(Colour.FromRgb(0, 0, 0));
                    }
                }
            }
        }

        /// <summary>
        /// Метод для обновления внешнего вида у игровых фигур.
        /// </summary>
        public void RefreshSymbols ()
        {
            foreach (UserControl1 uc in PointsPanel.Children)
            {
                if (uc.ButtonCell.Occupied && uc.ButtonCell.CurrentFigure.Type == FigureType.Special)
                {
                    uc.Content = GameFigure.KingFigure;
                }
            }
        }

        /// <summary>
        /// Событие, возникающее при закрытии формы. Нужно для закрытия окна-хаба, если игровой процесс был прерван.
        /// </summary>
        /// <param name="sender">Элемент, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        void GameWindow_Closed (object sender, EventArgs e)
        {
            if (FirstSide.Count > 0 && SecondSide.Count > 0)
            {
                //Если выход был произведен во время хода первого пользователя (Белые Фигуры), то побег засчитывается ему:
                if (whiteTurn)
                {
                    Hub.FirstUser.AddLeave();
                }

                //В ином случае побег засчитывается другому пользователю:
                else
                {
                    Hub.SecondUser.AddLeave();
                }

                //Закрытие Хаба, чтобы приложение было полностью остановлено:
                hub.Close();
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
                return mainEdge;
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
        public Cell ()
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
        public Cell (Bool occupied, Bool edge, GameFigure currentFigure, Bool secondEdge, Bool darkColour)
        {
            Occupied = occupied;
            MainEdge = edge;
            CurrentFigure = currentFigure;
            SecondEdge = secondEdge;
            DarkColour = darkColour;
        }

        /// <summary>
        /// Метод для "Обнуления" ячейки, когда фигура на ней уничтожена.
        /// </summary>
        /// <param name="toDestroy">Ячейка, которую необходимо очистить.</param>
        /// <returns>Очищенная ячейка.</returns>
        public static Cell Destroy (Cell toDestroy)
        {
            toDestroy.CurrentFigure = null;
            toDestroy.Occupied = false;

            return toDestroy;
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

        /// <summary>
        /// Метод для определения содержания какого-либо элемента в списке.
        /// </summary>
        /// <param name="pos">Список, который необходимо проверить.</param>
        /// <param name="toFind">Элемент, который необходимо найти.</param>
        /// <returns>Логическая переменная, отвечающая за то, содержится ли элемент в списке или нет.</returns>
        public static Bool Contains (List<Position> pos, Position toFind)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                if (pos[i].Column == toFind.Column && pos[i].Line == toFind.Line)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Перегрузка оператора '-' (Вычитание).
        /// </summary>
        /// <param name="posOne">Позиция, от которой нужно отнимать.</param>
        /// <param name="posTwo">Позиция, которую нужно отнять.</param>
        /// <returns>Новая позиция.</returns>
        public static Position operator - (Position posOne, Position posTwo)
        {
            return new Position(posOne.Line - posTwo.Line, posOne.Column - posTwo.Column);
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
        /// Поле, содержащее символ простой фигуры.
        /// </summary>
        static Char commonFigure = Convert.ToChar(11044);

        /// <summary>
        /// Поле, содержащее символ Дамки.
        /// </summary>
        static Char kingFigure = Convert.ToChar(9711);

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
        /// Свойство, содержащее символ простой фигуры.
        /// </summary>
        public static Char CommonFigure
        {
            get
            {
                return commonFigure;
            }

            set
            {
                commonFigure = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее символ Дамки.
        /// </summary>
        public static Char KingFigure
        {
            get
            {
                return kingFigure;
            }

            set
            {
                kingFigure = value;
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

        /// <summary>
        /// Метод для уничтожения какой-либо фигуры.
        /// </summary>
        /// <param name="destroyedFigure">Уничтожаемая фигура.</param>
        /// <param name="windowToCloseIfFigureIsLast">Экземпляр основного окна, необходимый для его закрытия, если фигура окажется последней.</param>
        /// <param name="hubToShowIfFigureIsLast">Экземпляр окна "Hub" для открытия, если фигура окажется последней.</param>
        public static void FigureDestroyed (GameFigure destroyedFigure, MainWindow windowToCloseIfFigureIsLast, Hub hubToShowIfFigureIsLast)
        {
            if (destroyedFigure.MainSide)
            {
                MainWindow.FirstSide.Remove(destroyedFigure);

                if (MainWindow.FirstSide.Count == 0)
                {
                    Hub.FirstUser.AddGame();
                    Hub.SecondUser.AddWin();

                    hubToShowIfFigureIsLast.RefreshInformation();

                    windowToCloseIfFigureIsLast.Close();
                    hubToShowIfFigureIsLast.Show();
                }
            }

            else
            {
                MainWindow.SecondSide.Remove(destroyedFigure);

                if (MainWindow.SecondSide.Count == 0)
                {
                    Hub.FirstUser.AddWin();
                    Hub.SecondUser.AddGame();

                    hubToShowIfFigureIsLast.RefreshInformation();

                    windowToCloseIfFigureIsLast.Close();
                    hubToShowIfFigureIsLast.Show();
                }
            }
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

    /// <summary>
    /// Перечисление типа "Enum" для определения движения сканирования позиций 
    /// для перемещения Дамки.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Сканирование будет произведено вправо вверх.
        /// </summary>
        RightUp,

        /// <summary>
        /// Сканирование будет произведено влево вверх.
        /// </summary>
        LeftUp,

        /// <summary>
        /// Сканирование будет произведено влево вниз.
        /// </summary>
        LeftDown,

        /// <summary>
        /// Сканирование будет произведено вправо вниз.
        /// </summary>
        RightDown
    }
}
