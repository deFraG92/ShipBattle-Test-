using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClientsPart
{
    static class Client1
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var options = new GameOptions();
            Application.Run(new Form2(options));
            Application.Run(new Form1(options));
            //Application.Run(new UserInterface());
            
        }
    }
}
