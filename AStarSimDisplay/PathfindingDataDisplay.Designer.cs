namespace AStarSimDisplay
{
    partial class PathfindingDataDisplay
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.graphSizeLabel = new System.Windows.Forms.Label();
            this.pathLengthLabel = new System.Windows.Forms.Label();
            this.nodesVistedLabel = new System.Windows.Forms.Label();
            this.timeTakenLabel = new System.Windows.Forms.Label();
            this.totalPathsComputedLabel = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.timeTakenTextBox = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.thisRunGroupBox = new System.Windows.Forms.GroupBox();
            this.nodesVisitedTextBox = new System.Windows.Forms.TextBox();
            this.pathLengthTextBox = new System.Windows.Forms.TextBox();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.graphSizeTextBox = new System.Windows.Forms.TextBox();
            this.totalGroupBox = new System.Windows.Forms.GroupBox();
            this.totalNodesVisitedTextBox = new System.Windows.Forms.TextBox();
            this.totalPathfindingTimeTextBox = new System.Windows.Forms.TextBox();
            this.toalPathFindingTimeLabel = new System.Windows.Forms.Label();
            this.totalPathsComputedTextBox = new System.Windows.Forms.TextBox();
            this.totalNodesVisitedLabel = new System.Windows.Forms.Label();
            this.averageGroupBox = new System.Windows.Forms.GroupBox();
            this.avgNodesVisitedTextBox = new System.Windows.Forms.TextBox();
            this.avgPathfindingTimeTextBox = new System.Windows.Forms.TextBox();
            this.avgPathfindingTimeLabel = new System.Windows.Forms.Label();
            this.avgNodesVisitedLabel = new System.Windows.Forms.Label();
            this.thisRunGroupBox.SuspendLayout();
            this.totalGroupBox.SuspendLayout();
            this.averageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphSizeLabel
            // 
            this.graphSizeLabel.AutoSize = true;
            this.graphSizeLabel.Location = new System.Drawing.Point(6, 94);
            this.graphSizeLabel.Name = "graphSizeLabel";
            this.graphSizeLabel.Size = new System.Drawing.Size(59, 13);
            this.graphSizeLabel.TabIndex = 0;
            this.graphSizeLabel.Text = "Graph Size";
            // 
            // pathLengthLabel
            // 
            this.pathLengthLabel.AutoSize = true;
            this.pathLengthLabel.Location = new System.Drawing.Point(6, 42);
            this.pathLengthLabel.Name = "pathLengthLabel";
            this.pathLengthLabel.Size = new System.Drawing.Size(65, 13);
            this.pathLengthLabel.TabIndex = 1;
            this.pathLengthLabel.Text = "Path Length";
            // 
            // nodesVistedLabel
            // 
            this.nodesVistedLabel.AutoSize = true;
            this.nodesVistedLabel.Location = new System.Drawing.Point(6, 68);
            this.nodesVistedLabel.Name = "nodesVistedLabel";
            this.nodesVistedLabel.Size = new System.Drawing.Size(72, 13);
            this.nodesVistedLabel.TabIndex = 2;
            this.nodesVistedLabel.Text = "Nodes Visited";
            // 
            // timeTakenLabel
            // 
            this.timeTakenLabel.AutoSize = true;
            this.timeTakenLabel.Location = new System.Drawing.Point(6, 16);
            this.timeTakenLabel.Name = "timeTakenLabel";
            this.timeTakenLabel.Size = new System.Drawing.Size(64, 13);
            this.timeTakenLabel.TabIndex = 3;
            this.timeTakenLabel.Text = "Time Taken";
            // 
            // totalPathsComputedLabel
            // 
            this.totalPathsComputedLabel.AutoSize = true;
            this.totalPathsComputedLabel.Location = new System.Drawing.Point(6, 16);
            this.totalPathsComputedLabel.Name = "totalPathsComputedLabel";
            this.totalPathsComputedLabel.Size = new System.Drawing.Size(85, 13);
            this.totalPathsComputedLabel.TabIndex = 5;
            this.totalPathsComputedLabel.Text = "Paths Computed";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(50, 291);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 10;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // timeTakenTextBox
            // 
            this.timeTakenTextBox.Location = new System.Drawing.Point(87, 13);
            this.timeTakenTextBox.Name = "timeTakenTextBox";
            this.timeTakenTextBox.ReadOnly = true;
            this.timeTakenTextBox.Size = new System.Drawing.Size(77, 20);
            this.timeTakenTextBox.TabIndex = 12;
            // 
            // thisRunGroupBox
            // 
            this.thisRunGroupBox.Controls.Add(this.graphSizeTextBox);
            this.thisRunGroupBox.Controls.Add(this.nodesVisitedTextBox);
            this.thisRunGroupBox.Controls.Add(this.pathLengthTextBox);
            this.thisRunGroupBox.Controls.Add(this.timeTakenLabel);
            this.thisRunGroupBox.Controls.Add(this.pathLengthLabel);
            this.thisRunGroupBox.Controls.Add(this.timeTakenTextBox);
            this.thisRunGroupBox.Controls.Add(this.nodesVistedLabel);
            this.thisRunGroupBox.Controls.Add(this.graphSizeLabel);
            this.thisRunGroupBox.Location = new System.Drawing.Point(3, 3);
            this.thisRunGroupBox.Name = "thisRunGroupBox";
            this.thisRunGroupBox.Size = new System.Drawing.Size(170, 115);
            this.thisRunGroupBox.TabIndex = 13;
            this.thisRunGroupBox.TabStop = false;
            this.thisRunGroupBox.Text = "This Run";
            // 
            // nodesVisitedTextBox
            // 
            this.nodesVisitedTextBox.Location = new System.Drawing.Point(87, 65);
            this.nodesVisitedTextBox.Name = "nodesVisitedTextBox";
            this.nodesVisitedTextBox.ReadOnly = true;
            this.nodesVisitedTextBox.Size = new System.Drawing.Size(77, 20);
            this.nodesVisitedTextBox.TabIndex = 14;
            // 
            // pathLengthTextBox
            // 
            this.pathLengthTextBox.Location = new System.Drawing.Point(87, 39);
            this.pathLengthTextBox.Name = "pathLengthTextBox";
            this.pathLengthTextBox.ReadOnly = true;
            this.pathLengthTextBox.Size = new System.Drawing.Size(77, 20);
            this.pathLengthTextBox.TabIndex = 13;
            // 
            // graphSizeTextBox
            // 
            this.graphSizeTextBox.Location = new System.Drawing.Point(87, 91);
            this.graphSizeTextBox.Name = "graphSizeTextBox";
            this.graphSizeTextBox.ReadOnly = true;
            this.graphSizeTextBox.Size = new System.Drawing.Size(77, 20);
            this.graphSizeTextBox.TabIndex = 15;
            // 
            // totalGroupBox
            // 
            this.totalGroupBox.Controls.Add(this.totalNodesVisitedTextBox);
            this.totalGroupBox.Controls.Add(this.totalPathfindingTimeTextBox);
            this.totalGroupBox.Controls.Add(this.toalPathFindingTimeLabel);
            this.totalGroupBox.Controls.Add(this.totalPathsComputedTextBox);
            this.totalGroupBox.Controls.Add(this.totalNodesVisitedLabel);
            this.totalGroupBox.Controls.Add(this.totalPathsComputedLabel);
            this.totalGroupBox.Location = new System.Drawing.Point(3, 124);
            this.totalGroupBox.Name = "totalGroupBox";
            this.totalGroupBox.Size = new System.Drawing.Size(170, 90);
            this.totalGroupBox.TabIndex = 14;
            this.totalGroupBox.TabStop = false;
            this.totalGroupBox.Text = "Totals";
            // 
            // totalNodesVisitedTextBox
            // 
            this.totalNodesVisitedTextBox.Location = new System.Drawing.Point(87, 65);
            this.totalNodesVisitedTextBox.Name = "totalNodesVisitedTextBox";
            this.totalNodesVisitedTextBox.ReadOnly = true;
            this.totalNodesVisitedTextBox.Size = new System.Drawing.Size(77, 20);
            this.totalNodesVisitedTextBox.TabIndex = 14;
            // 
            // totalPathfindingTimeTextBox
            // 
            this.totalPathfindingTimeTextBox.Location = new System.Drawing.Point(87, 39);
            this.totalPathfindingTimeTextBox.Name = "totalPathfindingTimeTextBox";
            this.totalPathfindingTimeTextBox.ReadOnly = true;
            this.totalPathfindingTimeTextBox.Size = new System.Drawing.Size(77, 20);
            this.totalPathfindingTimeTextBox.TabIndex = 13;
            // 
            // toalPathFindingTimeLabel
            // 
            this.toalPathFindingTimeLabel.AutoSize = true;
            this.toalPathFindingTimeLabel.Location = new System.Drawing.Point(6, 42);
            this.toalPathFindingTimeLabel.Name = "toalPathFindingTimeLabel";
            this.toalPathFindingTimeLabel.Size = new System.Drawing.Size(86, 13);
            this.toalPathFindingTimeLabel.TabIndex = 1;
            this.toalPathFindingTimeLabel.Text = "Pathfinding Time";
            // 
            // totalPathsComputedTextBox
            // 
            this.totalPathsComputedTextBox.Location = new System.Drawing.Point(87, 13);
            this.totalPathsComputedTextBox.Name = "totalPathsComputedTextBox";
            this.totalPathsComputedTextBox.ReadOnly = true;
            this.totalPathsComputedTextBox.Size = new System.Drawing.Size(77, 20);
            this.totalPathsComputedTextBox.TabIndex = 12;
            // 
            // totalNodesVisitedLabel
            // 
            this.totalNodesVisitedLabel.AutoSize = true;
            this.totalNodesVisitedLabel.Location = new System.Drawing.Point(6, 68);
            this.totalNodesVisitedLabel.Name = "totalNodesVisitedLabel";
            this.totalNodesVisitedLabel.Size = new System.Drawing.Size(72, 13);
            this.totalNodesVisitedLabel.TabIndex = 2;
            this.totalNodesVisitedLabel.Text = "Nodes Visited";
            // 
            // averageGroupBox
            // 
            this.averageGroupBox.Controls.Add(this.avgNodesVisitedTextBox);
            this.averageGroupBox.Controls.Add(this.avgPathfindingTimeTextBox);
            this.averageGroupBox.Controls.Add(this.avgPathfindingTimeLabel);
            this.averageGroupBox.Controls.Add(this.avgNodesVisitedLabel);
            this.averageGroupBox.Location = new System.Drawing.Point(3, 220);
            this.averageGroupBox.Name = "averageGroupBox";
            this.averageGroupBox.Size = new System.Drawing.Size(170, 65);
            this.averageGroupBox.TabIndex = 15;
            this.averageGroupBox.TabStop = false;
            this.averageGroupBox.Text = "Averages";
            // 
            // avgNodesVisitedTextBox
            // 
            this.avgNodesVisitedTextBox.Location = new System.Drawing.Point(87, 39);
            this.avgNodesVisitedTextBox.Name = "avgNodesVisitedTextBox";
            this.avgNodesVisitedTextBox.ReadOnly = true;
            this.avgNodesVisitedTextBox.Size = new System.Drawing.Size(77, 20);
            this.avgNodesVisitedTextBox.TabIndex = 14;
            // 
            // avgPathfindingTimeTextBox
            // 
            this.avgPathfindingTimeTextBox.Location = new System.Drawing.Point(87, 13);
            this.avgPathfindingTimeTextBox.Name = "avgPathfindingTimeTextBox";
            this.avgPathfindingTimeTextBox.ReadOnly = true;
            this.avgPathfindingTimeTextBox.Size = new System.Drawing.Size(77, 20);
            this.avgPathfindingTimeTextBox.TabIndex = 13;
            // 
            // avgPathfindingTimeLabel
            // 
            this.avgPathfindingTimeLabel.AutoSize = true;
            this.avgPathfindingTimeLabel.Location = new System.Drawing.Point(6, 16);
            this.avgPathfindingTimeLabel.Name = "avgPathfindingTimeLabel";
            this.avgPathfindingTimeLabel.Size = new System.Drawing.Size(86, 13);
            this.avgPathfindingTimeLabel.TabIndex = 1;
            this.avgPathfindingTimeLabel.Text = "Pathfinding Time";
            // 
            // avgNodesVisitedLabel
            // 
            this.avgNodesVisitedLabel.AutoSize = true;
            this.avgNodesVisitedLabel.Location = new System.Drawing.Point(6, 42);
            this.avgNodesVisitedLabel.Name = "avgNodesVisitedLabel";
            this.avgNodesVisitedLabel.Size = new System.Drawing.Size(72, 13);
            this.avgNodesVisitedLabel.TabIndex = 2;
            this.avgNodesVisitedLabel.Text = "Nodes Visited";
            // 
            // PathfindingDataDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.averageGroupBox);
            this.Controls.Add(this.totalGroupBox);
            this.Controls.Add(this.thisRunGroupBox);
            this.Controls.Add(this.resetButton);
            this.Name = "PathfindingDataDisplay";
            this.Size = new System.Drawing.Size(177, 318);
            this.thisRunGroupBox.ResumeLayout(false);
            this.thisRunGroupBox.PerformLayout();
            this.totalGroupBox.ResumeLayout(false);
            this.totalGroupBox.PerformLayout();
            this.averageGroupBox.ResumeLayout(false);
            this.averageGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label graphSizeLabel;
        private System.Windows.Forms.Label pathLengthLabel;
        private System.Windows.Forms.Label nodesVistedLabel;
        private System.Windows.Forms.Label timeTakenLabel;
        private System.Windows.Forms.Label totalPathsComputedLabel;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.TextBox timeTakenTextBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox thisRunGroupBox;
        private System.Windows.Forms.TextBox nodesVisitedTextBox;
        private System.Windows.Forms.TextBox pathLengthTextBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.TextBox graphSizeTextBox;
        private System.Windows.Forms.GroupBox totalGroupBox;
        private System.Windows.Forms.TextBox totalNodesVisitedTextBox;
        private System.Windows.Forms.TextBox totalPathfindingTimeTextBox;
        private System.Windows.Forms.Label toalPathFindingTimeLabel;
        private System.Windows.Forms.TextBox totalPathsComputedTextBox;
        private System.Windows.Forms.Label totalNodesVisitedLabel;
        private System.Windows.Forms.GroupBox averageGroupBox;
        private System.Windows.Forms.TextBox avgNodesVisitedTextBox;
        private System.Windows.Forms.TextBox avgPathfindingTimeTextBox;
        private System.Windows.Forms.Label avgPathfindingTimeLabel;
        private System.Windows.Forms.Label avgNodesVisitedLabel;
    }
}
