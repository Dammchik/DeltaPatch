using DeltaPatchCore.Interfaces;
using DeltaQ;
using DeltaQ.BsDiff;
using DeltaQ.SuffixSorting;
using DeltaQ.SuffixSorting.LibDivSufSort;
using System.IO;

namespace DeltaPatchCore.Delta
{
    public class BlockDiffStrategy : IDiffStrategy
    {
        public string Name => "BlockDiff (DeltaQ)";
        private readonly ISuffixSort _suffixSorter = new LibDivSufSort();

        public DeltaPatch ComputeDiff(byte[] oldData, byte[] newData)
        {
            using var deltaStream = new MemoryStream();
            Diff.Create(oldData, newData, deltaStream, _suffixSorter);
            return new DeltaPatch(deltaStream.ToArray());
        }
    }
}
