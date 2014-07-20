using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GameUtils;

namespace ClientUtils
{
    public class Drawing : IDrawing
    {
        private Coordinates _GameOptionList;
        private Coordinates _GridOptionList;

        public Drawing(Graphics Graph, Coordinates GameOptionList, Coordinates GridOptionList)
        {
            var DelaCoords = new Coordinates(1, 1);
            this._Graph = Graph;
            this._GridOptionList = GridOptionList;
            this._GameOptionList = GameOptionList + DelaCoords;
        }

        public void DrawBattleField()
        {
            Pen pen = new Pen(this._GridColor, 1);
            for (double i = 0; i <= this._GameOptionList.X; i += this._GridOptionList.X)
            {
                this._Graph.DrawLine(pen, new Point(Convert.ToInt16(i), 0),
                                     new Point(Convert.ToInt16(i), Convert.ToInt16(this._GameOptionList.Y)));

            }
            for (double i = 0; i < this._GameOptionList.Y; i += this._GridOptionList.Y)
            {
               this._Graph.DrawLine(pen, new Point(0, Convert.ToInt16(i)),
                                     new Point(Convert.ToInt16(this._GameOptionList.X), Convert.ToInt16(i)));

            }
        }

        public void DrawShip(List<Coordinates> CreateShipCoords)
        {
            Pen pen = new Pen(this._ShipColor, 3);
            
            foreach (var ShipCoord in CreateShipCoords)
            {
                this._Graph.DrawRectangle(pen, Convert.ToInt16(ShipCoord.X), Convert.ToInt16(ShipCoord.Y), Convert.ToInt16(this._GridOptionList.X), Convert.ToInt16(this._GridOptionList.Y));
            }

        }

        public void EraseShip(List<Coordinates> DeleeShipCoords) { }
        public void DrawHitShip(Coordinates HitCoord) { }
        public void DrawDestroySHip(List<Coordinates> DestroyShipCoords) { }

        private Graphics _Graph;

        private Color _ShipColor = Color.Blue;
        public Color ShipColor 
        {
            private get
            {
                return _ShipColor;
            }
            set
            {
                this._ShipColor = value;
            }
        }

        private Color _GridColor = Color.White;
        public Color GridColor 
        {
            private get
            {
                return this._GridColor;
            }
            set
            {
                this._GridColor = value;
            }
        }

        private Color _FieldColor;
        public Color FieldColor
        {
            private get
            {
                return _FieldColor;
            }
            set
            {
                this._FieldColor = value;
            }
        }

        private Color _HitShipColor;
        public Color HitShipColor
        {
            private get
            {
                return _HitShipColor;
            }
            set
            {
                this._HitShipColor = value;
            }
        }

        private Color _DestroyShipColor;
        public Color DestroyShipColor
        {
            private get
            {
                return _DestroyShipColor;
            }
            set
            {
                this._DestroyShipColor = value;
            }
        }

    }
}
