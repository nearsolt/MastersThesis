using static MastersThesis.MainForm.PlanarObjectStore;

namespace MastersThesis {
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

        #region Greedy Triangulation
        /// <summary>
        /// Конструктор для построения жадной триангуляции
        /// </summary>
        /// <param name="inputNodeList">Список узлов, передаваемый на вход</param>
        /// <param name="outputEdgeList">Список ребер, получаемый на выходе</param>
        /// <param name="outputAnimationList">Список элементов анимации, получаемый на выходе</param>
        internal MeshStore(List<NodeStore> inputNodeList, out List<Edge> outputEdgeList, out List<TweenAnimation> outputAnimationList) {
            _nodeList = inputNodeList;

            GreedyTriangulation();

            outputEdgeList = _outputEdgeList;
            outputAnimationList = _outputAnimationList;
        }
        #endregion

        #region Delaunay Triangulation
        /// <summary>
        /// Конструктор для построения триангуляции Делоне, используя алгоритм слияния "Разделяй и властвуй"
        /// </summary>
        /// <param name="inputNodeList">Список узлов, передаваемый на вход</param>
        /// <param name="nodeStoreList">Список узлов (NodeStore)</param>
        /// <param name="edgeStoreList">Список ребер (EdgeStore)</param>
        /// <param name="triangleStoreList">Список треугольников (TriangleStore)</param>
        /// <param name="outputEdgeList">Список ребер, получаемый на выходе</param>
        /// <param name="outputTriangleList">Список треугольников, получаемый на выходе</param>
        /// <param name="outputAnimationList">Список элементов анимации, получаемый на выходе</param>
        internal MeshStore(List<NodeStore> inputNodeList, ref List<NodeStore> nodeStoreList, ref List<EdgeStore> edgeStoreList, ref List<TriangleStore> triangleStoreList,
                           out List<Edge> outputEdgeList, out List<Triangle> outputTriangleList, out List<TweenAnimation> outputAnimationList) {

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

            nodeStoreList = _nodeList;
            edgeStoreList = _edgeList;
            triangleStoreList = _triangleList;

            outputEdgeList = _outputEdgeList;
            outputTriangleList = _outputTriangleList;
            outputAnimationList = _outputAnimationList;
        }

        #endregion

        #region Mesh Refinement Triangulation
        /// <summary>
        /// Конструктор для построения триангуляции методом измельчения
        /// </summary>
        /// <param name="inputNodeList">Список узлов, передаваемый на вход</param>
        /// <param name="inputEdgeList">Список ребер, передаваемый на вход</param>
        /// <param name="inputTriangleList">Список треугольников, передаваемый на вход</param>
        /// <param name="lastAnimation">Последний элемент анимации, полученный при построении триангуляции Делоне</param>
        /// <param name="decimalPlaces">Количество знаков после запятой при вычислении координат внутренних узлов</param>
        /// <param name="meshRefinementCoeff">Коэффициент измельчения q</param>
        /// <param name="outputNodeList">Список узлов, получаемый на выходе</param>
        /// <param name="outputEdgeList">Список ребер, получаемый на выходе</param>
        /// <param name="outputTriangleList">Список треугольников, получаемый на выходе</param>
        /// <param name="outputAnimationList">Список элементов анимации, получаемый на выходе</param>
        internal MeshStore(List<NodeStore> inputNodeList, List<EdgeStore> inputEdgeList, List<TriangleStore> inputTriangleList, TweenAnimation lastAnimation, int decimalPlaces, int meshRefinementCoeff,
                           out List<Node> outputNodeList, out List<Edge> outputEdgeList, out List<Triangle> outputTriangleList, out List<TweenAnimation> outputAnimationList) {

            _nodeList = inputNodeList;
            _edgeList = inputEdgeList;
            _triangleList = inputTriangleList;

            for (int j = 0; j < inputNodeList.Count; j++) {
                _outputNodeList.Add(new Node(inputNodeList[j].NodeID, inputNodeList[j].XCoordinate, inputNodeList[j].YCoordinate));
            }
            _outputAnimationList.Add(lastAnimation);

            MeshRefinement(inputTriangleList, decimalPlaces, meshRefinementCoeff);

            outputNodeList = _outputNodeList;
            outputEdgeList = _outputEdgeList;
            outputTriangleList = _outputTriangleList;
            outputAnimationList = _outputAnimationList;
        }
        #endregion

