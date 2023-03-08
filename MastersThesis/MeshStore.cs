using static MastersThesis.MainForm.PlanarObjectStore;

namespace MastersThesis {
    partial class MeshStore {

        #region Class Helper
        internal class Helper {
            private static bool CheckProjectionsIntersection(double firstNodeCoordOfEdge1, double secondNodeCoordOfEdge1, double firstNodeCoordOfEdge2, double secondNodeCoordOfEdge2) {
                if (firstNodeCoordOfEdge1 > secondNodeCoordOfEdge1) {
                    (firstNodeCoordOfEdge1, secondNodeCoordOfEdge1) = (secondNodeCoordOfEdge1, firstNodeCoordOfEdge1);
                }
                if (firstNodeCoordOfEdge2 > secondNodeCoordOfEdge2) {
                    (firstNodeCoordOfEdge2, secondNodeCoordOfEdge2) = (secondNodeCoordOfEdge2, firstNodeCoordOfEdge2);
                }
                return Math.Max(firstNodeCoordOfEdge1, firstNodeCoordOfEdge2) <= Math.Min(secondNodeCoordOfEdge1, secondNodeCoordOfEdge2);
            }
            private static bool CheckPairwiseIntersection(EdgeStore edge1, EdgeStore edge2) {
                return CheckProjectionsIntersection(edge1.FirstNode.XCoordinate, edge1.SecondNode.XCoordinate, edge2.FirstNode.XCoordinate, edge2.SecondNode.XCoordinate) &&
                    CheckProjectionsIntersection(edge1.FirstNode.YCoordinate, edge1.SecondNode.YCoordinate, edge2.FirstNode.YCoordinate, edge2.SecondNode.YCoordinate);
            }
            private static double PseudoscalarProduct(EdgeStore edge1, EdgeStore edge2) {
                return edge1.XCoordinateForVector * edge2.YCoordinateForVector - edge1.YCoordinateForVector * edge2.XCoordinateForVector;
            }
            private static bool CheckCommonNode(double p1, double p2, double p3, double p4) {
                return (p1 == 0 && p2 != 0 && p3 == 0 && p4 != 0) || (p1 != 0 && p2 == 0 && p3 != 0 && p4 == 0) ||
                       (p1 == 0 && p2 != 0 && p3 != 0 && p4 == 0) || (p1 != 0 && p2 == 0 && p3 == 0 && p4 != 0);
            }
            internal static bool CheckEdgeIntersection(EdgeStore edge1, EdgeStore edge2) {
                if (!CheckPairwiseIntersection(edge1, edge2)) {
                    return false;
                }

                double pseudoscalar1 = PseudoscalarProduct(new EdgeStore(-2, edge1.FirstNode, edge1.SecondNode), new EdgeStore(-2, edge1.FirstNode, edge2.FirstNode));
                double pseudoscalar2 = PseudoscalarProduct(new EdgeStore(-2, edge1.FirstNode, edge1.SecondNode), new EdgeStore(-2, edge1.FirstNode, edge2.SecondNode));

                if ((pseudoscalar1 > 0 && pseudoscalar2 > 0) || (pseudoscalar1 < 0 && pseudoscalar2 < 0)) {
                    return false;
                }

                double pseudoscalar3 = PseudoscalarProduct(new EdgeStore(-2, edge2.FirstNode, edge2.SecondNode), new EdgeStore(-2, edge2.FirstNode, edge1.FirstNode));
                double pseudoscalar4 = PseudoscalarProduct(new EdgeStore(-2, edge2.FirstNode, edge2.SecondNode), new EdgeStore(-2, edge2.FirstNode, edge1.SecondNode));

                if ((pseudoscalar3 > 0 && pseudoscalar4 > 0) || (pseudoscalar3 < 0 && pseudoscalar4 < 0)) {
                    return false;
                }
                return !CheckCommonNode(pseudoscalar1, pseudoscalar2, pseudoscalar3, pseudoscalar4);
            }

