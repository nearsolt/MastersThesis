namespace MastersThesis {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e) {
            this._planarObjectStoreInstance = new PlanarObjectStore();
            InitializeCanvasSize();
            GenerateRandomNodes();
        }
        private void pictureBox_mainPic_SizeChanged(object sender, EventArgs e) {
            InitializeCanvasSize();
        }
        private void timer_animationTimer_Tick(object sender, EventArgs e) {
            if (this._counterInstance == this._planarObjectStoreInstance.TrackerList.Count - 1) {
                this._counterInstance = 0;
            } else {
                this._counterInstance++;
            }
            this.pictureBox_mainPic.Refresh();
        }
        private void pictureBox_mainPic_Paint(object sender, PaintEventArgs e) {
            Graphics gr = e.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gr.TranslateTransform(this._canvasOrgin.X, this._canvasOrgin.Y);

            switch (this._triangulationType) {
                case TriangulationType.NodeGeneration:
                    this._planarObjectStoreInstance.DrawPicture(ref gr, this.checkBox_labelVisibility.Checked);
                    break;
                case TriangulationType.GreedyTriangulation:
                    this._planarObjectStoreInstance.DrawPicture(ref gr, this.checkBox_labelVisibility.Checked, this.checkBox_tweenAnimation.Checked, ref this._counterInstance);
                    break;
                case TriangulationType.DelaunayTriangulation:
                    this._planarObjectStoreInstance.DrawPicture(ref gr, this.checkBox_labelVisibility.Checked, this.checkBox_tweenAnimation.Checked, ref this._counterInstance,
                                                                this.checkBox_meshVisibility.Checked, this.checkBox_circumcircleVisibility.Checked);
                    break;
                case TriangulationType.MeshRefinement:
                    this._planarObjectStoreInstance.DrawPicture(ref gr, this.checkBox_labelVisibility.Checked, this.checkBox_tweenAnimation.Checked, ref this._counterInstance,
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
                this._counterInstance = 0;
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
        private void InitializeCanvasSize() {
            this._canvasSize = new SizeF((float)(this.pictureBox_mainPic.Width * 0.9), (float)(this.pictureBox_mainPic.Height * 0.9));
            this._canvasOrgin = new PointF((float)(this.pictureBox_mainPic.Width * 0.5), (float)(this.pictureBox_mainPic.Height * 0.5));
            this.pictureBox_mainPic.Refresh();
        }
        private void GenerateRandomNodes() {
            this._triangulationType = TriangulationType.NodeGeneration;
            int nodeCount = (int)this.numericUpDown_numberOfNodes.Value;

            this._nodeStoreList = new List<Triangulation.MeshStore.Node2DStore>();
            this._edgeStoreList = new List<Triangulation.MeshStore.Edge2DStore>();
            this._triangleStoreList = new List<Triangulation.MeshStore.Triangle2DStore>();

            this._planarObjectStoreInstance = new PlanarObjectStore();
            List<PlanarObjectStore.Node2D> tempNodeList = new List<PlanarObjectStore.Node2D>();

            Random random = new Random();
            int xCoordLimit = (int)(this._canvasSize.Width * 0.5);
            int yCoordLimit = (int)(this._canvasSize.Height * 0.5);

            do {
                for (int j = 0; j < nodeCount; j++) {
                    PointF randPoint = new PointF(random.Next(-xCoordLimit * 100, xCoordLimit * 100), random.Next(-yCoordLimit * 100, yCoordLimit * 100));
                    tempNodeList.Add(new PlanarObjectStore.Node2D(j, Math.Round(randPoint.X / 100, this._decimalPlaces, MidpointRounding.AwayFromZero),
                                                                  Math.Round(randPoint.Y / 100, this._decimalPlaces, MidpointRounding.AwayFromZero)));
                }
                tempNodeList = tempNodeList.Distinct().ToList();
                nodeCount -= tempNodeList.Count;

            } while (nodeCount > 0);
            this._planarObjectStoreInstance.NodeList = tempNodeList;
            this.pictureBox_mainPic.Refresh();
        }
        private void GreedyTriangulation() {
            if (this._triangulationType == TriangulationType.MeshRefinement) {
                MessageBox.Show("Perform node generation before Greedy triangulation.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            this._triangulationType = TriangulationType.GreedyTriangulation;
            this._counterInstance = 0;
            this._planarObjectStoreInstance.EdgeList = new List<PlanarObjectStore.Edge2D>();
            this._planarObjectStoreInstance.TrackerList = new List<PlanarObjectStore.AnimationTracker>();

            List<PlanarObjectStore.Edge2D> tempEdgeList = new List<PlanarObjectStore.Edge2D>();
            List<PlanarObjectStore.AnimationTracker> tempTrackerList = new List<PlanarObjectStore.AnimationTracker>();

            (new Triangulation()).GreedyTriangulationStart(this._planarObjectStoreInstance.NodeList, ref tempEdgeList, ref tempTrackerList);

            this._planarObjectStoreInstance.EdgeList = tempEdgeList;
            this._planarObjectStoreInstance.TrackerList = tempTrackerList;
            this.pictureBox_mainPic.Refresh();
        }
        private void DelaunayTriangulation() {
            if (this._triangulationType == TriangulationType.MeshRefinement) {
                MessageBox.Show("Perform node generation before Delaunay triangulation.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            this._triangulationType = TriangulationType.DelaunayTriangulation;
            this._counterInstance = 0;
            this._planarObjectStoreInstance.EdgeList = new List<PlanarObjectStore.Edge2D>();
            this._planarObjectStoreInstance.TrackerList = new List<PlanarObjectStore.AnimationTracker>();

            List<PlanarObjectStore.Edge2D> tempEdgeList = new List<PlanarObjectStore.Edge2D>();
            List<PlanarObjectStore.Triangle2D> tempTriangleList = new List<PlanarObjectStore.Triangle2D>();
            List<PlanarObjectStore.AnimationTracker> tempTrackerList = new List<PlanarObjectStore.AnimationTracker>();

            (new Triangulation()).DelaunayTriangulationStart(this._planarObjectStoreInstance.NodeList, ref tempEdgeList, ref tempTriangleList, ref tempTrackerList,
                                                             ref this._nodeStoreList, ref this._edgeStoreList, ref this._triangleStoreList);

            this._planarObjectStoreInstance.EdgeList = tempEdgeList;
            this._planarObjectStoreInstance.TriangleList = tempTriangleList;
            this._planarObjectStoreInstance.TrackerList = tempTrackerList;
            this.pictureBox_mainPic.Refresh();
        }
        private void MeshRefinement() {
            if (this._triangulationType != TriangulationType.DelaunayTriangulation) {
                MessageBox.Show("Perform Delaunay triangulation before the mesh refinement method.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            this._triangulationType = TriangulationType.MeshRefinement;
            this._meshRefinementCoefficient = (int)this.numericUpDown_meshRefinementCoefficient.Value;
            this._counterInstance = 0;

            List<PlanarObjectStore.Node2D> tempNodeList = new List<PlanarObjectStore.Node2D>();
            List<PlanarObjectStore.Edge2D> tempEdgeList = new List<PlanarObjectStore.Edge2D>();
            List<PlanarObjectStore.Triangle2D> tempTriangleList = new List<PlanarObjectStore.Triangle2D>();
            List<PlanarObjectStore.AnimationTracker> tempTrackerList = new List<PlanarObjectStore.AnimationTracker>();
            PlanarObjectStore.AnimationTracker tempTracker = this._planarObjectStoreInstance.TrackerList.LastOrDefault();

            (new Triangulation()).MeshRefinementStart(ref tempNodeList, ref tempEdgeList, ref tempTriangleList, ref tempTrackerList, tempTracker, this._decimalPlaces,
                                                      this._meshRefinementCoefficient, this._nodeStoreList, this._edgeStoreList, this._triangleStoreList);

            this._planarObjectStoreInstance.NodeList = tempNodeList;
            this._planarObjectStoreInstance.EdgeList = tempEdgeList;
            this._planarObjectStoreInstance.TriangleList = tempTriangleList;
            this._planarObjectStoreInstance.TrackerList = tempTrackerList;
            this.pictureBox_mainPic.Refresh();
        }
        #endregion

        #region Test Buttons & Debug
#warning To do: remove from the final version with control (button_test visible = false)
        private void button_test_Click(object sender, EventArgs e) {
            MessageBox.Show($"Count temp list {nameof(this._planarObjectStoreInstance.NodeList)}: {this._planarObjectStoreInstance.NodeList.Count}\n" +
                            $"Count temp list {nameof(this._planarObjectStoreInstance.EdgeList)}: {this._planarObjectStoreInstance.EdgeList.Count}\n" +
                            $"Count temp list {nameof(this._planarObjectStoreInstance.TriangleList)}: {this._planarObjectStoreInstance.TriangleList.Count}\n" +
                            $"Count temp list {nameof(this._planarObjectStoreInstance.TrackerList)}: {this._planarObjectStoreInstance.TrackerList.Count}\n" +
                            $"Count diff nodes: {this._planarObjectStoreInstance.NodeList.DistinctBy(obj => new { obj.XCoordinate, obj.YCoordinate }).Count()}\n" +
                            $"All nodes\n{string.Join("\n", this._planarObjectStoreInstance.NodeList.OrderBy(obj => obj.XCoordinate).Select(obj => $"<{obj.NodeID}; ({obj.XCoordinate}; {obj.YCoordinate})>"))}\n",
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

        }
        #endregion
    }
}
