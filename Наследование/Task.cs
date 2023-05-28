using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    public interface IOwner
    {
        int Owner { get; set; }
    }
    public interface IEnemy
    {
        Army Army { get; set; }
    }

    public interface ITreasure
    {
        Treasure Treasure { get; set; }
    }
    public class Dwelling : IOwner
    {
        public int Owner { get; set; }
    }
    public class Mine: IOwner, IEnemy, ITreasure
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps: IEnemy, ITreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves: IEnemy
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasure
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IEnemy enemy)
            {
                if (!player.CanBeat(enemy.Army))
                {
                    player.Die();
                    return;
                }
                else
                {
                    if (mapObject is IOwner enemyOwn)
                        enemyOwn.Owner = player.Id;
                    if (mapObject is ITreasure enemyTreasure)
                        player.Consume(enemyTreasure.Treasure);
                    return;
                }
            }
            if (mapObject is IOwner owner)
                owner.Owner = player.Id;
            if (mapObject is ITreasure treasure)
                player.Consume(treasure.Treasure);
        }
    }
}
