using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingKata
{
    interface ITacoRepository
    {
        void InsertTacoBell();
        IEnumerable<TacoBell> GetTacoBells();
    }
}
