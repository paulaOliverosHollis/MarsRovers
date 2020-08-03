using System;
using static MarsRovers.RoverPosition;

namespace MarsRovers
{
    public class Rover
    {
        protected int _xBound;
        protected int _yBound;
        protected RoverPosition _currentPosition;
        protected string _navigationInstructions;

        /// <summary>
        /// Initiates a rover's exploration session. 
        /// </summary>
        public void Explore()
        {
            SetGridBounds();

            // For a normal console program, this loop would be terminated by a menu selection. 
            // However, I've decided to use this while true loop to allow  
            // ease of use and easier testing for the purpose of the coding test.
            while (true)
            {
                SetStartingPosition();

                SetNavigationInstructions();

                NavigatePlateau();

                Console.WriteLine($"\n\n\tRover's current position: {_currentPosition.X} {_currentPosition.Y} {_currentPosition.Direction.ToString()}");
            }
        }

        /// <summary>
        /// Establishes the exploration grid bounds by getting the coordinates corresponding to the northeast corner of the plateau from the user. 
        /// </summary>
        protected void SetGridBounds()
        {
            Console.Write("\n\n\tPlease enter the exploration grid bounds (X Y): ");
            string gridBounds = Console.ReadLine();

            int xBound, yBound;

            while (!IsGridBoundsValid(gridBounds, out xBound, out yBound))
            {
                Console.Write("\n\n\tThe exploration grid bounds you entered are not valid. Please try again: ");
                gridBounds = Console.ReadLine();
            }

            _xBound = xBound;
            _yBound = yBound;
        }

        /// <summary>
        /// Establishes the rover's starting position by prompting the user to confirm rover's X and Y coordinates and heading direction.
        /// </summary>
        protected void SetStartingPosition()
        {
            Console.Write("\n\n\tConfirm Rover's current position (X Y D): ");
            string input = Console.ReadLine();

            RoverPosition position;
            while (!IsStartingPositionValid(input, out position))
            {
                Console.Write("\n\n\tThe coordinates you entered are not valid. Please try again: ");
                input = Console.ReadLine();
            }

            _currentPosition = position;
        }

        /// <summary>
        /// Establishes rover's navigation instructions according to the user's input.
        /// </summary>
        protected void SetNavigationInstructions()
        {
            Console.Write("\n\n\tNavigation Instructions: ");
            string input = Console.ReadLine();

            while (!IsInstructionsValid(input))
            {
                Console.Write("\n\n\tThe instructions you entered are not valid. Please try again: ");
                input = Console.ReadLine();
            }

            _navigationInstructions = input;
        }
        /// <summary>
        /// Verifies that rover's current position is within the bounds of the exploration grid.
        /// </summary>
        /// <returns></returns>
        protected bool IsCurrentPositionValid()
        {
            return
                _currentPosition.X <= _xBound &&
                _currentPosition.X >= 0 &&
                _currentPosition.Y <= _yBound &&
                _currentPosition.Y >= 0;
        }

        /// <summary>
        /// Rover navigates the exploration grid according to user's instructions.
        /// </summary>
        protected void NavigatePlateau()
        {
            foreach (char letter in _navigationInstructions)
            {
                char command = char.ToUpper(letter);

                if (command == 'M')
                {
                    if (_currentPosition.Direction == CardinalPoint.N)
                    {
                        _currentPosition.Y++;
                    }
                    else if (_currentPosition.Direction == CardinalPoint.E)
                    {
                        _currentPosition.X++;
                    }
                    else if (_currentPosition.Direction == CardinalPoint.S)
                    {
                        _currentPosition.Y--;
                    }
                    else if (_currentPosition.Direction == CardinalPoint.W)
                    {
                        _currentPosition.X--;
                    }

                    if (!IsCurrentPositionValid())
                    {
                        Console.Write("\n\n\tOh no! Your Rover fell off the Plateau. The instructions you gave it made it go outside the exploration grid.");
                        return;
                    }
                }
                else if (command == 'R')
                {
                    if (_currentPosition.Direction == CardinalPoint.W)
                    {
                        _currentPosition.Direction = CardinalPoint.N;
                    }
                    else
                    {
                        _currentPosition.Direction++;
                    }
                }
                else if (command == 'L')
                {
                    if (_currentPosition.Direction == CardinalPoint.N)
                    {
                        _currentPosition.Direction = CardinalPoint.W;
                    }
                    else
                    {
                        _currentPosition.Direction--;
                    }
                }
            }
        }

        /// <summary>
        /// Ensures input is a positive integer. Returns validated X and Y bound coordinates.
        /// </summary>
        protected static bool IsGridBoundsValid(string gridBounds, out int xBound, out int yBound)
        {
            xBound = yBound = 0;

            if (string.IsNullOrEmpty(gridBounds))
            {
                return false;
            }

            string[] temp = gridBounds.Split(" ");
            if (temp.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(temp[0], out int x) || !int.TryParse(temp[1], out int y))
            {
                return false;
            }

            if (x < 0 || y < 0)
            {
                return false;
            }

            xBound = x;
            yBound = y;
            return true;
        }

        /// <summary>
        /// Ensures input is a positive integer and that it is within the grid bounds. Returns a X coordinate.
        /// </summary>
        protected bool IsXCoordinateValid(string input, out int x)
        {
            return
                int.TryParse(input, out x) &&
                x <= _xBound &&
                x >= 0;
        }

        /// <summary>
        /// Ensures input is a positive integer and that it is within the grid bounds. Returns a Y coordinate.
        /// </summary>
        protected bool IsYCoordinateValid(string input, out int y)
        {
            return
                int.TryParse(input, out y) &&
                y <= _yBound &&
                y >= 0;
        }

        /// <summary>
        /// Ensures input is a valid cardinal point. Returns rover's heading direction.
        /// </summary>
        protected static bool IsDirectionValid(string input, out CardinalPoint direction)
        {
            direction = 0;

            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            char directionLetter = char.ToUpper(input[0]);

            switch (directionLetter)
            {
                case 'N':
                    direction = CardinalPoint.N;
                    break;
                case 'E':
                    direction = CardinalPoint.E;
                    break;
                case 'S':
                    direction = CardinalPoint.S;
                    break;
                case 'W':
                    direction = CardinalPoint.W;
                    break;
                default:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Ensures input is a valid rover position with X and Y coordinates and a cardinal compass point in which the rover is facing.
        /// </summary>
        protected bool IsStartingPositionValid(string input, out RoverPosition position)
        {
            position = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string[] splitInput = input.Split(" ");
            if (splitInput.Length != 3)
            {
                return false;
            }

            if (IsXCoordinateValid(splitInput[0], out int x) &&
                IsYCoordinateValid(splitInput[1], out int y) &&
                IsDirectionValid(splitInput[2], out CardinalPoint direction))
            {
                position = new RoverPosition(x, y, direction);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ensures input is a valid sequence of L, R, and M instructions.
        /// </summary>
        protected static bool IsInstructionsValid(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            foreach (char letter in input)
            {
                char command = char.ToUpper(letter);

                if (command != 'M' && command != 'L' && command != 'R')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
