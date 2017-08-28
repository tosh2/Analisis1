using Analisis_1.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Analisis_1.Modulos
{
    public partial class insertarCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAgregarCliente_Click(object sender, EventArgs e)
        {
            String nombre = txtNombre.Text;
            String apellido = txtApellido.Text;
            String nit = txtNit.Text;
            String dpi = txtDPI.Text;
            String telefono = txtTelefono.Text;

            Conexion Con = new Conexion();

            bool respuesta = Con.Crear("CLIENTE", 
                                        "nombre, apellidos, nit, dpi,telefono", "'" + nombre + "', " + "'" + apellido + "', " + "'" + nit + "', " + "'" + dpi + "', " + "'" + telefono + "'");

            txtNombre.Text = "";
            txtApellido.Text = "";
            txtNit.Text = "";
            txtDPI.Text = "";
            txtTelefono.Text = "";

            if (respuesta)
            {
                this.Page.Response.Write("<script language='JavaScript'>window.alert('Cliente insertado con éxito');</script>");

            }
            else
            {
                this.Page.Response.Write("<script language='JavaScript'>window.alert('Nit o DPi ya estan registrados');</script>");

            }
        }
    }
}