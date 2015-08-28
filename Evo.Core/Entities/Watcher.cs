using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace Evo.Core.Entities
{
    class Watcher : Entity
    {
        public int Ticks { get; set; }
        public int EndTimer { get; set; }
        public bool EndInitiated { get; set; }
        public Watcher()
        {
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
                        Console.WriteLine("Herbivores is too fast to die. All predators died from hunger");
                    else
                        Console.WriteLine("Herbivores is too slow. All herbivores died and all predators died from hunger");
                    Console.WriteLine("Tick's taken: {0}", Ticks);

                    Game.RemoveScene();
                    Global.Objects.ForEach(x => x.Destroy());
                    Global.Objects.Clear();
                    Game.AddScene(new GameScene());
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
