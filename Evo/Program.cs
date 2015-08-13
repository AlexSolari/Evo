using Evo.Core;
using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Evo
{
    class Program
    {
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static Game game = new Game("Castle War", Global.Width, Global.Height);
        static void Main(string[] args)
        {

            game.FirstScene = new GameScene();

            var handle = GetConsoleWindow();

            // Hide
            //ShowWindow(handle, SW_HIDE);

            game.Start();
        }
    }
}
