using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GameUtils;

namespace Shooting
{
    public abstract class Rocket
    {
        //public abstract void Fly(Coordinates startCoords, Coordinates destination);
        protected Trajectory _trajectory;
        protected ImageHandler _imgRocketHandler;
        protected ImageHandler _imgBangHandler;
        public abstract List<Coordinates> InitTrajectory(Coordinates startCoords, Coordinates destination, string direction = "FromLeftToRight");
        public abstract Image Fly(Coordinates rocketCoord);
        public abstract Image BangBang(object target);
    }

    public class RocketPrototype1 : Rocket
    {
        public RocketPrototype1(List<Image> rocketImgCollection, List<Image> bangImgCollection, Coordinates pictureSettings)
        {
            _imgRocketHandler = new ImageHandler();
            foreach (var img in rocketImgCollection)
            {
                _imgRocketHandler.AddImage(new Bitmap(img, (int)pictureSettings.X, (int)pictureSettings.Y));
            }
            _imgBangHandler = new ImageHandler();
            foreach (var img in bangImgCollection)
            {
                _imgBangHandler.AddImage(new Bitmap(img, (int)pictureSettings.X, (int)pictureSettings.Y));
            }
            
        }

        public override List<Coordinates> InitTrajectory(Coordinates startCoords, Coordinates destination, string direction = "FromLeftToRight")
        {
           _trajectory = new BezjeTrajectory();
            var pathCollection = (List<Coordinates>)_trajectory.GetTrajectory(new List<Coordinates> { 
                                                                                                    startCoords,
                                                                                                    new Coordinates((startCoords.X + destination.X) / 2, 50),
                                                                                                    destination
                                                                                                }, 20, direction);
            return pathCollection;
        }

        public override Image Fly(Coordinates rocketCoord)
        {
            if ((_trajectory != null) & (_imgRocketHandler != null))
            {
                double angle = _trajectory.GetAngleByCoord(rocketCoord);
                return _imgRocketHandler.GetRotateImage(_imgRocketHandler.GetImageByIndex(0), angle);
            }
            else
                throw new Exception("Fly");
        }

        public override Image BangBang(object target)
        {
            return _imgBangHandler.GetImageByIndex((int)target);
        }
    }
}
