using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniGamesProject.DealOrNoDeal;

namespace MiniGamesProject
{
    class DealOrNoDealMiniGame : MiniGame
    {
        private static int[] CASE_VALUES = 
            new int[] { 1, 1000, 5, 2500, 10, 5000, 25, 10000,
                        50, 25000, 75, 50000, 100, 100000,
                        150, 200000, 200, 300000, 300, 400000,
                        400, 500000, 500, 1000000, 750, 2000000 };

        public GameState State { get; set; }
        public Currency Currency { get; private set; }
        private Case[] cases { get; set; }
        private bool[] case_opened { get; set; }
        private bool[] values_opened { get; set; }

        public Banker Banker { get; private set; }
        public double CurrentOffer { get; private set; }

        public int ChosenCaseNumber { get; set; }
        public Case ChosenCase { get; private set; }

        public string Message { get; set; }

        /// <summary>
        /// Deal or No Deal Mini Game Constructor
        /// </summary>
        /// <param name="currency">Currency used in the game.</param>
        /// <param name="bankerMode">Mode of the banker in the game.</param>
        public DealOrNoDealMiniGame(Currency currency, BankerMode bankerMode) : base("Deal or No Deal")
        {
            this.Currency = currency;
            this.reshuffleCases();
            this.Banker = new Banker(bankerMode);
            this.CurrentOffer = 0;
            this.ChosenCaseNumber = 0;
            this.State = GameState.ChooseYourBriefCaseState;
            this.Message = "";
        }

        /// <summary>
        /// Called to reshuffle the values of the cases attribute.
        /// Called for every new game.
        /// </summary>
        private void reshuffleCases()
        {
            this.cases = null;
            this.case_opened = null;
            this.values_opened = null;
            GC.Collect();

            this.cases = new Case[DealOrNoDealMiniGame.CASE_VALUES.Length];
            this.case_opened = new bool[DealOrNoDealMiniGame.CASE_VALUES.Length];
            this.values_opened = new bool[DealOrNoDealMiniGame.CASE_VALUES.Length];

            List<Case> caselist = new List<Case>();
            foreach (int value in DealOrNoDealMiniGame.CASE_VALUES)
                caselist.Add(new Case(value, Currency));

            Random r = new Random(DateTime.Now.Second);
            for(int i=0; i < this.cases.Length; i++)
            {
                int index = r.Next(caselist.Count);
                this.cases[i] = caselist[index];
                caselist.RemoveAt(index);
            }
        }

        /// <summary>
        /// Assigns the Chosen Case based on the Chosen Case Number
        /// </summary>
        public void AssignChosenCase()
        {
            this.ChosenCase = this.cases[this.ChosenCaseNumber - 1];
        }

        /// <summary>
        /// Open the case with the specified case number.
        /// </summary>
        /// <param name="caseNumber">Case number from 1 to the last number of the brief cases.</param>
        /// <returns>The value inside the brief case of the selected case number.</returns>
        public int OpenCaseNumber(int caseNumber)
        {
            if (caseNumber < 1 || caseNumber > DealOrNoDealMiniGame.CASE_VALUES.Length || this.case_opened[caseNumber - 1])
                return -1;
            this.case_opened[caseNumber - 1] = true;
            this.values_opened[DealOrNoDealMiniGame.CASE_VALUES.ToList().IndexOf(this.cases[caseNumber - 1].Value)] = true;
            return this.cases[caseNumber - 1].Value;
        }

        /// <summary>
        /// Ask the banker for its offer.
        /// </summary>
        /// <returns>Returns the offer of the banker.</returns>
        public double AskBankerOffer()
        {
            List<int> valuesLeft = new List<int>();
            for (int i = 0; i < this.case_opened.Length; i++)
                if (!this.case_opened[i])
                    valuesLeft.Add(this.cases[i].Value);
            this.CurrentOffer = this.Banker.ComputeOffer(valuesLeft);
            return this.CurrentOffer;
        }

        /// <summary>
        /// Get the minimum value of the cases that have not been opened.
        /// </summary>
        /// <returns>Returns the minimum value of the cases that have not been opened.</returns>
        public int GetMinimumValue()
        {
            List<int> valuesLeft = new List<int>();
            for (int i = 0; i < this.case_opened.Length; i++)
                if (!this.case_opened[i])
                    valuesLeft.Add(this.cases[i].Value);
            return valuesLeft.Min();
        }


        /// <summary>
        /// Get the maximum value of the cases that have not been opened.
        /// </summary>
        /// <returns>Returns the maximum value of the cases that have not been opened.</returns>
        public int GetMaximumValue()
        {
            List<int> valuesLeft = new List<int>();
            for (int i = 0; i < this.case_opened.Length; i++)
                if (!this.case_opened[i])
                    valuesLeft.Add(this.cases[i].Value);
            return valuesLeft.Max();
        }

        /// <summary>
        /// Console Display for the cases
        /// </summary>
        /// <returns>Returns a formatted string representing the cases that have been and not have been opened.</returns>
        public string CaseDisplay()
        {
            string display = "";
            int casePerRow = 6;

            for (int i = 0; i < this.cases.Length; i++)
            {
                display += (i == this.ChosenCaseNumber - 1) ? "[CC]" : ((this.case_opened[i]) ? "[XX]" : ("[" + (i + 1).ToString("00") + "]"));
                if (i % casePerRow == casePerRow - 1)
                    display += "\n";
            }

            return display;
        }

        /// <summary>
        /// ToString function for this minigame.
        /// </summary>
        /// <returns>Returns a formatted string representing the money board.</returns>
        public override string ToString()
        {
            char curr = '$';
            switch(this.Currency)
            {
                case Currency.Dollar: curr = '$'; break;
                case Currency.Peso: curr = 'P'; break;
            }

            string display = "";
            List<int> case_values = DealOrNoDealMiniGame.CASE_VALUES.ToList();
            for(int i=0; i < 13; i++)
            {
                display += (this.values_opened[i*2]) ? "XXX" : String.Format(curr + " {000}", DealOrNoDealMiniGame.CASE_VALUES[i*2]);
                display += "\t";
                display += (this.values_opened[i * 2 + 1]) ? "XXXXXXX" : String.Format(curr + " {0:n0}", DealOrNoDealMiniGame.CASE_VALUES[i * 2 + 1]);
                display += "\n";
            }

            return display;
        }
    }
}
