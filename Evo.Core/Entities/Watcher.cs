using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace Evo.Core.Entities
{
    public class Watcher : Entity
    {
        Dictionary<string, int> CountOfResults = new Dictionary<string, int>();
        int Ticks { get; set; }
        int EndTimer { get; set; }
        bool EndInitiated { get; set; }
        public Watcher(Watcher watcher)
        {
            if (watcher != null)
            {
                CountOfResults.Add("Red", watcher.CountOfResults["Red"]);
                CountOfResults.Add("Green", watcher.CountOfResults["Green"]);
            }
            else
            {
                CountOfResults.Add("Red", 0);
                CountOfResults.Add("Green", 0);
            }
            EndInitiated = false;
            EndTimer = 0;
            
        }

        public override void Update()
        {
            Ticks++;
            if (EndInitiated)
            {
                EndTimer--;
                if (EndTimer < 0)
                {
                    Console.WriteLine("Cycle ended");
                    if (Global.Objects.Where(x => x is Herbivore).Count() != 0)
                    {
                        CountOfResults["Green"] = CountOfResults["Green"] + 1;
                        Console.WriteLine("Herbivores is too fast. All predators died from hunger");
                    }
                    else
                    {
                        CountOfResults["Red"] = CountOfResults["Red"] + 1;
                        Console.WriteLine("Herbivores is too slow. All herbivores died and all predators died from hunger");
                    }
                    Console.WriteLine("Tick's taken: {0}", Ticks);
                    Console.WriteLine("Green's: {0} @ Red's: {1}]", CountOfResults["Green"], CountOfResults["Red"]);

                    Game.RemoveScene();
                    Global.Objects.ForEach(x => x.Destroy());
                    Global.Objects.Clear();
                    Game.AddScene(new GameScene(this));
                    Scene.End();
                }
            }
            else if (Global.Objects.Where(x=>x is Predator).Count() == 0)
            {
                EndTimer = 500;
                EndInitiated = true;
            }
        }
    }
}
