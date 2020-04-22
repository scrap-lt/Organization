using System;
using System.Collections.Generic;

using Employees.Models;

namespace Employees.Helpers
{
    public class GenderHelper
    {
        private static readonly Dictionary<byte, string> genders = new Dictionary<byte, string>()
        {
            { 1, "М" },
            { 2, "Ж" }
        };

        public static string GetValue(byte key)
        {
            if (!genders.ContainsKey(key))
            {
                throw new ArgumentException("Пол не определен", nameof(key));
            }

            string value;
            genders.TryGetValue(key, out value);

            return value;
        }

        public static IEnumerable<Gender> CreateList()
        {
            List<Gender> list = new List<Gender>();
            foreach (var kvp in genders)
            {
                list.Add(new Gender { Value = kvp.Key, Text = kvp.Value });
            }

            return list;
        }
    }
}
