using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameUtils;

namespace ClientUtils
{
    public interface IDrawing 
    {
        void DrawBattleField();
        void DrawShip(List<Coordinates> CreateShipCoords);
        void EraseShip(List<Coordinates> DeleeShipCoords);
        void DrawHitShip(Coordinates HitCoord);
        void DrawDestroySHip(List<Coordinates> DestroyShipCoords);
    }
}
