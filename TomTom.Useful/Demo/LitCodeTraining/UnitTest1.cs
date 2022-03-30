using System;
using System.Collections.Generic;
using Xunit;

namespace LitCodeTraining
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // arrange
            var input = new[] { 1, 2, 3, 4, 5 };
            var expectedOutput = new[] { -1, -1, 6, 24, 60 };

            // act
            var result = findMaxProduct(input);

            // assert
            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void Test2()
        {
            // arrange
            var input = new[] { 2, 1, 2, 1, 2 };
            var expectedOutput = new[] { -1, -1, 4, 4, 8 };

            // act
            var result = findMaxProduct(input);

            // assert
            Assert.Equal(expectedOutput, result);
        }

        private int[] findMaxProduct(int[] arr)
        {
            var heap = new MaxHeap();
            var resultArr = new int[arr.Length];
            for (var i = 0; i < arr.Length; i++)
            {
                heap.Add(arr[i]);
                resultArr[i] = heap.GetProductOf3Max();
            }

            return resultArr;
        }
    }

    class MaxHeap
    {
        public List<int> _nodes { get; } = new List<int>();

        public void Add(int item)
        {
            _nodes.Add(item);
            if (_nodes.Count == 1)
            {
                return;
            }

            var index = _nodes.Count - 1;
            while (HasParent(index) && _nodes[index] > _nodes[GetParentIndex(index)])
            {
                Swap(index, GetParentIndex(index));
                index = GetParentIndex(index);
            }
        }

        public void RemoveMax()
        {
            if (_nodes.Count == 0)
            {
                throw new InvalidOperationException("Heap is empty");
            }

            Swap(0, _nodes.Count - 1);
            _nodes.RemoveAt(_nodes.Count - 1);


            var index = 0;
            while (HasLeftChild(index))
            {
                var biggerChildIndex = GetLeftChildIndex(index);
                if (HasRightChild(index) && GetRightChildValue(index) > GetLeftChildValue(index))
                {
                    biggerChildIndex = GetRightChildIndex(index);
                }

                if (_nodes[index] > _nodes[biggerChildIndex])
                {
                    break;
                }
                Swap(index, biggerChildIndex);
                index = biggerChildIndex;
            }
        }

        public int GetMax()
        {
            return 0;
        }

        public int GetProductOf3Max()
        {
            if(_nodes.Count < 3)
            {
                return -1;
            }

            var result = _nodes[0] * _nodes[1];
            
            if(_nodes.Count == 3)
            {
                result *= _nodes[2];
            } 
            else if(_nodes.Count == 4)
            {
                result *= Math.Max(_nodes[2], _nodes[3]);
            }
            else 
            {
                result *= Math.Max(_nodes[2], Math.Max(_nodes[3], _nodes[4]));
            }

            return result;
        }

        private int GetLeftChildValue(int index) => _nodes[GetLeftChildIndex(index)];
        private int GetLeftChildIndex(int index) => index * 2;
        private bool HasLeftChild(int index) => GetLeftChildIndex(index) < _nodes.Count;

        private int GetRightChildValue(int index) => _nodes[GetRightChildIndex(index)];
        private int GetRightChildIndex(int index) => index * 2 + 1;
        private bool HasRightChild(int index) => GetRightChildIndex(index) < _nodes.Count;

        private int GetParentIndex(int index) => (index - 1) / 2;
        private bool HasParent(int index) => GetParentIndex(index) >= 0;

        private void Swap(int index1, int index2)
        {
            var temp = _nodes[index1];
            _nodes[index1] = _nodes[index2];
            _nodes[index2] = temp;
        }
    }

}