            internal static double GetAngleBetween(EdgeStore withEdge, EdgeStore edge) {
                double xCoordOfVector1 = withEdge.SecondNode.XCoordinate - withEdge.FirstNode.XCoordinate;
                double yCoordOfVector1 = withEdge.SecondNode.YCoordinate - withEdge.FirstNode.YCoordinate;
                double normalize1 = Math.Sqrt(xCoordOfVector1 * xCoordOfVector1 + yCoordOfVector1 * yCoordOfVector1);
                xCoordOfVector1 /= normalize1;
                yCoordOfVector1 /= normalize1;

                double xCoordOfVector2 = edge.SecondNode.XCoordinate - edge.FirstNode.XCoordinate;
                double yCoordOfVector2 = edge.SecondNode.YCoordinate - edge.FirstNode.YCoordinate;
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
            internal static bool IsCollinear(NodeStore node1, NodeStore node2, NodeStore node3) {
                return (node2.XCoordinate - node1.XCoordinate) * (node3.YCoordinate - node1.YCoordinate) -
                    (node2.YCoordinate - node1.YCoordinate) * (node3.XCoordinate - node1.XCoordinate) == 0;
            }
            private static bool CCW(NodeStore node1, NodeStore node2, NodeStore node3) {
                return (node2.XCoordinate - node1.XCoordinate) * (node3.YCoordinate - node1.YCoordinate) -
                    (node2.YCoordinate - node1.YCoordinate) * (node3.XCoordinate - node1.XCoordinate) > 0;
            }
            internal static bool LeftOf(NodeStore node, EdgeStore edge) {
                return CCW(node, edge.FirstNode, edge.SecondNode);
            }
            internal static bool RightOf(NodeStore node, EdgeStore edge) {
                return CCW(node, edge.SecondNode, edge.FirstNode);
            }
            internal static bool InCircle(NodeStore node1, NodeStore node2, NodeStore node3, NodeStore otherNode) {
                double a1 = node1.XCoordinate - otherNode.XCoordinate;
                double a2 = node1.YCoordinate - otherNode.YCoordinate;

                double b1 = node2.XCoordinate - otherNode.XCoordinate;
                double b2 = node2.YCoordinate - otherNode.YCoordinate;

                double c1 = node3.XCoordinate - otherNode.XCoordinate;
                double c2 = node3.YCoordinate - otherNode.YCoordinate;

                double a3 = a1 * a1 + a2 * a2;
                double b3 = b1 * b1 + b2 * b2;
                double c3 = c1 * c1 + c2 * c2;

                double det = a1 * b2 * c3 + a2 * b3 * c1 + a3 * b1 * c2 - a3 * b2 * c1 - a1 * b3 * c2 - a2 * b1 * c3;
                return det > 0;
            }
        }
        #endregion

        #region Base Classes: NodeStore, EdgeAngleComparerVertical, EdgeStore, TriangleStore
        internal class NodeStore {

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
            /// <summary>
            /// Узел (Node)
            /// </summary>
            private Node _node;
            /// <summary>
            /// Список смежных ребер
            /// </summary>
            private List<EdgeStore> _connectedEdgeList = new List<EdgeStore>();
            #endregion

