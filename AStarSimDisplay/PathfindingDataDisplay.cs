using System;
using System.Windows.Forms;
using AStarSimulation;

namespace AStarSimDisplay
{
    public partial class PathfindingDataDisplay : UserControl
    {
        private PathfindingData m_Data { get; set; }

        public PathfindingDataDisplay()
        {
            InitializeComponent();
        }

        public void SetData(PathfindingData data)
        {
            m_Data = data;
        }

        public void UpdateDisplay()
        {
            //This Run
            timeTakenTextBox.Text = m_Data.PathfindingTime.TotalMilliseconds.ToString("N2") + @"ms";
            pathLengthTextBox.Text = m_Data.PathLength.ToString();
            nodesVisitedTextBox.Text = m_Data.NodesVisited.ToString();
            graphSizeTextBox.Text = m_Data.GraphSize.ToString();

            //Totals
            totalPathsComputedTextBox.Text = m_Data.TotalPathsComputed.ToString();
            totalPathfindingTimeTextBox.Text = m_Data.TotalPathfindingTime.TotalSeconds.ToString("N2") + @"s";
            totalNodesVisitedTextBox.Text = m_Data.TotalNodesVisited.ToString("G4");

            //Averages
            avgPathfindingTimeTextBox.Text = m_Data.AverageTime.ToString("N2") + @"ms";
            avgNodesVisitedTextBox.Text = m_Data.AverageNodesVisited.ToString("N2");
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            m_Data.Reset();
        }
    }
}
