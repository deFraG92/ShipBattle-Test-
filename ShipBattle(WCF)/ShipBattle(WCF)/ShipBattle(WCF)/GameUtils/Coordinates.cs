using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameUtils
{
    public enum ShipAction
    {
        CreateShip,
        DeleteShip,
        HitTheShip,
        DestroyShip
    }

    public struct Coordinates
    {
        public double X;
        public double Y;
        public Coordinates(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Coordinates operator +(Coordinates coord1, Coordinates coord2)
        {
            Coordinates addition = new Coordinates();
            addition.X = coord1.X + coord2.X;
            addition.Y = coord1.Y + coord2.Y;
            return addition;
        }

        public static Coordinates operator -(Coordinates coord1, Coordinates coord2)
        {
            Coordinates residual = new Coordinates();
            residual.X = coord1.X - coord2.X;
            residual.Y = coord1.Y - coord2.Y;
            return residual;
        }

        public static Coordinates operator *(int h, Coordinates coord)
        {
            Coordinates result = new Coordinates();
            result.X = coord.X * h;
            result.Y = coord.Y * h;
            return result;
        }

        public static Coordinates operator /(Coordinates coord, int h)
        {
            Coordinates result = new Coordinates();
            result.X = coord.X / h;
            result.Y = coord.Y / h;
            return result;
        }

        public static bool operator >=(Coordinates coord1, Coordinates coord2)
        {
            if ((coord1.X >= coord2.X) | (coord1.Y >= coord2.Y))
                return true;
            else
                return false;
        }

        public static bool operator <=(Coordinates coord1, Coordinates coord2)
        {
            if ((coord1.X <= coord2.X) | (coord1.Y <= coord2.Y))
                return true;
            else
                return false;
        }

        public static bool operator ==(Coordinates coord1, Coordinates coord2)
        {
            if ((coord1.X == coord2.X) & (coord1.Y == coord2.Y))
                return true;
            else
                return false;
        }

        public static bool operator !=(Coordinates coord1, Coordinates coord2)
        {
            if ((coord1.X != coord2.X) | (coord1.Y != coord2.Y))
                return true;
            else
                return false;
        }
    }
}
