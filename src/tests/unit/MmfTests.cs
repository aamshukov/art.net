//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using UILab.Art.Framework.Core.DataAccess.Repository.Mmf;

namespace UILab.Art.Tests;

public unsafe struct MmfData
{
    public int First;
    public Padding40 Padding;
    public int Second;
    public fixed byte Padding28[28];
    public int Third;
    public string Fourth;

    public struct InternalData
    {
        public int K;
        public int L;
        public string Kuku;
        public int M;
    }

    public InternalData Embedded;

    [InlineArray(40)]
    public struct Padding40
    {
        private byte Start;
    }

    [UnscopedRef]
    public ref T PaddingAs<T>() where T : struct => ref Unsafe.As<Padding40, T>(ref Padding);

    public unsafe ref T Padding28As<T>()
    {
        fixed(byte* p = Padding28)
        return ref Unsafe.AsRef<T>(p);
    }
}

[TestFixture]
internal class MmfTests
{
    [Test]
    public void Mmf_Create_Success()
    {
        {
            using FileStream file = new(@"d:\tmp\mmftmp.dat", FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.RandomAccess);
            using MemoryMappedFile mapping = MemoryMappedFile.CreateFromFile(file, null, 1000, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, leaveOpen: false);
            using MemoryMappedViewAccessor view = mapping.CreateViewAccessor();
            using MmfCursor cursor = new(view);

            ref MmfData data = ref cursor.As<MmfData>();

            data.First = 1;
            data.Second = 2;
            data.Third = 3;
            data.Fourth = "Fourth";
            data.Embedded.K = 123;
            data.Embedded.L = 123743;
            data.Embedded.M = 1239678;
            data.Embedded.Kuku = "KUKU";

            Assert.That(data.First, Is.EqualTo(1));
            Assert.That(data.Second, Is.EqualTo(2));
            Assert.That(data.Third, Is.EqualTo(3));
            Assert.That(data.Fourth, Is.EqualTo("Fourth"));
            Assert.That(data.Embedded.K, Is.EqualTo(123));
            Assert.That(data.Embedded.L, Is.EqualTo(123743));
            Assert.That(data.Embedded.M, Is.EqualTo(1239678));
            Assert.That(data.Embedded.Kuku, Is.EqualTo("KUKU"));
        }
        {
            using FileStream file = new(@"d:\tmp\mmftmp.dat", FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.RandomAccess);
            using MemoryMappedFile mapping = MemoryMappedFile.CreateFromFile(file, null, 1000, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, leaveOpen: false);
            using MemoryMappedViewAccessor view = mapping.CreateViewAccessor();
            using MmfCursor cursor = new(view);

            ref MmfData data = ref cursor.As<MmfData>();

            Assert.That(data.First, Is.EqualTo(1));
            Assert.That(data.Second, Is.EqualTo(2));
            Assert.That(data.Third, Is.EqualTo(3));
            Assert.That(data.Fourth, Is.EqualTo("Fourth"));
            Assert.That(data.Embedded.K, Is.EqualTo(123));
            Assert.That(data.Embedded.L, Is.EqualTo(123743));
            Assert.That(data.Embedded.M, Is.EqualTo(1239678));
            Assert.That(data.Embedded.Kuku, Is.EqualTo("KUKU"));
        }
    }
}
