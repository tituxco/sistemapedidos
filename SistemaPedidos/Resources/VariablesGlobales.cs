using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace SistemaPedidos
{
    public class VariablesGlobales
    {
        public static int Idvendedor=0;
        public static int IdCliente=0;
        public static bool PedidoEnCurso=false;
        public static int ListaPrecioCliente=0;
        public static int IdPedidoenCurso=0;
        public static double CotizacionDolar = 0;
        public static double MetodoCalculo = 1;
        public static string DireccWebService = "";
        public static string NombWebService = "";
        public static string NombreVendedor = "";
        public static string NombreEmpresa = "";
        public static int activo = 0;
        public static string version = "";



    }
}