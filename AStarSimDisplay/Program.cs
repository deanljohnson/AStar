using System;
using System.Windows.Forms;
using SFML.Graphics;

namespace AStarSimDisplay
{
    static class Program
    {
        private static MainForm m_MainForm { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            m_MainForm = new MainForm();
            m_MainForm.Show();
            //Application.Run(m_MainForm);

            var sfmlPanel = m_MainForm.Controls.Find("SFMLDrawingSurface", false)[0];
            var window = new RenderWindow(sfmlPanel.Handle);
            m_MainForm.InitializeSim(window);
            m_MainForm.InitializeDataDisplay();

            while (sfmlPanel.Visible)
            {
                Application.DoEvents();
                window.DispatchEvents();
                m_MainForm.Simulation.Update();
                m_MainForm.DataDisplay.UpdateDisplay();
                window.Clear(Color.Black);
                m_MainForm.Simulation.Render();
                window.Display();
            }
        }
    }
}
