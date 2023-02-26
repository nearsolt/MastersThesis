using static MastersThesis.Triangulation.MeshStore;

namespace MastersThesis {
    partial class MainForm {
        #region Global Variables
        /// <summary>
        /// Cостояния приложения
        /// </summary>
        private enum ApplicationStateType : int {
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
            MeshRefinement = 3
        }
        /// <summary>
        /// Текущиее состояние приложения
        /// </summary>
        private ApplicationStateType _applicationState = ApplicationStateType.NodeGeneration;
        /// <summary>
        /// Размеры используемой области в pictureBox_mainPic
        /// </summary>
        private SizeF _canvasSize;
        /// <summary>
        /// Точка начала координат в pictureBox_mainPic
        /// </summary>
        private PointF _canvasOrgin;
        /// <summary>
        /// Счетчик для анимации
        /// </summary>
        private int _animationCounter;
        /// <summary>
        /// Коэффициент измельчения q
        /// </summary>
        private int _meshRefinementCoeff = 3;
        /// <summary>
        /// Количество знаков после запятой 
        /// </summary>
        private readonly int _decimalPlaces = 2;
        /// <summary>
        /// Интервал для таймера анимации (мс)
        /// </summary>
        private readonly int _timerInterval = 1000;
        /// <summary>
        /// Экземпляр PlanarObjectStore
        /// </summary>
        private PlanarObjectStore _planarObjectStore = null!;
        /// <summary>
        /// Список узлов (NodeStore)
        /// </summary>
        private List<NodeStore> _nodeStoreList = null!;
        /// <summary>
        /// Список ребер (EdgeStore)
        /// </summary>
        private List<EdgeStore> _edgeStoreList = null!;
        /// <summary>
        /// Список треугольников (TriangleStore)
        /// </summary>
        private List<TriangleStore> _triangleStoreList = null!;
        #endregion

        #region Exact Solution
        private double ExactSolution(double x, double y) {
            return Math.Sin(x) * y + x * Math.Cos(y) + x + y;
        }
        #endregion

        #region Class PlanarObjectStore
        internal class PlanarObjectStore {
            #region Private Class Variables
            /// <summary>
            /// Список узлов
            /// </summary>
            private List<Node> _nodeList;
            /// <summary>
            /// Список ребер
            /// </summary>
            private List<Edge> _edgeList;
            /// <summary>
            /// Список треугольников
            /// </summary>
            private List<Triangle> _triangleList;
            /// <summary>
            /// Список элементов анимации
            /// </summary>
            private List<TweenAnimation> _animationList;
            #endregion

#warning w3: rename region
            #region ctor & set/get
            /// <summary>
            /// Конструктор
            /// </summary>
            internal PlanarObjectStore() {
                _nodeList = new List<Node>();
                _edgeList = new List<Edge>();
                _triangleList = new List<Triangle>();
                _animationList = new List<TweenAnimation>();
            }

            /// <summary>
            /// Список узлов
            /// </summary>
            internal List<Node> NodeList {
                get { return _nodeList; }
                set { _nodeList = value; }
            }
            /// <summary>
            /// Список ребер
            /// </summary>
            internal List<Edge> EdgeList {
                get { return _edgeList; }
                set { _edgeList = value; }
            }
            /// <summary>
            /// Список треугольников
            /// </summary>
            internal List<Triangle> TriangleList {
                get { return _triangleList; }
                set { _triangleList = value; }
            }
            /// <summary>
            /// Список элементов анимации
            /// </summary>
            internal List<TweenAnimation> AnimationList {
                get { return _animationList; }
                set { _animationList = value; }
            }
            #endregion

