using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using GameUtils;
using ClientsPart;
using Microsoft.VisualBasic.PowerPacks;

namespace Graphic
{
    public class Drawing : IDrawing
    {
        private readonly Graphics _graph;
        private Coordinates _gameOptionList;
        private Coordinates _gridOptionList;
        private readonly GameOptions _options;
        //private readonly RectangleData _rectData;
        private IEnumerable<LineShape> _lineCollect;

        public Drawing(Graphics graph, GameOptions options)
        {
            _options = options;
            _graph = graph;
            _gridOptionList = options.GridOptions;
            _gameOptionList = options.GameOption;
            //_rectData = new RectangleData(options.MyBattleFieldLocation, options.GameOption, options.GridOptions);
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
            if (hit)
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
                _graph.DrawRectangle(pen, (int)shipCoord.X, (int)shipCoord.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
                _graph.FillRectangle(new SolidBrush(_options.DestroyShipColor), (int)shipCoord.X, (int)shipCoord.Y, (int)_gridOptionList.X, (int)_gridOptionList.Y);
            }
        }

        private void DrawBattleField(Pen pen, Coordinates deltaCoord, Rectangle rectangle)
        {
            if (!rectangle.IsEmpty)
            {
                DrawAnimationRocket(rectangle, pen);
                return;
            }
            for (var i = deltaCoord.X; i <= (_gameOptionList + deltaCoord).X; i += _gridOptionList.X)
            {
                _graph.DrawLine(pen, new Point((int)i, (int)deltaCoord.Y),
                                      new Point((int)i, (int)(_gameOptionList + deltaCoord).Y));
            }
            for (var i = deltaCoord.Y; i <= (_gameOptionList + deltaCoord).Y; i += _gridOptionList.Y)
            {
                _graph.DrawLine(pen, new Point((int)deltaCoord.X, (int)i),
                    new Point((int)(_gameOptionList + deltaCoord).X, (int)i));
            }

        }
        
        private void DrawAnimationRocket(Rectangle rectangle, Pen pen)
        {
            var baseRect1 = new Rectangle((int)_options.MyBattleFieldLocation.X, (int)_options.MyBattleFieldLocation.Y, (int)_gameOptionList.X, (int)_gameOptionList.Y);
            var baseRect2 = new Rectangle((int)_options.EnemyBattleFieldLocation.X, (int)_options.EnemyBattleFieldLocation.Y, (int)_gameOptionList.X, (int)_gameOptionList.Y);
            if (baseRect1.Contains(rectangle) | baseRect2.Contains(rectangle))
            {
                foreach (var rect in ReturnRedrawRects(rectangle))
                {
                    //_graph.DrawLine(pen, rect.X1, rect.Y1, rect.X2, rect.Y2);
                    _graph.DrawLine(new Pen(Color.Blue, 2), rect.X1, rect.Y1, rect.X2, rect.Y2);
                }
            }
        }

        private IEnumerable<LineShape> ReturnRedrawRects(Rectangle rectangle)
        {
            if (_lineCollect == null)
            {
                _lineCollect = new List<LineShape>();
            }
            var resultRects = new List<LineShape>(_lineCollect);
            //_lineCollect = new List<LineShape>(_rectData.ReturnIntersectionLineCoordinates(rectangle));
            return resultRects;
        }

        



    }
}
