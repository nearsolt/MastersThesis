using static MastersThesis.MainForm.PlanarObjectStore;
using static MastersThesis.Triangulation.MeshStore;

namespace MastersThesis {
    internal partial class Triangulation {

        #region Greedy Triangulation, Delaunay Triangulation Mesh Refinement start functions
        /// <summary>
        /// Функция старта для построения жадной триангуляции
        /// </summary>
        /// <param name="inputNodeList">Список узлов, передаваемый на вход</param>
        /// <param name="outputEdgeList">Список ребер, получаемый на выходе</param>
        /// <param name="outputAnimationList">Список элементов анимации, получаемый на выходе</param>
        internal void GreedyTriangulationStart(List<Node> inputNodeList, ref List<Edge> outputEdgeList, ref List<TweenAnimation> outputAnimationList) {
            List<NodeStore> nodeList = new List<NodeStore>();

            for (int j = 0; j < inputNodeList.Count; j++) {
                nodeList.Add(new NodeStore(j, inputNodeList[j].XCoordinate, inputNodeList[j].YCoordinate, inputNodeList[j]));
            }
            MeshStore meshStore = new MeshStore(nodeList, ref outputEdgeList, ref outputAnimationList);
        }
        /// <summary>
        /// Функция старта для построения триангуляции Делоне
        /// </summary>
        /// <param name="inputNodeList">Список узлов, передаваемый на вход</param>
        /// <param name="outputEdgeList">Список ребер, получаемый на выходе</param>
        /// <param name="outputTriangleList">Список треугольников, получаемый на выходе</param>
        /// <param name="outputAnimationList">Список элементов анимации, получаемый на выходе</param>
        /// <param name="nodeStoreList">Список узлов (NodeStore)</param>
        /// <param name="edgeStoreList">Список ребер (EdgeStore)</param>
        /// <param name="triangleStoreList">Список треугольников (TriangleStore)</param>
        internal void DelaunayTriangulationStart(List<Node> inputNodeList, ref List<Edge> outputEdgeList, ref List<Triangle> outputTriangleList, ref List<TweenAnimation> outputAnimationList,
                                                 ref List<NodeStore> nodeStoreList, ref List<EdgeStore> edgeStoreList, ref List<TriangleStore> triangleStoreList) {

            List<Node> sortedInputNodeList = inputNodeList.OrderBy(obj => obj.XCoordinate).ThenBy(obj => obj.YCoordinate).ToList();
            List<NodeStore> nodeList = new List<NodeStore>();

            for (int j = 0; j < sortedInputNodeList.Count; j++) {
                nodeList.Add(new NodeStore(j, sortedInputNodeList[j].XCoordinate, sortedInputNodeList[j].YCoordinate, sortedInputNodeList[j]));
            }
            MeshStore meshStore = new MeshStore(nodeList, ref outputEdgeList, ref outputTriangleList, ref outputAnimationList,
                                                ref nodeStoreList, ref edgeStoreList, ref triangleStoreList);
        }
        /// <summary>
        /// Функция старта для построения триангуляции методом измельчения
        /// </summary>
        /// <param name="outputNodeList">Список узлов, получаемый на выходе</param>
        /// <param name="outputEdgeList">Список ребер, получаемый на выходе</param>
        /// <param name="outputTriangleList">Список треугольников, получаемый на выходе</param>
        /// <param name="outputAnimationList">Список элементов анимации, получаемый на выходе</param>
        /// <param name="nodeStoreList">Список узлов (NodeStore)</param>
        /// <param name="edgeStoreList">Список ребер (EdgeStore)</param>
        /// <param name="triangleStoreList">Список треугольников (TriangleStore)</param>
        /// <param name="tempAnimation">Элемент анимации</param>
        /// <param name="decimalPlaces">Количество знаков после запятой при вычислении координат внутренних узлов</param>
        /// <param name="meshRefinementCoeff">Коэффициент измельчения q</param>
        internal void MeshRefinementStart(ref List<Node> outputNodeList, ref List<Edge> outputEdgeList, ref List<Triangle> outputTriangleList, ref List<TweenAnimation> outputAnimationList,
                                          List<NodeStore> nodeStoreList, List<EdgeStore> edgeStoreList, List<TriangleStore> triangleStoreList,
                                          TweenAnimation tempAnimation, int decimalPlaces, int meshRefinementCoeff) {

            MeshStore meshStore = new MeshStore(ref outputNodeList, ref outputEdgeList, ref outputTriangleList, ref outputAnimationList,
                                                nodeStoreList, edgeStoreList, triangleStoreList, tempAnimation, decimalPlaces, meshRefinementCoeff);
        }
        #endregion


        internal partial class MeshStore {

