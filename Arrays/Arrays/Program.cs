using Arrays.ArrayFunctions;
public class Program
{
    public static void Main(string[] args)
    {
        // Declare and initialize an array of integers
        int[] numbers = new int[] { 1, 2, 3, 4, 5 };
        int target = 3;
        // Call the FindTargetFunction to search for the target in the array
        FindTarget.FindTargetLinear(numbers, target);
        FindTarget.FindTargetBinarySearch(numbers, target);

        int[] result = FindTarget.twoSum(numbers, 9);
        Console.WriteLine($"Two Sum Result Indices: [{string.Join(", ", result)}]");

    }
}
