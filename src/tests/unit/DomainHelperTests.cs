//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Domain;

namespace UILab.Art.Tests;

[TestFixture]
internal class DomainHelperTests
{
    internal class A : EntityType<string>
    {
        public string PropA1 { get; } = "PropertyA1";

        public string PropA2 { get; } = "PropertyA2";

        public A(string id, string? version = "1.0") : base(id, version: version)
        {
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            foreach(var component in base.GetEqualityComponents())
                yield return component;
            yield return PropA1;
            yield return PropA2;
        }
    }

    internal class B : A
    {
        public string PropB1 { get; } = "PropertyB1";

        public string PropB2 { get; } = "PropertyB2";

        public B(string? version = "2.0") : base("B--Id", version: version)
        {
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            foreach(var component in base.GetEqualityComponents())
                yield return component;
            yield return PropB1;
            yield return PropB2;
        }
    }

    [Test]
    public void DomainHelper_ToString_Success()
    {
        B b = new();
        string stringified = DomainHelper.Stringify(b);
        Assert.That(stringified, Is.EqualTo(b.ToString()));
    }
}
