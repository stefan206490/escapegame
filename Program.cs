using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace escapegame
{
    class Program
    {
        public static int currentRoom = 1;
        public static bool inGame = false;
        public static string asciiArt = null;
        public static string filePath = @"../../ascii/";
        public static bool[] roomVisited = new bool[6];
        public static int tickRate = 50;

        // kamer verhalen in een string array declareren
        public static string[] kamerVerhaal = new string[6];
        
        static void Main(string[] args)
        {
            kamerVerhaalFunctie();
            Program.inGame = mainMenu();
            while (Program.inGame == true)
            {
                gameLoop(Program.currentRoom);
            }
        }

        static void kamerVerhaalFunctie()
        {
            // initializes the strings for the chamber's story
            Program.kamerVerhaal[0] = "Dit is het verhaal van kamer 1.";
            Program.kamerVerhaal[1] = "Dit is het verhaal van kamer 2.";
            Program.kamerVerhaal[2] = "Dit is het verhaal van kamer 3.";
            Program.kamerVerhaal[3] = "Dit is het verhaal van kamer 4.";
            Program.kamerVerhaal[4] = "Dit is het verhaal van kamer 5.";
            Program.kamerVerhaal[5] = "Dit is het verhaal van kamer 6.";
        }

        static bool mainMenu()
        {
            // will show welcome screen string with 100 ms pause between chars
            string mainMenuString = "Welkom bij de escape game!";
            foreach (char c in mainMenuString)
            {
                System.Console.Write(c);
                Thread.Sleep(tickRate);
            }
            System.Console.Write("\n");

            // prints out the ascii art for the main menu
            Program.asciiArt = "mainmenu.txt";
            string mainMenu = File.ReadAllText(filePath + asciiArt);
            Console.WriteLine(mainMenu);

            // players gets to choose to start a new game or exit
            Console.WriteLine("Schrijf [new]/[1] voor nieuwe game, schrijf [exit]/[2] om de game af te sluiten");
            string menuChoice = Console.ReadLine();
            if (menuChoice == "new" || menuChoice == "1")
            {
                // sets all values in roomVisited array to false
                for (int i = 0; i < roomVisited.Length; i++)
                {
                    Program.roomVisited[i] = false;
                }

                // returns true to commence game
                return true;
            }
            else if (menuChoice == "exit" || menuChoice == "2")
            {
                // exits the game
                Console.WriteLine("Game wordt afgesloten...");
                Console.ReadLine();
                return false;
            }
            else
            {
                return false;
            }
        }

        static void gameLoop(int roomNumber)
        {
            switch (Program.currentRoom)
            {
                case 1:
                    room1_hall();
                    break;
                case 2:
                    Console.WriteLine("kamer 2");
                    Console.ReadLine();
                    break;
                case 3:
                    Console.WriteLine("kamer 3");
                    Console.ReadLine();
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                default:
                    Console.WriteLine("Game wordt afgesloten");
                    Console.ReadLine();
                    Program.inGame = false;
                    break;
            }
        }

        static int roomChoiceMenu(string choice1, string choice2)
        {
            string menuChoice = Console.ReadLine();
            if (menuChoice == choice1)
            {
                return 1;
            }
            else if (menuChoice == choice2)
            {
                return 2;
            }
            else if (menuChoice == "exit")
            {
                Program.currentRoom = 0;
                return 0;
            }
            else
            {
                Console.WriteLine("Ongeldige keuze! Probeer opnieuw");
                Console.ReadLine();
                return 0;
            }
        }

        static void printRoomStory(string story)
        {
            foreach (char c in story)
            {
                System.Console.Write(c);
                Thread.Sleep(tickRate);
            }
        }

        static void room1_hall()
        {
            // clears the console first
            Console.Clear();

            // prints out the ascii art
            Program.asciiArt = "Hal.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);

            // prints out room story
            printRoomStory(Program.kamerVerhaal[Program.currentRoom -1] + "\n");

            Console.WriteLine("Wil je naar de keuken of naar de woonkamer? [keuken]/[woonkamer] of [exit]");

            string choiceKeuken = "keuken";
            string choiceWoonkamer = "woonkamer";

            int choice = 0;
            choice = roomChoiceMenu(choiceKeuken, choiceWoonkamer);
            if (choice == 1)
            {
                Console.WriteLine("Je gaat naar de {0}", choiceKeuken);
                Program.currentRoom = 2;
            }
            else if (choice == 2)
            {
                Console.WriteLine("Je gaat naar de {0}", choiceWoonkamer);
                Program.currentRoom = 3;
            }
        }
    }
}
