using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaProjekt2
{
    class DrawFunctions
    {
        static void Swap(ref int x, ref int y)
        {
            int temp = 0;
            temp = x;
            x = y;
            y = temp;
        }
        public static void Brsenham(int x0, int y0, int x1, int y1, ref Bitmap bp) {


            Console.WriteLine(y0);
            Console.WriteLine(x0);
            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);      // Отражаем линию по диагонали, если угол наклона слишком большой
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2; 
            int ystep = (y0 < y1) ? 1 : -1; 
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                bp.SetPixel(steep ? y : x, steep ? x : y, Color.White); 
                
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        public static void AlgorytmPrzyrostowy(int x0, int y0, int x1, int y1, ref Bitmap bp) {
 
            int dx = x1 - x0;
            int dy = y1 - y0;

            // вычисляем шаг, необходимый для генерирования пикслей
            int steps = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy);

            // вычислеям шаг для инкремента по оси х и по оси у
            float Xinc = dx / (float)steps;
            float Yinc = dy / (float)steps;

            // 
            float X = x0;
            float Y = y0;
            for (int i = 0; i <= steps; i++)
            {
                bp.SetPixel((int)X, (int)Y, Color.Red);  // put pixel at (X,Y) 
                X += Xinc;           // increment in x at each step 
                Y += Yinc;           // increment in y at each step 
                 
            }
        }

    }
}
