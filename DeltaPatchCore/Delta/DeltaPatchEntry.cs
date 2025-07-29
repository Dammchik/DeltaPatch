using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaPatchCore.Delta
{
    public record DeltaPatchEntry(int Index, byte OldValue, byte NewValue);
}