            #region Private Class Variables
            /// <summary>
            /// Список узлов, получаемый на выходе
            /// </summary>
            private List<Node> _outputNodeList = new List<Node>();
            /// <summary>
            /// Список ребер, получаемый на выходе
            /// </summary>
            private List<Edge> _outputEdgeList = new List<Edge>();
            /// <summary>
            /// Список треугольников, получаемый на выходе
            /// </summary>
            private List<Triangle> _outputTriangleList = new List<Triangle>();
            /// <summary>
            /// Список элементов анимации, получаемый на выходе
            /// </summary>
            private List<TweenAnimation> _outputAnimationList = new List<TweenAnimation>();
            /// <summary>
            /// Список узлов (NodeStore)
            /// </summary>
            private List<NodeStore> _nodeList = new List<NodeStore>();
            /// <summary>
            /// Список ребер (EdgeStore)
            /// </summary>
            private List<EdgeStore> _edgeList = new List<EdgeStore>();
            /// <summary>
            /// Список треугольников (TriangleStore)
            /// </summary>
            private List<TriangleStore> _triangleList = new List<TriangleStore>();
            /// <summary>
            /// Список уникальных ID ребер
            /// </summary>
            private List<int> _uniqueEdgeIDList = new List<int>();
            /// <summary>
            /// Список уникальных ID треугольников
            /// </summary>
            private List<int> _uniqueTriangleIDList = new List<int>();
            #endregion

            #region Constructors
            #region Constructor: Greedy Triangulation
            /// <summary>
            /// Конструктор для жадной триангуляции
            /// </summary>
            /// <param name="inputNodeList">Список узлов, передаваемый на вход</param>
            /// <param name="outputEdgeList">Список ребер, получаемый на выходе</param>
            /// <param name="outputAnimationList">Список элементов анимации, получаемый на выходе</param>
            internal MeshStore(List<NodeStore> inputNodeList, ref List<Edge> outputEdgeList, ref List<TweenAnimation> outputAnimationList) {

                outputEdgeList = new List<Edge>();
                outputAnimationList = new List<TweenAnimation>();
                _nodeList = inputNodeList;

                GreedyTriangulation();

                outputEdgeList = _outputEdgeList;
                outputAnimationList = _outputAnimationList;
            }
            #endregion

            #region Constructor: Delaunay Triangulation
            /// <summary>
            /// Конструктор для триангуляции Делоне
            /// </summary>
            /// <param name="inputNodeList">Список узлов, передаваемый на вход</param>
            /// <param name="outputEdgeList">Список ребер, получаемый на выходе</param>
            /// <param name="outputTriangleList">Список треугольников, получаемый на выходе</param>
            /// <param name="outputAnimationList">Список элементов анимации, получаемый на выходе</param>
            /// <param name="nodeStoreList">Список узлов (NodeStore)</param>
            /// <param name="edgeStoreList">Список ребер (EdgeStore)</param>
            /// <param name="triangleStoreList">Список треугольников (TriangleStore)</param>
            internal MeshStore(List<NodeStore> inputNodeList, ref List<Edge> outputEdgeList, ref List<Triangle> outputTriangleList, ref List<TweenAnimation> outputAnimationList,
                               ref List<NodeStore> nodeStoreList, ref List<EdgeStore> edgeStoreList, ref List<TriangleStore> triangleStoreList) {

                outputEdgeList = new List<Edge>();
                outputTriangleList = new List<Triangle>();
                outputAnimationList = new List<TweenAnimation>();

                _nodeList = inputNodeList;

                if (_nodeList.Count > 3) {
                    int halfLength = Convert.ToInt32(Math.Ceiling(_nodeList.Count / 2.0));
                    List<NodeStore> leftList = _nodeList.GetRange(0, halfLength);
                    List<NodeStore> rightList = _nodeList.GetRange(halfLength, _nodeList.Count - halfLength);
                    DelaunayTriangulationDivideAndConquer(leftList, rightList);
                } else if (_nodeList.Count == 3) {
                    AddTriangle(_nodeList[0], _nodeList[1], _nodeList[2]);
                } else if (_nodeList.Count == 2) {
                    AddEdge(_nodeList[0], _nodeList[1]);
                }

                outputEdgeList = _outputEdgeList;
                outputTriangleList = _outputTriangleList;
                outputAnimationList = _outputAnimationList;

                nodeStoreList = _nodeList;
                edgeStoreList = _edgeList;
                triangleStoreList = _triangleList;
            }

            #endregion

            #region Constructor: Mesh Refinement Triangulation
#warning w1: Add summary params
            //Список узлов (NodeStore)
            //Список ребер (EdgeStore)
            //Список треугольников (TriangleStore)

