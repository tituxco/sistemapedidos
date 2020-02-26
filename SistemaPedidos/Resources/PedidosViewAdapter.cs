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
    public class PedidosViewAdapter : BaseAdapter
    {
        private Activity activity;
        private List<PedidosMaster> lstPedidos;
        private List<TablaClientes> lstClientes;
        public PedidosViewAdapter(Activity activity, List<PedidosMaster> lstPedidos)
        {
            this.activity = activity;
            this.lstPedidos = lstPedidos;
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override int Count
        {
            get
            {
                return lstPedidos.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return lstPedidos[position].id;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ConsultasTablas dbUser = new ConsultasTablas();
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListViewPedidos_template, parent, false);
            var Descripcion = view.FindViewById<TextView>(Resource.Id.txtPedidosItem);

            lstClientes = new List<TablaClientes>();
            lstClientes = dbUser.VerDetalleClienteMain(lstPedidos[position].id_cliente);
            Descripcion.Text = VariablesGlobales.Idvendedor +"-"+ lstPedidos[position].id + " - (" + lstPedidos[position].fecha  + ") " +  lstClientes[0].nomapell_razon;
            Descripcion.Tag = lstPedidos[position].id;          
            return view;
        }
    }

    class PedidosViewAdapterViewHolder : Java.Lang.Object
    {
       // public TextView itempedidos { get; set; }
        public TextView Descripcion { get; set; }

    }
}