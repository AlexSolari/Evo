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

        static void Main(string[] args)
        {
            var config = new Config();
            var tmp = 0;
            Console.WriteLine("---===CELL RANDOM-BASED-EVOLUTION EMULATOR===---");
            Console.WriteLine("We need some data to start emulation. Leave field empty to use default.");
            Console.Write("Enter field width: ");
            tmp = Convert.ToInt32('0' + Console.ReadLine());
            config.Width = (tmp == 0) ? 800 : tmp;

            Console.Write("Enter field height: ");
            tmp = Convert.ToInt32('0' + Console.ReadLine());
            config.Height = (tmp == 0) ? 800 : tmp;

            Console.Write("Enter predators start count: ");
            tmp = Convert.ToInt32('0' + Console.ReadLine());
            config.Predators = (tmp == 0) ? 3 : tmp;

            Console.Write("Enter herbivores start count: ");
            tmp = Convert.ToInt32('0' + Console.ReadLine());
            config.Herbivores = (tmp == 0) ? 30 : tmp;

            Console.Write("Enter predator targeting radius: ");
            tmp = Convert.ToInt32('0' + Console.ReadLine());
            config.TargetingRadius = (tmp == 0) ? 228 : tmp;

            Console.Write("Enter herbivores ranaway radius: ");
            tmp = Convert.ToInt32('0' + Console.ReadLine());
            config.TargetingRadius = (tmp == 0) ? 120 : tmp;

            Global.SystemConfig = config;

            Game game = new Game("Castle War", Global.Width, Global.Height);
            game.FirstScene = new GameScene();
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            game.Start();
        }
    }
}
