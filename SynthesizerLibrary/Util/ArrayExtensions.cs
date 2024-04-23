namespace SynthesizerLibrary.Util;

public static class ArrayExtensions
{
    // Extension method to mimic JavaScript's slice method for arrays
    public static T[] Slice<T>(this T[] source, int start, int end)
    {
        // Handle negative indices
        if (start < 0)
        {
            start = source.Length + start;
        }
        if (end < 0)
        {
            end = source.Length + end;
        }

        // Adjust the end index if it is out of bounds
        if (end > source.Length)
        {
            end = source.Length;
        }

        // Calculate the length of the new array
        int length = end - start;

        // If the calculated length is negative, return an empty array
        if (length <= 0)
        {
            return new T[0];
        }

        // Create a new array of the calculated length
        T[] result = new T[length];

        // Copy the elements from the source array to the result array
        Array.Copy(source, start, result, 0, length);

        return result;
    }
}