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

namespace SistemaPedidos
{
    [Activity(Label = "Configuracion")]
    public class Configuracion : Activity
    {
        ConsultasTablas db = new ConsultasTablas();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PaginaConfiguracion);

            var btnVaciarProductos = FindViewById<Button>(Resource.Id.btnVaciarProductos);
            var btnVaciarCategoriasProductos = FindViewById<Button>(Resource.Id.btnVaciarCategorias);
            var btnVaciarListasPrecio = FindViewById<Button>(Resource.Id.btnVaciarListasPrecio);
            var btnVaciarClientes = FindViewById<Button>(Resource.Id.btnVaciarClientes);
            var btnVaciarPedidos = FindViewById<Button>(Resource.Id.btnVaciarPedidos);
            var btnVaciarPedidosDetalle = FindViewById<Button>(Resource.Id.btnVariarDetallePed);

            btnVaciarCategoriasProductos.Click += delegate
            {
                if (db.VaciarTablaCategoriasProd())
                {
                    Toast.MakeText(this, "Se vacio la tabla de categorias", ToastLength.Long).Show();
                }
            };

            btnVaciarProductos.Click += delegate
            {
                if (db.VaciarTablaProductos())
                {
                    Toast.MakeText(this, "Se vacio la tabla de Productos", ToastLength.Long).Show();
                }
            };

            btnVaciarListasPrecio.Click += delegate
            {
                if (db.VaciarTablaListasPrecio())
                {
                    Toast.MakeText(this, "Se vacio la tabla de Listas de precio", ToastLength.Long).Show();
                }
            };

            btnVaciarClientes.Click += delegate
            {
                if (db.VaciarTablaClientes())
                {
                    Toast.MakeText(this, "Se vacio la tabla de Clientes", ToastLength.Long).Show();
                }
            };

            btnVaciarPedidos.Click += delegate
            {
                if (db.VaciarTablaPedidosMaster())
                {
                    Toast.MakeText(this, "Se vacio la tabla de Pedidos", ToastLength.Long).Show();
                }
            };

            btnVaciarPedidosDetalle.Click += delegate
            {
                if (db.VaciarTablaDetallePedidos())
                {
                    Toast.MakeText(this, "Se vacio la tabla de Detalle de pedidos", ToastLength.Long).Show();
                }
            };

        }
    }
}