using static MastersThesis.MainForm.PlanarObjectStore;
using static MastersThesis.Triangulation.MeshStore;

namespace MastersThesis {
    internal partial class Triangulation {

        #region Greedy Triangulation, Delaunay Triangulation Mesh Refinement start functions
        internal void GreedyTriangulationStart(List<Node> inputNodeList, ref List<Edge> outputEdgeList, ref List<TweenAnimation> outputTrackerList) {

            List<MeshStore.NodeStore> nodeList = new List<MeshStore.NodeStore>();
            int index = 0;
            foreach (Node node in inputNodeList) {
                nodeList.Add(new MeshStore.NodeStore(index, node.XCoordinate, node.YCoordinate, node));
                index++;
            }
            MeshStore meshStoreInstance = new MeshStore(nodeList, ref outputEdgeList, ref outputTrackerList);
        }

        internal void DelaunayTriangulationStart(List<MainForm.PlanarObjectStore.Node> inputNodeList, ref List<MainForm.PlanarObjectStore.Edge> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.Triangle> outputTriangleList, ref List<MainForm.PlanarObjectStore.TweenAnimation> outputTrackerList,
                        ref List<NodeStore> nodeStoreList, ref List<MeshStore.EdgeStore> edgeStoreList, ref List<MeshStore.TriangleStore> triangleStoreList) {

            List<MainForm.PlanarObjectStore.Node> sortedInputNodeList = inputNodeList.OrderBy(obj => obj.XCoordinate).ThenBy(obj => obj.YCoordinate).ToList();
            List<MeshStore.NodeStore> nodeList = new List<MeshStore.NodeStore>();
            int index = 0;
            foreach (MainForm.PlanarObjectStore.Node node in sortedInputNodeList) {
                nodeList.Add(new MeshStore.NodeStore(index, node.XCoordinate, node.YCoordinate, node));
                index++;
            }
            MeshStore meshStoreInstance = new MeshStore(nodeList, ref outputEdgeList, ref outputTriangleList, ref outputTrackerList,
                                                        ref nodeStoreList, ref edgeStoreList, ref triangleStoreList);
        }

        internal void MeshRefinementStart(ref List<MainForm.PlanarObjectStore.Node> outputNodeList, ref List<MainForm.PlanarObjectStore.Edge> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.Triangle> outputTriangleList, ref List<MainForm.PlanarObjectStore.TweenAnimation> outputTrackerList,
                        MainForm.PlanarObjectStore.TweenAnimation tempTracker, int decimalPlaces, int meshRefinementCoeff, List<MeshStore.NodeStore> nodeStoreList,
                        List<MeshStore.EdgeStore> edgeStoreList, List<MeshStore.TriangleStore> triangleStoreList) {

            MeshStore meshStoreInstance = new MeshStore(ref outputNodeList, ref outputEdgeList, ref outputTriangleList, ref outputTrackerList,
                                                        tempTracker, decimalPlaces, meshRefinementCoeff, nodeStoreList, edgeStoreList, triangleStoreList);
        }
        #endregion


        internal partial class MeshStore {
            private List<Node> _outputNodeList = new List<Node>();
            private List<Edge> _outputEdgeList = new List<Edge>();
            private List<Triangle> _outputTriangleList = new List<Triangle>();
            private List<TweenAnimation> _outputTrackerList = new List<TweenAnimation>();

            private List<NodeStore> _nodeList = new List<NodeStore>();
            private List<EdgeStore> _edgeList = new List<EdgeStore>();
            private List<TriangleStore> _triangleList = new List<TriangleStore>();

            private List<int> _uniqueEdgeIDList = new List<int>();
            private List<int> _uniqueTriangleIDList = new List<int>();


