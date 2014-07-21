using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameUtils;

namespace Engine
{

    public class ShipAction : IShipAction
    {
        public ShipAction(string ShipType, string Uid)
        {
            this._Uid = Uid;
            Type shipType = Type.GetType("Engine." + ShipType);
            this._Ship = (Ship)Activator.CreateInstance(shipType);
        }
        
        public void RotateShip()
        {
            int Buf = this._Ship.ColCount;
            this._Ship.ColCount = this._Ship.RowCount;
            this._Ship.RowCount = Buf;
            
        }

        public List<Coordinates> DeleteShip(Coordinates coord)
        {
            foreach (var Ship in _ShipCoords[this._Uid])
            {
                if (Ship.Contains(coord))
                {
                    _ShipCoords.Remove(this._Uid);
                    return Ship;
                }

            }
            return null;
        }

        private bool CheckList(Coordinates ShipCoords)
        {
            if (
                  (ShipCoords.X <= _GameOptionCoords[this._Uid].X - _GridOptionCoords[this._Uid].X) // up border
                & (ShipCoords.Y <= _GameOptionCoords[this._Uid].Y - _GridOptionCoords[this._Uid].Y) // down border
                & (ShipCoords.X >= 0) // left border
                & (ShipCoords.Y >= 0) // right border
               )
            {
                if (_ShipCoords.ContainsKey(this._Uid))
                {
                    foreach (var List in _BufList)
                    {
                        if (List.Key == this._Uid)
                        { 
                            if (
                                  List.Value.Contains(ShipCoords)
                                | List.Value.Contains(new Coordinates(ShipCoords.X + _GridOptionCoords[this._Uid].X, ShipCoords.Y))
                                | List.Value.Contains(new Coordinates(ShipCoords.X, ShipCoords.Y + _GridOptionCoords[this._Uid].Y))
                                | List.Value.Contains(new Coordinates(ShipCoords.X + _GridOptionCoords[this._Uid].X, ShipCoords.Y + _GridOptionCoords[this._Uid].Y))
                               )
                            {
                                return true;
                            }
                        }
                    }
                    return false;

                }
                else
                    return false;
            }
            else
                return true;
        }

        private Coordinates RoundCoords(Coordinates Coord)
        {
            Coordinates RoundCoord = new Coordinates();
            RoundCoord.X = Math.Truncate( (Coord.X / _GridOptionCoords[this._Uid].X) ) * _GridOptionCoords[this._Uid].X;
            RoundCoord.Y = Math.Truncate( (Coord.Y / _GridOptionCoords[this._Uid].Y) ) * _GridOptionCoords[this._Uid].Y;
            return RoundCoord;
        }

        private void FillBuffList(List<Coordinates> Coords)
        {
            int counter = 0;
            if (!_BufList.ContainsKey(this._Uid))
            {
                _BufList.Add(this._Uid, new List<Coordinates>());
            }
            foreach (var coord in Coords)
            {
                counter++;
                _BufList[this._Uid].Add(coord);
                if (counter != Coords.Count)
                {
                    _BufList[this._Uid].Add(new Coordinates(coord.X + _GridOptionCoords[this._Uid].X, coord.Y));
                }
                else
                {
                    _BufList[this._Uid].Add(new Coordinates(coord.X + _GridOptionCoords[this._Uid].X, coord.Y));
                    _BufList[this._Uid].Add(new Coordinates(coord.X, coord.Y + _GridOptionCoords[this._Uid].Y));
                    _BufList[this._Uid].Add(new Coordinates(coord.X + _GridOptionCoords[this._Uid].X, coord.Y + _GridOptionCoords[this._Uid].Y));
                }
            }
        }

