using System.Dynamic;

namespace Examples.DLR.Dynamic
{
    internal class Book2 : DynamicObject
    {
        public Book2(string title, int pageCount)
        {
            this.Title = title;
            this.PageCount = pageCount;
        }

        public string Title { get; }

        public int PageCount { get; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = "TryGetMember.";

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = "TryInvokeMember.";

            return true;
        }

    }
}
