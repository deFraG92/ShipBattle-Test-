using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameUtils;

namespace Engine
{

    public class ShipAction : IShipAction
    {
        public ShipAction(string ShipType, string shipTurning, string Uid)
        {
            this._uid = Uid;
            Type shipType = Type.GetType("Engine." + ShipType);
            this._ship = (Ship)Activator.CreateInstance(shipType);
            this._shipTurning = shipTurning;
        }
        
        public void RotateShip()
        {
            int Buf = this._ship.ColCount;
            this._ship.ColCount = this._ship.RowCount;
            this._ship.RowCount = Buf;
            
        }

        public List<Coordinates> DeleteShip(Coordinates coord)
        {
            Coordinates RoundCoord = RoundCoords(coord, _uid);
            int counter = 0;

            foreach (var Ship in _bufList[this._uid])
            {
                if (Ship.Contains(RoundCoord))
                {
                    _bufList[this._uid].RemoveAt(counter);
                    break;
                }
                counter++;
            }

            foreach (var Ship in _shipCoords[this._uid])
            {
                if (Ship.Contains(RoundCoord))
                {
                    _shipCoords[this._uid].Remove(Ship);
                    return Ship;
                }

            }
            return null;
        }

        private bool CheckList(Coordinates shipCoords)
        {
            if (
                  (shipCoords.X <= _gameOptionCoords[_uid].X - _gridOptionCoords[_uid].X + _deltaCoords[_uid].X) // up border
                & (shipCoords.Y <= _gameOptionCoords[_uid].Y - _gridOptionCoords[_uid].Y + _deltaCoords[_uid].Y) // down border
                & (shipCoords.X >= _deltaCoords[_uid].X) // left border
                & (shipCoords.Y >= _deltaCoords[_uid].Y) // right border
               )

            {
                if (_shipCoords.ContainsKey(this._uid))
                {
                    foreach (var List in _bufList[this._uid])
                    {
                        if (
                                List.Contains(shipCoords)
                              | List.Contains(new Coordinates(shipCoords.X + _gridOptionCoords[_uid].X, shipCoords.Y))
                              | List.Contains(new Coordinates(shipCoords.X, shipCoords.Y + _gridOptionCoords[_uid].Y))
                              | List.Contains(new Coordinates(shipCoords.X + _gridOptionCoords[_uid].X, shipCoords.Y + _gridOptionCoords[_uid].Y))
                             )
                        {
                            return true;
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

        private Coordinates RoundCoords(Coordinates coord, string uid)
        {
            Coordinates roundCoord = new Coordinates();
            double x = Math.Truncate((coord.X - _deltaCoords[uid].X) / _gridOptionCoords[uid].X);
            double y = Math.Truncate((coord.Y - _deltaCoords[uid].Y) / _gridOptionCoords[uid].Y);
            roundCoord.X = _deltaCoords[uid].X + x * _gridOptionCoords[uid].X;
            roundCoord.Y = _deltaCoords[uid].Y + y * _gridOptionCoords[uid].Y;
            return roundCoord;
        }

        //public int[] GetShipLength()
        //{
        //    if (this._Ship.GetShipList() != null)
        //    {
        //        int shipRows = this._Ship.RowCount;
        //        int shipCols = this._Ship.ColCount;
        //        if (shipCols > 0)
        //            return new int[]{shipRows, shipCols};
        //    }
        //    return null;
        //}

        private void FillBuffList(List<Coordinates> coords)
        {
            
            int counter = 0;
            var dict = new List<List<Coordinates>>();
            if (!_bufList.ContainsKey(this._uid))
            {
                _bufList.Add(this._uid, dict);
            }

            var myList = new List<Coordinates>();
            foreach (var coord in coords)
            {
                counter++;
                myList.Add(coord);

                if (counter != coords.Count)
                {
                    if (this._shipTurning == "TurnShip")
                    {
                        myList.Add(new Coordinates(coord.X + _gridOptionCoords[_uid].X, coord.Y));
                    }
                    else if (this._shipTurning == "StraightShip")
                    {
                        myList.Add(new Coordinates(coord.X, coord.Y + _gridOptionCoords[_uid].Y));
                    }
                }
                else
                {
                    myList.Add(new Coordinates(coord.X + _gridOptionCoords[_uid].X, coord.Y));
                    myList.Add(new Coordinates(coord.X, coord.Y + _gridOptionCoords[_uid].Y));
                    myList.Add(new Coordinates(coord.X + _gridOptionCoords[_uid].X, coord.Y + _gridOptionCoords[_uid].Y));
                }



            }
            _bufList[this._uid].Add(myList);
        }

        public List<Coordinates> GetShipImage()
        {
            if (this._ship != null)
            {
                int RowCount = this._ship.RowCount;
                int ColCount = this._ship.ColCount;
                var shipImageList = new List<Coordinates>();
                var shipImageCoords = new Coordinates();
                for (int j = 0; j < ColCount; j++)
                {
                    for (int i = 0; i < RowCount; i++)
                    {
                        if (this._ship.GetShipList()[i * ColCount + j] == 1)
                        {
                            shipImageCoords.X = shipImageCoords.X + j * _gridOptionCoords[this._uid].X;
                            shipImageCoords.Y = shipImageCoords.Y + i * _gridOptionCoords[this._uid].Y;
                            shipImageList.Add(shipImageCoords);
                        }
                    }
                }
                return shipImageList;
            }
            return null;
        }

        public List<Coordinates> CreateShip(Coordinates coord)
        {
            if (_shipCoords.ContainsKey(this._uid) )
            {
                if ( _shipCoords[this._uid].Count >= _shipsCount[this._uid] )
                    return null;
            }
            Coordinates ShipCoords = new Coordinates();
            Coordinates RoundCoord = RoundCoords(coord, _uid);
            ShipCoords.X = RoundCoord.X;
            ShipCoords.Y = RoundCoord.Y;
            var ListOfShipCoords = new List<Coordinates>();
            int RowCount = this._ship.RowCount;
            int ColCount = this._ship.ColCount;

            for (int j = 0; j < ColCount; j++)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    if (this._ship.GetShipList()[i * ColCount + j] == 1)
                    {
                        ShipCoords.X = ShipCoords.X + j * _gridOptionCoords[this._uid].X;
                        ShipCoords.Y = ShipCoords.Y + i * _gridOptionCoords[this._uid].Y;

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
                if (!_shipCoords.ContainsKey(this._uid))
                {
                    var NewShip = new List<List<Coordinates>>();
                    NewShip.Add(ListOfShipCoords);
                    _shipCoords.Add(this._uid, NewShip);
                }
                else
                    _shipCoords[this._uid].Add(ListOfShipCoords);
                return ListOfShipCoords;
            }
            return null;
        }

        public Coordinates HitTheShip(Coordinates coord, string Uid, out bool hit, out bool isDestroy)
        {
            isDestroy = false;
            hit = false;
            var roundCoords = new Coordinates(0, 0);
            foreach (var ship in _shipCoords)
            {
                if (ship.Key != Uid)
                {
                    roundCoords = RoundCoords(coord, ship.Key); 
                    foreach (var List in ship.Value)
                    {
                        if (List.Contains(roundCoords))
                        {
                            hit = true;
                            List.Remove(roundCoords);
                            if (List.Count == 0)
                            {
                                isDestroy = true;
                                ship.Value.Remove(List);
                            }
                            return roundCoords;
                        }
                    }
                }
            }
            return roundCoords;
        }

        public List<Coordinates> DestroyShipsCoords(Coordinates coord, string uid)
        {
            
            foreach (var Ship in _forDestroyShips)
            {
                if (Ship.Key != uid)
                {
                    Coordinates roundCoords = RoundCoords(coord, Ship.Key);
                    foreach (var ship in Ship.Value)
                    {
                        if (ship.Contains(roundCoords))
                            return ship;
                    }
                }
            }
            return null;
        }

        private string _shipTurning;
        private string _uid;
        private Ship _ship;
        
        private static Dictionary<string, List<List<Coordinates>>> _bufList = new Dictionary<string, List<List<Coordinates>>>();
        private static Dictionary<string, List<List<Coordinates>>> _shipCoords = new Dictionary<string, List<List<Coordinates>>>();


        private static Dictionary<string, int> _shipsCount = new Dictionary<string,int>();
        public static void SetShipsCount(string uid, int shipsCount)
        {
            if (!_shipsCount.ContainsKey(uid))
                _shipsCount.Add(uid, shipsCount);
            else
                _shipsCount[uid] = shipsCount;
        }
        private static Dictionary<string, Coordinates> _gameOptionCoords = new Dictionary<string, Coordinates>();
        public static void SetGameOptionCoords(string uid, Coordinates gameCoords)
        {
            if (!_gameOptionCoords.ContainsKey(uid))
                _gameOptionCoords.Add(uid, gameCoords);
            else
                _gameOptionCoords[uid] = gameCoords;
        }
        private static Dictionary<string, Coordinates> _gridOptionCoords = new Dictionary<string, Coordinates>();
        public static void SetGridOptionCoords(string uid, Coordinates gridCoords)
        {
            if (!_gridOptionCoords.ContainsKey(uid))
                _gridOptionCoords.Add(uid, gridCoords);
            else
                _gridOptionCoords[uid] = gridCoords;
            
        }
        private static Dictionary<string, List<List<Coordinates>>> _forDestroyShips;
        public static void ForDestroyShipsInit()
        {
            _forDestroyShips = new Dictionary<string, List<List<Coordinates>>>();
            foreach (var Ship in _shipCoords)
            {
                _forDestroyShips.Add(Ship.Key, new List<List<Coordinates>>());
                for (int i = 0; i < Ship.Value.Count; i++)
                { 
                    _forDestroyShips[Ship.Key].Add(new List<Coordinates>());
                    for (int j = 0; j < Ship.Value[i].Count; j++)
                    {
                        _forDestroyShips[Ship.Key][i].Add(Ship.Value[i][j]);
                    }
                }
            }
        }
        private static Dictionary<string, Coordinates> _deltaCoords = new Dictionary<string, Coordinates>();
        public static void SetDeltaCoords(string uid, Coordinates deltaCoords)
        {
            if (!_deltaCoords.ContainsKey(uid))
                _deltaCoords.Add(uid, deltaCoords);
            else
                _deltaCoords[uid] = deltaCoords;
        }
        
    }
}