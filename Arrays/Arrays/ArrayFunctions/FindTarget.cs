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

        public static int[] twoSum(int[] nums, int target )
        {
            Dictionary<int, int> hashmap = new();

            for (int i = 0; i < nums.Length; i++)
            {
                int complement = target - nums[i];
                if (hashmap.ContainsKey(complement))
                {
                    return new int[] {hashmap[complement], i};
                }

                if(!hashmap.ContainsKey(complement))
                {
                    hashmap[nums[i]] = i;
                }
            }

            return new int[] {};
        }


    }
}