            #region Methods
            /// <summary>
            /// Отрисовка узлов в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="nodeInfoVisibility">Отображение информации об узлах</param>
            internal void DrawNodeList(ref Graphics graphics, bool nodeInfoVisibility) {
                for (int j = 0; j < _nodeList.Count; j++) {
                    _nodeList[j].DrawNode(ref graphics, nodeInfoVisibility);
                }
            }
            /// <summary>
            /// Отрисовка ребер в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            private void DrawEdgeList(ref Graphics graphics) {
                for (int j = 0; j < _edgeList.Count; j++) {
                    _edgeList[j].DrawEdge(ref graphics);
                }
            }
            /// <summary>
            /// Анимированная отрисовка ребер в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            private void DrawEdgeListWithAnimation(ref Graphics graphics, ref int animationCounter) {
                for (int j = 0; j < _animationList[animationCounter].EdgeList.Count; j++) {
                    _animationList[animationCounter].EdgeList[j].DrawEdge(ref graphics);
                }
            }
            /// <summary>
            /// Отрисовка внутренних треугольников в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="triangleIDVisibility">Отображение ID треугольников</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            private void DrawInnerTriangles(ref Graphics graphics, bool triangleIDVisibility, bool innerTriangleVisibility) {
                for (int j = 0; j < _triangleList.Count; j++) {
                    _triangleList[j].DrawInnerTriangle(ref graphics, triangleIDVisibility, innerTriangleVisibility);
                }
            }
            /// <summary>
            /// Анимированная отрисовка внутренних треугольников в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            /// <param name="triangleIDVisibility">Отображение ID треугольников</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            private void DrawInnerTrianglesWithAnimation(ref Graphics graphics, ref int animationCounter, bool triangleIDVisibility, bool innerTriangleVisibility) {
                for (int j = 0; j < _animationList[animationCounter].TriangleList.Count; j++) {
                    _animationList[animationCounter].TriangleList[j].DrawInnerTriangle(ref graphics, triangleIDVisibility, innerTriangleVisibility);
                }
            }
            /// <summary>
            /// Отрисовка внутренних треугольников  и описанных окружностей в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="triangleIDVisibility">Отображение ID треугольников</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            /// <param name="circumcircleVisibility">Отображение описанных окружностей</param>
            private void DrawInnerTrianglesAndCircumcircles(ref Graphics graphics, bool triangleIDVisibility, bool innerTriangleVisibility, bool circumcircleVisibility) {
                for (int j = 0; j < _triangleList.Count; j++) {
                    _triangleList[j].DrawInnerTriangle(ref graphics, triangleIDVisibility, innerTriangleVisibility);
                    _triangleList[j].DrawCircumcircle(ref graphics, circumcircleVisibility);
                }
            }
            /// <summary>
            /// Анимированная отрисовка внутренних треугольников и описанных окружностей в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            /// <param name="triangleIDVisibility">Отображение ID треугольников</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            /// <param name="circumcircleVisibility">Отображение описанных окружностей</param>
            private void DrawInnerTrianglesAndCircumcirclesWithAnimation(ref Graphics graphics, ref int animationCounter, bool triangleIDVisibility, bool innerTriangleVisibility, bool circumcircleVisibility) {
                for (int j = 0; j < _animationList[animationCounter].TriangleList.Count; j++) {
                    _animationList[animationCounter].TriangleList[j].DrawInnerTriangle(ref graphics, triangleIDVisibility, innerTriangleVisibility);
                    _animationList[animationCounter].TriangleList[j].DrawCircumcircle(ref graphics, circumcircleVisibility);
                }
            }
            /// <summary>
            /// Анимированная отрисовка триангуляции (без отображения треугольников и описанных окружностей) в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="nodeInfoVisibility">Отображение информации об узлах</param>
            /// <param name="tweenAnimation">Отображение анимации</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            internal void DrawTriangulation(ref Graphics graphics, bool nodeInfoVisibility, bool tweenAnimation, ref int animationCounter) {
                if (tweenAnimation) {
                    if (animationCounter >= 0 && _animationList.Count != 0) {
                        DrawEdgeListWithAnimation(ref graphics, ref animationCounter);
                    }
                } else {
                    DrawEdgeList(ref graphics);
                }
                DrawNodeList(ref graphics, nodeInfoVisibility);
            }
            /// <summary>
            /// Анимированная отрисовка триангуляции (без отображения описанных окружностей) в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="labelVisibility">Отображение информации об узлах и треугольниках</param>
            /// <param name="tweenAnimation">Отображение анимации</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            internal void DrawTriangulation(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int animationCounter, bool innerTriangleVisibility) {
                if (tweenAnimation) {
                    if (animationCounter >= 0 && _animationList.Count != 0) {
                        DrawEdgeListWithAnimation(ref graphics, ref animationCounter);
                        DrawInnerTrianglesWithAnimation(ref graphics, ref animationCounter, labelVisibility, innerTriangleVisibility);
                    }
                } else {
                    DrawEdgeList(ref graphics);
                    DrawInnerTriangles(ref graphics, labelVisibility, innerTriangleVisibility);
                }
                DrawNodeList(ref graphics, labelVisibility);
            }
            /// <summary>
            /// Анимированная отрисовка триангуляции в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="labelVisibility">Отображение информации об узлах и треугольниках</param>
            /// <param name="tweenAnimation">Отображение анимации</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            /// <param name="circumcircleVisibility">Отображение описанных окружностей</param>
            internal void DrawTriangulation(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int animationCounter, bool innerTriangleVisibility, bool circumcircleVisibility) {
                if (tweenAnimation) {
                    if (animationCounter >= 0 && _animationList.Count != 0) {
                        DrawEdgeListWithAnimation(ref graphics, ref animationCounter);
                        DrawInnerTrianglesAndCircumcirclesWithAnimation(ref graphics, ref animationCounter, labelVisibility, innerTriangleVisibility, circumcircleVisibility);
                    }
                } else {
                    DrawEdgeList(ref graphics);
                    DrawInnerTrianglesAndCircumcircles(ref graphics, labelVisibility, innerTriangleVisibility, circumcircleVisibility);
                }
                DrawNodeList(ref graphics, labelVisibility);
            }
            #endregion

            #region Base Classes: Node, Edge, Triangle, AnimationTracker
            internal class Node {
                #region Private Class Variables
                /// <summary>
                /// ID узла
                /// </summary>
                private int _nodeID;
                /// <summary>
                /// Координата x
                /// </summary>
                private double _xCoordinate;
                /// <summary>
                /// Координата y
                /// </summary>
                private double _yCoordinate;
                #endregion
#warning w3: rename region
                #region ctor & set/get
                /// <summary>
                /// Конструктор
                /// </summary>
                /// <param name="nodeID">ID узла</param>
                /// <param name="xCoordinate">Координата x</param>
                /// <param name="yCoordinate">Координата y</param>
                internal Node(int nodeID, double xCoordinate, double yCoordinate) {
                    _nodeID = nodeID;
                    _xCoordinate = xCoordinate;
                    _yCoordinate = yCoordinate;
                }

                /// <summary>
                /// ID узла
                /// </summary>
                internal int NodeID { get { return _nodeID; } }
                /// <summary>
                /// Координата x
                /// </summary>
                internal double XCoordinate { get { return _xCoordinate; } }
                /// <summary>
                /// Координата y
                /// </summary>
                internal double YCoordinate { get { return _yCoordinate; } }
                #endregion

                #region Methods
                /// <summary>
                /// Проверка двух узлов на равенство координат
                /// </summary>
                /// <param name="other">Другой узел</param>
                /// <returns></returns>
                internal bool Equals(Node other) {
                    return _xCoordinate == other.XCoordinate && _yCoordinate == other.YCoordinate;
                }
                /// <summary>
                /// Получение точки (координаты узла в формате PointF)
                /// </summary>
                /// <returns></returns>
                internal PointF GetPoint() {
                    return new PointF((float)_xCoordinate, -(float)_yCoordinate);
                }
                /// <summary>
                /// Получение точки (координаты узла в формате PointF) со сдвигом относительно толщины
                /// </summary>
                /// <param name="halfThickness">Половина толщины точки</param>
                /// <returns></returns>
                private PointF GetPointForEllipse(int halfThickness) {
                    return new PointF((float)_xCoordinate - halfThickness, -(float)_yCoordinate - halfThickness);
                }
                /// <summary>
                /// Отрисовка узла в контроле PictureBox
                /// </summary>
                /// <param name="graphics">Экземпляр Graphics</param>
                /// <param name="nodeInfoVisibility">Отображение информации об узле</param>
                internal void DrawNode(ref Graphics graphics, bool nodeInfoVisibility) {
                    int halfThickness = 2;
                    graphics.FillEllipse(new Pen(Color.BlueViolet, halfThickness).Brush, new RectangleF(GetPointForEllipse(halfThickness), new SizeF(2 * halfThickness, 2 * halfThickness)));
                    if (nodeInfoVisibility) {
                        string nodeInfo = $"{_nodeID} ({_xCoordinate}; {_yCoordinate})";
                        SizeF strSize = graphics.MeasureString(nodeInfo, new Font("Cambria", 6));
                        graphics.DrawString(nodeInfo, new Font("Cambria", 6), new Pen(Color.DarkBlue, halfThickness).Brush,
                                            GetPointForEllipse(halfThickness).X + 3 - strSize.Width / 2, GetPointForEllipse(halfThickness).Y + 3 + strSize.Height / 2);
                    }
                }
                #endregion
            }

            internal class Edge {
                #region Private Class Variables
                /// <summary>
                /// ID ребра
                /// </summary>
                private int _edgeID;
                /// <summary>
                /// Первый узел ребра
                /// </summary>
                private Node _firstNode;
                /// <summary>
                /// Второй узел ребра
                /// </summary>
                private Node _secondNode;
                #endregion
#warning w3: rename region
                #region ctor & set/get
                /// <summary>
                /// Конструктор
                /// </summary>
                /// <param name="edgeID">ID ребра</param>
                /// <param name="firstNode">Первый узел ребра</param>
                /// <param name="secondNode">Второй узел ребра</param>
                internal Edge(int edgeID, Node firstNode, Node secondNode) {
                    _edgeID = edgeID;
                    _firstNode = firstNode;
                    _secondNode = secondNode;
                }

                /// <summary>
                /// ID ребра
                /// </summary>
                internal int EdgeID { get { return _edgeID; } }
                /// <summary>
                /// Первый узел ребра
                /// </summary>
                internal Node FirstNode { get { return _firstNode; } }
                /// <summary>
                /// Второй узел ребра
                /// </summary>
                internal Node SecondNode { get { return _secondNode; } }
                #endregion

                #region Methods
                /// <summary>
                /// Проверка двух ребер на равенство, сравнивая соответствующие узлы
                /// </summary>
                /// <param name="other">Другое ребро</param>
                /// <returns></returns>
                internal bool Equals(Edge other) {
                    return _firstNode.Equals(other.FirstNode) && _secondNode.Equals(other.SecondNode);
                }
                /// <summary>
                /// Проверка двух ребер на равенство, сравнивая узлы
                /// </summary>
                /// <param name="other">Другое ребро</param>
                /// <returns></returns>
                internal bool CommutativeEquals(Edge other) {
                    return (_firstNode.Equals(other.FirstNode) && _secondNode.Equals(other.SecondNode)) ||
                           (_firstNode.Equals(other.SecondNode) && _secondNode.Equals(other.FirstNode));
                }
                /// <summary>
                /// Отрисовка ребра в контроле PictureBox
                /// </summary>
                /// <param name="graphics">Экземпляр Graphics</param>
                internal void DrawEdge(ref Graphics graphics) {
                    graphics.DrawLine(new Pen(Color.DarkOrange, 1), _firstNode.GetPoint(), _secondNode.GetPoint());
                }
                #endregion
            }

            internal class Triangle {
                #region Private Class Variables
                /// <summary>
                /// ID треугольника
                /// </summary>
                private int _triangleID;
                /// <summary>
                /// Первый узел треугольника
                /// </summary>
                private Node _firstNode;
                /// <summary>
                /// Второй узел треугольника
                /// </summary>
                private Node _secondNode;
                /// <summary>
                /// Третий узел треугольника
                /// </summary>
                private Node _thirdNode;
                /// <summary>
                /// Геометрический центр треугольника
                /// </summary>
                private Node _geometricCenter;
                /// <summary>
                /// Коэффициент растяжения для отрисовки внутреннего треугольника
                /// </summary>
                private readonly double _scalingCoeff = 0.6;
                /// <summary>
                /// Радиус описанной окружности
                /// </summary>
                private double _circumcircleRadius;
                /// <summary>
                /// Центр описанной окружности
                /// </summary>
                private Node _circumcircleCenter = null!;
                /// <summary>
                /// Вспомогательный узел для отрисовки описанной окружности
                /// </summary>
                private Node _nodeForEllipse = null!;
                #endregion

#warning w3: rename region
                #region ctor & set/get
                /// <summary>
                /// Конструктор
                /// </summary>
                /// <param name="triangleID">ID треугольника</param>
                /// <param name="firstNode">Первый узел треугольника</param>
                /// <param name="secondNode">Второй узел треугольника</param>
                /// <param name="thirdNode">Третий узел треугольника</param>
                internal Triangle(int triangleID, Node firstNode, Node secondNode, Node thirdNode) {
                    _triangleID = triangleID;
                    _firstNode = firstNode;
                    _secondNode = secondNode;
                    _thirdNode = thirdNode;
                    _geometricCenter = new Node(-1, (firstNode.XCoordinate + secondNode.XCoordinate + thirdNode.XCoordinate) / 3,
                                                    (firstNode.YCoordinate + secondNode.YCoordinate + thirdNode.YCoordinate) / 3);
                    DefineCircumcircle();
                }

                /// <summary>
                /// ID треугольника
                /// </summary>
                internal int TriangleID { get { return _triangleID; } }
                /// <summary>
                /// Первый узел треугольника
                /// </summary>
                internal Node FirstNode { get { return _firstNode; } }
                /// <summary>
                /// Второй узел треугольника
                /// </summary>
                internal Node SecondNode { get { return _secondNode; } }
                /// <summary>
                /// Третий узел треугольника
                /// </summary>
                internal Node ThirdNode { get { return _thirdNode; } }
                /// <summary>
                /// Геометрический центр треугольника
                /// </summary>
                internal Node GeometricCenter { get { return _geometricCenter; } }
                #endregion

                #region Methods
                /// <summary>
                /// Получение первой точки (координаты первого узла в формате PointF) со сдвигом относительно коэффициента растяжения _scalingCoeff
                /// </summary>
                /// <returns></returns>
                private PointF GetFirstPoint() {
                    return new PointF((float)(_geometricCenter.GetPoint().X * (1 - _scalingCoeff) + _firstNode.GetPoint().X * _scalingCoeff),
                                      (float)(_geometricCenter.GetPoint().Y * (1 - _scalingCoeff) + _firstNode.GetPoint().Y * _scalingCoeff));
                }
                /// <summary>
                /// Получение второй точки (координаты второго узла в формате PointF) со сдвигом относительно коэффициента растяжения _scalingCoeff
                /// </summary>
                /// <returns></returns>
                private PointF GetSecondPoint() {
                    return new PointF((float)(_geometricCenter.GetPoint().X * (1 - _scalingCoeff) + _secondNode.GetPoint().X * _scalingCoeff),
                                      (float)(_geometricCenter.GetPoint().Y * (1 - _scalingCoeff) + _secondNode.GetPoint().Y * _scalingCoeff));
                }
                /// <summary>
                /// Получение третьей точки (координаты третьего узла в формате PointF) со сдвигом относительно коэффициента растяжения _scalingCoeff
                /// </summary>
                /// <returns></returns>
                private PointF GetThirdPoint() {
                    return new PointF((float)(_geometricCenter.GetPoint().X * (1 - _scalingCoeff) + _thirdNode.GetPoint().X * _scalingCoeff),
                                      (float)(_geometricCenter.GetPoint().Y * (1 - _scalingCoeff) + _thirdNode.GetPoint().Y * _scalingCoeff));
                }
                /// <summary>
                /// Определение элементов для описанной окружности (нахождение центра, радиуса и вспомогательного узла _nodeForEllipse)
                /// </summary>
                private void DefineCircumcircle() {
                    double dA = _firstNode.XCoordinate * _firstNode.XCoordinate + _firstNode.YCoordinate * _firstNode.YCoordinate;
                    double dB = _secondNode.XCoordinate * _secondNode.XCoordinate + _secondNode.YCoordinate * _secondNode.YCoordinate;
                    double dC = _thirdNode.XCoordinate * _thirdNode.XCoordinate + _thirdNode.YCoordinate * _thirdNode.YCoordinate;

                    double aux1 = dA * (_thirdNode.YCoordinate - _secondNode.YCoordinate) + dB * (_firstNode.YCoordinate - _thirdNode.YCoordinate) +
                                  dC * (_secondNode.YCoordinate - _firstNode.YCoordinate);

                    double aux2 = -(dA * (_thirdNode.XCoordinate - _secondNode.XCoordinate) + dB * (_firstNode.XCoordinate - _thirdNode.XCoordinate) +
                                  dC * (_secondNode.XCoordinate - _firstNode.XCoordinate));

                    double div = 2 * (_firstNode.XCoordinate * (_thirdNode.YCoordinate - _secondNode.YCoordinate) + _secondNode.XCoordinate *
                                 (_firstNode.YCoordinate - _thirdNode.YCoordinate) + _thirdNode.XCoordinate * (_secondNode.YCoordinate - _firstNode.YCoordinate));

                    double xCoord = aux1 / div;
                    double yCoord = aux2 / div;

                    _circumcircleCenter = new Node(-1, xCoord, yCoord);
                    _circumcircleRadius = Math.Sqrt((xCoord - _firstNode.XCoordinate) * (xCoord - _firstNode.XCoordinate) + (yCoord - _firstNode.YCoordinate) * (yCoord - _firstNode.YCoordinate));
                    _nodeForEllipse = new Node(-1, xCoord - _circumcircleRadius, yCoord + _circumcircleRadius);
                }
                /// <summary>
                /// Отрисовка треугольника в контроле PictureBox
                /// </summary>
                /// <param name="graphics">Экземпляр Graphics</param>
                /// <param name="triangleIDVisibility">Отображение ID треугольника</param>
                /// <param name="innerTriangleVisibility">Отображение внутреннего треугольника</param>
                internal void DrawInnerTriangle(ref Graphics graphics, bool triangleIDVisibility, bool innerTriangleVisibility) {
                    if (innerTriangleVisibility) {
                        PointF[] curvePoints = new PointF[] { GetFirstPoint(), GetSecondPoint(), GetThirdPoint() };
                        graphics.FillPolygon(new Pen(Color.LightGreen, 1).Brush, curvePoints);
                        if (triangleIDVisibility) {
                            graphics.DrawString(_triangleID.ToString(), new Font("Cambria", 6), new Pen(Color.DeepPink, 2).Brush, _geometricCenter.GetPoint());
                        }
                    }
                }
                /// <summary>
                /// Отрисовка описанной окружности в контроле PictureBox
                /// </summary>
                /// <param name="graphics">Экземпляр Graphics</param>
                /// <param name="circumcircleVisibility">Отображение описанной окружности</param>
                internal void DrawCircumcircle(ref Graphics graphics, bool circumcircleVisibility) {
                    if (circumcircleVisibility) {
                        graphics.DrawEllipse(new Pen(Color.LightGreen, 1), _nodeForEllipse.GetPoint().X, _nodeForEllipse.GetPoint().Y, 
                                             (float)_circumcircleRadius * 2, (float)_circumcircleRadius * 2);
                    }
                }
                #endregion
            }

            internal class TweenAnimation {
                #region Private Class Variables
                /// <summary>
                /// Список ребер
                /// </summary>
                private List<Edge> _edgeList = null!;
                /// <summary>
                /// Список треугольников
                /// </summary>
                private List<Triangle> _triangleList = null!;
                #endregion

#warning w3: rename region
                #region set/get
                /// <summary>
                /// Список ребер
                /// </summary>
                internal List<Edge> EdgeList {
                    get { return _edgeList; }
                    set { _edgeList = value; }
                }
                /// <summary>
                /// Список треугольников
                /// </summary>
                internal List<Triangle> TriangleList {
                    get { return _triangleList; }
                    set { _triangleList = value; }
                }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}
