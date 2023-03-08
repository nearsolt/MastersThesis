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
        /// Коэффициэнт растяжения для ширины в pictureBox_mainPic
        /// </summary>
        private double _widthScalingCoeff;
        /// <summary>
        /// Коэффициэнт растяжения для высоты в pictureBox_mainPic
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
        #endregion

        #region Exact Solution
        private double ExactSolution(double x, double y) {
            //return Math.Sin(x) * y + x * Math.Cos(y) + x + y;
            //return 0.1 * (Math.Pow(x, 2) - Math.Pow(y - 10, 2)) - 2 * x * Math.Cos(y) - Math.Sin(y);

            return Math.Cos(x) * Math.Sin(0.5 * y) * x + y;
        }
        #endregion

        #region Constructor & Event Handlers
        public MainForm() {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            GenerateRandomNodes();
        }
        private void timer_animationTimer_Tick(object sender, EventArgs e) {
            if (_animationCounter == _triangulation.AnimationList.Count - 1) {
                _animationCounter = 0;
            } else {
                _animationCounter++;
            }
            pictureBox_mainPic.Refresh();
        }
        private void pictureBox_mainPic_Paint(object sender, PaintEventArgs e) {
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
        private void checkBox_labelVisibility_CheckedChanged(object sender, EventArgs e) {
            pictureBox_mainPic.Refresh();
        }
        private void checkBox_circumcircleVisibility_CheckedChanged(object sender, EventArgs e) {
            pictureBox_mainPic.Refresh();
        }
        private void checkBox_tweenAnimation_CheckedChanged(object sender, EventArgs e) {
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
        private void checkBox_innerTriangleVisibility_CheckedChanged(object sender, EventArgs e) {
            pictureBox_mainPic.Refresh();
        }
        #endregion

        #region Buttons
        private void button_generateNodes_Click(object sender, EventArgs e) {
            checkBox_tweenAnimation.Checked = false;
            checkBox_innerTriangleVisibility.Checked = false;
            checkBox_innerTriangleVisibility.Enabled = true;
            checkBox_circumcircleVisibility.Checked = false;
            checkBox_circumcircleVisibility.Enabled = true;
            GenerateRandomNodes();
        }
        private void button_greedyTriangulation_Click(object sender, EventArgs e) {
            if (_applicationState == ApplicationState.MeshRefinement) {
                MessageBox.Show("Построение жадной триангуляции недоступно после применения метода измельчения. Сгенерируйте множество узлов еще раз.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            checkBox_innerTriangleVisibility.Checked = false;
            checkBox_innerTriangleVisibility.Enabled = false;
            checkBox_circumcircleVisibility.Checked = false;
            checkBox_circumcircleVisibility.Enabled = false;
            GreedyTriangulation();
        }
        private void button_delaunayTriangulation_Click(object sender, EventArgs e) {
            if (_applicationState == ApplicationState.MeshRefinement) {
                MessageBox.Show("Построение триангуляции Делоне недоступно после применения метода измельчения. Сгенерируйте множество узлов еще раз.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            checkBox_innerTriangleVisibility.Enabled = true;
            checkBox_circumcircleVisibility.Enabled = true;
            DelaunayTriangulation();
        }
        private void button_meshRefinement_Click(object sender, EventArgs e) {
            if (_applicationState != ApplicationState.DelaunayTriangulation) {
                MessageBox.Show("Построение триангуляции методом измельчения доступно только после триангуляции Делоне.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            checkBox_circumcircleVisibility.Checked = false;
            checkBox_circumcircleVisibility.Enabled = false;
            MeshRefinement();
        }
        private void button_interpolation_Click(object sender, EventArgs e) {
            if (_applicationState != ApplicationState.DelaunayTriangulation && _applicationState != ApplicationState.MeshRefinement) {
                MessageBox.Show("Построение интерполяции доступно только после триангуляции Делоне или применения метода измельчения.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            button_interpolation.Enabled = false;
            Interpolation();
            button_interpolation.Enabled = true;
        }
        #endregion

        #endregion

        #region Methods

        #region Initialization & Triangulation
        /// <summary>
        /// Инициализация области определения и коэффициэнтов растяжения ширины и высоты в pictureBox_mainPic
        /// </summary>
        /// <param name="isValidated">Проверка на корректность инициализации области</param>
        private void InitializeDomainOfDefinition(out bool isValidated) {
            if (checkBox_setDomainOfDefinition.Checked) {
                _xAxisStart = (double)numericUpDown_xAxisStart.Value;
                _xAxisEnd = (double)numericUpDown_xAxisEnd.Value;

                _yAxisStart = (double)numericUpDown_yAxisStart.Value;
                _yAxisEnd = (double)numericUpDown_yAxisEnd.Value;

                if (_xAxisStart >= _xAxisEnd) {
                    MessageBox.Show($"При инициализации области были введены некорректные днные: xAxisStart >= xAxisEnd ({_xAxisStart} >= {_xAxisEnd}).", "Information",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    isValidated = false;
                    return;
                }
                if (_yAxisStart >= _yAxisEnd) {
                    MessageBox.Show($"При инициализации области были введены некорректные днные: yAxisStart >= yAxisEnd ({_yAxisStart} >= {_yAxisEnd}).", "Information",
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
        /// Random-генерация узлов в используемой области
        /// </summary>
        private void GenerateRandomNodes() {
            bool isValidated = false;
            InitializeCanvasSize(out isValidated);
            if (!isValidated) {
                return;
            }
            _applicationState = ApplicationState.NodeGeneration;
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
            _animationCounter = 0;

            _triangulation.EdgeList.Clear();
            _triangulation.AnimationList.Clear();

            List<Edge> outputEdgeList = new List<Edge>();
            List<TweenAnimation> outputAnimationList = new List<TweenAnimation>();

            List<NodeStore> inputNodeList = new List<NodeStore>();

            for (int j = 0; j < _triangulation.NodeList.Count; j++) {
                inputNodeList.Add(new NodeStore(j, _triangulation.NodeList[j].XCoordinate, _triangulation.NodeList[j].YCoordinate, _triangulation.NodeList[j]));
            }
            MeshStore meshStore = new MeshStore(inputNodeList, ref outputEdgeList, ref outputAnimationList);

            _triangulation.EdgeList = outputEdgeList;
            _triangulation.AnimationList = outputAnimationList;
            pictureBox_mainPic.Refresh();

            #region Debug
#warning w1: remove logger
            int nLC = _triangulation.NodeList.Count;
            int eLC = _triangulation.EdgeList.Count;
            int tLC = _triangulation.TriangleList.Count;
            Debug.WriteLine($"GT:\tnodeListCount: {nLC}\tedgeListCount: {eLC}\ttriangleListCount: {tLC}\tidentity (n-e+t=1): {nLC - eLC + tLC == 1}");
            #endregion
        }
        /// <summary>
        /// Построение триангуляции Делоне
        /// </summary>
        private void DelaunayTriangulation() {
            _applicationState = ApplicationState.DelaunayTriangulation;
            _animationCounter = 0;

            _triangulation.EdgeList.Clear();
            _triangulation.AnimationList.Clear();

            List<Node> sortedNodeList = _triangulation.NodeList.OrderBy(obj => obj.XCoordinate).ThenBy(obj => obj.YCoordinate).ToList();
            List<Edge> outputEdgeList = new List<Edge>();
            List<Triangle> outputTriangleList = new List<Triangle>();
            List<TweenAnimation> outputAnimationList = new List<TweenAnimation>();

            List<NodeStore> inputNodeList = new List<NodeStore>();

            for (int j = 0; j < sortedNodeList.Count; j++) {
                inputNodeList.Add(new NodeStore(j, sortedNodeList[j].XCoordinate, sortedNodeList[j].YCoordinate, sortedNodeList[j]));
            }
            MeshStore meshStore = new MeshStore(inputNodeList, ref outputEdgeList, ref outputTriangleList, ref outputAnimationList,
                                                ref _nodeStoreList, ref _edgeStoreList, ref _triangleStoreList);

            _triangulation.EdgeList = outputEdgeList;
            _triangulation.TriangleList = outputTriangleList;
            _triangulation.AnimationList = outputAnimationList;
            pictureBox_mainPic.Refresh();

            #region Debug
#warning w1: remove logger            
            int nLC = _triangulation.NodeList.Count;
            int eLC = _triangulation.EdgeList.Count;
            int tLC = _triangulation.TriangleList.Count;
            Debug.WriteLine($"DT:\tnodeListCount: {nLC}\tedgeListCount: {eLC}\ttriangleListCount: {tLC}\tidentity (n-e+t=1): {nLC - eLC + tLC == 1}");
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

            TweenAnimation lastAnimation = _triangulation.AnimationList.Last();

            List<Node> outputNodeList = new List<Node>();
            List<Edge> outputEdgeList = new List<Edge>();
            List<Triangle> outputTriangleList = new List<Triangle>();
            List<TweenAnimation> outputAnimationList = new List<TweenAnimation>();

            MeshStore meshStore = new MeshStore(_nodeStoreList, _edgeStoreList, _triangleStoreList, lastAnimation, _decimalPlaces, _meshRefinementCoeff,
                                                ref outputNodeList, ref outputEdgeList, ref outputTriangleList, ref outputAnimationList);

            _triangulation.NodeList = outputNodeList;
            _triangulation.EdgeList = outputEdgeList;
            _triangulation.TriangleList = outputTriangleList;
            _triangulation.AnimationList = outputAnimationList;
            pictureBox_mainPic.Refresh();

            #region Debug
#warning w1: remove logger            
            int nLC_p = _parentTriangulation.NodeList.Count;
            int eLC_p = _parentTriangulation.EdgeList.Count;
            int tLC_p = _parentTriangulation.TriangleList.Count;
            Debug.WriteLine($"MR_prev:\tnodeListCount: {nLC_p}\tedgeListCount: {eLC_p}\ttriangleListCount: {tLC_p}\tidentity (n-e+t=1): {nLC_p - eLC_p + tLC_p == 1}");
            int nLC = _triangulation.NodeList.Count;
            int eLC = _triangulation.EdgeList.Count;
            int tLC = _triangulation.TriangleList.Count;
            Debug.WriteLine($"MR_cur:\t\tnodeListCount: {nLC}\tedgeListCount: {eLC}\ttriangleListCount: {tLC}\tidentity (n-e+t=1): {nLC - eLC + tLC == 1}");
            #endregion
        }
        #endregion

        #region Approximation
        /// <summary>
        /// Вычисление обратной лямбда функции \Lambda_j^{-1},
        /// используя метод алгебраических дополнений, где              / 1  x_1^j  y_1^j \
        ///                                                \Lambda_j = |  1  x_2^j  y_2^j  |
        ///                                                             \ 1  x_3^j  y_3^j /
        /// </summary>
        /// <param name="firstNode">Первый узел треугольника</param>
        /// <param name="secondNode">Второй узел треугольника</param>
        /// <param name="thirdNode">Третий узел треугольника</param>
        /// <returns></returns>
        private double[,] InverseLambdaFunction(Node firstNode, Node secondNode, Node thirdNode) {
            double[,] inverseLambda = new double[3, 3];

            double x1 = firstNode.XCoordinate;
            double y1 = firstNode.YCoordinate;

            double x2 = secondNode.XCoordinate;
            double y2 = secondNode.YCoordinate;

            double x3 = thirdNode.XCoordinate;
            double y3 = thirdNode.YCoordinate;

            double det = x2 * y3 + x1 * y2 + y1 * x3 - y1 * x2 - y2 * x3 - x1 * y3;

            inverseLambda[0, 0] = (x2 * y3 - y2 * x3) / det;
            inverseLambda[0, 1] = (y1 * x3 - x1 * y3) / det;
            inverseLambda[0, 2] = (x1 * y2 - y1 * x2) / det;

            inverseLambda[1, 0] = (y2 - y3) / det;
            inverseLambda[1, 1] = (y3 - y1) / det;
            inverseLambda[1, 2] = (y1 - y2) / det;

            inverseLambda[2, 0] = (x3 - x2) / det;
            inverseLambda[2, 1] = (x1 - x3) / det;
            inverseLambda[2, 2] = (x2 - x1) / det;

            return inverseLambda;
        }
        /// <summary>
        /// Вычисление функции веса w_j:    w_j(A,x,y) = 1/ ( (x-x_0^j)^2 + (y-y_0^j)^2 ),
        /// где p=2,
        ///     (x,y) - текущая точка,
        ///     (x_0^j,y_0^j) - геометрический центр j-го треугольника
        /// </summary>
        /// <param name="curXCoord">Координата x текущей точки</param>
        /// <param name="curYCoord">Координата y текущей точки</param>
        /// <param name="geoCenterXCoord">Координата x геометрического центра треугольника</param>
        /// <param name="geoCenterYCoord">Координата y геометрического центра треугольника</param>
        /// <returns></returns>
        private double WJFunction(double curXCoord, double curYCoord, double geoCenterXCoord, double geoCenterYCoord) {
            return 1 / ((curXCoord - geoCenterXCoord) * (curXCoord - geoCenterXCoord) + (curYCoord - geoCenterYCoord) * (curYCoord - geoCenterYCoord));
        }
        /// <summary>
        /// Вычисление аппроксимирующей функции G_j(A,x,y) в пределах j-го треугольника:         
        ///                                                              / z_1^j \
        ///                 G_j(A,x,y) = ( 1  x  y ) * \Lambda_j^{-1} * |  z_2^j  | ,
        ///                                                              \ z_3^j /
        /// где (x,y) - текущая точка,
        ///     (x_k^j  y_k^j)_{k=1}^3 - узлы j-го треугольника,
        ///     (z_k^j)_{k=1}^3 - экспериментальные значения функции f(x,y) в данных узлах
        /// </summary>
        /// <param name="data">Массив значений ( 1  x  y )</param>
        /// <param name="firstNode">Первый узел треугольника</param>
        /// <param name="secondNode">Второй узел треугольника</param>
        /// <param name="thirdNode">Третий узел треугольника</param>
        /// <param name="functionValues">Экспериментальные значения функции f(x,y)</param>
        /// <returns></returns>
        private double GJFunction(double[] data, Node firstNode, Node secondNode, Node thirdNode, double[] functionValues) {
            double result = 0;
            double[] tmp = new double[3];
            double[,] inverseLambda = InverseLambdaFunction(firstNode, secondNode, thirdNode);

            for (int j = 0; j < 3; j++) {
                tmp[j] = 0;
                for (int k = 0; k < 3; k++) {
                    tmp[j] += data[k] * inverseLambda[k, j];
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
        private double GFunction(double curXCoord, double curYCoord, List<Triangle> triangleList) {
            double result = 0;
            double w_j = 0;
            double w_sum = 0;
            double[] data = new double[] { 1, curXCoord, curYCoord };

            for (int j = 0; j < triangleList.Count; j++) {
                w_j = WJFunction(curXCoord, curYCoord, triangleList[j].GeometricCenter.XCoordinate, triangleList[j].GeometricCenter.YCoordinate);

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
        /// Построение функции f(x,y) и построение интерполяций аппроксимирующих функций 
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

            double[] approxError_prev = new double[xAxisNum * yAxisNum];
            double[] approxError = new double[xAxisNum * yAxisNum];

            int index = 0;
            string info = string.Empty;

            if (_applicationState == ApplicationState.DelaunayTriangulation) {
                for (int j = 0; j < xAxisNum; j++) {
                    for (int k = 0; k < yAxisNum; k++) {
                        xValues[index] = _xAxisStart + j * hX;
                        yValues[index] = _yAxisStart + k * hY;

                        exactValues[index] = ExactSolution(xValues[index], yValues[index]);
                        zValues[index] = GFunction(xValues[index], yValues[index], _triangulation.TriangleList);

                        approxError[index] = Math.Abs(exactValues[index] - zValues[index]);
                        index++;
                    }
                }
                info = $"[{_xAxisStart};{_xAxisEnd}]x[{_yAxisStart};{_yAxisEnd}]: {nameof(hX)}={hX}; {nameof(hY)}={hY}\nmax {nameof(approxError)} = {approxError.Max()}";

                #region Debug
#warning w1: remove logger
                DebugLog("Info", $"GnuPlot DT: {info}\n");
                #endregion
            }
            if (_applicationState == ApplicationState.MeshRefinement) {
                for (int j = 0; j < xAxisNum; j++) {
                    for (int k = 0; k < yAxisNum; k++) {
                        xValues[index] = _xAxisStart + j * hX;
                        yValues[index] = _yAxisStart + k * hY;

                        exactValues[index] = ExactSolution(xValues[index], yValues[index]);
                        zValues_prev[index] = GFunction(xValues[index], yValues[index], _parentTriangulation.TriangleList);
                        zValues[index] = GFunction(xValues[index], yValues[index], _triangulation.TriangleList);

                        approxError_prev[index] = Math.Abs(exactValues[index] - zValues_prev[index]);
                        approxError[index] = Math.Abs(exactValues[index] - zValues[index]);
                        index++;
                    }
                }
                info = $"[{_xAxisStart};{_xAxisEnd}]x[{_yAxisStart};{_yAxisEnd}]: {nameof(hX)}={hX}; {nameof(hY)}={hY}\n" +
                       $"max {nameof(approxError_prev)} = {approxError_prev.Max()}\nmax {nameof(approxError)} = {approxError.Max()}";

                #region Debug
#warning w1: remove logger
                DebugLog("Info", $"GnuPlot MR: {info}\n");
                #endregion
            }

            Thread thread = new Thread(() => PlotFunctionGraphs(xValues, yValues, exactValues, zValues_prev, zValues));
            thread.Start();

            MessageBox.Show($"Результаты построения интерполяции:\n{info}.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        /// <summary>
        /// Построение графиков функции f(x,y) и аппроксимирующих функций G(A,x,y), G_{prev}(A,x,y) с использованием GnuPlot
        /// </summary>
        /// <param name="xValues">Массив значений x</param>
        /// <param name="yValues">Массив значений y</param>
        /// <param name="exactValues">Массив значений f(x,y)</param>
        /// <param name="zValues_prev">Массив значений z_prev</param>
        /// <param name="zValues">Массив значений z</param>
        private void PlotFunctionGraphs(double[] xValues, double[] yValues, double[] exactValues, double[] zValues_prev, double[] zValues) {
            GnuPlot.Set("dgrid3d 50,50, qnorm 2");
            GnuPlot.Set("title \"Interpolation\"");
            GnuPlot.Set("xlabel \"X-Axis\"", "ylabel \"Y-Axis\"", "zlabel \"Z-Axis\"");
            GnuPlot.Set("pm3d");
            GnuPlot.Set("palette defined ( 0 \"blue\", 3 \"green\", 6 \"yellow\", 10 \"red\" )");
            GnuPlot.HoldOn();

            GnuPlot.SPlot(xValues, yValues, exactValues, "title \"f(x,y)\" lc rgb \"purple\"");

            if (_applicationState == ApplicationState.MeshRefinement) {
                GnuPlot.SPlot(xValues, yValues, zValues_prev, "title \"G_p_r_e_v(A,x,y)\" lc rgb  \"#fb8585\"");
            }

            GnuPlot.SPlot(xValues, yValues, zValues, "title \"G(A,x,y)\" lc rgb  \"#76c5f5\"");
        }
        #endregion

        #endregion

        private void checkBox_setDomainOfDefinition_CheckedChanged(object sender, EventArgs e) {

        }

        #region Test Button & Debug
#warning To do: remove from the final version with control (button_test visible = false)
        internal static void DebugLog(string type, string message) {
            File.AppendAllText(@"D:\mechmath\.Master's Thesis\logger.log", $"{DateTime.Now} {type}: {message}\n");
        }
        private void button_test_Click(object sender, EventArgs e) {

        }
        #endregion


        #region Useless
#warning Useless



        private void pictureBox_mainPic_SizeChanged(object sender, EventArgs e) {
            //InitializeCanvasSize();
        }
        private void numericUpDown_xAxisStart_ValueChanged(object sender, EventArgs e) {

        }

        private void numericUpDown_xAxisEnd_ValueChanged(object sender, EventArgs e) {

        }

        private void numericUpDown_yAxisStart_ValueChanged(object sender, EventArgs e) {

        }

        private void numericUpDown_yAxisEnd_ValueChanged(object sender, EventArgs e) {

        }
        #endregion


    }
}