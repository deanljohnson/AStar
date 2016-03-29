using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using AStarSimulation;
using SFML.Graphics;
using SFML.System;

namespace AStarSimDisplay
{
    public partial class MainForm : Form
    {
        private readonly Dictionary<String, GridType> m_GridTypeMap = new Dictionary<String, GridType>
        {
            {"Square", GridType.Square},
            {"Hex", GridType.Hex}
        }; 

        public Simulation Simulation { get; set; }
        public PathfindingDataDisplay DataDisplay { get; set; }

        public MainForm()
        {
            InitializeComponent();
            xNodeSizeTextBox.Validating += XNodeSizeTextBoxOnValidating;
            yNodeSizeTextBox.Validating += YNodeSizeTextBoxOnValidating;
            heuristicTextBox.Validating += HeuristicTextBoxOnValidating;
        }

        public void InitializeSim(RenderWindow window)
        {
            var x = int.Parse(xNodeSizeTextBox.Text);
            var y = int.Parse(yNodeSizeTextBox.Text);
            Simulation = new Simulation(window, GetSelectedGridType(), new Vector2i(x, y))
            {
                Heuristic = float.Parse(heuristicTextBox.Text)
            };
        }

        public void InitializeDataDisplay()
        {
            DataDisplay = Controls.Find("dataDisplay", false)[0] as PathfindingDataDisplay;
            DataDisplay?.SetData(Simulation.Data);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var gridTypePair in m_GridTypeMap)
            {
                gridTypeComboBox.Items.Add(gridTypePair.Key);
            }

            gridTypeComboBox.SelectedIndex = 0;

            algorithmComboBox.SelectedIndex = 0;
        }

        private void gridTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSimGridTypeToSelected();
        }

        private void toggleContinuousButton_Click(object sender, EventArgs e)
        {
            switch (Simulation.SimulationAction)
            {
                case SimulationAction.RunContinuously:
                    Simulation.SimulationAction = SimulationAction.None;
                    break;
                case SimulationAction.None:
                    Simulation.SimulationAction = SimulationAction.RunContinuously;
                    break;
            }
        }

        private void runOnceButton_Click(object sender, EventArgs e)
        {
            Simulation.SimulationAction = SimulationAction.RunOnce;
        }

        private void runOneStepButton_Click(object sender, EventArgs e)
        {
            Simulation.SimulationAction = SimulationAction.RunOneStep;
        }

        private void YNodeSizeTextBoxOnValidating(object sender, CancelEventArgs cancelEventArgs)
        {
            int y;
            if (!int.TryParse(yNodeSizeTextBox.Text, out y) || y == 0)
            {
                cancelEventArgs.Cancel = true;
                return;
            }

            //We are going to assume parsing succeeds because of it's validating handler
            var x = int.Parse(xNodeSizeTextBox.Text);
            Simulation.NodeSize = new Vector2i(x, y);
        }

        private void XNodeSizeTextBoxOnValidating(object sender, CancelEventArgs cancelEventArgs)
        {
            int x;
            if (!int.TryParse(xNodeSizeTextBox.Text, out x) || x == 0)
            {
                cancelEventArgs.Cancel = true;
                return;
            }

            //We are going to assume parsing succeeds because of it's validating handler
            var y = int.Parse(yNodeSizeTextBox.Text);
            Simulation.NodeSize = new Vector2i(x, y);
        }

        private void HeuristicTextBoxOnValidating(object sender, CancelEventArgs cancelEventArgs)
        {
            float h;
            if (!float.TryParse(heuristicTextBox.Text, out h))
            {
                cancelEventArgs.Cancel = true;
                return;
            }

            Simulation.Heuristic = h;
        }

        private void SetSimGridTypeToSelected()
        {
            if (Simulation == null) return;
            Simulation.GridType = GetSelectedGridType();
        }

        private GridType GetSelectedGridType()
        {
            var selectedString = gridTypeComboBox.Items[gridTypeComboBox.SelectedIndex].ToString();
            return m_GridTypeMap[selectedString];
        }

        private void saveEndPointsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Simulation.SaveEndPoints = saveEndPointsCheckBox.Checked;
        }

        private void generateWallsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Simulation.GenerateWalls = generateWallsCheckbox.Checked;
        }
    }
}
