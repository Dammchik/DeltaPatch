namespace DeltaPatchCore.ImageLoader
{
    public static class RandomByteArrayLoader
    {
        /// <summary>
        /// Генерирует случайный двумерный массив, записанный в строку (одномерный byte[]).
        /// </summary>
        /// <param name="width">Ширина двумерной матрицы</param>
        /// <param name="height">Высота двумерной матрицы</param>
        /// <returns>Одномерный массив байт, представляющий матрицу</returns>
        public static byte[] Generate(int width, int height)
        {
            if (width < 16 || height < 16 || width > 32000 || height > 32000)
                throw new ArgumentOutOfRangeException("Размеры должны быть в пределах от 16 до 32000");

            int size = width * height;
            byte[] data = new byte[size];
            var rng = new Random();

            rng.NextBytes(data);
            return data;
        }
    }
}
