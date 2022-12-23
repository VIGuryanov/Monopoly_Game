using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyMAUI.Graphics
{
    public class PlayerPoint : IDrawable
    {
        Color color;

        public PlayerPoint(Color col)
        {
            color = col;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = color;
            canvas.StrokeSize = 8;
            canvas.DrawCircle(6, 6, 2);
        }
    }
}
