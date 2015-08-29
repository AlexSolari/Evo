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
            if (EndInitiated)
            {
                EndTimer--;
                if (EndTimer < 0)
                {
                    Console.WriteLine("Cycle ended");
                    if (Global.Objects.Where(x => x is Herbivore).Count() != 0)
                    {
                        CountOfResults["Green"]++;
                        Console.WriteLine("All predators died from hunger");
                    }
                    else if (Global.Objects.Where(x => x is Predator).Count() != 0)
                    {
                        CountOfResults["Red"]++;
                        Console.WriteLine("All herbivores eaten.");
                    }
                    Console.WriteLine("Tick's taken: {0}", Ticks);
                    Console.WriteLine("Green's: {0} @ Red's: {1}", CountOfResults["Green"], CountOfResults["Red"]);

                    Game.RemoveScene();
                    foreach (var item in Scene.GetEntities<Cell>())
                    {
                        item.Destroy();
                    }
                    Global.Objects.Clear();
                    Game.AddScene(new GameScene(this));
                    Scene.End();
                    
                }
            }
            else if (Global.Objects.Where(x => x is Predator).Count() == 0 || Global.Objects.Where(x => x is Herbivore).Count() == 0)
            {
                EndTimer = 100;
                EndInitiated = true;
                Console.WriteLine("================");
            }
            else Ticks++;

            if (Ticks % 500 == 0)
                Global.Objects.ToList().ForEach(cell => 
                {
                    if (cell.Direction.Length == 0)
                        cell.Die();
                });
        }
    }
}