            /// <summary>
            /// Конструктор для триангуляции методом измельчения
            /// </summary>
            /// <param name="outputNodeList">Список узлов, получаемый на выходе</param>
            /// <param name="outputEdgeList">Список ребер, получаемый на выходе</param>
            /// <param name="outputTriangleList">Список треугольников, получаемый на выходе</param>
            /// <param name="outputAnimationList">Список элементов анимации, получаемый на выходе</param>
            /// <param name="nodeStoreList_extracted"></param>
            /// <param name="edgeStoreList_extracted"></param>
            /// <param name="triangleStoreList_extracted"></param>
            /// <param name="tempAnimation">Элемент анимации</param>
            /// <param name="decimalPlaces">Количество знаков после запятой при вычислении координат внутренних узлов</param>
            /// <param name="meshRefinementCoeff">Коэффициент измельчения q</param>
            internal MeshStore(ref List<Node> outputNodeList, ref List<Edge> outputEdgeList, ref List<Triangle> outputTriangleList, ref List<TweenAnimation> outputAnimationList,
                               List<NodeStore> nodeStoreList_extracted, List<EdgeStore> edgeStoreList_extracted, List<TriangleStore> triangleStoreList_extracted,
                               TweenAnimation tempAnimation, int decimalPlaces, int meshRefinementCoeff) {

                _nodeList = nodeStoreList_extracted.ToList();
                _edgeList = edgeStoreList_extracted.ToList();
                _triangleList = triangleStoreList_extracted.ToList();

                foreach (NodeStore node in nodeStoreList_extracted) {
                    _outputNodeList.Add(new Node(node.NodeID, node.XCoordinate, node.YCoordinate));
                }
                _outputAnimationList.Add(tempAnimation);

                MeshRefinement(decimalPlaces, meshRefinementCoeff, triangleStoreList_extracted);

                outputNodeList = _outputNodeList;
                outputEdgeList = _outputEdgeList;
                outputTriangleList = _outputTriangleList;
                outputAnimationList = _outputAnimationList;
            }
            #endregion
            #endregion

            #region Methods: Greedy Triangulation  
#warning w0: refactoring
            /// <summary>
            /// Добавление ребра для жадной триангуляции
            /// </summary>
            /// <param name="firstNode">Первый узел ребра</param>
            /// <param name="secondNode">Второй узел ребра</param>
            private void AddEdgeForGT(NodeStore firstNode, NodeStore secondNode) {
                int edgeID = GetUniqueEdgeID();
                EdgeStore edge = new EdgeStore(edgeID, firstNode, secondNode);
                _edgeList.Add(edge);
                _outputEdgeList.Add(new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node));
                UpdateParticalTrackers();
            }
            /// <summary>
            /// Удаление ребра для жадной триангуляции
            /// </summary>
            /// <param name="edge">Удаляемое ребро</param>
            private void RemoveEdgeFromOutputListForGT(EdgeStore edge) {
                Edge tempEdge = new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node);
                int removeIndex = _outputEdgeList.FindIndex(obj => obj.CommutativeEquals(tempEdge));
                if (removeIndex != -1) {
                    _outputEdgeList.RemoveAt(removeIndex);
                    UpdateParticalTrackers();
                }
            }
            /// <summary>
            /// Удаление ребра для жадной триангуляции
            /// </summary>
            /// <param name="edgeID">ID удаляемого ребра</param>
            private void RemoveEdgeForGT(int edgeID) {
                int removeIndex = _edgeList.FindIndex(obj => obj.EdgeID == edgeID);

                _nodeList[_edgeList[removeIndex].FirstNode.NodeID].RemoveEdge(_edgeList[removeIndex].EdgeID);
                _nodeList[_edgeList[removeIndex].SecondNode.NodeID].RemoveEdge(_edgeList[removeIndex].EdgeID);
                RemoveEdgeFromOutputListForGT(_edgeList[removeIndex]);

                _uniqueEdgeIDList.Add(_edgeList[removeIndex].EdgeID);
                _edgeList.RemoveAt(removeIndex);
            }
            private void GreedyTriangulation() {
#warning To do: remove from the final version
#warning true -> draw only correct edges order by edge length; false -> draw all possible edges and remove incorrect edges
#if true
                List<EdgeStore> tempEdgeList = new List<EdgeStore>();
                for (int j = 1; j < _nodeList.Count; j++) {
                    for (int k = 0; k < j; k++) {
                        NodeStore tempNode1 = _nodeList[j];
                        NodeStore tempNode2 = _nodeList[k];
                        EdgeStore tempEdge = new EdgeStore(-2, tempNode1, tempNode2);
#warning w1: refactoring  use exist
                        if (tempEdgeList.FindIndex(obj => obj.CommutativeEquals(tempEdge)) == -1) {
                            tempEdgeList.Add(tempEdge);
                        }
                    }
                }
                tempEdgeList = tempEdgeList.OrderBy(obj => obj.EdgeLength).ToList();
                AddEdgeForGT(tempEdgeList[0].FirstNode, tempEdgeList[0].SecondNode);

                bool checkEdgeIntersection = false;

                for (int j = 1; j < tempEdgeList.Count; j++) {
                    for (int k = 0; k < j; k++) {
                        if (Helper.CheckEdgeIntersection(tempEdgeList[j], tempEdgeList[k])) {
                            checkEdgeIntersection = true;
                            tempEdgeList.RemoveAt(j);
                            j--;
                            break;
                        }
                    }
                    if (!checkEdgeIntersection) {
                        AddEdgeForGT(tempEdgeList[j].FirstNode, tempEdgeList[j].SecondNode);
                    }
                    checkEdgeIntersection = false;
                }
#else
                for (int j = 1; j < _nodeList.Count; j++) {
                    for (int k = 0; k < j; k++) {
                        NodeStore tempNode1 = _nodeList[j];
                        NodeStore tempNode2 = _nodeList[k];
                        EdgeStore tempEdge = new EdgeStore(-2, tempNode1, tempNode2);
#warning w1: refactoring  use exist
                        if (_edgeList.FindIndex(obj => obj.CommutativeEquals(tempEdge)) == -1) {
                            AddEdgeForGT(tempNode1, tempNode2);
                        }
                    }
                }
                _edgeList = _edgeList.OrderBy(obj => obj.GetEdgeLegth).ToList();
                for (int j = 1; j < _edgeList.Count; j++) {
                    for (int k = 0; k < j; k++) {
                        if (Helper.CheckEdgeIntersection(_edgeList[j], _edgeList[k])) {
                            RemoveEdgeForGT(_edgeList[j].EdgeID);
                            j--;
                            break;
                        }
                    }
                }
#endif
            }
            #endregion

