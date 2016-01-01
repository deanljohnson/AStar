using System;
using System.ComponentModel;

namespace AStarSimDisplay
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridTypeComboBox = new System.Windows.Forms.ComboBox();
            this.gridTypeLabel = new System.Windows.Forms.Label();
            this.toggleContinuousButton = new System.Windows.Forms.Button();
            this.runOnceButton = new System.Windows.Forms.Button();
            this.runOneStepButton = new System.Windows.Forms.Button();
            this.nodeSizeLabel = new System.Windows.Forms.Label();
            this.xNodeSizeTextBox = new System.Windows.Forms.TextBox();
            this.yNodeSizeTextBox = new System.Windows.Forms.TextBox();
            this.xNodeSizeLabel = new System.Windows.Forms.Label();
            this.yNodeSizeLabel = new System.Windows.Forms.Label();
            this.SFMLDrawingSurface = new AStarSimDisplay.SFMLDrawingSurface();
            this.SuspendLayout();
            // 
            // gridTypeComboBox
            // 
            this.gridTypeComboBox.FormattingEnabled = true;
            this.gridTypeComboBox.Location = new System.Drawing.Point(12, 163);
            this.gridTypeComboBox.Name = "gridTypeComboBox";
            this.gridTypeComboBox.Size = new System.Drawing.Size(111, 21);
            this.gridTypeComboBox.TabIndex = 1;
            this.gridTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.gridTypeComboBox_SelectedIndexChanged);
            // 
            // gridTypeLabel
            // 
            this.gridTypeLabel.AutoSize = true;
            this.gridTypeLabel.Location = new System.Drawing.Point(12, 147);
            this.gridTypeLabel.Name = "gridTypeLabel";
            this.gridTypeLabel.Size = new System.Drawing.Size(53, 13);
            this.gridTypeLabel.TabIndex = 2;
            this.gridTypeLabel.Text = "Grid Type";
            // 
            // toggleContinuousButton
            // 
            this.toggleContinuousButton.Location = new System.Drawing.Point(12, 12);
            this.toggleContinuousButton.Name = "toggleContinuousButton";
            this.toggleContinuousButton.Size = new System.Drawing.Size(111, 23);
            this.toggleContinuousButton.TabIndex = 3;
            this.toggleContinuousButton.Text = "Toggle Continuous";
            this.toggleContinuousButton.UseVisualStyleBackColor = true;
            this.toggleContinuousButton.Click += new System.EventHandler(this.toggleContinuousButton_Click);
            // 
            // runOnceButton
            // 
            this.runOnceButton.Location = new System.Drawing.Point(12, 41);
            this.runOnceButton.Name = "runOnceButton";
            this.runOnceButton.Size = new System.Drawing.Size(111, 23);
            this.runOnceButton.TabIndex = 4;
            this.runOnceButton.Text = "Run Once";
            this.runOnceButton.UseVisualStyleBackColor = true;
            this.runOnceButton.Click += new System.EventHandler(this.runOnceButton_Click);
            // 
            // runOneStepButton
            // 
            this.runOneStepButton.Location = new System.Drawing.Point(12, 70);
            this.runOneStepButton.Name = "runOneStepButton";
            this.runOneStepButton.Size = new System.Drawing.Size(111, 23);
            this.runOneStepButton.TabIndex = 5;
            this.runOneStepButton.Text = "Run One Step";
            this.runOneStepButton.UseVisualStyleBackColor = true;
            this.runOneStepButton.Click += new System.EventHandler(this.runOneStepButton_Click);
            // 
            // nodeSizeLabel
            // 
            this.nodeSizeLabel.AutoSize = true;
            this.nodeSizeLabel.Location = new System.Drawing.Point(12, 187);
            this.nodeSizeLabel.Name = "nodeSizeLabel";
            this.nodeSizeLabel.Size = new System.Drawing.Size(56, 13);
            this.nodeSizeLabel.TabIndex = 6;
            this.nodeSizeLabel.Text = "Node Size";
            // 
            // xNodeSizeTextBox
            // 
            this.xNodeSizeTextBox.Location = new System.Drawing.Point(41, 203);
            this.xNodeSizeTextBox.Name = "xNodeSizeTextBox";
            this.xNodeSizeTextBox.Size = new System.Drawing.Size(82, 20);
            this.xNodeSizeTextBox.TabIndex = 7;
            this.xNodeSizeTextBox.Text = "5";
            this.xNodeSizeTextBox.Validating += XNodeSizeTextBoxOnValidating;
            // 
            // yNodeSizeTextBox
            // 
            this.yNodeSizeTextBox.Location = new System.Drawing.Point(41, 229);
            this.yNodeSizeTextBox.Name = "yNodeSizeTextBox";
            this.yNodeSizeTextBox.Size = new System.Drawing.Size(82, 20);
            this.yNodeSizeTextBox.TabIndex = 8;
            this.yNodeSizeTextBox.Text = "5";
            this.yNodeSizeTextBox.Validating += YNodeSizeTextBoxOnValidating;
            // 
            // xNodeSizeLabel
            // 
            this.xNodeSizeLabel.AutoSize = true;
            this.xNodeSizeLabel.Location = new System.Drawing.Point(21, 206);
            this.xNodeSizeLabel.Name = "xNodeSizeLabel";
            this.xNodeSizeLabel.Size = new System.Drawing.Size(14, 13);
            this.xNodeSizeLabel.TabIndex = 9;
            this.xNodeSizeLabel.Text = "X";
            // 
            // yNodeSizeLabel
            // 
            this.yNodeSizeLabel.AutoSize = true;
            this.yNodeSizeLabel.Location = new System.Drawing.Point(21, 232);
            this.yNodeSizeLabel.Name = "yNodeSizeLabel";
            this.yNodeSizeLabel.Size = new System.Drawing.Size(14, 13);
            this.yNodeSizeLabel.TabIndex = 10;
            this.yNodeSizeLabel.Text = "Y";
            // 
            // SFMLDrawingSurface
            // 
            this.SFMLDrawingSurface.Location = new System.Drawing.Point(129, 12);
            this.SFMLDrawingSurface.Name = "SFMLDrawingSurface";
            this.SFMLDrawingSurface.Size = new System.Drawing.Size(1443, 837);
            this.SFMLDrawingSurface.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.yNodeSizeLabel);
            this.Controls.Add(this.xNodeSizeLabel);
            this.Controls.Add(this.yNodeSizeTextBox);
            this.Controls.Add(this.xNodeSizeTextBox);
            this.Controls.Add(this.nodeSizeLabel);
            this.Controls.Add(this.runOneStepButton);
            this.Controls.Add(this.runOnceButton);
            this.Controls.Add(this.toggleContinuousButton);
            this.Controls.Add(this.gridTypeLabel);
            this.Controls.Add(this.gridTypeComboBox);
            this.Controls.Add(this.SFMLDrawingSurface);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "A* Simulation";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SFMLDrawingSurface SFMLDrawingSurface;
        private System.Windows.Forms.ComboBox gridTypeComboBox;
        private System.Windows.Forms.Label gridTypeLabel;
        private System.Windows.Forms.Button toggleContinuousButton;
        private System.Windows.Forms.Button runOnceButton;
        private System.Windows.Forms.Button runOneStepButton;
        private System.Windows.Forms.Label nodeSizeLabel;
        private System.Windows.Forms.TextBox xNodeSizeTextBox;
        private System.Windows.Forms.TextBox yNodeSizeTextBox;
        private System.Windows.Forms.Label xNodeSizeLabel;
        private System.Windows.Forms.Label yNodeSizeLabel;
    }
}

