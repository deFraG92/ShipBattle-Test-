using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Graphic;
using Microsoft.DirectX.AudioVideoPlayback;
using System.Web;


namespace ClientsPart
{
    public partial class Form1 : Form
    {
        public Form1(GameOptions options)
        {
            InitializeComponent();
            _controller = new GameController(this, this, options);
            BaseGameSettingsInit();
        }

        private readonly GameController _controller;
        private string _shipType;
        private string _shipTurn;
        private Cursor _cursor;
        private static int _counter = 0;
        

        private void BaseGameSettingsInit()
        {
            _controller.GameConnect();
            _controller.SetTypeCollection(ShipTypeCMB);
            _controller.SetTurnCollection(ShipTurnCMB);
            _controller.InitGame();
        }
        
        //private void InitGameplay()
        //{
        //    Bitmap bitmap = new Bitmap(@"C:\Projects\ShipBattle(WCF)_last\ShipBattle(WCF)\ShipBattle(WCF)\photos\bomb.png");
        //    IntPtr intPtr = bitmap.GetHicon();
        //    _cursor = new Cursor(intPtr);
        //}

        
        private void ReadyBut_Click(object sender, EventArgs e)
        {
            if (_controller.ShipChecker())
            {
                this.MouseClick -= Field_MouseClick;
                this.MouseClick += new MouseEventHandler(Field_Hit_MouseClick);
                ReadyBut.Enabled = false;
                _controller.ReadyGo();
                ShipTurnCMB.Enabled = false;
                ShipTypeCMB.Enabled = false;
            }
            //var graph = this.CreateGraphics();
            //var rect1 = new Rectangle(100, 100, 50, 50);
            //var rect2 = new Rectangle(100, 100, 50, 50);
            //graph.DrawRectangle(new Pen(Color.Red, 2), rect1);
            //graph.DrawRectangle(new Pen(Color.Transparent, 2), rect2);
            //var rect3 = Rectangle.Intersect(rect1, rect2).;
            //rect3.Location = new Point(200, 200);
            //graph.DrawRectangle(new Pen(Color.Red, 2), rect3);
            

        }


        private void Field_Paint(object sender, PaintEventArgs e)
        {
            _controller.Redraw(e.ClipRectangle);
            //if (e.ClipRectangle.Location.X != 0)
            //{
            //    _counter ++;
            //    textBox1.Text += _counter + ") " + "( " + e.ClipRectangle.Location.X.ToString() + ";" +
            //                 e.ClipRectangle.Location.Y.ToString() +
            //                 " ); ";
            //}
        }
        


        private void Field_Hit_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void ShipType_SelectedValueChanged(object sender, EventArgs e)
        {
            _shipType = ShipTypeCMB.SelectedItem.ToString();
        }

        private void ShipTurn_SelectedValueChanged(object sender, EventArgs e)
        {
            _shipTurn = ShipTurnCMB.SelectedItem.ToString();
        }

        private void Field_MouseClick(object sender, MouseEventArgs e)
        {
            _controller.CreateShipOnClick(_shipType, _shipTurn, e);
        }

        private void Field_Hit_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = String.Empty;
            if (!_controller.ClientsReady())
            {
                _controller.HitShipOnClick(e);
                _counter = 0;
            }
        }

        private void Field_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {

            //counter++;
            //double X = Math.Truncate(e.X / _GridOptions.X) * _GridOptions.X;
            //double Y = Math.Truncate(e.Y / _GridOptions.Y) * _GridOptions.Y;
            //if (counter == 1)
            //{
            //    if (this._shipType == null)
            //        this._shipType = this.comboBox1.Items[this.comboBox1.SelectedIndex].ToString();
            //    if (this._shipTurning == null)
            //        this._shipTurning = this.comboBox2.Items[this.comboBox2.SelectedIndex].ToString();
            //    this._GameContract.ShipInit(this._shipType, this._shipTurning, UID);
            //    this._GameContract.GetShipImage();
            //}
          
        }

        private void Field_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = _cursor;
        }

        private void Field_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

    }


    
}
