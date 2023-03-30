namespace MastersThesis {
    partial class MainForm {

        #region Class PlanarObjectStore & Base Classes (Node, Edge, Triangle, AnimationTracker)
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

            #region Constructor & Properties
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
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            internal void DrawNodeList(ref Graphics graphics, bool nodeInfoVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                for (int j = 0; j < _nodeList.Count; j++) {
                    _nodeList[j].DrawNode(ref graphics, nodeInfoVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                }
            }
            /// <summary>
            /// Отрисовка ребер в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            private void DrawEdgeList(ref Graphics graphics, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                for (int j = 0; j < _edgeList.Count; j++) {
                    _edgeList[j].DrawEdge(ref graphics, xCoordShiftCoeff, yCoordShiftCoeff);
                }
            }
            /// <summary>
            /// Анимированная отрисовка ребер в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            private void DrawEdgeListWithAnimation(ref Graphics graphics, ref int animationCounter, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                for (int j = 0; j < _animationList[animationCounter].EdgeList.Count; j++) {
                    _animationList[animationCounter].EdgeList[j].DrawEdge(ref graphics, xCoordShiftCoeff, yCoordShiftCoeff);
                }
            }
            /// <summary>
            /// Отрисовка внутренних треугольников в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="triangleIDVisibility">Отображение ID треугольников</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            private void DrawInnerTriangles(ref Graphics graphics, bool triangleIDVisibility, bool innerTriangleVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                for (int j = 0; j < _triangleList.Count; j++) {
                    _triangleList[j].DrawInnerTriangle(ref graphics, triangleIDVisibility, innerTriangleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                }
            }
            /// <summary>
            /// Анимированная отрисовка внутренних треугольников в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            /// <param name="triangleIDVisibility">Отображение ID треугольников</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            private void DrawInnerTrianglesWithAnimation(ref Graphics graphics, ref int animationCounter, bool triangleIDVisibility, bool innerTriangleVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                for (int j = 0; j < _animationList[animationCounter].TriangleList.Count; j++) {
                    _animationList[animationCounter].TriangleList[j].DrawInnerTriangle(ref graphics, triangleIDVisibility, innerTriangleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                }
            }
            /// <summary>
            /// Отрисовка внутренних треугольников и описанных окружностей в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="triangleIDVisibility">Отображение ID треугольников</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            /// <param name="circumcircleVisibility">Отображение описанных окружностей</param>
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            private void DrawInnerTrianglesAndCircumcircles(ref Graphics graphics, bool triangleIDVisibility, bool innerTriangleVisibility, bool circumcircleVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                for (int j = 0; j < _triangleList.Count; j++) {
                    _triangleList[j].DrawInnerTriangle(ref graphics, triangleIDVisibility, innerTriangleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                    _triangleList[j].DrawCircumcircle(ref graphics, circumcircleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
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
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            private void DrawInnerTrianglesAndCircumcirclesWithAnimation(ref Graphics graphics, ref int animationCounter, bool triangleIDVisibility, bool innerTriangleVisibility, bool circumcircleVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                for (int j = 0; j < _animationList[animationCounter].TriangleList.Count; j++) {
                    _animationList[animationCounter].TriangleList[j].DrawInnerTriangle(ref graphics, triangleIDVisibility, innerTriangleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                    _animationList[animationCounter].TriangleList[j].DrawCircumcircle(ref graphics, circumcircleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                }
            }
            /// <summary>
            /// Анимированная отрисовка триангуляции (без отображения треугольников и описанных окружностей) в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="nodeInfoVisibility">Отображение информации об узлах</param>
            /// <param name="tweenAnimation">Отображение анимации</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            internal void DrawTriangulation(ref Graphics graphics, bool nodeInfoVisibility, bool tweenAnimation, ref int animationCounter, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                if (tweenAnimation) {
                    if (animationCounter >= 0 && _animationList.Count != 0) {
                        DrawEdgeListWithAnimation(ref graphics, ref animationCounter, xCoordShiftCoeff, yCoordShiftCoeff);
                    }
                } else {
                    DrawEdgeList(ref graphics, xCoordShiftCoeff, yCoordShiftCoeff);
                }
                DrawNodeList(ref graphics, nodeInfoVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
            }
            /// <summary>
            /// Анимированная отрисовка триангуляции (без отображения описанных окружностей) в контроле PictureBox
            /// </summary>
            /// <param name="graphics">Экземпляр Graphics</param>
            /// <param name="labelVisibility">Отображение информации об узлах и треугольниках</param>
            /// <param name="tweenAnimation">Отображение анимации</param>
            /// <param name="animationCounter">Счетчик для анимации</param>
            /// <param name="innerTriangleVisibility">Отображение внутренних треугольников</param>
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            internal void DrawTriangulation(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int animationCounter, bool innerTriangleVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                if (tweenAnimation) {
                    if (animationCounter >= 0 && _animationList.Count != 0) {
                        DrawEdgeListWithAnimation(ref graphics, ref animationCounter, xCoordShiftCoeff, yCoordShiftCoeff);
                        DrawInnerTrianglesWithAnimation(ref graphics, ref animationCounter, labelVisibility, innerTriangleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                    }
                } else {
                    DrawEdgeList(ref graphics, xCoordShiftCoeff, yCoordShiftCoeff);
                    DrawInnerTriangles(ref graphics, labelVisibility, innerTriangleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                }
                DrawNodeList(ref graphics, labelVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
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
            /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
            /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
            internal void DrawTriangulation(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int animationCounter, bool innerTriangleVisibility, bool circumcircleVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                if (tweenAnimation) {
                    if (animationCounter >= 0 && _animationList.Count != 0) {
                        DrawEdgeListWithAnimation(ref graphics, ref animationCounter, xCoordShiftCoeff, yCoordShiftCoeff);
                        DrawInnerTrianglesAndCircumcirclesWithAnimation(ref graphics, ref animationCounter, labelVisibility, innerTriangleVisibility, circumcircleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                    }
                } else {
                    DrawEdgeList(ref graphics, xCoordShiftCoeff, yCoordShiftCoeff);
                    DrawInnerTrianglesAndCircumcircles(ref graphics, labelVisibility, innerTriangleVisibility, circumcircleVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
                }
                DrawNodeList(ref graphics, labelVisibility, xCoordShiftCoeff, yCoordShiftCoeff);
            }
            #endregion

            #region Base Classes: Node, Edge, Triangle, AnimationTracker
            internal class Node {

                #region Private Class Variables
                /// <summary>
                /// ID узла
                /// </summary>
                private readonly int _nodeID;
                /// <summary>
                /// Координата x
                /// </summary>
                private readonly double _xCoordinate;
                /// <summary>
                /// Координата y
                /// </summary>
                private readonly double _yCoordinate;
                #endregion

                #region Constructor & Properties
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
                /// Получение координат узла (PointF)
                /// </summary>
                /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
                /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
                /// <returns></returns>
                internal PointF GetPoint(double xCoordShiftCoeff, double yCoordShiftCoeff) {
                    return new PointF((float)(_xCoordinate * xCoordShiftCoeff), -(float)(_yCoordinate * yCoordShiftCoeff));
                }
                /// <summary>
                /// Получение координат узла (PointF) со сдвигом относительно толщины
                /// </summary>
                /// <param name="halfThickness">Половина толщины точки</param>
                /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
                /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
                /// <returns></returns>
                private PointF GetPointForEllipse(float halfThickness, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                    return new PointF((float)(_xCoordinate * xCoordShiftCoeff) - halfThickness, -(float)(_yCoordinate * yCoordShiftCoeff) - halfThickness);
                }
                /// <summary>
                /// Отрисовка узла в контроле PictureBox
                /// </summary>
                /// <param name="graphics">Экземпляр Graphics</param>
                /// <param name="nodeInfoVisibility">Отображение информации об узле</param>
                /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
                /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
                internal void DrawNode(ref Graphics graphics, bool nodeInfoVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                    float halfThickness = 2.5f;
                    graphics.FillEllipse(new Pen(Color.BlueViolet, halfThickness).Brush, new RectangleF(GetPointForEllipse(halfThickness, xCoordShiftCoeff, yCoordShiftCoeff),
                                         new SizeF(2 * halfThickness, 2 * halfThickness)));
                    if (nodeInfoVisibility) {
                        string nodeInfo = $"{_nodeID} ({_xCoordinate}; {_yCoordinate})";
                        SizeF strSize = graphics.MeasureString(nodeInfo, new Font("Cambria", 7));
                        graphics.DrawString(nodeInfo, new Font("Cambria", 7), new Pen(Color.DarkBlue, halfThickness).Brush,
                                            GetPointForEllipse(halfThickness, xCoordShiftCoeff, yCoordShiftCoeff).X + 3 - strSize.Width / 2,
                                            GetPointForEllipse(halfThickness, xCoordShiftCoeff, yCoordShiftCoeff).Y + 3 + strSize.Height / 2);
                    }
                }
                #endregion
            }

            internal class Edge {

                #region Private Class Variables
                /// <summary>
                /// ID ребра
                /// </summary>
                private readonly int _edgeID;
                /// <summary>
                /// Первый узел ребра
                /// </summary>
                private readonly Node _firstNode;
                /// <summary>
                /// Второй узел ребра
                /// </summary>
                private readonly Node _secondNode;
                #endregion

                #region Constructor & Properties
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
                    return (_firstNode.Equals(other.FirstNode) && _secondNode.Equals(other.SecondNode)) || (_firstNode.Equals(other.SecondNode) && _secondNode.Equals(other.FirstNode));
                }
                /// <summary>
                /// Отрисовка ребра в контроле PictureBox
                /// </summary>
                /// <param name="graphics">Экземпляр Graphics</param>
                /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
                /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
                internal void DrawEdge(ref Graphics graphics, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                    graphics.DrawLine(new Pen(Color.DarkOrange, 1.5f), _firstNode.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff), _secondNode.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff));
                }
                #endregion
            }

            internal class Triangle {

                #region Private Class Variables
                /// <summary>
                /// ID треугольника
                /// </summary>
                private readonly int _triangleID;
                /// <summary>
                /// Первый узел треугольника
                /// </summary>
                private readonly Node _firstNode;
                /// <summary>
                /// Второй узел треугольника
                /// </summary>
                private readonly Node _secondNode;
                /// <summary>
                /// Третий узел треугольника
                /// </summary>
                private readonly Node _thirdNode;
                /// <summary>
                /// Геометрический центр треугольника
                /// </summary>
                private readonly Node _geometricCenter;
                /// <summary>
                /// Коэффициент растяжения для отрисовки внутреннего треугольника
                /// </summary>
                private readonly double _scalingCoeff = 0.6;
                /// <summary>
                /// Радиус описанной окружности
                /// </summary>
                private double _circumcircleRadius;
                /// <summary>
                /// Вспомогательный узел для отрисовки описанной окружности
                /// </summary>
                private Node _nodeForEllipse = null!;
                #endregion

                #region Constructor & Properties
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
                /// Получение координат первого узла (PointF) со сдвигом относительно коэффициента растяжения _scalingCoeff
                /// </summary>
                /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
                /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
                /// <returns></returns>
                private PointF GetFirstPoint(double xCoordShiftCoeff, double yCoordShiftCoeff) {
                    return new PointF((float)(_geometricCenter.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).X * (1 - _scalingCoeff) + _firstNode.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).X * _scalingCoeff),
                                      (float)(_geometricCenter.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).Y * (1 - _scalingCoeff) + _firstNode.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).Y * _scalingCoeff));
                }
                /// <summary>
                /// Получение координат второго узла (PointF) со сдвигом относительно коэффициента растяжения _scalingCoeff
                /// </summary>
                /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
                /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
                /// <returns></returns>
                private PointF GetSecondPoint(double xCoordShiftCoeff, double yCoordShiftCoeff) {
                    return new PointF((float)(_geometricCenter.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).X * (1 - _scalingCoeff) + _secondNode.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).X * _scalingCoeff),
                                      (float)(_geometricCenter.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).Y * (1 - _scalingCoeff) + _secondNode.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).Y * _scalingCoeff));
                }
                /// <summary>
                /// Получение координат третьего узла (PointF) со сдвигом относительно коэффициента растяжения _scalingCoeff
                /// </summary>
                /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
                /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
                /// <returns></returns>
                private PointF GetThirdPoint(double xCoordShiftCoeff, double yCoordShiftCoeff) {
                    return new PointF((float)(_geometricCenter.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).X * (1 - _scalingCoeff) + _thirdNode.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).X * _scalingCoeff),
                                      (float)(_geometricCenter.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).Y * (1 - _scalingCoeff) + _thirdNode.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).Y * _scalingCoeff));
                }
                /// <summary>
                /// Определение элементов для описанной окружности: центр, радиус и вспомогательный узел _nodeForEllipse
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

                    _circumcircleRadius = Math.Sqrt((xCoord - _firstNode.XCoordinate) * (xCoord - _firstNode.XCoordinate) + (yCoord - _firstNode.YCoordinate) * (yCoord - _firstNode.YCoordinate));
                    _nodeForEllipse = new Node(-1, xCoord - _circumcircleRadius, yCoord + _circumcircleRadius);
                }
                /// <summary>
                /// Отрисовка треугольника в контроле PictureBox
                /// </summary>
                /// <param name="graphics">Экземпляр Graphics</param>
                /// <param name="triangleIDVisibility">Отображение ID треугольника</param>
                /// <param name="innerTriangleVisibility">Отображение внутреннего треугольника</param>
                /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
                /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
                internal void DrawInnerTriangle(ref Graphics graphics, bool triangleIDVisibility, bool innerTriangleVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                    if (innerTriangleVisibility) {
                        PointF[] curvePoints = new PointF[] { GetFirstPoint(xCoordShiftCoeff, yCoordShiftCoeff), GetSecondPoint(xCoordShiftCoeff, yCoordShiftCoeff), GetThirdPoint(xCoordShiftCoeff, yCoordShiftCoeff) };
                        graphics.FillPolygon(new Pen(Color.LightGreen, 1).Brush, curvePoints);
                        if (triangleIDVisibility) {
                            graphics.DrawString(_triangleID.ToString(), new Font("Cambria", 7), new Pen(Color.DeepPink, 2.5f).Brush, _geometricCenter.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff));
                        }
                    }
                }
                /// <summary>
                /// Отрисовка описанной окружности в контроле PictureBox
                /// </summary>
                /// <param name="graphics">Экземпляр Graphics</param>
                /// <param name="circumcircleVisibility">Отображение описанной окружности</param>
                /// <param name="xCoordShiftCoeff">Коэффициент сдвига координаты x в PictureBox</param>
                /// <param name="yCoordShiftCoeff">Коэффициент сдвига координаты y в PictureBox</param>
                internal void DrawCircumcircle(ref Graphics graphics, bool circumcircleVisibility, double xCoordShiftCoeff, double yCoordShiftCoeff) {
                    if (circumcircleVisibility) {
                        graphics.DrawEllipse(new Pen(Color.LightGreen, 1.5f), _nodeForEllipse.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).X, _nodeForEllipse.GetPoint(xCoordShiftCoeff, yCoordShiftCoeff).Y,
                                             (float)(_circumcircleRadius * xCoordShiftCoeff * 2), (float)(_circumcircleRadius * yCoordShiftCoeff * 2));
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

                #region Properties
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