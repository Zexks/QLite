using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLite.Engines;
using QLite.Forms;
using QLite.Plugins;
using QLite.Collections;
using QLite.Structures;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace QLite
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
            QMain.SetCommandLineArgs();
            QMain.SetDefaults();
            Application.Run(new frmMain());
        }
    }

    public class Global
    {
        public static PluginServices PluginServices = new PluginServices();
        public static Defaults Defaults = new Defaults();
        public static List<QProcess> Processes = new List<QProcess>();

        [Import(typeof(IPlugin))]
        public static CompositionContainer Plugins;

        public Global() { }

    }
}
