using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW3_clock
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;

        int cx = 150, cy = 150; //центр годинника
        // довжини стрілок
        int hour_hand = 65; 
        int min_hand = 85;
        int sec_hand = 100;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Clock";
            this.BackColor = Color.WhiteSmoke;
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            pictureBox1.Width = 500;
            pictureBox1.Height = 500;

            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image = bm);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private int[] msCoord(int val, int hlen)
        {// функція повертає координати кінця сек або хвил стрілок
            int[] coord = new int[2];
            val *= 6;

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        private int[] hrCoord(int hval, int mval, int hlen)
        {// функція повертає координати кінця годинної стрілок
            int[] coord = new int[2];
            int val = (int)((hval * 30) + (mval * 0.5));

            if (val >= 0 && val <= 180)
            {
                coord[0] = cx + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = cx - (int)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = cy - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            g.Clear(BackColor);
            Pen pen = new Pen(Color.Turquoise, 8);
            Pen thin = new Pen(Color.Turquoise, 4);
            g.DrawEllipse(pen, 10, 10, 280, 280);
            g.FillEllipse(Brushes.AliceBlue, 10, 10, 280, 280);
            g.FillEllipse(Brushes.Turquoise, 143, 143, 14, 14);

            g.DrawLine(thin, 150, 10, 150, 35);
            g.DrawLine(thin, 150, 265, 150, 290); 
            g.DrawLine(thin, 10, 150, 35, 150);
            g.DrawLine(thin, 265, 150, 290, 150);

            DateTime curr_time = DateTime.Now;
            int sec = curr_time.Second, min = curr_time.Minute, hour = curr_time.Hour;
            int[] coordinates = new int[2];

            Pen sec_color = new Pen(Color.DarkRed, 2);
            Pen min_color = new Pen(Color.DarkBlue, 3);
            Pen hour_color = new Pen(Color.DarkGray, 4);

            Graphics arr = Graphics.FromImage(pictureBox1.Image = bm);
            arr.SmoothingMode = SmoothingMode.AntiAlias;

            coordinates = msCoord(sec, sec_hand + 4);
            arr.DrawLine(sec_color, new Point(cx, cy), new Point(coordinates[0], coordinates[1]));

            coordinates = msCoord(min, min_hand + 4);
            arr.DrawLine(min_color, new Point(cx, cy), new Point(coordinates[0], coordinates[1]));

            coordinates = hrCoord(hour % 12, min, hour_hand + 4);
            arr.DrawLine(hour_color, new Point(cx, cy), new Point(coordinates[0], coordinates[1]));

            g.DrawRectangle(thin, 40, 330, 230, 40);
            g.DrawString(curr_time.ToLongTimeString(), new Font("Arial", 16), Brushes.DarkCyan, 110, 340);
            
            pictureBox1.Invalidate();
        }

    }
}
