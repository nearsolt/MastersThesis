﻿using System.Xml;
using System.Diagnostics;
using static MastersThesis.MainForm.PlanarObjectStore;
using static MastersThesis.MeshStore;

namespace MastersThesis {
    public partial class MainForm : Form {

        #region Private Class Variables
        /// <summary>
        /// Cостояния приложения
        /// </summary>
        private enum ApplicationState : int {
            /// <summary>
            /// Генерация узлов
            /// </summary>
            NodeGeneration = 0,
            /// <summary>
            /// Жадная триангуляция
            /// </summary>
            GreedyTriangulation = 1,
            /// <summary>
            /// Триангуляция Делоне
            /// </summary>
            DelaunayTriangulation = 2,
            /// <summary>
            /// Триангуляция методом измельчения
            /// </summary>
            MeshRefinement = 3,
            /// <summary>
            /// Триангуляция Делоне, по которой была построена триангуляция методом измельчения
            /// </summary>
            ParentDelaunayTriangulation = 4
        }
        /// <summary>
        /// Текущиее состояние приложения
        /// </summary>
        private ApplicationState _applicationState = ApplicationState.NodeGeneration;
        /// <summary>
        /// Размеры используемой области в pictureBox_mainPic
        /// </summary>
        private SizeF _canvasSize;
        /// <summary>
        /// Точка начала координат в pictureBox_mainPic
        /// </summary>
        private PointF _canvasOrgin;
        /// <summary>
        /// Начало диапазона по оси Ox
        /// </summary>
        private double _xAxisStart;
        /// <summary>
        /// Конец диапазона по оси Ox
        /// </summary>
        private double _xAxisEnd;
        /// <summary>
        /// Начало диапазона по оси Oy
        /// </summary>
        private double _yAxisStart;
        /// <summary>
        /// Конец диапазона по оси Oy
        /// </summary>
        private double _yAxisEnd;
        /// <summary>
        /// Коэффициент растяжения для ширины в pictureBox_mainPic
        /// </summary>
        private double _widthScalingCoeff;
        /// <summary>
        /// Коэффициент растяжения для высоты в pictureBox_mainPic
        /// </summary>
        private double _heightScalingCoeff;
        /// <summary>
        /// Счетчик для анимации
        /// </summary>
        private int _animationCounter;
        /// <summary>
        /// Коэффициент измельчения q
        /// </summary>
        private int _meshRefinementCoeff = 3;
        /// <summary>
        /// Количество знаков после запятой при вычислении координат внутренних узлов в методе измельчения
        /// </summary>
        private readonly int _decimalPlaces = 3;
        /// <summary>
        /// Интервал для таймера анимации (мс)
        /// </summary>
        private readonly int _timerInterval = 1000;
        /// <summary>
        /// Текущая триангуляция (PlanarObjectStore)
        /// </summary>
        private PlanarObjectStore _triangulation = null!;
        /// <summary>
        /// Триангуляция Делоне, по которой была построена триангуляция методом измельчения (PlanarObjectStore)
        /// </summary>
        private PlanarObjectStore _parentTriangulation = null!;
        /// <summary>
        /// Список узлов (NodeStore) для использования в методе измельчения
        /// </summary>
        private List<NodeStore> _nodeStoreList = new List<NodeStore>();
        /// <summary>
        /// Список ребер (EdgeStore) для использования в методе измельчения
        /// </summary>
        private List<EdgeStore> _edgeStoreList = new List<EdgeStore>();
        /// <summary>
        /// Список треугольников (TriangleStore) для использования в методе измельчения
        /// </summary>
        private List<TriangleStore> _triangleStoreList = new List<TriangleStore>();
        /// <summary>
        /// Путь к приложению GnuPlot
        /// </summary>
        private string? _gnuplotPath;
        #endregion

        #region Exact Solution
        /// <summary>
        /// Точное значение функции f(x,y)
        /// </summary>
        /// <param name="x">Значение x</param>
        /// <param name="y">Значение y</param>
        /// <returns></returns>
        private static double ExactSolution(double x, double y) {
            return Math.Cos(x) * Math.Sin(0.5 * y) * x + y;

            // Функция Матьяса
            //return 0.26 * (x * x + y * y) - 0.48 * x * y;
        }
        #endregion

