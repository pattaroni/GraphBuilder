using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace GraphBuilder
{
    // Збереження графіка в PNG і таблиці значень у текстовий файл
    public class FileHelper
    {
        public static void SavePng(Canvas canvas, string path)
        {
            int width = (int)canvas.Bounds.Width;
            int height = (int)canvas.Bounds.Height;

            using (RenderTargetBitmap bitmap = new RenderTargetBitmap(new PixelSize(width, height), new Vector(96, 96)))
            {
                bitmap.Render(canvas);
                bitmap.Save(path);
            }
        }

        public static void SaveTable(Graph graph, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(graph.Function.Name + ": " + graph.Function.GetFormula());
                writer.WriteLine();
                writer.WriteLine("x\ty");

                foreach (Point point in graph.Points)
                {
                    writer.WriteLine(InputHelper.FormatNumber(point.X) + "\t" + InputHelper.FormatNumber(point.Y));
                }
            }
        }
    }
}
