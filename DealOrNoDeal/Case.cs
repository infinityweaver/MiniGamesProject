using System;
using System.Collections.Generic;
using System.Text;

namespace MiniGamesProject.DealOrNoDeal
{
    class Case
    {
        public int Value { get; private set; }
        public Currency Currency { get; set; }

        /// <summary>
        /// Case Constructor
        /// </summary>
        /// <param name="value">The value of the brief case.</param>
        /// <param name="currency">The currency used in relation to the value of the brief case.</param>
        public Case(int value, Currency currency)
        {
            this.Value = value;
            this.Currency = currency;
        }
    }
}
