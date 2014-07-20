using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using GameUtils;

namespace ClientsPart
{
    public partial class Form2 : Form
    {
        public Form2(GameOptions options)
        {
            InitializeComponent();
            _options = options;
            BaseOptionsInit();
            SetLBlColors();
        }

        private GameOptions _options;

        private void BaseOptionsInit()
        {
            FieldGridXNumeric.Maximum = 300;
            FieldGridXNumeric.Minimum = 150;
            FieldGridXNumeric.Increment = 25;

            FieldGridYNumeric.Maximum = 300;
            FieldGridYNumeric.Minimum = 150;
            FieldGridYNumeric.Increment = 25;

            BattleGridXNumeric.Maximum = 50;
            BattleGridXNumeric.Minimum = 10;
            BattleGridXNumeric.Increment = 5;

            BattleGridYNumeric.Maximum = 50;
            BattleGridYNumeric.Minimum = 10;
            BattleGridYNumeric.Increment = 5;

            ShipsCountNumeric.Maximum = 15;
            ShipsCountNumeric.Minimum = 1;

            ShipListBox.SetItemChecked(0, true);
            ShipListBox.SetItemChecked(1, true);
            ShipListBox.SetItemChecked(2, true);
            ShipListBox.SetItemChecked(3, true);
            
            // Colors Init
            _options.FieldColor = Color.Black;
            _options.GridColor = Color.White;
            _options.ShipColor = Color.Blue;
            _options.DeleteshipColor = _options.GridColor;
            _options.HitShipColor = Color.Red;
            _options.DestroyShipColor = Color.DarkGray;
        }

        private void SetLBlColors()
        {
            FieldColorlbl.BackColor = _options.FieldColor;
            GridColorlbl.BackColor = _options.GridColor;
            ShipColorlbl.BackColor = _options.ShipColor;
            HitShipColorlbl.BackColor = _options.HitShipColor;
            DestroyShipColorlbl.BackColor = _options.DestroyShipColor;
        }

        private void GetLBlColors()
        {
            _options.FieldColor = FieldColorlbl.BackColor;
            _options.GridColor =  GridColorlbl.BackColor;
            _options.ShipColor = ShipColorlbl.BackColor;
            _options.HitShipColor = HitShipColorlbl.BackColor;
            _options.DestroyShipColor = DestroyShipColorlbl.BackColor;
        }

        private ColorDialog colorDialog = new ColorDialog();
        private void FieldColorlbl_MouseClick(object sender, MouseEventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Type type = sender.GetType();
                PropertyInfo[] propInfo = type.GetProperties();
                propInfo[28].SetValue(sender, colorDialog.Color, null);
            }
        }

        private void OptionsInit()
        {
            _options.Uri = UritxtBox.Text;
            _options.GameOption = new Coordinates((double) FieldGridXNumeric.Value, (double) FieldGridYNumeric.Value);
            _options.GridOptions = new Coordinates((double) BattleGridXNumeric.Value, (double) BattleGridYNumeric.Value);
            _options.ShipsCount = (int) ShipsCountNumeric.Value;
            _options.ActualShips = new List<string>();
            _options.MusicEnable = MusicCheckBox.Checked;
            for (int i = 0; i < ShipListBox.Items.Count; i++)
            {
                if (ShipListBox.GetItemChecked(i))
                    _options.ActualShips.Add((string) ShipListBox.Items[i]);
            }
            GetLBlColors();
            _options.RocketOption = new Coordinates(20, 20);
        }
        
        private void OKButton_Click(object sender, EventArgs e)
        {
            if (UritxtBox.Text != string.Empty)
            {
                OptionsInit();
                this.Close();
            }
            else
            {
                MessageBox.Show("Write URI address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