        #region Constructor & Event Handlers
        public MainForm() {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            GenerateRandomNodes();
        }
        private void AnimationTimer_Tick(object sender, EventArgs e) {
            if (_animationCounter == _triangulation.AnimationList.Count - 1) {
                _animationCounter = 0;
            } else {
                _animationCounter++;
            }
            pictureBox_mainPic.Refresh();
        }
        private void MainPic_Paint(object sender, PaintEventArgs e) {
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.TranslateTransform(_canvasOrgin.X, _canvasOrgin.Y);

            switch (_applicationState) {
                case ApplicationState.NodeGeneration:
                    _triangulation.DrawNodeList(ref graphics, checkBox_labelVisibility.Checked, _widthScalingCoeff, _heightScalingCoeff);
                    break;
                case ApplicationState.GreedyTriangulation:
                    _triangulation.DrawTriangulation(ref graphics, checkBox_labelVisibility.Checked, checkBox_tweenAnimation.Checked, ref _animationCounter,
                                                     _widthScalingCoeff, _heightScalingCoeff);
                    break;
                case ApplicationState.DelaunayTriangulation:
                    _triangulation.DrawTriangulation(ref graphics, checkBox_labelVisibility.Checked, checkBox_tweenAnimation.Checked, ref _animationCounter,
                                                     checkBox_innerTriangleVisibility.Checked, checkBox_circumcircleVisibility.Checked, _widthScalingCoeff, _heightScalingCoeff);
                    break;
                case ApplicationState.MeshRefinement:
                    _triangulation.DrawTriangulation(ref graphics, checkBox_labelVisibility.Checked, checkBox_tweenAnimation.Checked, ref _animationCounter,
                                                     checkBox_innerTriangleVisibility.Checked, _widthScalingCoeff, _heightScalingCoeff);
                    break;
                case ApplicationState.ParentDelaunayTriangulation:
                    break;
            }
        }

        #region CheckBoxs
        private void LabelVisibility_CheckedChanged(object sender, EventArgs e) {
            pictureBox_mainPic.Refresh();
        }
        private void CircumcircleVisibility_CheckedChanged(object sender, EventArgs e) {
            pictureBox_mainPic.Refresh();
        }
        private void TweenAnimation_CheckedChanged(object sender, EventArgs e) {
            if (checkBox_tweenAnimation.Checked) {
                _animationCounter = 0;
                timer_animationTimer.Interval = _timerInterval;
                timer_animationTimer.Enabled = true;
                timer_animationTimer.Start();
            } else {
                timer_animationTimer.Stop();
                timer_animationTimer.Enabled = false;
            }
            pictureBox_mainPic.Refresh();
        }
        private void InnerTriangleVisibility_CheckedChanged(object sender, EventArgs e) {
            pictureBox_mainPic.Refresh();
        }
        private void SetDomainOfDefinition_CheckedChanged(object sender, EventArgs e) {
            if (checkBox_setDomainOfDefinition.Checked) {
                groupBox_domainOfDefinition.Enabled = true;
            } else {
                groupBox_domainOfDefinition.Enabled = false;
            }
        }
        #endregion

