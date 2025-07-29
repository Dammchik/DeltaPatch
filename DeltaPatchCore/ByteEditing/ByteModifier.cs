
namespace DeltaPatchCore.ByteEditing
{
    public static class ByteModifier
    {
        /// <summary>
        /// Модифицирует заданный процент байтов в массиве случайным образом.
        /// </summary>
        /// <param name="data">Исходный массив байт</param>
        /// <param name="percent">Процент данных, которые нужно изменить (0-100)</param>
        public static void ModifyBytes(byte[] data, double percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentOutOfRangeException(nameof(percent), "Процент должен быть от 0 до 100");

            int totalCount = data.Length;
            int changeCount = (int)(totalCount * percent / 100.0);

            if (changeCount == 0)
                return;

            var rng = new Random();
            var changedIndices = new HashSet<int>();

            while (changedIndices.Count < changeCount)
            {
                int index = rng.Next(totalCount);
                if (changedIndices.Add(index))
                {
                    byte delta = (byte)rng.Next(1, 256); 
                    data[index] ^= delta;
                }
            }
        }
    }
}