            #region MeshStore constructors: Greedy Triangulation, Delaunay Triangulation, Mesh Refinement
            internal MeshStore(List<NodeStore> inputNodeList, ref List<MainForm.PlanarObjectStore.Edge> outputEdgeList,
                        ref List<TweenAnimation> outputTrackerList) {

                outputEdgeList = new List<Edge>();
                outputTrackerList = new List<MainForm.PlanarObjectStore.TweenAnimation>();
                this._nodeList = inputNodeList;

                GreedyTriangulation();

                outputEdgeList = this._outputEdgeList;
                outputTrackerList = this._outputTrackerList;
            }

            internal MeshStore(List<NodeStore> inputNodeList, ref List<MainForm.PlanarObjectStore.Edge> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.Triangle> outputTriangleList, ref List<MainForm.PlanarObjectStore.TweenAnimation> outputTrackerList,
                        ref List<NodeStore> nodeStoreList, ref List<EdgeStore> edgeStoreList, ref List<TriangleStore> triangleStoreList) {

                outputEdgeList = new List<MainForm.PlanarObjectStore.Edge>();
                outputTriangleList = new List<MainForm.PlanarObjectStore.Triangle>();
                outputTrackerList = new List<MainForm.PlanarObjectStore.TweenAnimation>();

                this._nodeList = inputNodeList;

                if (this._nodeList.Count > 3) {
                    int halfLength = Convert.ToInt32(Math.Ceiling(this._nodeList.Count / 2.0f));
                    List<NodeStore> leftList = this._nodeList.GetRange(0, halfLength);
                    List<NodeStore> rightList = this._nodeList.GetRange(halfLength, this._nodeList.Count - halfLength);
                    DelaunayTriangulationDivideAndConquer(leftList, rightList);
                } else if (this._nodeList.Count == 3) {
                    AddTriangle(this._nodeList[0], this._nodeList[1], this._nodeList[2]);
                } else if (this._nodeList.Count == 2) {
                    AddEdge(this._nodeList[0], this._nodeList[1]);
                }

                outputEdgeList = this._outputEdgeList;
                outputTriangleList = this._outputTriangleList;
                outputTrackerList = this._outputTrackerList;

                nodeStoreList = this._nodeList;
                edgeStoreList = this._edgeList;
                triangleStoreList = this._triangleList;
            }

            internal MeshStore(ref List<MainForm.PlanarObjectStore.Node> outputNodeList, ref List<MainForm.PlanarObjectStore.Edge> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.Triangle> outputTriangleList, ref List<MainForm.PlanarObjectStore.TweenAnimation> outputTrackerList,
                        MainForm.PlanarObjectStore.TweenAnimation tempTracker, int decimalPlaces, int meshRefinementCoeff, List<NodeStore> nodeStoreList_extracted,
                        List<EdgeStore> edgeStoreList_extracted, List<TriangleStore> triangleStoreList_extracted) {

                this._nodeList = nodeStoreList_extracted.ToList();
                this._edgeList = edgeStoreList_extracted.ToList();
                this._triangleList = triangleStoreList_extracted.ToList();

                foreach (NodeStore node in nodeStoreList_extracted) {
                    this._outputNodeList.Add(new MainForm.PlanarObjectStore.Node(node.NodeID, node.XCoordinate, node.YCoordinate));
                }
                this._outputTrackerList.Add(tempTracker);

                MeshRefinement(decimalPlaces, meshRefinementCoeff, triangleStoreList_extracted);

                outputNodeList = this._outputNodeList;
                outputEdgeList = this._outputEdgeList;
                outputTriangleList = this._outputTriangleList;
                outputTrackerList = this._outputTrackerList;
            }
            #endregion

