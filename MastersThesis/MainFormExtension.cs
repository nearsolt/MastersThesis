namespace MastersThesis {
    partial class MainForm {

        #region Global Variables
        private enum TriangulationType : int {
            NodeGeneration = 0,
            GreedyTriangulation = 1,
            DelaunayTriangulation = 2,
            MeshRefinement = 3
        }
        private TriangulationType _triangulationType = TriangulationType.NodeGeneration;
        private SizeF _canvasSize;
        private PointF _canvasOrgin;
        private int _counterInstance;
        private int _meshRefinementCoefficient = 3;
        private readonly int _decimalPlaces = 2;
        private readonly int _timerInterval = 1000;
        private PlanarObjectStore _planarObjectStoreInstance;
        private List<Triangulation.MeshStore.Node2DStore> _nodeStoreList;
        private List<Triangulation.MeshStore.Edge2DStore> _edgeStoreList;
        private List<Triangulation.MeshStore.Triangle2DStore> _triangleStoreList;
        #endregion

        internal class PlanarObjectStore {
            private List<Node2D> _nodeList = new List<Node2D>();
            private List<Edge2D> _edgeList = new List<Edge2D>();
            private List<Triangle2D> _triangleList = new List<Triangle2D>();
            private List<AnimationTracker> _trackerList = new List<AnimationTracker>();

            internal PlanarObjectStore() {
                this._nodeList = new List<Node2D>();
                this._edgeList = new List<Edge2D>();
                this._trackerList = new List<AnimationTracker>();
            }

            internal List<Node2D> NodeList {
                get { return this._nodeList; }
                set { this._nodeList = value; }
            }
            internal List<Edge2D> EdgeList {
                get { return this._edgeList; }
                set { this._edgeList = value; }
            }
            internal List<Triangle2D> TriangleList {
                get { return this._triangleList; }
                set { this._triangleList = value; }
            }
            internal List<AnimationTracker> TrackerList {
                get { return this._trackerList; }
                set { this._trackerList = value; }
            }

            internal void DrawPicture(ref Graphics graphics, bool labelVisibility) {
                Graphics gr = graphics;
                this._nodeList.ForEach(obj => obj.DrawNode(ref gr, labelVisibility));
            }
            internal void DrawPicture(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int counterInstance) {
                Graphics gr = graphics;
                if (tweenAnimation) {
                    if (counterInstance >= 0 && this._trackerList.Count != 0) {
                        this._trackerList[counterInstance].EdgeList.ForEach(obj => obj.DrawEdge(ref gr));
                    }
                } else {
                    this._edgeList.ForEach(obj => obj.DrawEdge(ref gr));
                }
                this._nodeList.ForEach(obj => obj.DrawNode(ref gr, labelVisibility));
            }
            internal void DrawPicture(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int counterInstance, bool meshVisibility) {
                Graphics gr = graphics;
                if (tweenAnimation) {
                    if (counterInstance >= 0 && this._trackerList.Count != 0) {
                        this._trackerList[counterInstance].EdgeList.ForEach(obj => obj.DrawEdge(ref gr));
                        this._trackerList[counterInstance].TriangleList.ForEach(obj => obj.DrawTriangle(ref gr, labelVisibility, meshVisibility));
                    }
                } else {
                    this._edgeList.ForEach(obj => obj.DrawEdge(ref gr));
                    this._triangleList.ForEach(obj => obj.DrawTriangle(ref gr, labelVisibility, meshVisibility));
                }
                this._nodeList.ForEach(obj => obj.DrawNode(ref gr, labelVisibility));
            }
            internal void DrawPicture(ref Graphics graphics, bool labelVisibility, bool tweenAnimation, ref int counterInstance, bool meshVisibility, bool circumcircleVisibility) {
                Graphics gr = graphics;
                if (tweenAnimation) {
                    if (counterInstance >= 0 && this._trackerList.Count != 0) {
                        this._trackerList[counterInstance].EdgeList.ForEach(obj => obj.DrawEdge(ref gr));
                        this._trackerList[counterInstance].TriangleList.ForEach(obj => obj.DrawTriangleAndCircumcircle(ref gr, labelVisibility, meshVisibility, circumcircleVisibility));
                    }
                } else {
                    this._edgeList.ForEach(obj => obj.DrawEdge(ref gr));
                    this._triangleList.ForEach(obj => obj.DrawTriangleAndCircumcircle(ref gr, labelVisibility, meshVisibility, circumcircleVisibility));
                }
                this._nodeList.ForEach(obj => obj.DrawNode(ref gr, labelVisibility));
            }

            #region Node2D, Edge2D, Triangle2D, AnimationTracker
            internal class Node2D {
                private int _nodeID;
                private double _xCoordinate;
                private double _yCoordinate;

                internal Node2D(int nodeID, double xCoordinate, double yCoordinate) {
                    this._nodeID = nodeID;
                    this._xCoordinate = xCoordinate;
                    this._yCoordinate = yCoordinate;
                }

                internal int NodeID { get { return this._nodeID; } }
                internal double XCoordinate { get { return this._xCoordinate; } }
                internal double YCoordinate { get { return this._yCoordinate; } }

                internal bool Equals(Node2D other) {
                    return this._xCoordinate == other.XCoordinate && this._yCoordinate == other.YCoordinate;
                }
                internal PointF GetPoint() {
                    return new PointF((float)this._xCoordinate, -(float)this._yCoordinate);
                }
                private PointF GetPointForEllipse() {
                    return new PointF((float)this._xCoordinate - 2, -(float)this._yCoordinate - 2);
                }
                internal void DrawNode(ref Graphics graphics, bool labelVisibility) {
                    graphics.FillEllipse(new Pen(Color.BlueViolet, 2).Brush, new RectangleF(GetPointForEllipse(), new SizeF(4, 4)));
                    if (labelVisibility) {
                        string str = $"{this._nodeID}({this._xCoordinate}; {this._yCoordinate})";
                        SizeF strSize = graphics.MeasureString(str, new Font("Cambria", 6));
                        graphics.DrawString(str, new Font("Cambria", 6), new Pen(Color.DarkBlue, 2).Brush, GetPointForEllipse().X + 3 - strSize.Width / 2, GetPointForEllipse().Y + 3 + strSize.Height / 2);
                    }
                }
            }

            internal class Edge2D {
                private int _edgeID;
                private Node2D _firstNode;
                private Node2D _secondNode;

                internal Edge2D(int edgeID, Node2D firstNode, Node2D secondNode) {
                    this._edgeID = edgeID;
                    this._firstNode = firstNode;
                    this._secondNode = secondNode;
                }

                internal int EdgeID { get { return this._edgeID; } }
                internal Node2D FirstNode { get { return this._firstNode; } }
                internal Node2D SecondNode { get { return this._secondNode; } }

                internal bool Equals(Edge2D other) {
                    return this._firstNode.Equals(other.FirstNode) && this._secondNode.Equals(other.SecondNode);
                }
                internal bool CommutativeEquals(Edge2D other) {
                    return (this._firstNode.Equals(other.FirstNode) && this._secondNode.Equals(other.SecondNode)) ||
                        (this._firstNode.Equals(other.SecondNode) && this._secondNode.Equals(other.FirstNode));
                }
                internal void DrawEdge(ref Graphics graphics) {
                    graphics.DrawLine(new Pen(Color.DarkOrange, 1), this._firstNode.GetPoint(), this._secondNode.GetPoint());
                }
            }

            internal class Triangle2D {
                private int _triangleID;
                private Node2D _firstNode;
                private Node2D _secondNode;
                private Node2D _thirdNode;
                private Node2D _middleNode;
                private readonly double _scalingCoeff = 0.6;
                private Node2D _circleCenter;
                private double _circleRadius;
                private Node2D _ellipseEdge;

                internal Triangle2D(int triangleID, Node2D firstNode, Node2D secondNode, Node2D thirdNode) {
                    this._triangleID = triangleID;
                    this._firstNode = firstNode;
                    this._secondNode = secondNode;
                    this._thirdNode = thirdNode;
                    this._middleNode = new Node2D(-1, (firstNode.XCoordinate + secondNode.XCoordinate + thirdNode.XCoordinate) / 3,
                                                  (firstNode.YCoordinate + secondNode.YCoordinate + thirdNode.YCoordinate) / 3);
                    SetIncircle();
                }

                internal int TriangleID { get { return this._triangleID; } }
                private PointF GetFirstPoint {
                    get {
                        return new PointF((float)(this._middleNode.GetPoint().X * (1 - this._scalingCoeff) + this._firstNode.GetPoint().X * this._scalingCoeff),
                                    (float)(this._middleNode.GetPoint().Y * (1 - this._scalingCoeff) + this._firstNode.GetPoint().Y * this._scalingCoeff));
                    }
                }
                private PointF GetSecondPoint {
                    get {
                        return new PointF((float)(this._middleNode.GetPoint().X * (1 - this._scalingCoeff) + this._secondNode.GetPoint().X * this._scalingCoeff),
                                    (float)(this._middleNode.GetPoint().Y * (1 - this._scalingCoeff) + this._secondNode.GetPoint().Y * this._scalingCoeff));
                    }
                }
                private PointF GetThirdPoint {
                    get {
                        return new PointF((float)(this._middleNode.GetPoint().X * (1 - this._scalingCoeff) + this._thirdNode.GetPoint().X * this._scalingCoeff),
                                    (float)(this._middleNode.GetPoint().Y * (1 - this._scalingCoeff) + this._thirdNode.GetPoint().Y * this._scalingCoeff));
                    }
                }

                private void SetIncircle() {
                    double dA = this._firstNode.XCoordinate * this._firstNode.XCoordinate + this._firstNode.YCoordinate * this._firstNode.YCoordinate;
                    double dB = this._secondNode.XCoordinate * this._secondNode.XCoordinate + this._secondNode.YCoordinate * this._secondNode.YCoordinate;
                    double dC = this._thirdNode.XCoordinate * this._thirdNode.XCoordinate + this._thirdNode.YCoordinate * this._thirdNode.YCoordinate;

                    double aux1 = dA * (this._thirdNode.YCoordinate - this._secondNode.YCoordinate) + dB * (this._firstNode.YCoordinate - this._thirdNode.YCoordinate) +
                                    dC * (this._secondNode.YCoordinate - this._firstNode.YCoordinate);

                    double aux2 = -(dA * (this._thirdNode.XCoordinate - this._secondNode.XCoordinate) + dB * (this._firstNode.XCoordinate - this._thirdNode.XCoordinate) +
                                    dC * (this._secondNode.XCoordinate - this._firstNode.XCoordinate));

                    double div = 2 * (this._firstNode.XCoordinate * (this._thirdNode.YCoordinate - this._secondNode.YCoordinate) + this._secondNode.XCoordinate *
                                    (this._firstNode.YCoordinate - this._thirdNode.YCoordinate) + this._thirdNode.XCoordinate * (this._secondNode.YCoordinate - this._firstNode.YCoordinate));

                    double centerXCoord = aux1 / div;
                    double centerYCoord = aux2 / div;

                    this._circleCenter = new Node2D(-1, centerXCoord, centerYCoord);
                    this._circleRadius = Math.Sqrt((centerXCoord - this._firstNode.XCoordinate) * (centerXCoord - this._firstNode.XCoordinate) +
                                                   (centerYCoord - this._firstNode.YCoordinate) * (centerYCoord - this._firstNode.YCoordinate));
                    this._ellipseEdge = new Node2D(-1, centerXCoord - this._circleRadius, centerYCoord + this._circleRadius);
                }
                internal void DrawTriangle(ref Graphics graphics, bool labelVisibility, bool meshVisibility) {
                    if (meshVisibility) {
                        PointF[] curvePoints = new PointF[] { this.GetFirstPoint, this.GetSecondPoint, this.GetThirdPoint };
                        graphics.FillPolygon(new Pen(Color.LightGreen, 1).Brush, curvePoints);
                        if (labelVisibility) {
                            graphics.DrawString(this._triangleID.ToString(), new Font("Cambria", 6), new Pen(Color.DeepPink, 2).Brush, this._middleNode.GetPoint());
                        }
                    }
                }
                internal void DrawTriangleAndCircumcircle(ref Graphics graphics, bool labelVisibility, bool meshVisibility, bool circumcircleVisibility) {
                    Pen trianglePen = new Pen(Color.LightGreen, 1);
                    if (meshVisibility) {
                        PointF[] curvePoints = new PointF[] { this.GetFirstPoint, this.GetSecondPoint, this.GetThirdPoint };
                        graphics.FillPolygon(trianglePen.Brush, curvePoints);
                        if (labelVisibility) {
                            graphics.DrawString(this._triangleID.ToString(), new Font("Cambria", 6), new Pen(Color.DeepPink, 2).Brush, this._middleNode.GetPoint());
                        }
                    }
                    if (circumcircleVisibility) {
                        graphics.DrawEllipse(trianglePen, this._ellipseEdge.GetPoint().X, this._ellipseEdge.GetPoint().Y, (float)this._circleRadius * 2, (float)this._circleRadius * 2);
                    }
                }
            }

            internal class AnimationTracker {
                private List<Edge2D> _edgeList;
                private List<Triangle2D> _triangleList;

                internal List<Edge2D> EdgeList {
                    get { return this._edgeList; }
                    set { this._edgeList = value; }
                }
                internal List<Triangle2D> TriangleList {
                    get { return this._triangleList; }
                    set { this._triangleList = value; }
                }
            }
            #endregion
        }
    }
}
