﻿namespace MastersThesis {
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
            this.groupBox_axisNum = new System.Windows.Forms.GroupBox();
            this.checkBox_combinedGraphs = new System.Windows.Forms.CheckBox();
            this.numericUpDown_xAxisNum = new System.Windows.Forms.NumericUpDown();
            this.label_yAxisNum = new System.Windows.Forms.Label();
            this.numericUpDown_yAxisNum = new System.Windows.Forms.NumericUpDown();
            this.label_xAxisNum = new System.Windows.Forms.Label();
            this.groupBox_domainOfDefinition = new System.Windows.Forms.GroupBox();
            this.numericUpDown_xAxisStart = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_xAxisEnd = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_yAxisStart = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_yAxisEnd = new System.Windows.Forms.NumericUpDown();
            this.label_yAxis = new System.Windows.Forms.Label();
            this.label_xAxis = new System.Windows.Forms.Label();
            this.button_interpolation = new System.Windows.Forms.Button();
            this.checkBox_setDomainOfDefinition = new System.Windows.Forms.CheckBox();
            this.checkBox_tweenAnimation = new System.Windows.Forms.CheckBox();
            this.checkBox_circumcircleVisibility = new System.Windows.Forms.CheckBox();
            this.checkBox_innerTriangleVisibility = new System.Windows.Forms.CheckBox();
            this.checkBox_labelVisibility = new System.Windows.Forms.CheckBox();
            this.numericUpDown_meshRefinementCoeff = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_numberOfNodes = new System.Windows.Forms.NumericUpDown();
            this.button_meshRefinement = new System.Windows.Forms.Button();
            this.button_delaunayTriangulation = new System.Windows.Forms.Button();
            this.button_greedyTriangulation = new System.Windows.Forms.Button();
            this.button_generateNodes = new System.Windows.Forms.Button();
            this.label_meshRefinementCoeff = new System.Windows.Forms.Label();
            this.label_numberOfNodes = new System.Windows.Forms.Label();
            this.pictureBox_mainPic = new System.Windows.Forms.PictureBox();
            this.timer_animationTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox_ribbon.SuspendLayout();
            this.groupBox_axisNum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_xAxisNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_yAxisNum)).BeginInit();
            this.groupBox_domainOfDefinition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_xAxisStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_xAxisEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_yAxisStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_yAxisEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_meshRefinementCoeff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numberOfNodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_mainPic)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_ribbon
            // 
            this.groupBox_ribbon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_ribbon.Controls.Add(this.groupBox_axisNum);
            this.groupBox_ribbon.Controls.Add(this.groupBox_domainOfDefinition);
            this.groupBox_ribbon.Controls.Add(this.button_interpolation);
            this.groupBox_ribbon.Controls.Add(this.checkBox_setDomainOfDefinition);
            this.groupBox_ribbon.Controls.Add(this.checkBox_tweenAnimation);
            this.groupBox_ribbon.Controls.Add(this.checkBox_circumcircleVisibility);
            this.groupBox_ribbon.Controls.Add(this.checkBox_innerTriangleVisibility);
            this.groupBox_ribbon.Controls.Add(this.checkBox_labelVisibility);
            this.groupBox_ribbon.Controls.Add(this.numericUpDown_meshRefinementCoeff);
            this.groupBox_ribbon.Controls.Add(this.numericUpDown_numberOfNodes);
            this.groupBox_ribbon.Controls.Add(this.button_meshRefinement);
            this.groupBox_ribbon.Controls.Add(this.button_delaunayTriangulation);
            this.groupBox_ribbon.Controls.Add(this.button_greedyTriangulation);
            this.groupBox_ribbon.Controls.Add(this.button_generateNodes);
            this.groupBox_ribbon.Controls.Add(this.label_meshRefinementCoeff);
            this.groupBox_ribbon.Controls.Add(this.label_numberOfNodes);
            this.groupBox_ribbon.Location = new System.Drawing.Point(5, 0);
            this.groupBox_ribbon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox_ribbon.Name = "groupBox_ribbon";
            this.groupBox_ribbon.Size = new System.Drawing.Size(805, 110);
            this.groupBox_ribbon.TabIndex = 0;
            this.groupBox_ribbon.TabStop = false;
            // 
            // groupBox_axisNum
            // 
            this.groupBox_axisNum.Controls.Add(this.checkBox_combinedGraphs);
            this.groupBox_axisNum.Controls.Add(this.numericUpDown_xAxisNum);
            this.groupBox_axisNum.Controls.Add(this.label_yAxisNum);
            this.groupBox_axisNum.Controls.Add(this.numericUpDown_yAxisNum);
            this.groupBox_axisNum.Controls.Add(this.label_xAxisNum);
            this.groupBox_axisNum.Enabled = false;
            this.groupBox_axisNum.Location = new System.Drawing.Point(481, 67);
            this.groupBox_axisNum.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox_axisNum.Name = "groupBox_axisNum";
            this.groupBox_axisNum.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox_axisNum.Size = new System.Drawing.Size(320, 40);
            this.groupBox_axisNum.TabIndex = 19;
            this.groupBox_axisNum.TabStop = false;
            // 
            // checkBox_combinedGraphs
            // 
            this.checkBox_combinedGraphs.AutoSize = true;
            this.checkBox_combinedGraphs.Location = new System.Drawing.Point(194, 13);
            this.checkBox_combinedGraphs.Name = "checkBox_combinedGraphs";
            this.checkBox_combinedGraphs.Size = new System.Drawing.Size(121, 19);
            this.checkBox_combinedGraphs.TabIndex = 26;
            this.checkBox_combinedGraphs.Text = "Combined graphs";
            this.checkBox_combinedGraphs.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_xAxisNum
            // 
            this.numericUpDown_xAxisNum.Location = new System.Drawing.Point(32, 12);
            this.numericUpDown_xAxisNum.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown_xAxisNum.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_xAxisNum.Name = "numericUpDown_xAxisNum";
            this.numericUpDown_xAxisNum.Size = new System.Drawing.Size(55, 23);
            this.numericUpDown_xAxisNum.TabIndex = 22;
            this.numericUpDown_xAxisNum.Value = new decimal(new int[] {
            101,
            0,
            0,
            0});
            // 
            // label_yAxisNum
            // 
            this.label_yAxisNum.AutoSize = true;
            this.label_yAxisNum.Location = new System.Drawing.Point(96, 14);
            this.label_yAxisNum.Name = "label_yAxisNum";
            this.label_yAxisNum.Size = new System.Drawing.Size(24, 15);
            this.label_yAxisNum.TabIndex = 25;
            this.label_yAxisNum.Text = "nY:";
            // 
            // numericUpDown_yAxisNum
            // 
            this.numericUpDown_yAxisNum.Location = new System.Drawing.Point(124, 12);
            this.numericUpDown_yAxisNum.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown_yAxisNum.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_yAxisNum.Name = "numericUpDown_yAxisNum";
            this.numericUpDown_yAxisNum.Size = new System.Drawing.Size(55, 23);
            this.numericUpDown_yAxisNum.TabIndex = 23;
            this.numericUpDown_yAxisNum.Value = new decimal(new int[] {
            101,
            0,
            0,
            0});
            // 
            // label_xAxisNum
            // 
            this.label_xAxisNum.AutoSize = true;
            this.label_xAxisNum.Location = new System.Drawing.Point(4, 14);
            this.label_xAxisNum.Name = "label_xAxisNum";
            this.label_xAxisNum.Size = new System.Drawing.Size(24, 15);
            this.label_xAxisNum.TabIndex = 24;
            this.label_xAxisNum.Text = "nX:";
            // 
            // groupBox_domainOfDefinition
            // 
            this.groupBox_domainOfDefinition.Controls.Add(this.numericUpDown_xAxisStart);
            this.groupBox_domainOfDefinition.Controls.Add(this.numericUpDown_xAxisEnd);
            this.groupBox_domainOfDefinition.Controls.Add(this.numericUpDown_yAxisStart);
            this.groupBox_domainOfDefinition.Controls.Add(this.numericUpDown_yAxisEnd);
            this.groupBox_domainOfDefinition.Controls.Add(this.label_yAxis);
            this.groupBox_domainOfDefinition.Controls.Add(this.label_xAxis);
            this.groupBox_domainOfDefinition.Enabled = false;
            this.groupBox_domainOfDefinition.Location = new System.Drawing.Point(102, 67);
            this.groupBox_domainOfDefinition.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox_domainOfDefinition.Name = "groupBox_domainOfDefinition";
            this.groupBox_domainOfDefinition.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox_domainOfDefinition.Size = new System.Drawing.Size(376, 40);
            this.groupBox_domainOfDefinition.TabIndex = 2;
            this.groupBox_domainOfDefinition.TabStop = false;
            // 
            // numericUpDown_xAxisStart
            // 
            this.numericUpDown_xAxisStart.DecimalPlaces = 2;
            this.numericUpDown_xAxisStart.Location = new System.Drawing.Point(52, 12);
            this.numericUpDown_xAxisStart.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown_xAxisStart.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.numericUpDown_xAxisStart.Name = "numericUpDown_xAxisStart";
            this.numericUpDown_xAxisStart.Size = new System.Drawing.Size(60, 23);
            this.numericUpDown_xAxisStart.TabIndex = 14;
            this.numericUpDown_xAxisStart.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // numericUpDown_xAxisEnd
            // 
            this.numericUpDown_xAxisEnd.DecimalPlaces = 2;
            this.numericUpDown_xAxisEnd.Location = new System.Drawing.Point(119, 12);
            this.numericUpDown_xAxisEnd.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown_xAxisEnd.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.numericUpDown_xAxisEnd.Name = "numericUpDown_xAxisEnd";
            this.numericUpDown_xAxisEnd.Size = new System.Drawing.Size(60, 23);
            this.numericUpDown_xAxisEnd.TabIndex = 15;
            this.numericUpDown_xAxisEnd.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDown_yAxisStart
            // 
            this.numericUpDown_yAxisStart.DecimalPlaces = 2;
            this.numericUpDown_yAxisStart.Location = new System.Drawing.Point(245, 12);
            this.numericUpDown_yAxisStart.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown_yAxisStart.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.numericUpDown_yAxisStart.Name = "numericUpDown_yAxisStart";
            this.numericUpDown_yAxisStart.Size = new System.Drawing.Size(60, 23);
            this.numericUpDown_yAxisStart.TabIndex = 16;
            this.numericUpDown_yAxisStart.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            // 
            // numericUpDown_yAxisEnd
            // 
            this.numericUpDown_yAxisEnd.DecimalPlaces = 2;
            this.numericUpDown_yAxisEnd.Location = new System.Drawing.Point(312, 12);
            this.numericUpDown_yAxisEnd.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown_yAxisEnd.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            -2147483648});
            this.numericUpDown_yAxisEnd.Name = "numericUpDown_yAxisEnd";
            this.numericUpDown_yAxisEnd.Size = new System.Drawing.Size(60, 23);
            this.numericUpDown_yAxisEnd.TabIndex = 17;
            this.numericUpDown_yAxisEnd.Value = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            // 
            // label_yAxis
            // 
            this.label_yAxis.AutoSize = true;
            this.label_yAxis.Location = new System.Drawing.Point(197, 14);
            this.label_yAxis.Name = "label_yAxis";
            this.label_yAxis.Size = new System.Drawing.Size(44, 15);
            this.label_yAxis.TabIndex = 20;
            this.label_yAxis.Text = "Y-Axis:";
            // 
            // label_xAxis
            // 
            this.label_xAxis.AutoSize = true;
            this.label_xAxis.Location = new System.Drawing.Point(4, 14);
            this.label_xAxis.Name = "label_xAxis";
            this.label_xAxis.Size = new System.Drawing.Size(44, 15);
            this.label_xAxis.TabIndex = 19;
            this.label_xAxis.Text = "X-Axis:";
            // 
            // button_interpolation
            // 
            this.button_interpolation.Location = new System.Drawing.Point(404, 12);
            this.button_interpolation.Name = "button_interpolation";
            this.button_interpolation.Size = new System.Drawing.Size(105, 26);
            this.button_interpolation.TabIndex = 18;
            this.button_interpolation.Text = "Interpolation";
            this.button_interpolation.UseVisualStyleBackColor = true;
            this.button_interpolation.Click += new System.EventHandler(this.Interpolation_Click);
            // 
            // checkBox_setDomainOfDefinition
            // 
            this.checkBox_setDomainOfDefinition.AutoSize = true;
            this.checkBox_setDomainOfDefinition.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox_setDomainOfDefinition.Location = new System.Drawing.Point(7, 71);
            this.checkBox_setDomainOfDefinition.Name = "checkBox_setDomainOfDefinition";
            this.checkBox_setDomainOfDefinition.Size = new System.Drawing.Size(91, 34);
            this.checkBox_setDomainOfDefinition.TabIndex = 13;
            this.checkBox_setDomainOfDefinition.Text = "Set domain \r\nof definition";
            this.checkBox_setDomainOfDefinition.UseVisualStyleBackColor = true;
            this.checkBox_setDomainOfDefinition.CheckedChanged += new System.EventHandler(this.SetDomainOfDefinition_CheckedChanged);
            // 
            // checkBox_tweenAnimation
            // 
            this.checkBox_tweenAnimation.AutoSize = true;
            this.checkBox_tweenAnimation.Location = new System.Drawing.Point(524, 45);
            this.checkBox_tweenAnimation.Name = "checkBox_tweenAnimation";
            this.checkBox_tweenAnimation.Size = new System.Drawing.Size(116, 19);
            this.checkBox_tweenAnimation.TabIndex = 12;
            this.checkBox_tweenAnimation.Text = "Tween animation";
            this.checkBox_tweenAnimation.UseVisualStyleBackColor = true;
            this.checkBox_tweenAnimation.CheckedChanged += new System.EventHandler(this.TweenAnimation_CheckedChanged);
            // 
            // checkBox_circumcircleVisibility
            // 
            this.checkBox_circumcircleVisibility.AutoSize = true;
            this.checkBox_circumcircleVisibility.Location = new System.Drawing.Point(648, 45);
            this.checkBox_circumcircleVisibility.Name = "checkBox_circumcircleVisibility";
            this.checkBox_circumcircleVisibility.Size = new System.Drawing.Size(139, 19);
            this.checkBox_circumcircleVisibility.TabIndex = 11;
            this.checkBox_circumcircleVisibility.Text = "Circumcircle visibility";
            this.checkBox_circumcircleVisibility.UseVisualStyleBackColor = true;
            this.checkBox_circumcircleVisibility.CheckedChanged += new System.EventHandler(this.CircumcircleVisibility_CheckedChanged);
            // 
            // checkBox_innerTriangleVisibility
            // 
            this.checkBox_innerTriangleVisibility.AutoSize = true;
            this.checkBox_innerTriangleVisibility.Location = new System.Drawing.Point(648, 19);
            this.checkBox_innerTriangleVisibility.Name = "checkBox_innerTriangleVisibility";
            this.checkBox_innerTriangleVisibility.Size = new System.Drawing.Size(142, 19);
            this.checkBox_innerTriangleVisibility.TabIndex = 10;
            this.checkBox_innerTriangleVisibility.Text = "Inner triangle visibility";
            this.checkBox_innerTriangleVisibility.UseVisualStyleBackColor = true;
            this.checkBox_innerTriangleVisibility.CheckedChanged += new System.EventHandler(this.InnerTriangleVisibility_CheckedChanged);
            // 
            // checkBox_labelVisibility
            // 
            this.checkBox_labelVisibility.AutoSize = true;
            this.checkBox_labelVisibility.Location = new System.Drawing.Point(524, 19);
            this.checkBox_labelVisibility.Name = "checkBox_labelVisibility";
            this.checkBox_labelVisibility.Size = new System.Drawing.Size(100, 19);
            this.checkBox_labelVisibility.TabIndex = 9;
            this.checkBox_labelVisibility.Text = "Label visibility";
            this.checkBox_labelVisibility.UseVisualStyleBackColor = true;
            this.checkBox_labelVisibility.CheckedChanged += new System.EventHandler(this.LabelVisibility_CheckedChanged);
            // 
            // numericUpDown_meshRefinementCoeff
            // 
            this.numericUpDown_meshRefinementCoeff.Location = new System.Drawing.Point(341, 13);
            this.numericUpDown_meshRefinementCoeff.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_meshRefinementCoeff.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_meshRefinementCoeff.Name = "numericUpDown_meshRefinementCoeff";
            this.numericUpDown_meshRefinementCoeff.Size = new System.Drawing.Size(55, 23);
            this.numericUpDown_meshRefinementCoeff.TabIndex = 8;
            this.numericUpDown_meshRefinementCoeff.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // numericUpDown_numberOfNodes
            // 
            this.numericUpDown_numberOfNodes.Location = new System.Drawing.Point(114, 13);
            this.numericUpDown_numberOfNodes.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown_numberOfNodes.Minimum = new decimal(new int[] {
            4,
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
            // button_meshRefinement
            // 
            this.button_meshRefinement.Location = new System.Drawing.Point(404, 42);
            this.button_meshRefinement.Name = "button_meshRefinement";
            this.button_meshRefinement.Size = new System.Drawing.Size(105, 26);
            this.button_meshRefinement.TabIndex = 5;
            this.button_meshRefinement.Text = "Mesh refinement";
            this.button_meshRefinement.UseVisualStyleBackColor = true;
            this.button_meshRefinement.Click += new System.EventHandler(this.MeshRefinement_Click);
            // 
            // button_delaunayTriangulation
            // 
            this.button_delaunayTriangulation.Location = new System.Drawing.Point(257, 42);
            this.button_delaunayTriangulation.Name = "button_delaunayTriangulation";
            this.button_delaunayTriangulation.Size = new System.Drawing.Size(135, 26);
            this.button_delaunayTriangulation.TabIndex = 4;
            this.button_delaunayTriangulation.Text = "Delaunay triangulation";
            this.button_delaunayTriangulation.UseVisualStyleBackColor = true;
            this.button_delaunayTriangulation.Click += new System.EventHandler(this.DelaunayTriangulation_Click);
            // 
            // button_greedyTriangulation
            // 
            this.button_greedyTriangulation.Location = new System.Drawing.Point(119, 42);
            this.button_greedyTriangulation.Name = "button_greedyTriangulation";
            this.button_greedyTriangulation.Size = new System.Drawing.Size(125, 26);
            this.button_greedyTriangulation.TabIndex = 3;
            this.button_greedyTriangulation.Text = "Greedy triangulation";
            this.button_greedyTriangulation.UseVisualStyleBackColor = true;
            this.button_greedyTriangulation.Click += new System.EventHandler(this.GreedyTriangulation_Click);
            // 
            // button_generateNodes
            // 
            this.button_generateNodes.Location = new System.Drawing.Point(7, 42);
            this.button_generateNodes.Name = "button_generateNodes";
            this.button_generateNodes.Size = new System.Drawing.Size(100, 26);
            this.button_generateNodes.TabIndex = 2;
            this.button_generateNodes.Text = "Generate nodes";
            this.button_generateNodes.UseVisualStyleBackColor = true;
            this.button_generateNodes.Click += new System.EventHandler(this.GenerateNodes_Click);
            // 
            // label_meshRefinementCoeff
            // 
            this.label_meshRefinementCoeff.AutoSize = true;
            this.label_meshRefinementCoeff.Location = new System.Drawing.Point(178, 15);
            this.label_meshRefinementCoeff.Name = "label_meshRefinementCoeff";
            this.label_meshRefinementCoeff.Size = new System.Drawing.Size(159, 15);
            this.label_meshRefinementCoeff.TabIndex = 1;
            this.label_meshRefinementCoeff.Text = "Mesh refinement coefficient:";
            // 
            // label_numberOfNodes
            // 
            this.label_numberOfNodes.AutoSize = true;
            this.label_numberOfNodes.Location = new System.Drawing.Point(7, 15);
            this.label_numberOfNodes.Name = "label_numberOfNodes";
            this.label_numberOfNodes.Size = new System.Drawing.Size(103, 15);
            this.label_numberOfNodes.TabIndex = 0;
            this.label_numberOfNodes.Text = "Number of nodes:";
            // 
            // pictureBox_mainPic
            // 
            this.pictureBox_mainPic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_mainPic.Location = new System.Drawing.Point(3, 115);
            this.pictureBox_mainPic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox_mainPic.Name = "pictureBox_mainPic";
            this.pictureBox_mainPic.Size = new System.Drawing.Size(805, 420);
            this.pictureBox_mainPic.TabIndex = 1;
            this.pictureBox_mainPic.TabStop = false;
            this.pictureBox_mainPic.Paint += new System.Windows.Forms.PaintEventHandler(this.MainPic_Paint);
            // 
            // timer_animationTimer
            // 
            this.timer_animationTimer.Tick += new System.EventHandler(this.AnimationTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 541);
            this.Controls.Add(this.pictureBox_mainPic);
            this.Controls.Add(this.groupBox_ribbon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(830, 580);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Planar Triangulation & Approximation & Interpolation (nearsolt)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox_ribbon.ResumeLayout(false);
            this.groupBox_ribbon.PerformLayout();
            this.groupBox_axisNum.ResumeLayout(false);
            this.groupBox_axisNum.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_xAxisNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_yAxisNum)).EndInit();
            this.groupBox_domainOfDefinition.ResumeLayout(false);
            this.groupBox_domainOfDefinition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_xAxisStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_xAxisEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_yAxisStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_yAxisEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_meshRefinementCoeff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numberOfNodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_mainPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox_ribbon;
        private PictureBox pictureBox_mainPic;
        private Button button_generateNodes;
        private Label label_meshRefinementCoeff;
        private Label label_numberOfNodes;
        private Button button_greedyTriangulation;
        private Button button_delaunayTriangulation;
        private Button button_meshRefinement;
        private NumericUpDown numericUpDown_meshRefinementCoeff;
        private NumericUpDown numericUpDown_numberOfNodes;
        private CheckBox checkBox_labelVisibility;
        private CheckBox checkBox_innerTriangleVisibility;
        private CheckBox checkBox_tweenAnimation;
        private CheckBox checkBox_circumcircleVisibility;
        private System.Windows.Forms.Timer timer_animationTimer;
        private CheckBox checkBox_setDomainOfDefinition;
        private NumericUpDown numericUpDown_xAxisStart;
        private NumericUpDown numericUpDown_xAxisEnd;
        private NumericUpDown numericUpDown_yAxisEnd;
        private NumericUpDown numericUpDown_yAxisStart;
        private Button button_interpolation;
        private Label label_yAxis;
        private Label label_xAxis;
        private Label label_yAxisNum;
        private Label label_xAxisNum;
        private NumericUpDown numericUpDown_yAxisNum;
        private NumericUpDown numericUpDown_xAxisNum;
        private GroupBox groupBox_domainOfDefinition;
        private GroupBox groupBox_axisNum;
        private CheckBox checkBox_combinedGraphs;
    }
}