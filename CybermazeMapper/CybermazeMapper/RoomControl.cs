using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CybermazeMapper
{
    public partial class RoomControl : UserControl
    {
        private string index;

        public event EventHandler<ExitClickedArgs> ExitClicked;

        public RoomControl()
        {
            InitializeComponent();
        }

        public int Row { get; set; }
        public int Col { get; set; }
        public bool Empty { get; internal set; }
        public string Index { 
            get => index;
            internal set
            {
                index = value;
                labelNumber.Text = index;
            }
        }

        private void buttonNorth_Click(object sender, EventArgs e)
        {
            ExitClicked?.Invoke(this, new ExitClickedArgs() { Direction = ExitClickedArgs.Directions.North });
        }

        internal void SetConnection(ExitClickedArgs.Directions p)
        {
            Button b;

            switch (p)
            {
                case ExitClickedArgs.Directions.North:
                    b = buttonNorth;
                    break;
                case ExitClickedArgs.Directions.South:
                    b = buttonSouth;
                    break;
                case ExitClickedArgs.Directions.East:
                    b = buttonEast;
                    break;
                case ExitClickedArgs.Directions.West:
                    b = buttonWest;
                    break;
                default:
                    return;
            }

            b.BackColor = Color.Blue;

        }


        private void buttonEast_Click(object sender, EventArgs e)
        {
            ExitClicked?.Invoke(this, new ExitClickedArgs() { Direction = ExitClickedArgs.Directions.East });
        }

        private void buttonSouth_Click(object sender, EventArgs e)
        {
            ExitClicked?.Invoke(this, new ExitClickedArgs() { Direction = ExitClickedArgs.Directions.South });
        }

        private void buttonWest_Click(object sender, EventArgs e)
        {
            ExitClicked?.Invoke(this, new ExitClickedArgs() { Direction = ExitClickedArgs.Directions.West });
        }
    }


    public class ExitClickedArgs
    {
        public enum Directions : int
        {
            North = -1,
            South = 1,
            East = 2,
            West = -2
        }

        public Directions Direction { get; set; }
    }
}
