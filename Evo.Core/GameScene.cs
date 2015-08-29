using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using Evo.Core.Entities;

namespace Evo.Core
{
    public class GameScene : Scene
    {
        public GameScene(Watcher watcher = null)
        {
            Add(new Watcher(watcher));
            for (int i = 0; i < Global.SystemConfig.Predators; i++)
            {
                Add(new Predator(new Point(Rand.Int(Global.Width), Rand.Int(Global.Height))));
            }
            for (int i = 0; i < Global.SystemConfig.Herbivores; i++)
            {
                Add(new Herbivore(new Point(Rand.Int(Global.Width), Rand.Int(Global.Height))));
            }            
        }
    }
}
