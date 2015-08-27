using Evo.Core;
using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        static int ReadInt()
        {
            var result = 0;
            var tmpstr = '0' + Console.ReadLine();
            Int32.TryParse(tmpstr, out result);
            return result;
        }

        static void Main(string[] args)
        {
            var config = new Config();
            var tmp = 0;
            var screen = Screen.PrimaryScreen.WorkingArea;
            
            Console.WriteLine("---===CELL RANDOM-BASED-EVOLUTION EMULATOR===---");
            Console.WriteLine("We need some data to start emulation. Leave field empty to use default.");
            Console.Write("Enter field width: ");
            tmp = ReadInt();
            config.Width = (tmp == 0) ? (int)(screen.Width * 0.9) : tmp;

            Console.Write("Enter field height: ");
            tmp = ReadInt();
            config.Height = (tmp == 0) ? (int)(screen.Height * 0.9) : tmp;

            Console.Write("Enter predators start count: ");
            tmp = ReadInt();
            config.Predators = (tmp == 0) ? (config.Width * config.Height) / 300000 : tmp;

            Console.Write("Enter herbivores start count: ");
            tmp = ReadInt();
            config.Herbivores = (tmp == 0) ? config.Predators * 10 : tmp;

            Console.Write("Enter predator targeting radius: ");
            tmp = ReadInt();
            config.TargetingRadius = (tmp == 0) ? 230 : tmp;

            Console.Write("Enter herbivores ranaway radius: ");
            tmp = ReadInt();
            config.TargetingRadius = (tmp == 0) ? 120 : tmp;

            Global.SystemConfig = config;

            Game game = new Game("Castle War", Global.Width, Global.Height);
            game.FirstScene = new GameScene();
            var handle = GetConsoleWindow();
            //ShowWindow(handle, SW_HIDE);
            game.Start();
        }
    }
}
