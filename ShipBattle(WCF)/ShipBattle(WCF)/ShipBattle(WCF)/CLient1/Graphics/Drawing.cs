using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GameUtils;
using ClientsPart;
using System.Web;

namespace Graphic
{
    // test com
    public class Drawing : IDrawing
    {
        private Graphics _graph;
        private Coordinates _gameOptionList;
        private Coordinates _gridOptionList;
        private GameOptions _options;
        private Rectangle _bufRocketCoords;

        public Drawing(Graphics graph, GameOptions options)
        {
            _options = options;
            _graph = graph;
            _gridOptionList = options.GridOptions;
            _gameOptionList = options.GameOption;
            
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

        private void DrawAnimationRocket(Rectangle rectangle, Pen pen)
        {
            var baseRect1 = new Rectangle((int)_options.MyBattleFieldLocation.X - (int)_gridOptionList.X, (int)_options.MyBattleFieldLocation.Y - (int)_gridOptionList.Y, (int)_gameOptionList.X, (int)_gameOptionList.Y);
            var baseRect2 = new Rectangle((int)_options.EnemyBattleFieldLocation.X, (int)_options.EnemyBattleFieldLocation.Y, (int)_gameOptionList.X, (int)_gameOptionList.Y);
            if (baseRect1.Contains(rectangle) | baseRect2.Contains(rectangle))
            {
                var redrawX = (int)(Math.Truncate((rectangle.Location.X + rectangle.Width) / _gridOptionList.X) * _gridOptionList.X) - 10;
                var redrawY = (int)(Math.Truncate((rectangle.Location.Y + rectangle.Height) / _gridOptionList.Y) * _gridOptionList.Y);
                if (_bufRocketCoords.Equals(null))
                {
                    _bufRocketCoords = new Rectangle(redrawX, redrawY, rectangle.Width, rectangle.Height);
                }
                var currRocket = new Rectangle(redrawX, redrawY, (int)_gridOptionList.X, (int)_gridOptionList.Y);
                if (!_bufRocketCoords.IntersectsWith(currRocket))
                {
                    //_graph.DrawRectangle(new Pen(Color.Blue, 2), rectangle);
                    _graph.DrawRectangle(pen, _bufRocketCoords.X, _bufRocketCoords.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
                    _bufRocketCoords = new Rectangle(redrawX, redrawY, (int)_gridOptionList.X, (int)_gridOptionList.Y);
                }
            }
        }

        public void DrawBattleFields(Rectangle rectangle)
        {
            Pen pen = new Pen(_options.GridColor, 1);
            DrawBattleField(pen, _options.MyBattleFieldLocation, rectangle);
            DrawBattleField(pen, _options.EnemyBattleFieldLocation, rectangle);
        }

        public void DrawShip(List<Coordinates> createShipCoords, ShipAction action)
        {
            Pen pen;
            if (action == ShipAction.CreateShip)
                pen = new Pen(_options.ShipColor, 3);
            else
                pen = new Pen(_options.FieldColor, 3);
            if (createShipCoords != null)
            {
                foreach (var ShipCoord in createShipCoords)
                {
                    _graph.DrawRectangle(pen, (int)ShipCoord.X, (int)ShipCoord.Y,  (int)_gridOptionList.X, (int)_gridOptionList.Y);
                }

                if (action == ShipAction.DeleteShip)
                {
                    pen = new Pen(_options.GridColor, 1);
                    foreach (var ShipCoord in createShipCoords)
                    {
                        _graph.DrawRectangle(pen, (int)ShipCoord.X, (int)ShipCoord.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
                    }
                }
            }

        }

        public void EraseShip(List<Coordinates> DeleeShipCoords) { }
        public void DrawHitShip(Coordinates hitCoord, bool hit) 
        {
            Pen pen;
            if (hit == true)
            {
                pen = new Pen(_options.HitShipColor, 2);
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
            Pen pen = new Pen(_options.GridColor, 2);
            foreach (var shipCoord in destroyShipCoords)
            {
                this._graph.DrawRectangle(pen, (int)shipCoord.X, (int)shipCoord.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
                this._graph.FillRectangle(new SolidBrush(_options.DestroyShipColor), (int)shipCoord.X, (int)shipCoord.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
            }
        }

        



    }
}
