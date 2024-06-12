using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramaProyectoMate
{
    public partial class Form1 : Form
    {
        List<NuevaMedida> lMedida;

        public Form1()
        {
            InitializeComponent();
            lMedida = new List<NuevaMedida>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NuevaMedida m = new NuevaMedida();
            try
            {
                m.latitud = Convert.ToDouble(textBoxLatitud.Text);
                m.longitud = Convert.ToDouble(textBoxLongitud.Text);
                m.altoTubo = Convert.ToDouble(textBoxAltoTubo.Text);
                m.largoTubo = Convert.ToDouble(textBoxLargoTubo.Text);
                m.alturaMedidor = Convert.ToDouble(textBoxAlturaMedidor.Text);
                listBox1.Items.Add(m.tostring());
                lMedida.Add(m);
                CalcularAltura();
            }
            catch
            {
                MessageBox.Show("Valores introducidos no válidos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                textBoxLatitud.Text = "";
                textBoxLongitud.Text = "";
                textBoxAltoTubo.Text = "";
                textBoxLargoTubo.Text = "";
                textBoxAlturaMedidor.Text = "";
            }
        }

        private void CalcularAltura()
        {
            List<double> alturas = new List<double>();
            foreach(NuevaMedida m in lMedida)
            {   
                double h = MathFunctions.TowerHeight(m.longitud, m.latitud, m.altoTubo/m.largoTubo);
                alturas.Add(h);
            }
            double media = MathFunctions.AverageValue(alturas);
            labelMediaAlturas.Text = Convert.ToString(media);
        }

        private void toolStripButtonInicalizar_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            labelMediaAlturas.Text = "";
            lMedida.Clear();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            textBoxLatitud.Text = "1"; textBoxLongitud.Text = "2"; textBoxAltoTubo.Text = "3"; textBoxLargoTubo.Text = "4"; textBoxAlturaMedidor.Text = "5";
            button1_Click(null, null); 
            textBoxLatitud.Text = "1"; textBoxLongitud.Text = "2"; textBoxAltoTubo.Text = "3"; textBoxLargoTubo.Text = "4"; textBoxAlturaMedidor.Text = "5";
            button1_Click(null, null); 
            textBoxLatitud.Text = "1"; textBoxLongitud.Text = "2"; textBoxAltoTubo.Text = "3"; textBoxLargoTubo.Text = "4"; textBoxAlturaMedidor.Text = "5";
            button1_Click(null, null);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            AcercaDe acercade = new AcercaDe();
            acercade.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            pictureBox1_Paint(null, null);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawLine(new Pen(Color.Black), 50, 425, 100, 425);
            g.DrawLine(new Pen(Color.Black), 50, 425, 50, 600);
            g.DrawLine(new Pen(Color.Black), 100, 425, 100, 600);

            g.DrawLine(new Pen(Color.Black), 0, 600, pictureBox1.Width, 600);

        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1_Paint(null, null);
        }
    }

    public class NuevaMedida
    {
        public double latitud, longitud, largoTubo, altoTubo, alturaMedidor;
        public string tostring()
        {
            return latitud + " - " + longitud + " - " + largoTubo + " - " + altoTubo + " - " + alturaMedidor;
        }
    }

    public class MathFunctions
    {
        private static double AbsoluteValue(double num)
        {
            if (num < 0) return num * -1;
            return num;
        }

        public static double AverageValue(List<double> list)
        {
            double acum = 0;
            int listElements = 0;
            foreach (double i in list)
            {
                acum += i;
                listElements++;
            }
            return acum / listElements;
        }

        private static double CircunferenceLenght(double radius)
        {
            return 2 * Math.PI * radius;
        }

        private static double DistanceToTower(double x, double y)
        {
            const double EARTHRADIO = 6371000;
            const double towerX = -8.385137;
            const double towerY = 43.368952;
            double distX = AbsoluteValue(towerX - x) * CircunferenceLenght(EARTHRADIO) / 360;
            double distY = AbsoluteValue(towerY - y) * CircunferenceLenght(EARTHRADIO) / 360;
            return Hypotenuse(distX, distY);
        }

        private static double DistanceToTower2(double x, double y)
        {
            //const double EARTHRADIO = 6371000; 
            //double towerX = ToRadians(-8.385137);
            //double towerY = ToRadians(43.368952);
            //x = ToRadians(x);
            //y = ToRadians(y);
            //double deltaX = AbsoluteValue(towerX - x);
            //double deltaY = AbsoluteValue(towerY - y);
            //double temp = Math.Pow(Math.Sin(deltaY / 2), 2) + Math.Cos(towerY) + Math.Cos(y) + Math.Pow(Math.Sin(deltaX / 2), 2);
            //return EARTHRADIO * 2 * Math.Atan2(Math.Sqrt(temp), (Math.Sqrt(1 - temp)));




            const double EARTHRADIO = 6371000;
            double towerX = -8.385137;
            double towerY = 43.368952;

            double difLatitud = ToRadians(towerY - y);






    }

        private static double Hypotenuse(double num1, double num2)
        {
            return Math.Sqrt(Square(num1) + Square(num2));
        }

        private static double Square(double num)
        {
            return num * num;
        }

        private static double ToRadians(double angle)
        {
            return angle * Math.PI / 180;
        }

        public static double TowerHeight(double posX, double posY, double tan)
        {
            double distToBase = DistanceToTower2(posX, posY);
            return distToBase * tan;
        }
    }
}