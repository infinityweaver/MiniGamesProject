using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGamesProject.DealOrNoDeal
{
    class Banker
    {
        public BankerMode Mode { get; set; }

        /// <summary>
        /// Banker Constructor
        /// </summary>
        /// <param name="mode">The mode/attitude of the banker for how he computes his/her offer.</param>
        public Banker(BankerMode mode)
        {
            this.Mode = mode;
        }

        /// <summary>
        /// Computes the banker's offer based on the banker's mode.
        /// </summary>
        /// <param name="valuesLeft">An array of integer values containing the values of the cases left unopened.</param>
        /// <returns>The banker's offer.</returns>
        public double ComputeOffer(List<int> valuesLeft)
        {
            int largest = valuesLeft.Max();
            int smallest = valuesLeft.Min();
            double arithmetic_mean = valuesLeft.Average();
            int num_cases_above_mean = valuesLeft.Count(x => x > arithmetic_mean);
            int num_cases_below_mean = valuesLeft.Count(x => x < arithmetic_mean);
            double offer = 0;

            switch(this.Mode)
            {
                case BankerMode.Generous: offer = arithmetic_mean + (largest - arithmetic_mean) * (num_cases_above_mean / valuesLeft.Count);
                    break;
                case BankerMode.Robotic: offer = arithmetic_mean;
                    break;
                case BankerMode.Merciless: offer = arithmetic_mean - (arithmetic_mean - smallest) * (num_cases_below_mean / valuesLeft.Count);
                    break;
            }

            return offer;
        }
    }
}
