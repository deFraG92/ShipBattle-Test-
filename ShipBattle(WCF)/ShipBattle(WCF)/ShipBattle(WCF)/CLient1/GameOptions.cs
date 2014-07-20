using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GameUtils;

namespace ClientsPart
{
    public class GameOptions
    {
        public string Uri { get; set; }

        public Coordinates GameOption { get; set; }
        public Coordinates GridOptions { get; set; }
        public int ShipsCount { get; set; }
        public List<string> ActualShips { get; set; }
        public bool MusicEnable { get; set; }

        public Color FieldColor { get; set; }
        public Color GridColor { get; set; }
        public Color ShipColor { get; set; }
        public Color DeleteshipColor { get; set; }
        public Color HitShipColor { get; set; }
        public Color DestroyShipColor { get; set; }

        public Coordinates RocketOption { get; set; }

        public Coordinates MyBattleFieldLocation { get; set; }
        public Coordinates EnemyBattleFieldLocation { get; set; }
    }
}
