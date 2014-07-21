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

        protected void FillShipList(double[] GreatShipList, int rowCount, int colCount)
        {
            this._ShipList = new double[rowCount * colCount];
            if (GreatShipList.Length == this._ShipList.Length)
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        this._ShipList[i * colCount + j] = GreatShipList[i * colCount + j];
                    }
                }
        }
        
        protected double[] _ShipList;
        public double GetShipListByIndex(int index)
        {
            if (_ShipList.Length > 0)
                return this._ShipList[index];
            else
                throw new Exception("_ShipList is Empty!");
        }

        public double[] GetShipList()
        {
            return this._ShipList;
        }
        protected int _RowCount;
        protected int _ColCount;
        public int RowCount
        {
            get
            {
                return this._RowCount;
            }

            set
            {
                this._RowCount = value;
            }
        }

        public int ColCount
        {
            get
            {
                return this._ColCount;
            }

            set
            {
                this._ColCount = value;
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
            this._RowCount = 4;
            this._ColCount = 1;
            this.FillShipList(GreatShipList, this._RowCount, this._ColCount);
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
            this._RowCount = 3;
            this._ColCount = 1;
            this.FillShipList(GreatShipList, this._RowCount, this._ColCount);
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
            this._RowCount = 2;
            this._ColCount = 1;
            this.FillShipList(GreatShipList, this._RowCount, this._ColCount);
        }
    }

    public class SmallShip : Ship
    {
        public SmallShip() : base()
        {
            double[] GreatShipList =  { 
                                          1 
                                      };
            this._RowCount = 1;
            this._ColCount = 1;
            this.FillShipList(GreatShipList, this._RowCount, this._ColCount);
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
            this._RowCount = 4;
            this._ColCount = 3;
            this.FillShipList(GreatShipList, this._RowCount, this._ColCount);
        }
    }
}
