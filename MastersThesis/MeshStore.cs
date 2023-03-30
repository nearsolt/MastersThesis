using static MastersThesis.MainForm;
using static MastersThesis.MainForm.PlanarObjectStore;

namespace MastersThesis {
    partial class MeshStore {

        #region Class Helper
        internal class Helper {

            #region Methods
            /// <summary>
            /// Проверка пересечения проекций ребер на определенную ось
            /// </summary>
            /// <param name="firstNodeCoordOfEdge1">Координата первого узла первого ребра</param>
            /// <param name="secondNodeCoordOfEdge1">Координата второго узла первого ребра</param>
            /// <param name="firstNodeCoordOfEdge2">Координата первого узла второго ребра</param>
            /// <param name="secondNodeCoordOfEdge2">Координата второго узла второго ребра</param>
            /// <returns></returns>
            private static bool IsProjectionsIntersection(double firstNodeCoordOfEdge1, double secondNodeCoordOfEdge1, double firstNodeCoordOfEdge2, double secondNodeCoordOfEdge2) {
                if (firstNodeCoordOfEdge1 > secondNodeCoordOfEdge1) {
                    (firstNodeCoordOfEdge1, secondNodeCoordOfEdge1) = (secondNodeCoordOfEdge1, firstNodeCoordOfEdge1);
                }
                if (firstNodeCoordOfEdge2 > secondNodeCoordOfEdge2) {
                    (firstNodeCoordOfEdge2, secondNodeCoordOfEdge2) = (secondNodeCoordOfEdge2, firstNodeCoordOfEdge2);
                }
                return Math.Max(firstNodeCoordOfEdge1, firstNodeCoordOfEdge2) <= Math.Min(secondNodeCoordOfEdge1, secondNodeCoordOfEdge2);
            }
            /// <summary>
            /// Проверка попарного пересечения проекций ребер на оси координат
            /// </summary>
            /// <param name="edge1">Первое ребро</param>
            /// <param name="edge2">Второе ребро</param>
            /// <returns></returns>
            private static bool IsPairwiseIntersection(EdgeStore edge1, EdgeStore edge2) {
                return IsProjectionsIntersection(edge1.FirstNode.XCoordinate, edge1.SecondNode.XCoordinate, edge2.FirstNode.XCoordinate, edge2.SecondNode.XCoordinate) &&
                       IsProjectionsIntersection(edge1.FirstNode.YCoordinate, edge1.SecondNode.YCoordinate, edge2.FirstNode.YCoordinate, edge2.SecondNode.YCoordinate);
            }
            /// <summary>
            /// Вычисление псевдоскалярного произведения
            /// </summary>
            /// <param name="edge1">Первое ребро</param>
            /// <param name="edge2">Второе ребро</param>
            /// <returns></returns>
            private static double PseudoscalarProduct(EdgeStore edge1, EdgeStore edge2) {
                return edge1.XCoordinateOfVector * edge2.YCoordinateOfVector - edge1.YCoordinateOfVector * edge2.XCoordinateOfVector;
            }
            /// <summary>
            /// Проверка двух ребер на наличие общего узла через псевдоскалярные произведения
            /// </summary>
            /// <param name="p1">Первое псевдоскалярное произведение первого ребра</param>
            /// <param name="p2">Второе псевдоскалярное произведение первого ребра</param>
            /// <param name="p3">Первое псевдоскалярное произведение второго ребра</param>
            /// <param name="p4">Второе псевдоскалярное произведение второго ребра</param>
            /// <returns>true: у ребер есть один общий граничный узел</returns>
            private static bool HasCommonNode(double p1, double p2, double p3, double p4) {
                return (p1 == 0 && p2 != 0 && p3 == 0 && p4 != 0) || (p1 != 0 && p2 == 0 && p3 != 0 && p4 == 0) ||
                       (p1 == 0 && p2 != 0 && p3 != 0 && p4 == 0) || (p1 != 0 && p2 == 0 && p3 == 0 && p4 != 0);
            }
            /// <summary>
            /// Проверка пересечения двух ребер
            /// </summary>
            /// <param name="edge1">Первое ребро</param>
            /// <param name="edge2">Второе ребро</param>
            /// <returns>true: ребра пересекаются</returns>
            internal static bool IsEdgesIntersect(EdgeStore edge1, EdgeStore edge2) {
                // Проверка необходимого условия пересечения отрезков:
                // если отсутствует попарное пересечение проекций отрезков на оси координат, то отрезки не пересекаются
                if (!IsPairwiseIntersection(edge1, edge2)) {
                    return false;
                }

                // Вычисляем псевдоскалярные произведения для первого ребра
                double pseudoscalar1 = PseudoscalarProduct(new EdgeStore(-2, edge1.FirstNode, edge1.SecondNode), new EdgeStore(-2, edge1.FirstNode, edge2.FirstNode));
                double pseudoscalar2 = PseudoscalarProduct(new EdgeStore(-2, edge1.FirstNode, edge1.SecondNode), new EdgeStore(-2, edge1.FirstNode, edge2.SecondNode));

                // Если псевдоскалярные произведения одного знака, то ребра не пересекаются
                if ((pseudoscalar1 > 0 && pseudoscalar2 > 0) || (pseudoscalar1 < 0 && pseudoscalar2 < 0)) {
                    return false;
                }

                // Вычисляем псевдоскалярные произведения для второго ребра
                double pseudoscalar3 = PseudoscalarProduct(new EdgeStore(-2, edge2.FirstNode, edge2.SecondNode), new EdgeStore(-2, edge2.FirstNode, edge1.FirstNode));
                double pseudoscalar4 = PseudoscalarProduct(new EdgeStore(-2, edge2.FirstNode, edge2.SecondNode), new EdgeStore(-2, edge2.FirstNode, edge1.SecondNode));

                // Если псевдоскалярные произведения одного знака, то ребра не пересекаются
                if ((pseudoscalar3 > 0 && pseudoscalar4 > 0) || (pseudoscalar3 < 0 && pseudoscalar4 < 0)) {
                    return false;
                }
                // Добавочные условия на проверку пересечения (исключение случая, когда ребра имеют общий граничный узел)
                return !HasCommonNode(pseudoscalar1, pseudoscalar2, pseudoscalar3, pseudoscalar4);
            }

