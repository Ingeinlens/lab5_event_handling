using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp.Objects
{
    internal class Figure : BaseObject
    {
        public Figure(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Green), -15, -15, 30, 30);
        }

        //Создаём форму зелёного круга
        public override GraphicsPath GetGraphicsPath()
        {
            //Создаём путь
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            //Забираем путь
            return path;
        }

        public void ReducedSize()
        {
        }
    }
}