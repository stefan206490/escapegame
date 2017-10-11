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
        public static int dotProgressTickRate = 400;

        // holds value if you have found the ROT13 note
        public static bool noteSeen = false;

        // bool holding value of kitchen and living room keys
        public static bool hasKitchenKey = false;
        public static bool hasLivingRoomKey = false;

        // chamber's stories initialized in a string array
        public static string[] kamerVerhaal = new string[6];
        
        static void Main(string[] args)
        {
            kamerVerhaalFunctie();
            Console.SetWindowSize(150, 45);
            Program.inGame = mainMenu();
            while (Program.inGame == true)
            {
                gameLoop(Program.currentRoom);
            }
        }

        static void kamerVerhaalFunctie()
        {
            // initializes the strings for the chamber's story
            Program.kamerVerhaal[0] = "Je bent in de hal. Je kan naar de woonkamer of naar de keuken. Wat wil je doen?";
            Program.kamerVerhaal[1] = "Je bent nu in de keuken. Wat wil je doen?";
            Program.kamerVerhaal[2] = "Je bent in de woonkamer. Je ziet een paar dingen wat je wel kan doorzoeken. Wat wil je doen?";
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
                    room2_kitchen();
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

        static int roomChoiceMenu2(string choice1, string choice2)
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
            else if (menuChoice == "terug")
            {
                return 0;
            }
            else
            {
                Console.WriteLine("Ongeldige keuze! Probeer opnieuw");
                Console.ReadLine();
                return 0;
            }
        }

        static int roomChoiceMenu3(string choice1, string choice2, string choice3)
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
            else if (menuChoice == choice3)
            {
                return 3;
            }
            else if (menuChoice == "exit")
            {
                Program.currentRoom = 0;
                return 0;
            }
            else if (menuChoice == "terug")
            {
                return 99;
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
            if (Program.roomVisited[Program.currentRoom - 1] == false)
            {
                foreach (char c in story)
                {
                    System.Console.Write(c);
                    Thread.Sleep(tickRate);
                }
                System.Console.Write("\n");
                Program.roomVisited[Program.currentRoom - 1] = true;
            }
            else
            {
                Console.WriteLine(Program.kamerVerhaal[Program.currentRoom - 1]);
            }
        }

        static void dotProgress()
        {
            // shows a dotting progress bar
            foreach (char c in ".....")
            {
                System.Console.Write(c);
                Thread.Sleep(dotProgressTickRate);
            }
            System.Console.Write("\n");
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
            printRoomStory(kamerVerhaal[Program.currentRoom -1]);
            
            // options
            Console.WriteLine("Wil je naar de keuken of naar de woonkamer? [keuken]/[woonkamer] of [exit]");
            string choiceKeuken = "keuken";
            string choiceWoonkamer = "woonkamer";

            int choice = 0;
            choice = roomChoiceMenu2(choiceKeuken, choiceWoonkamer);
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

        static void room2_kitchen()
        {
            // clears the console first
            Console.Clear();

            // prints out the ascii art
            Program.asciiArt = "Kitchen.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);

            // prints out room story
            printRoomStory(kamerVerhaal[Program.currentRoom - 1]);
            
            // options
            Console.WriteLine("Wat wil je doen in de keuken?\n[koelkast]/[tafel]/[haard] onderzoeken of [terug] of [exit]");
            string choiceKoelkast = "koelkast";
            string choiceTafel = "tafel";
            string choiceHaard = "haard";

            string doorzoeken = "Je kiest voor de";

            int choice = 0;
            choice = roomChoiceMenu3(choiceKoelkast, choiceTafel, choiceHaard);
            if (choice == 1)
            {
                if (Program.noteSeen == true && Program.hasKitchenKey == false)
                {
                    Console.WriteLine("Je doorzoekt de koelkast nog eens goed.");
                    dotProgress();
                    Console.WriteLine("Je vindt een sleutel!");
                    Console.WriteLine("[!] Sleutel werd toegevoegd aan je inventaris!");
                    Program.hasKitchenKey = true;
                    Console.ReadLine();
                }
                else if (Program.noteSeen == true && Program.hasKitchenKey == true)
                {
                    Console.WriteLine("Je hebt de sleutel in de koelkast al gevonden.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("{0} {1}", doorzoeken, choiceKoelkast);
                    dotProgress();
                    Console.WriteLine("Je maakt de koelkast open en ziet veel vlees.");
                    Console.ReadLine();
                }
            }
            else if (choice == 2)
            {
                if (Program.noteSeen == false)
                {
                    // table contains note encrypted with ROT13
                    Console.WriteLine("{0} {1}", doorzoeken, choiceTafel);
                    dotProgress();
                    Console.WriteLine("Je ziet een briefje onder de tafel liggen!");
                    dotProgress();
                    Console.WriteLine("\"Qr fyrhgry yvtg va qr xbryxnfg\"");
                    Thread.Sleep(2000);
                    Console.WriteLine("Je realiseert dat dit briefje met ROT13 is gecodeerd");
                    Console.ReadLine();
                    Program.noteSeen = true;
                }
                else if (Program.noteSeen == true)
                {
                    // you have already found the note in this case
                    Console.WriteLine("\"Qr fyrhgry yvtg va qr xbryxnfg\"");
                    Console.ReadLine();
                }
            }
            else if (choice == 3)
            {
                if (Program.noteSeen == true)
                {
                    // hint if you have already found the note
                    Console.WriteLine("Je ziet een stuk vlees op het spit hangen.\nJe bent hier al geweest, misschien moet je het briefje ontcijferen?");
                }
                else
                {
                    Console.WriteLine("{0} {1}", doorzoeken, choiceHaard);
                    dotProgress();
                    Console.WriteLine("Je ziet een stuk vlees op het spit hangen.");
                    Console.ReadLine();
                }
            }
            else if (choice == 99)
            {
                Console.WriteLine("Je gaat terug naar de hal...");
                Program.currentRoom = 1;
                Console.ReadLine();
            }
        }
    }
}
