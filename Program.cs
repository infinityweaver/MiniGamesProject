using System;
using System.Threading;

namespace MiniGamesProject
{
    class Program
    {
        static void Main(string[] args)
        {
            /* TODO:
             * Re-implement this main program to let the user choose
             * what mini game they would like to play.
             * Add additional function just like
             * static void DealOrNoDeal()
             * for your other minigames.
             */
            DealOrNoDeal();
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Deal or No Deal Minigame
        /// </summary>
        static void DealOrNoDeal()
        {
            Console.WriteLine("Welcome to the Gameshow: Deal or No Deal!\n\n");
            Console.WriteLine("What is your name? ");
            string name = Console.ReadLine();
            if (name == "")
                name = "Juan";
            Console.Clear();

            Console.WriteLine("Good Day, " + name + "!\n\n");
            Console.WriteLine("Choose a currency ([0]-Dollar, [any number]-Peso): ");
            int cur_ans = Convert.ToInt16(Console.ReadLine());
            Currency currency = (cur_ans == 0) ? Currency.Dollar : Currency.Peso;
            Console.WriteLine("Choose a banker mode ([0]-Generous, [1]-Robotic, [any number]-Merciless): ");
            int bm_ans = Convert.ToInt16(Console.ReadLine());
            BankerMode mode = (bm_ans == 0) ? BankerMode.Generous : ((bm_ans == 1) ? BankerMode.Robotic : BankerMode.Merciless);

            DealOrNoDealMiniGame dond = new DealOrNoDealMiniGame(currency, mode);
            int casesOpened = 0, caseNumber = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("Money Board:");
                Console.Write(dond);
                Console.WriteLine(dond.Message);

                switch (dond.State)
                {
                    case GameState.ChooseYourBriefCaseState:
                        Thread.Sleep(2000);
                        Console.WriteLine("Cases:");
                        Console.WriteLine(dond.CaseDisplay());

                        Console.WriteLine("\n\nChoose your case number: ");
                        string response = Console.ReadLine();
                        int choice = -1;
                        try
                        {
                            choice = Convert.ToInt16(response);
                            if (choice < 1 || choice > 26)
                            {
                                dond.Message = "Invalid response. Please choose a valid case number.";
                                continue;
                            }
                            dond.ChosenCaseNumber = choice;
                        }
                        catch(Exception e)
                        {
                            dond.Message = "Invalid response. Please choose a valid case number.";
                            continue;
                        }
                        dond.AssignChosenCase();
                        dond.Message = ("\nYou have chosen Case Number " + dond.ChosenCaseNumber + "!\n");
                        dond.State = GameState.OpenBriefCaseState;
                        break;

                    case GameState.OpenBriefCaseState:
                        Console.WriteLine("Cases:");
                        Console.WriteLine(dond.CaseDisplay());
                        Console.WriteLine("\nPlease choose a case number to open: ");
                        caseNumber = Convert.ToInt16(Console.ReadLine());
                        if(caseNumber < 1 || caseNumber > 26)
                        {
                            dond.Message = "Invalid Case Number! Please choose from 1 to 26";
                            continue;
                        }
                        else if(caseNumber == dond.ChosenCaseNumber)
                        {
                            dond.Message = "You have already chosen that brief case! Please choose anothe brief case.";
                            continue;
                        }
                        else
                        {
                            int result = dond.OpenCaseNumber(caseNumber);
                            if(result == -1)
                            {
                                dond.Message = "This case has been opened. Please choose an unopened brief case.";
                                continue;
                            }
                            else
                            {
                                dond.Message = "You have opened brief case #" + caseNumber + "! It contained " + ((dond.Currency == Currency.Dollar)? "$":"P") + String.Format("{0:n}", result) + "!";
                                casesOpened++;
                                if (casesOpened == 6 || casesOpened == 11 || casesOpened == 15 ||
                                    casesOpened == 18 || casesOpened == 20 || casesOpened == 21 ||
                                    casesOpened == 22 || casesOpened == 23)
                                    dond.State = GameState.BankerOfferState;
                                else if (casesOpened == 24)
                                    dond.State = GameState.FinalRoundState;
                                continue;
                            }
                        }
                    case GameState.BankerOfferState:
                        Console.Write("The banker decides for an offer.");
                        for (int i = 0; i < 5; i++)
                        {
                            Thread.Sleep(1500);
                            Console.Write(".");
                        }
                        Console.WriteLine();
                        dond.Message = "\nThe banker's offer is: " + ((dond.Currency == Currency.Dollar) ? "$" : "P") + String.Format("{0:n}", dond.AskBankerOffer());
                        dond.State = GameState.DealOrNoDealState;
                        break;
                    case GameState.DealOrNoDealState:
                        Console.Write("\nIs it a deal[D] or no deal[N]? ");
                        string ans = Console.ReadLine();
                        if(ans == "D")
                        {
                            dond.State = GameState.DealState;
                        }
                        else if(ans == "N")
                        {
                            dond.State = GameState.OpenBriefCaseState;
                            dond.Message = "You choose No Deal! So let's continue...\n";
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Please choose a valid response.");
                            Thread.Sleep(2000);
                        }
                        break;
                    case GameState.DealState:
                        Console.WriteLine("You choose Deal! You have won " + ((dond.Currency == Currency.Dollar) ? "$" : "P") + String.Format("{0:n}", dond.AskBankerOffer()));
                        Console.Write("The brief case you have chosen contains");
                        for (int i = 0; i < 5; i++)
                        {
                            Thread.Sleep(1500);
                            Console.Write(".");
                        }
                        Console.WriteLine("\n\t");
                        Console.WriteLine(((dond.Currency == Currency.Dollar) ? "$" : "P") + String.Format("{0:n}", dond.ChosenCase.Value) + "!!!!");
                        dond.State = GameState.GameOverState;
                        break;
                    case GameState.FinalRoundState:
                        Console.WriteLine("We now come to the Final Round...");
                        Console.WriteLine("Either your brief case will contain " + ((dond.Currency == Currency.Dollar) ? "$" : "P") + String.Format("{0:n}", dond.GetMinimumValue()));
                        Console.WriteLine("or");
                        Console.WriteLine("it will contain " + ((dond.Currency == Currency.Dollar) ? "$" : "P") + String.Format("{0:n}", dond.GetMaximumValue()));
                        Console.WriteLine("\nThe banker's offer stands at " + ((dond.Currency == Currency.Dollar) ? "$" : "P") + String.Format("{0:n}", dond.AskBankerOffer()));
                        Console.Write("The question remains");
                        for (int i = 0; i < 3; i++)
                        {
                            Thread.Sleep(1500);
                            Console.Write(".");
                        }
                        Console.Write("\nIs it a deal[D] or no deal[N]? ");
                        string ans_final = Console.ReadLine();
                        if (ans_final == "D")
                        {
                            dond.State = GameState.DealState;
                        }
                        else if (ans_final == "N")
                        {
                            Console.Clear();
                            Console.WriteLine("\nYour brief case contains");
                            for (int i = 0; i < 5; i++)
                            {
                                Thread.Sleep(1500);
                                Console.WriteLine(".");
                            }
                            bool win = dond.ChosenCase.Value == dond.GetMaximumValue();
                            double prize_lost = dond.GetMaximumValue();
                            Console.WriteLine(((dond.Currency == Currency.Dollar) ? "$" : "P") + dond.OpenCaseNumber(dond.ChosenCaseNumber) + "!!!!!");
                            if (win)
                                Console.WriteLine("Congratulations!!! You won " + ((dond.Currency == Currency.Dollar) ? "$" : "P") + String.Format("{0:n}", dond.ChosenCase.Value));
                            else
                            {
                                Console.WriteLine("Unfortunately... You will take home " + ((dond.Currency == Currency.Dollar) ? "$" : "P") + String.Format("{0:n}", prize_lost));
                                Console.WriteLine("Better luck next time!");
                            }
                            dond.State = GameState.GameOverState;
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Please choose a valid response.");
                            Thread.Sleep(2000);
                        }
                        break;
                }
            } while (dond.State != GameState.GameOverState);

            Console.WriteLine("THANK YOU FOR PLAYING DEAL OR NO DEAL!");
        }
    }
}
