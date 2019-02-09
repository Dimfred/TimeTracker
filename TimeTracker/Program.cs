﻿using System;
using System.Windows.Forms;

namespace TimeTracker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TimeTrackerController Controller = new TimeTrackerController();
            TimeTrackerView View = new TimeTrackerView(Controller);

            Application.Run(View);
        }
    }
}
