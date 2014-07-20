using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameUtils;

namespace Shooting
{
    public abstract class Trajectory
    {
        public abstract object GetTrajectory(List<Coordinates> shootParams, int otherSettings, string direction = "FromLeftToRight");
        public abstract double GetAngleByCoord(Coordinates coord);
        
    }

    public class BezjeTrajectory : Trajectory
    {

        private Dictionary<Coordinates, double> _angleCollection;
        public BezjeTrajectory()
        {
            _angleCollection = new Dictionary<Coordinates, double>();
        }

        public override object GetTrajectory(List<Coordinates> shootParams, int n, string direction = "FromLeftToRight")
        {
            var vect1 = new Vector(shootParams[0], shootParams[1]);
            var vect2 = new Vector(shootParams[1], shootParams[2]);

            var delta1 = vect1.GetDeltaStep(n);
            var delta2 = vect2.GetDeltaStep(n);

            var start = new Coordinates();
            var end = new Coordinates();
            var resultCoords = new List<Coordinates>();
            resultCoords.Add(shootParams[0]);
            _angleCollection.Add(shootParams[0], vect1.SetInclineAngleForCoord(shootParams[0], direction));
            int resultCounter = 0;
            for (int i = 0; i < n; i++)
            {
                resultCounter++;
                start = shootParams[0] + resultCounter * delta1;
                end = shootParams[1] + resultCounter * delta2;

                var vector = new Vector(start, end);
                var newCoord = start + resultCounter * vector.GetDeltaStep(n);
                resultCoords.Add(newCoord);
                vector.SetInclineAngleForCoord(newCoord, direction);
                _angleCollection.Add(newCoord, vector.SetInclineAngleForCoord(newCoord, direction));
            }
            resultCoords.Add(shootParams[2]);
            _angleCollection.Add(shootParams[2], vect2.SetInclineAngleForCoord(shootParams[2], direction));
            
            return resultCoords;
        }

        public override double GetAngleByCoord(Coordinates coord)
        {
            if (_angleCollection.ContainsKey(coord))
            {
                return _angleCollection[coord];
            }
            else
                throw new Exception("GetAngleByCoord");
        }
        
    }
}
