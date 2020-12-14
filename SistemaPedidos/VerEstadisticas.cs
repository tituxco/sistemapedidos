using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Refit;
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Interface;
using SistemaPedidos.Resources.Model;
using SQLite;

namespace SistemaPedidos
{
    [Activity(Label = "Estadisticas de ventas")]
    public class verEstadisticas : Activity
    {
        IVerEstadisticasVentas interfazEstadisticasVentas;
        IVerDevolucionesTotales interfazDevolucionesTotales;
        List<EstadisticasVenta> estadisticasVentas=new List<EstadisticasVenta>();
        List<EstadisticasDevolucion> estadisticasDevoluciones = new List<EstadisticasDevolucion>();
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (VariablesGlobales.Idvendedor == 0)
            {
                Intent i = new Intent(this.ApplicationContext, typeof(MainActivity));
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop); 
                StartActivity(i);
                this.Finish();
            }
            SetContentView(Resource.Layout.Estadisticas);
        
            var txtTotalVentas = FindViewById<TextView>(Resource.Id.txtEstadisticasTotalVentas);

            interfazEstadisticasVentas = RestService.For<IVerEstadisticasVentas>("http://66.97.35.86");
            interfazDevolucionesTotales = RestService.For<IVerDevolucionesTotales>("http://66.97.35.86");

            var btnObtenerEstadisticas = FindViewById<Button>(Resource.Id.btnEstadisticasObtener);
            btnObtenerEstadisticas.Click += delegate
            {              
                ObtenerEstadisticasVentas();
            };

           
        }
        private async void ObtenerEstadisticasVentas()
        {
            var txtTotalVentas=FindViewById<TextView>(Resource.Id.txtEstadisticasTotalVentas);
            var txtTotalDevoluciones= FindViewById<TextView>(Resource.Id.txtEstadisticasTotalDevoluciones);

            var textDesde= FindViewById<EditText>(Resource.Id.estadisticasFechaDesde);
            var textHasta= FindViewById<EditText>(Resource.Id.estadisticasFechaHasta);

            double totVentas = 0;
            double totDevoluciones = 0;
            double comisionVendedor = 0;
            double ventaReal = 0;
            double montoComision = 0;
            double totProdObjetivos = 0;


            DateTime desde = (Convert.ToDateTime(textDesde.Text,CultureInfo.CreateSpecificCulture("es-AR")));
            DateTime hasta= (Convert.ToDateTime(textHasta.Text, CultureInfo.CreateSpecificCulture("es-AR")));


            RespuestaEstadisticasVenta respuestaVentas = await interfazEstadisticasVentas.verEstadisticasVentas(VariablesGlobales.Idvendedor,desde.ToString("yyyyMMdd"), hasta.ToString("yyyyMMdd"));
            estadisticasVentas = respuestaVentas.estadisticasVenta;         
            RespuestaEstadisticasDevolucion respuestaDevoluciones = await interfazDevolucionesTotales.verDevolucionesTotales(VariablesGlobales.Idvendedor, desde.ToString("yyyyMMdd"), hasta.ToString("yyyyMMdd"));
            estadisticasDevoluciones = respuestaDevoluciones.estadisticasDevolucion;


            
           



            if (respuestaVentas.estado == "2") {
                txtTotalVentas.Text = respuestaVentas.estado + "(error::::"+VariablesGlobales.Idvendedor +"---" + desde.ToString("yyyyMMdd") + "---"+ hasta.ToString("yyyyMMdd") + " )";
            }
            else
            {
                txtTotalVentas.Text =  estadisticasVentas[0].totVentas;
            }

            if (respuestaDevoluciones.estado == "2")
            {
                txtTotalDevoluciones.Text = respuestaDevoluciones.estado + "(error::::" + VariablesGlobales.Idvendedor + "---" + desde.ToString("yyyyMMdd") + "---" + hasta.ToString("yyyyMMdd") + " )";
            }
            else
            {
                txtTotalDevoluciones.Text= estadisticasDevoluciones[0].totDevolucion;
            }


            //Toast.MakeText(this, estadisticasVentas.Count + "- total:" + estadisticasVentas[0].totVentas, ToastLength.Long).Show();
            //if (estadisticasVentas.Count > 0)
            //{
            //    txtTotalVentas.Text = estadisticasVentas[0].totVentas;
            //}
            //else
            //{
            //    txtTotalVentas.Text = "no hay registros";
            //}

        }

    }    
}