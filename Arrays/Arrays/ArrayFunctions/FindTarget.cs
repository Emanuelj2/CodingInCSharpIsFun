namespace Arrays.ArrayFunctions
{
    internal class FindTarget
    {
        public static void FindTargetLinear(int[] arr, int target)
        {
            
            for(int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == target)
                {
                    Console.WriteLine($"Target {target} found at index {i}");
                }
                
            }
        }

        public static int FindTargetBinarySearch(int[] arr, int target)
        {
            int left = 0;
            int right = arr.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                
                if (arr[mid] == target)
                    return mid;
                
                if (arr[mid] < target)
                    left = mid + 1;
                
                else
                    right = mid - 1;
            }
            
            return -1;
        }
    }
}
