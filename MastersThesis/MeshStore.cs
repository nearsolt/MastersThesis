using static MastersThesis.MainForm.PlanarObjectStore;

namespace MastersThesis {
    partial class Triangulation {
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
                    return edge1.GetXCoordinateForVector * edge2.GetYCoordinateForVector - edge1.GetYCoordinateForVector * edge2.GetXCoordinateForVector;
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
                    return CheckCommonNode(pseudoscalar1, pseudoscalar2, pseudoscalar3, pseudoscalar4) ? false : true;
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

                    double angle = Math.Atan2(sinValue, cosValue) / Math.PI * 180f;
                    if (angle <= 0) {
                        angle += 360f;
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
                private int _nodeID;
                private double _xCoordinate;
                private double _yCoordinate;
                private Node _nodeData;
                private List<EdgeStore> _connectedEdgeList = new List<EdgeStore>();

                internal NodeStore(int nodeID, double xCoordinate, double yCoordinate, Node nodeDataAsNode2D) {
                    this._nodeID = nodeID;
                    this._xCoordinate = xCoordinate;
                    this._yCoordinate = yCoordinate;
                    this._nodeData = nodeDataAsNode2D;
                }

                internal int NodeID { get { return this._nodeID; } }
                internal double XCoordinate { get { return this._xCoordinate; } }
                internal double YCoordinate { get { return this._yCoordinate; } }
                internal MainForm.PlanarObjectStore.Node GetNodeData { get { return this._nodeData; } }

                internal bool Equals(NodeStore other) {
                    return this._xCoordinate == other.XCoordinate && this._yCoordinate == other.YCoordinate;
                }
                internal EdgeStore CCVerticalEdge(int id) {
                    if (id > this._connectedEdgeList.Count - 1) {
                        return null;
                    }
                    return this._connectedEdgeList[id];
                }
                internal EdgeStore CCVerticalEdge(EdgeStore withEdge) {
                    int nextIndex = this._connectedEdgeList.FindIndex(obj => obj.EdgeID == withEdge.EdgeID);
                    if (nextIndex == -1) {
                        return null;
                    }
                    nextIndex++;
                    if (nextIndex == this._connectedEdgeList.Count) {
                        nextIndex = 0;
                    }
                    return CCVerticalEdge(nextIndex);
                }
                internal EdgeStore CWVerticalEdge(int id) {
                    int count = this._connectedEdgeList.Count - 1;
                    if (id > count) {
                        return null;
                    }
                    return this._connectedEdgeList[count - id];
                }
                internal EdgeStore CWVerticalEdge(EdgeStore withEdge) {
                    int nextIndex = this._connectedEdgeList.FindIndex(obj => obj.EdgeID == withEdge.EdgeID);
                    if (nextIndex == -1) {
                        return null;
                    }
                    nextIndex--;
                    if (nextIndex == -1) {
                        nextIndex = this._connectedEdgeList.Count - 1;
                    }
                    return CCVerticalEdge(nextIndex);
                }
                private EdgeStore GetOrientedEdgeFromThisNode(EdgeStore edge) {
                    NodeStore node = new NodeStore(this._nodeID, this._xCoordinate, this._yCoordinate, this._nodeData);
                    return edge.FirstNode.Equals(node) ? edge : new EdgeStore(edge.EdgeID, node, edge.GetOtherNode(node));
                }
                internal void AddSortedEdge(EdgeStore edge) {
                    edge = GetOrientedEdgeFromThisNode(edge);
                    if (this._connectedEdgeList.Count == 0) {
                        this._connectedEdgeList.Add(edge);
                        return;
                    }
                    if (new EdgeAngleComparerVertical().Compare(this._connectedEdgeList[this._connectedEdgeList.Count - 1], edge) <= 0) {
                        this._connectedEdgeList.Add(edge);
                        return;
                    }
                    if (new EdgeAngleComparerVertical().Compare(this._connectedEdgeList[0], edge) >= 0) {
                        this._connectedEdgeList.Insert(0, edge);
                        return;
                    }

                    int index = this._connectedEdgeList.BinarySearch(edge, new EdgeAngleComparerVertical());
                    if (index < 0) {
                        index = ~index;
                    }
                    this._connectedEdgeList.Insert(index, edge);
                }
                internal void RemoveEdge(EdgeStore edge) {
                    int index = this._connectedEdgeList.FindIndex(obj => obj.Equals(edge));
                    if (index != -1) {
                        this._connectedEdgeList.RemoveAt(index);
                    }
                }
                internal void RemoveEdge(int edgeID) {
                    int index = this._connectedEdgeList.FindIndex(obj => obj.EdgeID == edgeID);
                    if (index != -1) {
                        this._connectedEdgeList.RemoveAt(index);
                    }
                }
            }

