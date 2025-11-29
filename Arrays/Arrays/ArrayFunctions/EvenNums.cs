namespace Arrays.ArrayFunctions
{
    public class ArrayUtils
    {
        public int[] EvenNumbers(int[] nums)
        {
            List<int> res = new List<int>();

            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] % 2 == 0)
                {
                    res.Add(nums[i]);
                }
            }

            return res.ToArray();
        }
    }
}
