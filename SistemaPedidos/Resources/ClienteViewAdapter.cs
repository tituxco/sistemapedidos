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
    public class ViewClientes : Java.Lang.Object
    {
        public TextView ClieNombreRazon { get; set; }
        public TextView ClieDirDomicilio { get; set; }
        public TextView ClieTelefono { get; set; }

    }
    public class ClienteViewAdapter : BaseAdapter
    {

        private Activity activity;
        private List<TablaClientes> lstClientes;

        public ClienteViewAdapter(Activity activity, List<TablaClientes> lstClientes)
        {
            this.activity = activity;
            this.lstClientes = lstClientes;
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
        public override int Count
        {
            get
            {
                return lstClientes.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return lstClientes[position].idclientes;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //ConsultasTablas dbUser;
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListViewCliente_template, parent, false);
            var ClieNombreRazon = view.FindViewById<TextView>(Resource.Id.txtClieNombreRazon);
            var ClieDirDomicilio = view.FindViewById<TextView>(Resource.Id.txtClieDomicilioLocalidad);
            var ClieTelefono = view.FindViewById<TextView>(Resource.Id.txtClieTelefono);

            ClieNombreRazon.Text = lstClientes[position].nomapell_razon;
            ClieDirDomicilio.Text = "DOMCILIO: " + lstClientes[position].domicilio + " - " + lstClientes[position].localidad;
            ClieTelefono.Text = "TELEFONO/CELULAR: " + lstClientes[position].telefono+"/"+lstClientes[position].celular;
            ClieNombreRazon.Tag = lstClientes[position].idclientes;
            //dbUser = new ConsultasTablas();
            
            return view;
        }
    }
}