            internal class EdgeAngleComparerVertical : IComparer<EdgeStore> {
                public int Compare(EdgeStore edge1, EdgeStore edge2) {
                    EdgeStore vertEdge1 = new EdgeStore(-1, edge1.FirstNode, new NodeStore(-1, edge1.FirstNode.XCoordinate, edge1.FirstNode.YCoordinate - 100, null));
                    double angle1 = Helper.GetAngleBetween(vertEdge1, edge1);

                    EdgeStore vertEdge2 = new EdgeStore(-1, edge2.FirstNode, new NodeStore(-1, edge2.FirstNode.XCoordinate, edge2.FirstNode.YCoordinate - 100, null));
                    double angle2 = Helper.GetAngleBetween(vertEdge2, edge2);

                    return angle1 < angle2 ? -1 : angle1 > angle2 ? 1 : 0;
                }
            }

            internal class EdgeStore {
                private int _edgeID;
                private NodeStore _firstNode;
                private NodeStore _secondNode;
                private int _firstTriangleID;
                private int _secondTriangleID;

                internal EdgeStore(int edgeID, NodeStore firstNode, NodeStore secondNode) {
                    this._edgeID = edgeID;
                    this._firstNode = firstNode;
                    this._secondNode = secondNode;
                    this._firstTriangleID = -1;
                    this._secondTriangleID = -1;
                }

                internal int EdgeID { get { return this._edgeID; } }
                internal NodeStore FirstNode { get { return this._firstNode; } }
                internal NodeStore SecondNode { get { return this._secondNode; } }
                internal int GetFirstTriangleID { get { return this._firstTriangleID; } }
                internal int GetSecondTriangleID { get { return this._secondTriangleID; } }
                internal double GetXCoordinateForVector {
                    get {
                        return this._secondNode.XCoordinate - this._firstNode.XCoordinate;
                    }
                }
                internal double GetYCoordinateForVector {
                    get {
                        return this._secondNode.YCoordinate - this._firstNode.YCoordinate;
                    }
                }
                internal double GetEdgeLegth {
                    get {
                        return Math.Sqrt((this._secondNode.XCoordinate - this._firstNode.XCoordinate) * (this._secondNode.XCoordinate - this._firstNode.XCoordinate) +
                                         (this._secondNode.YCoordinate - this._firstNode.YCoordinate) * (this._secondNode.YCoordinate - this._firstNode.YCoordinate));
                    }
                }

                internal void AddTriangleID(int triangleID) {
                    if (this._firstTriangleID == -1 || this._firstTriangleID == triangleID) {
                        this._firstTriangleID = triangleID;
                    } else {
                        this._secondTriangleID = triangleID;
                    }
                }
                internal void RemoveTriangleID(int triangleID) {
                    if (this._firstTriangleID == triangleID) {
                        this._firstTriangleID = -1;
                    }
                    if (this._secondTriangleID == triangleID) {
                        this._secondTriangleID = -1;
                    }
                }
                internal bool Equals(EdgeStore other) {
                    return this._firstNode.Equals(other.FirstNode) && this._secondNode.Equals(other.SecondNode);
                }
                internal bool CommutativeEquals(EdgeStore other) {
                    return (this._firstNode.Equals(other.FirstNode) && this._secondNode.Equals(other.SecondNode)) ||
                        (this._firstNode.Equals(other.SecondNode) && this._secondNode.Equals(other.FirstNode));
                }
                internal NodeStore GetOtherNode(NodeStore node) {
                    return node.Equals(this._firstNode) ? this._secondNode : this._firstNode;
                }
            }

            internal class TriangleStore {
                private int _triangleID;
                private NodeStore _firstNode;
                private NodeStore _secondNode;
                private NodeStore _thirdNode;
                private EdgeStore _firstEdge;
                private EdgeStore _secondEdge;
                private EdgeStore _thirdEdge;

                internal TriangleStore(int triangleID, NodeStore firstNode, NodeStore secondNode, NodeStore thirdNode) {
                    this._triangleID = triangleID;
                    this._firstNode = firstNode;
                    this._secondNode = secondNode;
                    this._thirdNode = thirdNode;
                }

                internal int TriangleID { get { return this._triangleID; } }
                internal NodeStore FirstNode { get { return this._firstNode; } }
                internal NodeStore SecondNode { get { return this._secondNode; } }
                internal NodeStore ThirdNode { get { return this._thirdNode; } }
                internal EdgeStore FirstEdge {
                    get { return this._firstEdge; }
                    set { this._firstEdge = value; }
                }
                internal EdgeStore SecondEdge {
                    get { return this._secondEdge; }
                    set { this._secondEdge = value; }
                }
                internal EdgeStore ThirdEdge {
                    get { return this._thirdEdge; }
                    set { this._thirdEdge = value; }
                }
            }
            #endregion
        }
    }
}
