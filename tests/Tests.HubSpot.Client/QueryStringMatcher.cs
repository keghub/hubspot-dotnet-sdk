using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Tests
{
    public static class QueryStringMatcher
    {
        public static IQueryString That(IResolveConstraint constraint)
        {
            return Match.Create<IQueryString>(qs =>
            {
                try
                { 
                    Assert.That(qs.Query, constraint);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public static IQueryString HasItems()
        {
            return Match.Create<IQueryString>(qs => 
            {
                try
                {
                    Assert.That(qs.HasItems, Is.True);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}