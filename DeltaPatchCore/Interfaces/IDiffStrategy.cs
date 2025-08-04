using DeltaPatchCore.Delta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaPatchCore.Interfaces
{
    public interface IDiffStrategy
    {
        DeltaPatch ComputeDiff(byte[] oldData, byte[] newData);
    }
}
