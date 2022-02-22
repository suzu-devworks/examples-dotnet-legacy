using System;

namespace Examples.DLR.LateBinding
{
    public class Parson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBitrh { get; set; }

        public int GetAge(DateTime baseDate)
        {
            var dx = ((DateOfBitrh.Month < baseDate.Month) ||
                      ((DateOfBitrh.Month == baseDate.Month) && (DateOfBitrh.Day < baseDate.Day)))
                      ? 1
                      : 0;
            var age = baseDate.Year - this.DateOfBitrh.Year + dx;

            return age;
        }

    }
}
