using System;
using SFML.Graphics;
using SFML.Window;

namespace AStarSimulation
{
    internal static class Program
    {
        private static RenderWindow m_Window;

        public static void Main(string[] args)
        {
            InitializeWindow();

            var control = new Simulation(m_Window);

            while (m_Window.IsOpen)
            {
                m_Window.DispatchEvents();

                control.Update();

                m_Window.Clear(Color.Black);

                control.Render();

                m_Window.Display();
            }
        }

        private static void InitializeWindow()
        {
            m_Window = new RenderWindow(new VideoMode(1600, 900, 32), "A* Pathfinding", Styles.Default);
            //window.SetVerticalSyncEnabled(true);
            m_Window.SetActive(false);
            m_Window.SetVisible(true);

            m_Window.Closed += WindowClosedEvent;
        }

        private static void WindowClosedEvent(object sender, EventArgs e)
        {
            m_Window.Close();
        }
    }
}