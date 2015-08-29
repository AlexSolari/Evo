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
            config.Width = (tmp == 0) ? (int)(screen.Width) : tmp;

            Console.Write("Enter field height: ");
            tmp = ReadInt();
            config.Height = (tmp == 0) ? (int)(screen.Height) : tmp;

            Console.Write("Enter predators start count: ");
            tmp = ReadInt();
            config.Predators = (tmp == 0) ? 30 : tmp;

            Console.Write("Enter herbivores start count: ");
            tmp = ReadInt();
            config.Herbivores = (tmp == 0) ? 50 : tmp;

            Console.Write("Enter predator targeting radius: ");
            tmp = ReadInt();
            config.TargetingRadius = (tmp == 0) ? 400 : tmp;

            Console.Write("Enter herbivores ranaway radius: ");
            tmp = ReadInt();
            config.RanawayRadius = (tmp == 0) ? 20 : tmp;

            Console.Write("Run in fullscreen? (Y/N): ");

            Global.SystemConfig = config;

            Game game = new Game("CRBEE", Global.Width, Global.Height, 60, Char.ToUpper(Console.ReadKey().KeyChar) == 'Y');
            game.FirstScene = new GameScene();
            Console.Clear();
            game.Start();
        }
    }
}
