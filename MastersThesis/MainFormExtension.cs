using static MastersThesis.Triangulation.MeshStore;

namespace MastersThesis {
    partial class MainForm {

        /*
         * ToDo: рефакторинг 
         *                  не обработано: 
         *                              регион Global Variables в классе MainForm
         *                              регион Methods          в классе PlanarObjectStore
         *                              функция ExactSolution
         * 
         *                  обработка ворнингов "w3: rename region" в классе и подклассах PlanarObjectStore
         */


        #region Global Variables
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private enum TriangulationType : int {
            NodeGeneration = 0,
            GreedyTriangulation = 1,
            DelaunayTriangulation = 2,
            MeshRefinement = 3
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private TriangulationType _triangulationType = TriangulationType.NodeGeneration;
        /// <summary>
        /// Размеры используемой области в pictureBox_mainPic
        /// </summary>
        private SizeF _canvasSize;
        /// <summary>
        /// Точка начала координат в pictureBox_mainPic
        /// </summary>
        private PointF _canvasOrgin;
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private int _counterInstance;
        /// <summary>
        /// Коэффициент измельчения
        /// </summary>
        private int _meshRefinementCoefficient = 3;
        /// <summary>
        /// 
        /// </summary>
        private readonly int _decimalPlaces = 2;
        private readonly int _timerInterval = 1000;
        private PlanarObjectStore _planarObjectStoreInstance;
        private List<Node2DStore> _nodeStoreList;
        private List<Edge2DStore> _edgeStoreList;
        private List<Triangle2DStore> _triangleStoreList;
        #endregion

        private double ExactSolution(double x, double y) {
            return Math.Sin(x) * y + x * Math.Cos(y) + x + y;
        }


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
            internal void DrawPicture(ref Graphics graphics, bool labelVisibility) {
                //Graphics gr = graphics;
                //_nodeList.ForEach(obj => obj.DrawNode(ref gr, labelVisibility));
                for (int j = 0; j < _nodeList.Count; j++) {
                    _nodeList[j].DrawNode(ref graphics, labelVisibility);
                }
            }
            internal void DrawPicture(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int counterInstance) {
                Graphics gr = graphics;
                if (tweenAnimation) {
                    if (counterInstance >= 0 && _animationList.Count != 0) {
                        //_animationList[counterInstance].EdgeList.ForEach(obj => obj.DrawEdge(ref gr));
                        for (int j = 0; j < _animationList[counterInstance].EdgeList.Count; j++) {
                            _animationList[counterInstance].EdgeList[j].DrawEdge(ref gr);
                        }
                    }
                } else {
                    //_edgeList.ForEach(obj => obj.DrawEdge(ref gr));
                    for (int j = 0; j < _edgeList.Count; j++) {
                        _edgeList[j].DrawEdge(ref gr);
                    }
                }
                //_nodeList.ForEach(obj => obj.DrawNode(ref gr, labelVisibility));
                for (int j = 0; j < _nodeList.Count; j++) {
                    _nodeList[j].DrawNode(ref gr, labelVisibility);
                }
            }
            internal void DrawPicture(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int counterInstance, bool meshVisibility) {
                Graphics gr = graphics;
                if (tweenAnimation) {
                    if (counterInstance >= 0 && _animationList.Count != 0) {
                        //_animationList[counterInstance].EdgeList.ForEach(obj => obj.DrawEdge(ref gr));
                        for (int j = 0; j < _animationList[counterInstance].EdgeList.Count; j++) {
                            _animationList[counterInstance].EdgeList[j].DrawEdge(ref gr);
                        }
                        //_animationList[counterInstance].TriangleList.ForEach(obj => obj.DrawTriangle(ref gr, labelVisibility, meshVisibility));
                        for (int j = 0; j < _animationList[counterInstance].TriangleList.Count; j++) {
                            _animationList[counterInstance].TriangleList[j].DrawTriangle(ref gr, labelVisibility, meshVisibility);
                        }
                    }
                } else {
                    //_edgeList.ForEach(obj => obj.DrawEdge(ref gr));
                    for (int j = 0; j < _edgeList.Count; j++) {
                        _edgeList[j].DrawEdge(ref gr);
                    }
                    //_triangleList.ForEach(obj => obj.DrawTriangle(ref gr, labelVisibility, meshVisibility));
                    for (int j = 0; j < _triangleList.Count; j++) {
                        _triangleList[j].DrawTriangle(ref gr, labelVisibility, meshVisibility);
                    }
                }
                //_nodeList.ForEach(obj => obj.DrawNode(ref gr, labelVisibility));
                for (int j = 0; j < _nodeList.Count; j++) {
                    _nodeList[j].DrawNode(ref gr, labelVisibility);
                }
            }
            internal void DrawPicture(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int counterInstance, bool meshVisibility, bool circumcircleVisibility) {
                Graphics gr = graphics;
                if (tweenAnimation) {
                    if (counterInstance >= 0 && _animationList.Count != 0) {
                        //_animationList[counterInstance].EdgeList.ForEach(obj => obj.DrawEdge(ref gr));
                        for (int j = 0; j < _animationList[counterInstance].EdgeList.Count; j++) {
                            _animationList[counterInstance].EdgeList[j].DrawEdge(ref gr);
                        }
                        //_animationList[counterInstance].TriangleList.ForEach(obj => obj.DrawTriangleAndCircumcircle(ref gr, labelVisibility, meshVisibility, circumcircleVisibility));
                        for (int j = 0; j < _animationList[counterInstance].TriangleList.Count; j++) {
                            _animationList[counterInstance].TriangleList[j].DrawTriangleAndCircumcircle(ref gr, labelVisibility, meshVisibility, circumcircleVisibility);
                        }
                    }
                } else {
                    //_edgeList.ForEach(obj => obj.DrawEdge(ref gr));
                    for (int j = 0; j < _edgeList.Count; j++) {
                        _edgeList[j].DrawEdge(ref gr);
                    }
                    //_triangleList.ForEach(obj => obj.DrawTriangleAndCircumcircle(ref gr, labelVisibility, meshVisibility, circumcircleVisibility));
                    for (int j = 0; j < _triangleList.Count; j++) {
                        _triangleList[j].DrawTriangleAndCircumcircle(ref gr, labelVisibility, meshVisibility, circumcircleVisibility);
                    }
                }
                //_nodeList.ForEach(obj => obj.DrawNode(ref gr, labelVisibility));
                for (int j = 0; j < _nodeList.Count; j++) {
                    _nodeList[j].DrawNode(ref gr, labelVisibility);
                }
            }
            #endregion

            #region Node, Edge, Triangle, AnimationTracker
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
                /// <param name="nodeInfoVisibility">Видимость информации об узле</param>
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
                /// <param name="triangleIDVisibility">Видимость ID треугольника</param>
                /// <param name="innerTriangleVisibility">Видимость внутреннего треугольника</param>
                internal void DrawTriangle(ref Graphics graphics, bool triangleIDVisibility, bool innerTriangleVisibility) {
                    if (innerTriangleVisibility) {
                        PointF[] curvePoints = new PointF[] { GetFirstPoint(), GetSecondPoint(), GetThirdPoint() };
                        graphics.FillPolygon(new Pen(Color.LightGreen, 1).Brush, curvePoints);
                        if (triangleIDVisibility) {
                            graphics.DrawString(_triangleID.ToString(), new Font("Cambria", 6), new Pen(Color.DeepPink, 2).Brush, _geometricCenter.GetPoint());
                        }
                    }
                }
                /// <summary>
                /// Отрисовка треугольника и описанной окружности в контроле PictureBox
                /// </summary>
                /// <param name="graphics">Экземпляр Graphics</param>
                /// <param name="triangleIDVisibility">Видимость ID треугольника</param>
                /// <param name="innerTriangleVisibility">Видимость внутреннего треугольника</param>
                /// <param name="circumcircleVisibility">Видимость описанной окружности</param>
                internal void DrawTriangleAndCircumcircle(ref Graphics graphics, bool triangleIDVisibility, bool innerTriangleVisibility, bool circumcircleVisibility) {
                    DrawTriangle(ref graphics, triangleIDVisibility, innerTriangleVisibility);
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
    }
}
