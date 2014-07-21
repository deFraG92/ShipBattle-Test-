using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameUtils;

namespace GameUtils
{
    public class GameContext
    {
        public List<Coordinates> ShipCoords { get; set; }
        public List<Coordinates> DeleteShipCoords { get; set; }
        public Dictionary<bool, List<Coordinates>> EnemyHitCoords { get; set; }
        public Dictionary<bool, List<Coordinates>> MyHitCoords { get; set; }
        public List<Coordinates> MyDestroyShipCoords { get; set; }
        public List<Coordinates> EnemyDestroyShipCoords { get; set; }

        public GameContext()
        {
            ShipCoords = new List<Coordinates>();
            MyHitCoords = new Dictionary<bool, List<Coordinates>>();
            EnemyHitCoords = new Dictionary<bool, List<Coordinates>>();
            DeleteShipCoords = new List<Coordinates>();
            MyDestroyShipCoords = new List<Coordinates>();
            EnemyDestroyShipCoords = new List<Coordinates>();
        }
        
        public void UpdateOnShipCoords(Coordinates coord, ShipAction action, bool myCoords = true)
        {
            if (action == ShipAction.CreateShip)
            {
                if (!ShipCoords.Contains(coord))
                    ShipCoords.Add(coord);
            }
            else if (action == ShipAction.DeleteShip)
            {
                DeleteShipCoords.Add(coord);
                ShipCoords.Remove(coord);
            }
            else if (action == ShipAction.DestroyShip)
            {
                if (myCoords)
                {
                    if (!EnemyDestroyShipCoords.Contains(coord))
                        EnemyDestroyShipCoords.Add(coord);
                }
                else
                {
                    if (!MyDestroyShipCoords.Contains(coord))
                        MyDestroyShipCoords.Add(coord);
                }
            }
        }

        private void CheckDictionary(Dictionary<bool, List<Coordinates>> list, bool key, Coordinates coords)
        {
            if (!list.ContainsKey(key))
            {
                list.Add(key, new List<Coordinates>());
            }
            foreach (var coord in list)
            {
                if (!coord.Value.Contains(coords))
                    list[key].Add(coords);
            }
        }

        public void UpdateOnHitShipCoords(Coordinates coord, bool hit, bool myCoords = true)
        {
            if (myCoords)
            {
                CheckDictionary(EnemyHitCoords, hit, coord);
            }
            else
            {
                CheckDictionary(MyHitCoords, hit, coord);
            }
        }
    }
}
