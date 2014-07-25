using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GameUtils;
using System.Threading;
using Shooting;
using GameOptions = ClientsPart.GameOptions;
using Timer = System.Threading.Timer;
using System.Windows.Forms;

namespace ClientWithForm.Shooting
{
    public abstract class Shoot
    {
        protected Rocket Rocket;
        protected Timer Timer;
        protected string Direction = "FromLeftToRight";
        public Action ShootCompleted;
        public abstract void MakeShoot(Coordinates startShoot, Coordinates destination, string direction = "FromLeftToRight");
    }

    public class AnimationShootOnBaseControl : Shoot
    {
        private readonly Control _baseControl;
        private List<Coordinates> _trajectoryCoordsCollection;
        private readonly GameOptions _options;
        private int _counter = 0;
        private System.Drawing.Graphics _graphic;
        public AnimationShootOnBaseControl(IEnumerable<Image> rocketImgCollection,
                          IEnumerable<Image> bangImgCollection,
                          Control baseControl,
                          GameOptions options)
        {
            Rocket = new RocketPrototype1(rocketImgCollection, bangImgCollection, options.RocketOption);
            _baseControl = baseControl;
            _options = options;
        }

        public override void MakeShoot(Coordinates startShoot, Coordinates destination, string direction = "FromLeftToRight")
        {
            _trajectoryCoordsCollection = Rocket.InitTrajectory(startShoot, destination, direction);
            //DrawShoot();
            DrawAnimationShoot();
        }

        private void DrawAnimationShoot()
        {
            _graphic = _baseControl.CreateGraphics();
            Timer = new Timer(new TimerCallback(Shoot_OnTick), null, 0, 100);
        }

        private void FireShootCompleted()
        {
            var handler = this.ShootCompleted;
            if (handler != null)
            {
                handler();
            }
        }

        private void Shoot_OnTick(object state)
        {
            if (_counter < _trajectoryCoordsCollection.Count - 1)
            {
                _baseControl.Invalidate(new Rectangle((int)_trajectoryCoordsCollection[_counter].X, (int)_trajectoryCoordsCollection[_counter].Y, (int)_options.RocketOption.X, (int)_options.RocketOption.Y), false);
                _counter++;
                _graphic.DrawImage(Rocket.Fly(_trajectoryCoordsCollection[_counter]), (int)_trajectoryCoordsCollection[_counter].X, (int)_trajectoryCoordsCollection[_counter].Y);
            }
            else
            {
                //_baseControl.Invalidate(new Rectangle((int)_trajectoryCoordsCollection[_counter].X, (int)_trajectoryCoordsCollection[_counter].Y, (int)_options.RocketOption.X, (int)_options.RocketOption.Y), false);
                Timer.Dispose();
                FireShootCompleted();
            }
        
        }
        
        private void DrawShoot()
        {
            var graphic = _baseControl.CreateGraphics();
            foreach (var coord in _trajectoryCoordsCollection)
            {
                graphic.DrawImage(Rocket.Fly(coord), (int)coord.X, (int)coord.Y);
            }
            
        }
    }
    
    public class AnimationShootOnSomeControls : Shoot // Not finished realization
    {
        private List<Coordinates> _trajectoryCoordsCollection;
        private readonly GameOptions _options;
        private List<Control> _controlCollection;
        private readonly Control _baseControl;
        private List<int> _trajectoryPartitionCollection;
        private int _counter = 0;
        public AnimationShootOnSomeControls(IEnumerable<Image> rocketImgCollection,
                          IEnumerable<Image> bangImgCollection, 
                          IEnumerable<Control> controlCollection, 
                          Control baseControl,
                          GameOptions options)
        {
            
            Rocket = new RocketPrototype1(rocketImgCollection, bangImgCollection, options.RocketOption);
            _controlCollection = new List<Control>(controlCollection);
            _baseControl = baseControl;
            _options = options;
        }

        public override void MakeShoot(Coordinates startShoot, Coordinates destination, string uid)
        {
            if (uid == "Client2")
            {
                Direction = "FromRightToLeft";
            }
            var newCoords = ToFormCoordTransForm(startShoot, destination);
            _trajectoryCoordsCollection = Rocket.InitTrajectory(newCoords[0], newCoords[1], Direction);
            ChooseControl();
            Draw();
            //_timer = new Timer(new TimerCallback(DrawShoot), null, 0, 100);
        }

