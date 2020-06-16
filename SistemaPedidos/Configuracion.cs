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
using SQLite;

namespace SistemaPedidos
{
    [Activity(Label = "Configuracion")]
    public class Configuracion : Activity
    {
        ConsultasTablas db = new ConsultasTablas();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (VariablesGlobales.Idvendedor == 0)
            {
                Intent i = new Intent(this.ApplicationContext, typeof(MainActivity));
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
                StartActivity(i);
                this.Finish();
            }
            SetContentView(Resource.Layout.PaginaConfiguracion);

            var btnVaciarProductos = FindViewById<Button>(Resource.Id.btnVaciarProductos);
            var btnVaciarCategoriasProductos = FindViewById<Button>(Resource.Id.btnVaciarCategorias);
            var btnVaciarListasPrecio = FindViewById<Button>(Resource.Id.btnVaciarListasPrecio);
            var btnVaciarClientes = FindViewById<Button>(Resource.Id.btnVaciarClientes);
            var btnVaciarPedidos = FindViewById<Button>(Resource.Id.btnVaciarPedidos);
            var btnVaciarPedidosDetalle = FindViewById<Button>(Resource.Id.btnVariarDetallePed);
            var btnGuardarCotizacionMoneda= FindViewById<Button>(Resource.Id.btnCotizacionDolar);
            var btnVolver = FindViewById<Button>(Resource.Id.btnConfVolverInicio);
            var txtCotizacion= FindViewById<EditText>(Resource.Id.txtCotizacionDolar);

            txtCotizacion.Text = VariablesGlobales.CotizacionDolar.ToString();

            btnVaciarCategoriasProductos.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Specify Action");
                alert.SetMessage("Esta seguro que desea vaciar la tabla categoria?");
                alert.SetPositiveButton("Aceptar", (senderAlert, args) =>
                {
                    if (db.VaciarTablaCategoriasProd())
                    {
                        Toast.MakeText(this, "Se vacio la tabla de categorias", ToastLength.Long).Show();
                    }
                });
                alert.SetNegativeButton("Cancelar", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Proceso cancelado", ToastLength.Long).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };

            btnVaciarProductos.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Specify Action");
                alert.SetMessage("Esta seguro que desea vaciar la tabla de productos?");
                alert.SetPositiveButton("Aceptar", (senderAlert, args) =>
                {
                    if (db.VaciarTablaProductos())
                {
                    Toast.MakeText(this, "Se vacio la tabla de PRODUCTOS", ToastLength.Long).Show();
                }
                });
                    alert.SetNegativeButton("Cancelar", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Proceso cancelado", ToastLength.Long).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };

            btnVaciarListasPrecio.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Specify Action");
                alert.SetMessage("Esta seguro que desea vaciar la tabla de productos?");
                alert.SetPositiveButton("Aceptar", (senderAlert, args) =>
                {
                    if (db.VaciarTablaListasPrecio())
                    {
                        Toast.MakeText(this, "Se vacio la tabla de LISTAS DE PRECIO", ToastLength.Long).Show();
                    }
                });
                    alert.SetNegativeButton("Cancelar", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Proceso cancelado", ToastLength.Long).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };

            btnVaciarClientes.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Specify Action");
                alert.SetMessage("Esta seguro que desea vaciar la tabla de CLIENTES?");
                alert.SetPositiveButton("Aceptar", (senderAlert, args) =>
                {
                    if (db.VaciarTablaClientes())
                {
                    Toast.MakeText(this, "Se vacio la tabla de Clientes", ToastLength.Long).Show();
                }
                });
                alert.SetNegativeButton("Cancelar", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Proceso cancelado", ToastLength.Long).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };

            btnVaciarPedidos.Click += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Specify Action");
                alert.SetMessage("Esta seguro que desea vaciar la tabla de PEDIDOS?");
                alert.SetPositiveButton("Aceptar", (senderAlert, args) =>
                {
                    if (db.VaciarTablaPedidosMaster())
                     {
                     Toast.MakeText(this, "Se vacio la tabla de Pedidos", ToastLength.Long).Show();
                    
                    }
                    if (db.VaciarTablaDetallePedidos())
                    {
                        Toast.MakeText(this, "Se vacio la tabla de Detalle de pedidos", ToastLength.Long).Show();
                    }
                });
                alert.SetNegativeButton("Cancelar", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Proceso cancelado", ToastLength.Long).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            };

            btnGuardarCotizacionMoneda.Click += delegate
            {
                double tempCotiza = 0;
                if(txtCotizacion.Text !="" & double.TryParse(txtCotizacion.Text, out tempCotiza))
                {
                    CotizacionMoneda moneda = new CotizacionMoneda()
                    {
                        id = 2,
                        cotizacion = txtCotizacion.Text
                    };
                    db.ActualizarMoneda(moneda);
                    VariablesGlobales.CotizacionDolar = double.Parse(txtCotizacion.Text);
                    Toast.MakeText(this,"Se actualizo la cotizacion de moneda", ToastLength.Long).Show();
                }
                
            };
            btnVolver.Click += delegate
            {
                StartActivity(typeof(PaginaPrincipal)); 
            };
        }      
    }
}