        public List<Coordinates> CreateShip(Coordinates coord)
        {
            if (_ShipCoords.ContainsKey(this._Uid) )
            {
                if ( _ShipCoords[this._Uid].Count <= _ShipsCount[this._Uid] )
                    return null;
            }
            Coordinates ShipCoords = new Coordinates();
            Coordinates RoundCoord = RoundCoords(coord);
            ShipCoords.X = RoundCoord.X;
            ShipCoords.Y = RoundCoord.Y;
            var ListOfShipCoords = new List<Coordinates>();
            int RowCount = this._Ship.RowCount;
            int ColCount = this._Ship.ColCount;

            for (int j = 0; j < ColCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    if (this._Ship.GetShipList()[i * ColCount + j] == 1)
                    {
                        ShipCoords.X = ShipCoords.X + j * _GridOptionCoords[this._Uid].X;
                        ShipCoords.Y = ShipCoords.Y + i * _GridOptionCoords[this._Uid].Y;

                        if (!this.CheckList(ShipCoords))
                        {
                            ListOfShipCoords.Add(ShipCoords);
                            ShipCoords.X = RoundCoord.X;
                            ShipCoords.Y = RoundCoord.Y;
                        }
                        else
                        {
                            ListOfShipCoords.Clear();
                            ColCount = 0;
                            break;
                        }
                    }
                }
            }
            if (ListOfShipCoords.Count > 0)
            {
                this.FillBuffList(ListOfShipCoords);
                if (!_ShipCoords.ContainsKey(this._Uid))
                {
                    var NewShip = new List<List<Coordinates>>();
                    NewShip.Add(ListOfShipCoords);
                    _ShipCoords.Add(this._Uid, NewShip);
                }
                else
                    _ShipCoords[this._Uid].Add(ListOfShipCoords);
                return ListOfShipCoords;
            }
            return null;
        }

        public static bool HitTheShip(Coordinates coord, string Uid, out bool IsDestroy)
        {
            IsDestroy = false;
            foreach (var Ship in _ShipCoords)
            {
                if (Ship.Key != Uid)
                {
                    foreach (var List in Ship.Value)
                    {
                        if (List.Contains(coord))
                        {
                            List.Remove(coord);
                            if (List.Count == 0)
                            {
                                IsDestroy = true;
                                Ship.Value.Remove(List);
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static List<Coordinates> DestroyShipsCoords(Coordinates coord, string Uid)
        {
            foreach (var Ship in _ForDestroyShips)
            {
                if (Ship.Key != Uid)
                {
                    foreach (var ship in Ship.Value)
                    {
                        if (ship.Contains(coord))
                            return ship;
                    }
                }
            }
            return null;
        }
        private string _Uid;
        private Ship _Ship;
        private static Dictionary<string, List<Coordinates>> _BufList = new Dictionary<string, List<Coordinates>>();
        private static Dictionary<string, List<List<Coordinates>>> _ShipCoords = new Dictionary<string, List<List<Coordinates>>>();


        private static Dictionary<string, int> _ShipsCount = new Dictionary<string,int>();
        public static void SetShipsCount(string Uid, int ShipsCount)
        {
            if (!_ShipsCount.ContainsKey(Uid))
                _ShipsCount.Add(Uid, ShipsCount);
            else
                _ShipsCount[Uid] = ShipsCount;
        }
        private static Dictionary<string, Coordinates> _GameOptionCoords = new Dictionary<string, Coordinates>();
        public static void SetGameOptionCoords(string Uid, Coordinates GameCoords)
        {
            if (!_GameOptionCoords.ContainsKey(Uid))
                _GameOptionCoords.Add(Uid, GameCoords);
            else
                _GameOptionCoords[Uid] = GameCoords;
        }
        private static Dictionary<string, Coordinates> _GridOptionCoords = new Dictionary<string, Coordinates>();
        public static void SetGridOptionCoords(string Uid, Coordinates GridCoords)
        {
            if (!_GridOptionCoords.ContainsKey(Uid))
                _GridOptionCoords.Add(Uid, GridCoords);
            else
                _GridOptionCoords[Uid] = GridCoords;
            
        }
        private static Dictionary<string, List<List<Coordinates>>> _ForDestroyShips;
        public static void ForDestroyShipsInit()
        {
            _ForDestroyShips = new Dictionary<string, List<List<Coordinates>>>();
            foreach (var Ship in _ShipCoords)
            {
                _ForDestroyShips.Add(Ship.Key, new List<List<Coordinates>>());
                for (int i = 0; i < Ship.Value.Count; i++)
                { 
                    _ForDestroyShips[Ship.Key].Add(new List<Coordinates>());
                    for (int j = 0; j < Ship.Value[i].Count; j++)
                    {
                        _ForDestroyShips[Ship.Key][i].Add(Ship.Value[i][j]);
                    }
                }
            }
        }
        
    }
}