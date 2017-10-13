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
        public static int tickRate = 1;
        public static int dotProgressTickRate = 400;
        public static int moveRoomPAuse = 1000;

        // holds value if you have found the ROT13 note
        public static bool noteSeen = false;

        // inventory
        public static bool hasKitchenKey = false;
        public static bool hasLivingRoomKey = false;
        public static bool hasBedroomKey = false;
        public static bool hasHallKey = false;

        public static bool hasMovedChest = false;
        public static bool hasMovedWardrobe = false;

        public static bool hasCandle = false;

        // chamber's stories initialized in a string array
        public static string[] kamerVerhaal = new string[6];
        
        static void Main(string[] args)
        {
            kamerVerhaalFunctie();
            Console.SetWindowSize(140, 40);
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
            Program.kamerVerhaal[3] = "[kamer 4]Je bent nu op de eerste etage. Je ziet 2 deuren. Wat wil je doen?";
            Program.kamerVerhaal[4] = "[kamer 5]Je bent nu in de linker kamer Wat wil je doen.";
            Program.kamerVerhaal[5] = "[kamer 6]Je loopt de rechter slaapkamer binnen en je hoort iemand iets vragen.";
        }

        static bool mainMenu()
        {
            // will show welcome screen string with 100 ms pause between chars
            string mainMenuString = "Welkom bij de escape game!";
            foreach (char c in mainMenuString)
            {
                System.Console.Write(c);
                Thread.Sleep(100);
            }
            System.Console.Write("\n");

            // intro text
            string introText = File.ReadAllText("../../intro.txt");
            foreach (char c in introText)
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
                    room3_livingroom();
                    break;
                case 4:
                    room4_stairs();
                    break;
                case 5:
                    room5_bedroom1();
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

        static int roomChoiceMenu4(string choice1, string choice2, string choice3, string choice4)
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
            else if (menuChoice == choice4)
            {
                return 4;
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
            Console.WriteLine("Wil je naar de keuken, woonkamer of trap? [keuken]/[woonkamer] of [exit]");
            string choiceKeuken = "keuken";
            string choiceWoonkamer = "woonkamer";

            int choice = 0;
            choice = roomChoiceMenu2(choiceKeuken, choiceWoonkamer);
            if (choice == 1)
            {
                Console.WriteLine("Je gaat naar de {0}", choiceKeuken);
                Program.currentRoom = 2;
                Thread.Sleep(Program.moveRoomPAuse);

            }
            else if (choice == 2)
            {
                Console.WriteLine("Je gaat naar de {0}", choiceWoonkamer);
                Program.currentRoom = 3;
                Thread.Sleep(Program.moveRoomPAuse);
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
                    Console.WriteLine("Je doorzoekt de koelkast grondig.");
                    dotProgress();
                    Console.WriteLine("Je vindt een sleutel!");
                    Console.WriteLine("[!] Sleutel 1 werd toegevoegd aan je inventaris!");
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
                    Console.ReadLine();
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

        static void room3_livingroom()
        {
            // clears the console first
            Console.Clear();

            // prints out the ascii art
            Program.asciiArt = "Livingroom.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);

            // prints out room story, no delay if already visited
            if (Program.roomVisited[currentRoom -1] == false)
            {
                printRoomStory(kamerVerhaal[Program.currentRoom - 1]);
                Program.roomVisited[currentRoom - 1] = true;
            }
            else
            {
                Console.WriteLine(kamerVerhaal[Program.currentRoom -1]);
            }

            // options
            Console.WriteLine("Wil je de kist, boekenplank of tafel doorzoeken? [kist]/[boekenplank]/[tafel]/[trap] of [terug] of [exit]");
            string choiceKist = "kist";
            string choiceBoekenplank = "boekenplank";
            string choiceTafel = "tafel";
            string choiceStairs = "trap";

            int choice = 0;
            choice = roomChoiceMenu4(choiceKist, choiceBoekenplank, choiceTafel, choiceStairs);
            if (choice == 1)
            {
                if (Program.hasCandle == false)
                {
                    Console.WriteLine("Je kiest voor de {0}", choiceKist);
                    dotProgress();
                    Console.WriteLine("Je vindt een kaars!");
                    Console.ReadLine();
                    Program.hasCandle = true;
                }
                else if (Program.hasCandle == true)
                {
                    Console.WriteLine("Je had hier eerder een kaars gevonden. Misschien is dit een hint?");
                    Console.ReadLine();
                }
            }
            else if (choice == 2)
            {
                Console.WriteLine("Je kiest voor de {0}", choiceBoekenplank);
                dotProgress();
                Console.WriteLine("Je ziet een paar boeken maar niks speciaals.");
                Console.ReadLine();
            }
            else if (choice == 3)
            {
                if (Program.hasLivingRoomKey == false)
                {
                    Console.WriteLine("Je kiest voor de {0}", choiceTafel);
                    dotProgress();
                    Console.WriteLine("Je vindt een briefje met een raadsel: \"Ik ben lang als ik jong ben en kort als ik oud ben. Wat ben ik?\"\n(hint: het brandt)");
                    System.Console.Write("Antwoord: ");
                    string antwoord = Console.ReadLine();
                    if (antwoord == "kaars")
                    {
                        Console.WriteLine("Gefeliciteerd! Dat was het juiste antwoord!");
                        Console.WriteLine("[!] Sleutel 2 werdt toegevoegd aan je inventaris!");
                        Program.hasLivingRoomKey = true;
                    }
                    else
                    {
                        Console.WriteLine("Helaas! Verkeerd antwoord.");
                    }
                    Console.ReadLine();
                }
                else if (Program.hasLivingRoomKey == true)
                {
                    Console.WriteLine("Je hebt al eerder hier een sleutel gevonden. Je hebt hier niks meer te zoeken.");
                    Console.ReadLine();
                }
            }
            else if (choice == 4)
            {
                if (Program.hasKitchenKey == true && Program.hasLivingRoomKey == true)
                {
                    Console.WriteLine("Je hebt de sleutels en gaat naar de {0}", choiceStairs);
                    Program.currentRoom = 4;
                }
                else
                {
                    Console.WriteLine("De deur bij de trap zit op slot! Je hebt twee sleutels nodig...");
                    Program.currentRoom = 3;
                }
                Console.ReadLine();
            }
            else if (choice == 99)
            {
                Console.WriteLine("Je besluit terug naar de hal te gaan...");
                Program.currentRoom = 1;
                Thread.Sleep(moveRoomPAuse);
            }
        }

        static void room4_stairs()
        {
            // clears the console first
            Console.Clear();

            // prints out the ascii art
            Program.asciiArt = "Stairs.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);

            // prints out room story
            printRoomStory(kamerVerhaal[Program.currentRoom - 1]);

            // options
            string choiceSlaapkamer1 = "links";
            string choiceSlaapkamerEind = "rechts";
            string choiceCode = "trap";
            Console.WriteLine("Wil je naar de linker [links] of rechter [rechts] slaapkamer of [terug] of [exit]?");

            int choice = 0;
            choice = roomChoiceMenu3(choiceSlaapkamer1, choiceSlaapkamerEind, choiceCode);
            if (choice == 1)
            {
                Console.WriteLine("Je gaat naar de linker slaapkamer");
                Program.currentRoom = 5;
                Thread.Sleep(Program.moveRoomPAuse);
            }
            else if (choice == 2)
            {
                Console.WriteLine("Je hebt de twee sleutels en betreedt de rechter slaapkamer");
                Program.currentRoom = 6;
                Thread.Sleep(Program.moveRoomPAuse);
            }
            else if (choice == 3)
            {
                Console.WriteLine("");
            }
            else if (choice == 99)
            {
                Console.WriteLine("Je gaat terug naar de woonkamer.");
                Program.currentRoom = 3;
                Thread.Sleep(Program.moveRoomPAuse);
            }
        }

        static void room5_bedroom1()
        {
            Console.Clear();

            // prints out the ascii art
            Program.asciiArt = "Stairs.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);

            // prints out room story
            printRoomStory(kamerVerhaal[Program.currentRoom - 1]);

            // options
            string choiceChest = "kist";
            string choiceWardrobe = "kast";
            string choiceBed = "bed";
            Console.WriteLine("Wil je de [kist]/[kast]/[bed] doorzoeken of [terug] of [exit]?");

            int choice = 0;
            choice = roomChoiceMenu3(choiceChest, choiceWardrobe, choiceBed);
            if (choice == 1)
            {
                if (Program.hasBedroomKey == true)
                {
                    Console.WriteLine("Je hebt hier eerder al een sleutel gevonden.");
                }
                else if (Program.hasBedroomKey == false)
                {
                    Console.WriteLine("Wil je de kist [open]en of [verplaats]en?");
                    string choiceForChest = Console.ReadLine();
                    if (choiceForChest == "open")
                    {
                        Console.WriteLine("Je probeert de kist te openen...");
                        Thread.Sleep(2000);
                        Console.WriteLine("De kist zit op slot en je hebt een code nodig!");
                        Console.WriteLine("Wat is de code?");
                        System.Console.Write("> ");
                        string chestCode = Convert.ToString(Console.ReadLine());
                        if (chestCode == "462")
                        {
                            Console.WriteLine("De kist opent...");
                            dotProgress();
                            Console.WriteLine("Je vindt een sleutel!");
                            Program.hasBedroomKey = true;
                        }
                        else
                        {
                            Console.WriteLine("De ingevoerde code is niet juist, de kist wilt niet openen");
                        }
                    }
                    else if (choiceChest == "verplaats" && Program.hasMovedChest == false)
                    {
                        Console.WriteLine("Je verplaatst de kist");
                        dotProgress();
                        Console.WriteLine("Je vindt niks");
                        Program.hasMovedChest = true;
                        Console.ReadLine();
                    }
                    else if (choiceChest == "verplaats" && Program.hasMovedChest == true)
                    {
                        Console.WriteLine("Je hebt net de kist van verplaatst en niks gevonden");
                        Console.ReadLine();
                    }
                }
            }
            else if (choice == 2)
            {
                Console.WriteLine("Je doorzoekt de kast");
                dotProgress();
                Console.WriteLine("Je vindt een briefje:");
                Console.WriteLine("\"Er ligt een briefje met de code achter de schilderij in de hal.\"");
                Console.ReadLine();
            }
            else if (choice == 3)
            {
                // todo
            }
            else if (choice == 99)
            {
                Console.WriteLine("Je gaat terug de traphal");
                Program.currentRoom = 4;
                Thread.Sleep(Program.moveRoomPAuse);
            }
        }
    }  
}
