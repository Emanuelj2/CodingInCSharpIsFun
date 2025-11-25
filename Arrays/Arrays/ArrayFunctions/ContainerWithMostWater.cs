using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrays.ArrayFunctions
{
    public class ContainerWithMostWater
    {
        public static int MaxWater(int[] height)
        {
            int left = 0;
            int right = height.Length - 1;
            int maxArea = 0;

            while(left < right)
            {
                int width = right - left;
                int currentArea = Math.Min(height[left], height[right]) * width;

                maxArea = Math.Max(maxArea, currentArea);

                if (height[left] < height[right])
                {
                    left++;
                }
                else
                {
                    right--;
                }
            }
            return maxArea;
        }
    }
}
