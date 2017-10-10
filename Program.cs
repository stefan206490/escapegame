using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace escapegame
{
    class Program
    {
        public static int currentRoom = 1;
        public static bool inGame = false;
        public static string asciiArt = null;
        public static string filePath = @"../../ascii/";

        static void Main(string[] args)
        {
            Program.inGame = mainMenu();
            while (Program.inGame == true)
            {
                gameLoop(Program.currentRoom);
            }
        }

        static bool mainMenu()
        {
            Console.WriteLine("Welkom bij Escape Game!");
            Program.asciiArt = "mainmenu.txt";
            string mainMenu = File.ReadAllText(filePath + asciiArt);
            Console.WriteLine(mainMenu);
            string menuChoice = Console.ReadLine();
            if (menuChoice == "new" || menuChoice == "New")
            {
                return true;
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
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                default:
                    Console.WriteLine("Default");
                    break;
            }
        }

        static void room1()
        {

        }
    }
}
