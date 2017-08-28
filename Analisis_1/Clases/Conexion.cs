using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Analisis_1.Clases
{
    public class Conexion
    {
        /*
         * QUE ONDA MUCHA !!! 
         * CUANDO CREEN LA CADENA DE CONEXION PONGANLE DE NOMBRE 
         * "SistemaERPConnectionString",
         * SI NO SABEN COMO CREARLA VAYANSE HASTA ARRIBA Y 
         * SELECCIONEN LA PESTANIA Tools/Herramientas Y DENLE EN
         * CONECTAR A LA BASE DE DATOS, BUSCAN EL NOMBRE DEL 
         * SERVIDOR Y LA BD.... DE ALLI LE PONEN ESE NOMBRE....
         */

        private string mostrarError;

        SqlConnection conexion = new SqlConnection();


        public string MostrarError

        {
            get { return mostrarError; }
            set { mostrarError = value; }
        }


        private bool ConectarServer()
        {
            bool respuesta = false;
            //JARVIS\SQLEXPRESS
            string cadenaConexion = WebConfigurationManager.ConnectionStrings["SistemaERPConnectionString"].ConnectionString;
            //string cadenaConexion = @"Data Source=192.168.1.6;Initial Catalog=PROYECT_1;User ID=sa;Password=Proteccionsolar123";
            //Data Source=DESKTOP-NG22G5J\SQLEXPRESS;Initial Catalog=PROYECT_1;Integrated Security=True
            try
            {

                conexion.ConnectionString = cadenaConexion;
                conexion.Open();
                respuesta = true;


            }
            catch (Exception ex)
            {
                respuesta = false;
                MostrarError = "No se ha podido conectado con el servidor. Mensaje de la excepción: " + ex.Message.ToString();
            }
            return respuesta;
        }




        public bool Crear(string tabla, string campos, string valores)
        {
            bool respuesta = false;

            try
            {
                SqlCommand comando = new SqlCommand();
                comando.Connection = conexion;
                //INSERT INTO DEPARTAMENTO(nombre_depto) VALUES('Guatemala');
                comando.CommandText = "INSERT INTO " + tabla + "(" + campos + ") VALUES(" + valores + ");";
                if (ConectarServer())
                {
                    if (comando.ExecuteNonQuery() == 1)
                        respuesta = true;
                    else
                        respuesta = false;
                }
                else
                {
                    respuesta = false;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                MostrarError = "Mensaje de la excepcion: " + ex.Message.ToString();
            }
            finally
            {
                conexion.Close();
            }
            return respuesta;
        }

        public DataSet Consulta(string query)
        {
            DataSet respuesta = new DataSet();
            try
            {
                //SELECT cod_depto, nombre_depto FROM DEPARTAMENTO;
                SqlDataAdapter adaptador = new SqlDataAdapter(query, conexion);

                if (ConectarServer())
                {
                    adaptador.Fill(respuesta);
                }
            }
            catch (Exception ex)
            {
                MostrarError = "Mensaje de la exepción: " + ex.Message.ToString();
            }
            finally
            {
                conexion.Close();
            }

            return respuesta;
        }


        public DataSet Buscar_Mostrar(string tabla, string condicion)
        {
            DataSet respuesta = new DataSet();
            try
            {
                string instruccionSQL = "SELECT * FROM " + tabla + " WHERE " + condicion + ";";
                SqlDataAdapter adaptador = new SqlDataAdapter(instruccionSQL, conexion);

                if (ConectarServer())
                {
                    adaptador.Fill(respuesta, condicion);
                }
            }
            catch (Exception ex)
            {
                MostrarError = "Mensaje de la exepción: " + ex.Message.ToString();
            }
            finally
            {
                conexion.Close();
            }

            return respuesta;
        }


        public bool Modificar(string tabla, string campos, string condicion)
        {
            bool respuesta = false;

            try
            {
                SqlCommand comando = new SqlCommand();
                comando.Connection = conexion;
                //UPDATE DEPARTAMENTO SET nombre_depto = 'San Marcos' WHERE cod_depto = 2;
                comando.CommandText = "UPDATE " + tabla + " SET " + campos + " WHERE " + condicion + ";";
                if (ConectarServer())
                {
                    if (comando.ExecuteNonQuery() == 1)
                        respuesta = true;
                    else
                        respuesta = false;
                }
                else
                {
                    respuesta = false;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                MostrarError = "Mensaje de la excepcion: " + ex.Message.ToString();
            }
            finally
            {
                conexion.Close();
            }
            return respuesta;
        }

    }
}