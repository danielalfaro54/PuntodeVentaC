﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PuntoDeVenta
{
    public partial class Form1 : Form
    {
        Conexion c = null;
        List<Producto> productos = null;
        List<Producto> vendidos = null;
        /*private String[,] productos =
        {
            {"1","Burro","100" },
            {"2","Hot dog","60" },
            {"3","Ensalada","130" },
            {"4","Boneless","120" },
            {"5","Hamburguesa","110" }
        };*/

        private float total()
        {
            float total = 0.0f;
            for(int i = 0; i< dataGridView1.Rows.Count; i++)
            {
                Console.WriteLine("Rows = " +dataGridView1.Rows.Count);
                total += float.Parse(dataGridView1[4, i].Value.ToString());
                Console.WriteLine("Total = " + total ) ;

            }
            label3.Text = "Total = " + total;
            textBox1.Clear();
            textBox1.Focus();
            return total;
        }

        private void buscarProductos()
        {
            if (textBox1.Text.IndexOf('*') != -1)
            {
                String[] sarray = textBox1.Text.Split('*');
                for (int i = 0; i < productos.Count; i++)
                {
                    try
                    {
                        if (int.Parse(sarray[1]) == productos[i].Codigo)
                        {
                            if (int.Parse(sarray[0]) > productos[i].Inventario)
                            {
                                MessageBox.Show("Solamente hay " + productos[i].Inventario + " en existencia de ese producto.");
                            }
                            else
                            {
                                Producto p = productos[i];
                                dataGridView1.Rows.Add(p.Nombre, sarray[0], p.Precio, (float)p.Precio * ((float)p.Descuento / (float)100), float.Parse(sarray[0]) * ((float)p.Precio - (float)p.Precio * (float)p.Descuento / (float)100));
                                total();
                                c.Reduce(p, int.Parse(sarray[0]));
                                vendidos.Add(p);
                            }

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingrese una cantidad válida");
                        textBox1.Clear();
                    }
                }
            }
            else
            {
                for (int i = 0; i < productos.Count; i++)
                {
                    try
                    {
                        if (int.Parse(textBox1.Text) == productos[i].Codigo)
                        {
                            if (productos[i].Inventario == 0)
                            {
                                MessageBox.Show("No hay de ese producto en existencia");
                            }
                            else
                            {
                                Producto p = productos[i];
                                dataGridView1.Rows.Add(p.Nombre, "1", p.Precio, (float)p.Precio * ((float)p.Descuento / (float)100), (float)p.Precio - ((float)p.Precio * (float)p.Descuento / (float)100));
                                total();
                                c.Reduce(p, 1);
                                vendidos.Add(p);
                            }

                        }
                        
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Ingrese un código válido");
                        textBox1.Clear();
                    }
                }
            }
            this.ActiveControl = textBox1;
            productos = c.getProductos();
        }
        public Form1()
        {
            InitializeComponent();
            this.ActiveControl = textBox1;
            c = new Conexion();
            productos = c.getProductos();
            vendidos = new List<Producto>();
            for (int i = 0; i < productos.Count; i++)
            {
                Console.WriteLine(productos[i].Nombre);
            }
            label3.Text = "Total =";
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Location = new Point((this.Width / 2) - label1.Width / 2, 5);
            label2.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
            label2.Location = new Point(label4.Width, label1.Height + 10);
            dataGridView1.Width = this.Width - 20;
            dataGridView1.Height = this.Height * 3 / 4;
            dataGridView1.Location = new Point(10, label2.Height + label1.Height + 25);
            textBox1.Width = this.Width - 20;
            textBox1.Location = new Point(10, this.Height - textBox1.Height - 10);
            
            label3.Location = new Point(this.Width - dataGridView1.Columns[3].Width, label1.Height + label2.Height + dataGridView1.Height + 35);
            label4.Location = new Point((this.Width * 3 / 4), label1.Height + 10);

        }

       private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                productos = c.getProductos();
                buscarProductos();
                textBox1.Text = String.Empty;
            }
            if(e.KeyCode  == Keys.Escape)
            {
                if (dataGridView1.Rows.Count >= 1)
                {
                   
                    Producto p = vendidos[vendidos.Count - 1];
                    Console.WriteLine("Inv = " + p.Codigo +","+p.Inventario+ ","+ dataGridView1[1, dataGridView1.Rows.Count - 1].Value.ToString());
                    c.Increment(p.Codigo,  p.Inventario);
                    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                    vendidos.Remove(p);
                    total();
                }
                


            }
            if (e.KeyCode == Keys.P)
            {
                float subtotal = total() / (float)1.16;
                float iva = total() - subtotal;
                DialogResult dialogResult = MessageBox.Show("¿Listo para pagar?", "Pagar", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Interaction.InputBox("Subtotal =" +subtotal +"\nIVA ="+iva+"\nTotal ="+total() , "¿Cómo va a pagar?", "$"+total());
                    
                    dataGridView1.Rows.Clear();
                    vendidos.RemoveRange(0, vendidos.Count);
                    
                    
                }
                else if (dialogResult == DialogResult.No)
                {
                    textBox1.Clear();
                }
                //MessageBox.Show("Vas a pagar");
                //textBox1.Text = String.Empty;
            }
            if (e.KeyCode == Keys.S)
            {
                DialogResult dialogResult = MessageBox.Show("¿Seguro que desea salir?", "Salir", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else if (dialogResult == DialogResult.No)
                {
                    textBox1.Clear();
                }
            }
        }
    }
}
