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
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Model;

namespace SistemaPedidos.Resources
{
    public class ViewProductos : Java.Lang.Object
    {
        public TextView Descripcion { get; set; }
        public TextView Detalles { get; set; }
        public TextView Codigo { get; set; }
        public TextView Presentacion { get; set; }
        public TextView Precio { get; set; }
        public TextView Mensaje { get; set; }
    }
    public class ProductoViewAdapter : BaseAdapter
        {
        private Activity activity;
        private List<Productos> lstProductos;
        
        public ProductoViewAdapter(Activity activity, List<Productos> lstProductos)
        {
            this.activity = activity;
            this.lstProductos = lstProductos;
        }
        public override Java.Lang.Object GetItem(int position)
            {
                return null;
            }
        public override int Count
            {
                get
                {
                    return lstProductos.Count;
                }
            }
        public override long GetItemId(int position)
            {
                return lstProductos[position].id;
            }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            FuncionesGlobales funcionesGlobales = new FuncionesGlobales();
            ConsultasTablas dbUser=new ConsultasTablas();
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListViewProductos_template, parent, false);
            var Descripcion = view.FindViewById<TextView>(Resource.Id.txtProductoDescripcion);
         
            var Codigo = view.FindViewById<TextView>(Resource.Id.txtProductoCodigo);
            var Presentacion = view.FindViewById<TextView>(Resource.Id.txtProductoPresentacion);
            var Precio = view.FindViewById<TextView>(Resource.Id.txtProductoPrecio);
            var Promos= view.FindViewById<TextView>(Resource.Id.txtProductoPromos);
            Descripcion.Text = lstProductos[position].descripcion;

            Codigo.Text = "CODIGO: " + lstProductos[position].codigo;
            Descripcion.Tag = lstProductos[position].id;
            Presentacion.Text = "PRESENTACION: X" + lstProductos[position].presentacion;
            Precio.Text = funcionesGlobales.CalcularPrecioLista(lstProductos[position].precio, 
                lstProductos[position].ganancia,lstProductos[position].utilidad1, lstProductos[position].utilidad2,
                lstProductos[position].utilidad3, lstProductos[position].utilidad4, lstProductos[position].utilidad5,
                lstProductos[position].iva, VariablesGlobales.ListaPrecioCliente,lstProductos[position].calcular_precio );
            Promos.Text = funcionesGlobales.CalcularPromosDescuentos(lstProductos[position].codProdMain);
            dbUser = new ConsultasTablas();
            return view;
        }
     }
  
}