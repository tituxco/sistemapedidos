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
            SetContentView(Resource.Layout.ListViewListasPrecio);
            lstDatosListas = FindViewById<ListView>(Resource.Id.lstListasPrecioLista);
            var btnVolver = FindViewById<Button>(Resource.Id.btnListasPrecioVolver);
            dbUser = new ConsultasTablas();
            base.OnCreate(bundle);

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