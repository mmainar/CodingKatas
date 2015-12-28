using System;
using NUnit.Framework;

namespace BowlingGameKata
{
    [TestFixture]
    public class BowlingGameTests
    {
        private BowlingGame game;
        private int value;

        [SetUp]
        public void Setup()
        {
            game = new BowlingGame();
        }

        private void RollMany(int n, int pins)
        {
            for (int i = 0; i < n; i++)
                game.Roll(pins);
        }

        private void RollStrike()
        {
            game.Roll(10);
        }

        private void RollSpare()
        {
            game.Roll(5);
            game.Roll(5);
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Roll_MoreThanTenPins_ThrowsException()
        {
            game.Roll(11);
        }

        [TestCase]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Roll_LessThanZeroPins_ThrowsException()
        {
            game.Roll(-4);
        }

        [TestCase]
        public void GutterGame()
        {
            RollMany(20, 0);
            Assert.AreEqual(0, game.Score());
        }

        [TestCase]
        public void AllOnes()
        {
            RollMany(20, 1);
            Assert.AreEqual(20, game.Score());
        }

        [TestCase]
        public void OneStrike_RestZeros_Game()
        {
            RollStrike();
            RollMany(18, 0);
            Assert.AreEqual(10, game.Score());
        }

        [TestCase]
        public void OneStrike_FollowedByOnePinDown_FollowedByZeros_Game()
        {
            RollStrike();
            game.Roll(1);
            RollMany(16, 0);
            Assert.AreEqual(12, game.Score());
        }

        [TestCase]
        public void OneSpare()
        {
            RollSpare();
            // After a spare we roll one more ball
            game.Roll(3);
            RollMany(17, 0);
            Assert.AreEqual(16, game.Score());
        }

        [TestCase]
        public void OneStrike()
        {
            RollStrike();

            // After a strike we roll two more balls
            game.Roll(3);
            game.Roll(6);

            RollMany(16, 0);
            Assert.AreEqual(28, game.Score());
        }

        [TestCase]
        public void PerfectGame()
        {
            RollMany(12, 10);
            Assert.AreEqual(300, game.Score());
        }
    }

    public class BowlingGame
    {
        private int[] rolls = new int[21];
        private int currentRoll = 0;

        public void Roll(int pins)
        {
            if (pins < 0 || pins > 10)
                throw new ArgumentOutOfRangeException("pins");

            rolls[currentRoll++] = pins;
        }

        public int Score()
        {
            int score = 0;
            int firstInFrame = 0;

            for (int frame = 1; frame <= 10; frame++)
            {
                if (IsStrike(firstInFrame)) // Strike 
                {
                    score += 10 + NextTwoBallsForStrike(firstInFrame);
                    firstInFrame++;
                }
                else if (IsSpare(firstInFrame)) // Spare 
                {
                    score += 10 + NextBallForSpare(firstInFrame);
                    firstInFrame += 2;
                }
                else // Open
                {
                    score += TwoBallsInFrame(firstInFrame);
                    firstInFrame += 2;
                }
            }

            return score;
        }

        private int TwoBallsInFrame(int firstInFrame)
        {
            return rolls[firstInFrame] + rolls[firstInFrame + 1];
        }

        private int NextTwoBallsForStrike(int firstInFrame)
        {
            return rolls[firstInFrame + 1] + rolls[firstInFrame + 2];
        }

        private int NextBallForSpare(int firstInFrame)
        {
            return rolls[firstInFrame + 2];
        }

        private bool IsStrike(int firstInFrame)
        {
            return rolls[firstInFrame] == 10;
        }

        private bool IsSpare(int firstInFrame)
        {
            return rolls[firstInFrame] + rolls[firstInFrame + 1] == 10;
        }
    }


}
