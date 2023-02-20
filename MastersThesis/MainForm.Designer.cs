namespace MastersThesis {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox_ribbon = new System.Windows.Forms.GroupBox();
            this.pictureBox_mainPic = new System.Windows.Forms.PictureBox();
            this.label_numberOfNodes = new System.Windows.Forms.Label();
            this.label_meshRefinementCoefficient = new System.Windows.Forms.Label();
            this.button_generateNodes = new System.Windows.Forms.Button();
            this.button_greedyTriangulation = new System.Windows.Forms.Button();
            this.button_delaunayTriangulation = new System.Windows.Forms.Button();
            this.button_meshRefinement = new System.Windows.Forms.Button();
            this.button_test = new System.Windows.Forms.Button();
            this.numericUpDown_numberOfNodes = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_meshRefinementCoefficient = new System.Windows.Forms.NumericUpDown();
            this.checkBox_labelVisibility = new System.Windows.Forms.CheckBox();
            this.checkBox_meshVisibility = new System.Windows.Forms.CheckBox();
            this.checkBox_circumcircleVisibility = new System.Windows.Forms.CheckBox();
            this.checkBox_tweenAnimation = new System.Windows.Forms.CheckBox();
            this.timer_animationTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox_ribbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_mainPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numberOfNodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_meshRefinementCoefficient)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_ribbon
            // 
            this.groupBox_ribbon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_ribbon.Controls.Add(this.checkBox_tweenAnimation);
            this.groupBox_ribbon.Controls.Add(this.checkBox_circumcircleVisibility);
            this.groupBox_ribbon.Controls.Add(this.checkBox_meshVisibility);
            this.groupBox_ribbon.Controls.Add(this.checkBox_labelVisibility);
            this.groupBox_ribbon.Controls.Add(this.numericUpDown_meshRefinementCoefficient);
            this.groupBox_ribbon.Controls.Add(this.numericUpDown_numberOfNodes);
            this.groupBox_ribbon.Controls.Add(this.button_test);
            this.groupBox_ribbon.Controls.Add(this.button_meshRefinement);
            this.groupBox_ribbon.Controls.Add(this.button_delaunayTriangulation);
            this.groupBox_ribbon.Controls.Add(this.button_greedyTriangulation);
            this.groupBox_ribbon.Controls.Add(this.button_generateNodes);
            this.groupBox_ribbon.Controls.Add(this.label_meshRefinementCoefficient);
            this.groupBox_ribbon.Controls.Add(this.label_numberOfNodes);
            this.groupBox_ribbon.Location = new System.Drawing.Point(5, 0);
            this.groupBox_ribbon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox_ribbon.Name = "groupBox_ribbon";
            this.groupBox_ribbon.Size = new System.Drawing.Size(850, 90);
            this.groupBox_ribbon.TabIndex = 0;
            this.groupBox_ribbon.TabStop = false;
            // 
            // pictureBox_mainPic
            // 
            this.pictureBox_mainPic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_mainPic.Location = new System.Drawing.Point(5, 95);
            this.pictureBox_mainPic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox_mainPic.Name = "pictureBox_mainPic";
            this.pictureBox_mainPic.Size = new System.Drawing.Size(850, 430);
            this.pictureBox_mainPic.TabIndex = 1;
            this.pictureBox_mainPic.TabStop = false;
            this.pictureBox_mainPic.SizeChanged += new System.EventHandler(this.pictureBox_mainPic_SizeChanged);
            this.pictureBox_mainPic.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_mainPic_Paint);
            // 
            // label_numberOfNodes
            // 
            this.label_numberOfNodes.AutoSize = true;
            this.label_numberOfNodes.Location = new System.Drawing.Point(7, 18);
            this.label_numberOfNodes.Name = "label_numberOfNodes";
            this.label_numberOfNodes.Size = new System.Drawing.Size(103, 15);
            this.label_numberOfNodes.TabIndex = 0;
            this.label_numberOfNodes.Text = "Number of nodes:";
            // 
            // label_meshRefinementCoefficient
            // 
            this.label_meshRefinementCoefficient.AutoSize = true;
            this.label_meshRefinementCoefficient.Location = new System.Drawing.Point(190, 18);
            this.label_meshRefinementCoefficient.Name = "label_meshRefinementCoefficient";
            this.label_meshRefinementCoefficient.Size = new System.Drawing.Size(159, 15);
            this.label_meshRefinementCoefficient.TabIndex = 1;
            this.label_meshRefinementCoefficient.Text = "Mesh refinement coefficient:";
            // 
            // button_generateNodes
            // 
            this.button_generateNodes.Location = new System.Drawing.Point(7, 50);
            this.button_generateNodes.Name = "button_generateNodes";
            this.button_generateNodes.Size = new System.Drawing.Size(103, 26);
            this.button_generateNodes.TabIndex = 2;
            this.button_generateNodes.Text = "Generate nodes";
            this.button_generateNodes.UseVisualStyleBackColor = true;
            this.button_generateNodes.Click += new System.EventHandler(this.button_generateNodes_Click);
            // 
            // button_greedyTriangulation
            // 
            this.button_greedyTriangulation.Location = new System.Drawing.Point(120, 50);
            this.button_greedyTriangulation.Name = "button_greedyTriangulation";
            this.button_greedyTriangulation.Size = new System.Drawing.Size(140, 26);
            this.button_greedyTriangulation.TabIndex = 3;
            this.button_greedyTriangulation.Text = "Greedy triangulation";
            this.button_greedyTriangulation.UseVisualStyleBackColor = true;
            this.button_greedyTriangulation.Click += new System.EventHandler(this.button_greedyTriangulation_Click);
            // 
            // button_delaunayTriangulation
            // 
            this.button_delaunayTriangulation.Location = new System.Drawing.Point(270, 50);
            this.button_delaunayTriangulation.Name = "button_delaunayTriangulation";
            this.button_delaunayTriangulation.Size = new System.Drawing.Size(140, 26);
            this.button_delaunayTriangulation.TabIndex = 4;
            this.button_delaunayTriangulation.Text = "Delaunay triangulation";
            this.button_delaunayTriangulation.UseVisualStyleBackColor = true;
            this.button_delaunayTriangulation.Click += new System.EventHandler(this.button_delaunayTriangulation_Click);
            // 
            // button_meshRefinement
            // 
            this.button_meshRefinement.Location = new System.Drawing.Point(420, 50);
            this.button_meshRefinement.Name = "button_meshRefinement";
            this.button_meshRefinement.Size = new System.Drawing.Size(130, 26);
            this.button_meshRefinement.TabIndex = 5;
            this.button_meshRefinement.Text = "Mesh refinement";
            this.button_meshRefinement.UseVisualStyleBackColor = true;
            this.button_meshRefinement.Click += new System.EventHandler(this.button_meshRefinement_Click);
            // 
            // button_test
            // 
            this.button_test.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_test.Location = new System.Drawing.Point(575, 65);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(60, 20);
            this.button_test.TabIndex = 6;
            this.button_test.Text = "test";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // numericUpDown_numberOfNodes
            // 
            this.numericUpDown_numberOfNodes.Location = new System.Drawing.Point(115, 16);
            this.numericUpDown_numberOfNodes.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown_numberOfNodes.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_numberOfNodes.Name = "numericUpDown_numberOfNodes";
            this.numericUpDown_numberOfNodes.Size = new System.Drawing.Size(55, 23);
            this.numericUpDown_numberOfNodes.TabIndex = 7;
            this.numericUpDown_numberOfNodes.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDown_meshRefinementCoefficient
            // 
            this.numericUpDown_meshRefinementCoefficient.Location = new System.Drawing.Point(354, 16);
            this.numericUpDown_meshRefinementCoefficient.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_meshRefinementCoefficient.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_meshRefinementCoefficient.Name = "numericUpDown_meshRefinementCoefficient";
            this.numericUpDown_meshRefinementCoefficient.Size = new System.Drawing.Size(55, 23);
            this.numericUpDown_meshRefinementCoefficient.TabIndex = 8;
            this.numericUpDown_meshRefinementCoefficient.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // checkBox_labelVisibility
            // 
            this.checkBox_labelVisibility.AutoSize = true;
            this.checkBox_labelVisibility.Location = new System.Drawing.Point(575, 20);
            this.checkBox_labelVisibility.Name = "checkBox_labelVisibility";
            this.checkBox_labelVisibility.Size = new System.Drawing.Size(100, 19);
            this.checkBox_labelVisibility.TabIndex = 9;
            this.checkBox_labelVisibility.Text = "Label visibility";
            this.checkBox_labelVisibility.UseVisualStyleBackColor = true;
            this.checkBox_labelVisibility.CheckedChanged += new System.EventHandler(this.checkBox_labelVisibility_CheckedChanged);
            // 
            // checkBox_meshVisibility
            // 
            this.checkBox_meshVisibility.AutoSize = true;
            this.checkBox_meshVisibility.Location = new System.Drawing.Point(705, 20);
            this.checkBox_meshVisibility.Name = "checkBox_meshVisibility";
            this.checkBox_meshVisibility.Size = new System.Drawing.Size(101, 19);
            this.checkBox_meshVisibility.TabIndex = 10;
            this.checkBox_meshVisibility.Text = "Mesh visibility";
            this.checkBox_meshVisibility.UseVisualStyleBackColor = true;
            this.checkBox_meshVisibility.CheckedChanged += new System.EventHandler(this.checkBox_meshVisibility_CheckedChanged);
            // 
            // checkBox_circumcircleVisibility
            // 
            this.checkBox_circumcircleVisibility.AutoSize = true;
            this.checkBox_circumcircleVisibility.Location = new System.Drawing.Point(705, 45);
            this.checkBox_circumcircleVisibility.Name = "checkBox_circumcircleVisibility";
            this.checkBox_circumcircleVisibility.Size = new System.Drawing.Size(139, 19);
            this.checkBox_circumcircleVisibility.TabIndex = 11;
            this.checkBox_circumcircleVisibility.Text = "Circumcircle visibility";
            this.checkBox_circumcircleVisibility.UseVisualStyleBackColor = true;
            this.checkBox_circumcircleVisibility.CheckedChanged += new System.EventHandler(this.checkBox_circumcircleVisibility_CheckedChanged);
            // 
            // checkBox_tweenAnimation
            // 
            this.checkBox_tweenAnimation.AutoSize = true;
            this.checkBox_tweenAnimation.Location = new System.Drawing.Point(575, 45);
            this.checkBox_tweenAnimation.Name = "checkBox_tweenAnimation";
            this.checkBox_tweenAnimation.Size = new System.Drawing.Size(116, 19);
            this.checkBox_tweenAnimation.TabIndex = 12;
            this.checkBox_tweenAnimation.Text = "Tween animation";
            this.checkBox_tweenAnimation.UseVisualStyleBackColor = true;
            this.checkBox_tweenAnimation.CheckedChanged += new System.EventHandler(this.checkBox_tweenAnimation_CheckedChanged);
            // 
            // timer_animationTimer
            // 
            this.timer_animationTimer.Tick += new System.EventHandler(this.timer_animationTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 533);
            this.Controls.Add(this.pictureBox_mainPic);
            this.Controls.Add(this.groupBox_ribbon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(877, 572);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Construction of planar triangulations (nearsolt)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox_ribbon.ResumeLayout(false);
            this.groupBox_ribbon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_mainPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numberOfNodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_meshRefinementCoefficient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox_ribbon;
        private PictureBox pictureBox_mainPic;
        private Button button_generateNodes;
        private Label label_meshRefinementCoefficient;
        private Label label_numberOfNodes;
        private Button button_greedyTriangulation;
        private Button button_delaunayTriangulation;
        private Button button_test;
        private Button button_meshRefinement;
        private NumericUpDown numericUpDown_meshRefinementCoefficient;
        private NumericUpDown numericUpDown_numberOfNodes;
        private CheckBox checkBox_labelVisibility;
        private CheckBox checkBox_meshVisibility;
        private CheckBox checkBox_tweenAnimation;
        private CheckBox checkBox_circumcircleVisibility;
        private System.Windows.Forms.Timer timer_animationTimer;
    }
}