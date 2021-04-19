using System;
using System.Windows;
using System.Collections.Generic;
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
        /// Поле, содержащее выбранный при первом клике элемент 
        /// ("Шашка").
        /// </summary>
        UserControl1 chosedCell;
        /// <summary>
        /// Поле, содержащее все ячейки игрового поля.
        /// </summary>
        Cell[,] battleField = new Cell[8, 8];
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
        /// Точка входа в Приложение с Интерфесом.
        /// </summary>
        public MainWindow()
        {
            Int32 j = 0;
            Int32 line = 0;
            String latestButton = "10";
            Int32 limit = battleField.GetLength(0) / 2;

            battleFieldMiddleCellsRows.Add(limit);
            battleFieldMiddleCellsRows.Add(limit + 1);

            whiteTurn = true;

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

                            button.Content = "Белая ";
                        }

                        else
                        {
                            secondSide.Add(button.ButtonCell.CurrentFigure);

                            button.Content = "Черная ";
                        }

                        button.Click += UserControl1_Click;

                        button.Content += "Пешка.";
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

                            button.Content = "Белая ";
                        }

                        else
                        {
                            secondSide.Add(button.ButtonCell.CurrentFigure);

                            button.Content = "Черная ";
                        }

                        button.Click += UserControl1_Click;

                        button.Content += "Пешка.";
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

            for (int i = 0; i < 63; i++)
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
        void UserControl1_Click(object sender, RoutedEventArgs e)
        {
            UserControl1 button = (UserControl1)sender;

            if (clickCounter == 0 && button.ButtonCell.Occupied &&
            button.ButtonCell.CurrentFigure.MainSide == whiteTurn)
            {
                chosedCell = button;
                List<Position> positionsToEnableIfCellIsKing = new List<Position>(1);

                if (chosedCell.ButtonCell.CurrentFigure.Type == FigureType.Special)
                {
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

                BattleFieldCheck(button, 1, 1);
                BattleFieldCheck(button, 1, -1);
                BattleFieldCheck(button, -1, 1);
                BattleFieldCheck(button, -1, -1);

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
                Int32 rowInd = 0;
                Int32 colInd = 0;
                clickCounter = 0;
                Cell tmpCell = chosedCell.ButtonCell;
                Object tmpContent = chosedCell.Content;

                if (enemyTerminateList.Count == 0)
                {
                    whiteTurn = !whiteTurn;
                }

                else
                {
                    for (int i = 0; i < battleField.GetLength(0); i++)
                    {
                        for (int j = 0; j < battleField.GetLength(1); j++)
                        {
                            if (button.ButtonCell == battleField[i, j])
                            {
                                rowInd = i;
                                colInd = j;

                                break;
                            }
                        }
                    }

                    foreach (UserControl1 uc1 in enemyTerminateList)
                    {
                        try
                        {
                            if (uc1.ButtonCell == battleField[rowInd - 1, colInd - 1])
                            {
                                GameFigureClear(uc1);

                                break;
                            }
                        }

                        catch
                        { }

                        try
                        {
                            if (uc1.ButtonCell == battleField[rowInd - 1, colInd + 1])
                            {
                                GameFigureClear(uc1);

                                break;
                            }
                        }

                        catch
                        { }

                        try
                        {
                            if (uc1.ButtonCell == battleField[rowInd + 1, colInd - 1])
                            {
                                GameFigureClear(uc1);

                                break;
                            }
                        }

                        catch
                        { }

                        try
                        {
                            if (uc1.ButtonCell == battleField[rowInd + 1, colInd + 1])
                            {
                                GameFigureClear(uc1);

                                break;
                            }
                        }

                        catch
                        { }
                    }

                    enemyTerminateList.Clear();
                    jumpTroughEnemyPlaces.Clear();
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

                                swapLocation.Content = "Белая Дамка.";

                                tmpContent = "Белая Дамка.";
                            }
                        }

                        else
                        {
                            if (button.ButtonPosition.Line == 1)
                            {
                                swapLocation.ButtonCell.CurrentFigure.Type = FigureType.Special;

                                swapLocation.Content = "Черная Дамка.";

                                tmpContent = "Черная Дамка.";
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

                TurnChange(whiteTurn);
            }

            else if (button.ButtonCell.Occupied)
            {
                if (clickCounter == 1 && button == chosedCell)
                {
                    clickCounter = 0;

                    foreach (UserControl1 selectedButton in PointsPanel.Children)
                    {
                        selectedButton.IsEnabled = true;
                    }

                    TurnChange(whiteTurn);
                }
            }
        }

        /// <summary>
        /// Метод для проверки клеток на возможность "перескочить". Код, вынесенный в отдельный метод.
        /// </summary>
        /// <param name="button">Клетка, которая была выбрана.</param>
        /// <param name="line">Линия. Первое число для прохода по массиву.</param>
        /// <param name="column">Столбец. Второе число для прохода по массиву.</param>
        void BattleFieldCheck(UserControl1 button, Int32 line, Int32 column)
        {
            try
            {
                Position centerPosition = new Position(Int32.Parse(button.Name[1].ToString()) - 1,
                Int32.Parse(button.Name[2].ToString()) - 1);

                if (battleField[centerPosition.Line + line, centerPosition.Column + column].Occupied
                && battleField[centerPosition.Line + line, centerPosition.Column + column].CurrentFigure.MainSide != whiteTurn)
                {
                    UserControl1 terminate = (UserControl1)FindName($"_{Int32.Parse(button.Name[1].ToString()) + line}" +
                    $"{Int32.Parse(button.Name[2].ToString()) + column}");

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
        /// Метод для комплексного выполнения последовательности методов. Сокращает код.
        /// </summary>
        /// <param name="uc1">Ячейка, которую необходимо очистить.</param>
        void GameFigureClear(UserControl1 uc1)
        {
            GameFigure.FigureDestroyed(uc1.ButtonCell.CurrentFigure);

            uc1.ButtonCell = Cell.Destroy(uc1.ButtonCell);

            uc1.Content = "";
        }

        /// <summary>
        /// Метод для сканирования ячеек, в которые может переместиться Дамка.
        /// </summary>
        /// <param name="startPosition">Стартовая позиция, с которой надо начать сканирование.</param>
        /// <param name="dir">Направление, по которому необходимо провести сканирование.</param>
        /// <returns>Список с доступными позициями.</returns>
        List<Position> KingFigureScanPositions(Position startPosition, Direction dir)
        {
            Int32 startLine = startPosition.Line;
            Int32 startColumn = startPosition.Column;
            List<Position> enablePositions = new List<Position>(1);

            if (dir == Direction.RightUp)
            {
                while (startColumn <= battleField.GetLength(1) && startLine > 0)
                {
                    enablePositions.Add(new Position(startLine, startColumn));

                    startLine--;
                    startColumn++;
                }
            }

            else if (dir == Direction.LeftUp)
            {
                while (startColumn > 0 && startLine > 0)
                {
                    enablePositions.Add(new Position(startLine, startColumn));

                    startLine--;
                    startColumn--;
                }
            }

            else if (dir == Direction.LeftDown)
            {
                while (startLine <= battleField.GetLength(0) && startColumn > 0)
                {
                    enablePositions.Add(new Position(startLine, startColumn));

                    startLine++;
                    startColumn--;
                }
            }

            else
            {
                while (startColumn <= battleField.GetLength(1) && startLine <= battleField.GetLength(0))
                {
                    enablePositions.Add(new Position(startLine, startColumn));

                    startLine++;
                    startColumn++;
                }
            }

            return enablePositions;
        }

        /// <summary>
        /// Метод для смены активных фигур ("Шашек").
        /// </summary>
        /// <param name="turn">Сторона, которая сейчас будет ходить.</param>
        public void TurnChange(Bool turn)
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

        /// <summary>
        /// Метод для "Обнуления" ячейки, когда фигура на ней уничтожена.
        /// </summary>
        /// <param name="toDestroy">Ячейка, которую необходимо очистить.</param>
        /// <returns>Очищенная ячейка.</returns>
        public static Cell Destroy(Cell toDestroy)
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
        public Position(Int32 line, Int32 column)
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
        public static Bool Contains(List<Position> pos, Position toFind)
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
        public GameFigure(Position location, FigureType type, Bool side)
        {
            Location = location;
            Type = type;
            MainSide = side;
        }

        /// <summary>
        /// Метод для уничтожения какой-либо фигуры.
        /// </summary>
        /// <param name="destroyedFigure">Уничтожаемая фигура.</param>
        public static void FigureDestroyed(GameFigure destroyedFigure)
        {
            if (destroyedFigure.MainSide)
            {
                MainWindow.FirstSide.Remove(destroyedFigure);

                if (MainWindow.FirstSide.Count == 0)
                {
                    MessageBox.Show("ВТОРОЙ ПОБЕДИЛ!");
                }
            }

            else
            {
                MainWindow.SecondSide.Remove(destroyedFigure);

                if (MainWindow.SecondSide.Count == 0)
                {
                    MessageBox.Show("ПЕРВЫЙ ПОБЕДИЛ!");
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
