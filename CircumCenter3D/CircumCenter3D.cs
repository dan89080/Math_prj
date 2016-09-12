using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CircumCenter3D
{
    public partial class CircumCenter3D_Page : Form
    {
        string STR_ang;
        public CircumCenter3D_Page()
        {
            InitializeComponent();
            STR_ang = label_Ang.Text;
        }

        public class Poi3D
        {
            public double x, y, z;
            public Poi3D(Poi3D Q)
            {
                x = Q.x;
                y = Q.y;
                z = Q.z;
            }
            public Poi3D(double a, double b, double c)
            {
                x = a;
                y = b;
                z = c;
            }
            public void Add(Poi3D Q)
            {
                x += Q.x;
                y += Q.y;
                z += Q.z;
            }
            public void Sub(Poi3D Q)
            {
                x -= Q.x;
                y -= Q.y;
                z -= Q.z;
            }
            public void Multi(double scale)
            {
                x *= scale;
                y *= scale;
                z *= scale;
            }
            public double Product_In(Poi3D Q)
            {
                return x * Q.x + y * Q.y + z * Q.z;
            }
            public Poi3D Product_Out(Poi3D Q)
            {
                double X = y * Q.z - z * Q.y;
                double Y = z * Q.x - x * Q.z;
                double Z = x * Q.y - y * Q.x;

                return new Poi3D(X, Y, Z);
            }
            public double Dis2Orig()
            {
                return Math.Sqrt(x * x + y * y + z * z);
            }
            public double Dis2Poi(Poi3D Q)
            {
                double a = x - Q.x;
                double b = y - Q.y;
                double c = z - Q.z;

                return Math.Sqrt(a * a + b * b + c * c);
            }
            public double Ang_Poi2Poi(Poi3D Q)
            {
                double L_P = this.Dis2Orig();
                double L_Q = Q.Dis2Orig();

                return Math.Acos(this.Product_In(Q) / (L_P * L_Q));
            }
        }
        public class Triangle
        {
            public Poi3D A, B, C;
            public double a, b, c;
            /// <summary>三角形的半周長。</summary>
            public double S;
            public Triangle(Poi3D x, Poi3D y, Poi3D z)
            {
                A = x;
                B = y;
                C = z;

                a = B.Dis2Poi(C);
                b = C.Dis2Poi(A);
                c = A.Dis2Poi(B);

                S = (a + b + c) / 2;
            }
            public Poi3D CalcCircumCenter()
            {
                double area = Math.Sqrt(S * (S - a) * (S - b) * (S - c));
                double R = a * b * c / (4 * area);

                double x = (a * a * (b * b + c * c - a * a)) / (16 * area * area);
                double y = (b * b * (a * a + c * c - b * b)) / (16 * area * area);
                double z = (c * c * (a * a + b * b - c * c)) / (16 * area * area);
                Poi3D alpha = new Poi3D(A);
                alpha.Multi(x);
                Poi3D beta = new Poi3D(B);
                beta.Multi(y);
                Poi3D gamma = new Poi3D(C);
                gamma.Multi(z);

                Poi3D result = new Poi3D(alpha);
                result.Add(beta);
                result.Add(gamma);

                return result;
            }
            public double Calc_CircumRadius()
            {
                double area = Math.Sqrt(S * (S - a) * (S - b) * (S - c));
                double R = a * b * c / (4 * area);
                return R;
            }
        }

        private void button_test1_Click(object sender, EventArgs e)
        {
            textBox_Ax.Text = "2";
            textBox_Ay.Text = "0";
            textBox_Az.Text = "0";

            textBox_Bx.Text = "-1";
            textBox_By.Text = "1.732";
            textBox_Bz.Text = "0";

            textBox_Cx.Text = "-1";
            textBox_Cy.Text = "-1.732";
            textBox_Cz.Text = "0";

            textBox_Nx.Text = "2";
            textBox_Ny.Text = "0";
            textBox_Nz.Text = "0";

            textBox_Px_Ang.Text = "1.732";
            textBox_Py_Ang.Text = "1";
            textBox_Pz_Ang.Text = "0";

            textBox_GenX.Text = "10";
            textBox_GenY.Text = "10";
            textBox_GenZ.Text = "10";
            textBox_GenR.Text = "3";

            textBox_Px_Err.Text = "2";
            textBox_Py_Err.Text = "0";
            textBox_Pz_Err.Text = "1";
        }

        private void button_calcCC_Click(object sender, EventArgs e)
        {
            try
            {
                double Ax = Convert.ToDouble(textBox_Ax.Text);
                double Ay = Convert.ToDouble(textBox_Ay.Text);
                double Az = Convert.ToDouble(textBox_Az.Text);

                double Bx = Convert.ToDouble(textBox_Bx.Text);
                double By = Convert.ToDouble(textBox_By.Text);
                double Bz = Convert.ToDouble(textBox_Bz.Text);

                double Cx = Convert.ToDouble(textBox_Cx.Text);
                double Cy = Convert.ToDouble(textBox_Cy.Text);
                double Cz = Convert.ToDouble(textBox_Cz.Text);

                Poi3D A = new Poi3D(Ax, Ay, Az);
                Poi3D B = new Poi3D(Bx, By, Bz);
                Poi3D C = new Poi3D(Cx, Cy, Cz);

                Triangle myTriangle = new Triangle(A, B, C);

                Poi3D W = myTriangle.CalcCircumCenter();

                textBox_Wx.Text = Math.Round(W.x, 5).ToString();
                textBox_Wy.Text = Math.Round(W.y, 5).ToString();
                textBox_Wz.Text = Math.Round(W.z, 5).ToString();
                textBox_calcR.Text = Math.Round(myTriangle.Calc_CircumRadius(), 5).ToString();
            }
            catch
            {

            }

        }

        private void button_test2_Click(object sender, EventArgs e)
        {
            textBox_Ax.Text = "0";
            textBox_Ay.Text = "0";
            textBox_Az.Text = "0";

            textBox_Bx.Text = "0";
            textBox_By.Text = "1";
            textBox_Bz.Text = "0";

            textBox_Cx.Text = "1";
            textBox_Cy.Text = "0";
            textBox_Cz.Text = "0";

            textBox_Nx.Text = "1.5";
            textBox_Ny.Text = "0.5";
            textBox_Nz.Text = "0";

            textBox_Px_Ang.Text = "1";
            textBox_Py_Ang.Text = "1";
            textBox_Pz_Ang.Text = "0";

            textBox_GenX.Text = "1";
            textBox_GenY.Text = "2";
            textBox_GenZ.Text = "3";
            textBox_GenR.Text = "9";
        }

        private void button_calcAng_Click(object sender, EventArgs e)
        {
            double Wx = Convert.ToDouble(textBox_Wx.Text);
            double Wy = Convert.ToDouble(textBox_Wy.Text);
            double Wz = Convert.ToDouble(textBox_Wz.Text);

            double Nx = Convert.ToDouble(textBox_Nx.Text);
            double Ny = Convert.ToDouble(textBox_Ny.Text);
            double Nz = Convert.ToDouble(textBox_Nz.Text);

            double Px = Convert.ToDouble(textBox_Px_Ang.Text);
            double Py = Convert.ToDouble(textBox_Py_Ang.Text);
            double Pz = Convert.ToDouble(textBox_Pz_Ang.Text);

            Poi3D N2W = new Poi3D(Nx - Wx, Ny - Wy, Nz - Wz);
            Poi3D P2W = new Poi3D(Px - Wx, Py - Wy, Pz - Wz);

            double ang = P2W.Ang_Poi2Poi(N2W);
            ang = ang * 180 / 3.1415926;
            label_Ang.Text = STR_ang + Math.Round(ang, 2) + " °";
        }

        private void button_Gen3Dcircle_Click(object sender, EventArgs e)
        {
            double GenX = Convert.ToDouble(textBox_GenX.Text);
            double GenY = Convert.ToDouble(textBox_GenY.Text);
            double GenZ = Convert.ToDouble(textBox_GenZ.Text);

            Poi3D Out_Ctr = new Poi3D(GenX, GenY, GenZ);

            double r = Convert.ToDouble(textBox_GenR.Text);
            double fi = 90.0 / 180 * 3.1415926, theta;
            double Out_X, Out_Y, Out_Z;

            theta = 0;
            Out_X = r * Math.Sin(fi) * Math.Cos(theta);
            Out_Y = r * Math.Sin(fi) * Math.Sin(theta);
            Out_Z = r * Math.Cos(fi);
            Poi3D Out1 = new Poi3D(Out_X, Out_Y, Out_Z);

            theta = 120.0 / 180 * 3.1415926;
            Out_X = r * Math.Sin(fi) * Math.Cos(theta);
            Out_Y = r * Math.Sin(fi) * Math.Sin(theta);
            Out_Z = r * Math.Cos(fi);
            Poi3D Out2 = new Poi3D(Out_X, Out_Y, Out_Z);

            theta = 240.0 / 180 * 3.1415926;
            Out_X = r * Math.Sin(fi) * Math.Cos(theta);
            Out_Y = r * Math.Sin(fi) * Math.Sin(theta);
            Out_Z = r * Math.Cos(fi);
            Poi3D Out3 = new Poi3D(Out_X, Out_Y, Out_Z);

            Out1 = new Poi3D(Out1.Product_Out(Out_Ctr));
            Out2 = new Poi3D(Out2.Product_Out(Out_Ctr));
            Out3 = new Poi3D(Out3.Product_Out(Out_Ctr));
            Out1.Multi(r / Out1.Dis2Orig());
            Out2.Multi(r / Out2.Dis2Orig());
            Out3.Multi(r / Out3.Dis2Orig());
            Out1.Add(Out_Ctr);
            Out2.Add(Out_Ctr);
            Out3.Add(Out_Ctr);

            textBox_Ax.Text = Math.Round(Out1.x, 4).ToString();
            textBox_Ay.Text = Math.Round(Out1.y, 4).ToString();
            textBox_Az.Text = Math.Round(Out1.z, 4).ToString();

            textBox_Bx.Text = Math.Round(Out2.x, 4).ToString();
            textBox_By.Text = Math.Round(Out2.y, 4).ToString();
            textBox_Bz.Text = Math.Round(Out2.z, 4).ToString();

            textBox_Cx.Text = Math.Round(Out3.x, 4).ToString();
            textBox_Cy.Text = Math.Round(Out3.y, 4).ToString();
            textBox_Cz.Text = Math.Round(Out3.z, 4).ToString();
        }

        private void button_CalcDis2Cir_Click(object sender, EventArgs e)
        {
            double Ccx = Convert.ToDouble(textBox_Wx.Text);
            double Ccy = Convert.ToDouble(textBox_Wy.Text);
            double Ccz = Convert.ToDouble(textBox_Wz.Text);

            double Ax = Convert.ToDouble(textBox_Ax.Text);
            double Ay = Convert.ToDouble(textBox_Ay.Text);
            double Az = Convert.ToDouble(textBox_Az.Text);

            double Bx = Convert.ToDouble(textBox_Bx.Text);
            double By = Convert.ToDouble(textBox_By.Text);
            double Bz = Convert.ToDouble(textBox_Bz.Text);

            double Cx = Convert.ToDouble(textBox_Cx.Text);
            double Cy = Convert.ToDouble(textBox_Cy.Text);
            double Cz = Convert.ToDouble(textBox_Cz.Text);

            Poi3D AB = new Poi3D(Bx - Ax, By - Ay, Bz - Az);
            Poi3D AC = new Poi3D(Cx - Ax, Cy - Ay, Cz - Az);
            Poi3D N = new Poi3D(AB.Product_Out(AC));

            //double Det =    (Ax * By * Cz + Ay * Bz + Cx + Az * Bx * Cy) - 
            //                (Az * By * Cx + Ay * Bx * Cz + Ax * Bz * Cy);
            double Nx = N.x;
            double Ny = N.y;
            double Nz = N.z;
            //double D

            double Px = Convert.ToDouble(textBox_Px_Err.Text);
            double Py = Convert.ToDouble(textBox_Py_Err.Text);
            double Pz = Convert.ToDouble(textBox_Pz_Err.Text);

            Poi3D P = new Poi3D(Px, Py, Pz);
            Poi3D Cc = new Poi3D(Ccx, Ccy, Ccz);

            //Ax + By + Cz = Aa + Bb + Cc;
            //A(At+P) + ...= Aa + ...;
            //t = (A * (a - P) + ...) / (Aa + ...);
            double t = (Nx * (Ccx - Px) + Ny * (Ccy - Py) + Nz * (Ccz - Pz)) /
                        (Nx * Nx + Ny * Ny + Nz * Nz);
            double Ppx = Px + Nx * t;
            double Ppy = Py + Ny * t;
            double Ppz = Pz + Nz * t;

            Poi3D Pp = new Poi3D(Ppx, Ppy, Ppz);

            Poi3D A = new Poi3D(Ax, Ay, Az);
            Poi3D B = new Poi3D(Bx, By, Bz);
            Poi3D C = new Poi3D(Cx, Cy, Cz);

            Triangle myTriangle = new Triangle(A, B, C);
            double R = myTriangle.Calc_CircumRadius();

            double opp = P.Dis2Poi(Pp);
            double adj = Pp.Dis2Poi(Cc) - R;
            double hyp = Math.Sqrt(opp * opp + adj * adj);
            textBox_dis_P2Cc.Text = Math.Round(hyp, 2).ToString();
            textBox_dis_P2Cc_ratio.Text = Math.Round(hyp / R * 100, 2) + "%";
        }
    }
}
