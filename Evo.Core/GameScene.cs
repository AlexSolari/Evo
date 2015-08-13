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
        public GameScene()
        {
            Add(new Predator(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            Add(new Herbivore(new Point(Rand.Int(500), Rand.Int(500))));
            
        }
    }
}