        #region Buttons
        private void GenerateNodes_Click(object sender, EventArgs e) {
            checkBox_tweenAnimation.Checked = false;
            checkBox_innerTriangleVisibility.Checked = false;
            checkBox_innerTriangleVisibility.Enabled = true;
            checkBox_circumcircleVisibility.Checked = false;
            checkBox_circumcircleVisibility.Enabled = true;
            GenerateRandomNodes();
        }
        private void GreedyTriangulation_Click(object sender, EventArgs e) {
            if (_applicationState == ApplicationState.MeshRefinement) {
                MessageBox.Show("Построение жадной триангуляции недоступно после применения метода измельчения. Сгенерируйте множество узлов еще раз.", "Информация",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            checkBox_innerTriangleVisibility.Checked = false;
            checkBox_innerTriangleVisibility.Enabled = false;
            checkBox_circumcircleVisibility.Checked = false;
            checkBox_circumcircleVisibility.Enabled = false;
            GreedyTriangulation();
        }
        private void DelaunayTriangulation_Click(object sender, EventArgs e) {
            if (_applicationState == ApplicationState.MeshRefinement) {
                MessageBox.Show("Построение триангуляции Делоне недоступно после применения метода измельчения. Сгенерируйте множество узлов еще раз.", "Информация",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            checkBox_innerTriangleVisibility.Enabled = true;
            checkBox_circumcircleVisibility.Enabled = true;
            DelaunayTriangulation();
        }
        private void MeshRefinement_Click(object sender, EventArgs e) {
            if (_applicationState != ApplicationState.DelaunayTriangulation) {
                MessageBox.Show("Построение триангуляции методом измельчения доступно только после триангуляции Делоне.", "Информация",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            checkBox_circumcircleVisibility.Checked = false;
            checkBox_circumcircleVisibility.Enabled = false;
            MeshRefinement();
        }
        private void Interpolation_Click(object sender, EventArgs e) {
            if (_applicationState != ApplicationState.DelaunayTriangulation && _applicationState != ApplicationState.MeshRefinement) {
                MessageBox.Show("Построение интерполяции доступно только после триангуляции Делоне или применения метода измельчения.", "Информация",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }

            if (_gnuplotPath == null) {
                GetGnuPlotPath();
            }
            if (_gnuplotPath == null) {
                MessageBox.Show("Построение графиков не доступно: отсутствует доступ к приложению GnuPlot. Будут представлены только численнные результаты аппроксимации функции.",
                                "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            button_interpolation.Enabled = false;
            Interpolation();
            button_interpolation.Enabled = true;
        }
        #endregion

        #endregion

        #region Methods

        #region Initialization
        /// <summary>
        /// Инициализация области определения и коэффициентов растяжения ширины и высоты в pictureBox_mainPic
        /// </summary>
        /// <param name="isValidated">Проверка на корректность инициализации области</param>
        private void InitializeDomainOfDefinition(out bool isValidated) {
            if (checkBox_setDomainOfDefinition.Checked) {
                _xAxisStart = (double)numericUpDown_xAxisStart.Value;
                _xAxisEnd = (double)numericUpDown_xAxisEnd.Value;

                _yAxisStart = (double)numericUpDown_yAxisStart.Value;
                _yAxisEnd = (double)numericUpDown_yAxisEnd.Value;

                if (_xAxisStart >= _xAxisEnd) {
                    MessageBox.Show($"При инициализации области были введены некорректные данные: xAxisStart >= xAxisEnd ({_xAxisStart} >= {_xAxisEnd}).", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    isValidated = false;
                    return;
                }
                if (_yAxisStart >= _yAxisEnd) {
                    MessageBox.Show($"При инициализации области были введены некорректные данные: yAxisStart >= yAxisEnd ({_yAxisStart} >= {_yAxisEnd}).", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    isValidated = false;
                    return;
                }

                _widthScalingCoeff = Math.Round(_canvasSize.Width / (_xAxisEnd - _xAxisStart), 2, MidpointRounding.AwayFromZero);
                _heightScalingCoeff = Math.Round(_canvasSize.Height / (_yAxisEnd - _yAxisStart), 2, MidpointRounding.AwayFromZero);

            } else {
                _xAxisStart = -Math.Round(_canvasSize.Width * 0.5, 2, MidpointRounding.AwayFromZero);
                _xAxisEnd = Math.Round(_canvasSize.Width * 0.5, 2, MidpointRounding.AwayFromZero);

                _yAxisStart = -Math.Round(_canvasSize.Height * 0.5, 2, MidpointRounding.AwayFromZero);
                _yAxisEnd = Math.Round(_canvasSize.Height * 0.5, 2, MidpointRounding.AwayFromZero);

                _widthScalingCoeff = 1;
                _heightScalingCoeff = 1;
            }
            isValidated = true;
        }
        /// <summary>
        /// Инициализация используемой области в pictureBox_mainPic и точки начала координат 
        /// </summary>
        /// <param name="isValidated">Проверка на корректность инициализации области</param>
        private void InitializeCanvasSize(out bool isValidated) {
            _canvasSize = new SizeF((float)(pictureBox_mainPic.Width * 0.9), (float)(pictureBox_mainPic.Height * 0.9));
            InitializeDomainOfDefinition(out isValidated);
            if (!isValidated) {
                return;
            }
            if (checkBox_setDomainOfDefinition.Checked) {
                _canvasOrgin = new PointF((float)(pictureBox_mainPic.Width * 0.5 - (_xAxisStart + _xAxisEnd) * 0.5 * _widthScalingCoeff),
                                          (float)(pictureBox_mainPic.Height * 0.5 + (_yAxisStart + _yAxisEnd) * 0.5 * _heightScalingCoeff));
            } else {
                _canvasOrgin = new PointF((float)(pictureBox_mainPic.Width * 0.5), (float)(pictureBox_mainPic.Height * 0.5));
            }
            pictureBox_mainPic.Refresh();
        }
        /// <summary>
        /// Получение пути к приложению GnuPlot из конфигурационного файла
        /// </summary>
        private void GetGnuPlotPath() {
            try {
                XmlDocument config = new XmlDocument();
                config.Load("GnuPlot.config");
                XmlElement? root = config.DocumentElement;

                if (root == null) {
                    throw new Exception("в config файле отсутствует корневой элемент. Воспользуйтесь корректным конфигурационным файлом.");
                }

                XmlNode? pathNode = root.SelectSingleNode("GnuPlotPath");

                if (pathNode == null) {
                    throw new Exception("в config файле отсутствует тег <GnuPlotPath>. Воспользуйтесь корректным конфигурационным файлом.");
                }

                string path = pathNode.InnerText;

                if (!new FileInfo(path).Exists) {
                    throw new Exception($"в config файле в теге <GnuPlotPath> указан некорректный путь к приложению GnuPlot:\n{path}\nВведите корректные данные.");
                }

                _gnuplotPath = $@"{path}";

            } catch (Exception ex) {
                _gnuplotPath = null;
                DebugLog("Error", $"GetGnuPlotPath: при получении данных из конфигурационного файла произошла ошибка.\n{ex}.");
                MessageBox.Show($"При получении данных из конфигурационного файла произошла ошибка:\n{ex.Message}.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

            }
        }
        #endregion

        #region Triangulation
        /// <summary>
        /// Random-генерация узлов в используемой области
        /// </summary>
        private void GenerateRandomNodes() {
            InitializeCanvasSize(out bool isValidated);
            if (!isValidated) {
                return;
            }
            _applicationState = ApplicationState.NodeGeneration;
            groupBox_axisNum.Enabled = false;
            int nodeCount = (int)numericUpDown_numberOfNodes.Value;

            _nodeStoreList.Clear();
            _edgeStoreList.Clear();
            _triangleStoreList.Clear();

            _triangulation = new PlanarObjectStore();

            _triangulation.NodeList.Add(new Node(0, _xAxisStart, _yAxisStart));
            _triangulation.NodeList.Add(new Node(1, _xAxisStart, _yAxisEnd));
            _triangulation.NodeList.Add(new Node(2, _xAxisEnd, _yAxisStart));
            _triangulation.NodeList.Add(new Node(3, _xAxisEnd, _yAxisEnd));

            Random random = new Random();
            if (nodeCount > 4) {
                for (int j = 4; j < nodeCount; j++) {
                    double tmpXCoord = Math.Round(random.Next(Convert.ToInt32(Math.Ceiling(_xAxisStart)) * 100, Convert.ToInt32(Math.Floor(_xAxisEnd)) * 100) / 100.0, 2, MidpointRounding.AwayFromZero);
                    double tmpYCoord = Math.Round(random.Next(Convert.ToInt32(Math.Ceiling(_yAxisStart)) * 100, Convert.ToInt32(Math.Floor(_yAxisEnd)) * 100) / 100.0, 2, MidpointRounding.AwayFromZero);

                    if (_triangulation.NodeList.Exists(obj => obj.XCoordinate == tmpXCoord && obj.YCoordinate == tmpYCoord)) {
                        j--;
                        continue;
                    }
                    _triangulation.NodeList.Add(new Node(j, tmpXCoord, tmpYCoord));
                }
            }
            pictureBox_mainPic.Refresh();
        }
        /// <summary>
        /// Построение жадной триангуляции
        /// </summary>
        private void GreedyTriangulation() {
            _applicationState = ApplicationState.GreedyTriangulation;
            groupBox_axisNum.Enabled = false;
            _animationCounter = 0;

            _triangulation.EdgeList.Clear();
            _triangulation.AnimationList.Clear();

            List<NodeStore> inputNodeList = new List<NodeStore>();

            for (int j = 0; j < _triangulation.NodeList.Count; j++) {
                inputNodeList.Add(new NodeStore(j, _triangulation.NodeList[j].XCoordinate, _triangulation.NodeList[j].YCoordinate, _triangulation.NodeList[j]));
            }

            _ = new MeshStore(inputNodeList, out List<Edge> outputEdgeList, out List<TweenAnimation> outputAnimationList);

            _triangulation.EdgeList = outputEdgeList;
            _triangulation.AnimationList = outputAnimationList;
            pictureBox_mainPic.Refresh();

            #region Debug Logger
            DebugLog("Info", $"GT: nodeListCount: {_triangulation.NodeList.Count}; edgeListCount: {_triangulation.EdgeList.Count}.");
            #endregion
        }
        /// <summary>
        /// Построение триангуляции Делоне
        /// </summary>
        private void DelaunayTriangulation() {
            _applicationState = ApplicationState.DelaunayTriangulation;
            groupBox_axisNum.Enabled = true;
            _animationCounter = 0;

            _triangulation.EdgeList.Clear();
            _triangulation.AnimationList.Clear();

            List<NodeStore> inputNodeList = new List<NodeStore>();
            List<Node> sortedNodeList = _triangulation.NodeList.OrderBy(obj => obj.XCoordinate).ThenBy(obj => obj.YCoordinate).ToList();

            for (int j = 0; j < sortedNodeList.Count; j++) {
                inputNodeList.Add(new NodeStore(j, sortedNodeList[j].XCoordinate, sortedNodeList[j].YCoordinate, sortedNodeList[j]));
            }

            _ = new MeshStore(inputNodeList, ref _nodeStoreList, ref _edgeStoreList, ref _triangleStoreList,
                              out List<Edge> outputEdgeList, out List<Triangle> outputTriangleList, out List<TweenAnimation> outputAnimationList);

            _triangulation.EdgeList = outputEdgeList;
            _triangulation.TriangleList = outputTriangleList;
            _triangulation.AnimationList = outputAnimationList;
            pictureBox_mainPic.Refresh();

            #region Debug Logger            
            int nLC = _triangulation.NodeList.Count;
            int eLC = _triangulation.EdgeList.Count;
            int tLC = _triangulation.TriangleList.Count;
            DebugLog("Info", $"DT: nodeListCount: {nLC}; edgeListCount: {eLC}; triangleListCount: {tLC}; identity (n-e+t=1): {nLC - eLC + tLC == 1}.");
            #endregion
        }
        /// <summary>
        /// Построение триангуляции методом измельчения
        /// </summary>
        private void MeshRefinement() {
            _applicationState = ApplicationState.MeshRefinement;
            _meshRefinementCoeff = (int)numericUpDown_meshRefinementCoeff.Value;
            _animationCounter = 0;

            _parentTriangulation = new PlanarObjectStore();
            _parentTriangulation.NodeList = _triangulation.NodeList.ToList();
            _parentTriangulation.EdgeList = _triangulation.EdgeList.ToList();
            _parentTriangulation.TriangleList = _triangulation.TriangleList.ToList();

            _ = new MeshStore(_nodeStoreList, _edgeStoreList, _triangleStoreList, _triangulation.AnimationList.Last(), _decimalPlaces, _meshRefinementCoeff,
                              out List<Node> outputNodeList, out List<Edge> outputEdgeList, out List<Triangle> outputTriangleList, out List<TweenAnimation> outputAnimationList);

            _triangulation.NodeList = outputNodeList;
            _triangulation.EdgeList = outputEdgeList;
            _triangulation.TriangleList = outputTriangleList;
            _triangulation.AnimationList = outputAnimationList;
            pictureBox_mainPic.Refresh();

            #region Debug Logger            
            int nLC_p = _parentTriangulation.NodeList.Count;
            int eLC_p = _parentTriangulation.EdgeList.Count;
            int tLC_p = _parentTriangulation.TriangleList.Count;
            DebugLog("Info", $"MR_prev: nodeListCount: {nLC_p}; edgeListCount: {eLC_p}; triangleListCount: {tLC_p}; identity (n-e+t=1): {nLC_p - eLC_p + tLC_p == 1}.");
            int nLC = _triangulation.NodeList.Count;
            int eLC = _triangulation.EdgeList.Count;
            int tLC = _triangulation.TriangleList.Count;
            DebugLog("Info", $"MR_cur:  nodeListCount: {nLC}; edgeListCount: {eLC}; triangleListCount: {tLC}; identity (n-e+t=1): {nLC - eLC + tLC == 1}.");
            #endregion
        }
        #endregion

        #region Approximation
        /// <summary>
        /// Вычисление обратной функции T_j^{-1},
        /// используя метод алгебраических дополнений, где        / 1  x_1^j  y_1^j \
        ///                                                T_j = |  1  x_2^j  y_2^j  |
        ///                                                       \ 1  x_3^j  y_3^j /
        /// </summary>
        /// <param name="firstNode">Первый узел треугольника</param>
        /// <param name="secondNode">Второй узел треугольника</param>
        /// <param name="thirdNode">Третий узел треугольника</param>
        /// <returns></returns>
        private static double[,] InverseTJFunction(Node firstNode, Node secondNode, Node thirdNode) {
            double[,] inverseTJ = new double[3, 3];

            double x1 = firstNode.XCoordinate;
            double y1 = firstNode.YCoordinate;

            double x2 = secondNode.XCoordinate;
            double y2 = secondNode.YCoordinate;

            double x3 = thirdNode.XCoordinate;
            double y3 = thirdNode.YCoordinate;

            double det = x2 * y3 + x1 * y2 + y1 * x3 - y1 * x2 - y2 * x3 - x1 * y3;

            inverseTJ[0, 0] = (x2 * y3 - y2 * x3) / det;
            inverseTJ[0, 1] = (y1 * x3 - x1 * y3) / det;
            inverseTJ[0, 2] = (x1 * y2 - y1 * x2) / det;

            inverseTJ[1, 0] = (y2 - y3) / det;
            inverseTJ[1, 1] = (y3 - y1) / det;
            inverseTJ[1, 2] = (y1 - y2) / det;

            inverseTJ[2, 0] = (x3 - x2) / det;
            inverseTJ[2, 1] = (x1 - x3) / det;
            inverseTJ[2, 2] = (x2 - x1) / det;

            return inverseTJ;
        }
        /// <summary>
        /// Вычисление функции веса w_j: w_j(A,x,y) = 1/ ( (x-x_0^j)^2 + (y-y_0^j)^2 ),
        /// где p=2,
        ///     (x,y) - текущая точка,
        ///     (x_0^j,y_0^j) - геометрический центр треугольника T_j
        /// </summary>
        /// <param name="curXCoord">Координата x текущей точки</param>
        /// <param name="curYCoord">Координата y текущей точки</param>
        /// <param name="geoCenterXCoord">Координата x геометрического центра треугольника</param>
        /// <param name="geoCenterYCoord">Координата y геометрического центра треугольника</param>
        /// <returns></returns>
        private static double WJFunction(double curXCoord, double curYCoord, double geoCenterXCoord, double geoCenterYCoord) {
            return 1 / ((curXCoord - geoCenterXCoord) * (curXCoord - geoCenterXCoord) + (curYCoord - geoCenterYCoord) * (curYCoord - geoCenterYCoord));
        }
        /// <summary>
        /// Вычисление аппроксимирующей функции G_j(A,x,y) в пределах треугольника T_j:         
        ///                                                        / z_1^j \
        ///                 G_j(A,x,y) = ( 1  x  y ) * T_j^{-1} * |  z_2^j  | ,
        ///                                                        \ z_3^j /
        /// где (x,y) - текущая точка,
        ///     (x_k^j  y_k^j)_{k=1}^3 - узлы треугольника T_j,
        ///     (z_k^j)_{k=1}^3 - экспериментальные значения функции f(x,y) в данных узлах
        /// </summary>
        /// <param name="data">Массив значений ( 1  x  y )</param>
        /// <param name="firstNode">Первый узел треугольника</param>
        /// <param name="secondNode">Второй узел треугольника</param>
        /// <param name="thirdNode">Третий узел треугольника</param>
        /// <param name="functionValues">Экспериментальные значения функции f(x,y)</param>
        /// <returns></returns>
        private static double GJFunction(double[] data, Node firstNode, Node secondNode, Node thirdNode, double[] functionValues) {
            double result = 0;
            double[] tmp = new double[3];
            double[,] inverseTJ = InverseTJFunction(firstNode, secondNode, thirdNode);

            for (int j = 0; j < 3; j++) {
                tmp[j] = 0;
                for (int k = 0; k < 3; k++) {
                    tmp[j] += data[k] * inverseTJ[k, j];
                }
            }
            for (int j = 0; j < 3; j++) {
                result += tmp[j] * functionValues[j];
            }
            return result;
        }
        /// <summary>
        /// Вычисление аппроксимирующей функции G(A,x,y)
        /// </summary>
        /// <param name="curXCoord">Координата x текущей точки</param>
        /// <param name="curYCoord">Координата y текущей точки</param>
        /// <param name="triangleList">Список треугольников</param>
        /// <returns></returns>
        private static double GFunction(double curXCoord, double curYCoord, List<Triangle> triangleList) {
            double w_sum = 0;
            double result = 0;
            double[] data = new double[] { 1, curXCoord, curYCoord };

            for (int j = 0; j < triangleList.Count; j++) {
                double w_j = WJFunction(curXCoord, curYCoord, triangleList[j].GeometricCenter.XCoordinate, triangleList[j].GeometricCenter.YCoordinate);

                double[] functionValues = new double[] {
                    ExactSolution(triangleList[j].FirstNode.XCoordinate,triangleList[j].FirstNode.YCoordinate),
                    ExactSolution(triangleList[j].SecondNode.XCoordinate,triangleList[j].SecondNode.YCoordinate),
                    ExactSolution(triangleList[j].ThirdNode.XCoordinate,triangleList[j].ThirdNode.YCoordinate)
                };
                w_sum += w_j;
                result += w_j * GJFunction(data, triangleList[j].FirstNode, triangleList[j].SecondNode, triangleList[j].ThirdNode, functionValues);
            }
            return result /= w_sum;
        }
        #endregion

        #region Interpolation
        /// <summary>
        /// Построение интерполяций функции f(x,y) и аппроксимирующих функций 
        ///     G(A,x,y) - для текущей триангуляции, 
        ///     G_{prev}(A,x,y) - для parent триангуляции Делоне (если была построена триангуляция методом измельчения)
        /// </summary>
        private void Interpolation() {
            int xAxisNum = (int)numericUpDown_xAxisNum.Value;
            int yAxisNum = (int)numericUpDown_yAxisNum.Value;

            double hX = (_xAxisEnd - _xAxisStart) / (xAxisNum - 1);
            double hY = (_yAxisEnd - _yAxisStart) / (yAxisNum - 1);

            double[] xValues = new double[xAxisNum * yAxisNum];
            double[] yValues = new double[xAxisNum * yAxisNum];

            double[] exactValues = new double[xAxisNum * yAxisNum];
            double[] zValues_prev = new double[xAxisNum * yAxisNum];
            double[] zValues = new double[xAxisNum * yAxisNum];

            double tmpApproxError;
            double maxApproxError = 0;

            int index = 0;
            string info = string.Empty;

            if (_applicationState == ApplicationState.DelaunayTriangulation) {
                for (int j = 0; j < xAxisNum; j++) {
                    for (int k = 0; k < yAxisNum; k++) {
                        xValues[index] = _xAxisStart + j * hX;
                        yValues[index] = _yAxisStart + k * hY;

                        exactValues[index] = ExactSolution(xValues[index], yValues[index]);
                        zValues[index] = GFunction(xValues[index], yValues[index], _triangulation.TriangleList);

                        tmpApproxError = Math.Abs(exactValues[index] - zValues[index]);

                        if (tmpApproxError > maxApproxError) {
                            maxApproxError = tmpApproxError;
                        }
                        index++;
                    }
                }
                info = $"[{_xAxisStart};{_xAxisEnd}]x[{_yAxisStart};{_yAxisEnd}]: {nameof(hX)}={hX}; {nameof(hY)}={hY}\n{nameof(maxApproxError)} = {maxApproxError}";

                if (_gnuplotPath != null) {
                    Thread thread = new Thread(() => PlotFunctionGraphs(xValues, yValues, exactValues, zValues, _gnuplotPath, checkBox_combinedGraphs.Checked));
                    thread.Start();
                }

                #region Debug Logger
                DebugLog("Info", $"GnuPlot DT: {info}.");
                #endregion
            }

            if (_applicationState == ApplicationState.MeshRefinement) {
                double tmpApproxError_prev;
                double maxApproxError_prev = 0;

                for (int j = 0; j < xAxisNum; j++) {
                    for (int k = 0; k < yAxisNum; k++) {
                        xValues[index] = _xAxisStart + j * hX;
                        yValues[index] = _yAxisStart + k * hY;

                        exactValues[index] = ExactSolution(xValues[index], yValues[index]);
                        zValues_prev[index] = GFunction(xValues[index], yValues[index], _parentTriangulation.TriangleList);
                        zValues[index] = GFunction(xValues[index], yValues[index], _triangulation.TriangleList);

                        tmpApproxError_prev = Math.Abs(exactValues[index] - zValues_prev[index]);
                        if (tmpApproxError_prev > maxApproxError_prev) {
                            maxApproxError_prev = tmpApproxError_prev;
                        }

                        tmpApproxError = Math.Abs(exactValues[index] - zValues[index]);
                        if (tmpApproxError > maxApproxError) {
                            maxApproxError = tmpApproxError;
                        }
                        index++;
                    }
                }
                info = $"[{_xAxisStart};{_xAxisEnd}]x[{_yAxisStart};{_yAxisEnd}]: {nameof(hX)}={hX}; {nameof(hY)}={hY}\n" +
                       $"{nameof(maxApproxError_prev)} = {maxApproxError_prev}\n{nameof(maxApproxError)} = {maxApproxError}";

                if (_gnuplotPath != null) {
                    Thread thread = new Thread(() => PlotFunctionGraphs(xValues, yValues, exactValues, zValues_prev, zValues, _gnuplotPath, checkBox_combinedGraphs.Checked));
                    thread.Start();
                }

                #region Debug Logger
                DebugLog("Info", $"GnuPlot MR: {info}.");
                #endregion
            }

            MessageBox.Show($"Результаты аппроксимации функции f(x,y):\n{info}.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        /// <summary>
        /// Построение графиков функции f(x,y) и аппроксимирующей функции G(A,x,y) с использованием GnuPlot
        /// </summary>
        /// <param name="xValues">Массив значений x</param>
        /// <param name="yValues">Массив значений y</param>
        /// <param name="exactValues">Массив значений f(x,y)</param>
        /// <param name="zValues">Массив значений z</param>
        /// <param name="gnuplotPath">Путь к приложению GnuPlot</param>
        /// <param name="combinedGraphs">Построение графиков в одном окне GnuPlot</param>
        private void PlotFunctionGraphs(double[] xValues, double[] yValues, double[] exactValues, double[] zValues, string gnuplotPath, bool combinedGraphs) {
            GnuPlot gnuplot = new GnuPlot(gnuplotPath);
            SetGnuPlotParams(ref gnuplot);
            gnuplot.SPlot(xValues, yValues, exactValues, "title \"f(x,y)\" lc rgb \"purple\"");

            if (combinedGraphs) {
                gnuplot.SPlot(xValues, yValues, zValues, "title \"G(A,x,y)\" lc rgb \"#76c5f5\"");
            } else {
                GnuPlot gnuplot2 = new GnuPlot(gnuplotPath);
                SetGnuPlotParams(ref gnuplot2);
                gnuplot2.SPlot(xValues, yValues, zValues, "title \"G(A,x,y)\" lc rgb \"#76c5f5\"");
            }
        }
        /// <summary>
        /// Построение графиков функции f(x,y) и аппроксимирующих функций G(A,x,y), G_{prev}(A,x,y) с использованием GnuPlot
        /// </summary>
        /// <param name="xValues">Массив значений x</param>
        /// <param name="yValues">Массив значений y</param>
        /// <param name="exactValues">Массив значений f(x,y)</param>
        /// <param name="zValues_prev">Массив значений z_prev</param>
        /// <param name="zValues">Массив значений z</param>
        /// <param name="gnuplotPath">Путь к приложению GnuPlot</param>
        /// <param name="combinedGraphs">Построение графиков в одном окне GnuPlot</param>
        private void PlotFunctionGraphs(double[] xValues, double[] yValues, double[] exactValues, double[] zValues_prev, double[] zValues, string gnuplotPath, bool combinedGraphs) {
            GnuPlot gnuplot = new GnuPlot(gnuplotPath);
            SetGnuPlotParams(ref gnuplot);
            gnuplot.SPlot(xValues, yValues, exactValues, "title \"f(x,y)\" lc rgb \"purple\"");

            if (combinedGraphs) {
                gnuplot.SPlot(xValues, yValues, zValues_prev, "title \"G_p_r_e_v(A,x,y)\" lc rgb \"#fb8585\"");
                gnuplot.SPlot(xValues, yValues, zValues, "title \"G(A,x,y)\" lc rgb \"#76c5f5\"");
            } else {
                GnuPlot gnuplot2 = new GnuPlot(gnuplotPath);
                SetGnuPlotParams(ref gnuplot2);
                gnuplot2.SPlot(xValues, yValues, zValues_prev, "title \"G_p_r_e_v(A,x,y)\" lc rgb \"#fb8585\"");

                GnuPlot gnuplot3 = new GnuPlot(gnuplotPath);
                SetGnuPlotParams(ref gnuplot3);
                gnuplot3.SPlot(xValues, yValues, zValues, "title \"G(A,x,y)\" lc rgb \"#76c5f5\"");
            }
        }
        /// <summary>
        /// Настройка параметров для построения графика
        /// </summary>
        /// <param name="gnuplot">Экземпляр GnuPlot</param>
        private void SetGnuPlotParams(ref GnuPlot gnuplot) {
            gnuplot.Set("dgrid3d 40,40 gauss .75", "pm3d");
            gnuplot.Set("title \"Interpolation\"", "xlabel \"X-Axis\"", "ylabel \"Y-Axis\"", "zlabel \"Z-Axis\"");
            gnuplot.Set("palette defined ( 0 \"blue\", 3 \"green\", 6 \"yellow\", 10 \"red\" )");
            gnuplot.Set($"xrange[{_xAxisStart.ToString().Replace(',', '.')}:{_xAxisEnd.ToString().Replace(',', '.')}]");
            gnuplot.Set($"yrange[{_yAxisStart.ToString().Replace(',', '.')}:{_yAxisEnd.ToString().Replace(',', '.')}]");
        }
        #endregion

        #endregion

        #region Test & Debug
        /// <summary>
        /// Логгер
        /// </summary>
        /// <param name="type">Тип сообщения</param>
        /// <param name="message">Сообщение</param>
        internal static void DebugLog(string type, string message) {
            File.AppendAllText("debug.log", $"{DateTime.Now} {type}: {message}\n");
        }
        #endregion
    }
}