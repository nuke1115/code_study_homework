using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw3
{
    public static class ArrayUtils
    {
        public static bool ResizeArray<T>(ref T[] oldArray, int size)
        {
            if (oldArray is null || size < 0)
            {
                return false;
            }

            if (oldArray.Length == size)
            {
                return true;
            }

            T[] newArray = new T[size];

            for(int idx = 0; idx < oldArray.Length && idx < newArray.Length; idx++)
            {
                newArray[idx] = oldArray[idx];
            }

            oldArray = newArray;

            //Array.Resize나 Array.Copy같은 함수가 이미 있는데, 학습 목적이니까 몇번은 직접 구현해보는것도 괜찮지

            return true;
        }
    }
}
