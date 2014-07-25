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
        protected Trajectory Trajectory;
        protected ImageHandler ImgRocketHandler;
        protected ImageHandler ImgBangHandler;
        public abstract List<Coordinates> InitTrajectory(Coordinates startCoords, Coordinates destination, string direction = "FromLeftToRight");
        public abstract Image Fly(Coordinates rocketCoord);
        public abstract Image BangBang(object target);
    }

    public class RocketPrototype1 : Rocket
    {
        public RocketPrototype1(IEnumerable<Image> rocketImgCollection, IEnumerable<Image> bangImgCollection, Coordinates pictureSettings)
        {
            ImgRocketHandler = new ImageHandler();
            foreach (var img in rocketImgCollection)
            {
                ImgRocketHandler.AddImage(new Bitmap(img, (int)pictureSettings.X, (int)pictureSettings.Y));
            }
            ImgBangHandler = new ImageHandler();
            foreach (var img in bangImgCollection)
            {
                ImgBangHandler.AddImage(new Bitmap(img, (int)pictureSettings.X, (int)pictureSettings.Y));
            }
            
        }

        public override List<Coordinates> InitTrajectory(Coordinates startCoords, Coordinates destination, string direction = "FromLeftToRight")
        {
            Trajectory = new BezjeTrajectory();
            var pathCollection = (List<Coordinates>)Trajectory.GetTrajectory(new List<Coordinates> { 
                                                                                                    startCoords,
                                                                                                    new Coordinates((startCoords.X + destination.X) / 2, 50),
                                                                                                    destination
                                                                                                }, 20, direction);
            return pathCollection;
        }

        public override Image Fly(Coordinates rocketCoord)
        {
            if ((Trajectory != null) & (ImgRocketHandler != null))
            {
                double angle = Trajectory.GetAngleByCoord(rocketCoord);
                return ImgRocketHandler.GetRotateImage(ImgRocketHandler.GetImageByIndex(0), angle);
            }
            else
                throw new Exception("Fly");
        }

        public override Image BangBang(object target)
        {
            return ImgBangHandler.GetImageByIndex((int)target);
        }
    }
}
