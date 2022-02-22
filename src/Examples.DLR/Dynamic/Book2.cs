using System.Dynamic;
using System.Linq.Expressions;

namespace Examples.DLR.Dynamic
{
    internal class Book : IDynamicMetaObjectProvider
    {
        public Book(string title, int pageCount)
        {
            this.Title = title;
            this.PageCount = pageCount;
        }

        public string Title { get; }

        public int PageCount { get; }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new FixedMetaObject(parameter, this);
        }

        public class FixedMetaObject : DynamicMetaObject
        {
            public FixedMetaObject(Expression expression, object value)
                : base(expression, BindingRestrictions.Empty, value)
            {
            }

            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                DynamicMetaObject result =
                    new DynamicMetaObject(
                        Expression.Convert(Expression.Constant("Called BindGetMember."), typeof(string)),
                        BindingRestrictions.GetExpressionRestriction(Expression.Constant(true)));

                return result;
            }

            public override DynamicMetaObject BindInvokeMember(
                InvokeMemberBinder binder,
                DynamicMetaObject[] args)
            {
                DynamicMetaObject result =
                    new DynamicMetaObject(
                        Expression.Convert(Expression.Constant("Called BindInvokeMember."), typeof(string)),
                        BindingRestrictions.GetExpressionRestriction(Expression.Constant(true)));

                return result;
            }

        }
    }
}
