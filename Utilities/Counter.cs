using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAB.Utilities
{
    public static class Counter
    {
        public static int CountNumbers(string input)
        {
            string[] nums = input.Split(' ');
            int sum = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                sum += int.Parse(nums[i]);
            }

            return sum;
        }

        public static int CountWords(string input)
        {
            return input.Length;
        }

        public static bool IsAllAreNumbers(string input)
        {
            string[] nums = input.Split(' ');

            foreach (string num in nums)
            {
                bool bl = int.TryParse(num, out int a);

                if (bl == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
