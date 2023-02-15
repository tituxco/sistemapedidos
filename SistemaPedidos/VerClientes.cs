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
using SistemaPedidos.Resources.Model;
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources;
using Android.Content.Res;
using Xamarin;


namespace SistemaPedidos
{
    [Activity(Label = "Sistema de pedidos - Listado de clientes")]
    public class VerClientes : Activity        
    {
        
        ListView lstDatosClientes;
        List<TablaClientes> lstOrigenClientes = new List<TablaClientes>();
        ConsultasTablas dbUser;
        SearchView BusquedaCliente;
        //Android.Widget.SearchView busquedaCliente;
        protected override void OnCreate(Bundle bundle)
        {
            if (VariablesGlobales.Idvendedor == 0)
            {
                Intent i = new Intent(this.ApplicationContext, typeof(MainActivity));
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
                StartActivity(i);
                this.Finish();
            }
            SetContentView(Resource.Layout.ListViewCliente);
            lstDatosClientes = FindViewById<ListView>(Resource.Id.lstClienteLista);
            dbUser = new ConsultasTablas();
            BusquedaCliente = (SearchView)FindViewById(Resource.Id.srchClienteBuscar);
            var btnVolver = FindViewById<Button>(Resource.Id.btnClienteListaVolver);
            var btnAgregar = FindViewById<Button>(Resource.Id.btnClienteAgregar);

            base.OnCreate(bundle);
            LoadData();

            BusquedaCliente.QueryTextSubmit += BusquedaCliente_QuerySubmit;
            BusquedaCliente.QueryTextChange += BusquedaCliente_TextChange;
            btnAgregar.Click += delegate
            {
                Intent abrirWeb = new Intent(Intent.ActionView);
                abrirWeb.SetData(Android.Net.Uri.Parse(VariablesGlobales.DireccWebService + VariablesGlobales.NombWebService+"/pedidos_test"));

                StartActivity(abrirWeb);
            };

            btnVolver.Click += delegate
            {
                StartActivity(typeof(PaginaPrincipal));
            };
            lstDatosClientes.ItemClick +=(s, e)=>
            {
                DetalleCliente.IdCliente=int.Parse(e.Id.ToString());
                StartActivity(typeof(DetalleCliente));
            };            
        }
       
        void BusquedaCliente_QuerySubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            if (e.Query == "")
            {
                LoadData();
            }
            else
            { 
            LoadDataBusqueda(e.Query);
            }
        }
        void BusquedaCliente_TextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (e.NewText == "")
            {
                LoadData();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuClientes, menu); 
            
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.btnMnuCliVolver:
                    StartActivity(typeof(PaginaPrincipal));
                    return true;
                case Resource.Id.btnMnuCliSincronizar:                                                                                 
                    StartActivity(typeof(SincronizarClientes));
                    return true;                             
            }
            return base.OnOptionsItemSelected(item);
        }

        public void LoadData()
        {
            lstOrigenClientes = dbUser.VerListaCLientes();
            var adapter = new ClienteViewAdapter(this, lstOrigenClientes);
            lstDatosClientes.Adapter = adapter;
        }
        public void LoadDataBusqueda(string busqueda)
        {
            lstOrigenClientes = dbUser.BuscarCliente(busqueda);
            var adapter = new ClienteViewAdapter(this, lstOrigenClientes);
            lstDatosClientes.Adapter = adapter;
        }
    }
}