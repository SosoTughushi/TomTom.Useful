using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.DataTypes
{
    public class Either<T1, T2>
    {
        private bool isLeft;
        public T1 Left { get; }

        public T2 Right { get; }

        public Either(T1 left)
        {
            this.Left = left;
            this.isLeft = true;
        }

        public Either(T2 right)
        {
            this.Right = right;
            this.isLeft = false;
        }

        public TResult Match<TResult>(Func<T1, TResult> leftMatch, Func<T2, TResult> rightMatch)
        {
            if (isLeft)
            {
                return leftMatch(Left);
            }

            return rightMatch(Right);
        }
    }
}
