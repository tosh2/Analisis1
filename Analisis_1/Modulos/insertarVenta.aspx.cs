using Analisis_1.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Analisis_1.Modulos
{

    public class Product
    {
        public String idventa;
        public String cantidad;
        public String codigo;
        public String nombre;
        public Double subTotal;
        public string precio;
        public string usuario;
        public Product(String idventa, String cant, String cod, String nombre, string precio)
        {
            this.precio = precio;
            cantidad = cant;
            codigo = cod;
            this.nombre = nombre;
            this.idventa = idventa;

            Double Cantidad = Convert.ToDouble(cantidad, CultureInfo.InvariantCulture);

            Double Precio = Convert.ToDouble(precio);


            Double Total = 0.0;
            Total = Cantidad * Precio;

            this.subTotal = Math.Ceiling(Total * 2) / 2.0;

        }


    }

    public partial class insertarVenta : System.Web.UI.Page
    {
        private List<string> _countryItems;

        public List<string> CountryItems
        {
            get
            {
                if (_countryItems == null)
                {
                    _countryItems = (List<string>)Session["CountryItems"];
                    if (_countryItems == null)
                    {
                        _countryItems = new List<string>();
                        Session["CountryItems"] = _countryItems;
                    }
                }
                return _countryItems;
            }
            set { _countryItems = value; }
        }

        Conexion conexion;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["Carrito"] = new List<Product>();


                conexion = new Conexion();
                DataSet Productos = conexion.Consulta("select * from Producto");
                String html = "<select  runat=\"server\" class=\"form - control\" style=\"font-size: 15px;\" data-placeholder=\"Agregar producto\"   onChange=\"cambios()\"  id=\"cmbproductos\" >";
                html += "<option value=\"\"></option> ";

                if (!Productos.HasErrors) {

                    foreach (DataRow item in Productos.Tables[0].Rows)
                    {
                        DataSet Categoria = conexion.Consulta("select id_categoria from Producto where id = " + item["id"]);
                        String Categor = Convert.ToString(Categoria.Tables[0].Rows[0][0]);
                        DataSet DescripcionTipo = conexion.Consulta("select Nombre from Categoria where id= " + Categor);
                        String Descripcion = Convert.ToString(DescripcionTipo.Tables[0].Rows[0][0]);
                        DataSet Cantidad = conexion.Consulta("select Cantidad from Producto where id = " + item["id"]);
                        int cantidad = Convert.ToInt16(Cantidad.Tables[0].Rows[0][0]);

                        html += "<option value=\"" + item["id"] + "\">" + item["Nombre"] + "</option> ";


                    }

                }               

                html += "</select>";

                this.productos.InnerHtml = html;
            }
        }




        [WebMethod]
        public static string MostrarModalPago(string cliente)
        {
            Conexion nueva = new Conexion();
            Double total = 0;

            List<Product> carrito = (HttpContext.Current.Session["Carrito"] != null) ? (List<Product>)HttpContext.Current.Session["Carrito"] : null;

            foreach (var item in carrito)
            {
                total += item.subTotal;
                /*
                if (item.usuario.Equals(user))
                {
                    total += item.subTotal;
                }*/
            }

            if (total == 0) { return "0"; }

            string innerhtml =
                "<div class=\"modal fade\" id=\"ModalPago\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"myModalLabel\" aria-hidden=\"true\"> \n" +
                "<div class=\"modal-dialog\"> \n" +
                "<div class=\"modal-content\"> \n" +
                "<div class=\"modal-header\"> \n" +
                "<button type=\"button\" onclick=\"closeModalPago();\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button> \n" +
                "<div class=\"step-title\"> \n" +
                "<h3 class=\"box-title\">Area Pago</h3> \n" +
                "</div> \n" +
                "</div>\n"

                 ;
            //content del modal

            innerhtml +=
                "<form id=\"formulario_modal\" class=\"sidebar - form\"> \n" +
                "<div class=\"modal-body\"> \n" +
                "<table border=\"0\" width=\"100%\" > \n" +
                "<div> \n" +
                //ID DEL CLIENTE
                "<div class=\"col-md-12\"> \n" +
                "<div class=\"form-group\">\n" +
                "<label class=\"control-label\" style=\"font-size: 15px;\" ><b>NIT Cliente</b></label> \n" +
                "<div class=\"controls\"><input  style=\"font-size: 15px disabled =\"disabled\" readonly=\"readonly\"  type=\"text\" value=\"" + cliente + "\" id=\"codclientepago\" runat=\"server\" class=\"form - control\"/></div> \n" +
                "</div> \n" +
                "</div> \n" +


                "<div class=\"col-md-12\"> \n" +
                "<div class=\"form-group\">\n" +
                "<label class=\"control-label\" style=\"font-size: 15px;\" ><b>Total:</b></label> \n" +
                "<div class=\"controls\"><input placeholder=\"Total\" disabled =\"disabled\" readonly=\"readonly\" style=\"font-size: 15px;\" value=\"" + total + "\"type=\"text\" name=\"total\" id=\"totalpago\" runat=\"server\" class=\"form - control\" /></div> \n" +
                "</div> \n" +
                "</div> \n" +
                //

                "<div class=\"col-md-12\"> \n" +
                "<div class=\"form-group\">\n" +
                "<label class=\"control-label\" style=\"font-size: 15px;\" ><b>Pago (Q.): </b></label> \n" +
                "<div class=\"controls\"><input placeholder=\"Cantidad\" style=\"font-size: 15px;\" type=\"text\" name=\"total\" id=\"totalabonado\" runat=\"server\" class=\"form - control\" /></div> \n" +
                "</div> \n" +
                "</div> \n" +


                "<tr> \n" +
                "<td colspan=\"2\"> \n" +
                "<div style=\"font-size: 15px;\" id=\"mensaje\"></div> \n" +
                "<div style=\"font-size: 20px;\" id=\"vuelto\"></div> \n" +
                "<div style=\"font-size: 25px;\" id=\"venta\"></div> \n" +

                "<div class=\"alert margin\"> \n" +
                "<button type=\"button\"  class=\"close\" data-dismiss=\"alert\">×</button> \n" +
                "Campos Obligatorios (*) \n" +
                "</div> \n" +
                "<button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\" onclick=\"closeModalPago();\" id=\"cerrar_modal\">Cerrar</button>\n" +
                "<button type=\"button\" class=\"btn btn-large btn-success\" onclick=\"AddPago();\" name=\"reg\" id=\"reg_modal\">Registrar</button>\n" +
                "</td> \n" +
                "</tr> \n" +
                "</div> \n" +
                "</table> \n" +
                "</div> \n"
                ;


            //footer del modal
            innerhtml += "</div>\n" +
                "</div>\n" +
                "</div>\n"
            ;

            return innerhtml;
        }

        [WebMethod]
        public static string AddProducto(String idventa, String producto, String cantidad, String codigo)
        {


            if (true)
            {
                try
                {
                    Conexion conn = new Conexion();

                    DataSet Producto_ = conn.Buscar_Mostrar("Producto", "id" + "= " + codigo);
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(Producto_.GetXml());
                    XmlNodeList _Producto = xDoc.GetElementsByTagName("NewDataSet");
                    XmlNodeList lista_producto = ((XmlElement)_Producto[0]).GetElementsByTagName("id_x003D__x0020_" + codigo);


                    XmlNodeList nDescripcion = ((XmlElement)lista_producto[0]).GetElementsByTagName("descripcion");
                    XmlNodeList categoria_id = ((XmlElement)lista_producto[0]).GetElementsByTagName("id_categoria");

                    DataSet Tipo_ = conn.Buscar_Mostrar("Categoria", "id" + "= " + categoria_id[0].InnerText);
                    XmlDocument xTipo = new XmlDocument();
                    xTipo.LoadXml(Tipo_.GetXml());
                    XmlNodeList _Tipo = xTipo.GetElementsByTagName("NewDataSet");
                    XmlNodeList nombreCategoria = ((XmlElement)_Tipo[0]).GetElementsByTagName("nombre");

                    string tipo = nombreCategoria[0].InnerText;

                    List<Product> carrito = (HttpContext.Current.Session["Carrito"] != null) ? (List<Product>)HttpContext.Current.Session["Carrito"] : null;

                    //string user = HttpContext.Current.Session["IdUser"].ToString();
                    Conexion nueva = new Conexion();
                    DataSet datos = nueva.Consulta("select precio from producto where id=" + codigo);
                    String precio = datos.Tables[0].Rows[0][0].ToString();
                    carrito.Add(new Product(idventa, cantidad, codigo, producto, precio));

                    HttpContext.Current.Session["Carrito"] = carrito;
                }
                catch (Exception e)
                {
                    string error = e.ToString();
                }

            }
            return Graficar();
        }



        public static string Graficar()
        {

            string str = "<div class=\"widget\">" +
                    "<div class=\"box box-default\">" +
                    "    <div class=\"box-header with-border\">" +
                    "       <h6 class=\" box-title\">Detalle</h6>" +
                    "        <div class=\"nav pull-right\">" +
                    "        </div>" +
                    "           <div class=\"box-tools pull-right\">" +
                    "               <button type = \"button\" class=\"btn btn-box-tool\" data-widget=\"collapse\"><i class=\"fa fa-minus\"></i></button>" +
                    "               <button type = \"button\" class=\"btn btn-box-tool\" data-widget=\"remove\"><i class=\"fa fa-remove\"></i></button>" +
                     "          </div>" +
                    "    </div>" +
                    "</div>" +
                    "<div class=\"table-overflow\">" +
                    "    <table class=\"table table-striped table-bordered align-center\">" +
                    "        <thead>" +
                    "           <tr>" +
                                     "<th>Id</th>" +
                    "                <th>Nombre</th>" +
                    "                <th>Cantidad</th>" +
                    "                <th>Precio Unitario</th>" +
                    "                <th>Precio Total</th>" +
                    "                <th>Acciones</th>" +
                    "            </tr>" +
                    "        </thead>" +
                    "        <tbody>";



            List<Product> carrito = (HttpContext.Current.Session["Carrito"] != null) ? (List<Product>)HttpContext.Current.Session["Carrito"] : null;


            for (int i = 0; i < carrito.Count; i++)
            {

                try
                {
                    string cant = carrito[i].cantidad;
                    str += "            <tr>" +
                                    "                <td>" + carrito[i].idventa + "</td>" +
                                    "                <td>" + carrito[i].nombre + "</td>" +
                                    "                <td>" +
                                                        cant +
                                    "                </td>" +
                                    "               <td>" + carrito[i].precio + "</td>" +
                                    "               <td>" + carrito[i].subTotal + "</td>" +

                                                    "<td>" +
                                                        "   <ul class=\"table-controls\">" +
                                                  "          <li><a href=\"javascript:removecarrito('" + carrito[i].idventa + "')\"class=\"tip\" title=\"Remover\"><i class=\"glyphicon glyphicon-remove\"></i></a> </li>" +
                                                 "       </ul>" +
                                                "    </td>" +
                                                " </tr>";
                }
                catch (Exception e)
                {

                }


            }

            str += "</tbody>" +
     " </table>" +
     " </div>" +
     "</div>";
            return str;

        }



        [WebMethod]
        public static string Busca_Datos(string id)
        {
            Conexion conn = new Conexion();

            DataSet Producto_ = conn.Buscar_Mostrar("Producto", "id" + "= " + id);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(Producto_.GetXml());
            XmlNodeList _Producto = xDoc.GetElementsByTagName("NewDataSet");
            XmlNodeList lista_producto = ((XmlElement)_Producto[0]).GetElementsByTagName("Producto_x003D__x0020_" + id);

            XmlNodeList cantidad = ((XmlElement)_Producto[0]).GetElementsByTagName("cantidad");

            XmlNodeList nDescripcion = ((XmlElement)_Producto[0]).GetElementsByTagName("descripcion");

            XmlNodeList nTipo = ((XmlElement)_Producto[0]).GetElementsByTagName("id_categoria");




            DataSet Tipo_ = conn.Buscar_Mostrar("Categoria", "id" + "= " + nTipo[0].InnerText);
            XmlDocument xTipo = new XmlDocument();
            xTipo.LoadXml(Tipo_.GetXml());
            XmlNodeList _Tipo = xTipo.GetElementsByTagName("NewDataSet");
            XmlNodeList nombreCategoria = ((XmlElement)_Tipo[0]).GetElementsByTagName("nombre");

            DataSet Inventario = conn.Buscar_Mostrar("Producto", "id" + "= " + id);
            XmlDocument xInventario = new XmlDocument();
            xInventario.LoadXml(Inventario.GetXml());
            string verifica = Inventario.GetXml();
            string precio;

            if (Inventario.Tables[0].Rows.Count > 0)
            {
                precio = "1";
            }
            else
            {
                precio = "0";
            }

            string[] producto = new string[4];
            try
            {
                producto[0] = nDescripcion[0].InnerText;
                producto[1] = cantidad[0].InnerText;
                producto[2] = nombreCategoria[0].InnerText;
                producto[3] = precio;
            }
            catch (Exception ex)
            {

                string MostrarError = "Mensaje de la excepcion: " + ex.Message.ToString();
            }


            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(producto);
            return json;



        }
        //VENTAS
        [WebMethod]
        public static string GetVenta()
        {

            string venta;
            Conexion nueva = new Conexion();
            DataSet ConVenta = nueva.Consulta("(select max(Venta) from venta)");
            venta = Convert.ToString(ConVenta.Tables[0].Rows[0][0]);

            return venta;

        }





        //AGREGAR pago




        [WebMethod]
        public static bool AddPago(string total, string cliente)
        {
            Conexion nueva = new Conexion();
            bool respuesta;

            Double totalventa = 0;

            List<Product> carrito = (HttpContext.Current.Session["Carrito"] != null) ? (List<Product>)HttpContext.Current.Session["Carrito"] : null;


            foreach (var item in carrito)
            {
                totalventa += item.subTotal;

            }

            if (totalventa == 0) { return false; }

            respuesta = nueva.Crear("Venta", "nitCliente, fechaVenta, Total, vendedordpi ", "'" + cliente + "', GETDATE()," + Convert.ToString(total).Replace(",", ".") + ", " + "'2618007790101'");
            if (respuesta == true)
            {
                foreach (var item in carrito)
                {

                    respuesta = nueva.Crear("Detalle", "numeroVenta, idProducto, cantidad, subtotal", "(select max(Venta) from venta)," + item.codigo + "," + item.cantidad + " , " + Convert.ToString(item.subTotal).Replace(",", "."));


                    respuesta = nueva.Modificar("Producto", " Cantidad = Cantidad - " + item.cantidad + " ", " id = " + item.codigo + " ");


                }


            }   

            carrito.Clear();

            HttpContext.Current.Session["Carrito"] = carrito;

            return respuesta;
        }



        [WebMethod]
        public static string CleanCarrito()
        {
            List<Product> carrito = (HttpContext.Current.Session["Carrito"] != null) ? (List<Product>)HttpContext.Current.Session["Carrito"] : null;
            /*
            string user = HttpContext.Current.Session["IdUser"].ToString();
            for (int i = 0; i < carrito.Count; i++)
            {
                if (carrito[i].usuario.Equals(user))
                {
                    carrito.RemoveAt(i);
                }

            }
             * */
            carrito.Clear();
            HttpContext.Current.Session["Carrito"] = carrito;
            return Graficar();

        }


        [WebMethod]
        public static string removecarrito(String codigo)
        {
            List<Product> carrito = (HttpContext.Current.Session["Carrito"] != null) ? (List<Product>)HttpContext.Current.Session["Carrito"] : null;


            for (int i = 0; i < carrito.Count; i++)
            {

                if (carrito[i].idventa.Equals(codigo))
                {
                    carrito.RemoveAt(i);
                }
                /*
                if (carrito[i].usuario.Equals(user)) {
                    if (carrito[i].idventa.Equals(codigo))
                    {
                        carrito.RemoveAt(i);
                    }
                }*/


            }

            HttpContext.Current.Session["Carrito"] = carrito;
            return Graficar();
        }

        [WebMethod]
        public static Double quitarcantidad(String codigo)
        {
            string cantidad;

            Double total = 0;

            List<Product> carrito = (HttpContext.Current.Session["Carrito"] != null) ? (List<Product>)HttpContext.Current.Session["Carrito"] : null;

            for (int i = 0; i < carrito.Count; i++)
            {
                if (carrito[i].idventa.Equals(codigo))
                {

                    cantidad = carrito[i].cantidad;
                    total = Convert.ToDouble(cantidad, CultureInfo.InvariantCulture);
                    break;
                }
                /*
                if (carrito[i].usuario.Equals(user)) {
                    if (carrito[i].idventa.Equals(codigo))
                    {

                        cantidad = carrito[i].cantidad;
                        largo = carrito[i].largo;
                        total = Convert.ToDouble(largo, CultureInfo.InvariantCulture) * Convert.ToDouble(cantidad, CultureInfo.InvariantCulture);
                        break;
                    }

                }
                */

            }


            HttpContext.Current.Session["Carrito"] = carrito;

            return total;
        }


    }
}