            #region Constructors & Properties
            ///// <summary>
            ///// Конструктор
            ///// </summary>
            ///// <param name="nodeID">ID узла</param>
            ///// <param name="xCoordinate">Координата x</param>
            ///// <param name="yCoordinate">Координата y</param>
            //internal NodeStore(int nodeID, double xCoordinate, double yCoordinate) {
            //    _nodeID = nodeID;
            //    _xCoordinate = xCoordinate;
            //    _yCoordinate = yCoordinate;
            //}
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
#warning refactoring
            internal EdgeStore CCVerticalEdge(int id) {
                if (id > _connectedEdgeList.Count - 1) {
                    return null;
                }
                return _connectedEdgeList[id];
            }
            internal EdgeStore CCVerticalEdge(EdgeStore withEdge) {
#warning refactoring use exist
                int nextIndex = _connectedEdgeList.FindIndex(obj => obj.EdgeID == withEdge.EdgeID);
                if (nextIndex == -1) {
                    return null;
                }
                nextIndex++;
                if (nextIndex == _connectedEdgeList.Count) {
                    nextIndex = 0;
                }
                return CCVerticalEdge(nextIndex);
            }
            internal EdgeStore CWVerticalEdge(int id) {
                int count = _connectedEdgeList.Count - 1;
                if (id > count) {
                    return null;
                }
                return _connectedEdgeList[count - id];
            }
            internal EdgeStore CWVerticalEdge(EdgeStore withEdge) {
#warning refactoring use exist
                int nextIndex = _connectedEdgeList.FindIndex(obj => obj.EdgeID == withEdge.EdgeID);
                if (nextIndex == -1) {
                    return null;
                }
                nextIndex--;
                if (nextIndex == -1) {
                    nextIndex = _connectedEdgeList.Count - 1;
                }
                return CCVerticalEdge(nextIndex);
            }
            private EdgeStore GetOrientedEdgeFromThisNode(EdgeStore edge) {
                NodeStore node = new NodeStore(_nodeID, _xCoordinate, _yCoordinate, _node);
                return edge.FirstNode.Equals(node) ? edge : new EdgeStore(edge.EdgeID, node, edge.GetOtherNode(node));
            }
            internal void AddSortedEdge(EdgeStore edge) {
                edge = GetOrientedEdgeFromThisNode(edge);
                if (_connectedEdgeList.Count == 0) {
                    _connectedEdgeList.Add(edge);
                    return;
                }
                if (new EdgeAngleComparerVertical().Compare(_connectedEdgeList[_connectedEdgeList.Count - 1], edge) <= 0) {
                    _connectedEdgeList.Add(edge);
                    return;
                }
                if (new EdgeAngleComparerVertical().Compare(_connectedEdgeList[0], edge) >= 0) {
                    _connectedEdgeList.Insert(0, edge);
                    return;
                }

                int index = _connectedEdgeList.BinarySearch(edge, new EdgeAngleComparerVertical());
                if (index < 0) {
                    index = ~index;
                }
                _connectedEdgeList.Insert(index, edge);
            }
            internal void RemoveEdge(EdgeStore edge) {
                int index = _connectedEdgeList.FindIndex(obj => obj.Equals(edge));
                if (index != -1) {
                    _connectedEdgeList.RemoveAt(index);
                }
            }
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
            public int Compare(EdgeStore edge1, EdgeStore edge2) {
                EdgeStore vertEdge1 = new EdgeStore(-1, edge1.FirstNode, new NodeStore(-1, edge1.FirstNode.XCoordinate, edge1.FirstNode.YCoordinate - 100, null));
                double angle1 = Helper.GetAngleBetween(vertEdge1, edge1);

                EdgeStore vertEdge2 = new EdgeStore(-1, edge2.FirstNode, new NodeStore(-1, edge2.FirstNode.XCoordinate, edge2.FirstNode.YCoordinate - 100, null));
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
            private int _edgeID;
            /// <summary>
            /// Первый узел ребра
            /// </summary>
            private NodeStore _firstNode;
            /// <summary>
            /// Второй узел ребра
            /// </summary>
            private NodeStore _secondNode;
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
            /// Координата x для вектора
            /// </summary>
            internal double XCoordinateForVector { get { return _secondNode.XCoordinate - _firstNode.XCoordinate; } }
            /// <summary>
            /// Координата y для вектора
            /// </summary>
            internal double YCoordinateForVector { get { return _secondNode.YCoordinate - _firstNode.YCoordinate; } }
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
            internal void AddTriangleID(int triangleID) {
                if (_firstTriangleID == -1 || _firstTriangleID == triangleID) {
                    _firstTriangleID = triangleID;
                } else {
                    _secondTriangleID = triangleID;
                }
            }
            internal void RemoveTriangleID(int triangleID) {
                if (_firstTriangleID == triangleID) {
                    _firstTriangleID = -1;
                }
                if (_secondTriangleID == triangleID) {
                    _secondTriangleID = -1;
                }
            }
            internal bool Equals(EdgeStore other) {
                return _firstNode.Equals(other.FirstNode) && _secondNode.Equals(other.SecondNode);
            }
            internal bool CommutativeEquals(EdgeStore other) {
                return (_firstNode.Equals(other.FirstNode) && _secondNode.Equals(other.SecondNode)) ||
                    (_firstNode.Equals(other.SecondNode) && _secondNode.Equals(other.FirstNode));
            }
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
            private int _triangleID;
            /// <summary>
            /// Первый узел треугольника
            /// </summary>
            private NodeStore _firstNode;
            /// <summary>
            /// Второй узел треугольника
            /// </summary>
            private NodeStore _secondNode;
            /// <summary>
            /// Третий узел треугольника
            /// </summary>
            private NodeStore _thirdNode;
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