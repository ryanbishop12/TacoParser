using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingKata
{
    interface ITacoRepository
    {
        void InsertTacoBell(double latitude, double longitude, string taconame);
        IEnumerable<TacoBell> GetTacoBells();
    }
}
