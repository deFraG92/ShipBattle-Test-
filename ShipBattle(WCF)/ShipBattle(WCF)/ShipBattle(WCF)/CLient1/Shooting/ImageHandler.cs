using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Shooting
{
    public class ImageHandler
    {
        private readonly List<Image> _imageCollection;
        private Graphics _graphics;
        public ImageHandler()
        {
            _imageCollection = new List<Image>();
        }

        public ImageHandler(Graphics graphics)
        {
            _imageCollection = new List<Image>();
            _graphics = graphics;
        }

        public void AddImage(Image img)
        {
            if (_imageCollection != null)

                _imageCollection.Add(img);
            else
                throw new Exception("AddImage");

        }

        public Image GetImageByIndex(int index)
        {
            if (_imageCollection != null)
            {
                if (index < _imageCollection.Count)
                    return _imageCollection[index];
                else
                    throw new IndexOutOfRangeException();
            }
            else
                throw new Exception("GetImageByIndex");
        }

        public void DestroyImageByIndex(int index)
        {
            if (_imageCollection != null)
            {
                if (index < _imageCollection.Count)
                    _imageCollection.RemoveAt(index);
            }
        }

        public Image GetRotateImage(Image img, double angle)
        {
            var bitmap = new Bitmap(img.Width, img.Height);

            using (Graphics grph = Graphics.FromImage(bitmap))
            {
                grph.TranslateTransform((float)bitmap.Width / 2, (float)bitmap.Height / 2);

                grph.RotateTransform((float)angle);

                grph.TranslateTransform(-(float)bitmap.Width / 2, -(float)bitmap.Height / 2);

                grph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grph.DrawImage(img, 0, 0);

                grph.Dispose();
            }
            return bitmap;
        }

    }
}
