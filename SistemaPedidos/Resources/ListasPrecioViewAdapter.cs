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
    public class ViewListasPrecio : Java.Lang.Object
    {
        public TextView Nombre { get; set; }
        public TextView Utilidad { get; set; }
       
    }
    public class ListasPrecioViewAdapter : BaseAdapter
    {
        private Activity activity;
        private List<ListasPrecio> lstListas;

        public ListasPrecioViewAdapter(Activity activity, List<ListasPrecio> lstListas)
        {
            this.activity = activity;
            this.lstListas= lstListas;
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override int Count
        {
            get
            {
                return lstListas.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return lstListas[position].id;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //ConsultasTablas dbUser;
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListViewListasPrecio_template, parent, false);
            var nombre = view.FindViewById<TextView>(Resource.Id.txtListasPrecioNombre);
            var utilidad = view.FindViewById<TextView>(Resource.Id.txtListasPrecioUtilidad);
            

            nombre.Text = lstListas[position].nombre;
            //utilidad.Text = "" + lstListas[position].utilidad;
            
            //dbUser = new ConsultasTablas();
            return view;
        }
    }
}