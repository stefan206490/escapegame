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
        public static int tickRate = 10;
        public static int dialogueTickRate = 40;
        public static int dotProgressTickRate = 400;
        public static int moveRoomPause = 1000;
        public static int introTickRate = 3;
        public static int getal;

        public static ThreadStart ts = new ThreadStart(Timer);
        public static Thread TimerThread = new Thread(ts);
        public static int intTimer = 1200;

        // inventory
        public static bool hasKitchenKey = false;
        public static bool hasLivingRoomKey = false;
        public static bool hasBedroomKey = false;

        // environment changes
        public static bool firePlaceSeen = false;
        public static bool noteSeen = false;
        public static bool hasMovedChest = false;
        public static bool hasMovedWardrobe = false;
        public static bool hasPaintingCode = false;
        public static bool miniGameCompleted = false;

        public static bool hasCandle = false;

        // determines ending
        public static bool friendIsDead = false;

        // ROT13 unecrypted
        public static string kitchenCodeCapital = "De sleutel ligt in de koelkast";
        public static string kitchenCodeNonCapital = "de sleutel ligt in de koelkast";

        // chamber's stories initialized in a string array
        public static string[] kamerVerhaal = new string[6];

        // final dialogue string array
        public static string[] dialogueArray = new string[13];

        public static string cheatCodeString = "thereisnospoon1337";

        static void Main(string[] args)
        {
            kamerVerhaalFunctie();
            finalDialogue();
            Console.SetWindowSize(150, 45);
            Program.inGame = mainMenu();
            
            while (Program.inGame == true)
            {
                gameLoop(Program.currentRoom);
            }
        }

        static void cheatCodeFunction()
        {
            // gives keys
            Program.hasKitchenKey = true;
            Program.hasLivingRoomKey = true;
            Program.hasBedroomKey = true;

            // gives objects
            Program.hasCandle = true;
            Program.hasPaintingCode = true;

            Console.WriteLine("Cheat code geactiveerd!");
            Thread.Sleep(moveRoomPause);
        }

        static void kamerVerhaalFunctie()
        {
            // initializes the strings for the chamber's story
            Program.kamerVerhaal[0] = "Je bent in de hal. Je kan naar de woonkamer of naar de keuken. Wat wil je doen?";
            Program.kamerVerhaal[1] = "Je bent nu in de keuken. Wat wil je doen?";
            Program.kamerVerhaal[2] = "Je bent in de woonkamer. Je ziet een paar dingen die je wel kan doorzoeken. Wat wil je doen?";
            Program.kamerVerhaal[3] = "Je bent nu op de eerste etage. Je ziet 2 deuren. Wat wil je doen?";
            Program.kamerVerhaal[4] = "Je bent nu in de linker kamer Wat wil je doen.";
            Program.kamerVerhaal[5] = "Je loopt de rechter slaapkamer binnen en je hoort iemand iets vragen.";
        }

        static void finalDialogue()
        {
            Program.dialogueArray[0] = "Jochem: Hallo, wie is daar?";
            Program.dialogueArray[1] = "Ik: Ben jij dat Jochem?";
            Program.dialogueArray[2] = "Jochem: Ja ik ben het, hoe gaat het met jou?";
            Program.dialogueArray[3] = "Ik: Het gaat goed met mij. De heks heeft niks opgemerkt.";
            Program.dialogueArray[4] = "Ik: Nee het gaat niet goed! Ik was doodongerust over jou. Hoe heb je je ooit kunnen laten opsluiten door die oude taart!?";
            Program.dialogueArray[5] = "Jochem: Je zegt het alsof het mijn schuld is dat ik hier opgesloten zit.";
            Program.dialogueArray[6] = "Ik: Hoe dan ook laten hier snel weg wezen voordat de heks terugkomt.";
            Program.dialogueArray[7] = "Jochem: Hoe kunnen we hier wegkomen?";
            Program.dialogueArray[8] = "Ik: Tegen het raam staat een ladder, daarmee kunnen we uit het raam klimmen.";
            Program.dialogueArray[9] = "Wil je je Jochem uit het raam duwen? [ja]/[nee]";
            Program.dialogueArray[10] = "Ik: Ga snel via de ladder naar beneden. Alleen zo komen we weg uit dit huis.";
            Program.dialogueArray[11] = "Ik: Sorry, je ouders denken al dat je dood bent. Je bent het niet waard om hier levend uit te komen.";
            Program.dialogueArray[12] = "(Je duwt je Jochem uit het raam, hij valt op zijn hoofd. Je bent zelf ontsnapt)";
        }

        public static void Timer()
        {
            do
            {
                intTimer--;
                Thread.Sleep(1000);
            }
            while (intTimer >= 0);
            //Gameover stuff
        }

        static bool mainMenu()
        {
            // will show welcome screen string with 100 ms pause between chars
            string mainMenuString = "Welkom bij de escape game!";
            foreach (char c in mainMenuString)
            {
                System.Console.Write(c);
                Thread.Sleep(introTickRate);
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

            
            
            bool validChoice = false;
            bool startGameChoice = false;
            do
            {
                // players gets to choose to start a new game or exit
                Console.WriteLine("Schrijf [new]/[1] voor nieuwe game,[load]/[2] om een opgeslagen game te laden of schrijf [exit]/[3] om de game af te sluiten");
                string menuChoice = Console.ReadLine();
                if (menuChoice == "new" || menuChoice == "1")
                {
                    // sets all values in roomVisited array to false
                    for (int i = 0; i < roomVisited.Length; i++)
                    {
                        Program.roomVisited[i] = false;
                    }

                    // returns true to commence game
                    TimerThread.Start();
                    validChoice = true;
                    startGameChoice = true;
                }
                else if (menuChoice == "load" || menuChoice == "2")
                {
                    Program.load();
                    return true;
                }
                else if (menuChoice == "exit" || menuChoice == "3")
                {
                    // exits the game
                    Console.WriteLine("Game wordt afgesloten...");
                    Thread.Sleep(2000);
                    validChoice = true;
                    startGameChoice = false;
                }
                else
                {
                    Console.WriteLine("Ongeldige keuze!");
                    Thread.Sleep(500);
                    Console.Clear();
                }

                if (validChoice == true && startGameChoice == true)
                {
                    if (startGameChoice == true)
                    {
                        return true;
                    }
                    else if (startGameChoice == false)
                    {
                        return false;
                    }
                }
            } while (validChoice == false);
            return false;
        }
        static void save()
        {
            string array = hasKitchenKey.ToString() + "\r\n" + hasLivingRoomKey.ToString() + "\r\n" + hasBedroomKey.ToString() + "\r\n" + firePlaceSeen.ToString() + "\r\n" + noteSeen.ToString() + "\r\n" + hasMovedChest.ToString() + "\r\n" + hasMovedWardrobe.ToString() + "\r\n" + hasPaintingCode.ToString() + "\r\n" + miniGameCompleted.ToString() + "\r\n" + hasCandle.ToString() + "\r\n" + intTimer.ToString() + "\r\n" + currentRoom.ToString();
            System.IO.StreamWriter savewriter = new System.IO.StreamWriter("SavedGame.txt"); //Picks up the place and the Streamwriter
            savewriter.WriteLine(array); //Writes the document (string array)
            savewriter.Close(); //Closes the stream
        }

        static void load()
        {
            string array = File.ReadAllText("SavedGame.txt"); //Reads the file
            intTimer = int.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(10)); //Reads every line 1 by 1 and converts it to bool
            hasKitchenKey = bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(0));
            hasLivingRoomKey = bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(1));
            hasBedroomKey = bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(2));
            firePlaceSeen = bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(3));
            noteSeen = bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(4));
            hasMovedChest = bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(5));
            hasMovedWardrobe= bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(6));
            hasPaintingCode = bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(7));
            miniGameCompleted = bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(8));
            hasCandle = bool.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(9));
            currentRoom = int.Parse(File.ReadLines("SavedGame.txt").ElementAtOrDefault(11));
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
                    room6_bedroom2_final();
                    break;
                case 7:
                    showCredits();
                    break;
                case 8:
                    numberMiniGame();
                    break;
                case 9:
                    debugMode();
                    break;
                default:
                    Console.WriteLine("Game wordt afgesloten");
                    Thread.Sleep(2000);
                    TimerThread.Abort();
                    Program.inGame = false;
                    break;
            }
        }

        static void timeGameOver()
        {
            Console.Clear();
            Console.WriteLine("Sorry, je tijd is op. Game over!");
            Console.ReadLine();
            Program.currentRoom = 0;

        }

        static int roomChoiceMenu2(string choice1, string choice2)
        {
            if (intTimer <= 0)
            {
                timeGameOver();
                return 0;
            }
            string menuChoice = Console.ReadLine();
            if (menuChoice == choice1)
            {

                return 1;
            }
            else if (menuChoice == choice2)
            {
                return 2;
            }
            else if (menuChoice == "save")
            {
                Program.save();
                Console.WriteLine("Game wordt opgeslagen!");
                dotProgress();
                return 8;
            }
            else if (menuChoice == "debug")
            {
                Program.currentRoom = 9;
                return 9;
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

        static int roomChoiceMenu3(string choice1, string choice2, string choice3)
        {
            if (intTimer <= 0)
            {
                timeGameOver();
                return 0;
            }
            string menuChoice = Console.ReadLine();
            if (menuChoice == choice1)
            {
                return 1;
            }
            else if (menuChoice == choice2)
            {
                return 2;
            }
            else if (menuChoice == "save")
            {
                Program.save();
                Console.WriteLine("Game wordt opgeslagen!");
                dotProgress();
                return 8;
            }
            else if (menuChoice == "debug")
            {
                Program.currentRoom = 9;
                return 9;
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
            if (intTimer <= 0)
            {
                timeGameOver();
                return 0;
            }
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
            else if (menuChoice == "save")
            {
                Program.save();
                Console.WriteLine("Game wordt opgeslagen!");
                dotProgress();
                return 8;
            }
            else if (menuChoice == "debug")
            {
                Program.currentRoom = 9;
                return 9;
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

        static int roomChoiceMenu5(string choice1, string choice2, string choice3, string choice4, string choice5)
        {
            if (intTimer <= 0)
            {
                timeGameOver();
                return 0;
            }
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
            else if (menuChoice == choice5)
            {
                return 5;
            }
            else if (menuChoice == "save")
            {
                Program.save();
                Console.WriteLine("Game wordt opgeslagen!");
                dotProgress();
                return 8;
            }
            else if (menuChoice == "debug")
            {
                Program.currentRoom = 9;
                return 9;
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

        static int roomChoiceMenu6(string choice1, string choice2, string choice3, string choice4, string choice5, string choice6)
        {
            if (intTimer <= 0)
            {
                timeGameOver();
                return 0;
            }
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
            else if (menuChoice == choice5)
            {
                return 5;
            }
            else if (menuChoice == choice6)
            {
                return 6;
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

        static void printDialogue(string story)
        {
            foreach (char c in story)
            {
                System.Console.Write(c);
                Thread.Sleep(dialogueTickRate);
            }
            System.Console.Write("\n");
        }

        static void printInventory()
        {
            if (intTimer > 0)
            {
                string zeroSpace;
                if (intTimer % 60 < 10 && intTimer > 0)
                {
                    zeroSpace = "0";
                }
                else if (intTimer % 60 == 0)
                {
                    zeroSpace = "00";
                }
                else
                {
                    zeroSpace = "";
                }
                Console.WriteLine("Timer: {0}:{1}{2}", intTimer / 60, zeroSpace, intTimer % 60);
            }
            if (Program.hasKitchenKey == true && Program.hasLivingRoomKey == false)
            {
                Console.WriteLine("Sleutels: keuken sleutel");
            }
            else if (Program.hasLivingRoomKey == true && Program.hasKitchenKey == false)
            {
                Console.WriteLine("Sleutels: woonkamer sleutel");
            }
            else if (Program.hasKitchenKey == true && Program.hasLivingRoomKey == true && hasBedroomKey == false)
            {
                Console.WriteLine("Sleutels: keuken sleutel; woonkamer sleutel");
            }
            else if (Program.hasKitchenKey == true && Program.hasLivingRoomKey == true && Program.hasBedroomKey == true)
            {
                Console.WriteLine("Sleutels: keuken sleutel; woonkamer sleutel; slaapkamer sleutel");
            }

            if (Program.hasCandle == true && Program.hasPaintingCode == false)
            {
                Console.WriteLine("Objecten: kaars");
            }
            else if (Program.hasCandle == false && Program.hasPaintingCode == true)
            {
                Console.WriteLine("Objecten: code: 264");
            }
            else if (Program.hasCandle == true && Program.hasPaintingCode == true)
            {
                Console.WriteLine("Objecten: kaars; code 264");
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

            printInventory();

            // prints out the ascii art
            Program.asciiArt = "Hal.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);

            // prints out room story
            printRoomStory(kamerVerhaal[Program.currentRoom -1]);
            
            // options (before)

            if (Program.hasMovedWardrobe == false)
            {
                Console.WriteLine("Wil je naar de keuken of woonkamer? [keuken]/[woonkamer] of [save] of [exit]");
            }
            else if (Program.hasMovedWardrobe == true)
            {
                Console.WriteLine("Kies: [keuken]/[woonkamer]/[schilderij] of [save] of [exit]");
            }
            
            string choiceKeuken = "keuken";
            string choiceWoonkamer = "woonkamer";
            string choicePainting = "schilderij";

            int choice = 0;
            choice = roomChoiceMenu4(choiceKeuken, choiceWoonkamer, choicePainting, Program.cheatCodeString);
            if (choice == 1)
            {
                Console.WriteLine("Je gaat naar de {0}", choiceKeuken);
                Program.currentRoom = 2;
                Thread.Sleep(Program.moveRoomPause);

            }
            else if (choice == 2)
            {
                Console.WriteLine("Je gaat naar de {0}", choiceWoonkamer);
                Program.currentRoom = 3;
                Thread.Sleep(Program.moveRoomPause);
            }
            else if (choice == 3)
            {
                if (Program.hasMovedWardrobe == true && Program.hasPaintingCode == false)
                {
                    Console.WriteLine("Je verplaatst de schilderij.");
                    dotProgress();
                    Console.WriteLine("Je vindt een papiertje met drie cijfers: 264");
                    Program.hasPaintingCode = true;
                    Console.ReadLine();
                }
                else if (Program.hasMovedWardrobe == true && Program.hasPaintingCode == true)
                {
                    Console.WriteLine("Je hebt hier net een code gevonden: 264");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Ongeldige keuze!");
                    Thread.Sleep(moveRoomPause);
                }
            }
            else if (choice == 4)
            {
                cheatCodeFunction();
            }
        }

        static void room2_kitchen()
        {
            // clears the console first
            Console.Clear();

            printInventory();

            // prints out the ascii art
            Program.asciiArt = "Kitchen.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);

            // prints out room story
            printRoomStory(kamerVerhaal[Program.currentRoom - 1]);
            
            // options
            Console.WriteLine("Wat wil je doen in de keuken?\n[koelkast]/[tafel]/[haard] onderzoeken of [terug] of [save] of [exit]");
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
                    Thread.Sleep(1000);
                    dotProgress();
                    Console.WriteLine("Je realiseert dat dit briefje met ROT13 is gecodeerd");

                    Console.WriteLine("Wat staat er eigenlijk echt op het briefje? (Voer de ontcijferde tekst in):");
                    string ROT13Code = Console.ReadLine();
                    if (ROT13Code == Program.kitchenCodeNonCapital || ROT13Code == Program.kitchenCodeCapital)
                    {
                        Console.WriteLine("Dat klopt! Je hebt de code ontcijferd.");
                        Console.ReadLine();
                        Program.noteSeen = true;
                    }
                    else
                    {
                        Console.WriteLine("Jouw input was niet correct. Probeer opnieuw!");
                        Thread.Sleep(moveRoomPause);
                    }
                }
                else if (Program.noteSeen == true)
                {
                    // you have already found the note in this case
                    Console.WriteLine("\"de sleutel ligt in de koelkast\"");
                    Console.ReadLine();
                }
            }
            else if (choice == 3)
            {
                if (Program.noteSeen == true)
                {
                    // hint if you have already found the note
                    Console.WriteLine("Je ziet een stuk vlees op het spit hangen.");
                    if (Program.firePlaceSeen == true)
                    {
                        Console.WriteLine("Je bent hier al geweest, misschien moet je naar het briefje kijken?");
                    }
                    Console.ReadLine();
                }
                else if (Program.firePlaceSeen == true)
                {
                    Console.WriteLine("Je ziet een stuk vlees op het spit hangen.\nJe bent hier al geweest.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("{0} {1}", doorzoeken, choiceHaard);
                    dotProgress();
                    Console.WriteLine("Je ziet een stuk vlees op het spit hangen.");
                    Console.ReadLine();
                }
                Program.firePlaceSeen = true;
            }
            else if (choice == 99)
            {
                Console.WriteLine("Je gaat terug naar de hal...");
                Program.currentRoom = 1;
                Thread.Sleep(moveRoomPause);
            }
        }

        static void room3_livingroom()
        {
            // clears the console first
            Console.Clear();

            printInventory();

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
            Console.WriteLine("Wil je de kist, boekenplank of tafel doorzoeken? [kist]/[boekenplank]/[tafel]/[trap] of [hal] of [save] of [exit]");
            string choiceKist = "kist";
            string choiceBoekenplank = "boekenplank";
            string choiceTafel = "tafel";
            string choiceStairs = "trap";
            string choiceHall = "hal";

            int choice = 0;
            choice = roomChoiceMenu5(choiceKist, choiceBoekenplank, choiceTafel, choiceStairs, choiceHall);
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
                    Console.WriteLine("Je vindt een briefje met een raadsel: \"Ik ben lang als ik jong ben en kort als ik oud ben. Wat ben ik?\"");
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
                    Thread.Sleep(moveRoomPause);
                }
                else
                {
                    Console.WriteLine("De deur bij de trap zit op slot! Je hebt twee sleutels nodig...");
                    Program.currentRoom = 3;
                    Console.ReadLine();
                }
            }
            else if (choice == 5)
            {
                Console.WriteLine("Je besluit terug naar de hal te gaan...");
                Program.currentRoom = 1;
                Thread.Sleep(moveRoomPause);
            }
        }

        static void room4_stairs()
        {
            // clears the console first
            Console.Clear();

            printInventory();

            // prints out the ascii art
            Program.asciiArt = "Stairs.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);

            // prints out room story
            printRoomStory(kamerVerhaal[Program.currentRoom - 1]);

            // options
            string choiceSlaapkamer1 = "links";
            string choiceSlaapkamerEind = "rechts";
            string choiceCandle = "kaars";
            string choiceLivingRoom = "woonkamer";
            Console.WriteLine("Wil je naar de linker [links] of rechter [rechts] slaapkamer? of [woonkamer] of [save] of [exit]?");
            

            int choice = 0;
            choice = roomChoiceMenu4(choiceSlaapkamer1, choiceSlaapkamerEind, choiceCandle, choiceLivingRoom);
            if (choice == 1)
            {
                Console.WriteLine("Je gaat naar de linker slaapkamer");
                Program.currentRoom = 5;
                Thread.Sleep(Program.moveRoomPause);
            }
            else if (choice == 2)
            {
                if (Program.hasBedroomKey == true)
                {
                    Console.WriteLine("Je hebt de sleutel voor de laatste kamer en betreedt de rechter slaapkamer.");
                    Program.currentRoom = 6;
                    Thread.Sleep(Program.moveRoomPause);
                }
                else if (Program.hasBedroomKey == false)
                {
                    Console.WriteLine("Je hebt geen sleutel om de rechter deur te openen.");
                    Console.ReadLine();
                }

            }
            else if (choice == 3 && Program.hasCandle == true)
            {
                Program.currentRoom = 8;
            }
            else if (choice == 4)
            {
                Console.WriteLine("Je gaat terug naar de woonkamer.");
                Program.currentRoom = 3;
                Thread.Sleep(Program.moveRoomPause);
            }
        }

        static void numberMiniGame()
        {
            Console.Clear();
            Console.WriteLine("Je belicht de kamer en ziet een geheime doorgang!");
            dotProgress();

            Random rnd = new Random();
            int randGetal = rnd.Next(1, 10001);

            bool inMiniGame = true;

            while (inMiniGame == true)
            {
                Console.Clear();
            
                // prints out the ascii art
                Program.asciiArt = "Easteregg.txt";
                string printOut = File.ReadAllText(filePath + Program.asciiArt);
                Console.WriteLine(printOut);

                // loop only exits if input is valid numeric
                bool inputIsNumeric = false;

                // resets the global getal variable and prepares for new input
                Program.getal = 0;
               
                while (inputIsNumeric == false)
                {
                    Console.WriteLine("(Mini game) raad een getal tussen de 1 en de 10000 en -1 om terug te gaan: ");
                    string c = Console.ReadLine();
                    
                    if (int.TryParse(c, out getal))
                    {
                        getal = Convert.ToInt32(c);
                        inputIsNumeric = true;
                    }
                }

                if (Program.getal < randGetal && Program.getal >= 1)
                {
                    Console.Beep(500, 500);
                    Console.WriteLine("Te laag!");
                    Console.ReadLine();
                }
                else if (Program.getal > randGetal && Program.getal >= 1)
                {
                    Console.Beep(3000, 500);
                    Console.WriteLine("Te hoog!");
                    Console.ReadLine();
                }
                else if (Program.getal == randGetal && Program.getal >= 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Beep(500, 500);
                        Thread.Sleep(300);
                    }
                    Console.WriteLine("Dat was juist! Het nummer was: {0}", randGetal);
                    Console.WriteLine("De cheatcode is: \"thereisnospoon1337\"\nVoer deze in bij de hal.");
                    Program.miniGameCompleted = true;
                    Console.ReadLine();

                    inMiniGame = false;
                    Program.currentRoom = 4;
                    Console.WriteLine("Mini game wordt afgesloten...");
                    Thread.Sleep(moveRoomPause);

                }
                else if (Program.getal == -1)
                {
                    inMiniGame = false;
                    Program.currentRoom = 4;
                    Console.WriteLine("Mini game wordt afgesloten...");
                    Thread.Sleep(moveRoomPause);
                }
            }
            
        }

        static void room5_bedroom1()
        {
            Console.Clear();

            printInventory();

            // prints out the ascii art
            Program.asciiArt = "Bedroom1.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);

            // prints out room story
            printRoomStory(kamerVerhaal[Program.currentRoom - 1]);

            // options
            string choiceChest = "kist";
            string choiceWardrobe = "kast";
            string choiceBed = "bed";
            Console.WriteLine("Wil je de [kist]/[kast]/[bed] doorzoeken of [terug] of [save] of [exit]?");

            int choice = 0;
            choice = roomChoiceMenu3(choiceChest, choiceWardrobe, choiceBed);
            if (choice == 1)
            {
                if (Program.hasBedroomKey == true)
                {
                    Console.WriteLine("Je hebt hier eerder al een sleutel gevonden.");
                    Console.ReadLine();
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
                            Console.ReadLine();
                            Program.hasBedroomKey = true;
                        }
                        else
                        {
                            Console.WriteLine("De ingevoerde code is niet juist, de kist wilt niet openen");
                            Thread.Sleep(moveRoomPause);
                        }
                    }
                    else if (choiceForChest == "verplaats" && Program.hasMovedChest == false)
                    {
                        Console.WriteLine("Je verplaatst de kist");
                        dotProgress();
                        Console.WriteLine("Je vindt niks");
                        Program.hasMovedChest = true;
                        Console.ReadLine();
                    }
                    else if (choiceForChest == "verplaats" && Program.hasMovedChest == true)
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
                Program.hasMovedWardrobe = true;
                Console.ReadLine();
            }
            else if (choice == 3)
            {
                Console.WriteLine("Je zoekt onder het bed");
                dotProgress();
                Console.WriteLine("Je vindt niks");
                Thread.Sleep(moveRoomPause);
            }
            else if (choice == 99)
            {
                Console.WriteLine("Je gaat terug de traphal");
                Program.currentRoom = 4;
                Thread.Sleep(Program.moveRoomPause);
            }
        }

        static void room6_bedroom2_final()
        {
            Console.Clear();

            Program.asciiArt = "Bedroom2.txt";
            string printOut = File.ReadAllText(filePath + Program.asciiArt);
            Console.WriteLine(printOut);
            
            // ending dialogue
            endingDialogue();
        }

        static void debugMode()
        {
            Console.Clear();

            Console.WriteLine("Debug mode: [hal][woonkamer][keuken][trap][slaapkamer1][slaapkamer2] of [exit]");

            string choiceHall = "hal";
            string choiceLivingRoom = "woonkamer";
            string choiceKitchen = "keuken";
            string choiceStairs = "trap";
            string choiceBedroomLeft = "slaapkamer1";
            string choiceBedroomRight = "slaapkamer2";

            int choice = roomChoiceMenu6(choiceHall, choiceKitchen, choiceLivingRoom, choiceStairs, choiceBedroomLeft, choiceBedroomRight);
            switch (choice)
            {
                case 1:
                    Program.currentRoom = 1;
                    break;
                case 2:
                    Program.currentRoom = 2;
                    break;
                case 3:
                    Program.currentRoom = 3;
                    break;
                case 4:
                    Program.currentRoom = 4;
                    break;
                case 5:
                    Program.currentRoom = 5;
                    break;
                case 6:
                    Program.currentRoom = 6;
                    break;
                case 99:
                    Program.currentRoom = 0;
                    break;
            }
        }

        static void endingDialogue()
        {
            for (int i = 0; i < 3; i++)
            {
                printDialogue(Program.dialogueArray[i]);
                Thread.Sleep(moveRoomPause);
            }

            // string variablse for first dialogue choices
            string dialogueChoice1;
            string dialogueChoice2;

            // dialogue choice 1
            while (true)
            {
                Console.WriteLine("Wat wil je zeggen? [1] Met mij gaat het goed / [2] Nee, het gaat niet goed");
                dialogueChoice1 = Console.ReadLine();
                if (dialogueChoice1 == "1")
                {
                    printDialogue(Program.dialogueArray[3]);
                    Thread.Sleep(moveRoomPause);
                    break;
                }
                else if (dialogueChoice1 == "2")
                {
                    for (int j = 4; j < 7; j++)
                    {
                        printDialogue(Program.dialogueArray[j]);
                        Thread.Sleep(moveRoomPause);
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldige keuze. Kies [1] of [2]");
                }
            }

            for (int x = 7; x < 9; x++)
            {
                printDialogue(Program.dialogueArray[x]);
                Thread.Sleep(moveRoomPause);
            }

            // dialogue choice 2
            while (true)
            {
                Console.WriteLine(dialogueArray[9]);
                dialogueChoice2 = Console.ReadLine();
                if (dialogueChoice2 == "nee")
                {
                    printDialogue(Program.dialogueArray[10]);
                    Thread.Sleep(moveRoomPause);
                    break;
                }
                else if (dialogueChoice2 == "ja")
                {
                    for (int j = 11; j < 13; j++)
                    {
                        printDialogue(Program.dialogueArray[j]);
                        Thread.Sleep(moveRoomPause);
                    }
                    Program.friendIsDead = true;
                    break;
                }
                else
                {
                    Console.WriteLine("Ongeldige keuze. Kies [ja] of [nee]");
                }
            }
            Thread.Sleep(moveRoomPause * 2);

            Console.Clear();

            if (Program.friendIsDead == true)
            {
                Program.asciiArt = "EndingBad.txt";
                string printOut = File.ReadAllText(filePath + Program.asciiArt);
                Console.WriteLine(printOut);
                
                Console.WriteLine("Jochem is dood! Game over.");
                Console.ReadLine();
            }
            else
            {
                Program.asciiArt = "EndingGood.txt";
                string printOut = File.ReadAllText(filePath + Program.asciiArt);
                Console.WriteLine(printOut);
                
                Console.WriteLine("Je bent samen met Jochem uit het huis ontsnapt! Game over.");
                Console.ReadLine();
            }

            if (Program.hasCandle == true &&
                Program.hasMovedChest == true &&
                Program.hasMovedWardrobe == true &&
                Program.hasPaintingCode == true &&
                Program.miniGameCompleted == true &&
                Program.noteSeen == true)
            {
                Console.WriteLine("Achievement: Completionist");
            }
            Program.currentRoom = 7;
            Console.ReadLine();
        }

        static void showCredits()
        {
            Console.Clear();
            Console.WriteLine("Dank je voor het spelen van het spel!");

            // show credits
            string introText = File.ReadAllText("../../credits.txt");
            foreach (char c in introText)
            {
                System.Console.Write(c);
                Thread.Sleep(dialogueTickRate);
            }
            System.Console.Write("\n");

            Console.ReadLine();
            Program.currentRoom = 0;
        }
    }  
}
