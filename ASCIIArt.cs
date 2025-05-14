using System.IO;
using System;
using System.Drawing;

namespace part_two_poe
{
   public class ASCIIArt
    {
        public ASCIIArt()
        {
            // Get the current project directory
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;

            // Adjust the path to remove 'bin\\Debug\\'
            string adjustedPath = projectPath.Replace("bin\\Debug\\", "");

            // Combine the project path with the image file name
            string imagePath = Path.Combine(adjustedPath, "download.jpg");

            // Load and resize the image
            Bitmap image = new Bitmap(imagePath);
            image = new Bitmap(image, new Size(100, 60));

            // Iterate through the image pixels and create colorful ASCII art
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    // Get pixel color
                    Color pixelColor = image.GetPixel(x, y);

                    // Calculate brightness
                    int brightness = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    // Determine ASCII character
                    char asciiChar = brightness > 200 ? '.' :
                                     brightness > 150 ? '*' :
                                     brightness > 100 ? 'O' :
                                     brightness > 50 ? '#' : '@';

                    // Set console color based on pixel color
                    Console.ForegroundColor = GetConsoleColor(pixelColor);

                    // Print the ASCII character
                    Console.Write(asciiChar);
                }

                // Move to the next line after each row
                Console.WriteLine();
            }

            // Reset console color
            Console.ResetColor();
        }

        private ConsoleColor GetConsoleColor(Color color)
        {
            // Convert color to a matching console color
            if (color.R > 200 && color.G > 200 && color.B > 200) return ConsoleColor.White;
            if (color.R > 200) return ConsoleColor.Red;
            if (color.G > 200) return ConsoleColor.Green;
            if (color.B > 200) return ConsoleColor.Blue;
            if (color.R > 100 && color.G > 100) return ConsoleColor.Yellow;
            if (color.R > 100 && color.B > 100) return ConsoleColor.Magenta;
            if (color.G > 100 && color.B > 100) return ConsoleColor.Cyan;
            return ConsoleColor.DarkGray;
        }
    }
}