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

        public override void Update()
        {
            foreach (var action in Global.PendingActions)
            {
                action();
            }
            Global.PendingActions.Clear();
            var preds = Global.Predators.Cast<Cell>().ToList();
            var herbs = Global.Herbivores.Cast<Cell>().ToList();
            var pCount = preds.Count();
            var hCount = herbs.Count();

            for (int i = 0; i < pCount; i++)
            {
                var pred = preds[i];

                if (!Global.SquaredDistances.ContainsKey(pred))
                    Global.SquaredDistances[pred] = new Dictionary<Interfaces.ILifeForm, double>();

                for (int j = 0; j < hCount; j++)
                {
                    var herb = herbs[j];
                    Global.SquaredDistances[pred][herb] = Global.DistanceSquared(herb, pred);
                }
            }
            
            base.Update();
        }
    }
}
