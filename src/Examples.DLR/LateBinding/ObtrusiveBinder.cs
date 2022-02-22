using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Examples.DLR.LateBinding
{
    internal class ObtrusiveBinder : CallSiteBinder
    {
        public override Expression Bind(
            object[] args,
            ReadOnlyCollection<ParameterExpression> parameters,
            LabelTarget returnLabel)
        {
            Console.WriteLine("Called ObtusiveBinder.Bind() Method.");

            // NOT return Expression.Constant("I'm ObtrusiveBinder!");
            return Expression.Return(returnLabel,
                Expression.Constant("I'm ObtrusiveBinder!"));
        }
    }
}
