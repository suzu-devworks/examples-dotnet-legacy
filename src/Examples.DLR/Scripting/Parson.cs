namespace Examples.DLR.Scripting
{
    public class Parson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName()
        {
            return LastName + " " + FirstName;
        }

    }
}
