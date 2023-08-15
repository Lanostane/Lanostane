using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LST.GamePlay.Scoring
{
    //The Dream of Zero Allocation
    public static class ScoreStrings
    {
        public static readonly char[] ScoreBuffer = new char[8];
        public static readonly char[] ComboBuffer = new char[5];

        public static int ComboCharsCount { get; private set; } = 0;

        public static void SetScoreBuffer(int score)
        {
            ScoreBuffer[0] = GetDigitChar(score, 8);
            ScoreBuffer[1] = GetDigitChar(score, 7);
            ScoreBuffer[2] = GetDigitChar(score, 6);
            ScoreBuffer[3] = GetDigitChar(score, 5);
            ScoreBuffer[4] = GetDigitChar(score, 4);
            ScoreBuffer[5] = GetDigitChar(score, 3);
            ScoreBuffer[6] = GetDigitChar(score, 2);
            ScoreBuffer[7] = GetDigitChar(score, 1);
        }

        public static void SetComboBuffer(int combo)
        {
            if (combo >= 10000)
            {
                ComboBuffer[0] = GetDigitChar(combo, 5);
                ComboBuffer[1] = GetDigitChar(combo, 4);
                ComboBuffer[2] = GetDigitChar(combo, 3);
                ComboBuffer[3] = GetDigitChar(combo, 2);
                ComboBuffer[4] = GetDigitChar(combo, 1);
                ComboCharsCount = 5;
            }
            else if (combo >= 1000)
            {
                ComboBuffer[0] = GetDigitChar(combo, 4);
                ComboBuffer[1] = GetDigitChar(combo, 3);
                ComboBuffer[2] = GetDigitChar(combo, 2);
                ComboBuffer[3] = GetDigitChar(combo, 1);
                ComboCharsCount = 4;
            }
            else if (combo >= 100)
            {
                ComboBuffer[0] = GetDigitChar(combo, 3);
                ComboBuffer[1] = GetDigitChar(combo, 2);
                ComboBuffer[2] = GetDigitChar(combo, 1);
                ComboCharsCount = 3;
            }
            else if (combo >= 10)
            {
                ComboBuffer[0] = GetDigitChar(combo, 2);
                ComboBuffer[1] = GetDigitChar(combo, 1);
                ComboCharsCount = 2;
            }
            else
            {
                ComboBuffer[0] = GetDigitChar(combo, 1);
                ComboCharsCount = 1;
            }
        }

        private static char GetDigitChar(int baseNumber, int digit)
        {
            var n = Math.Pow(10, digit);
            var n2 = Math.Pow(10, digit - 1);
            var i = (int)Math.Floor((baseNumber % n) / n2);
            return i switch
            {
                0 => '0',
                1 => '1',
                2 => '2',
                3 => '3',
                4 => '4',
                5 => '5',
                6 => '6',
                7 => '7',
                8 => '8',
                9 => '9',
                _ => throw new NotImplementedException()
            };
        }
    }
}
