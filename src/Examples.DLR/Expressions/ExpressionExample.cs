using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Examples.DLR.Expressions
{
    internal class ExpressionExample
    {
        public static void SayHello()
        {
            MethodInfo methodInfo =
                typeof(System.Console).GetMethod(nameof(System.Console.WriteLine), new Type[]
                {
                    typeof(string),
                });

            ConstantExpression constantExpression =
                Expression.Constant("Hello World With Expression.");

            MethodCallExpression methodCall =
                Expression.Call(null, methodInfo, constantExpression);

            Action action = Expression.Lambda<Action>(methodCall).Compile();

            action.Invoke();

            return;
        }


        public static void Calculate()
        {
            BinaryExpression exp = Expression.Add(
                Expression.Constant(2),
                Expression.Constant(3));

            Func<int> func = Expression.Lambda<Func<int>>(exp).Compile();

            int result = func();
            Console.WriteLine($"> {exp.Left} + {exp.Right} = {result}.");

            return;
        }


        public static void DoArrayAccess()
        {
            List<Expression> exps = new List<Expression>()
            {
                Expression.Constant("C#"),
                Expression.Constant("Python"),
                Expression.Constant("Ruby"),
            };

            NewArrayExpression arrayExp =
                Expression.NewArrayInit(typeof(string), exps);

            IndexExpression indexExp =
                Expression.ArrayAccess(arrayExp, Expression.Constant(2));

            Func<string> func = Expression.Lambda<Func<string>>(indexExp).Compile();

            string result = func();
            Console.WriteLine($"> string[2] = \"{result}\".");

            return;
        }


        public static void DoMemberAccess()
        {
            Computer computer = new Computer("Core i7", 8);

            MemberExpression propertyMemberException =
                Expression.Property(Expression.Constant(computer), nameof(Computer.Cpu));

            MemberExpression fieldMEmberExpression =
                Expression.Field(Expression.Constant(computer), "_memotySizeGByte");

            string cpu = Expression.Lambda<Func<string>>(propertyMemberException).Compile()();
            int memory = Expression.Lambda<Func<int>>(fieldMEmberExpression).Compile()();
            Console.WriteLine($"> Computer {{ Cpu = {cpu}, Memory = {memory} GB }}");

            return;
        }


    }
}
