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
        void RotateShip();
        List<Coordinates> DeleteShip(Coordinates coord);
    }
}
