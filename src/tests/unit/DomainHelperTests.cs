//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System;
using System.Buffers;
using Newtonsoft.Json.Linq;
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
        string bStr = b.ToString();
        Assert.That(stringified, Is.EqualTo(bStr));
    }

    [Test]
    public void DomainHelper_DateTimeOffset_Success()
    {
        (var dt, var ianaId) = DomainHelper.CurrentDateTime();
        TimeZoneInfo localZone = TimeZoneInfo.Local;
        Console.WriteLine($"{dt}: {localZone.Id} → {ianaId}");
    }

    private class BufferSegment : ReadOnlySequenceSegment<byte>
    {
        public BufferSegment(ReadOnlyMemory<byte> memory)
        {
            Memory = memory;
        }

        public BufferSegment Append(ReadOnlyMemory<byte> memory)
        {
            var segment = new BufferSegment(memory);
            Next = segment;
            segment.RunningIndex = RunningIndex + Memory.Length;
            return segment;
        }
    }

    [Test]
    public void DomainHelper_Linearize_ReadOnlySequence_Success()
    {
        var first = new BufferSegment(new byte[] { 1, 2, 3 });
        var last = first.Append(new byte[] { 4, 5, 6 }).Append(new byte[] { 7, 8, 9 });

        ReadOnlySequence<byte> sequence = new(first, 0, last, last.Memory.Length);

        DomainHelper.Linearize(in sequence, out byte[]? lease);
        Assert.That(lease, Is.Not.Null);
        DomainHelper.Recycle(lease);
    }
}
