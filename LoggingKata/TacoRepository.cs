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
            List<TacoBell> bells = _connection.Query<TacoBell>("SELECT * FROM tacobells;").ToList();
            List<Point> points = _connection.Query<Point>("SELECT * FROM tacobells;").ToList();
            for(int i = 0; i < bells.Count; i++)
            {
                bells[i].Location = points[i];
            }
            return bells;
        }

        public void InsertTacoBell(double latitude, double longitude, string taconame)
        {
            _connection.Execute("INSERT INTO tacobells (latitude, longitude, name) VALUES (@lat, @lon, @name)", new { lat = latitude, lon = longitude, name = taconame });
        }
    }
}
