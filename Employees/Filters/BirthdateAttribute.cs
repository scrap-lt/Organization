using System;
using System.ComponentModel.DataAnnotations;

namespace Employees.Filters
{
    public class BirthdateAttribute : RangeAttribute
    {
        private const string min = "1.1.1940";
        private const string max = "31.12.2005";
        private const string errorMessage = "Можно выбрать дату от 01.01.1940 до 31.12.2005";

        public BirthdateAttribute()
            : base(typeof(DateTime), min, max)
        { }

        public override string FormatErrorMessage(string name)
        {
            return errorMessage;
        }
    }
}