            /// <summary>
            /// Получение угла между двух ребер (между вертикалью и самим ребром)
            /// </summary>
            /// <param name="vertEdge">Вертикаль-ребро</param>
            /// <param name="edge">Некоторое ребро</param>
            /// <returns></returns>
            internal static double GetAngleBetween(EdgeStore vertEdge, EdgeStore edge) {
                double xCoordOfVector1 = vertEdge.XCoordinateOfVector;
                double yCoordOfVector1 = vertEdge.YCoordinateOfVector;
                double normalize1 = Math.Sqrt(xCoordOfVector1 * xCoordOfVector1 + yCoordOfVector1 * yCoordOfVector1);
                xCoordOfVector1 /= normalize1;
                yCoordOfVector1 /= normalize1;

                double xCoordOfVector2 = edge.XCoordinateOfVector;
                double yCoordOfVector2 = edge.YCoordinateOfVector;
                double normalize2 = Math.Sqrt(xCoordOfVector2 * xCoordOfVector2 + yCoordOfVector2 * yCoordOfVector2);
                xCoordOfVector2 /= normalize2;
                yCoordOfVector2 /= normalize2;

                double sinValue = xCoordOfVector1 * yCoordOfVector2 - xCoordOfVector2 * yCoordOfVector1;
                double cosValue = xCoordOfVector1 * xCoordOfVector2 + yCoordOfVector1 * yCoordOfVector2;

                double angle = Math.Atan2(sinValue, cosValue) / Math.PI * 180;
                if (angle <= 0) {
                    angle += 360;
                }
                return angle;
            }
            /// <summary>
            /// Проверка на коллинеарность двух векторов node1node2 и node1node3
            /// </summary>
            /// <param name="node1">Первый узел</param>
            /// <param name="node2">Второй узел</param>
            /// <param name="node3">Третий узел</param>
            /// <returns></returns>
            internal static bool IsCollinear(NodeStore node1, NodeStore node2, NodeStore node3) {
                return (node2.XCoordinate - node1.XCoordinate) * (node3.YCoordinate - node1.YCoordinate) - (node2.YCoordinate - node1.YCoordinate) * (node3.XCoordinate - node1.XCoordinate) == 0;
            }
            /// <summary>
            /// Проверка лежит ли узел node левее ориентированного ребра node1node2:
            /// 
            ///          | node.X   node.Y   1 |   | node1.X - node.X  node1.Y - node.Y |
            ///    det = | node1.X  node1.Y  1 | = | node2.X - node.X  node2.Y - node.Y | > 0,                
            ///          | node2.X  node2.Y  1 |
            ///
            /// </summary>
            /// <param name="node">Проверяемый узел</param>
            /// <param name="node1">Первый узел(начало ориентированного ребра)</param>
            /// <param name="node2">Второй узел(конец ориентированного ребра)</param>
            /// <returns>true: проверяемый узел node лежит левее ориентированного ребра node1node2</returns>
            private static bool IsNodeLocatedOnLeft(NodeStore node, NodeStore node1, NodeStore node2) {
                return (node1.XCoordinate - node.XCoordinate) * (node2.YCoordinate - node.YCoordinate) - (node1.YCoordinate - node.YCoordinate) * (node2.XCoordinate - node.XCoordinate) > 0;
            }
            /// <summary>
            /// Вспомогательная функция для определения левого кандидата: 
            /// лежит ли узел node левее ориентированного ребра edge
            /// </summary>
            /// <param name="node">Узел</param>
            /// <param name="edge">Ребро</param>
            /// <returns>true: узел node лежит левее ориентированного ребра fNsN</returns>
            internal static bool LeftOf(NodeStore node, EdgeStore edge) {
                return IsNodeLocatedOnLeft(node, edge.FirstNode, edge.SecondNode);
            }
            /// <summary>
            /// Вспомогательная функция для определения правого кандидата:
            /// лежит ли узел node правее ориентированного ребра edge
            /// </summary>
            /// <param name="node">Узел</param>
            /// <param name="edge">Ребро</param>
            /// <returns>true: узел node лежит правее ориентированного ребра fNsN</returns>
            internal static bool RightOf(NodeStore node, EdgeStore edge) {
                return IsNodeLocatedOnLeft(node, edge.SecondNode, edge.FirstNode);
            }
            /// <summary>
            /// Проверка через уравнение описанной окружности: 
            /// лежит ли проверяемый узел внутри окружности, определенной тремя узлами
            /// 
            ///         | node1.X  node1.Y  node1.X^2 + node1.Y^2  1 |   | a1  a2  a3 |
            ///   det = | node2.X  node2.Y  node2.X^2 + node2.Y^2  1 | = | b1  b2  b3 | > 0,
            ///         | node3.X  node3.Y  node3.X^2 + node3.Y^2  1 |   | c1  c2  c3 |
            ///         | node4.X  node4.Y  node4.X^2 + node4.Y^2  1 |
            ///   где
            ///         a1 = node1.X - node4.X,   b1 = node2.X - node4.X,   c1 = node3.X - node4.X,
            ///         a2 = node1.Y - node4.Y,   b2 = node2.Y - node4.Y,   c2 = node3.Y - node4.Y,
            ///         a3 = a1^2 + a2^2,         b3 = b1^2 + b2^2,         c3 = c1^2 + c2^2,  
            /// 
            /// </summary>
            /// <param name="node1">Первый узел, определяющий окружность</param>
            /// <param name="node2">Второй узел, определяющий окружность</param>
            /// <param name="node3">Третий узел, определяющий окружность</param>
            /// <param name="node4">Проверяемый узел</param>
            /// <returns>true: проверяемый узел лежит внутри окружности</returns>
            internal static bool InCircle(NodeStore node1, NodeStore node2, NodeStore node3, NodeStore node4) {
                double a1 = node1.XCoordinate - node4.XCoordinate;
                double a2 = node1.YCoordinate - node4.YCoordinate;
                double a3 = a1 * a1 + a2 * a2;

                double b1 = node2.XCoordinate - node4.XCoordinate;
                double b2 = node2.YCoordinate - node4.YCoordinate;
                double b3 = b1 * b1 + b2 * b2;

                double c1 = node3.XCoordinate - node4.XCoordinate;
                double c2 = node3.YCoordinate - node4.YCoordinate;
                double c3 = c1 * c1 + c2 * c2;

                double det = a1 * b2 * c3 + a2 * b3 * c1 + a3 * b1 * c2 - a3 * b2 * c1 - a1 * b3 * c2 - a2 * b1 * c3;
                return det > 0;
            }
            #endregion
        }
        #endregion

