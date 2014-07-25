using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameUtils;

namespace Engine
{
    public interface IShipAction
    {
        List<Coordinates> CreateShip(Coordinates coord);
        List<Coordinates> GetShipImage();
        void RotateShip();
        List<Coordinates> DeleteShip(Coordinates coord);
        Coordinates HitTheShip(Coordinates coord, string uid, out bool hit, out bool IsDestroy);
        List<Coordinates> DestroyShipsCoords(Coordinates coord, string uid);
    }
}
