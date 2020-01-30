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

namespace SistemaPedidos
{
    [Activity(Label = "Sistema de pedidos - Bienvenido")]
    public class PaginaPrincipal : Activity
    {
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuPrincipal, menu);
            return base.OnCreateOptionsMenu(menu);
            
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //acciones de los botones de menu
                case Resource.Id.btnMnuPrinVerClientes:
                    StartActivity(typeof(VerClientes));
                    return true;
                case Resource.Id.btnMnuPrinVerPedidos:
                    StartActivity(typeof(VerPedidos));
                    return true;
                case Resource.Id.btnMnuPrinVerProductos:
                    StartActivity(typeof(VerProductos));
                    return true;
                case Resource.Id.btnMnuPrinVerListas:
                    StartActivity(typeof(VerListasPrecio));
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PaginaPrincipal);

            var sincroClientes = FindViewById<Button>(Resource.Id.btnPrincSincroClie);
            var sincroProductos = FindViewById<Button>(Resource.Id.btnPrincSincProd);
            var sincroPedidos = FindViewById<Button>(Resource.Id.btnPrincSincroPed);
            var sincroListas = FindViewById<Button>(Resource.Id.btnPrincSincListasPrecio);
            var manejoTablas = FindViewById<Button>(Resource.Id.btnPrincManejoTablas);
            sincroClientes.Click += delegate
            {
                StartActivity(typeof(SincronizarClientes));
            };
            sincroProductos.Click += delegate
            {
                StartActivity(typeof(SincronizarProductos));
            };
            sincroListas.Click += delegate
            {
                StartActivity(typeof(SincronizarListasPrecio));
            };
            manejoTablas.Click += delegate
            {
                StartActivity(typeof(Configuracion));
            };

            var verClientes = FindViewById<Button>(Resource.Id.btnPrincVerClientes);
            var verProductos = FindViewById<Button>(Resource.Id.btnPrincVerProductos);
            var verPedidos = FindViewById<Button>(Resource.Id.btnPrincVerPedidos);
            
            verClientes.Click += delegate
            {
                StartActivity(typeof(VerClientes));
            };
            verProductos.Click += delegate 
            {
                StartActivity(typeof(VerProductos));
            };
            verPedidos.Click += delegate
            {
                StartActivity(typeof(VerPedidos));
            };
        }
    }
}