using System;
using AStarSimulation;
using SFML.Graphics;
using SFML.Window;

namespace AStar
{
    internal static class Program
    {
        private static RenderWindow Window;

        public static void Main(string[] args)
        {
            InitializeWindow();

            var control = new Simulation(Window);

            while (Window.IsOpen())
            {
                Window.DispatchEvents();

                control.Update();

                Window.Clear(Color.White);

                control.Render();

                Window.Display();
            }
        }

        private static void InitializeWindow()
        {
            Window = new RenderWindow(new VideoMode(1600, 900, 32), "A* Pathfinding", Styles.Default);
            //window.SetVerticalSyncEnabled(true);
            Window.SetActive(false);
            Window.SetVisible(true);

            Window.Closed += WindowClosedEvent;
        }

        private static void WindowClosedEvent(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}