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
    public class ViewPedidoDetalle : Java.Lang.Object
    {
        public TextView Descripcion { get; set; }
        public TextView Codigo { get; set; }
        public TextView CantPrecio { get; set; }

    }
    public class PedidoDetalleViewAdapter : BaseAdapter
    {
        private Activity activity;
        private List<PedidosDetalle> lstProductos;

        public PedidoDetalleViewAdapter(Activity activity, List<PedidosDetalle> lstProductos)
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
            ConsultasTablas dbUser;
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.PedidoActualDetalle_template, parent, false);
            var Descripcion = view.FindViewById<TextView>(Resource.Id.txtPedidoDetalleProducto);
            var Codigo = view.FindViewById<TextView>(Resource.Id.txtPedidoDetalleCodigo);
            var cantPrecio = view.FindViewById<TextView>(Resource.Id.txtPedidoCantPrecio);
                      
            Descripcion.Text = lstProductos[position].descripcion;
            Codigo.Text = "CODIGO: " + lstProductos[position].cod; // lstProductos[position].codProdMain+") "+ lstProductos[position].cod ;
            Descripcion.Tag = lstProductos[position].id;
            cantPrecio.Text = "Cant: " + lstProductos[position].cantidad + " | Punit: $" + lstProductos[position].punit + " | Ptotal: $"+ lstProductos[position].ptotal;
            dbUser = new ConsultasTablas();
            return view;
        }
    }
}