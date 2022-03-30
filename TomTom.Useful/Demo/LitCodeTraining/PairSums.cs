using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LitCodeTraining
{
    public class PairSums
    {
        [Theory]
        [InlineData(new int[] { 3, 4, 1, 6, 2 }, new int[] { 1, 3, 1, 5, 1 })]
        public void TestCountSubarrays(int[] input, int[] expectedOutput)
        {
            // act
            var actualOutput = countSubarrays(input);

            // assert
            Assert.Equal(expectedOutput, actualOutput);
        }

        private static int[] countSubarrays(int[] arr)
        {

            // Write your code here
            return new int[0];
        }



        class AverageSubarrayCacheItem
        {
            public int Count { get; set; }
            public int Average { get; set; }
            public int Total => Count * Average;
        }

        Dictionary<(int, int), AverageSubarrayCacheItem> cache = new Dictionary<(int, int), AverageSubarrayCacheItem>();

        private AverageSubarrayCacheItem Add(AverageSubarrayCacheItem a, AverageSubarrayCacheItem b)
        {
            var newCount = a.Count + b.Count;
            var newAverage = a.Total + b.Total / (newCount);
            return new AverageSubarrayCacheItem
            {
                Count = newCount,
                Average = newAverage
            };
        }

        AverageSubarrayCacheItem GetSubArrayAverage(int start, int end, int[] values)
        {
            if (cache.TryGetValue((start, end), out var val))
            {
                return val;
            }

            AverageSubarrayCacheItem result;

            if (start == end)
            {
                result = new AverageSubarrayCacheItem
                {
                    Average = values[start],
                    Count = 1
                };
            }
            else
            {
                var left = GetSubArrayAverage(start, end - 1, values);
                var right = GetSubArrayAverage(end, end, values);
                result = Add(left, right);
            }

            cache.Add((start, end), result);

            return result;
        }

        bool IsAverageLessThenRest(int start, int end, int[] values)
        {
            var left = GetSubArrayAverage(0, start - 1, values);
            var right = GetSubArrayAverage(end + 1, values.Length, values);

            var outsideAverage = Add(left, right);
            var thisAverage = GetSubArrayAverage(start, end, values);

            if (thisAverage.Average < outsideAverage.Average)
            {
                return true;
            }

            return false;
        }

        List<(int, int)> Combine(int[] values)
        {
            var result = new List<(int, int)>();
            for (var start = 0; start < values.Length; start++)
            {
                for(var end = start; end < values.Length; end++)
                {
                    if(IsAverageLessThenRest(start, end, values))
                    {
                        result.Add((start, end));
                    }
                }
            }
            return result;
        }
    }
}
