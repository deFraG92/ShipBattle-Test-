using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GameUtils;
using System.Windows.Forms;

namespace ClientsPart
{
    public class GameController
    {
        private readonly GameOptions _options;
        private readonly IGame _game;
        private readonly Control _baseControl;
        private readonly Control _animationControl;
        private bool _isConnected;
        private string _uid;
        private int _shipsCounter = 0;
        private List<string> _turnCollection;
        private bool _clientReady;
        public GameController(Control baseControl, Control animationControl, GameOptions options)
        {
            _baseControl = baseControl;
            _animationControl = animationControl;
            _options = options;
            _game = new Game(_options);
        }

        public void GameConnect()
        {
            try
            {
                _isConnected = _game.ConnectToService();
                _uid = (string)_game.GetPlayerUid();
            }
            catch(Exception exp)
            {
                _isConnected = false;

            }
        }

        public void SetTypeCollection(Control control)
        {
            if (_options.ActualShips.Count > 0)
            {
                foreach (var item in _options.ActualShips)
                {
                    ((ComboBox)control).Items.Add(item);
                }

            }
            else
            {
                ((ComboBox)control).Items.Add("GreatShip");
            }
        }

        private void SetBattleFieldLocation()
        {
            if (_uid == "Client1")
            {
                _options.MyBattleFieldLocation = new Coordinates(50, 150);
                _options.EnemyBattleFieldLocation = new Coordinates(500, 150);
            }
            else if (_uid == "Client2")
            {
                _options.MyBattleFieldLocation = new Coordinates(500, 150);
                _options.EnemyBattleFieldLocation = new Coordinates(50, 150);
            }
            _game.UpdateOptions(_options);
        }

        public void Redraw(Rectangle rectangle)
        {
            _game.ReDraw(rectangle);
        }

        public void SetTurnCollection(Control control)
        {
            _turnCollection = new List<string>();
            foreach (var elem in ((ComboBox)control).Items)
            {
                _turnCollection.Add(elem.ToString());
            }
        }

        public void InitGame()
        {
            SetBattleFieldLocation();
            _game.InitGame();
            var graphic = _baseControl.CreateGraphics();
            _game.SetGraphic(graphic);
            _game.SetAnimationControl(_animationControl);
        }
        
        public void CreateShipOnClick(string shipType, string shipTurning, MouseEventArgs e)
        {
            _shipsCounter ++;
            if (_shipsCounter <= _options.ShipsCount)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (shipType == null)
                        shipType = _options.ActualShips[0];
                    if (shipTurning == null)
                        shipTurning = _turnCollection[0];
                    _game.CreateShip(new Coordinates(e.X, e.Y), shipType, shipTurning);
                }

                else if (e.Button == MouseButtons.Right)
                {
                    if (_shipsCounter >= 1)
                    {
                        _game.DeleteShip(new Coordinates(e.X, e.Y));
                        _shipsCounter--;
                        
                    }
                }
            }
        }

        public void HitShipOnClick(MouseEventArgs e)
        {
            var hitCoord = new Coordinates(e.X, e.Y);
            if ( (hitCoord >= _options.EnemyBattleFieldLocation) & (hitCoord <= _options.EnemyBattleFieldLocation + _options.GameOption) )
            {
                _game.HitTheShip(hitCoord);
            }
        }

        public bool ShipChecker()
        {
            if (_shipsCounter != _options.ShipsCount)
            {
                if (_shipsCounter == 0)
                {
                    MessageBox.Show("Though you must create a one ship!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                var result = MessageBox.Show("You can put more ships! Continue?", "Info", MessageBoxButtons.OKCancel,
                                         MessageBoxIcon.Question);
                if (result != DialogResult.OK)
                {
                    return false;
                }
            }
            return true;
        }

        public void ReadyGo()
        {
            _clientReady = true;
            _game.ReadyGo();
        }

        public bool ClientsReady()
        {
            return _clientReady && _game.GetReady();
        }

    }
}
