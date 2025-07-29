using System;
using System.Collections.Generic;

namespace DeltaPatchCore.Delta
{
    public class DeltaPatch
    {
        private readonly List<DeltaPatchEntry> _entries;

        public DeltaPatch(IEnumerable<DeltaPatchEntry> entries)
        {
            _entries = new List<DeltaPatchEntry>(entries);
        }

        public static DeltaPatch Create(byte[] oldData, byte[] newData)
        {
            if (oldData.Length != newData.Length)
                throw new ArgumentException("Размеры массивов должны совпадать");

            var changes = new List<DeltaPatchEntry>();

            for (int i = 0; i < oldData.Length; i++)
            {
                if (oldData[i] != newData[i])
                    changes.Add(new DeltaPatchEntry(i, oldData[i], newData[i]));
            }

            return new DeltaPatch(changes);
        }

        public void Apply(byte[] data)
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                var entry = _entries[i];
                // меняем байт и переворачиваем патч
                byte tmp = data[entry.Index];
                data[entry.Index] = entry.OldValue;

                _entries[i] = new DeltaPatchEntry(entry.Index, tmp, entry.OldValue); // swap
            }
        }

        public int Count => _entries.Count;
    }

}