            #region MeshStore methods: Greedy Triangulation  
            private void AddEdgeForGreedyTriangulation(NodeStore node1, NodeStore node2) {
                int edgeID = GetUniqueEdgeID();
                EdgeStore edge = new EdgeStore(edgeID, node1, node2);
                this._edgeList.Add(edge);
                this._outputEdgeList.Add(new MainForm.PlanarObjectStore.Edge(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData));
                UpdateParticalTrackers();
            }
            private void RemoveFromOutputListForGreedyTriangulation(EdgeStore edge) {
                MainForm.PlanarObjectStore.Edge tempEdge = new MainForm.PlanarObjectStore.Edge(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData);
                int removeIndex = this._outputEdgeList.FindIndex(obj => obj.CommutativeEquals(tempEdge));
                if (removeIndex != -1) {
                    this._outputEdgeList.RemoveAt(removeIndex);
                    UpdateParticalTrackers();
                }
            }
            private void RemoveEdgeForGreedyTriangulation(int edgeID) {
                int removeIndex = this._edgeList.FindIndex(obj => obj.EdgeID == edgeID);

                this._nodeList[this._edgeList[removeIndex].FirstNode.NodeID].RemoveEdge(this._edgeList[removeIndex].EdgeID);
                this._nodeList[this._edgeList[removeIndex].SecondNode.NodeID].RemoveEdge(this._edgeList[removeIndex].EdgeID);
                RemoveFromOutputListForGreedyTriangulation(this._edgeList[removeIndex]);

                this._uniqueEdgeIDList.Add(this._edgeList[removeIndex].EdgeID);
                this._edgeList.RemoveAt(removeIndex);
            }
            private void GreedyTriangulation() {
#warning To do: remove from the final version
#warning true -> draw only correct edges order by edge length; false -> draw all possible edges and remove incorrect edges
#if true
                List<EdgeStore> tempEdgeList = new List<EdgeStore>();
                for (int j = 1; j < this._nodeList.Count; j++) {
                    for (int k = 0; k < j; k++) {
                        NodeStore tempNode1 = this._nodeList[j];
                        NodeStore tempNode2 = this._nodeList[k];
                        EdgeStore tempEdge = new EdgeStore(-2, tempNode1, tempNode2);

                        if (tempEdgeList.FindIndex(obj => obj.CommutativeEquals(tempEdge)) == -1) {
                            tempEdgeList.Add(tempEdge);
                        }
                    }
                }
                tempEdgeList = tempEdgeList.OrderBy(obj => obj.GetEdgeLegth).ToList();
                AddEdgeForGreedyTriangulation(tempEdgeList[0].FirstNode, tempEdgeList[0].SecondNode);

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
                        AddEdgeForGreedyTriangulation(tempEdgeList[j].FirstNode, tempEdgeList[j].SecondNode);
                    }
                    checkEdgeIntersection = false;
                }
#else
                for (int j = 1; j < this._nodeList.Count; j++) {
                    for (int k = 0; k < j; k++) {
                        Node2DStore tempNode1 = this._nodeList[j];
                        Node2DStore tempNode2 = this._nodeList[k];
                        Edge2DStore tempEdge = new Edge2DStore(-2, tempNode1, tempNode2);

                        if (this._edgeList.FindIndex(obj => obj.CommutativeEquals(tempEdge)) == -1) {
                            AddEdgeForGreedyTriangulation(tempNode1, tempNode2);
                        }
                    }
                }
                this._edgeList = this._edgeList.OrderBy(obj => obj.GetEdgeLegth).ToList();
                for (int j = 1; j < this._edgeList.Count; j++) {
                    for (int k = 0; k < j; k++) {
                        if (Helper.CheckEdgeIntersection(this._edgeList[j], this._edgeList[k])) {
                            RemoveEdgeForGreedyTriangulation(this._edgeList[j].EdgeID);
                            j--;
                            break;
                        }
                    }
                }
#endif
            }
            #endregion

            #region MeshStore methods: Mesh Refinement
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
                    MainForm.PlanarObjectStore.Node intermediateNode =
                        new MainForm.PlanarObjectStore.Node(nextNodeId, Math.Round(firstNode.XCoordinate + j * stepByXCoord, decimalPlaces, MidpointRounding.AwayFromZero),
                                                              Math.Round(firstNode.YCoordinate + j * stepByYCoord, decimalPlaces, MidpointRounding.AwayFromZero));

                    NodeStore intermediateNodeStore = new NodeStore(intermediateNode.NodeID, intermediateNode.XCoordinate, intermediateNode.YCoordinate, intermediateNode);

