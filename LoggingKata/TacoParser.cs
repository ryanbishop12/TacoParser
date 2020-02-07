namespace LoggingKata
{
    /// <summary>
    /// Parses a POI file to locate all the Taco Bells
    /// </summary>
    public class TacoParser
    {
        readonly ILog logger = new TacoLogger();
        
        public ITrackable Parse(string line)
        {
            string[] cells = line.Split(',');

            if(cells.Length < 3)
            {
                logger.LogInfo("Invalid Data");
                return null;
            }

            Point loc = new Point() { Latitude = double.Parse(cells[0]), Longitude = double.Parse(cells[1]) };
            ITrackable parsed = new TacoBell() {Location = loc, Name = cells[2] };
            return parsed;
        }
    }
}