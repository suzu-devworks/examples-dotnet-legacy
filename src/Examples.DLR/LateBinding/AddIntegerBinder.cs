using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Examples.DLR.LateBinding
{
    internal class AddIntegerBinder : CallSiteBinder
    {
        public override Expression Bind(
            object[] args,
            ReadOnlyCollection<ParameterExpression> parameters,
            LabelTarget returnLabel)
        {
            Dump(args);
            Dump(parameters);

            return Expression.Return(returnLabel,
                Expression.Add(
                    parameters[0],
                    parameters[1]));
        }

        private static void Dump(object[] args)
        {
            Console.WriteLine("--- args ---");
            foreach (object o in args)
            {
                Console.WriteLine($"Type: {o.GetType()}, Value: {o}");
            }
        }

        private static void Dump(ReadOnlyCollection<ParameterExpression> parameters)
        {
            Console.WriteLine("--- parameters ---");
            foreach (ParameterExpression p in parameters)
            {
                Console.WriteLine($"Name: {p.Name}, NodeType: {p.NodeType}");
            }
        }

    }
}
