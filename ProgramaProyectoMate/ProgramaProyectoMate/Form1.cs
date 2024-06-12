using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramaProyectoMate
{ 
    public partial class Form1 : Form
    {     
        List<NuevaMedida> lMedida;
        List<double> alturas = new List<double>();
        Graphics g;
        Pen lapiz;
        int alturaTorre;
        int anchoPilar;
        int margen;
        const double towerX = -8.385137;
        const double endX = -8.374938;

        // Inicializa el programa
        public Form1()
        {
            InitializeComponent();
            lMedida = new List<NuevaMedida>();
            
            lapiz = new Pen(Color.Black, 2);
            alturaTorre = pictureBox1.Height * 2 / 3;
            anchoPilar = 30;
            margen = anchoPilar;
        }

        // Lee los datos escritos en los cuadros de texto
        private void button1_Click(object sender, EventArgs e)
        {
            NuevaMedida m = new NuevaMedida();
            try
            {
                m.latitud = Convert.ToDouble(textBoxLatitud.Text.Replace('.',','));
                m.longitud = Convert.ToDouble(textBoxLongitud.Text.Replace('.', ','));
                m.altoTubo = Convert.ToDouble(textBoxAltoTubo.Text.Replace('.', ','));
                m.largoTubo = Convert.ToDouble(textBoxLargoTubo.Text.Replace('.', ','));
                m.alturaMedidor = Convert.ToDouble(textBoxAlturaMedidor.Text.Replace('.', ','));
                if (m.longitud < towerX || m.longitud >endX) throw new Exception("Coordenadas fuera del dique");
                string[] x = new string[] { textBoxLatitud.Text, textBoxLongitud.Text, textBoxAltoTubo.Text, textBoxLargoTubo.Text, textBoxAlturaMedidor.Text };
                ListViewItem item = new ListViewItem(x);
                listView1.Items.Add(item);
                lMedida.Add(m);
                CalcularAltura();
                pictureBox1.Refresh();
            }
            catch(Exception ex)
            {   
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Calcula una nueva altura y recalcula la media añadiendo esta
        private void CalcularAltura()
        {
            alturas.Clear();
            foreach(NuevaMedida m in lMedida)
            {   
                double h = MathFunctions.TowerHeight(m.longitud, m.latitud, m.altoTubo/m.largoTubo);
                alturas.Add(h+m.alturaMedidor);
            }
            double media = MathFunctions.AverageValue(alturas);
            labelMediaAlturas.Text = String.Format("{0:0.00}m", media);
        }

        // Reinicia el programa sin cerrarlo
        private void toolStripButtonInicalizar_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            labelMediaAlturas.Text = "";
            lMedida.Clear();
            pictureBox1.Refresh();
        }

        // Escribe valores de muestra
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            textBoxLatitud.Text = "43,368053"; textBoxLongitud.Text = "-8,382838"; textBoxAltoTubo.Text = "14,6"; textBoxLargoTubo.Text = "37,5"; textBoxAlturaMedidor.Text = "0,35";
            button1_Click(null, null);

            textBoxLatitud.Text = "43,367904"; textBoxLongitud.Text = "-8,382432"; textBoxAltoTubo.Text = "11,5"; textBoxLargoTubo.Text = "37,1"; textBoxAlturaMedidor.Text = "0,35";
            button1_Click(null, null);

            textBoxLatitud.Text = "43,367571"; textBoxLongitud.Text = "-8,381546"; textBoxAltoTubo.Text = "9,1"; textBoxLargoTubo.Text = "36,5"; textBoxAlturaMedidor.Text = "0,35";
            button1_Click(null, null);

            textBoxLatitud.Text = "43,367308"; textBoxLongitud.Text = "-8,380852"; textBoxAltoTubo.Text = "7,1"; textBoxLargoTubo.Text = "35,8"; textBoxAlturaMedidor.Text = "0,35";
            button1_Click(null, null);

            textBoxLatitud.Text = "43,366975"; textBoxLongitud.Text = "-8,379966"; textBoxAltoTubo.Text = "5,9"; textBoxLargoTubo.Text = "35,1"; textBoxAlturaMedidor.Text = "0,35";
            button1_Click(null, null);

            textBoxLatitud.Text = "43,366686"; textBoxLongitud.Text = "-8,379194"; textBoxAltoTubo.Text = "4,8"; textBoxLargoTubo.Text = "34,6"; textBoxAlturaMedidor.Text = "0,35";
            button1_Click(null, null);

            textBoxLatitud.Text = "43,366375"; textBoxLongitud.Text = "-8,378357"; textBoxAltoTubo.Text = "4,4"; textBoxLargoTubo.Text = "34,2"; textBoxAlturaMedidor.Text = "0,35";
            button1_Click(null, null);
        }

        // Muestra la ventana de error
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            AcercaDe acercade = new AcercaDe();
            acercade.ShowDialog();
        }


        // Pinta la representación
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            g = pictureBox1.CreateGraphics();
            g.Clear(Color.WhiteSmoke);
            DrawScenary();
            DrawMesures();
        }

        private void DrawScenary()
        {

            Rectangle rectIzquierdo = new Rectangle(margen, pictureBox1.Height - alturaTorre, anchoPilar, pictureBox1.Height),
                rectCentral = new Rectangle(margen + anchoPilar, pictureBox1.Height - alturaTorre*6/10, anchoPilar * 4, anchoPilar * 4),
                rectDerecho = new Rectangle(margen + anchoPilar * 5, pictureBox1.Height - alturaTorre, anchoPilar, pictureBox1.Height);
            g.DrawRectangle(lapiz, rectDerecho);
            g.DrawRectangle(lapiz, rectCentral);
            g.DrawRectangle(lapiz, rectIzquierdo);
            g.DrawLine(lapiz, 0, pictureBox1.Height - 1, pictureBox1.Width, pictureBox1.Height - 1);
        }

        private void DrawMesures()
        {
            for (int i=0; i<lMedida.Count; i++)
            {
                NuevaMedida m = lMedida[i];
                double h = Math.Round(alturas[i]);
                double posMedida = towerX- m.longitud;
                double largoGrados = towerX - endX;
                int xPintado = Convert.ToInt32((pictureBox1.Width- margen + anchoPilar * 3) * posMedida / largoGrados)+ margen + anchoPilar * 3;
                try
                {
                    g.DrawLine(lapiz, xPintado, pictureBox1.Height - 1, margen + anchoPilar * 3, pictureBox1.Height - Convert.ToSingle(h) * 4.33333f);
                }
                catch
                {
                    MessageBox.Show( "No se puede calcular con los datos introducidos.\nSe ignorará la medición:\n\n"+m.tostring(), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lMedida.RemoveAt(i);
                    i--;
                }
            }
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1_Paint(null, null);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            System.Diagnostics.Process.Start(dir + "\\manual.pdf");

        }
    }

    // Guarda cada componente de cada medida
    public class NuevaMedida
    {
        public double latitud, longitud, largoTubo, altoTubo, alturaMedidor;
        public string tostring()
        {
            return latitud + " ; " + longitud + " ; " + largoTubo + " ; " + altoTubo + " ; " + alturaMedidor;
        }
    }

    // Funciones matemáticas
    public class MathFunctions
    {
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

        private static double DistanceToTower(double x, double y)
        {
            const double EARTHRADIO = 6371000;
            double towerX = ToRadians(-8.385137);
            double towerY = ToRadians(43.368952);
            x = ToRadians(x);
            y = ToRadians(y);
            double dist = 2*EARTHRADIO*Math.Asin(Math.Sqrt(  Square(Math.Sin((y-towerY)/2)) + Math.Cos(towerY)*Math.Cos(y)*Square(Math.Sin((x-towerX)/2))  ));
            return dist;
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
            double distToBase = DistanceToTower(posX, posY);
            return distToBase * tan;
        }
    }
}