using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameUtils;

namespace Shooting
{
    public class Vector
    {
        private Coordinates _start;
        private Coordinates _end;
        public Vector(Coordinates start, Coordinates end)
        {
            _start.X = start.X;
            _start.Y = start.Y;
            _end.X = end.X;
            _end.Y = end.Y;
        }

        public Coordinates GetDeltaStep(int n)
        {
            var resCoords = _end - _start;
            return new Coordinates(resCoords.X / (n + 1), resCoords.Y / (n + 1));
        }

        private double GetAngleInGradus(double angle)
        {
            return angle * 180 / Math.PI;
        }

        public double SetInclineAngleForCoord(Coordinates coord, string direction = "FromLeftToRight")
        {
            double k = (_end.Y - _start.Y) / (_end.X - _start.X);
            double angle = GetAngleInGradus(Math.Atan(k));
            if (direction == "FromLeftToRight")
                angle = angle + 90;
            else if (direction == "FromRightToLeft")
                angle = angle + 90 + 180;
            return angle;
        }

        public double GetLength()
        {
            return Math.Sqrt((_start.X - _end.X) * (_start.X - _end.X) + (_start.Y - _end.Y) * (_start.Y - _end.Y));
        }
    }
}
