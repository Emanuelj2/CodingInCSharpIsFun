using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Arrays.ArrayFunctions
{
    public class MaximumSubarray
    {
        public static int MaxSubarray(int[] array)
        {
            //i am goint to take a double pointer approach
            int maxSoFar = array[0];
            int maxEndingHere = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                maxEndingHere = Math.Max(array[i], maxEndingHere + array[i]);
                maxSoFar = Math.Max(maxSoFar, maxEndingHere);
            }
            return maxSoFar;
        }

        // Time Complexity: O(n)
        // Space Complexity: O(1)
        //array = { -2, 1, -3, 4, -1, 2, 1, -5, 4 }
        //explination: We iterate through the array once, updating the maximum subarray sum ending at each position and keeping track of the overall maximum found so far.
    }
}
