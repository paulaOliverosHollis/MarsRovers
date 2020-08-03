using NUnit.Framework;
using MarsRovers;
using static MarsRovers.RoverPosition;

namespace RoverUnitTests
{
    public class RoverUnitTests : Rover
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("-1 -5")]
        [TestCase("3")]
        [TestCase("U Y")]
        [TestCase("1 7 2")]
        [TestCase("22")]
        public void IsGridBoundsValid_WithInvalidInput_ReturnsFalseAndZero(string input)
        {
            int x, y;

            bool result = IsGridBoundsValid(input, out x, out y);

            Assert.AreEqual(0, x);
            Assert.AreEqual(0, y);
            Assert.IsFalse(result);
        }

        [TestCase("1 2", 1, 2)]
        [TestCase("15 9", 15, 9)]
        [TestCase("0 0", 0, 0)]
        [TestCase("5 5", 5, 5)]
        [TestCase("200 200", 200, 200)]
        [TestCase("1567 2987", 1567, 2987)]
        public void IsGridBoundsValid_WithValidInput_ReturnsTrueaAndNotZero(string input, int expectedX, int expectedY)
        {
            int actualX, actualY;

            bool result = IsGridBoundsValid(input, out actualX, out actualY);

            Assert.AreEqual(expectedX, actualX);
            Assert.AreEqual(expectedY, actualY);
            Assert.IsTrue(result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("-2 -4 N")]
        [TestCase("1 2 5 S")]
        [TestCase("4 5W")]
        [TestCase("11 8 N")]
        [TestCase("9 8 W")] // Outside of y bound
        [TestCase("7 9 S")] // Outside of x bound
        public void IsStartingPositionValid_WithInvalidInput_ReturnsFalseAndNull(string input)
        {
            _xBound = 9;
            _yBound = 6;
            RoverPosition output;

            bool result = IsStartingPositionValid(input, out output);

            Assert.IsFalse(result);
            Assert.IsNull(output);
        }

        [TestCase("1 2 N", 1, 2, CardinalPoint.N)]
        [TestCase("3 3 E", 3, 3, CardinalPoint.E)]
        [TestCase("3 1 W", 3, 1, CardinalPoint.W)]
        [TestCase("5 6 N", 5, 6, CardinalPoint.N)]
        [TestCase("0 0 E", 0, 0, CardinalPoint.E)]
        public void IsStartingPositionValid_WithValidInput_ReturnsTrueAndRoverPositionObject(string input, int expectedX, int expectedY, CardinalPoint expectedDirection)
        {
            _xBound = 5;
            _yBound = 6;
            RoverPosition output;

            bool result = IsStartingPositionValid(input, out output);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedX, output.X);
            Assert.AreEqual(expectedY, output.Y);
            Assert.AreEqual(expectedDirection, output.Direction);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        [TestCase("LMLMUM")]
        [TestCase("RLMRMLT")]
        [TestCase("mlmlmly")]
        [TestCase("L M L R L ")]
        public void IsInstructionsValid_WithInvalidInput_ReturnsFalseAndInstructionsIsNull(string input)
        {
            bool result = IsInstructionsValid(input);

            Assert.IsFalse(result);
            Assert.IsNull(_navigationInstructions);
        }

        [TestCase("L")]
        [TestCase("M")]
        [TestCase("R")]
        [TestCase("lmrmm")]
        [TestCase("rMrLLm")]
        public void IsInstructionsValid_WithValidInput_ReturnsTrue(string input)
        {
            Assert.IsTrue(IsInstructionsValid(input));
        }

        [TestCase(11, 13)]
        [TestCase(10, 14)] // Outside of y bound
        [TestCase(20, 12)] // Outside of x bound
        [TestCase(-3, 12)]
        [TestCase(10, -1)]
        public void IsCurrentPositionValid_WithInvalidInput_ReturnsFalse(int x, int y)
        {
            _xBound = 10;
            _yBound = 12;
            _currentPosition = new RoverPosition(x, y, CardinalPoint.N); // CardinalPoint is hardcoded since its value is not relevant to a rover being out or within the bounds.

            bool result = IsCurrentPositionValid();

            Assert.IsFalse(result);
        }

        [TestCase(10, 12)]
        [TestCase(8, 12)]
        [TestCase(10, 6)]
        [TestCase(0, 0)]
        [TestCase(0, 11)]
        public void IsCurrentPositionValid_WithValidInput_ReturnsTrue(int x, int y)
        {
            _xBound = 10;
            _yBound = 12;
            _currentPosition = new RoverPosition(x, y, CardinalPoint.N);

            bool result = IsCurrentPositionValid();

            Assert.IsTrue(result);
        }
    }
}