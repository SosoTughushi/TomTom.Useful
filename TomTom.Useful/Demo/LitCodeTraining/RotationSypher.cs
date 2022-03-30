using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LitCodeTraining
{
    public class RotationSypher
    {

        [Theory]
        [InlineData("9999", 1, "0000")]
        [InlineData("zzz", 1, "aaa")]
        [InlineData("ZZZ", 1, "AAA")]
        [InlineData("Zebra-493?", 3, "Cheud-726?")]
        [InlineData("abcdefghijklmNOPQRSTUVWXYZ0123456789", 39, "nopqrstuvwxyzABCDEFGHIJKLM9012345678")]
        public void Test1(string input, int factor, string expected)
        {

            // act
            var output = rotationalCipher(input, factor);

            // assert
            Assert.Equal(expected, output);
        }
        private static string rotationalCipher(String input, int rotationFactor)
        {

            var resultStr = new String(input.Select(character => Rotate(character, rotationFactor)).ToArray());

            return resultStr;
        }

        private static char Rotate(char character, int rotationFactor)
        {
            if (InRange(character, 'a', 'z'))
            {
                return RotateInRange(character, 'a', 'z', rotationFactor);
            }

            if (InRange(character, 'A', 'Z'))
            {
                return RotateInRange(character, 'A', 'Z', rotationFactor);
            }

            if (InRange(character, '0', '9'))
            {
                return RotateInRange(character, '0', '9', rotationFactor);
            }

            return character;
        }

        private static bool InRange(char character, char from, char to)
        {
            var val = (int)character;
            if (val >= ((int)from) && val <= ((int)to))
            {
                return true;
            }
            return false;
        }

        private static char RotateInRange(char character, char from, char to, int rotationFactor)
        {
            var fromVal = (int)from;
            var range = (int)to - fromVal + 1;

            var offset = (int)character - fromVal;
            var offsetRotated = (offset + rotationFactor) % range;

            var resultVal = (char)(fromVal + offsetRotated);

            return resultVal;
        }


    }
}
