using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Tarea7_DavidRangel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string ruta = Application.StartupPath + @"\archivo\tabla.csv";
        List<string> listRuts;
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != String.Empty && txtRut.Text != String.Empty && txtNota.Text != String.Empty)
            {
                if (!listRuts.Contains(txtRut.Text))
                {
                    StreamWriter sw = File.AppendText(ruta);
                    string registro = String.Format("{0};{1};{2}", txtRut.Text, txtNombre.Text, (double.Parse(txtNota.Text)).ToString());
                    sw.WriteLine(registro);
                    sw.Close();
                    listRuts.Add(txtRut.Text);
                }
                else
                {
                    if (MessageBox.Show("Rut ya fue ingresado. ¿Desea modificar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        StreamReader sr = new StreamReader(ruta);          
                        string lectura;
                        lectura = sr.ReadLine();
                        int i = 0;
                        int posicion = 0;
                        string[,] tabla = new string[listRuts.Count+1, 3];
                        while (lectura != null)
                        {
                            string[] registro = lectura.Split(';');

                            for (int col = 0; col < 3; col++)
                            {
                                tabla[i, col] = registro[col];
                            }

                            if (registro[0] == txtRut.Text)
                            {
                                posicion = i;
                            }
                            i++;
                            lectura = sr.ReadLine();
                        }
                        sr.Close();

                        tabla[posicion, 1] = txtNombre.Text;
                        tabla[posicion, 2] = txtNota.Text;

                        string texto = "";
                        for (int fil = 0; fil < tabla.GetLength(0); fil++)
                        {
                            texto += String.Format("{0};{1};{2}\n", tabla[fil, 0], tabla[fil, 1], tabla[fil, 2]);
                        }
                        File.WriteAllText(ruta, texto);
                    }
                }
                txtRut.Clear();
                txtNombre.Clear();
                txtNota.Clear();
                txtRut.Focus();
            }
            else
            {
                MessageBox.Show("Campo vacio");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listRuts = new List<string>();
            bool existe = File.Exists(ruta);

            if (existe)
            {
                StreamReader sr = new StreamReader(ruta);
                string lectura;
                lectura = sr.ReadLine();
                if (lectura != "rut;nombre;nota")
                {
                    string headers = "rut;nombre;nota\n";
                    sr.Close();
                    File.WriteAllText(ruta, headers);
                }
                else
                {
                    lectura = sr.ReadLine();
                    while (lectura != null)
                    {
                        string[] registro = lectura.Split(';');
                        listRuts.Add(registro[0]);
                        lectura = sr.ReadLine();
                    }
                    sr.Close();
                }
            }
            else
            {
                string headers = "rut;nombre;nota\n";
                File.WriteAllText(ruta, headers);
            }
                     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            StreamReader sr = new StreamReader(ruta);
            String lectura;
            lectura = sr.ReadLine();
            dataGridView1.ColumnCount = 3;
            while (lectura != null)
            {
                string[] registro = lectura.Split(';');

                if (registro[0] == "rut")
                {
                    dataGridView1.Columns[0].HeaderText = registro[0];
                    dataGridView1.Columns[1].HeaderText = registro[1];
                    dataGridView1.Columns[2].HeaderText = registro[2];
                }
                else
                {
                    dataGridView1.Rows.Add(registro);
                }
                lectura = sr.ReadLine();
            }
            sr.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] res = buscarRut(textBox1.Text);

            txtRut.Text = res[0];
            txtNombre.Text = res[1];
            txtNota.Text = res[2];
        }

        string[] buscarRut(string rut)
        {
            StreamReader sr = new StreamReader(ruta);
            string lectura;
            lectura = sr.ReadLine();

            string nombreFinded = "", rutFinded = "", notaFinded = "";
            while (lectura != null)
            {
                string[] registro = lectura.Split(';');

                if (registro[0] == rut)
                {
                    rutFinded = registro[0];
                    nombreFinded = registro[1];
                    notaFinded = registro[2];
                }
                lectura = sr.ReadLine();
            }
            sr.Close();
            return new string[] { rutFinded, nombreFinded, notaFinded };
        }

        private void button4_Click(object sender, EventArgs e)
        {
            

            string[] resultados = buscarRut(txtRut.Text);

            if (resultados[0] != "" && MessageBox.Show("¿Desea eliminar registro?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                StreamReader sr = new StreamReader(ruta);
                string lectura;
                lectura = sr.ReadLine();
                string[,] tabla = new string[listRuts.Count, 3];
                int i = 0;
                while (lectura != null)
                {
                    string[] registro = lectura.Split(';');

                    for (int col = 0; col < 3; col++)
                    {
                        if (registro[0] != txtRut.Text)
                        {
                            tabla[i, col] = registro[col];                   
                        }
                        else
                        {
                            listRuts.Remove(txtRut.Text);
                        }
                    }
                    lectura = sr.ReadLine();
                    if (registro[0] != txtRut.Text)
                    {
                        i++;
                    }                   
                }
                sr.Close();

                string texto = "";
                for (int fil = 0; fil < tabla.GetLength(0); fil++)
                {
                    texto += String.Format("{0};{1};{2}\n", tabla[fil, 0], tabla[fil, 1], tabla[fil, 2]);
                }
                File.WriteAllText(ruta, texto);
            }          
        }
    }
}