        #endregion

        #region Methods

        #region Common
        /// <summary>
        /// Частичное обновление списка элементов анимации
        /// </summary>
        private void UpdateAnimationListPartially() {
            TweenAnimation tweenAnimation = new TweenAnimation();
            tweenAnimation.EdgeList = new List<Edge>();
            tweenAnimation.EdgeList.AddRange(_outputEdgeList);
            _outputAnimationList.Add(tweenAnimation);
        }
        /// <summary>
        /// Обновление списка элементов анимации
        /// </summary>
        private void UpdateAnimationList() {
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
        #endregion

        #region Greedy Triangulation  
        /// <summary>
        /// Добавление ребра (жадная триангуляция)
        /// </summary>
        /// <param name="firstNode">Первый узел</param>
        /// <param name="secondNode">Второй узел</param>
        private void AddEdgeForGT(NodeStore firstNode, NodeStore secondNode) {
            EdgeStore edge = new EdgeStore(GetUniqueEdgeID(), firstNode, secondNode);

            _edgeList.Add(edge);
            _outputEdgeList.Add(new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node));
            UpdateAnimationListPartially();
        }
        /// <summary>
        /// Вычисление жадной триангуляции
        /// </summary>
        private void GreedyTriangulation() {
            List<EdgeStore> tempEdgeList = new List<EdgeStore>();

            // Получим список всех возможных ребер, исключая дубли
            for (int j = 1; j < _nodeList.Count; j++) {
                for (int k = 0; k < j; k++) {
                    EdgeStore tempEdge = new EdgeStore(-2, _nodeList[j], _nodeList[k]);

                    if (!tempEdgeList.Exists(obj => obj.CommutativeEquals(tempEdge))) {
                        tempEdgeList.Add(tempEdge);
                    }
                }
            }
            // Сортируем по возрастанию длины
            tempEdgeList = tempEdgeList.OrderBy(obj => obj.EdgeLength).ToList();

            AddEdgeForGT(tempEdgeList[0].FirstNode, tempEdgeList[0].SecondNode);

            // Два ребра пересекаются
            bool isEdgesIntersect = false;

            // Добавляем ребра по возрастанию длины, пропуская те, которые имеют пересечение с любым ранее добавленным ребром
            for (int j = 1; j < tempEdgeList.Count; j++) {
                for (int k = 0; k < j; k++) {
                    if (Helper.IsEdgesIntersect(tempEdgeList[j], tempEdgeList[k])) {
                        isEdgesIntersect = true;
                        tempEdgeList.RemoveAt(j);
                        j--;
                        break;
                    }
                }
                if (!isEdgesIntersect) {
                    AddEdgeForGT(tempEdgeList[j].FirstNode, tempEdgeList[j].SecondNode);
                }
                isEdgesIntersect = false;
            }
        }
        #endregion

