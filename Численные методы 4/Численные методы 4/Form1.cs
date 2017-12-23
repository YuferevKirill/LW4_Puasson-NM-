using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Численные_методы_3
{
    public partial class Form1 : Form
    {
        int iterationCount;
        double eps;
        double A;
        int n;
        int m;
        double a = -1;
        double b = 0;
        double c = 0;
        double d = 1;
        double h;
        double k;
        double h2;
        double k2;
        double realEps;
        double ZeidEps;


        public Form1()
        {
            InitializeComponent();
        }

        private double TestFunction(double x, double y)
        {
            return Math.Exp(x*y); 
        }

        private double GradientTestFunction(double x, double y)
        {
            return -Math.Exp(x * y) * (x*x+y*y); 
        }

        private double MainFunction(double x, double y)
        {
            return Math.Cosh(x*x*y);
        }

        private double Mu1(double y)
        {
            return Math.Sin(Math.PI * y);    
        } 

        private double Mu2(double y)
        {
            return Math.Abs(Math.Sin(2* Math.PI * y));
        }

        private double Mu3(double x)
        {
            return -x*(x+1);
        }

        private double Mu4(double x)
        {
            return -x * (x + 1);
        }

        void AddValueInTable(ref double[] x, ref double[] y, ref double[,] arr, int N, int M)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(M + 2);
            dataGridView1.ColumnCount = N + 3;
            dataGridView1.Rows[0].Cells[1].Value = "i";
            for (int j = 2; j <= N + 2; j++)
                dataGridView1.Rows[0].Cells[j].Value = (j - 2).ToString();
            dataGridView1.Rows[1].Cells[0].Value = "j";
            for (int i = 2; i <= M + 2; i++)
                dataGridView1.Rows[i].Cells[0].Value = (M + 2 - i).ToString();
            dataGridView1.Rows[1].Cells[1].Value = " y\\x";
            for (int i = 2; i <= N + 2; i++)
                dataGridView1.Rows[1].Cells[i].Value = x[i - 2].ToString();
            for (int j = 2; j <= M + 2; j++)
                dataGridView1.Rows[j].Cells[1].Value = y[M + 2 - j];
            for (int j = 2; j <= M + 2; j++)
                for (int i = 2; i <= N + 2; i++)
                    dataGridView1.Rows[j].Cells[i].Value = Math.Round(arr[i - 2, M + 2 - j],6).ToString();
        }
        int ZeidelMethodForTestFunction(ref double[,] v, ref double[,] u, ref double[,] f)
        {
            for (int s = 0; s < iterationCount; s++)
            {
                IterationInZeidelMethod(ref v, ref u, ref f);
                if (ZeidEps < eps)
                {
                    return (s + 1);
                }
            }
            return -1;
        }
        void IterationInZeidelMethod(ref double[,] v, ref double[,] u, ref double[,] f)
        {
            double temp;
            double value_real = 0.0;
            double value_norm = 0.0;

            double value;
            for (int i = 1; i < n; i++)
                for (int j = 1; j < m; j++)
                {
                    temp = v[i, j];
                    v[i, j] = (-f[i, j] - (v[i + 1, j] + v[i - 1, j]) / (h * h) - (v[i, j - 1] + v[i, j + 1]) / (k * k)) / A;

                    value = Math.Abs(u[i, j] - v[i, j]);
                    if (value > value_real)
                        value_real = value;

                    value = Math.Abs(temp - v[i, j]);
                    if (value > value_norm)
                        value_norm = value;
                }
            realEps = value_real;
            ZeidEps = value_norm;
        }
        int ZeidelMethodForMainFunction(ref double[,] v, ref double[,] f, int N, int M, double H, double K)
        {
            A = -2.0 * (1.0 / (H * H) + 1.0 / (K * K));
            for (int s = 0; s < iterationCount; s++)
            {
                IterationZeidelMethodInMainFunction(ref v, ref f, N, M, H, K);
                if (ZeidEps < eps)
                {
                    return (s + 1);
                }
            }

            return -1;
        }
        void IterationZeidelMethodInMainFunction(ref double[,] v, ref double[,] f, int N, int M, double H, double K)
        {
            double temp;
            double value_norm = 0.0;
            double value;
            for (int i = 1; i < N; i++)
                for (int j = 1; j < M; j++)
                {
                    temp = v[i, j];
                    v[i, j] = (-f[i, j] - (v[i + 1, j] + v[i - 1, j]) / (H * H) - (v[i, j - 1] + v[i, j + 1]) / (K * K)) / A;

                    value = Math.Abs(temp - v[i, j]);
                    if (value > value_norm)
                        value_norm = value;
                }
            ZeidEps = value_norm;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "1e-008";
            textBox2.Text = "500";
            label6.Visible = false;
            label9.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Visible = false;
            label9.Visible = false;
            n = Convert.ToInt32(numericUpDown1.Value);
            m = Convert.ToInt32(numericUpDown2.Value);
            h = (b - a) / (double)n;
            k = (d - c) / (double)m;
            A = -2.0 * (1.0 / (h * h) + 1.0 / (k * k));
            eps = Convert.ToDouble(textBox1.Text);
            iterationCount = Convert.ToInt32(textBox2.Text);

            double[] x = new double[n + 1];
            double[] y = new double[m + 1]; 
            for (int i = 0; i < n + 1; i++)
                x[i] = i * h + a;
            for (int j = 0; j < m + 1; j++)
                y[j] = j * k + c;

            double[,] u = new double[n + 1, m + 1];
            for (int i = 0; i < n + 1; i++)
                for (int j = 0; j < m + 1; j++)
                    u[i, j] = TestFunction(x[i], y[j]);

            double[,] f = new double[n + 1, m + 1];
            for (int i = 0; i < n + 1; i++)
                for (int j = 0; j < m + 1; j++)
                    f[i, j] = GradientTestFunction(x[i], y[j]);

            double[,] v = new double[n + 1, m + 1];
            for (int i = 0; i <= n; i++)
            {
                v[i, 0] = TestFunction(x[i], c);
                v[i, m] = TestFunction(x[i], d);
            }
            for (int j = 1; j < m; j++)
            {
                v[0, j] = TestFunction(a, y[j]);
                v[n, j] = TestFunction(b, y[j]);
            }

            int flag = ZeidelMethodForTestFunction(ref v, ref u, ref f);

            label5.Text = "Достигнутая погрешность:" + ZeidEps.ToString(); 
            label7.Text = "Реальная погрешность:" + realEps.ToString();  


            if (flag != -1)
                label8.Text = "Число итераций:" + flag.ToString(); 
            else
                label8.Text = "Число итераций:" + iterationCount.ToString();




            if (radioButton1.Checked)
                AddValueInTable(ref x, ref y, ref u, n, m); 
            if (radioButton2.Checked)
                AddValueInTable(ref x, ref y, ref v, n, m); 
            if (radioButton3.Checked)
            {
                double[,] errorMod = new double[n + 1, m + 1];
                for (int i = 0; i <= n; i++)
                    for (int j = 0; j <= m; j++)
                        errorMod[i, j] = Math.Abs(u[i, j] - v[i, j]);
                AddValueInTable(ref x, ref y, ref errorMod, n, m);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label6.Visible = true;
            label9.Visible = true;
            n = Convert.ToInt32(numericUpDown1.Value);
            m = Convert.ToInt32(numericUpDown2.Value);
            h = (b - a) / (double)n;
            k = (d - c) / (double)m;
            h2 = h / 2;
            k2 = k / 2;
            eps = Double.Parse(textBox1.Text);
            iterationCount = int.Parse(textBox2.Text);
            realEps = 0;

            double[] x = new double[n + 1]; 
            double[] y = new double[m + 1]; 
            for (int i = 0; i < n + 1; i++)
                x[i] = i * h + a;
            for (int j = 0; j < m + 1; j++)
                y[j] = j * k + c;

            double[] x2 = new double[2 * n + 1];
            double[] y2 = new double[2 * m + 1];
            for (int i = 0; i < 2 * n + 1; i++)
                x2[i] = i * h2 + a;
            for (int j = 0; j < 2 * m + 1; j++)
                y2[j] = j * k2 + c;

            double[,] f = new double[n + 1, m + 1];
            for (int i = 0; i < n + 1; i++)
                for (int j = 0; j < m + 1; j++)
                    f[i, j] = MainFunction(x[i], y[j]);

            double[,] f2 = new double[2 * n + 1, 2 * m + 1];
            for (int i = 0; i < 2 * n + 1; i++)
                for (int j = 0; j < 2 * m + 1; j++)
                    f2[i, j] = MainFunction(x2[i], y2[j]);

            double[,] v = new double[n + 1, m + 1];
            for (int i = 0; i < n + 1; i++)
            {
                v[i, 0] = Mu3(x[i]);
                v[i, m] = Mu4(x[i]);
            }
            for (int j = 1; j < m; j++)
            {
                v[0, j] = Mu1(y[j]);
                v[n, j] = Mu2(y[j]);
            }

            double[,] v2 = new double[2 * n + 1, 2 * m + 1];
            for (int i = 0; i < 2 * n + 1; i++)
            {
                v2[i, 0] = Mu3(x2[i]);
                v2[i, 2 * m] = Mu4(x2[i]);
            }
            for (int j = 1; j < 2 * m; j++)
            {
                v2[0, j] = Mu1(y2[j]);
                v2[2 * n, j] = Mu2(y2[j]);
            }

            int flag = ZeidelMethodForMainFunction(ref v, ref f, n, m, h, k);

            label5.Text = "Достигнутая погрешность для v:" + ZeidEps.ToString(); 

            if (flag != -1)
                label8.Text = "Число итераций для v:" + flag.ToString(); 
            else
                label8.Text = "Число итераций для v:" + iterationCount.ToString();

            flag = ZeidelMethodForMainFunction(ref v2, ref f2, 2 * n, 2 * m, h2, k2);

            label9.Text = "Достигнутая погрешность для v2(x, y):" + ZeidEps.ToString(); 

            if (flag != -1)
                label6.Text = "Число итераций для v2:" + flag.ToString();
            else
                label6.Text = "Число итераций для v2:" + iterationCount.ToString();

            for (int i = 0; i < n + 1; ++i)
                for (int j = 0; j < m + 1; ++j)
                {
                    if ((Math.Abs(v[i, j] - v2[2 * i, 2 * j]) > realEps))
                        realEps = Math.Abs(v[i, j] - v2[2 * i, 2 * j]);
                }
            label7.Text = "Реальная погрешность:" + realEps.ToString();


            if (radioButton4.Checked)
                AddValueInTable(ref x, ref y, ref v, n, m);
            if (radioButton5.Checked)
                AddValueInTable(ref x2, ref y2, ref v2, 2 * n, 2 * m);
            if (radioButton6.Checked)
            {
                double[,] errorMod = new double[n + 1, m + 1];
                for (int i = 0; i < n + 1; i++)
                    for (int j = 0; j < m + 1; j++)
                        errorMod[i, j] = Math.Abs(v[i, j] - v2[2 * i, 2 * j]);
                AddValueInTable(ref x, ref y, ref errorMod, n, m);
            }
        }
    }
}