        private void DrawShoot(object target)
        {
            //if (_counter == _trajectoryCoordsCollection.Count)
            //{
            //    _timer.Dispose();
            //    _counter = 0;
            //    _timer = new Timer(new TimerCallback(DrawBang), null, 0, 100);
            //    return;
            //}
        }

        private void DrawBang(object target)
        {
            if (_counter == 17)
            {
                Timer.Dispose();
                return;
            }
            var bangCoords = _trajectoryCoordsCollection[_trajectoryCoordsCollection.Count - 1];
            //_graphCollection[0].DrawImage(Rocket.BangBang(_counter), (int)bangCoords.X, (int)bangCoords.Y);
            Thread.Sleep(100);
           //_drawForm.Invalidate(new Rectangle((int)bangCoords.X, (int)bangCoords.Y, 20, 20));
           _counter++;
        }

        private Coordinates[] ToFormCoordTransForm(Coordinates startCoords, Coordinates destination)
        {
            var newStartCoord = new Coordinates(startCoords.X + _options.MyBattleFieldLocation.X + _options.GridOptions.X / 2,
                                                startCoords.Y + _options.MyBattleFieldLocation.Y + _options.GridOptions.Y / 2);
            var newDestination = new Coordinates(destination.X + _options.EnemyBattleFieldLocation.X + _options.GridOptions.X / 2,
                                                 destination.Y + _options.EnemyBattleFieldLocation.Y + _options.GridOptions.Y / 2);
            return new Coordinates[] { newStartCoord, newDestination };
        }

        private bool IsOwner(Control control, Coordinates coord)
        {
            var location = new Coordinates();
            var size = new Coordinates();
            location.X = (double)control.Location.X;
            location.Y = (double)control.Location.Y;
            size.X = (double)control.Size.Width;
            size.Y = (double)control.Size.Height;
            return ((coord >= location) & (coord <= location + size));
        }

        private void ChooseControl()
        {
            _trajectoryPartitionCollection = new List<int>();
            var orderControlCollection = new List<Control>();
            var counter = 0;
            var bufCounter = 0;
            var formCounter = 0;
            var bufControl = new Control();
            var need = false;
            var isOwner = false;
            var isForm = false;
            foreach (var coord in _trajectoryCoordsCollection)
            {
                foreach (var control in _controlCollection)
                {
                    if (IsOwner(control, coord))
                    {
                        bufCounter++;
                        isOwner = true;
                        if (bufCounter == 1)
                            bufControl = control;
                        if (bufControl == control)
                            need = true;
                        else if (!isForm)
                        {
                            orderControlCollection.Add(bufControl);
                            _trajectoryPartitionCollection.Add(counter);
                        }
                        bufControl = control;
                    }
                }
                if (!isOwner)
                {
                    formCounter++;
                    if (formCounter == 1)
                    {
                        orderControlCollection.Add(bufControl);
                        _trajectoryPartitionCollection.Add(counter);
                        counter = 0;
                    }
                    bufControl = _baseControl;
                    isForm = true;
                    counter++;
                }
                else
                {
                    if (isForm)
                    {
                        orderControlCollection.Add(_baseControl);
                        _trajectoryPartitionCollection.Add(counter);
                        isForm = false;

                        formCounter = 0;
                    }
                    if (need)
                        counter++;
                    else
                        counter = 1;
                }
                isOwner = false;
                need = false;
            }
            orderControlCollection.Add(bufControl);
            _trajectoryPartitionCollection.Add(counter);
            _controlCollection = new List<Control>(orderControlCollection);
        }

        private void Draw()
        {
            int k = 0;
            int j = 0;
            for (int i = 0; i < _controlCollection.Count; i++)
            {
                var graphic = _controlCollection[i].CreateGraphics();
                for (j = 0; j < _trajectoryPartitionCollection[i]; j++)
                {
                    int deltaX = _controlCollection[i].Location.X;
                    int deltaY = _controlCollection[i].Location.Y;
                    if (_controlCollection[i] == _baseControl)
                    {
                        deltaX = 0;
                        deltaY = 0;
                    }
                    graphic.DrawRectangle(new Pen(Color.Red, 2), (int)_trajectoryCoordsCollection[j + k].X - deltaX,
                                          (int)_trajectoryCoordsCollection[j + k].Y - deltaY, 5, 5);

                }
                k += j;
            }
        }
    }
}
