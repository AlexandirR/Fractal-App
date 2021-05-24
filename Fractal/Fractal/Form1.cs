using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractal {
    public partial class Form1 : Form {
        public class Complex {
            private double r, i;
            public void setR(double r) {
                this.r = r;
            }

            public double getR() {
                return this.r;
            }

            public void setI(double i) {
                this.i = i;
            }

            public double getI() {
                return this.i;
            }

                public Complex() {
                this.r = 0.0;
                this.i = 0.0;
            }
            public void Add(Complex b) {
                this.r += b.getR();
                this.i += b.getI();
            }
            public static Complex Sum(Complex a, Complex b) {
                Complex res = new Complex();
                res.setR(a.getR() + b.getR());
                res.setI(a.getI() + b.getI());
                return res;
            }
            public static Complex Multiplication(Complex a, Complex b) {
                Complex res = new Complex();
                res.setR(a.getR() * b.getR() - a.getI() * b.getI());
                res.setI(a.getI() * b.getR() + a.getR() * b.getI());
                return res;
            }
            public static Complex Subtract(Complex a, Complex b)
            {
                Complex res = new Complex();
                res.setR(a.getR() - b.getR());
                res.setI(a.getI() - b.getI());
                return res;
            }
            public static Complex operator +(Complex a, Complex b)
            {
                return Complex.Sum(a, b);
            }
            public static Complex operator -(Complex a, Complex b)
            {
                return Complex.Subtract(a, b);
            }
            public static Complex operator *(Complex a, Complex b)
            {
                return Complex.Multiplication(a, b);
            }
        }
        double inff = 2.0;
        int color_cnt = 5; 
        double xl = -2.5;
        double xr = 1.5;
        double yu = 2;
        double yd = -2;
        int iter = 255;
        Complex inf = new Complex();
        int get_cnt(Complex c) {
            inf.setR(inff);
            Complex z = new Complex();
            int i = 0;
            while ((z.getR() * z.getR() + z.getI() * z.getI()) < (inf.getR() * inf.getR()) && i < iter) {
                z = z*z;
                z.Add(c);
                i++;
            }
            return i;
        }
        Color get_color1(int cnt) {
            return Color.FromArgb(((255 - cnt) * cnt)%256, ((255 - cnt) * cnt)%256, ((255 - cnt) * cnt)%256);
        }
        Color get_color2(int cnt) {
            return Color.FromArgb(Math.Min((255 - cnt), 255), ((255 - cnt) * cnt)%256, (((255 - cnt) * 255) % 255*cnt)%256);
        }
        Color get_color3(int cnt) {
            return Color.FromArgb((( cnt) * cnt) % 256, (((255 - cnt) * 255) % 255 * cnt) % 256, Math.Min((cnt), 255));
        }
        Color get_color4(int cnt) {
            return Color.FromArgb((((255 - cnt) * 255) % 255 * cnt) % 256, ((255 - cnt) * cnt) % 256, Math.Min((255 - cnt), 255));
        }
        Color get_color5(int cnt) {
            return Color.FromArgb(255 - cnt, 255 - cnt, 255 - cnt);
        }
        public Form1() {
            InitializeComponent();
        }
        void Draw_Picture() {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                double dx = (xr - xl) / pictureBox1.Width;
                double dy = (yu - yd) / pictureBox1.Height;
                for (int i = 0; i < pictureBox1.Height; ++i)
                {
                    for (int j = 0; j < pictureBox1.Width; ++j)
                    {
                        Complex c = new Complex();
                        c.setR(xl + j * dx);
                        c.setI(yu - i * dy);
                        if (color_cnt == 1)
                            bmp.SetPixel(j, i, get_color1(get_cnt(c)));
                        if (color_cnt == 2)
                            bmp.SetPixel(j, i, get_color2(get_cnt(c)));
                        if (color_cnt == 3)
                            bmp.SetPixel(j, i, get_color3(get_cnt(c)));
                        if (color_cnt == 4)
                            bmp.SetPixel(j, i, get_color4(get_cnt(c)));
                        if(color_cnt == 5)
                            bmp.SetPixel(j, i, get_color5(get_cnt(c)));
                    }
                }
                pictureBox1.BackgroundImage = bmp;
                pictureBox1.Image = null;
            }
        }
        private void button1_Click(object sender, EventArgs e) {
            xl = -2.5;
            xr = 1.5;
            yu = 2;
            yd = -2;
            Draw_Picture();
        }
        int XN = 0;
        int YN = 0;
        bool flag = false;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
            XN = e.X;
            YN = e.Y;
            flag = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
            int X = e.X;
            int Y = e.Y;
            double dx = (xr - xl) / pictureBox1.Width;
            double dy = (yu - yd) / pictureBox1.Height;
            if (XN > X) {
                int t = XN;
                XN = X;
                X = t;
            }
            if(Y > YN) {
                int t = YN;
                YN = Y;
                Y = t;
            }
            if(X - XN > YN - Y) {
                Y += (X - XN) - (YN - Y);
            }
            else {
                X += (YN - Y) - (X - XN);
            }
            xr = xl + X * dx;
            xl = xl + XN * dx;
            yd = yu - YN * dy;
            yu = yu - Y * dy;
            flag = false;
            Draw_Picture();
        }

        private void button2_Click(object sender, EventArgs e) {
            color_cnt = 1;
            Draw_Picture();
        }

        private void button3_Click(object sender, EventArgs e) {
            color_cnt = 2;
            Draw_Picture();
        }

        private void button4_Click(object sender, EventArgs e) {
            color_cnt = 3;
            Draw_Picture();
        }

        private void button5_Click(object sender, EventArgs e) {
            color_cnt = 4;
            Draw_Picture();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            if(flag) {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                using (var g = Graphics.FromImage(bmp)) {
                    Pen p = new Pen(Color.Red, 2);
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawRectangle(p, Math.Min(XN, e.X), Math.Min(YN, e.Y), Math.Abs(XN - e.X), Math.Abs(YN - e.Y));
                    pictureBox1.Image = bmp;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e) {
            color_cnt = 5;
            Draw_Picture();
        }
    }
}
