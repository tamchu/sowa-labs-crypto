using System;
using System.Collections.Generic;
using System.Text;

namespace SowaLabsOrderBooks.Models
{
    public class Balance
    {
       /* private decimal? _eur;

        public decimal? Eur
        {
            get { return _eur; }
            set 
            { 
                _eur = value;
                if (_eur == null)
                    _eur = new Random().Next(3000, 3500);
            }
        }

        private decimal? _btc;

        public decimal? Btc
        {
            get { return _btc; }
            set 
            {
                _btc = value; 
                if (_btc == null)
                    _btc = (decimal)new Random().NextDouble();
            }
        }*/

        public decimal Eur { get; set; }
        public decimal Btc { get; set; }
    }
}
