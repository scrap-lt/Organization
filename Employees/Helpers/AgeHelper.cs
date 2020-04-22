using System;

namespace Employees.Helpers
{
    public class AgeHelper
    {
        public static int GetAge(DateTime birthdate)
        {
            DateTime current = DateTime.Now;
            int age = current.Year - birthdate.Year;
            if (birthdate.AddYears(age) > current)
            {
                age--;
            }

            return age;
        }

        public static int GetDecade(DateTime dateTime)
        {
            int age = GetAge(dateTime);

            return (int)Math.Truncate((double)age / 10) * 10;
        }
    }
}