        #region Base Classes: NodeStore, EdgeAngleComparerVertical, EdgeStore, TriangleStore
        internal class NodeStore {

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
            /// <summary>
            /// Узел (Node)
            /// </summary>
            private readonly Node _node = null!;
            /// <summary>
            /// Список смежных ребер
            /// </summary>
            private List<EdgeStore> _connectedEdgeList = new List<EdgeStore>();
            #endregion

            #region Constructors & Properties
            /// <summary>
            /// Конструктор без инициализации переменной узла (Node)
            /// </summary>
            /// <param name="nodeID">ID узла</param>
            /// <param name="xCoordinate">Координата x</param>
            /// <param name="yCoordinate">Координата y</param>
            internal NodeStore(int nodeID, double xCoordinate, double yCoordinate) {
                _nodeID = nodeID;
                _xCoordinate = xCoordinate;
                _yCoordinate = yCoordinate;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="nodeID">ID узла</param>
            /// <param name="xCoordinate">Координата x</param>
            /// <param name="yCoordinate">Координата y</param>
            /// <param name="node">Узел (Node)</param>
            internal NodeStore(int nodeID, double xCoordinate, double yCoordinate, Node node) {
                _nodeID = nodeID;
                _xCoordinate = xCoordinate;
                _yCoordinate = yCoordinate;
                _node = node;
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
            /// <summary>
            /// Узел (Node)
            /// </summary>
            internal Node Node { get { return _node; } }
            #endregion

            #region Methods
            /// <summary>
            /// Проверка двух узлов на равенство координат
            /// </summary>
            /// <param name="other">Другой узел</param>
            /// <returns></returns>
            internal bool Equals(NodeStore other) {
                return _xCoordinate == other.XCoordinate && _yCoordinate == other.YCoordinate;
            }
            /// <summary>
            /// Получение ребра из списка смежных ребер по его индексу (против часовой стрелки)
            /// CCW - Counter ClockWise
            /// </summary>
            /// <param name="index">Индекс ребра</param>
            /// <returns></returns>
            internal EdgeStore CCWVerticalEdge(int index) {
                if (index > _connectedEdgeList.Count - 1) {
                    DebugLog("FatalError", "Метод <CCWVerticalEdge(int index)>: передаваемый на вход индекс ребра выходит за границы диапозона возможных индексов для списка смежных ребер.");
                }
                return _connectedEdgeList[index];
            }
            /// <summary>
            /// Получение ребра из списка смежных ребер, которое идет следующим против часовой стрелки после некоторого ребра
            /// CCW - Counter ClockWise
            /// </summary>
            /// <param name="edge">Некоторое ребро</param>
            /// <returns></returns>
            internal EdgeStore CCWVerticalEdge(EdgeStore edge) {
                int index = _connectedEdgeList.FindIndex(obj => obj.EdgeID == edge.EdgeID);
                if (index == -1) {
                    DebugLog("FatalError", "Метод <CCWVerticalEdge(EdgeStore edge)>: передаваемое на вход ребро не найдено в списке смежных ребер.");
                }
                index++;
                if (index == _connectedEdgeList.Count) {
                    index = 0;
                }
                return CCWVerticalEdge(index);
            }
            /// <summary>
            ///  Получение ребра из списка смежных ребер по его индексу (по часовой стрелке)
            ///  CW - ClockWise
            /// </summary>
            /// <param name="index">Индекс ребра</param>
            /// <returns></returns>
            internal EdgeStore CWVerticalEdge(int index) {
                int count = _connectedEdgeList.Count - 1;
                if (index > count) {
                    DebugLog("FatalError", "Метод <CWVerticalEdge(int index)>: передаваемый на вход индекс ребра выходит за границы диапозона возможных индексов для списка смежных ребер.");
                }
                return _connectedEdgeList[count - index];
            }
            /// <summary>
            /// Получение ребра из списка смежных ребер, которое идет следующим по часовой стрелке после некоторого ребра
            /// CW - ClockWise
            /// </summary>
            /// <param name="edge">Некоторое ребро</param>
            /// <returns></returns>
            internal EdgeStore CWVerticalEdge(EdgeStore edge) {
                int nextIndex = _connectedEdgeList.FindIndex(obj => obj.EdgeID == edge.EdgeID);
                if (nextIndex == -1) {
                    DebugLog("FatalError", "Метод <CWVerticalEdge(EdgeStore edge)>: передаваемое на вход ребро не найдено в списке смежных ребер.");
                }
                nextIndex--;
                if (nextIndex == -1) {
                    nextIndex = _connectedEdgeList.Count - 1;
                }
                return CCWVerticalEdge(nextIndex);
            }
            /// <summary>
            /// Получение ориентированного ребра из текущего узла (текущий узел будет являться первым узлом ребра)
            /// </summary>
            /// <param name="edge">Некоторое ребро</param>
            /// <returns></returns>
            private EdgeStore GetOrientedEdgeFromThisNode(EdgeStore edge) {
                NodeStore node = new NodeStore(_nodeID, _xCoordinate, _yCoordinate, _node);
                return edge.FirstNode.Equals(node) ? edge : new EdgeStore(edge.EdgeID, node, edge.GetOtherNode(node));
            }
            /// <summary>
            /// Добавление ребра в список смежных ребер с применением сортировки        . <- текущий узел
            /// (сортировка ребер осуществляется против часовой стрелки)                |\ \
            ///                                                                         | \  \
            ///                                                                         |  \    \
            ///                                                                         |   \     v
            ///                                                                         |    \     . <- edge_1
            ///                                                                         v     v
            ///                                                           вертикаль ->  .      . <- edge_0
            /// </summary>
            /// <param name="edge">Добавляемое ребро</param>
            internal void AddSortedEdge(EdgeStore edge) {
                edge = GetOrientedEdgeFromThisNode(edge);
                if (_connectedEdgeList.Count == 0) {
                    _connectedEdgeList.Add(edge);
                    return;
                }
                // Если последнее ориентированное ребро из списка отсортированных смежных ребер составляет угол между вертикалью меньше,
                // чем новое ребро, то новое ребро добавляется в конец списка
                if (new EdgeAngleComparerVertical().Compare(_connectedEdgeList[_connectedEdgeList.Count - 1], edge) <= 0) {
                    _connectedEdgeList.Add(edge);
                    return;
                }
                // Если первое ориентированное ребро из списка отсортированных смежных ребер составляет угол между вертикалью больше,
                // чем новое ребро, то новое ребро добавляется в начало списка
                if (new EdgeAngleComparerVertical().Compare(_connectedEdgeList[0], edge) >= 0) {
                    _connectedEdgeList.Insert(0, edge);
                    return;
                }
                // В других случаях, используя бинарный поиск в отсотрированном массиве, находим index для вставки нового ребра
                int index = _connectedEdgeList.BinarySearch(edge, new EdgeAngleComparerVertical());
                _connectedEdgeList.Insert(index < 0 ? ~index : index, edge);
            }
            /// <summary>
            /// Удаление ребра
            /// </summary>
            /// <param name="edgeID">ID ребра</param>
            internal void RemoveEdge(int edgeID) {
                int index = _connectedEdgeList.FindIndex(obj => obj.EdgeID == edgeID);
                if (index != -1) {
                    _connectedEdgeList.RemoveAt(index);
                }
            }
            #endregion
        }

        internal class EdgeAngleComparerVertical : IComparer<EdgeStore> {

            #region Methods
            /// <summary>
            /// Сравнение углов между вертикалью и ребрами (вычисление угла против часовой стрелки):
            ///             angle1 - угол между вертикалью и ребром edge1, 
            ///             angle2 - угол между вертикалью и ребром edge2, 
            /// </summary>
            /// <param name="edge1">Первое ребро</param>
            /// <param name="edge2">Второе ребро</param>
            /// <returns></returns>
            public int Compare(EdgeStore? edge1, EdgeStore? edge2) {
                EdgeStore vertEdge1 = new EdgeStore(-1, edge1!.FirstNode, new NodeStore(-1, edge1.FirstNode.XCoordinate, edge1.FirstNode.YCoordinate - 100));
                double angle1 = Helper.GetAngleBetween(vertEdge1, edge1);

                EdgeStore vertEdge2 = new EdgeStore(-1, edge2!.FirstNode, new NodeStore(-1, edge2.FirstNode.XCoordinate, edge2.FirstNode.YCoordinate - 100));
                double angle2 = Helper.GetAngleBetween(vertEdge2, edge2);

                return angle1 < angle2 ? -1 : angle1 > angle2 ? 1 : 0;
            }
            #endregion
        }

        internal class EdgeStore {

            #region Private Class Variables
            /// <summary>
            /// ID ребра
            /// </summary>
            private readonly int _edgeID;
            /// <summary>
            /// Первый узел ребра
            /// </summary>
            private readonly NodeStore _firstNode;
            /// <summary>
            /// Второй узел ребра
            /// </summary>
            private readonly NodeStore _secondNode;
            /// <summary>
            /// ID первого смежного треугольника
            /// </summary>
            private int _firstTriangleID;
            /// <summary>
            /// ID второго смежного треугольника
            /// </summary>
            private int _secondTriangleID;
            #endregion

            #region Constructor & Properties
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="edgeID">ID ребра</param>
            /// <param name="firstNode">Первый узел ребра</param>
            /// <param name="secondNode">Второй узел ребра</param>
            internal EdgeStore(int edgeID, NodeStore firstNode, NodeStore secondNode) {
                _edgeID = edgeID;
                _firstNode = firstNode;
                _secondNode = secondNode;
                _firstTriangleID = -1;
                _secondTriangleID = -1;
            }

            /// <summary>
            /// ID ребра
            /// </summary>
            internal int EdgeID { get { return _edgeID; } }
            /// <summary>
            /// Первый узел ребра
            /// </summary>
            internal NodeStore FirstNode { get { return _firstNode; } }
            /// <summary>
            /// Второй узел ребра
            /// </summary>
            internal NodeStore SecondNode { get { return _secondNode; } }
            /// <summary>
            /// ID первого смежного треугольника
            /// </summary>
            internal int FirstTriangleID { get { return _firstTriangleID; } }
            /// <summary>
            /// ID второго смежного треугольника
            /// </summary>
            internal int SecondTriangleID { get { return _secondTriangleID; } }
            /// <summary>
            /// Координата x вектора fN sN
            /// </summary>
            internal double XCoordinateOfVector { get { return _secondNode.XCoordinate - _firstNode.XCoordinate; } }
            /// <summary>
            /// Координата y вектора fN sN
            /// </summary>
            internal double YCoordinateOfVector { get { return _secondNode.YCoordinate - _firstNode.YCoordinate; } }
            /// <summary>
            /// Длина ребра
            /// </summary>
            internal double EdgeLength {
                get {
                    return Math.Sqrt((_secondNode.XCoordinate - _firstNode.XCoordinate) * (_secondNode.XCoordinate - _firstNode.XCoordinate) +
                                     (_secondNode.YCoordinate - _firstNode.YCoordinate) * (_secondNode.YCoordinate - _firstNode.YCoordinate));
                }
            }
            #endregion

            #region Methods
            /// <summary>
            /// Добавление ID треугольника
            /// </summary>
            /// <param name="triangleID">Добавляемый ID треугольника</param>
            internal void AddTriangleID(int triangleID) {
                if (_firstTriangleID == -1 || _firstTriangleID == triangleID) {
                    _firstTriangleID = triangleID;
                } else {
                    _secondTriangleID = triangleID;
                }
            }
            /// <summary>
            /// Удаление ID треугольника
            /// </summary>
            /// <param name="triangleID">Удаляемый ID треугольника</param>
            internal void RemoveTriangleID(int triangleID) {
                if (_firstTriangleID == triangleID) {
                    _firstTriangleID = -1;
                }
                if (_secondTriangleID == triangleID) {
                    _secondTriangleID = -1;
                }
            }
            /// <summary>
            /// Проверка двух ребер на равенство, сравнивая соответствующие узлы
            /// </summary>
            /// <param name="other">Другое ребро</param>
            /// <returns></returns>
            internal bool Equals(EdgeStore other) {
                return _firstNode.Equals(other.FirstNode) && _secondNode.Equals(other.SecondNode);
            }
            /// <summary>
            /// Проверка двух ребер на равенство, сравнивая узлы
            /// </summary>
            /// <param name="other">Другое ребро</param>
            /// <returns></returns>
            internal bool CommutativeEquals(EdgeStore other) {
                return (_firstNode.Equals(other.FirstNode) && _secondNode.Equals(other.SecondNode)) || (_firstNode.Equals(other.SecondNode) && _secondNode.Equals(other.FirstNode));
            }
            /// <summary>
            /// Получение другого узла ребра
            /// </summary>
            /// <param name="node">Некоторый узел ребра</param>
            /// <returns></returns>
            internal NodeStore GetOtherNode(NodeStore node) {
                return node.Equals(_firstNode) ? _secondNode : _firstNode;
            }
            #endregion
        }

        internal class TriangleStore {

            #region Private Class Variables
            /// <summary>
            /// ID треугольника
            /// </summary>
            private readonly int _triangleID;
            /// <summary>
            /// Первый узел треугольника
            /// </summary>
            private readonly NodeStore _firstNode;
            /// <summary>
            /// Второй узел треугольника
            /// </summary>
            private readonly NodeStore _secondNode;
            /// <summary>
            /// Третий узел треугольника
            /// </summary>
            private readonly NodeStore _thirdNode;
            /// <summary>
            /// Первое ребро треугольника
            /// </summary>
            private EdgeStore _firstEdge = null!;
            /// <summary>
            /// Второе ребро треугольника
            /// </summary>
            private EdgeStore _secondEdge = null!;
            /// <summary>
            /// Третье ребро треугольника
            /// </summary>
            private EdgeStore _thirdEdge = null!;
            #endregion

            #region Constructor & Properties
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="triangleID">ID треугольника</param>
            /// <param name="firstNode">Первый узел треугольника</param>
            /// <param name="secondNode">Второй узел треугольника</param>
            /// <param name="thirdNode">Третий узел треугольника</param>
            internal TriangleStore(int triangleID, NodeStore firstNode, NodeStore secondNode, NodeStore thirdNode) {
                _triangleID = triangleID;
                _firstNode = firstNode;
                _secondNode = secondNode;
                _thirdNode = thirdNode;
            }

            /// <summary>
            /// ID треугольника
            /// </summary>
            internal int TriangleID { get { return _triangleID; } }
            /// <summary>
            /// Первый узел треугольника
            /// </summary>
            internal NodeStore FirstNode { get { return _firstNode; } }
            /// <summary>
            /// Второй узел треугольника
            /// </summary>
            internal NodeStore SecondNode { get { return _secondNode; } }
            /// <summary>
            /// Третий узел треугольника
            /// </summary>
            internal NodeStore ThirdNode { get { return _thirdNode; } }
            /// <summary>
            /// Первое ребро треугольника
            /// </summary>
            internal EdgeStore FirstEdge {
                get { return _firstEdge; }
                set { _firstEdge = value; }
            }
            /// <summary>
            /// Второе ребро треугольника
            /// </summary>
            internal EdgeStore SecondEdge {
                get { return _secondEdge; }
                set { _secondEdge = value; }
            }
            /// <summary>
            /// Третье ребро треугольника
            /// </summary>
            internal EdgeStore ThirdEdge {
                get { return _thirdEdge; }
                set { _thirdEdge = value; }
            }
            #endregion
        }
        #endregion
    }
}