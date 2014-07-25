using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameUtils;

namespace Engine
{

    public class ShipAction : IShipAction
    {
        public ShipAction(string shipType, string shipTurning, string uid)
        {
            _uid = uid;
            var shipT = Type.GetType("Engine." + shipType);
            if (shipT != null)
                _ship = (Ship)Activator.CreateInstance(shipT);
            _shipTurning = shipTurning;
        }
        
        public void RotateShip()
        {
            var buf = _ship.ColCount;
            _ship.ColCount = _ship.RowCount;
            _ship.RowCount = buf;
            
        }

        public List<Coordinates> DeleteShip(Coordinates coord)
        {
            var roundCoords = RoundCoords(coord, _uid);
            var counter = 0;

            foreach (var ship in _bufList[_uid])
            {
                if (ship.Contains(roundCoords))
                {
                    _bufList[_uid].RemoveAt(counter);
                    break;
                }
                counter++;
            }

            foreach (var ship in _shipCoords[_uid])
            {
                if (ship.Contains(roundCoords))
                {
                    _shipCoords[_uid].Remove(ship);
                    return ship;
                }

            }
            return null;
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

        

        public List<Coordinates> GetShipImage()
        {
            if (this._ship != null)
            {
                var rowCount = _ship.RowCount;
                var colCount = _ship.ColCount;
                var shipImageList = new List<Coordinates>();
                var shipImageCoords = new Coordinates();
                for (int j = 0; j < colCount; j++)
                {
                    for (int i = 0; i < rowCount; i++)
                    {
                        if (this._ship.GetShipList()[i * colCount + j] == 1)
                        {
                            shipImageCoords.X = shipImageCoords.X + j * _gridOptionCoords[_uid].X;
                            shipImageCoords.Y = shipImageCoords.Y + i * _gridOptionCoords[_uid].Y;
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
                if ( _shipCoords[this._uid].Count >= _shipsCount[_uid] )
                    return null;
            }
            var shipCoords = new Coordinates();
            var roundCoord = RoundCoords(coord, _uid);
            shipCoords.X = roundCoord.X;
            shipCoords.Y = roundCoord.Y;
            var listOfShipCoords = new List<Coordinates>();
            var rowCount = this._ship.RowCount;
            var colCount = this._ship.ColCount;

            for (var j = 0; j < colCount; j++)
            {
                for (var i = 0; i < rowCount; i++)
                {
                    if (_ship.GetShipList()[i * colCount + j] == 1)
                    {
                        shipCoords.X = shipCoords.X + j * _gridOptionCoords[_uid].X;
                        shipCoords.Y = shipCoords.Y + i * _gridOptionCoords[_uid].Y;

                        if (!this.CheckList(shipCoords))
                        {
                            listOfShipCoords.Add(shipCoords);
                            shipCoords.X = roundCoord.X;
                            shipCoords.Y = roundCoord.Y;
                        }
                        else
                        {
                            listOfShipCoords.Clear();
                            colCount = 0;
                            break;
                        }
                    }
                }
            }
            if (listOfShipCoords.Count > 0)
            {
                FillBuffList(listOfShipCoords);
                if (!_shipCoords.ContainsKey(this._uid))
                {
                    var newShip = new List<List<Coordinates>>();
                    newShip.Add(listOfShipCoords);
                    _shipCoords.Add(this._uid, newShip);
                }
                else
                    _shipCoords[this._uid].Add(listOfShipCoords);
                return listOfShipCoords;
            }
            return null;
        }

        public Coordinates HitTheShip(Coordinates coord, string uid, out bool hit, out bool isDestroy)
        {
            isDestroy = false;
            hit = false;
            var roundCoords = new Coordinates(0, 0);
            foreach (var ship in _shipCoords)
            {
                if (ship.Key != uid)
                {
                    roundCoords = RoundCoords(coord, ship.Key); 
                    foreach (var list in ship.Value)
                    {
                        if (list.Contains(roundCoords))
                        {
                            hit = true;
                            list.Remove(roundCoords);
                            if (list.Count == 0)
                            {
                                isDestroy = true;
                                ship.Value.Remove(list);
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
            foreach (var destroyShip in _forDestroyShips)
            {
                if (destroyShip.Key != uid)
                {
                    Coordinates roundCoords = RoundCoords(coord, destroyShip.Key);
                    foreach (var ship in destroyShip.Value)
                    {
                        if (ship.Contains(roundCoords))
                            return ship;
                    }
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
                if (_shipCoords.ContainsKey(_uid))
                {
                    foreach (var list in _bufList[_uid])
                    {
                        if (
                                list.Contains(shipCoords)
                              | list.Contains(new Coordinates(shipCoords.X + _gridOptionCoords[_uid].X, shipCoords.Y))
                              | list.Contains(new Coordinates(shipCoords.X, shipCoords.Y + _gridOptionCoords[_uid].Y))
                              | list.Contains(new Coordinates(shipCoords.X + _gridOptionCoords[_uid].X, shipCoords.Y + _gridOptionCoords[_uid].Y))
                             )
                        {
                            return true;
                        }

                    }
                    return false;

                }
                return false;
            }
            return true;
        }

        private Coordinates RoundCoords(Coordinates coord, string uid)
        {
            var roundCoord = new Coordinates();
            var x = Math.Truncate((coord.X - _deltaCoords[uid].X) / _gridOptionCoords[uid].X);
            var y = Math.Truncate((coord.Y - _deltaCoords[uid].Y) / _gridOptionCoords[uid].Y);
            roundCoord.X = _deltaCoords[uid].X + x * _gridOptionCoords[uid].X;
            roundCoord.Y = _deltaCoords[uid].Y + y * _gridOptionCoords[uid].Y;
            return roundCoord;
        }

        private void FillBuffList(List<Coordinates> coords)
        {

            var counter = 0;
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
        // REMAKE THIS!!!
        #region
        private string _shipTurning;
        private string _uid;
        private Ship _ship;
        
        private static Dictionary<string, List<List<Coordinates>>> _bufList = new Dictionary<string, List<List<Coordinates>>>();
        private static Dictionary<string, List<List<Coordinates>>> _shipCoords = new Dictionary<string, List<List<Coordinates>>>();


        private readonly static Dictionary<string, int> _shipsCount = new Dictionary<string, int>();
        public static void SetShipsCount(string uid, int shipsCount)
        {
            if (!_shipsCount.ContainsKey(uid))
                _shipsCount.Add(uid, shipsCount);
            else
                _shipsCount[uid] = shipsCount;
        }
        private readonly static Dictionary<string, Coordinates> _gameOptionCoords = new Dictionary<string, Coordinates>();
        public static void SetGameOptionCoords(string uid, Coordinates gameCoords)
        {
            if (!_gameOptionCoords.ContainsKey(uid))
                _gameOptionCoords.Add(uid, gameCoords);
            else
                _gameOptionCoords[uid] = gameCoords;
        }
        private readonly static Dictionary<string, Coordinates> _gridOptionCoords = new Dictionary<string, Coordinates>();
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
        private readonly static Dictionary<string, Coordinates> _deltaCoords = new Dictionary<string, Coordinates>();
        public static void SetDeltaCoords(string uid, Coordinates deltaCoords)
        {
            if (!_deltaCoords.ContainsKey(uid))
                _deltaCoords.Add(uid, deltaCoords);
            else
                _deltaCoords[uid] = deltaCoords;
        }
        #endregion
        //////////////////////
    }
}