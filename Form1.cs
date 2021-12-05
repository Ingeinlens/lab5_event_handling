using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Objects;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private Marker marker;
        private Player player;
        private List<BaseObject> objects = new();

        public Form1()
        {
            InitializeComponent();

            //Создаём игрока
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            //Добавляем реакцию на пересечение
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            //Добавляем реакцию на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            //Создаём маркер
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);

            objects.Add(marker);
            objects.Add(player);

            objects.Add(new Figure(50, 25, 45));
            objects.Add(new Figure(100, 80, 45));
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            //Создаём объект класса графики
            var g = e.Graphics;

            //Очищаем поле
            g.Clear(Color.White);

            //Вызываем пересчёт положения игрока
            updatePlayer();

            //Считаем пересечения
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overpal(obj);
                    obj.Overpal(player);
                }
            }

            //Рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            // тут добавляем проверку на marker не нулевой
            if (marker != null)
            {
                // рассчитываем вектор между игроком и маркером
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                //находим его длину
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length; //нормализуем координаты
                dy /= length;

                //пересчитываем координаты игрока
                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                //рассчитываем угол поворота игрока
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            // тормозящий момент,
            // нужен чтобы, когда игрок достигнет маркера произошло постепенное замедление
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // пересчет позиция игрока с помощью вектора скорости
            player.X += player.vX;
            player.Y += player.vY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //запрашиваем обновление pbMain
            //это вызовет метод pbMain_Paint по новой
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }
            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}