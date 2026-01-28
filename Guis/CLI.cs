using AveBusControllerDLL;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace standalone_gui_for_avebus_controller.Guis
{
    internal class CLI
    {
        byte UPPER_BOUND = 0;
        byte LOWER_BOUND = 2;

        public CLI(string port)
        {
            initAveBusConnection(port);
        }

        public void initAveBusConnection(string port)
        {

            if (!AveBusController.getAvailablePorts().Contains(port))
            {
                Console.WriteLine("Port [" + port + "] is not available. Exiting.");
                return;
            }

            AveBusController.configureSerialPort(
                port,
                4800,
                Parity.Odd,
                8,
                StopBits.One,
                Handshake.None
            );

            AveBusController.openSerialPort();
            Console.WriteLine("Successfully estabilished connection with AveBus.\n");

        }

        public void execute()
        {

            // menu initialization
            ConsoleKey pressedKey;
            byte currentSelection = 0; // initial cursor position = 0

            Console.Clear();
            printMenu(currentSelection);


            // menu 
            while (true)
            {
                Console.Write(".");

                // PEEK -> evita il blocco
                if (Console.KeyAvailable)
                {
                    pressedKey = Console.ReadKey(true).Key; // istruzione bloccante

                    // increase or decrease current selection
                    if (pressedKey.Equals(ConsoleKey.UpArrow) && currentSelection > UPPER_BOUND)
                    {
                        Console.Clear();

                        currentSelection--;
                        printMenu(currentSelection);
                    }

                    if (pressedKey.Equals(ConsoleKey.DownArrow) && currentSelection < LOWER_BOUND)
                    {
                        Console.Clear();

                        currentSelection++;
                        printMenu(currentSelection);
                    }

                    // sends command on avebus
                    if (pressedKey.Equals(ConsoleKey.Enter))
                    {
                        Console.Clear();
                        performActionOnAveBus(currentSelection);

                    }

                    if (pressedKey.Equals(ConsoleKey.Escape))
                    {
                        Console.Clear();
                        Console.WriteLine("Exiting...");
                        return;

                    }

                } 
                else
                {
                    Thread.Sleep(50);
                }


            }

        }

        private void performActionOnAveBus(byte currentSelection)
        {
            switch (currentSelection)
            {
                case 0:
                    AveBusController.changeLight1Status();
                    break;

                case 1:
                    AveBusController.turnOnLight_1();
                    break;

                case 2:
                    AveBusController.turnOffLight_1();
                    break;
                default:
                    Console.WriteLine("how did you even press that");
                    return;
            }
        }

        private void printMenu(byte currentSelection)
        {

            if (currentSelection == 0)
            {
                Console.WriteLine("AveBusManager:");
                Console.WriteLine("+-------------------------+");
                Console.WriteLine("| > CHANGE LIGHT 1 STATUS |");
                Console.WriteLine("| TURN ON  LIGHT 1        |");
                Console.WriteLine("| TURN OFF LIGHT 1        |");
                Console.WriteLine("+-------------------------+");
            }

            if (currentSelection == 1)
            {
                Console.WriteLine("AveBusManager:");
                Console.WriteLine("+-------------------------+");
                Console.WriteLine("| CHANGE LIGHT 1 STATUS   |");
                Console.WriteLine("| > TURN ON  LIGHT 1      |");
                Console.WriteLine("| TURN OFF LIGHT 1        |");
                Console.WriteLine("+-------------------------+");
            }

            if (currentSelection == 2)
            {
                Console.WriteLine("AveBusManager:");
                Console.WriteLine("+-------------------------+");
                Console.WriteLine("| CHANGE LIGHT 1 STATUS   |");
                Console.WriteLine("| TURN ON  LIGHT 1        |");
                Console.WriteLine("| > TURN OFF LIGHT 1      |");
                Console.WriteLine("+-------------------------+");
            }

        }
    }
}
