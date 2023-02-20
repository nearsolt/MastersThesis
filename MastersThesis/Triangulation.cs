namespace MastersThesis {
    internal class Triangulation {

        #region Greedy Triangulation, Delaunay Triangulation Mesh Refinement functions
        internal void GreedyTriangulationStart(List<MainForm.PlanarObjectStore.Node2D> inputNodeList, ref List<MainForm.PlanarObjectStore.Edge2D> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.AnimationTracker> outputTrackerList) {

            List<MeshStore.Node2DStore> nodeList = new List<MeshStore.Node2DStore>();
            int index = 0;
            foreach (MainForm.PlanarObjectStore.Node2D node in inputNodeList) {
                nodeList.Add(new MeshStore.Node2DStore(index, node.XCoordinate, node.YCoordinate, node));
                index++;
            }
            MeshStore meshStoreInstance = new MeshStore(nodeList, ref outputEdgeList, ref outputTrackerList);
        }

        internal void DelaunayTriangulationStart(List<MainForm.PlanarObjectStore.Node2D> inputNodeList, ref List<MainForm.PlanarObjectStore.Edge2D> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.Triangle2D> outputTriangleList, ref List<MainForm.PlanarObjectStore.AnimationTracker> outputTrackerList,
                        ref List<MeshStore.Node2DStore> nodeStoreList, ref List<MeshStore.Edge2DStore> edgeStoreList, ref List<MeshStore.Triangle2DStore> triangleStoreList) {

            List<MainForm.PlanarObjectStore.Node2D> sortedInputNodeList = inputNodeList.OrderBy(obj => obj.XCoordinate).ThenBy(obj => obj.YCoordinate).ToList();
            List<MeshStore.Node2DStore> nodeList = new List<MeshStore.Node2DStore>();
            int index = 0;
            foreach (MainForm.PlanarObjectStore.Node2D node in sortedInputNodeList) {
                nodeList.Add(new MeshStore.Node2DStore(index, node.XCoordinate, node.YCoordinate, node));
                index++;
            }
            MeshStore meshStoreInstance = new MeshStore(nodeList, ref outputEdgeList, ref outputTriangleList, ref outputTrackerList,
                                                        ref nodeStoreList, ref edgeStoreList, ref triangleStoreList);
        }

        internal void MeshRefinementStart(ref List<MainForm.PlanarObjectStore.Node2D> outputNodeList, ref List<MainForm.PlanarObjectStore.Edge2D> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.Triangle2D> outputTriangleList, ref List<MainForm.PlanarObjectStore.AnimationTracker> outputTrackerList,
                        MainForm.PlanarObjectStore.AnimationTracker tempTracker, int decimalPlaces, int meshRefinementCoeff, List<MeshStore.Node2DStore> nodeStoreList,
                        List<MeshStore.Edge2DStore> edgeStoreList, List<MeshStore.Triangle2DStore> triangleStoreList) {

            MeshStore meshStoreInstance = new MeshStore(ref outputNodeList, ref outputEdgeList, ref outputTriangleList, ref outputTrackerList,
                                                        tempTracker, decimalPlaces, meshRefinementCoeff, nodeStoreList, edgeStoreList, triangleStoreList);
        }
        #endregion


        internal class MeshStore {
            private List<MainForm.PlanarObjectStore.Node2D> _outputNodeList = new List<MainForm.PlanarObjectStore.Node2D>();
            private List<MainForm.PlanarObjectStore.Edge2D> _outputEdgeList = new List<MainForm.PlanarObjectStore.Edge2D>();
            private List<MainForm.PlanarObjectStore.Triangle2D> _outputTriangleList = new List<MainForm.PlanarObjectStore.Triangle2D>();
            private List<MainForm.PlanarObjectStore.AnimationTracker> _outputTrackerList = new List<MainForm.PlanarObjectStore.AnimationTracker>();

            private List<Node2DStore> _nodeList = new List<Node2DStore>();
            private List<Edge2DStore> _edgeList = new List<Edge2DStore>();
            private List<Triangle2DStore> _triangleList = new List<Triangle2DStore>();

            private List<int> _uniqueEdgeIDList = new List<int>();
            private List<int> _uniqueTriangleIDList = new List<int>();


            #region MeshStore constructors: Greedy Triangulation, Delaunay Triangulation, Mesh Refinement
            internal MeshStore(List<Node2DStore> inputNodeList, ref List<MainForm.PlanarObjectStore.Edge2D> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.AnimationTracker> outputTrackerList) {

                outputEdgeList = new List<MainForm.PlanarObjectStore.Edge2D>();
                outputTrackerList = new List<MainForm.PlanarObjectStore.AnimationTracker>();
                this._nodeList = inputNodeList;

                GreedyTriangulation();

                outputEdgeList = this._outputEdgeList;
                outputTrackerList = this._outputTrackerList;
            }

            internal MeshStore(List<Node2DStore> inputNodeList, ref List<MainForm.PlanarObjectStore.Edge2D> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.Triangle2D> outputTriangleList, ref List<MainForm.PlanarObjectStore.AnimationTracker> outputTrackerList,
                        ref List<Node2DStore> nodeStoreList, ref List<Edge2DStore> edgeStoreList, ref List<Triangle2DStore> triangleStoreList) {

                outputEdgeList = new List<MainForm.PlanarObjectStore.Edge2D>();
                outputTriangleList = new List<MainForm.PlanarObjectStore.Triangle2D>();
                outputTrackerList = new List<MainForm.PlanarObjectStore.AnimationTracker>();

                this._nodeList = inputNodeList;

                if (this._nodeList.Count > 3) {
                    int halfLength = Convert.ToInt32(Math.Ceiling(this._nodeList.Count / 2.0f));
                    List<Node2DStore> leftList = this._nodeList.GetRange(0, halfLength);
                    List<Node2DStore> rightList = this._nodeList.GetRange(halfLength, this._nodeList.Count - halfLength);
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

            internal MeshStore(ref List<MainForm.PlanarObjectStore.Node2D> outputNodeList, ref List<MainForm.PlanarObjectStore.Edge2D> outputEdgeList,
                        ref List<MainForm.PlanarObjectStore.Triangle2D> outputTriangleList, ref List<MainForm.PlanarObjectStore.AnimationTracker> outputTrackerList,
                        MainForm.PlanarObjectStore.AnimationTracker tempTracker, int decimalPlaces, int meshRefinementCoeff, List<Node2DStore> nodeStoreList_extracted,
                        List<Edge2DStore> edgeStoreList_extracted, List<Triangle2DStore> triangleStoreList_extracted) {

                this._nodeList = nodeStoreList_extracted.ToList();
                this._edgeList = edgeStoreList_extracted.ToList();
                this._triangleList = triangleStoreList_extracted.ToList();

                foreach (Node2DStore node in nodeStoreList_extracted) {
                    this._outputNodeList.Add(new MainForm.PlanarObjectStore.Node2D(node.NodeID, node.XCoordinate, node.YCoordinate));
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
            private void AddEdgeForGreedyTriangulation(Node2DStore node1, Node2DStore node2) {
                int edgeID = GetUniqueEdgeID();
                Edge2DStore edge = new Edge2DStore(edgeID, node1, node2);
                this._edgeList.Add(edge);
                this._outputEdgeList.Add(new MainForm.PlanarObjectStore.Edge2D(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData));
                UpdateParticalTrackers();
            }
            private void RemoveFromOutputListForGreedyTriangulation(Edge2DStore edge) {
                MainForm.PlanarObjectStore.Edge2D tempEdge = new MainForm.PlanarObjectStore.Edge2D(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData);
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
                List<Edge2DStore> tempEdgeList = new List<Edge2DStore>();
                for (int j = 1; j < this._nodeList.Count; j++) {
                    for (int k = 0; k < j; k++) {
                        Node2DStore tempNode1 = this._nodeList[j];
                        Node2DStore tempNode2 = this._nodeList[k];
                        Edge2DStore tempEdge = new Edge2DStore(-2, tempNode1, tempNode2);

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
            private void GetIntermediateNodes(Tuple<Edge2DStore, List<Node2DStore>, bool> tuple, int decimalPlaces, int meshRefinementCoeff, ref int nextNodeId) {
                Node2DStore firstNode;
                Node2DStore secondNode;
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
                    MainForm.PlanarObjectStore.Node2D intermediateNode =
                        new MainForm.PlanarObjectStore.Node2D(nextNodeId, Math.Round(firstNode.XCoordinate + j * stepByXCoord, decimalPlaces, MidpointRounding.AwayFromZero),
                                                              Math.Round(firstNode.YCoordinate + j * stepByYCoord, decimalPlaces, MidpointRounding.AwayFromZero));

                    Node2DStore intermediateNodeStore = new Node2DStore(intermediateNode.NodeID, intermediateNode.XCoordinate, intermediateNode.YCoordinate, intermediateNode);

                    if (this._nodeList.FindIndex(obj => obj.Equals(intermediateNodeStore)) == -1) {
                        this._outputNodeList.Add(intermediateNode);
                        this._nodeList.Add(intermediateNodeStore);
                        nextNodeId++;
                    }
                    tuple.Item2.Add(intermediateNodeStore);
                }
                tuple.Item2.Add(secondNode);
            }
            private List<Node2DStore> GetIntermediateNodes(Node2DStore firstNode, Node2DStore secondNode, int decimalPlaces, int meshRefinementCoeff, ref int nextNodeId) {
                double stepByXCoord = (secondNode.XCoordinate - firstNode.XCoordinate) / meshRefinementCoeff;
                double stepByYCoord = (secondNode.YCoordinate - firstNode.YCoordinate) / meshRefinementCoeff;

                List<Node2DStore> tempNodeList = new List<Node2DStore>();
                tempNodeList.Add(firstNode);

                for (int j = 1; j < meshRefinementCoeff; j++) {
                    MainForm.PlanarObjectStore.Node2D intermediateNode =
                        new MainForm.PlanarObjectStore.Node2D(nextNodeId, Math.Round(firstNode.XCoordinate + j * stepByXCoord, decimalPlaces, MidpointRounding.AwayFromZero),
                                                              Math.Round(firstNode.YCoordinate + j * stepByYCoord, decimalPlaces, MidpointRounding.AwayFromZero));

                    Node2DStore intermediateNodeStore = new Node2DStore(intermediateNode.NodeID, intermediateNode.XCoordinate, intermediateNode.YCoordinate, intermediateNode);

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
            private void AddEdgeForMeshRefinement(Node2DStore node1, Node2DStore node2) {
                int edgeID = GetUniqueEdgeID();
                Edge2DStore edge = new Edge2DStore(edgeID, node1, node2);

                if (this._edgeList.FindIndex(obj => obj.CommutativeEquals(edge)) == -1) {
                    this._edgeList.Add(edge);
                    this._outputEdgeList.Add(new MainForm.PlanarObjectStore.Edge2D(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData));
                    UpdateAllTrackers();
                }
            }
            private void MeshRefinement(int decimalPlaces, int meshRefinementCoeff, List<Triangle2DStore> triangleStoreList_extracted) {
                int nextNodeId = this._nodeList.Count;
                int nextTriangleId = this._triangleList.Count;

                foreach (Triangle2DStore triangle in triangleStoreList_extracted) {
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
                    Tuple<Edge2DStore, List<Node2DStore>, bool> tuple1 = new Tuple<Edge2DStore, List<Node2DStore>, bool>(triangle.FirstEdge, new List<Node2DStore>(), reverseForEdge1);
                    GetIntermediateNodes(tuple1, decimalPlaces, meshRefinementCoeff, ref nextNodeId);

                    Tuple<Edge2DStore, List<Node2DStore>, bool> tuple2 = new Tuple<Edge2DStore, List<Node2DStore>, bool>(triangle.SecondEdge, new List<Node2DStore>(), reverseForEdge2);
                    GetIntermediateNodes(tuple2, decimalPlaces, meshRefinementCoeff, ref nextNodeId);

                    List<Node2DStore> prevList = new List<Node2DStore>();
                    List<Node2DStore> curList = new List<Node2DStore>();
                    //j=-1
                    curList.Add(tuple1.Item2[0]);
                    //j=0
                    prevList = curList.ToList();
                    curList = new List<Node2DStore>() { tuple1.Item2[1], tuple2.Item2[1] };
                    AddEdgeForMeshRefinement(prevList[0], curList[0]);
                    AddEdgeForMeshRefinement(curList[0], curList[1]);
                    AddEdgeForMeshRefinement(curList[1], prevList[0]);
                    this._outputTriangleList.Add(new MainForm.PlanarObjectStore.Triangle2D(nextTriangleId, prevList[0].GetNodeData, curList[0].GetNodeData, curList[1].GetNodeData));
                    UpdateAllTrackers();
                    nextTriangleId++;

                    for (int j = 1; j < meshRefinementCoeff; j++) {
                        prevList = curList.ToList();
                        curList = GetIntermediateNodes(tuple1.Item2[j + 1], tuple2.Item2[j + 1], decimalPlaces, j + 1, ref nextNodeId);
                        for (int k = 0; k < prevList.Count; k++) {
                            AddEdgeForMeshRefinement(prevList[k], curList[k]);
                            AddEdgeForMeshRefinement(curList[k], curList[k + 1]);
                            AddEdgeForMeshRefinement(curList[k + 1], prevList[k]);
                            this._outputTriangleList.Add(new MainForm.PlanarObjectStore.Triangle2D(nextTriangleId, prevList[k].GetNodeData, curList[k].GetNodeData, curList[k + 1].GetNodeData));
                            UpdateAllTrackers();
                            nextTriangleId++;
                            if (k < prevList.Count - 1) {
                                this._outputTriangleList.Add(new MainForm.PlanarObjectStore.Triangle2D(nextTriangleId, prevList[k].GetNodeData, curList[k + 1].GetNodeData, prevList[k + 1].GetNodeData));
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
                MainForm.PlanarObjectStore.AnimationTracker tempTracker = new MainForm.PlanarObjectStore.AnimationTracker();
                tempTracker.EdgeList = new List<MainForm.PlanarObjectStore.Edge2D>();
                tempTracker.EdgeList.AddRange(this._outputEdgeList);
                this._outputTrackerList.Add(tempTracker);
            }
            private void UpdateAllTrackers() {
                MainForm.PlanarObjectStore.AnimationTracker tempTracker = new MainForm.PlanarObjectStore.AnimationTracker();
                tempTracker.EdgeList = new List<MainForm.PlanarObjectStore.Edge2D>();
                tempTracker.EdgeList.AddRange(this._outputEdgeList);
                tempTracker.TriangleList = new List<MainForm.PlanarObjectStore.Triangle2D>();
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
            private void AddToOutputList(Edge2DStore edge, Triangle2DStore triangle) {
                MainForm.PlanarObjectStore.Edge2D tempEdge = new MainForm.PlanarObjectStore.Edge2D(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData);
                this._outputEdgeList.Add(tempEdge);
                if (triangle != null) {
                    MainForm.PlanarObjectStore.Triangle2D tempTriangle =
                        new MainForm.PlanarObjectStore.Triangle2D(triangle.TriangleID, triangle.FirstNode.GetNodeData, triangle.SecondNode.GetNodeData, triangle.ThirdNode.GetNodeData);
                    this._outputTriangleList.Add(tempTriangle);
                }
                UpdateAllTrackers();
            }
            private void RemoveFromOutputList(Edge2DStore edge, int firstTriangleID, int secondTriangleID) {
                MainForm.PlanarObjectStore.Edge2D tempEdge = new MainForm.PlanarObjectStore.Edge2D(edge.EdgeID, edge.FirstNode.GetNodeData, edge.SecondNode.GetNodeData);
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
            private void AddEdge(Node2DStore node1, Node2DStore node2) {
                int edgeID = GetUniqueEdgeID();
                Edge2DStore edge = new Edge2DStore(edgeID, node1, node2);

                this._nodeList[node1.NodeID].AddSortedEdge(edge);
                this._nodeList[node2.NodeID].AddSortedEdge(edge);

                this._edgeList.Add(edge);
                AddToOutputList(edge, null);
            }
            private void AddEdgeWithTriangle(Node2DStore node1, Node2DStore node2) {
                int edgeID = GetUniqueEdgeID();
                Edge2DStore edge = new Edge2DStore(edgeID, node1, node2);

                this._nodeList[node1.NodeID].AddSortedEdge(edge);
                this._nodeList[node2.NodeID].AddSortedEdge(edge);

                bool triangleFound = false;
                Edge2DStore secondEdge = edge.SecondNode.CCVerticalEdge(edge);
                Edge2DStore thirdEdge = secondEdge.SecondNode.CCVerticalEdge(secondEdge);

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

                    Triangle2DStore tempTriangle = new Triangle2DStore(triangleID, edge.FirstNode, edge.SecondNode, secondEdge.GetOtherNode(edge.SecondNode));
                    this._triangleList.Add(tempTriangle);

                    this._triangleList[this._triangleList.Count - 1].FirstEdge = this._edgeList[thirdEdgeIndex];
                    this._triangleList[this._triangleList.Count - 1].SecondEdge = this._edgeList[secondEdgeIndex];
                    this._triangleList[this._triangleList.Count - 1].ThirdEdge = this._edgeList[this._edgeList.Count - 1];
                }
                AddToOutputList(edge, triangleFound ? this._triangleList[this._triangleList.Count - 1] : null);
            }
            private void AddTriangle(Node2DStore node1, Node2DStore node2, Node2DStore node3) {
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
            private Edge2DStore FindInitialBaseLR(List<Node2DStore> leftList, List<Node2DStore> rightList) {
                Node2DStore leftEnd = leftList[leftList.Count - 1];
                Node2DStore rightEnd = rightList[0];

                Edge2DStore leftEdgeCWD = leftEnd.CWVerticalEdge(0);
                Edge2DStore rightEdgeCCWD = rightEnd.CCVerticalEdge(0);

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
            private void MergeLREdge(Edge2DStore baseLREdge) {
                Edge2DStore baseLREdge_symm = new Edge2DStore(baseLREdge.EdgeID, baseLREdge.SecondNode, baseLREdge.FirstNode);
                Edge2DStore leftCandidate = baseLREdge.FirstNode.CCVerticalEdge(baseLREdge);
                Edge2DStore rightCandidate = baseLREdge.SecondNode.CWVerticalEdge(baseLREdge_symm);

                if (Helper.LeftOf(leftCandidate.SecondNode, baseLREdge)) {
                    Edge2DStore leftCandidate_next = baseLREdge.FirstNode.CCVerticalEdge(leftCandidate);
                    while (Helper.InCircle(baseLREdge.FirstNode, baseLREdge.SecondNode, leftCandidate.SecondNode, leftCandidate_next.SecondNode)) {
                        Edge2DStore leftCandidate_new = baseLREdge.FirstNode.CCVerticalEdge(leftCandidate);
                        RemoveEdge(leftCandidate.EdgeID);
                        leftCandidate = leftCandidate_new;
                        leftCandidate_next = baseLREdge.FirstNode.CCVerticalEdge(leftCandidate);
                    }
                }
                if (Helper.RightOf(rightCandidate.SecondNode, baseLREdge_symm)) {
                    Edge2DStore rightCandidate_next = baseLREdge.SecondNode.CWVerticalEdge(rightCandidate);
                    while (Helper.InCircle(baseLREdge_symm.SecondNode, baseLREdge_symm.FirstNode, rightCandidate.SecondNode, rightCandidate_next.SecondNode)) {
                        Edge2DStore rightCandidate_new = baseLREdge.SecondNode.CWVerticalEdge(rightCandidate);
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
                Edge2DStore baseLREdge_new = this._edgeList[this._edgeList.Count - 1];
                MergeLREdge(baseLREdge_new);
            }
            private void DelaunayTriangulationDivideAndConquer(List<Node2DStore> leftList, List<Node2DStore> rightList) {
                if (leftList.Count > 3) {
                    int halfLength = Convert.ToInt32(Math.Ceiling(leftList.Count / 2.0f));
                    List<Node2DStore> leftLeftList = leftList.GetRange(0, halfLength);
                    List<Node2DStore> leftRightList = leftList.GetRange(halfLength, leftList.Count - halfLength);
                    DelaunayTriangulationDivideAndConquer(leftLeftList, leftRightList);
                } else if (leftList.Count == 3) {
                    AddTriangle(leftList[0], leftList[1], leftList[2]);
                } else if (leftList.Count == 2) {
                    AddEdge(leftList[0], leftList[1]);
                }
                if (rightList.Count > 3) {
                    int halfLength = Convert.ToInt32(Math.Ceiling(rightList.Count / 2.0f));
                    List<Node2DStore> rightLeftList = rightList.GetRange(0, halfLength);
                    List<Node2DStore> rightRightList = rightList.GetRange(halfLength, rightList.Count - halfLength);
                    DelaunayTriangulationDivideAndConquer(rightLeftList, rightRightList);
                } else if (rightList.Count == 3) {
                    AddTriangle(rightList[0], rightList[1], rightList[2]);
                } else if (rightList.Count == 2) {
                    AddEdge(rightList[0], rightList[1]);
                }
                Edge2DStore baseLREdge = FindInitialBaseLR(leftList, rightList);
                MergeLREdge(baseLREdge);
            }
            #endregion

            #region Node2DStore, EdgeAngleComparerVertical, Edge2DStore, Triangle2DStore
            internal class Node2DStore {
                private int _nodeID;
                private double _xCoordinate;
                private double _yCoordinate;
                private MainForm.PlanarObjectStore.Node2D _nodeData;
                private List<Edge2DStore> _connectedEdgeList = new List<Edge2DStore>();

                internal Node2DStore(int nodeID, double xCoordinate, double yCoordinate, MainForm.PlanarObjectStore.Node2D nodeDataAsNode2D) {
                    this._nodeID = nodeID;
                    this._xCoordinate = xCoordinate;
                    this._yCoordinate = yCoordinate;
                    this._nodeData = nodeDataAsNode2D;
                }

                internal int NodeID { get { return this._nodeID; } }
                internal double XCoordinate { get { return this._xCoordinate; } }
                internal double YCoordinate { get { return this._yCoordinate; } }
                internal MainForm.PlanarObjectStore.Node2D GetNodeData { get { return this._nodeData; } }

                internal bool Equals(Node2DStore other) {
                    return this._xCoordinate == other.XCoordinate && this._yCoordinate == other.YCoordinate;
                }
                internal Edge2DStore CCVerticalEdge(int id) {
                    if (id > this._connectedEdgeList.Count - 1) {
                        return null;
                    }
                    return this._connectedEdgeList[id];
                }
                internal Edge2DStore CCVerticalEdge(Edge2DStore withEdge) {
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
                internal Edge2DStore CWVerticalEdge(int id) {
                    int count = this._connectedEdgeList.Count - 1;
                    if (id > count) {
                        return null;
                    }
                    return this._connectedEdgeList[count - id];
                }
                internal Edge2DStore CWVerticalEdge(Edge2DStore withEdge) {
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
                private Edge2DStore GetOrientedEdgeFromThisNode(Edge2DStore edge) {
                    Node2DStore node = new Node2DStore(this._nodeID, this._xCoordinate, this._yCoordinate, this._nodeData);
                    return edge.FirstNode.Equals(node) ? edge : new Edge2DStore(edge.EdgeID, node, edge.GetOtherNode(node));
                }
                internal void AddSortedEdge(Edge2DStore edge) {
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
                internal void RemoveEdge(Edge2DStore edge) {
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

            internal class EdgeAngleComparerVertical : IComparer<Edge2DStore> {
                public int Compare(Edge2DStore edge1, Edge2DStore edge2) {
                    Edge2DStore vertEdge1 = new Edge2DStore(-1, edge1.FirstNode, new Node2DStore(-1, edge1.FirstNode.XCoordinate, edge1.FirstNode.YCoordinate - 100, null));
                    double angle1 = Helper.GetAngleBetween(vertEdge1, edge1);

                    Edge2DStore vertEdge2 = new Edge2DStore(-1, edge2.FirstNode, new Node2DStore(-1, edge2.FirstNode.XCoordinate, edge2.FirstNode.YCoordinate - 100, null));
                    double angle2 = Helper.GetAngleBetween(vertEdge2, edge2);

                    return angle1 < angle2 ? -1 : angle1 > angle2 ? 1 : 0;
                }
            }

            internal class Edge2DStore {
                private int _edgeID;
                private Node2DStore _firstNode;
                private Node2DStore _secondNode;
                private int _firstTriangleID;
                private int _secondTriangleID;

                internal Edge2DStore(int edgeID, Node2DStore firstNode, Node2DStore secondNode) {
                    this._edgeID = edgeID;
                    this._firstNode = firstNode;
                    this._secondNode = secondNode;
                    this._firstTriangleID = -1;
                    this._secondTriangleID = -1;
                }

                internal int EdgeID { get { return this._edgeID; } }
                internal Node2DStore FirstNode { get { return this._firstNode; } }
                internal Node2DStore SecondNode { get { return this._secondNode; } }
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
                internal bool Equals(Edge2DStore other) {
                    return this._firstNode.Equals(other.FirstNode) && this._secondNode.Equals(other.SecondNode);
                }
                internal bool CommutativeEquals(Edge2DStore other) {
                    return (this._firstNode.Equals(other.FirstNode) && this._secondNode.Equals(other.SecondNode)) ||
                        (this._firstNode.Equals(other.SecondNode) && this._secondNode.Equals(other.FirstNode));
                }
                internal Node2DStore GetOtherNode(Node2DStore node) {
                    return node.Equals(this._firstNode) ? this._secondNode : this._firstNode;
                }
            }

            internal class Triangle2DStore {
                private int _triangleID;
                private Node2DStore _firstNode;
                private Node2DStore _secondNode;
                private Node2DStore _thirdNode;
                private Edge2DStore _firstEdge;
                private Edge2DStore _secondEdge;
                private Edge2DStore _thirdEdge;

                internal Triangle2DStore(int triangleID, Node2DStore firstNode, Node2DStore secondNode, Node2DStore thirdNode) {
                    this._triangleID = triangleID;
                    this._firstNode = firstNode;
                    this._secondNode = secondNode;
                    this._thirdNode = thirdNode;
                }

                internal int TriangleID { get { return this._triangleID; } }
                internal Node2DStore FirstNode { get { return this._firstNode; } }
                internal Node2DStore SecondNode { get { return this._secondNode; } }
                internal Node2DStore ThirdNode { get { return this._thirdNode; } }
                internal Edge2DStore FirstEdge {
                    get { return this._firstEdge; }
                    set { this._firstEdge = value; }
                }
                internal Edge2DStore SecondEdge {
                    get { return this._secondEdge; }
                    set { this._secondEdge = value; }
                }
                internal Edge2DStore ThirdEdge {
                    get { return this._thirdEdge; }
                    set { this._thirdEdge = value; }
                }
            }
            #endregion

            #region Helper
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
                private static bool CheckPairwiseIntersection(Edge2DStore edge1, Edge2DStore edge2) {
                    return CheckProjectionsIntersection(edge1.FirstNode.XCoordinate, edge1.SecondNode.XCoordinate, edge2.FirstNode.XCoordinate, edge2.SecondNode.XCoordinate) &&
                        CheckProjectionsIntersection(edge1.FirstNode.YCoordinate, edge1.SecondNode.YCoordinate, edge2.FirstNode.YCoordinate, edge2.SecondNode.YCoordinate);
                }
                private static double PseudoscalarProduct(Edge2DStore edge1, Edge2DStore edge2) {
                    return edge1.GetXCoordinateForVector * edge2.GetYCoordinateForVector - edge1.GetYCoordinateForVector * edge2.GetXCoordinateForVector;
                }
                private static bool CheckCommonNode(double p1, double p2, double p3, double p4) {
                    return (p1 == 0 && p2 != 0 && p3 == 0 && p4 != 0) || (p1 != 0 && p2 == 0 && p3 != 0 && p4 == 0) ||
                           (p1 == 0 && p2 != 0 && p3 != 0 && p4 == 0) || (p1 != 0 && p2 == 0 && p3 == 0 && p4 != 0);
                }
                internal static bool CheckEdgeIntersection(Edge2DStore edge1, Edge2DStore edge2) {
                    if (!CheckPairwiseIntersection(edge1, edge2)) {
                        return false;
                    }

                    double pseudoscalar1 = PseudoscalarProduct(new Edge2DStore(-2, edge1.FirstNode, edge1.SecondNode), new Edge2DStore(-2, edge1.FirstNode, edge2.FirstNode));
                    double pseudoscalar2 = PseudoscalarProduct(new Edge2DStore(-2, edge1.FirstNode, edge1.SecondNode), new Edge2DStore(-2, edge1.FirstNode, edge2.SecondNode));

                    if ((pseudoscalar1 > 0 && pseudoscalar2 > 0) || (pseudoscalar1 < 0 && pseudoscalar2 < 0)) {
                        return false;
                    }

                    double pseudoscalar3 = PseudoscalarProduct(new Edge2DStore(-2, edge2.FirstNode, edge2.SecondNode), new Edge2DStore(-2, edge2.FirstNode, edge1.FirstNode));
                    double pseudoscalar4 = PseudoscalarProduct(new Edge2DStore(-2, edge2.FirstNode, edge2.SecondNode), new Edge2DStore(-2, edge2.FirstNode, edge1.SecondNode));

                    if ((pseudoscalar3 > 0 && pseudoscalar4 > 0) || (pseudoscalar3 < 0 && pseudoscalar4 < 0)) {
                        return false;
                    }
                    return CheckCommonNode(pseudoscalar1, pseudoscalar2, pseudoscalar3, pseudoscalar4) ? false : true;
                }

                internal static double GetAngleBetween(Edge2DStore withEdge, Edge2DStore edge) {
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
                internal static bool IsCollinear(Node2DStore node1, Node2DStore node2, Node2DStore node3) {
                    return (node2.XCoordinate - node1.XCoordinate) * (node3.YCoordinate - node1.YCoordinate) -
                        (node2.YCoordinate - node1.YCoordinate) * (node3.XCoordinate - node1.XCoordinate) == 0;
                }
                private static bool CCW(Node2DStore node1, Node2DStore node2, Node2DStore node3) {
                    return (node2.XCoordinate - node1.XCoordinate) * (node3.YCoordinate - node1.YCoordinate) -
                        (node2.YCoordinate - node1.YCoordinate) * (node3.XCoordinate - node1.XCoordinate) > 0;
                }
                internal static bool LeftOf(Node2DStore node, Edge2DStore edge) {
                    return CCW(node, edge.FirstNode, edge.SecondNode);
                }
                internal static bool RightOf(Node2DStore node, Edge2DStore edge) {
                    return CCW(node, edge.SecondNode, edge.FirstNode);
                }
                internal static bool InCircle(Node2DStore node1, Node2DStore node2, Node2DStore node3, Node2DStore otherNode) {
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
        }
    }
}
