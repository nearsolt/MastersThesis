//using static MastersThesis.MainForm.PlanarObjectStore;

namespace MastersThesis {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e) {
            this._planarObjectStore = new PlanarObjectStore();
            InitializeCanvasSize();
            GenerateRandomNodes();
        }
        private void pictureBox_mainPic_SizeChanged(object sender, EventArgs e) {
            InitializeCanvasSize();
        }
        private void timer_animationTimer_Tick(object sender, EventArgs e) {
            if (this._animationCounter == this._planarObjectStore.AnimationList.Count - 1) {
                this._animationCounter = 0;
            } else {
                this._animationCounter++;
            }
            this.pictureBox_mainPic.Refresh();
        }
        private void pictureBox_mainPic_Paint(object sender, PaintEventArgs e) {
            Graphics gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gr.TranslateTransform(this._canvasOrgin.X, this._canvasOrgin.Y);

            switch (this._applicationState) {
                case ApplicationStateType.NodeGeneration:
                    this._planarObjectStore.DrawNodeList(ref gr, this.checkBox_labelVisibility.Checked);
                    break;
                case ApplicationStateType.GreedyTriangulation:
                    this._planarObjectStore.DrawTriangulation(ref gr, this.checkBox_labelVisibility.Checked, this.checkBox_tweenAnimation.Checked, ref this._animationCounter);
                    break;
                case ApplicationStateType.DelaunayTriangulation:
                    this._planarObjectStore.DrawTriangulation(ref gr, this.checkBox_labelVisibility.Checked, this.checkBox_tweenAnimation.Checked, ref this._animationCounter,
                                                                this.checkBox_meshVisibility.Checked, this.checkBox_circumcircleVisibility.Checked);
                    break;
                case ApplicationStateType.MeshRefinement:
                    this._planarObjectStore.DrawTriangulation(ref gr, this.checkBox_labelVisibility.Checked, this.checkBox_tweenAnimation.Checked, ref this._animationCounter,
                                                                this.checkBox_meshVisibility.Checked);
                    break;
            }
        }

        #region CheckBoxs
        private void checkBox_labelVisibility_CheckedChanged(object sender, EventArgs e) {
            this.pictureBox_mainPic.Refresh();
        }
        private void checkBox_circumcircleVisibility_CheckedChanged(object sender, EventArgs e) {
            this.pictureBox_mainPic.Refresh();
        }
        private void checkBox_tweenAnimation_CheckedChanged(object sender, EventArgs e) {
            if (this.checkBox_tweenAnimation.Checked) {
                this._animationCounter = 0;
                this.timer_animationTimer.Interval = this._timerInterval;
                this.timer_animationTimer.Enabled = true;
                this.timer_animationTimer.Start();
            } else {
                this.timer_animationTimer.Enabled = false;
                this.timer_animationTimer.Stop();
            }
            this.pictureBox_mainPic.Refresh();
        }
        private void checkBox_meshVisibility_CheckedChanged(object sender, EventArgs e) {
            this.pictureBox_mainPic.Refresh();
        }
        #endregion

        #region Buttons
        private void button_generateNodes_Click(object sender, EventArgs e) {
            this.checkBox_meshVisibility.Checked = false;
            this.checkBox_meshVisibility.Enabled = true;
            this.checkBox_circumcircleVisibility.Checked = false;
            this.checkBox_circumcircleVisibility.Enabled = true;
            GenerateRandomNodes();
        }
        private void button_greedyTriangulation_Click(object sender, EventArgs e) {
            this.checkBox_meshVisibility.Checked = false;
            this.checkBox_meshVisibility.Enabled = false;
            this.checkBox_circumcircleVisibility.Checked = false;
            this.checkBox_circumcircleVisibility.Enabled = false;
            GreedyTriangulation();
        }
        private void button_delaunayTriangulation_Click(object sender, EventArgs e) {
            this.checkBox_meshVisibility.Enabled = true;
            this.checkBox_circumcircleVisibility.Enabled = true;
            DelaunayTriangulation();
        }
        private void button_meshRefinement_Click(object sender, EventArgs e) {
            this.checkBox_circumcircleVisibility.Checked = false;
            this.checkBox_circumcircleVisibility.Enabled = false;
            MeshRefinement();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Инициализация используемой области в pictureBox_mainPic и точки начала координат 
        /// </summary>
        private void InitializeCanvasSize() {
            this._canvasSize = new SizeF((float)(this.pictureBox_mainPic.Width * 0.9), (float)(this.pictureBox_mainPic.Height * 0.9));
            this._canvasOrgin = new PointF((float)(this.pictureBox_mainPic.Width * 0.5), (float)(this.pictureBox_mainPic.Height * 0.5));
            this.pictureBox_mainPic.Refresh();
        }
        /// <summary>
        /// Random-генерация узлов
        /// </summary>
        private void GenerateRandomNodes() {
            this._applicationState = ApplicationStateType.NodeGeneration;
            int nodeCount = (int)this.numericUpDown_numberOfNodes.Value;

            this._nodeStoreList = new List<Triangulation.MeshStore.NodeStore>();
            this._edgeStoreList = new List<Triangulation.MeshStore.EdgeStore>();
            this._triangleStoreList = new List<Triangulation.MeshStore.TriangleStore>();

            this._planarObjectStore = new PlanarObjectStore();
            List<PlanarObjectStore.Node> tempNodeList = new List<PlanarObjectStore.Node>();

            Random random = new Random();

            #region hf_1.1 хотфикс на лимит 
            int xCoordLimit = (int)(this._canvasSize.Width * 0.5);
            int yCoordLimit = (int)(this._canvasSize.Height * 0.5);

            //int xCoordLimit = 10;
            //int yCoordLimit = 10;
            #endregion

            tempNodeList.Add(new PlanarObjectStore.Node(0, -xCoordLimit, -yCoordLimit));
            tempNodeList.Add(new PlanarObjectStore.Node(1, -xCoordLimit, yCoordLimit));
            tempNodeList.Add(new PlanarObjectStore.Node(2, xCoordLimit, yCoordLimit));
            tempNodeList.Add(new PlanarObjectStore.Node(3, xCoordLimit, -yCoordLimit));

            if (nodeCount > 4) {
                for (int j = 4; j < nodeCount; j++) {
                    double tmpXCoord = Math.Round((float)random.Next(-xCoordLimit * 100, xCoordLimit * 100) / 100, this._decimalPlaces, MidpointRounding.AwayFromZero);
                    double tmpYCoord = Math.Round((float)random.Next(-yCoordLimit * 100, yCoordLimit * 100) / 100, this._decimalPlaces, MidpointRounding.AwayFromZero);

                    if (tempNodeList.Exists(obj => obj.XCoordinate == tmpXCoord && obj.YCoordinate == tmpYCoord)) {
                        j--;
                        continue;
                    }
                    tempNodeList.Add(new PlanarObjectStore.Node(j, tmpXCoord, tmpYCoord));
                }
            }
            this._planarObjectStore.NodeList = tempNodeList;
            this.pictureBox_mainPic.Refresh();
        }
        private void GreedyTriangulation() {
            if (this._applicationState == ApplicationStateType.MeshRefinement) {
                MessageBox.Show("Perform node generation before Greedy triangulation.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            this._applicationState = ApplicationStateType.GreedyTriangulation;
            this._animationCounter = 0;
            this._planarObjectStore.EdgeList = new List<PlanarObjectStore.Edge>();
            this._planarObjectStore.AnimationList = new List<PlanarObjectStore.TweenAnimation>();

            List<PlanarObjectStore.Edge> tempEdgeList = new List<PlanarObjectStore.Edge>();
            List<PlanarObjectStore.TweenAnimation> tempTrackerList = new List<PlanarObjectStore.TweenAnimation>();

            (new Triangulation()).GreedyTriangulationStart(this._planarObjectStore.NodeList, ref tempEdgeList, ref tempTrackerList);

            this._planarObjectStore.EdgeList = tempEdgeList;
            this._planarObjectStore.AnimationList = tempTrackerList;
            this.pictureBox_mainPic.Refresh();
        }
        private void DelaunayTriangulation() {
            if (this._applicationState == ApplicationStateType.MeshRefinement) {
                MessageBox.Show("Perform node generation before Delaunay triangulation.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            this._applicationState = ApplicationStateType.DelaunayTriangulation;
            this._animationCounter = 0;
            this._planarObjectStore.EdgeList = new List<PlanarObjectStore.Edge>();
            this._planarObjectStore.AnimationList = new List<PlanarObjectStore.TweenAnimation>();

            List<PlanarObjectStore.Edge> tempEdgeList = new List<PlanarObjectStore.Edge>();
            List<PlanarObjectStore.Triangle> tempTriangleList = new List<PlanarObjectStore.Triangle>();
            List<PlanarObjectStore.TweenAnimation> tempTrackerList = new List<PlanarObjectStore.TweenAnimation>();

            (new Triangulation()).DelaunayTriangulationStart(this._planarObjectStore.NodeList, ref tempEdgeList, ref tempTriangleList, ref tempTrackerList,
                                                             ref this._nodeStoreList, ref this._edgeStoreList, ref this._triangleStoreList);

            this._planarObjectStore.EdgeList = tempEdgeList;
            this._planarObjectStore.TriangleList = tempTriangleList;
            this._planarObjectStore.AnimationList = tempTrackerList;
            this.pictureBox_mainPic.Refresh();
        }
        private void MeshRefinement() {
            if (this._applicationState != ApplicationStateType.DelaunayTriangulation) {
                MessageBox.Show("Perform Delaunay triangulation before the mesh refinement method.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            this._applicationState = ApplicationStateType.MeshRefinement;
            this._meshRefinementCoeff = (int)this.numericUpDown_meshRefinementCoefficient.Value;
            this._animationCounter = 0;

            List<PlanarObjectStore.Node> tempNodeList = new List<PlanarObjectStore.Node>();
            List<PlanarObjectStore.Edge> tempEdgeList = new List<PlanarObjectStore.Edge>();
            List<PlanarObjectStore.Triangle> tempTriangleList = new List<PlanarObjectStore.Triangle>();
            List<PlanarObjectStore.TweenAnimation> tempTrackerList = new List<PlanarObjectStore.TweenAnimation>();
            PlanarObjectStore.TweenAnimation tempTracker = this._planarObjectStore.AnimationList.Last();

            (new Triangulation()).MeshRefinementStart(ref tempNodeList, ref tempEdgeList, ref tempTriangleList, ref tempTrackerList, tempTracker, this._decimalPlaces,
                                                      this._meshRefinementCoeff, this._nodeStoreList, this._edgeStoreList, this._triangleStoreList);

            this._planarObjectStore.NodeList = tempNodeList;
            this._planarObjectStore.EdgeList = tempEdgeList;
            this._planarObjectStore.TriangleList = tempTriangleList;
            this._planarObjectStore.AnimationList = tempTrackerList;
            this.pictureBox_mainPic.Refresh();
        }


        #endregion

        #region Test Buttons & Debug
#warning To do: remove from the final version with control (button_test visible = false)
        private void button_test_Click(object sender, EventArgs e) {
            //MessageBox.Show($"Count temp list {nameof(this._planarObjectStore.NodeList)}: {this._planarObjectStore.NodeList.Count}\n" +
            //                $"Count temp list {nameof(this._planarObjectStore.EdgeList)}: {this._planarObjectStore.EdgeList.Count}\n" +
            //                $"Count temp list {nameof(this._planarObjectStore.TriangleList)}: {this._planarObjectStore.TriangleList.Count}\n" +
            //                $"Count temp list {nameof(this._planarObjectStore.AnimationList)}: {this._planarObjectStore.AnimationList.Count}\n" +
            //                $"Count diff nodes: {this._planarObjectStore.NodeList.DistinctBy(obj => new { obj.XCoordinate, obj.YCoordinate }).Count()}\n"
            //                //+ $"All nodes\n{string.Join("\n", this._planarObjectStore.NodeList.OrderBy(obj => obj.XCoordinate).Select(obj => $"<{obj.NodeID}; ({obj.XCoordinate}; {obj.YCoordinate})>"))}\n"
            //                , "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            int numOX = 100;
            int numOy = 100;
            MessageBox.Show($"количествое разбиениий по оси х = {numOX - 1}, по оси  y= {numOy - 1}");
            ApproxFunction(numOX, numOy, _planarObjectStore);
        }
        #endregion


        #region new methods
        /// <summary>
        /// Вычисление лямбда функции
        /// </summary>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        /// <param name="thirdNode"></param>
        /// <returns></returns>
        private double[,] LambdaFunction(PlanarObjectStore.Node firstNode, PlanarObjectStore.Node secondNode, PlanarObjectStore.Node thirdNode) {
            double[,] lambda = new double[3, 3];

            lambda[0, 0] = 1;
            lambda[0, 1] = firstNode.XCoordinate;
            lambda[0, 2] = firstNode.YCoordinate;

            lambda[1, 0] = 1;
            lambda[1, 1] = secondNode.XCoordinate;
            lambda[1, 2] = secondNode.YCoordinate;

            lambda[2, 0] = 1;
            lambda[2, 1] = thirdNode.XCoordinate;
            lambda[2, 2] = thirdNode.YCoordinate;

            return lambda;
        }

        /// <summary>
        /// Вычисление обратной лямбда функции
        /// </summary>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        /// <param name="thirdNode"></param>
        /// <returns></returns>
        private double[,] InverseLambdaFunction(PlanarObjectStore.Node firstNode, PlanarObjectStore.Node secondNode, PlanarObjectStore.Node thirdNode) {
            double[,] inverseLambda = new double[3, 3];

            double x1 = firstNode.XCoordinate;
            double y1 = firstNode.YCoordinate;

            double x2 = secondNode.XCoordinate;
            double y2 = secondNode.YCoordinate;

            double x3 = thirdNode.XCoordinate;
            double y3 = thirdNode.YCoordinate;

            double det = x2 * y3 + x1 * y2 + y1 * x3 - y1 * x2 - y2 * x3 - x1 * y3;

            inverseLambda[0, 0] = (x2 * y3 - y2 * x3) / det;
            inverseLambda[0, 1] = (y1 * x3 - x1 * y3) / det;
            inverseLambda[0, 2] = (x1 * y2 - y1 * x2) / det;

            inverseLambda[1, 0] = (y2 - y3) / det;
            inverseLambda[1, 1] = (y3 - y1) / det;
            inverseLambda[1, 2] = (y1 - y2) / det;

            inverseLambda[2, 0] = (x3 - x2) / det;
            inverseLambda[2, 1] = (x1 - x3) / det;
            inverseLambda[2, 2] = (x2 - x1) / det;

            return inverseLambda;
        }



        /// <summary>
        ///                 data (1,x,y)   -> 1x3
        ///                 inverseLambda  -> 3x3
        ///                 functionValues -> 3x1
        ///                 
        ///                 tmp = data x inverseLambda -> 1x3
        /// </summary>
        /// <param name="data"></param>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        /// <param name="thirdNode"></param>
        /// <param name="functionValues"></param>
        /// <returns></returns>
        private double GJFunction(double[] data, PlanarObjectStore.Node firstNode, PlanarObjectStore.Node secondNode, PlanarObjectStore.Node thirdNode, double[] functionValues) {
            double result = 0;
            double[] tmp = new double[3];

            double[,] inverseLambda = InverseLambdaFunction(firstNode, secondNode, thirdNode);

            for (int j = 0; j < 3; j++) {
                tmp[j] = 0;
                for (int k = 0; k < 3; k++) {
                    tmp[j] += data[k] * inverseLambda[k, j];
                }
            }
            for (int j = 0; j < 3; j++) {
                result += tmp[j] * functionValues[j];
            }
            return result;
        }

        private double GJFunctionTwo(double[] data, PlanarObjectStore.Node firstNode, PlanarObjectStore.Node secondNode, PlanarObjectStore.Node thirdNode, double[] functionValues) {
            double result = 0;
            double[] tmp = new double[3];

            double[,] inverseLambda = InverseLambdaFunction(firstNode, secondNode, thirdNode);

            for (int j = 0; j < 3; j++) {
                tmp[j] = 0;
                for (int k = 0; k < 3; k++) {
                    tmp[j] += functionValues[k] * inverseLambda[k, j];
                }
            }
            for (int j = 0; j < 3; j++) {
                result += tmp[j] * data[j];
            }
            return result;
        }

        /// <summary>
        /// Вычисление взвешенных расстояний
        /// </summary>
        /// <param name="curXCoord"></param>
        /// <param name="curYCoord"></param>
        /// <param name="midXCoord"></param>
        /// <param name="midYCoord"></param>
        /// <returns></returns>
        private double WJFunction(double curXCoord, double curYCoord, double midXCoord, double midYCoord) {
#warning Переключил на второй кейс, т.к. первый кейс некорректен
            //return Math.Sqrt((curXCoord - midXCoord) * (curXCoord - midXCoord) + (curYCoord - midYCoord) * (curYCoord - midYCoord));
            return 1 / ((curXCoord - midXCoord) * (curXCoord - midXCoord) + (curYCoord - midYCoord) * (curYCoord - midYCoord));
        }

        private double GFunction(double curX, double curY, List<PlanarObjectStore.Triangle> triangleList) {

            double result = 0;
            double w_cur = 0;
            double w_sum = 0;
            double[] data = new double[] { 1, curX, curY };
#warning добавить массив длиной 8 для координат узлов и центра и переписать убрав кастомные классы из параметоров сзявных функций GJFunction
            for (int j = 0; j < triangleList.Count; j++) {
                w_cur = WJFunction(curX, curY, triangleList[j].GeometricCenter.XCoordinate, triangleList[j].GeometricCenter.YCoordinate);

                double[] functionValues = new double[] {
                    ExactSolution(triangleList[j].FirstNode.XCoordinate,triangleList[j].FirstNode.YCoordinate),
                    ExactSolution(triangleList[j].SecondNode.XCoordinate,triangleList[j].SecondNode.YCoordinate),
                    ExactSolution(triangleList[j].ThirdNode.XCoordinate,triangleList[j].ThirdNode.YCoordinate)
                };
                w_sum += w_cur;
#warning Переключил на перйвый кейс, т.к. второй - точно ошибочный
                result += w_cur * GJFunction(data, triangleList[j].FirstNode, triangleList[j].SecondNode, triangleList[j].ThirdNode, functionValues);
            }

            return result /= w_sum;
        }

        private void ApproxFunction(int nodeNum_oX, int nodeNum_oY, PlanarObjectStore planarObjectStore) {

            #region hf_1.2 хотфикс на лимит 
            int xCoordLimit = (int)(this._canvasSize.Width * 0.5);
            int yCoordLimit = (int)(this._canvasSize.Height * 0.5);

            double h_oX = this._canvasSize.Width / (nodeNum_oX - 1);
            double h_oY = this._canvasSize.Height / (nodeNum_oY - 1);

            double[] xVal = new double[nodeNum_oX * nodeNum_oY];
            double[] yVal = new double[nodeNum_oX * nodeNum_oY];
            double[] zVal = new double[nodeNum_oX * nodeNum_oY];
            double[] exactVal = new double[nodeNum_oX * nodeNum_oY];
            int index = 0;

            //for (double j = -xCoordLimit; j <= xCoordLimit; j += h_oX) {
            //    for (double k = -yCoordLimit; k <= yCoordLimit; k += h_oY) {
            //        xVal[index] = j;
            //        yVal[index] = k;
            //        zVal[index] = GFunction(j, k, planarObjectStore.TriangleList);
            //        exactVal[index] = ExactSolution(j, k);
            //        index++;
            //    }
            //}

            //double xCoordLimit = 10.0;
            //double yCoordLimit = 10.0;

            //double h_oX = 20.0 / (nodeNum_oX - 1);
            //double h_oY = 20.0 / (nodeNum_oY - 1);

            //double[] xVal = new double[nodeNum_oX * nodeNum_oY];
            //double[] yVal = new double[nodeNum_oX * nodeNum_oY];
            //double[] zVal = new double[nodeNum_oX * nodeNum_oY];
            //double[] exactVal = new double[nodeNum_oX * nodeNum_oY];
            //int index = 0;

            for (double j = 0; j < nodeNum_oX; j++) {
                for (double k = 0; k < nodeNum_oY; k++) {
                    xVal[index] = -xCoordLimit + j * h_oX;
                    yVal[index] = -yCoordLimit + k * h_oY;
                    zVal[index] = GFunction(xVal[index], yVal[index], planarObjectStore.TriangleList);
                    exactVal[index] = ExactSolution(xVal[index], yVal[index]);
                    index++;
                }
            }
            #endregion

            GnuPlot.Set("dgrid3d 40,40,2");
            GnuPlot.WriteLine($"set xrange[{-xCoordLimit}:{xCoordLimit}]");
            GnuPlot.WriteLine($"set yrange[{-yCoordLimit}:{yCoordLimit}]");
            //GnuPlot.Set("zrange[-50:50]");
            GnuPlot.HoldOn();


            int numC = planarObjectStore.NodeList.Count;
            double[] xValQ = new double[numC];
            double[] yValQ = new double[numC];
            double[] fValQ = new double[numC];
            for (int indexx = 0; indexx < numC; indexx++) {
                xValQ[indexx] = planarObjectStore.NodeList[indexx].XCoordinate;
                yValQ[indexx] = planarObjectStore.NodeList[indexx].YCoordinate;
                fValQ[indexx] = ExactSolution(xValQ[indexx], yValQ[indexx]);
            }
            GnuPlot.SPlot(xValQ, yValQ, fValQ);
            MessageBox.Show("qqwe");
            GnuPlot.SPlot(xVal, yVal, zVal);
            //GnuPlot.SPlot(xVal, yVal, exactVal, "with pm3d");
        }


        #endregion
    }
}
