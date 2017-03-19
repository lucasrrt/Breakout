using System;

namespace Breakout
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static Breakout game;

        public static int WIDTH = 800, HEIGHT = 480;

        [STAThread]
        static void Main()
        {
            game = new Breakout();
            game.Run();
        }
    }
}
