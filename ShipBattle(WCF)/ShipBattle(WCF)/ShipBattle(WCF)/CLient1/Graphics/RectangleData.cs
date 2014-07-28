using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using GameUtils;
using Microsoft.VisualBasic.PowerPacks;

namespace Graphic
{
    public class RectangleData
    {
        private readonly Coordinates _location;
        private readonly Coordinates _rectSize;
        private readonly Coordinates _rectPartitionOptions;
        /// <summary> 
        /// Create Rect Collection By Size And Partition
        /// </summary>
        /// <param name="location"></param>
        /// <param name="rectSize"></param>
        /// <param name="rectPartitionOptions"></param>
        public RectangleData(Coordinates location, Coordinates rectSize, Coordinates rectPartitionOptions)
        {
            _location = location;
            _rectSize = rectSize;
            _rectPartitionOptions = rectPartitionOptions;
        }

        public IEnumerable<LineShape> ReturnIntersectionLineCoordinates(Rectangle rect)
        {
            if (!rect.IsEmpty)
            {
                var leftUpCoords = GetDeltaRectangle(new Coordinates(rect.X, rect.Y));
                var rightDownCoords =
                    GetDeltaRectangle(new Coordinates(rect.X, rect.Y) + new Coordinates(rect.Width, rect.Height));
                var lineCollection = new List<LineShape>();
                for (var x = leftUpCoords.X; x <= rightDownCoords.X; x += (int)_rectPartitionOptions.X)
                {
                    if ((x >= rect.X) & (x <= rect.X + rect.Width))
                    {
                        lineCollection.Add(new LineShape(x, rect.Y, x, rect.Y + rect.Height));
                    }
                }
                for (var y = leftUpCoords.Y; y <= rightDownCoords.Y; y += (int)_rectPartitionOptions.Y)
                {
                    if ((y >= rect.Y) & (y <= rect.Y + rect.Height))
                    {
                        lineCollection.Add(new LineShape(rect.X, y, rect.X + rect.Width, y));
                    }
                }
                return lineCollection;
            }
            throw new Exception("ReturnIntersectionLineCoordinates"); 
        }

        //public IEnumerable<Rectangle> ReturnIntersectionRectCoordinates(Rectangle rect)
        //{
            
        //}

        private Rectangle GetDeltaRectangle(Coordinates coord)
        {
            var x = Math.Floor((coord.X - _location.X) /_rectPartitionOptions.X);
            var y = Math.Floor((coord.Y - _location.Y) / _rectPartitionOptions.Y);
            var deltaX = x * _rectPartitionOptions.X + _location.X;
            var deltaY = y * _rectPartitionOptions.Y + _location.Y;
            return new Rectangle((int)deltaX, (int)deltaY, (int)_rectPartitionOptions.X, (int)_rectPartitionOptions.Y);
        }
    }
}
