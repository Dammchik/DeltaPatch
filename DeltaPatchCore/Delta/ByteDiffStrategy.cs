using DeltaPatchCore.Interfaces;

namespace DeltaPatchCore.Delta
{
    public class ByteDiffStrategy : IDiffStrategy
    {
        public DeltaPatch ComputeDiff(byte[] oldData, byte[] newData)
        {
            var entries = new List<DeltaPatchEntry>();

            for (int i = 0; i < oldData.Length; i++)
            {
                if (oldData[i] != newData[i])
                {
                    entries.Add(new DeltaPatchEntry(i, oldData[i], newData[i]));
                }
            }

            return new DeltaPatch(entries);
        }
    }
}
