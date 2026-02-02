using AveBusControllerDLL;
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace standalone_gui_for_avebus_controller.Guis
{
    internal class CLI
    {
        byte UPPER_BOUND = 0;
        byte LOWER_BOUND = 4;
        byte currentSelection;
        bool l_1_s = false; // light_1_status
        bool l_2_s = false; // light_2_status
        bool read  = false; // read bus true/false

        public CLI(string port)
        {
            initAveBusConnection(port);
            AveBusController.registerEventHandler(avebusEventHandler);
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

        void avebusEventHandler(string key, string value)
        {
            this.handleLightStatusUpdate(key, value);
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

                case 3:
                    AveBusController.turnOnLight_2();
                    break;

                case 4:
                    AveBusController.turnOffLight_2();
                    break;

                default:
                    Console.WriteLine("how did you even press that");
                    return;
            }

        }

        public void handleLightStatusUpdate(string eventKey, string eventValue)
        {
            switch (eventKey)
            {
                case "LIGHT_STATUS":
                    if (eventValue.Equals("TURN_ON_LIGHT_1_FRAME_COMMAND")) l_1_s = true;
                    if (eventValue.Equals("TURN_OFF_LIGHT_1_FRAME_COMMAND")) l_1_s = false;
                    if (eventValue.Equals("TURN_ON_LIGHT_2_FRAME_COMMAND")) l_2_s = true;
                    if (eventValue.Equals("TURN_OFF_LIGHT_2_FRAME_COMMAND")) l_2_s = false;
                    if (eventValue.Equals("CHANGE_LIGHT_STATUS_FRAME_COMMAND")) { }

                    drawGui(currentSelection);

                    break;

                default:
                    break;
            }
        }

        // Graphical methods ================================================================================

        public void execute()
        {

            // menu initialization
            ConsoleKey pressedKey;
            currentSelection = 0; // initial cursor position = 0

            Console.Clear();
            drawGui(currentSelection);


            // menu 
            while (true)
            {

                // PEEK -> evita il blocco
                if (Console.KeyAvailable)
                {
                    pressedKey = Console.ReadKey(true).Key; // istruzione bloccante

                    // increase or decrease current selection
                    if (pressedKey.Equals(ConsoleKey.UpArrow) && currentSelection > UPPER_BOUND)
                    {
                        Console.Clear();
                        currentSelection--;
                        drawGui(currentSelection);
                    }

                    if (pressedKey.Equals(ConsoleKey.DownArrow) && currentSelection < LOWER_BOUND)
                    {
                        Console.Clear();
                        currentSelection++;
                        drawGui(currentSelection);
                    }

                    if (pressedKey.Equals(ConsoleKey.Enter))
                    {
                        Console.Clear();
                        performActionOnAveBus(currentSelection);
                        drawGui(currentSelection);

                    }

                    if (pressedKey.Equals(ConsoleKey.R))
                    {
                        Console.Clear();
                        if (!read)
                        {
                            AveBusController.startReadingBus();
                            read = true;
                        }
                        else
                        {
                            AveBusController.stopReadingBus();
                            read = false;
                        }

                        drawGui(currentSelection);

                    }

                    if (pressedKey.Equals(ConsoleKey.Escape))
                    {
                        Console.Clear();
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);

                    }

                } 
                else
                {
                    Thread.Sleep(50);
                }


            }

        }
        private void drawGui(byte currentSelection)
        {
            Console.Clear();

            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║            AVE BUS MANAGER           ║");
            Console.WriteLine("╠══════════════════════════════════════╣");

            if (currentSelection == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("║ ▶ CHANGE LIGHT 1 STATUS              ║");
                Console.ResetColor();
            }
            else
                Console.WriteLine("║   CHANGE LIGHT 1 STATUS              ║");

            if (currentSelection == 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("║ ▶ TURN ON  LIGHT 1                   ║");
                Console.ResetColor();
            }
            else
                Console.WriteLine("║   TURN ON  LIGHT 1                   ║");

            if (currentSelection == 2)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("║ ▶ TURN OFF LIGHT 1                   ║");
                Console.ResetColor();
            }
            else
                Console.WriteLine("║   TURN OFF LIGHT 1                   ║");

            if (currentSelection == 3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("║ ▶ TURN ON  LIGHT 2                   ║");
                Console.ResetColor();
            }
            else
                Console.WriteLine("║   TURN ON  LIGHT 2                   ║");

            if (currentSelection == 4)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("║ ▶ TURN OFF LIGHT 2                   ║");
                Console.ResetColor();
            }
            else
                Console.WriteLine("║   TURN OFF LIGHT 2                   ║");

            Console.WriteLine("╠══════════════════════════════════════╣");

            Console.WriteLine($"║ Read Bus status: {this.read,-18}  ║");
            Console.WriteLine($"║ Light 1  status: {this.l_1_s,-18}  ║");
            Console.WriteLine($"║ Light 2  status: {this.l_2_s,-18}  ║");

            Console.WriteLine("╚══════════════════════════════════════╝");


            drawHelpWindow();
        }
        private void drawHelpWindow()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║              INFORMATION             ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║                                      ║");
            Console.WriteLine("║ ↑  Press UP to move selection        ║");
            Console.WriteLine("║ ↓  Press DOWN to move selection      ║");
            Console.WriteLine("║ ENTER : Execute selected command     ║");
            Console.WriteLine("║ 'R'   : Read data from Ave Bus       ║");
            Console.WriteLine("║ ESC   : Exit program                 ║");
            Console.WriteLine("║                                      ║");
            Console.WriteLine("╚══════════════════════════════════════╝");

            Console.ResetColor();
        }

    }
}