                    if (this._nodeList.FindIndex(obj => obj.Equals(intermediateNodeStore)) == -1) {
                        this._outputNodeList.Add(intermediateNode);
                        this._nodeList.Add(intermediateNodeStore);
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
                    MainForm.PlanarObjectStore.Node intermediateNode =
                        new MainForm.PlanarObjectStore.Node(nextNodeId, Math.Round(firstNode.XCoordinate + j * stepByXCoord, decimalPlaces, MidpointRounding.AwayFromZero),
                                                              Math.Round(firstNode.YCoordinate + j * stepByYCoord, decimalPlaces, MidpointRounding.AwayFromZero));

                    NodeStore intermediateNodeStore = new NodeStore(intermediateNode.NodeID, intermediateNode.XCoordinate, intermediateNode.YCoordinate, intermediateNode);

                    if (this._nodeList.FindIndex(obj => obj.Equals(intermediateNodeStore)) == -1) {
                        this._outputNodeList.Add(intermediateNode);
                        this._nodeList.Add(intermediateNodeStore);
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

                if (this._edgeList.FindIndex(obj => obj.CommutativeEquals(edge)) == -1) {
                    this._edgeList.Add(edge);
                    this._outputEdgeList.Add(new MainForm.PlanarObjectStore.Edge(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData));
                    UpdateAllTrackers();
                }
            }
            private void MeshRefinement(int decimalPlaces, int meshRefinementCoeff, List<TriangleStore> triangleStoreList_extracted) {
                int nextNodeId = this._nodeList.Count;
                int nextTriangleId = this._triangleList.Count;

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
                    this._outputTriangleList.Add(new MainForm.PlanarObjectStore.Triangle(nextTriangleId, prevList[0].GetNodeData, curList[0].GetNodeData, curList[1].GetNodeData));
                    UpdateAllTrackers();
                    nextTriangleId++;

                    for (int j = 1; j < meshRefinementCoeff; j++) {
                        prevList = curList.ToList();
                        curList = GetIntermediateNodes(tuple1.Item2[j + 1], tuple2.Item2[j + 1], decimalPlaces, j + 1, ref nextNodeId);
                        for (int k = 0; k < prevList.Count; k++) {
                            AddEdgeForMeshRefinement(prevList[k], curList[k]);
                            AddEdgeForMeshRefinement(curList[k], curList[k + 1]);
                            AddEdgeForMeshRefinement(curList[k + 1], prevList[k]);
                            this._outputTriangleList.Add(new MainForm.PlanarObjectStore.Triangle(nextTriangleId, prevList[k].GetNodeData, curList[k].GetNodeData, curList[k + 1].GetNodeData));
                            UpdateAllTrackers();
                            nextTriangleId++;
                            if (k < prevList.Count - 1) {
                                this._outputTriangleList.Add(new MainForm.PlanarObjectStore.Triangle(nextTriangleId, prevList[k].GetNodeData, curList[k + 1].GetNodeData, prevList[k + 1].GetNodeData));
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
                MainForm.PlanarObjectStore.TweenAnimation tempTracker = new MainForm.PlanarObjectStore.TweenAnimation();
                tempTracker.EdgeList = new List<MainForm.PlanarObjectStore.Edge>();
                tempTracker.EdgeList.AddRange(this._outputEdgeList);
                this._outputTrackerList.Add(tempTracker);
            }
            private void UpdateAllTrackers() {
                MainForm.PlanarObjectStore.TweenAnimation tempTracker = new MainForm.PlanarObjectStore.TweenAnimation();
                tempTracker.EdgeList = new List<MainForm.PlanarObjectStore.Edge>();
                tempTracker.EdgeList.AddRange(this._outputEdgeList);
                tempTracker.TriangleList = new List<MainForm.PlanarObjectStore.Triangle>();
                tempTracker.TriangleList.AddRange(this._outputTriangleList);
                this._outputTrackerList.Add(tempTracker);
            }
            private int GetUniqueEdgeID() {
                int edgeID;
                if (this._uniqueEdgeIDList.Count != 0) {
                    edgeID = this._uniqueEdgeIDList[0];
                    this._uniqueEdgeIDList.RemoveAt(0);
                } else {
                    edgeID = this._edgeList.Count;
                }
                return edgeID;
            }
            private int GetUniqueTriangleID() {
                int triangleID;
                if (this._uniqueTriangleIDList.Count != 0) {
                    triangleID = this._uniqueTriangleIDList[0];
                    this._uniqueTriangleIDList.RemoveAt(0);
                } else {
                    triangleID = this._triangleList.Count;
                }
                return triangleID;
            }
            private void AddToOutputList(EdgeStore edge, TriangleStore triangle) {
                MainForm.PlanarObjectStore.Edge tempEdge = new MainForm.PlanarObjectStore.Edge(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData);
                this._outputEdgeList.Add(tempEdge);
                if (triangle != null) {
                    MainForm.PlanarObjectStore.Triangle tempTriangle =
                        new MainForm.PlanarObjectStore.Triangle(triangle.TriangleID, triangle.FirstNode.GetNodeData, triangle.SecondNode.GetNodeData, triangle.ThirdNode.GetNodeData);
                    this._outputTriangleList.Add(tempTriangle);
                }
                UpdateAllTrackers();
            }
            private void RemoveFromOutputList(EdgeStore edge, int firstTriangleID, int secondTriangleID) {
                MainForm.PlanarObjectStore.Edge tempEdge = new MainForm.PlanarObjectStore.Edge(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData);
                int removeIndex = this._outputEdgeList.FindIndex(obj => obj.CommutativeEquals(tempEdge));
                if (removeIndex != -1) {
                    this._outputEdgeList.RemoveAt(removeIndex);
                    if (firstTriangleID != -1 || secondTriangleID != -1) {
                        if (firstTriangleID < secondTriangleID) {
                            (firstTriangleID, secondTriangleID) = (secondTriangleID, firstTriangleID);
                        }
                        if (firstTriangleID != -1) {
                            int index1 = this._outputTriangleList.FindIndex(obj => obj.TriangleID == this._triangleList[firstTriangleID].TriangleID);
                            this._outputTriangleList.RemoveAt(index1);
                        }
                        if (secondTriangleID != -1) {
                            int index2 = this._outputTriangleList.FindIndex(obj => obj.TriangleID == this._triangleList[secondTriangleID].TriangleID);
                            this._outputTriangleList.RemoveAt(index2);
                        }
                    }
                    UpdateAllTrackers();
                }
            }
            private void AddEdge(NodeStore node1, NodeStore node2) {
                int edgeID = GetUniqueEdgeID();
                EdgeStore edge = new EdgeStore(edgeID, node1, node2);

                this._nodeList[node1.NodeID].AddSortedEdge(edge);
                this._nodeList[node2.NodeID].AddSortedEdge(edge);

                this._edgeList.Add(edge);
                AddToOutputList(edge, null);
            }
            private void AddEdgeWithTriangle(NodeStore node1, NodeStore node2) {
                int edgeID = GetUniqueEdgeID();
                EdgeStore edge = new EdgeStore(edgeID, node1, node2);

                this._nodeList[node1.NodeID].AddSortedEdge(edge);
                this._nodeList[node2.NodeID].AddSortedEdge(edge);

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
                this._edgeList.Add(edge);

                if (triangleFound) {
                    int triangleID = GetUniqueTriangleID();
                    int secondEdgeIndex = this._edgeList.FindIndex(obj => obj.EdgeID == secondEdge.EdgeID);
                    int thirdEdgeIndex = this._edgeList.FindIndex(obj => obj.EdgeID == thirdEdge.EdgeID);

                    this._edgeList[secondEdgeIndex].AddTriangleID(triangleID);
                    this._edgeList[thirdEdgeIndex].AddTriangleID(triangleID);
                    this._edgeList[this._edgeList.Count - 1].AddTriangleID(triangleID);

                    TriangleStore tempTriangle = new TriangleStore(triangleID, edge.FirstNode, edge.SecondNode, secondEdge.GetOtherNode(edge.SecondNode));
                    this._triangleList.Add(tempTriangle);

                    this._triangleList[this._triangleList.Count - 1].FirstEdge = this._edgeList[thirdEdgeIndex];
                    this._triangleList[this._triangleList.Count - 1].SecondEdge = this._edgeList[secondEdgeIndex];
                    this._triangleList[this._triangleList.Count - 1].ThirdEdge = this._edgeList[this._edgeList.Count - 1];
                }
                AddToOutputList(edge, triangleFound ? this._triangleList[this._triangleList.Count - 1] : null);
            }
            private void AddTriangle(NodeStore node1, NodeStore node2, NodeStore node3) {
                AddEdge(node1, node2);
                AddEdge(node2, node3);

                if (!Helper.IsCollinear(node1, node2, node3)) {
                    AddEdgeWithTriangle(node3, node1);
                }
            }
            private void RemoveTriangle(int triangleID) {
                this._triangleList[triangleID].FirstEdge.RemoveTriangleID(this._triangleList[triangleID].TriangleID);
                this._triangleList[triangleID].SecondEdge.RemoveTriangleID(this._triangleList[triangleID].TriangleID);
                this._triangleList[triangleID].ThirdEdge.RemoveTriangleID(this._triangleList[triangleID].TriangleID);

                this._uniqueTriangleIDList.Add(this._triangleList[triangleID].TriangleID);
                this._triangleList.RemoveAt(triangleID);
            }
            private void RemoveEdge(int edgeID) {
                int removeIndex = this._edgeList.FindIndex(obj => obj.EdgeID == edgeID);

                this._nodeList[this._edgeList[removeIndex].FirstNode.NodeID].RemoveEdge(this._edgeList[removeIndex].EdgeID);
                this._nodeList[this._edgeList[removeIndex].SecondNode.NodeID].RemoveEdge(this._edgeList[removeIndex].EdgeID);

                int triangleIndex1 = this._triangleList.FindIndex(obj => obj.TriangleID == this._edgeList[removeIndex].GetFirstTriangleID);
                int triangleIndex2 = -1;

                if (triangleIndex1 == -1) {
                    triangleIndex2 = this._triangleList.FindIndex(obj => obj.TriangleID == this._edgeList[removeIndex].GetSecondTriangleID);
                }
                RemoveFromOutputList(this._edgeList[removeIndex], triangleIndex1, triangleIndex2);

                if (triangleIndex1 != -1) {
                    RemoveTriangle(triangleIndex1);
                } else if (triangleIndex2 != -1) {
                    RemoveTriangle(triangleIndex2);
                }
                this._uniqueEdgeIDList.Add(this._edgeList[removeIndex].EdgeID);
                this._edgeList.RemoveAt(removeIndex);
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
                return this._edgeList[this._edgeList.Count - 1];
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
                EdgeStore baseLREdge_new = this._edgeList[this._edgeList.Count - 1];
                MergeLREdge(baseLREdge_new);
            }
            private void DelaunayTriangulationDivideAndConquer(List<NodeStore> leftList, List<NodeStore> rightList) {
                if (leftList.Count > 3) {
                    int halfLength = Convert.ToInt32(Math.Ceiling(leftList.Count / 2.0f));
                    List<NodeStore> leftLeftList = leftList.GetRange(0, halfLength);
                    List<NodeStore> leftRightList = leftList.GetRange(halfLength, leftList.Count - halfLength);
                    DelaunayTriangulationDivideAndConquer(leftLeftList, leftRightList);
                } else if (leftList.Count == 3) {
                    AddTriangle(leftList[0], leftList[1], leftList[2]);
                } else if (leftList.Count == 2) {
                    AddEdge(leftList[0], leftList[1]);
                }
                if (rightList.Count > 3) {
                    int halfLength = Convert.ToInt32(Math.Ceiling(rightList.Count / 2.0f));
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
