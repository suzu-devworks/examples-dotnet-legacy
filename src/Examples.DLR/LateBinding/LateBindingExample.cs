using System;
using System.Runtime.CompilerServices;
using Microsoft.Scripting.Hosting;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;
using CSharpArgumentInfo = Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo;
using CSharpArgumentInfoFlags = Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags;
using CSharpBinderFlags = Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags;

namespace Examples.DLR.LateBinding
{
    internal class LateBindingExample
    {

        public static void DoBehindDynamicBinding()
        {
            #region dynamic person = new Parson
            object person = new Parson
            #endregion
            {
                FirstName = "Foo",
                LastName = "Bar",
                DateOfBitrh = DateTime.Parse("2000-02-29")
            };

            #region int age = person.GetAge(DateTime.Parse("2020-02-28"));

            CallSite<Func<CallSite, object, int>> callSite0 =
                CallSite<Func<CallSite, object, int>>.Create(
                    Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(LateBindingExample)));

            CallSite<Func<CallSite, object, DateTime, object>> callSite1 =
                CallSite<Func<CallSite, object, DateTime, object>>.Create(
                    Binder.InvokeMember(CSharpBinderFlags.None,
                        "GetAge",
                        null,
                        typeof(LateBindingExample),
                        new CSharpArgumentInfo[2]
                        {
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                        }));

            int age = callSite0.Target(callSite0, callSite1.Target(callSite1, person, DateTime.Parse("2020-02-28")));
            #endregion

            Console.WriteLine($"age = {age}");

            return;
        }


        public static void DoCompanyBindingFromFile()
        {
            ScriptEngine engine = IronPython.Hosting.Python.CreateEngine();
            ScriptScope scope = engine.ExecuteFile(@"LateBinding/company.py");

            dynamic company1 = scope.GetVariable("company1");
            string companyName = company1.Name;
            int companyCapital = company1.Capital;

            Console.WriteLine($"companyName: {companyName}");
            Console.WriteLine($"companyCapital: {companyCapital}");

            return;
        }


        public static void DoUseObtrusiveBinder()
        {
            ObtrusiveBinder binder = new ObtrusiveBinder();
            Console.WriteLine("Complete create Obtrusiveinder.");

            CallSite<Func<CallSite, string>> callSite =
                CallSite<Func<CallSite, string>>.Create(binder);
            Console.WriteLine("Complete create CallSite.");

            string result = callSite.Target(callSite);
            Console.WriteLine("Complete Call Target().");

            Console.WriteLine($"result = {result}");

            return;
        }


        public static void DoUseObtrusiveBinderForCheckCache()
        {
            ObtrusiveBinder binder = new ObtrusiveBinder();
            Console.WriteLine("Complete create Obtrusiveinder.");

            CallSite<Func<CallSite, string>> callSite =
                CallSite<Func<CallSite, string>>.Create(binder);
            Console.WriteLine("Complete create CallSite.");

            string result = "";

            result = callSite.Target(callSite);
            Console.WriteLine("[ 1st time ] Complete Call Target().");
            Console.WriteLine($"[ 1st time ] result = {result}");

            result = callSite.Target(callSite);
            Console.WriteLine("[ 2nd time ] Complete Call Target().");
            Console.WriteLine($"[ 2nd time ] result = {result}");

            return;
        }

        public static void DoUseAddIntegerBinder()
        {
            AddIntegerBinder binder = new AddIntegerBinder();
            CallSite<Func<CallSite, int, int, int>> callSite =
                CallSite<Func<CallSite, int, int, int>>.Create(binder);

            int result = callSite.Target(callSite, 10, 20);
            Console.WriteLine($"result = {result}");

            return;
        }

    }
}