            #region Methods: Mesh Refinement
            private void GetIntermediateNodes(Tuple<EdgeStore, List<NodeStore>, bool> tuple, int decimalPlaces, int meshRefinementCoeff, ref int nextNodeId) {
                NodeStore firstNode;
                NodeStore secondNode;
                if (tuple.Item3) {
                    firstNode = tuple.Item1.SecondNode;
                    secondNode = tuple.Item1.FirstNode;
                } else {
                    firstNode = tuple.Item1.FirstNode;
                    secondNode = tuple.Item1.SecondNode;
                }
                double stepByXCoord = (secondNode.XCoordinate - firstNode.XCoordinate) / meshRefinementCoeff;
                double stepByYCoord = (secondNode.YCoordinate - firstNode.YCoordinate) / meshRefinementCoeff;

                tuple.Item2.Add(firstNode);

                for (int j = 1; j < meshRefinementCoeff; j++) {
                    Node intermediateNode = new Node(nextNodeId, Math.Round(firstNode.XCoordinate + j * stepByXCoord, decimalPlaces, MidpointRounding.AwayFromZero),
                                                                 Math.Round(firstNode.YCoordinate + j * stepByYCoord, decimalPlaces, MidpointRounding.AwayFromZero));

                    NodeStore intermediateNodeStore = new NodeStore(intermediateNode.NodeID, intermediateNode.XCoordinate, intermediateNode.YCoordinate, intermediateNode);
#warning w1: refactoring  use exist
                    if (_nodeList.FindIndex(obj => obj.Equals(intermediateNodeStore)) == -1) {
                        _outputNodeList.Add(intermediateNode);
                        _nodeList.Add(intermediateNodeStore);
                        nextNodeId++;
                    }
                    tuple.Item2.Add(intermediateNodeStore);
                }
                tuple.Item2.Add(secondNode);
            }
            private List<NodeStore> GetIntermediateNodes(NodeStore firstNode, NodeStore secondNode, int decimalPlaces, int meshRefinementCoeff, ref int nextNodeId) {
                double stepByXCoord = (secondNode.XCoordinate - firstNode.XCoordinate) / meshRefinementCoeff;
                double stepByYCoord = (secondNode.YCoordinate - firstNode.YCoordinate) / meshRefinementCoeff;

                List<NodeStore> tempNodeList = new List<NodeStore>();
                tempNodeList.Add(firstNode);

                for (int j = 1; j < meshRefinementCoeff; j++) {
                    Node intermediateNode = new Node(nextNodeId, Math.Round(firstNode.XCoordinate + j * stepByXCoord, decimalPlaces, MidpointRounding.AwayFromZero),
                                                                 Math.Round(firstNode.YCoordinate + j * stepByYCoord, decimalPlaces, MidpointRounding.AwayFromZero));

                    NodeStore intermediateNodeStore = new NodeStore(intermediateNode.NodeID, intermediateNode.XCoordinate, intermediateNode.YCoordinate, intermediateNode);
#warning w1: refactoring  use exist
                    if (_nodeList.FindIndex(obj => obj.Equals(intermediateNodeStore)) == -1) {
                        _outputNodeList.Add(intermediateNode);
                        _nodeList.Add(intermediateNodeStore);
                        tempNodeList.Add(intermediateNodeStore);
                        nextNodeId++;
                    }
                }
                tempNodeList.Add(secondNode);
                return tempNodeList;
            }
            private void AddEdgeForMeshRefinement(NodeStore node1, NodeStore node2) {
                int edgeID = GetUniqueEdgeID();
                EdgeStore edge = new EdgeStore(edgeID, node1, node2);
#warning w1: refactoring  use exist
                if (_edgeList.FindIndex(obj => obj.CommutativeEquals(edge)) == -1) {
                    _edgeList.Add(edge);
                    _outputEdgeList.Add(new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node));
                    UpdateAllTrackers();
                }
            }
            private void MeshRefinement(int decimalPlaces, int meshRefinementCoeff, List<TriangleStore> triangleStoreList_extracted) {
                int nextNodeId = _nodeList.Count;
                int nextTriangleId = _triangleList.Count;

                foreach (TriangleStore triangle in triangleStoreList_extracted) {
                    bool reverseForEdge1;
                    bool reverseForEdge2;
                    if (triangle.FirstEdge.FirstNode == triangle.SecondEdge.FirstNode) {
                        reverseForEdge1 = false;
                        reverseForEdge2 = false;
                    } else if (triangle.FirstEdge.FirstNode == triangle.SecondEdge.SecondNode) {
                        reverseForEdge1 = false;
                        reverseForEdge2 = true;
                    } else if (triangle.FirstEdge.SecondNode == triangle.SecondEdge.FirstNode) {
                        reverseForEdge1 = true;
                        reverseForEdge2 = false;
                    } else {
                        reverseForEdge1 = true;
                        reverseForEdge2 = true;
                    }
                    // bool - reverse: true -> сhange orientation
                    Tuple<EdgeStore, List<NodeStore>, bool> tuple1 = new Tuple<EdgeStore, List<NodeStore>, bool>(triangle.FirstEdge, new List<NodeStore>(), reverseForEdge1);
                    GetIntermediateNodes(tuple1, decimalPlaces, meshRefinementCoeff, ref nextNodeId);

                    Tuple<EdgeStore, List<NodeStore>, bool> tuple2 = new Tuple<EdgeStore, List<NodeStore>, bool>(triangle.SecondEdge, new List<NodeStore>(), reverseForEdge2);
                    GetIntermediateNodes(tuple2, decimalPlaces, meshRefinementCoeff, ref nextNodeId);

                    List<NodeStore> prevList = new List<NodeStore>();
                    List<NodeStore> curList = new List<NodeStore>();
                    //j=-1
                    curList.Add(tuple1.Item2[0]);
                    //j=0
                    prevList = curList.ToList();
                    curList = new List<NodeStore>() { tuple1.Item2[1], tuple2.Item2[1] };
                    AddEdgeForMeshRefinement(prevList[0], curList[0]);
                    AddEdgeForMeshRefinement(curList[0], curList[1]);
                    AddEdgeForMeshRefinement(curList[1], prevList[0]);

                    _outputTriangleList.Add(new Triangle(nextTriangleId, prevList[0].Node, curList[0].Node, curList[1].Node));
                    UpdateAllTrackers();
                    nextTriangleId++;

                    for (int j = 1; j < meshRefinementCoeff; j++) {
                        prevList = curList.ToList();
                        curList = GetIntermediateNodes(tuple1.Item2[j + 1], tuple2.Item2[j + 1], decimalPlaces, j + 1, ref nextNodeId);
                        for (int k = 0; k < prevList.Count; k++) {
                            AddEdgeForMeshRefinement(prevList[k], curList[k]);
                            AddEdgeForMeshRefinement(curList[k], curList[k + 1]);
                            AddEdgeForMeshRefinement(curList[k + 1], prevList[k]);
                            _outputTriangleList.Add(new Triangle(nextTriangleId, prevList[k].Node, curList[k].Node, curList[k + 1].Node));
                            UpdateAllTrackers();
                            nextTriangleId++;
                            if (k < prevList.Count - 1) {
                                _outputTriangleList.Add(new Triangle(nextTriangleId, prevList[k].Node, curList[k + 1].Node, prevList[k + 1].Node));
                                UpdateAllTrackers();
                                nextTriangleId++;
                            }
                        }
                    }
                }

            }
            #endregion

            #region MeshStore methods: "Divide And Conquer" Delaunay Triangulation
            private void UpdateParticalTrackers() {
                TweenAnimation tweenAnimation = new TweenAnimation();
                tweenAnimation.EdgeList = new List<Edge>();
                tweenAnimation.EdgeList.AddRange(_outputEdgeList);
                _outputAnimationList.Add(tweenAnimation);
            }
            private void UpdateAllTrackers() {
                TweenAnimation tweenAnimation = new TweenAnimation();
                tweenAnimation.EdgeList = new List<Edge>();
                tweenAnimation.EdgeList.AddRange(_outputEdgeList);
                tweenAnimation.TriangleList = new List<Triangle>();
                tweenAnimation.TriangleList.AddRange(_outputTriangleList);
                _outputAnimationList.Add(tweenAnimation);
            }
            /// <summary>
            /// Получение уникального ID ребра
            /// </summary>
            /// <returns></returns>
            private int GetUniqueEdgeID() {
                int edgeID;
                if (_uniqueEdgeIDList.Count != 0) {
                    edgeID = _uniqueEdgeIDList[0];
                    _uniqueEdgeIDList.RemoveAt(0);
                } else {
                    edgeID = _edgeList.Count;
                }
                return edgeID;
            }
            /// <summary>
            /// Получение уникального ID треугольника
            /// </summary>
            /// <returns></returns>
            private int GetUniqueTriangleID() {
                int triangleID;
                if (_uniqueTriangleIDList.Count != 0) {
                    triangleID = _uniqueTriangleIDList[0];
                    _uniqueTriangleIDList.RemoveAt(0);
                } else {
                    triangleID = _triangleList.Count;
                }
                return triangleID;
            }
            private void AddToOutputList(EdgeStore edge, TriangleStore triangle) {
                Edge tempEdge = new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node);
                _outputEdgeList.Add(tempEdge);
                if (triangle != null) {
                    Triangle tempTriangle = new Triangle(triangle.TriangleID, triangle.FirstNode.Node, triangle.SecondNode.Node, triangle.ThirdNode.Node);
                    _outputTriangleList.Add(tempTriangle);
                }
                UpdateAllTrackers();
            }
            private void RemoveFromOutputList(EdgeStore edge, int firstTriangleID, int secondTriangleID) {
                Edge tempEdge = new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node);
                int removeIndex = _outputEdgeList.FindIndex(obj => obj.CommutativeEquals(tempEdge));
                if (removeIndex != -1) {
                    _outputEdgeList.RemoveAt(removeIndex);
                    if (firstTriangleID != -1 || secondTriangleID != -1) {
                        if (firstTriangleID < secondTriangleID) {
                            (firstTriangleID, secondTriangleID) = (secondTriangleID, firstTriangleID);
                        }
                        if (firstTriangleID != -1) {
                            int index1 = _outputTriangleList.FindIndex(obj => obj.TriangleID == _triangleList[firstTriangleID].TriangleID);
                            _outputTriangleList.RemoveAt(index1);
                        }
                        if (secondTriangleID != -1) {
                            int index2 = _outputTriangleList.FindIndex(obj => obj.TriangleID == _triangleList[secondTriangleID].TriangleID);
                            _outputTriangleList.RemoveAt(index2);
                        }
                    }
                    UpdateAllTrackers();
                }
            }
            private void AddEdge(NodeStore node1, NodeStore node2) {
                int edgeID = GetUniqueEdgeID();
                EdgeStore edge = new EdgeStore(edgeID, node1, node2);

                _nodeList[node1.NodeID].AddSortedEdge(edge);
                _nodeList[node2.NodeID].AddSortedEdge(edge);

                _edgeList.Add(edge);
                AddToOutputList(edge, null);
            }
            private void AddEdgeWithTriangle(NodeStore node1, NodeStore node2) {
                int edgeID = GetUniqueEdgeID();
                EdgeStore edge = new EdgeStore(edgeID, node1, node2);

                _nodeList[node1.NodeID].AddSortedEdge(edge);
                _nodeList[node2.NodeID].AddSortedEdge(edge);

                bool triangleFound = false;
                EdgeStore secondEdge = edge.SecondNode.CCVerticalEdge(edge);
                EdgeStore thirdEdge = secondEdge.SecondNode.CCVerticalEdge(secondEdge);

                if (thirdEdge.SecondNode.Equals(edge.FirstNode)) {
                    triangleFound = true;
                } else {
                    secondEdge = edge.SecondNode.CWVerticalEdge(edge);
                    thirdEdge = secondEdge.SecondNode.CWVerticalEdge(secondEdge);
                    if (thirdEdge.SecondNode.Equals(edge.FirstNode)) {
                        triangleFound = true;
                    }
                }
                _edgeList.Add(edge);

                if (triangleFound) {
                    int triangleID = GetUniqueTriangleID();
                    int secondEdgeIndex = _edgeList.FindIndex(obj => obj.EdgeID == secondEdge.EdgeID);
                    int thirdEdgeIndex = _edgeList.FindIndex(obj => obj.EdgeID == thirdEdge.EdgeID);

                    _edgeList[secondEdgeIndex].AddTriangleID(triangleID);
                    _edgeList[thirdEdgeIndex].AddTriangleID(triangleID);
                    _edgeList[_edgeList.Count - 1].AddTriangleID(triangleID);

                    TriangleStore tempTriangle = new TriangleStore(triangleID, edge.FirstNode, edge.SecondNode, secondEdge.GetOtherNode(edge.SecondNode));
                    _triangleList.Add(tempTriangle);

                    _triangleList[_triangleList.Count - 1].FirstEdge = _edgeList[thirdEdgeIndex];
                    _triangleList[_triangleList.Count - 1].SecondEdge = _edgeList[secondEdgeIndex];
                    _triangleList[_triangleList.Count - 1].ThirdEdge = _edgeList[_edgeList.Count - 1];
                }
                AddToOutputList(edge, triangleFound ? _triangleList[_triangleList.Count - 1] : null);
            }
            private void AddTriangle(NodeStore node1, NodeStore node2, NodeStore node3) {
                AddEdge(node1, node2);
                AddEdge(node2, node3);

                if (!Helper.IsCollinear(node1, node2, node3)) {
                    AddEdgeWithTriangle(node3, node1);
                }
            }
            private void RemoveTriangle(int triangleID) {
                _triangleList[triangleID].FirstEdge.RemoveTriangleID(_triangleList[triangleID].TriangleID);
                _triangleList[triangleID].SecondEdge.RemoveTriangleID(_triangleList[triangleID].TriangleID);
                _triangleList[triangleID].ThirdEdge.RemoveTriangleID(_triangleList[triangleID].TriangleID);

                _uniqueTriangleIDList.Add(_triangleList[triangleID].TriangleID);
                _triangleList.RemoveAt(triangleID);
            }
            private void RemoveEdge(int edgeID) {
                int removeIndex = _edgeList.FindIndex(obj => obj.EdgeID == edgeID);

                _nodeList[_edgeList[removeIndex].FirstNode.NodeID].RemoveEdge(_edgeList[removeIndex].EdgeID);
                _nodeList[_edgeList[removeIndex].SecondNode.NodeID].RemoveEdge(_edgeList[removeIndex].EdgeID);

                int triangleIndex1 = _triangleList.FindIndex(obj => obj.TriangleID == _edgeList[removeIndex].FirstTriangleID);
                int triangleIndex2 = -1;

                if (triangleIndex1 == -1) {
                    triangleIndex2 = _triangleList.FindIndex(obj => obj.TriangleID == _edgeList[removeIndex].SecondTriangleID);
                }
                RemoveFromOutputList(_edgeList[removeIndex], triangleIndex1, triangleIndex2);

                if (triangleIndex1 != -1) {
                    RemoveTriangle(triangleIndex1);
                } else if (triangleIndex2 != -1) {
                    RemoveTriangle(triangleIndex2);
                }
                _uniqueEdgeIDList.Add(_edgeList[removeIndex].EdgeID);
                _edgeList.RemoveAt(removeIndex);
            }
            private EdgeStore FindInitialBaseLR(List<NodeStore> leftList, List<NodeStore> rightList) {
                NodeStore leftEnd = leftList[leftList.Count - 1];
                NodeStore rightEnd = rightList[0];

                EdgeStore leftEdgeCWD = leftEnd.CWVerticalEdge(0);
                EdgeStore rightEdgeCCWD = rightEnd.CCVerticalEdge(0);

                while (true) {
                    if (Helper.LeftOf(rightEnd, leftEdgeCWD)) {
                        leftEnd = leftEdgeCWD.GetOtherNode(leftEnd);
                        leftEdgeCWD = leftEnd.CWVerticalEdge(0);
                    } else if (Helper.RightOf(leftEnd, rightEdgeCCWD)) {
                        rightEnd = rightEdgeCCWD.GetOtherNode(rightEnd);
                        rightEdgeCCWD = rightEnd.CCVerticalEdge(0);
                    } else {
                        break;
                    }
                }
                AddEdge(leftEnd, rightEnd);
                return _edgeList[_edgeList.Count - 1];
            }
            private void MergeLREdge(EdgeStore baseLREdge) {
                EdgeStore baseLREdge_symm = new EdgeStore(baseLREdge.EdgeID, baseLREdge.SecondNode, baseLREdge.FirstNode);
                EdgeStore leftCandidate = baseLREdge.FirstNode.CCVerticalEdge(baseLREdge);
                EdgeStore rightCandidate = baseLREdge.SecondNode.CWVerticalEdge(baseLREdge_symm);

                if (Helper.LeftOf(leftCandidate.SecondNode, baseLREdge)) {
                    EdgeStore leftCandidate_next = baseLREdge.FirstNode.CCVerticalEdge(leftCandidate);
                    while (Helper.InCircle(baseLREdge.FirstNode, baseLREdge.SecondNode, leftCandidate.SecondNode, leftCandidate_next.SecondNode)) {
                        EdgeStore leftCandidate_new = baseLREdge.FirstNode.CCVerticalEdge(leftCandidate);
                        RemoveEdge(leftCandidate.EdgeID);
                        leftCandidate = leftCandidate_new;
                        leftCandidate_next = baseLREdge.FirstNode.CCVerticalEdge(leftCandidate);
                    }
                }
                if (Helper.RightOf(rightCandidate.SecondNode, baseLREdge_symm)) {
                    EdgeStore rightCandidate_next = baseLREdge.SecondNode.CWVerticalEdge(rightCandidate);
                    while (Helper.InCircle(baseLREdge_symm.SecondNode, baseLREdge_symm.FirstNode, rightCandidate.SecondNode, rightCandidate_next.SecondNode)) {
                        EdgeStore rightCandidate_new = baseLREdge.SecondNode.CWVerticalEdge(rightCandidate);
                        RemoveEdge(rightCandidate.EdgeID);
                        rightCandidate = rightCandidate_new;
                        rightCandidate_next = baseLREdge.SecondNode.CWVerticalEdge(rightCandidate);
                    }
                }

                bool leftCandidateValidity = Helper.LeftOf(leftCandidate.SecondNode, baseLREdge);
                bool rightCandidateValidity = Helper.RightOf(rightCandidate.SecondNode, baseLREdge_symm);

                if (leftCandidateValidity && rightCandidateValidity) {
                    if (Helper.InCircle(baseLREdge.FirstNode, baseLREdge.SecondNode, leftCandidate.SecondNode, rightCandidate.SecondNode)) {
                        AddEdgeWithTriangle(baseLREdge.FirstNode, rightCandidate.SecondNode);
                    } else {
                        AddEdgeWithTriangle(leftCandidate.SecondNode, baseLREdge_symm.FirstNode);
                    }
                } else if (leftCandidateValidity) {
                    AddEdgeWithTriangle(leftCandidate.SecondNode, baseLREdge_symm.FirstNode);
                } else if (rightCandidateValidity) {
                    AddEdgeWithTriangle(baseLREdge.FirstNode, rightCandidate.SecondNode);
                } else {
                    return;
                }
                EdgeStore baseLREdge_new = _edgeList[_edgeList.Count - 1];
                MergeLREdge(baseLREdge_new);
            }
            private void DelaunayTriangulationDivideAndConquer(List<NodeStore> leftList, List<NodeStore> rightList) {
                if (leftList.Count > 3) {
                    int halfLength = Convert.ToInt32(Math.Ceiling(leftList.Count / 2.0));
                    List<NodeStore> leftLeftList = leftList.GetRange(0, halfLength);
                    List<NodeStore> leftRightList = leftList.GetRange(halfLength, leftList.Count - halfLength);
                    DelaunayTriangulationDivideAndConquer(leftLeftList, leftRightList);
                } else if (leftList.Count == 3) {
                    AddTriangle(leftList[0], leftList[1], leftList[2]);
                } else if (leftList.Count == 2) {
                    AddEdge(leftList[0], leftList[1]);
                }
                if (rightList.Count > 3) {
                    int halfLength = Convert.ToInt32(Math.Ceiling(rightList.Count / 2.0));
                    List<NodeStore> rightLeftList = rightList.GetRange(0, halfLength);
                    List<NodeStore> rightRightList = rightList.GetRange(halfLength, rightList.Count - halfLength);
                    DelaunayTriangulationDivideAndConquer(rightLeftList, rightRightList);
                } else if (rightList.Count == 3) {
                    AddTriangle(rightList[0], rightList[1], rightList[2]);
                } else if (rightList.Count == 2) {
                    AddEdge(rightList[0], rightList[1]);
                }
                EdgeStore baseLREdge = FindInitialBaseLR(leftList, rightList);
                MergeLREdge(baseLREdge);
            }
            #endregion


        }
    }
}
