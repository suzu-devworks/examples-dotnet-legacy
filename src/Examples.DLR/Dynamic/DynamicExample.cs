using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Examples.DLR.Dynamic
{
    internal class DynamicExample
    {

        public static void DoDynamicBook()
        {
            dynamic book = new Book("Practical DLR.", 256);

            string title = book.Title;
            Console.WriteLine($"Title: {title}");

            string invalidProperty = book.InvalidProperty;
            Console.WriteLine($"InvalidProperty: {invalidProperty}");

            string invalidMethod = book.InvalidMethod();
            Console.WriteLine($"InvalidMethod: {invalidMethod}");

            return;
        }

        public static void DoDynamicBook2()
        {
            dynamic book = new Book2("Practical DLR.", 256);

            string title = book.Title;
            Console.WriteLine($"Title: {title}");

            string invalidProperty = book.InvalidProperty;
            Console.WriteLine($"InvalidProperty: {invalidProperty}");

            string invalidMethod = book.InvalidMethod();
            Console.WriteLine($"InvalidMethod: {invalidMethod}");

            return;
        }

        public static void DoUseExpandoObject()
        {
            dynamic parson = new ExpandoObject();
            parson.FirstName= "Foo";
            parson.LastName = "Bar";

            Console.WriteLine($"{parson.FirstName}.{parson.LastName}");

            return;
        }

        public static void DoModifyProperty()
        {
            dynamic parson = new ExpandoObject();
            parson.FirstName= "Foo";
            Console.WriteLine($"FirstName[a] => Type: {parson.FirstName.GetType()}, Value: {parson.FirstName}");

            parson.FirstName= 102;
            Console.WriteLine($"FirstName[b] => Type: {parson.FirstName.GetType()}, Value: {parson.FirstName}");

            parson.FirstName= DateTimeOffset.Now;
            Console.WriteLine($"FirstName[c] => Type: {parson.FirstName.GetType()}, Value: {parson.FirstName}");

            return;
        }

        public static void DoRemoveProperty()
        {
            dynamic parson = new ExpandoObject();
            parson.FirstName= "Foo";
            parson.LastName = "Bar";

            Console.WriteLine("--- before ---");
            // ExpandoObject as IEnumerable<KeyValuePair<string, object>>.
            foreach (KeyValuePair<string, object> pair in parson)
            {
                Console.WriteLine($"key: {pair.Key}, Type: {pair.Value.GetType()}. Value: {pair.Value}");
            }

            // ExpandoObject as IDictionary<string, object>.
            ((IDictionary<string, object>)parson).Remove("FirstName");

            Console.WriteLine("--- after ---");
            // ExpandoObject as IEnumerable<KeyValuePair<string, object>>.
            foreach (KeyValuePair<string, object> pair in parson)
            {
                Console.WriteLine($"key: {pair.Key}, Type: {pair.Value.GetType()}. Value: {pair.Value}");
            }

            return;
        }

        public static void DoAddMethod()
        {
            dynamic parson = new ExpandoObject();
            parson.DateOfBirth = DateTime.Parse("2000-02-29");
            parson.GetAge = (Func<int>)(() =>
            {
                int result = 0;
                DateTime now = DateTime.Now;
                result = now.Year - parson.DateOfBirth.Year;

                if (now.Month < parson.DateOfBirth.Month ||
                    (now.Month == parson.DateOfBirth.Month &&
                        now.Day < parson.DateOfBirth.Day))
                {
                    result--;
                }

                return result;
            });

            int age = parson.GetAge();
            Console.WriteLine($"GetAge: {age}.");

            return;
        }

    }
}
