using System;
using System.Collections.Generic;
using System.IO;
using DeltaQ;
using DeltaQ.BsDiff;

namespace DeltaPatchCore.Delta
{
    public class DeltaPatch
    {
        private readonly List<DeltaPatchEntry>? _entries;   // побайтовые изменения
        private readonly byte[]? _blockDelta;               // блочный патч (DeltaQ)

        public DeltaPatch(IEnumerable<DeltaPatchEntry> entries)
        {
            _entries = new List<DeltaPatchEntry>(entries);
        }

        public DeltaPatch(byte[] blockDelta)
        {
            _blockDelta = blockDelta;
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
            if (_blockDelta != null)
            {
                using var baseStream = new MemoryStream(data);
                using var delta = new MemoryStream(_blockDelta);
                using var output = new MemoryStream();

                Patch.Apply(baseStream.ToArray(), delta.ToArray(), output);

                // Копируем результат поверх оригинала
                var patched = output.ToArray();
                Array.Copy(patched, data, patched.Length);
            }
            else if (_entries != null)
            {
                for (int i = 0; i < _entries.Count; i++)
                {
                    var entry = _entries[i];
                    byte tmp = data[entry.Index];
                    data[entry.Index] = entry.OldValue;

                    _entries[i] = new DeltaPatchEntry(entry.Index, tmp, entry.OldValue); // swap
                }
            }
            else
            {
                throw new InvalidOperationException("Патч не инициализирован");
            }
        }

        public int Count
        {
            get
            {
                if (_entries != null) return _entries.Count;
                if (_blockDelta != null) return _blockDelta.Length;
                return 0;
            }
        }
    }
}
