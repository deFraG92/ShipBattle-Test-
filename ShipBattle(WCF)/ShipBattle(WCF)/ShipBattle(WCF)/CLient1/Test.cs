#region using

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using GameUtils;
using Shooting;
using System.IO;
#endregion


namespace ClientsPart
{

    public sealed partial class UserInterface : Form
    {
        public UserInterface()
        {
            InitializeComponent();
            this.myEvent += new MyEvent(UserInterface_myEvent);
        }

        private Shoot _shoot;
        private List<Image> _imgCollection;
        private void button1_Click(object sender, EventArgs e)
        {
            GameOptions options = new GameOptions();
            options.RocketOption = new Coordinates(30, 30);
            _imgCollection = new List<Image>();
            string path = @"C:\Projects\ShipBattle(WCF)_last\ShipBattle(WCF)\ShipBattle(WCF)\photos\bang\";
            string[] fileNames = Directory.GetFiles(path);
            foreach (var fileName in fileNames)
            { 
                _imgCollection.Add(Image.FromFile(fileName));
            }
            options.MyBattleFieldLocation = new Coordinates(100, 200);
            options.EnemyBattleFieldLocation = new Coordinates(450, 200);
            options.GridOptions = new Coordinates(20, 20);
            var controlCollection = new List<Control>() {  panel1, panel2, panel3 };
            _shoot = new AnimationShootOnSomeControls(_imgCollection, _imgCollection, controlCollection, this, options);
            _shoot.MakeShoot(new Coordinates(25, 175), new Coordinates(175, 175), "Client2");
        }


        int counter = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            var arg = new MyEventArgs();
            arg.str = counter.ToString();
            counter++;
            myEvent(sender, arg);
        }



        public delegate void MyEvent(object sender, MyEventArgs e);
        public event MyEvent myEvent;
        private void UserInterface_Load(object sender, EventArgs e)
        {
            
        }

        void UserInterface_myEvent(object sender, MyEventArgs e)
        {
            MessageBox.Show(e.str);
        }




    }

    public class MyEventArgs : EventArgs
    {

        public string str { get; set;  }
    
    }


    public class Test
    {
        Thread _thread;
        TextBox _text;

        public Test(TextBox text)
        {
            _text = text;
            _thread = new Thread(SetTestText);
            _thread.Start();
        }

        private void SetTestText()
        {
            for (int i = 0; i < 5; i++)
            {
                SetText("This is Test Thread!", i);
                Thread.Sleep(1000);
            }
        }

        private delegate void SetTextCallBack(string message, int i);
        private void SetText(string message, int i)
        {
            if (_text.InvokeRequired)
            {
                var callBack = new SetTextCallBack(SetText);
                _text.Invoke(callBack, new object[] { message, i });
            }
            else
                _text.Text += i.ToString() + " " + message + Environment.NewLine;
        }






    
    }
}