        #region "Divide And Conquer" Delaunay Triangulation
        /// <summary>
        /// Добавление ребра в получаемый на выходе список ребер
        /// </summary>
        /// <param name="edge">Добавляемое ребро</param>
        private void AddEdgeToOutputList(EdgeStore edge) {
            _outputEdgeList.Add(new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node));
            UpdateAnimationList();
        }
        /// <summary>
        /// Добавление ребра и треугольника в получаемые на выходе списки ребер и треугольников
        /// </summary>
        /// <param name="edge">Добавляемое ребро</param>
        /// <param name="triangle">Добавляемый треугольник</param>
        private void AddEdgeAndTriangleToOutputList(EdgeStore edge, TriangleStore triangle) {
            _outputEdgeList.Add(new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node));
            _outputTriangleList.Add(new Triangle(triangle.TriangleID, triangle.FirstNode.Node, triangle.SecondNode.Node, triangle.ThirdNode.Node));
            UpdateAnimationList();
        }
        /// <summary>
        /// Удаление ребра и треугольника из получаемых на выходе списков ребер и треугольников
        /// </summary>
        /// <param name="edge">Удаляемое ребро</param>
        /// <param name="firstTriangleID">ID первого смежного треугольника</param>
        /// <param name="secondTriangleID">ID второго смежного треугольника</param>
        private void RemoveFromOutputList(EdgeStore edge, int firstTriangleID, int secondTriangleID) {
            Edge tempEdge = new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node);
            int deletionObjIndex = _outputEdgeList.FindIndex(obj => obj.CommutativeEquals(tempEdge));

            if (deletionObjIndex != -1) {
                _outputEdgeList.RemoveAt(deletionObjIndex);

                if (firstTriangleID != -1 || secondTriangleID != -1) {
                    if (firstTriangleID < secondTriangleID) {
                        (firstTriangleID, secondTriangleID) = (secondTriangleID, firstTriangleID);
                    }
                    if (firstTriangleID != -1) {
                        int deletionIndex = _outputTriangleList.FindIndex(obj => obj.TriangleID == _triangleList[firstTriangleID].TriangleID);
                        _outputTriangleList.RemoveAt(deletionIndex);
                    }
                    if (secondTriangleID != -1) {
                        int deletionIndex = _outputTriangleList.FindIndex(obj => obj.TriangleID == _triangleList[secondTriangleID].TriangleID);
                        _outputTriangleList.RemoveAt(deletionIndex);
                    }
                }
                UpdateAnimationList();
            }
        }
        /// <summary>
        /// Добавление ребра (триангуляция Делоне)
        /// </summary>
        /// <param name="firstNode">Первый узел</param>
        /// <param name="secondNode">Второй узел</param>
        private void AddEdge(NodeStore firstNode, NodeStore secondNode) {
            EdgeStore edge = new EdgeStore(GetUniqueEdgeID(), firstNode, secondNode);

            _nodeList[firstNode.NodeID].AddSortedEdge(edge);
            _nodeList[secondNode.NodeID].AddSortedEdge(edge);

            _edgeList.Add(edge);
            AddEdgeToOutputList(edge);
        }
        /// <summary>
        /// Добавление ребра вместе с треугольником
        /// </summary>
        /// <param name="firstNode">Первый узел</param>
        /// <param name="secondNode">Второй узел</param>
        private void AddEdgeWithTriangle(NodeStore firstNode, NodeStore secondNode) {
            EdgeStore edge = new EdgeStore(GetUniqueEdgeID(), firstNode, secondNode);

            _nodeList[firstNode.NodeID].AddSortedEdge(edge);
            _nodeList[secondNode.NodeID].AddSortedEdge(edge);

            // Треугольник найден
            bool isTriangleFound = false;

            // Поиск двух ребер, соединенных с данным, которые образуют треугольник (поиск по часовой стрелки)
            EdgeStore secondEdge = edge.SecondNode.CCWVerticalEdge(edge);
            EdgeStore thirdEdge = secondEdge.SecondNode.CCWVerticalEdge(secondEdge);

            if (thirdEdge.SecondNode.Equals(edge.FirstNode)) {
                isTriangleFound = true;
            } else {
                // поиск против часовой стрелки
                secondEdge = edge.SecondNode.CWVerticalEdge(edge);
                thirdEdge = secondEdge.SecondNode.CWVerticalEdge(secondEdge);
                if (thirdEdge.SecondNode.Equals(edge.FirstNode)) {
                    isTriangleFound = true;
                }
            }
            _edgeList.Add(edge);

            if (isTriangleFound) {
                int triangleID = GetUniqueTriangleID();
                int secondEdgeIndex = _edgeList.FindIndex(obj => obj.EdgeID == secondEdge.EdgeID);
                int thirdEdgeIndex = _edgeList.FindIndex(obj => obj.EdgeID == thirdEdge.EdgeID);

                _edgeList[secondEdgeIndex].AddTriangleID(triangleID);
                _edgeList[thirdEdgeIndex].AddTriangleID(triangleID);
                _edgeList[_edgeList.Count - 1].AddTriangleID(triangleID);

                _triangleList.Add(new TriangleStore(triangleID, edge.FirstNode, edge.SecondNode, secondEdge.GetOtherNode(edge.SecondNode)));

                _triangleList[_triangleList.Count - 1].FirstEdge = _edgeList[thirdEdgeIndex];
                _triangleList[_triangleList.Count - 1].SecondEdge = _edgeList[secondEdgeIndex];
                _triangleList[_triangleList.Count - 1].ThirdEdge = _edgeList[_edgeList.Count - 1];

                AddEdgeAndTriangleToOutputList(edge, _triangleList[_triangleList.Count - 1]);
            } else {
                AddEdgeToOutputList(edge);
            }
        }
        /// <summary>
        /// Добавление треугольника
        /// </summary>
        /// <param name="firstNode">Первый узел</param>
        /// <param name="secondNode">Второй узел</param>
        /// <param name="thirdNode">Третий узел</param>
        private void AddTriangle(NodeStore firstNode, NodeStore secondNode, NodeStore thirdNode) {
            AddEdge(firstNode, secondNode);
            AddEdge(secondNode, thirdNode);

            if (!Helper.IsCollinear(firstNode, secondNode, thirdNode)) {
                AddEdgeWithTriangle(thirdNode, firstNode);
            }
        }
        /// <summary>
        /// Удаление треугольник
        /// </summary>
        /// <param name="triangleID">ID треугольника</param>
        private void RemoveTriangle(int triangleID) {
            _triangleList[triangleID].FirstEdge.RemoveTriangleID(_triangleList[triangleID].TriangleID);
            _triangleList[triangleID].SecondEdge.RemoveTriangleID(_triangleList[triangleID].TriangleID);
            _triangleList[triangleID].ThirdEdge.RemoveTriangleID(_triangleList[triangleID].TriangleID);

            _uniqueTriangleIDList.Add(_triangleList[triangleID].TriangleID);
            _triangleList.RemoveAt(triangleID);
        }
        /// <summary>
        /// Удаление ребра
        /// </summary>
        /// <param name="edgeID">ID ребра</param>
        private void RemoveEdge(int edgeID) {
            int deletionObjIndex = _edgeList.FindIndex(obj => obj.EdgeID == edgeID);

            _nodeList[_edgeList[deletionObjIndex].FirstNode.NodeID].RemoveEdge(_edgeList[deletionObjIndex].EdgeID);
            _nodeList[_edgeList[deletionObjIndex].SecondNode.NodeID].RemoveEdge(_edgeList[deletionObjIndex].EdgeID);

            int firstTriangleIndex = _triangleList.FindIndex(obj => obj.TriangleID == _edgeList[deletionObjIndex].FirstTriangleID);
            int secondTriangleIndex = -1;

            if (firstTriangleIndex == -1) {
                secondTriangleIndex = _triangleList.FindIndex(obj => obj.TriangleID == _edgeList[deletionObjIndex].SecondTriangleID);
            }
            RemoveFromOutputList(_edgeList[deletionObjIndex], firstTriangleIndex, secondTriangleIndex);

            if (firstTriangleIndex != -1) {
                RemoveTriangle(firstTriangleIndex);
            } else if (secondTriangleIndex != -1) {
                RemoveTriangle(secondTriangleIndex);
            }
            _uniqueEdgeIDList.Add(_edgeList[deletionObjIndex].EdgeID);
            _edgeList.RemoveAt(deletionObjIndex);
        }
        /// <summary>
        /// Поиск первоначального базового LR-ребра
        /// </summary>
        /// <param name="leftList">Левое подмножество узлов</param>
        /// <param name="rightList">Правое подмножество узлов</param>
        /// <returns></returns>
        private EdgeStore FindInitialBaseLREdge(List<NodeStore> leftList, List<NodeStore> rightList) {
            NodeStore leftEnd = leftList[leftList.Count - 1];
            NodeStore rightEnd = rightList[0];

            // Первое ребро-вертикаль из точки в направлении по часовой стрелке
            EdgeStore leftEdgeCW = leftEnd.CWVerticalEdge(0);
            // Первое ребро-вертикаль из точки в направлении против часовой стрелки
            EdgeStore rightEdgeCCW = rightEnd.CCWVerticalEdge(0);

            // Поиск левого и правого узлов для первоначального базового LR-ребра
            for (; ; ) {
                if (Helper.LeftOf(rightEnd, leftEdgeCW)) {
                    leftEnd = leftEdgeCW.GetOtherNode(leftEnd);
                    leftEdgeCW = leftEnd.CWVerticalEdge(0);
                } else if (Helper.RightOf(leftEnd, rightEdgeCCW)) {
                    rightEnd = rightEdgeCCW.GetOtherNode(rightEnd);
                    rightEdgeCCW = rightEnd.CCWVerticalEdge(0);
                } else {
                    break;
                }
            }
            // Добавление первоначального базового LR-ребра
            AddEdge(leftEnd, rightEnd);
            return _edgeList[_edgeList.Count - 1];
        }
        /// <summary>
        /// Рекурсивное слияние левой и правой триангуляций
        /// </summary>
        /// <param name="baseLREdge">Базовое LR-ребро</param>
        private void MergeLREdge(EdgeStore baseLREdge) {
            // Ребро, симметричное базовому LR-ребру
            EdgeStore baseLREdge_symm = new EdgeStore(baseLREdge.EdgeID, baseLREdge.SecondNode, baseLREdge.FirstNode);
            // Ребро, содержащее левого потенциального кандидата и левый узел базового LR-ребра
            EdgeStore leftCandidate = baseLREdge.FirstNode.CCWVerticalEdge(baseLREdge);
            // Ребро, содержащее правого потенциального кандидата и правый узел базового LR-ребра
            EdgeStore rightCandidate = baseLREdge.SecondNode.CWVerticalEdge(baseLREdge_symm);

            // Поиск левого кандидата (шаг 3.2)
            if (Helper.LeftOf(leftCandidate.SecondNode, baseLREdge)) {
                EdgeStore leftCandidate_next = baseLREdge.FirstNode.CCWVerticalEdge(leftCandidate);
                while (Helper.InCircle(baseLREdge.FirstNode, baseLREdge.SecondNode, leftCandidate.SecondNode, leftCandidate_next.SecondNode)) {
                    EdgeStore leftCandidate_new = baseLREdge.FirstNode.CCWVerticalEdge(leftCandidate);
                    RemoveEdge(leftCandidate.EdgeID);
                    leftCandidate = leftCandidate_new;
                    leftCandidate_next = baseLREdge.FirstNode.CCWVerticalEdge(leftCandidate);
                }
            }
            // Поиск правого кандидата (шаг 3.2)
            if (Helper.RightOf(rightCandidate.SecondNode, baseLREdge_symm)) {
                EdgeStore rightCandidate_next = baseLREdge.SecondNode.CWVerticalEdge(rightCandidate);
                while (Helper.InCircle(baseLREdge_symm.SecondNode, baseLREdge_symm.FirstNode, rightCandidate.SecondNode, rightCandidate_next.SecondNode)) {
                    EdgeStore rightCandidate_new = baseLREdge.SecondNode.CWVerticalEdge(rightCandidate);
                    RemoveEdge(rightCandidate.EdgeID);
                    rightCandidate = rightCandidate_new;
                    rightCandidate_next = baseLREdge.SecondNode.CWVerticalEdge(rightCandidate);
                }
            }
            // Левый кандидат найден
            bool isFoundLeftCandidate = Helper.LeftOf(leftCandidate.SecondNode, baseLREdge);
            // Правый кандидат найден
            bool isFoundRightCandidate = Helper.RightOf(rightCandidate.SecondNode, baseLREdge_symm);

            // шаг 3.3
            if (isFoundLeftCandidate && isFoundRightCandidate) {
                //  если найдены оба кандидата, выполняем проверку через уравнение описанной окружности
                if (Helper.InCircle(baseLREdge.FirstNode, baseLREdge.SecondNode, leftCandidate.SecondNode, rightCandidate.SecondNode)) {
                    AddEdgeWithTriangle(baseLREdge.FirstNode, rightCandidate.SecondNode);
                } else {
                    AddEdgeWithTriangle(leftCandidate.SecondNode, baseLREdge_symm.FirstNode);
                }
            } else if (isFoundLeftCandidate) {
                AddEdgeWithTriangle(leftCandidate.SecondNode, baseLREdge_symm.FirstNode);
            } else if (isFoundRightCandidate) {
                AddEdgeWithTriangle(baseLREdge.FirstNode, rightCandidate.SecondNode);
            } else {
                return;
            }
            // Определение следующего базового LR-ребра
            EdgeStore baseLREdge_new = _edgeList[_edgeList.Count - 1];
            MergeLREdge(baseLREdge_new);
        }
        /// <summary>
        /// Рекурсивное вычисление триангуляции Делоне, используя алгоритм слияния "Разделяй и властвуй"
        /// </summary>
        /// <param name="leftList">Левое подмножество узлов</param>
        /// <param name="rightList">Правое подмножество узлов</param>
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
            // Определение первоначального базового LR-ребра
            EdgeStore baseLREdge = FindInitialBaseLREdge(leftList, rightList);
            MergeLREdge(baseLREdge);
        }
        #endregion

        #region Mesh Refinement Triangulation
        /// <summary>
        /// Вычисление внутренних узлов и заполнение списка узлов для ребра, которое определено в виде кортежа 
        /// (Item1 - само ребро; Item2 - список узлов данного ребра, включая внутренний узлы; Item3 - информация об ориентации)
        /// </summary>
        /// <param name="tuple">Ребро, определенное в виде кортежа</param>                                             
        /// <param name="decimalPlaces">Количество знаков после запятой при вычислении координат внутренних узлов</param>
        /// <param name="meshRefinementCoeff">Коэффициент измельчения q</param>
        /// <param name="nextNodeID">ID для следующего узла</param>
        private void GetIntermediateNodes(ref Tuple<EdgeStore, List<NodeStore>, bool> tuple, int decimalPlaces, int meshRefinementCoeff, ref int nextNodeID) {
            NodeStore firstNode;
            NodeStore secondNode;

            // Переопределяем узлы ребра, используя информацию об ориентации
            if (tuple.Item3) {
                firstNode = tuple.Item1.SecondNode;
                secondNode = tuple.Item1.FirstNode;
            } else {
                firstNode = tuple.Item1.FirstNode;
                secondNode = tuple.Item1.SecondNode;
            }
            double stepByXCoord = (secondNode.XCoordinate - firstNode.XCoordinate) / meshRefinementCoeff;
            double stepByYCoord = (secondNode.YCoordinate - firstNode.YCoordinate) / meshRefinementCoeff;

            // Добавление первого (общего) узла
            tuple.Item2.Add(firstNode);
            // Вычисление внутренних узлов
            for (int j = 1; j < meshRefinementCoeff; j++) {
                Node intermediateNode = new Node(nextNodeID, Math.Round(firstNode.XCoordinate + j * stepByXCoord, decimalPlaces, MidpointRounding.AwayFromZero),
                                                             Math.Round(firstNode.YCoordinate + j * stepByYCoord, decimalPlaces, MidpointRounding.AwayFromZero));

                NodeStore intermediateNodeStore = new NodeStore(intermediateNode.NodeID, intermediateNode.XCoordinate, intermediateNode.YCoordinate, intermediateNode);

                // Добавление новых узлов в триангуляцию
                if (!_nodeList.Exists(obj => obj.Equals(intermediateNodeStore))) {
                    _outputNodeList.Add(intermediateNode);
                    _nodeList.Add(intermediateNodeStore);
                    nextNodeID++;
                }
                // Добавление внутренних узлов
                tuple.Item2.Add(intermediateNodeStore);
            }
            // Добавление последнего узла
            tuple.Item2.Add(secondNode);
        }
        /// <summary>
        /// Вычисление внутренних узлов и заполнение списка узлов для ребра, ||-ому третьему ребру
        /// </summary>
        /// <param name="firstNode">Узел, расположенный на первом ребре</param>
        /// <param name="secondNode">Узел, расположенный на втором ребре</param>
        /// <param name="decimalPlaces">Количество знаков после запятой при вычислении координат внутренних узлов</param>
        /// <param name="meshRefinementCoeff">Коэффициент измельчения q</param>
        /// <param name="nextNodeID">ID для следующего узла</param>
        /// <returns>Список узлов</returns>
        private List<NodeStore> GetIntermediateNodes(NodeStore firstNode, NodeStore secondNode, int decimalPlaces, int meshRefinementCoeff, ref int nextNodeID) {
            double stepByXCoord = (secondNode.XCoordinate - firstNode.XCoordinate) / meshRefinementCoeff;
            double stepByYCoord = (secondNode.YCoordinate - firstNode.YCoordinate) / meshRefinementCoeff;

            // Добавление первого узла
            List<NodeStore> tempNodeList = new List<NodeStore>() { firstNode };
            // Вычисление внутренних узлов
            for (int j = 1; j < meshRefinementCoeff; j++) {
                Node intermediateNode = new Node(nextNodeID, Math.Round(firstNode.XCoordinate + j * stepByXCoord, decimalPlaces, MidpointRounding.AwayFromZero),
                                                             Math.Round(firstNode.YCoordinate + j * stepByYCoord, decimalPlaces, MidpointRounding.AwayFromZero));

                NodeStore intermediateNodeStore = new NodeStore(intermediateNode.NodeID, intermediateNode.XCoordinate, intermediateNode.YCoordinate, intermediateNode);

                // Добавление новых узлов в триангуляцию
                if (!_nodeList.Exists(obj => obj.Equals(intermediateNodeStore))) {
                    _outputNodeList.Add(intermediateNode);
                    _nodeList.Add(intermediateNodeStore);
                    tempNodeList.Add(intermediateNodeStore);
                    nextNodeID++;
                }
            }
            // Добавление последнего узла
            tempNodeList.Add(secondNode);
            return tempNodeList;
        }
        /// <summary>
        /// Добавление ребра (триангуляция методом измельчения)
        /// </summary>
        /// <param name="firstNode">Первый узел</param>
        /// <param name="secondNode">Второй узел</param>
        private void AddEdgeForMR(NodeStore firstNode, NodeStore secondNode) {
            EdgeStore edge = new EdgeStore(GetUniqueEdgeID(), firstNode, secondNode);

            if (!_edgeList.Exists(obj => obj.CommutativeEquals(edge))) {
                _edgeList.Add(edge);
                _outputEdgeList.Add(new Edge(edge.EdgeID, edge.FirstNode.Node, edge.SecondNode.Node));
                UpdateAnimationList();
            }
        }
        /// <summary>
        /// Вычисление триангуляции методом измельчения
        /// </summary>
        /// <param name="inputTriangleList">Список треугольников, передаваемый на вход</param>
        /// <param name="decimalPlaces">Количество знаков после запятой при вычислении координат внутренних узлов</param>
        /// <param name="meshRefinementCoeff">Коэффициент измельчения q</param>
        private void MeshRefinement(List<TriangleStore> inputTriangleList, int decimalPlaces, int meshRefinementCoeff) {
            int nextNodeID = _nodeList.Count;
            int nextTriangleID = _triangleList.Count;

            // Проходим по всем треугольникам
            foreach (TriangleStore triangle in inputTriangleList) {
                // Первое ребро треугольника поменяло ориентацию
                bool isReverseEdge1;
                // Второе ребро треугольника поменяло ориентацию
                bool isReverseEdge2;

                // Берем первые два ребра треугольника и определяем их ориентацию так, чтобы первые узлы каждого из ребер являлись общей точкой этих ребер
                if (triangle.FirstEdge.FirstNode == triangle.SecondEdge.FirstNode) {
                    isReverseEdge1 = false;
                    isReverseEdge2 = false;
                } else if (triangle.FirstEdge.FirstNode == triangle.SecondEdge.SecondNode) {
                    isReverseEdge1 = false;
                    isReverseEdge2 = true;
                } else if (triangle.FirstEdge.SecondNode == triangle.SecondEdge.FirstNode) {
                    isReverseEdge1 = true;
                    isReverseEdge2 = false;
                } else {
                    isReverseEdge1 = true;
                    isReverseEdge2 = true;
                }

                /* Создаем кортежи для ребер:
                 *      Item1 - само ребро,
                 *      Item2 - список узлов данного ребра, включая внутренний узлы
                 *      Item3 - информация об ориентации
                */

                Tuple<EdgeStore, List<NodeStore>, bool> tuple1 = new Tuple<EdgeStore, List<NodeStore>, bool>(triangle.FirstEdge, new List<NodeStore>(), isReverseEdge1);
                GetIntermediateNodes(ref tuple1, decimalPlaces, meshRefinementCoeff, ref nextNodeID);

                Tuple<EdgeStore, List<NodeStore>, bool> tuple2 = new Tuple<EdgeStore, List<NodeStore>, bool>(triangle.SecondEdge, new List<NodeStore>(), isReverseEdge2);
                GetIntermediateNodes(ref tuple2, decimalPlaces, meshRefinementCoeff, ref nextNodeID);

                /*                       . <- общий узел первых двух ребер
                 *                      /|\
                 *                     / v \
                 *                    * ~~~ *  <- Внутренние узлы ребер
                 *                   /   |   \
                 *                       |<- итерации по проходу
                 *                 v     v     v
                 *   1ое ребро -> . ~~~~~~~~~~~ . <- 2ое ребро
                 *   
                 *   Далее через цикл пройдем через все подобные треугольники, двигаясь от общей точки к третьему ребру
                */

                // 0-я итерация
                // Список узлов (узлы расположены на ребре, ||-ом третьему ребру), на предыдущей итерации (на 0-ой итерации - список содержит один узел - общий узел)
                List<NodeStore> prevList = new List<NodeStore>() { tuple1.Item2[0] };
                // Список узлов (узлы расположены на ребре, ||-ом третьему ребру), на текущей итерации
                List<NodeStore> curList = new List<NodeStore>() { tuple1.Item2[1], tuple2.Item2[1] };

                AddEdgeForMR(prevList[0], curList[0]);
                AddEdgeForMR(curList[0], curList[1]);
                AddEdgeForMR(curList[1], prevList[0]);

                _outputTriangleList.Add(new Triangle(nextTriangleID, prevList[0].Node, curList[0].Node, curList[1].Node));
                UpdateAnimationList();
                nextTriangleID++;

                // итерации, начиная с 1-ой
                for (int j = 1; j < meshRefinementCoeff; j++) {
                    prevList = curList.ToList();
                    curList = GetIntermediateNodes(tuple1.Item2[j + 1], tuple2.Item2[j + 1], decimalPlaces, j + 1, ref nextNodeID);
                    for (int k = 0; k < prevList.Count; k++) {
                        AddEdgeForMR(prevList[k], curList[k]);
                        AddEdgeForMR(curList[k], curList[k + 1]);
                        AddEdgeForMR(curList[k + 1], prevList[k]);
                        _outputTriangleList.Add(new Triangle(nextTriangleID, prevList[k].Node, curList[k].Node, curList[k + 1].Node));
                        UpdateAnimationList();
                        nextTriangleID++;
                        if (k < prevList.Count - 1) {
                            _outputTriangleList.Add(new Triangle(nextTriangleID, prevList[k].Node, curList[k + 1].Node, prevList[k + 1].Node));
                            UpdateAnimationList();
                            nextTriangleID++;
                        }
                    }
                }
            }

        }
        #endregion

        #endregion
    }
}