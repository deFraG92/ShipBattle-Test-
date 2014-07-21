using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using GameUtils;
using System.ServiceModel;
using Graphic;
using Shooting;
using System.IO;
using System.Windows.Forms;
using System.Threading;


namespace ClientsPart
{
    public class Game : IGame
    {
        private IGameContract _contract;
        private bool _isConnected = false;
        private string _uid = "Client";
        private IDrawing _drawShips;
        private GameOptions _options;
        private readonly GameContext _gameContext = new GameContext();
        private Shoot _shoot;
        private Control _animationControl;
        private List<Image> _rocketImg;
        private List<Image> _bangImg;

        public Game()
        {
            
        }

        public Game(GameOptions options)
        {
            _options = options;
        }

        public void UpdateOptions(GameOptions options)
        {
            _options = options;
        }

        public void SetGraphic(Graphics shipGraphics)
        {
            _drawShips = new Drawing(shipGraphics, _options);
        }

        public void SetAnimationControl(Control animationControl)
        {
            _animationControl = animationControl;
        
        }

        public bool ConnectToService()
        {
            string uri = _options.Uri;
            var address = new Uri("net.tcp://" + uri + "/IGameContract");
            var bind = new NetTcpBinding();
            
            var context = new InstanceContext(new GameContractCallback(this));
            var point = new EndpointAddress(address);
            try
            {
                var factory = new DuplexChannelFactory<IGameContract>(context, bind);
                _contract = factory.CreateChannel(point);
                _isConnected = true;
            }
            
            catch
            {
                _isConnected = false;
            }
            return _isConnected;
        }

        public object GetPlayerUid()
        {
            if (_isConnected)
            {
                int playerCounter = (int)_contract.GetPlayerUid();
                if (playerCounter > 0)
                {
                    _uid += playerCounter.ToString();
                    return _uid;
                }
                
            }
            return new Exception("GetPlayerUid");
        }

        public void InitGame()
        {
            if ( (_isConnected) & (_uid != "Client") )
            {
                int shipsCount = _options.ShipsCount;
                
                _contract.SetToList(_uid);
                _contract.InitGame(_uid, _options.GameOption, _options.GridOptions, _options.MyBattleFieldLocation, shipsCount);
                RocketAnimationInit();
            }
            else
                throw new Exception("InitGame");
        }

        private void RocketAnimationInit()
        {
            _rocketImg = new List<Image>();
            _rocketImg.Add(Image.FromFile(@"C:\Projects\ShipBattle(WCF)\ShipBattle(WCF)_last\ShipBattle(WCF)\ShipBattle(WCF)\photos\rocket.png"));
            _options.RocketOption = new Coordinates(20, 20);
        }
        
        public void CreateShip(Coordinates shipCoord, string shipType, string shipTurning)
        {
            if ( (_isConnected) & (_uid != "Client") )
            {
                _contract.ShipInit(shipType, shipTurning, _uid);
                var shipList = _contract.CreateShip(shipCoord);
                _drawShips.DrawShip(shipList, ShipAction.CreateShip);
                _contract.UpdateContext(ShipAction.CreateShip);
            }
            else
                throw new Exception("CreateShip");
        }

        public void DeleteShip(Coordinates shipCoord)
        { 
            if ( (_isConnected) & (_uid != "Client") )
            {
                var deleteShipList = _contract.DeleteShip(shipCoord);
                _drawShips.DrawShip(deleteShipList, ShipAction.DeleteShip);
                _contract.UpdateContext(ShipAction.DeleteShip);
            }
            else
                throw new Exception("DeleteShip");
            
        }

        public void DrawHitShip(Coordinates hitCoords, bool hit)
        {
            _drawShips.DrawHitShip(hitCoords, hit);

        }

        public void DrawDestroyShip(List<Coordinates> listOfDestroyShip)
        {
            _drawShips.DrawDestroyShip(listOfDestroyShip);
        }

        public void AnimatedShooting(Coordinates start, Coordinates destination, bool reverse = false)
        {
            //string path = @"D:\Projects\ShipBattle\ShipBattle(WCF)_last\ShipBattle(WCF)\ShipBattle(WCF)\photos";
            //string[] fileNames = Directory.GetFiles(path);
            //foreach (var fileName in fileNames)
            //{ 
            //    _imgCollection.Add(Image.FromFile(fileName));
            //}
            string direction = "FromLeftToRight";
            if (!reverse)
            {
                if (_uid == "Client2")
                    direction = "FromRightToLeft";
            }
            else
            {
                if (_uid == "Client1")
                    direction = "FromRightToLeft";
            }
            this._isShootAnimated = true;
            _shoot = new AnimationShootOnBaseControl(_rocketImg, _rocketImg, _animationControl, _options);
            _shoot.MakeShoot(start, destination, direction);
            _shoot.ShootCompleted += () => { this._isShootAnimated = false; };
        }

        private Coordinates SeedRandStartCoord()
        {
            var rand = new Random();
            return _gameContext.ShipCoords[rand.Next(0, _gameContext.ShipCoords.Count - 1)];
        }

        public Coordinates GetRandomCoords()
        { 
            return _options.GameOption / 2 + _options.EnemyBattleFieldLocation;
        }

        public void HitTheShip(Coordinates shipCoord)
        {
            if ( (_isConnected) & (_uid != "Client") )
            {
                bool hit = false;
                bool isDestroy = false;
                Coordinates start = SeedRandStartCoord();
                var hitCoord = _contract.HitTheShip(shipCoord, _uid, out hit, out isDestroy);
                AnimatedShooting(start, shipCoord);
                if (isDestroy == false)
                {
                    //Thread.Sleep(100);
                    //_drawShips.DrawHitShip(hitCoord[0], hit);
                    _gameContext.UpdateOnHitShipCoords(hitCoord[0], hit, false);
                    _contract.UpdateContext(ShipAction.HitTheShip);
                }
                else
                {
                    _drawShips.DrawDestroyShip(hitCoord);
                    foreach (var coords in hitCoord)
                    {
                        _gameContext.UpdateOnShipCoords(coords, ShipAction.DestroyShip, false);
                    }
                    _contract.UpdateContext(ShipAction.DestroyShip);
                }
            }
        }

        private bool _clientReady = false;
        private bool _isShootAnimated;
        public void ReadyGo()
        {
            _contract.ReadyGo(_uid);
        }

        public bool GetReady()
        {
            return _clientReady;
        }

        public void SetReady()
        {
            _clientReady = true;
        }

        public GameContext GetGameContext()
        {
            return _gameContext;
        }

        private void DrawHitShip(Dictionary<bool, List<Coordinates>> dict)
        {
            foreach (var list in dict)
            {
                foreach (var coords in list.Value)
                {
                    _drawShips.DrawHitShip(coords, list.Key);
                }
            }
        }

        public void ReDraw(Rectangle rectangle)
        {
            if (!this._isShootAnimated)
            {
                rectangle = Rectangle.Empty;
            }
            _drawShips.DrawBattleFields(rectangle);
            _drawShips.DrawShip(_gameContext.ShipCoords, ShipAction.CreateShip);
            //DrawHitShip(_gameContext.MyHitCoords);
            //DrawHitShip(_gameContext.EnemyHitCoords);
            //_drawShips.DrawDestroyShip(_gameContext.MyDestroyShipCoords);
            //_drawShips.DrawDestroyShip(_gameContext.EnemyDestroyShipCoords);
        }
    }
}
 