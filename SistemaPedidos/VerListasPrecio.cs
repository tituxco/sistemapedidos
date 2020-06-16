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
using SistemaPedidos.Resources;
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Model;

namespace SistemaPedidos
{
    [Activity(Label = "Sistema Pedidos - Ver Listas Precio")]
    public class VerListasPrecio : Activity
    {
        ListView lstDatosListas;
        List<ListasPrecio> lstOrigenListas = new List<ListasPrecio>();
        ConsultasTablas dbUser;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (VariablesGlobales.Idvendedor == 0)
            {
                Intent i = new Intent(this.ApplicationContext, typeof(MainActivity));
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
                StartActivity(i);
                this.Finish();
            }
            SetContentView(Resource.Layout.ListViewListasPrecio);
            lstDatosListas = FindViewById<ListView>(Resource.Id.lstListasPrecioLista);
            var btnVolver = FindViewById<Button>(Resource.Id.btnListasPrecioVolver);
            dbUser = new ConsultasTablas();
            

            btnVolver.Click += delegate
            {
                StartActivity(typeof(PaginaPrincipal));
            };
            LoadData();           
        }
       
        public void LoadData()
        {
            lstOrigenListas = dbUser.VerListaPrecio();
            var adapter = new ListasPrecioViewAdapter(this, lstOrigenListas);
            lstDatosListas.Adapter = adapter;
        }
    }
}