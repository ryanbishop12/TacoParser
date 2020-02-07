using System;
using Xunit;

namespace LoggingKata.Test
{
    public class TacoParserTests
    {
        [Fact]
        public void ShouldDoSomething()
        {
            //Act
            Point location = new Point() { Latitude = 34.252214, Longitude = -84.088239};
            ITrackable expected = new TacoBell() { Name = "Taco Bell Cummin...", Location = location };

            //Arrange
            TacoParser parser = new TacoParser();
            ITrackable actual = parser.Parse("34.252214,84.088239,Taco Bell Cummin...");
            //Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Location.Latitude, actual.Location.Latitude);
            Assert.Equal(expected.Location.Longitude, actual.Location.Longitude);
        }

        [Theory]
        [InlineData("34.764965,-86.48607,Taco Bell Huntsville")]
        [InlineData("34.764965,-86.48607,Taco Bell Huntsville")]
        public void ShouldParse(string str)
        {
            //Act
            Point location = new Point() { Latitude = 34.764965 , Longitude = -86.48607};
            ITrackable expected = new TacoBell() { Name = "Taco Bell Huntsville", Location = location};

            //Arrange
            TacoParser parser = new TacoParser();
            ITrackable actual = parser.Parse(str);
            //Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Location.Latitude, actual.Location.Latitude);
            Assert.Equal(expected.Location.Longitude, actual.Location.Longitude);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldFailParse(string str)
        {
            ITrackable expected = null;
            //Arrange
            TacoParser parser = new TacoParser();
            ITrackable actual = parser.Parse(str);
            //Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Location.Latitude, actual.Location.Latitude);
            Assert.Equal(expected.Location.Longitude, actual.Location.Longitude);

        }
    }
}
