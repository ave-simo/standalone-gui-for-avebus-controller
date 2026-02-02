using AveBusControllerDLL;
using System;
using System.Linq;
using System.IO.Ports;
using System.Windows.Forms;
using standalone_gui_for_avebus_controller.Guis;

namespace standalone_gui_for_avebus_controller
{
    internal static class Program
    {
        [STAThread] //thread principale
        static void Main(string[] args)
        {


            if (args.Length > 0 && args.Contains("--cli"))
            {
                // starts CLI
                cliStartProcedure();
            }
            else
            {
                // starts GUI
                guiStartProcedure();
            }

        }

        private static void cliStartProcedure()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CLI cli = new CLI("COM3");
            cli.execute();

        }

        private static void guiStartProcedure()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }


    }
}
