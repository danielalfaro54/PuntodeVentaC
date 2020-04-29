using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PuntoDeVenta
{
    class Conexion
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public Conexion()
        {
            iniciar();
        }

        private void iniciar()
        {
            try
            {
                server = "bin8acqfuoisdxxg0vgs-mysql.services.clever-cloud.com";
                database = "bin8acqfuoisdxxg0vgs";
                uid = "utbd8vd2wnzjdyy1";
                password = "lyx8T96JZqwDz0AE5fU4";
                string connectionString;
                connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                connection = new MySqlConnection(connectionString);
                
            }
            catch (MySqlException)
            {
                Console.WriteLine("Not Connected");
            }
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Reduce(Producto p, int bought)
        {
            int codigo = p.Codigo;
            string nombre = p.Nombre;
            int inventario = p.Inventario-bought;
            int precio = p.Precio;
            int descuento = p.Descuento;

            string query = "UPDATE producto SET inventario =" + inventario + " WHERE idproducto = " + codigo ;

            if(this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }


            

        }
        public void Increment(int code, int invent)
        {
           
            
            string query = "UPDATE producto SET inventario = "+invent
                + " WHERE idproducto = " + code;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }




        }

        public List<Producto> getProductos()
        {
            String query = "SELECT * FROM producto";

            List<Producto> productos= new List<Producto>();

            
            if(this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Producto p = new Producto();
                    p.Codigo = (int)dataReader["idproducto"];
                    p.Nombre = (String)dataReader["Nombre"];
                    p.Inventario = (int)dataReader["inventario"];
                    p.Precio = (int)dataReader["unidad"];
                    p.Descuento = (int)dataReader["descuento"];
                    productos.Add(p);
                }
                dataReader.Close();
                this.CloseConnection();
                return productos;
            }
            else
            {
                return productos;
            }
        }



    }
}
