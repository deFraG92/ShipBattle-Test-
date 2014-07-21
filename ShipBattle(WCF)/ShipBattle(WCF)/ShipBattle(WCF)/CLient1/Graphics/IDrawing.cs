using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GameUtils;

namespace Graphic
{
    public interface IDrawing 
    {
        void DrawBattleFields(Rectangle rectangle);
        void DrawShip(List<Coordinates> createShipCoords, ShipAction action);
        void EraseShip(List<Coordinates> deleteShipCoords);
        void DrawHitShip(Coordinates hitCoord, bool hit);
        void DrawDestroyShip(List<Coordinates> destroyShipCoords);
    }
}
