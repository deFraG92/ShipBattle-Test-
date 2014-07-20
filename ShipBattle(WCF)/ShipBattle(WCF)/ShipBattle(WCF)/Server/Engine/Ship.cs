using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public abstract class Ship
    {
        public Ship()
        {
           
        }

        protected void FillShipList(double[] greatShipList, int rowCount, int colCount)
        {
            this._shipList = new double[rowCount * colCount];
            if (greatShipList.Length == this._shipList.Length)
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        this._shipList[i * colCount + j] = greatShipList[i * colCount + j];
                    }
                }
        }
        
        protected double[] _shipList;
        public double GetShipListByIndex(int index)
        {
            if (_shipList.Length > 0)
                return this._shipList[index];
            else
                throw new Exception("_ShipList is Empty!");
        }

        public double[] GetShipList()
        {
            return this._shipList;
        }
        protected int _rowCount;
        protected int _colCount;
        public int RowCount
        {
            get
            {
                return this._rowCount;
            }

            set
            {
                this._rowCount = value;
            }
        }

        public int ColCount
        {
            get
            {
                return this._colCount;
            }

            set
            {
                this._colCount = value;
            }
        }
    }

    public class GreatShip : Ship
    {
        public GreatShip() : base()
        {
            double[] GreatShipList = 
                                        {
                                            1,
                                            1,
                                            1,
                                            1
                                        };
            this._rowCount = 4;
            this._colCount = 1;
            this.FillShipList(GreatShipList, this._rowCount, this._colCount);
        }

    }

    public class BigShip : Ship
    {
        public BigShip() : base()
        {
            double[] GreatShipList = 
                                        {
                                            1, 
                                            1, 
                                            1
                                        };
            this._rowCount = 3;
            this._colCount = 1;
            this.FillShipList(GreatShipList, this._rowCount, this._colCount);
        }
    }

    public class MediumShip : Ship
    {
        public MediumShip() : base()
        {
            double[] GreatShipList = 
                                        {
                                            1,
                                            1
                                        };
            this._rowCount = 2;
            this._colCount = 1;
            this.FillShipList(GreatShipList, this._rowCount, this._colCount);
        }
    }

    public class SmallShip : Ship
    {
        public SmallShip() : base()
        {
            double[] GreatShipList =  { 
                                          1 
                                      };
            this._rowCount = 1;
            this._colCount = 1;
            this.FillShipList(GreatShipList, this._rowCount, this._colCount);
        }
    }

    public class SubMarine : Ship
    {
        public SubMarine() : base()
        {
            double[] GreatShipList =  { 
                                          0, 1, 0,
                                          0, 1, 0,
                                          1, 1, 1,
                                          0, 1, 0
                                      };
            this._rowCount = 4;
            this._colCount = 3;
            this.FillShipList(GreatShipList, this._rowCount, this._colCount);
        }
    }
}
