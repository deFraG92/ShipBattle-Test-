using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GameUtils;
using ClientsPart;
using System.Web;

namespace Graphic
{
    public class Drawing : IDrawing
    {
        private readonly Graphics _graph;
        private Coordinates _gameOptionList;
        private Coordinates _gridOptionList;
        private readonly GameOptions _options;
        private List<Rectangle> _bufRocketCoords;

        public Drawing(Graphics graph, GameOptions options)
        {
            _options = options;
            _graph = graph;
            _gridOptionList = options.GridOptions;
            _gameOptionList = options.GameOption;
            
        }

        public void DrawBattleFields(Rectangle rectangle)
        {
            var pen = new Pen(_options.GridColor, 1);
            DrawBattleField(pen, _options.MyBattleFieldLocation, rectangle);
            DrawBattleField(pen, _options.EnemyBattleFieldLocation, rectangle);
        }

        public void DrawShip(List<Coordinates> createShipCoords, ShipAction action)
        {
            var pen = action == ShipAction.CreateShip ? new Pen(_options.ShipColor, 3) : new Pen(_options.FieldColor, 3);
            if (createShipCoords != null)
            {
                foreach (var shipCoord in createShipCoords)
                {
                    _graph.DrawRectangle(pen, (int)shipCoord.X, (int)shipCoord.Y,  (int)_gridOptionList.X, (int)_gridOptionList.Y);
                }

                if (action == ShipAction.DeleteShip)
                {
                    pen = new Pen(_options.GridColor, 1);
                    foreach (var shipCoord in createShipCoords)
                    {
                        _graph.DrawRectangle(pen, (int)shipCoord.X, (int)shipCoord.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
                    }
                }
            }
        }

        public void EraseShip(List<Coordinates> deleteShipCoords) { }
        public void DrawHitShip(Coordinates hitCoord, bool hit) 
        {
            if (hit == true)
            {
                var pen = new Pen(_options.HitShipColor, 2);
                _graph.DrawLine(pen, (int)hitCoord.X, (int)hitCoord.Y, (int)(hitCoord.X + _gridOptionList.X),
                                     (int)(hitCoord.Y + _gridOptionList.Y));
                _graph.DrawLine(pen, (int)(hitCoord.X + _gridOptionList.X), (int)hitCoord.Y, (int)(hitCoord.X),
                                     (int)(hitCoord.Y + _gridOptionList.Y));
            }

            else
            {
                _graph.FillRectangle(new SolidBrush(Color.LightGray),
                                    (int)hitCoord.X, (int)hitCoord.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
            }
        }

        public void DrawDestroyShip(List<Coordinates> destroyShipCoords)
        {
            var pen = new Pen(_options.GridColor, 2);
            foreach (var shipCoord in destroyShipCoords)
            {
                this._graph.DrawRectangle(pen, (int)shipCoord.X, (int)shipCoord.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
                this._graph.FillRectangle(new SolidBrush(_options.DestroyShipColor), (int)shipCoord.X, (int)shipCoord.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
            }
        }

        private void DrawBattleField(Pen pen, Coordinates deltaCoord, Rectangle rectangle)
        {
            if (!rectangle.IsEmpty)
            {
                DrawAnimationRocket(rectangle, pen);
                return;
            }
            for (double i = deltaCoord.X; i <= (_gameOptionList + deltaCoord).X; i += _gridOptionList.X)
            {
                _graph.DrawLine(pen, new Point((int)i, (int)deltaCoord.Y),
                                      new Point((int)i, (int)(_gameOptionList + deltaCoord).Y));
            }
            for (double i = deltaCoord.Y; i <= (_gameOptionList + deltaCoord).Y; i += _gridOptionList.Y)
            {
                _graph.DrawLine(pen, new Point((int)deltaCoord.X, (int)i),
                    new Point((int)(_gameOptionList + deltaCoord).X, (int)i));
            }

        }

        private Rectangle GetDeltaRectangle(Coordinates coord)
        {
            var deltaX = (int)(Math.Truncate(coord.X / _gridOptionList.X) * _gridOptionList.X) - 10;
            var deltaY = (int)(Math.Truncate(coord.Y / _gridOptionList.Y) * _gridOptionList.Y);
            return new Rectangle(deltaX, deltaY, (int)_gridOptionList.X, (int)_gridOptionList.Y);
        }

        private List<Rectangle> ReturnRedrawRects(Rectangle rectangle)
        {
            bool first = false;
            if (_bufRocketCoords.Equals(null))
            {
                first = true;
                _bufRocketCoords = new List<Rectangle>();

            }
            var rectangleHeadCoords = new List<Rectangle>();
            rectangleHeadCoords.Add(GetDeltaRectangle(new Coordinates(rectangle.X, rectangle.Y)));
            rectangleHeadCoords.Add(GetDeltaRectangle(new Coordinates(rectangle.X + rectangle.Width, rectangle.Y)));
            rectangleHeadCoords.Add(GetDeltaRectangle(new Coordinates(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height)));
            rectangleHeadCoords.Add(GetDeltaRectangle(new Coordinates(rectangle.X, rectangle.Y + rectangle.Height)));
            rectangleHeadCoords.Distinct();
            var redrawList = new List<Rectangle>();
            if (!first)
            {
                redrawList = (from bufRocketCoord in _bufRocketCoords
                    from rectangleHeadCoord in rectangleHeadCoords
                    where !bufRocketCoord.Contains(rectangleHeadCoord)
                    select bufRocketCoord).ToList();
            }
            foreach (var rectangleHeadCoord in
                    rectangleHeadCoords.Where(rectangleHeadCoord => !_bufRocketCoords.Contains(rectangleHeadCoord)))
            {
                _bufRocketCoords.Add(rectangleHeadCoord);
            }
            if (!first)
            {
                foreach (var item in redrawList.Where(item => _bufRocketCoords.Contains(item)))
                {
                    _bufRocketCoords.Remove(item);
                }
            }
            return redrawList;
        }


        private void DrawAnimationRocket(Rectangle rectangle, Pen pen)
        {
            var baseRect1 = new Rectangle((int)_options.MyBattleFieldLocation.X, (int)_options.MyBattleFieldLocation.Y, (int)_gameOptionList.X, (int)_gameOptionList.Y);
            var baseRect2 = new Rectangle((int)_options.EnemyBattleFieldLocation.X, (int)_options.EnemyBattleFieldLocation.Y, (int)_gameOptionList.X, (int)_gameOptionList.Y);
            if (baseRect1.Contains(rectangle) | baseRect2.Contains(rectangle))
            {
                if (_bufRocketCoords.Equals(null))
                {
                    _bufRocketCoords = new List<Rectangle>();
                }


            }
        }



    }
}
