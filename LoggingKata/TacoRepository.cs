using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace LoggingKata
{
    class TacoRepository : ITacoRepository
    {
        private readonly IDbConnection _connection;

        public TacoRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public IEnumerable<TacoBell> GetTacoBells()
        {
            return _connection.Query<TacoBell>("SELECT * FROM tacobells;").ToList();
        }

        public void InsertTacoBell(double latitude, double longitude, string taconame)
        {
            _connection.Execute("INSERT INTO tacobells (latitude, longitude, name) VALUES (@lat, @lon, @name)", new { lat = latitude, lon = longitude, name = taconame });
        }
    }
}
