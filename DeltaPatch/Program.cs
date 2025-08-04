using DeltaPatchCore.ByteEditing;
using DeltaPatchCore.Delta;
using DeltaPatchCore.ImageLoader;
using DeltaPatchCore.Interfaces;
using System.Diagnostics;

namespace DeltaPatch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== DeltaPatch Interactive ===");

            // 1. Размеры
            int width = ReadInt("Введите ширину массива (от 16 до 32000): ", 16, 32000);
            int height = ReadInt("Введите высоту массива (от 16 до 32000): ", 16, 32000);

            byte[] data = RandomByteArrayLoader.Generate(width, height);
            Console.WriteLine($"Сгенерирован массив {width} x {height} = {data.Length} байт");
            Print2D(data, width, height);

            IDiffStrategy strategy = SelectStrategy();
            var history = new History(strategy);

            // 2. Цикл команд
            while (true)
            {
                Console.Write("\nВведите команду [edit / undo / redo / exit]: ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (input == "edit")
                {
                    double percent = ReadDouble("Введите процент изменения байтов (0–100): ", 0, 100);
                    byte[] before = (byte[])data.Clone();
                    ByteModifier.ModifyBytes(data, percent);
                    history.Record(before, data);
                    Console.WriteLine($"Изменено ~{(int)(data.Length * percent / 100)} байт");
                    Print2D(data, width, height);
                }
                else if (input == "undo")
                {
                    if (!history.CanUndo)
                    {
                        Console.WriteLine("Нет доступных изменений для отката.");
                        continue;
                    }

                    var time = Measure(() => history.Undo(data));
                    Console.WriteLine($"Undo выполнен за {time.TotalMilliseconds:F2} мс");
                    Print2D(data, width, height);
                }
                else if (input == "redo")
                {
                    if (!history.CanRedo)
                    {
                        Console.WriteLine("Нет изменений для возврата.");
                        continue;
                    }

                    var time = Measure(() => history.Redo(data));
                    Console.WriteLine($"Redo выполнен за {time.TotalMilliseconds:F2} мс");
                    Print2D(data, width, height);
                }
                else if (input == "exit")
                {
                    Console.WriteLine("Завершение работы.");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда.");
                }
            }
        }

        static int ReadInt(string message, int min, int max)
        {
            int value;
            do
            {
                Console.Write(message);
            }
            while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max);

            return value;
        }

        static double ReadDouble(string message, double min, double max)
        {
            double value;
            do
            {
                Console.Write(message);
            }
            while (!double.TryParse(Console.ReadLine(), out value) || value < min || value > max);

            return value;
        }

        static TimeSpan Measure(Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            return sw.Elapsed;
        }

        static void Print2D(byte[] data, int width, int height)
        {
            Console.WriteLine($"\nТекущий вид массива ({height}x{width}):");
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write($"{data[y * width + x],3} ");
                }
                Console.WriteLine();
            }
        }

        static IDiffStrategy SelectStrategy()
        {
            while (true)
            {
                Console.WriteLine("\nВыберите стратегию дифференцирования:");
                Console.WriteLine("1 - ByteDiff (побайтовая)");
                Console.WriteLine("2 - BlockDiff (DeltaQ, блочная)");

                Console.Write("Ваш выбор [1/2]: ");
                string? choice = Console.ReadLine()?.Trim();

                return choice switch
                {
                    "1" => new ByteDiffStrategy(),
                    "2" => new BlockDiffStrategy(),
                    _ => throw new ArgumentException("Неверный выбор. Повторите.")
                };
            }
        }
    }
}
