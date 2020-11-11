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
        Spinner SeleccListaPrecios;
        List<ListasPrecio> ListasPrecio;
        ArrayAdapter adapter;
        List<CotizacionMoneda> MonedaYConfig = new List<CotizacionMoneda>();
        List<TablaClientes> ClientesLista;
        Boolean listaOK = false;
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
            var btnGuardarCotizacionMoneda = FindViewById<Button>(Resource.Id.btnCotizacionDolar);
            var btnVolver = FindViewById<Button>(Resource.Id.btnConfVolverInicio);
            var txtCotizacion = FindViewById<EditText>(Resource.Id.txtCotizacionDolar);
            var txtMetodoCalculo = FindViewById<EditText>(Resource.Id.txtMetodoCalculo);

            SeleccListaPrecios = (Spinner)FindViewById(Resource.Id.spnListaPrecios);
            //dbUser = new ConsultasTablas();

            ListasPrecio = db.VerListaPrecio();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, ListasPrecio);
            SeleccListaPrecios.Adapter = adapter;
            SeleccListaPrecios.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ListasPrecios_ItemSelected);
            MonedaYConfig = db.VerListaMonedas();
            txtCotizacion.Text = MonedaYConfig[1].cotizacion;
            txtMetodoCalculo.Text = MonedaYConfig[2].cotizacion;

            if (int.Parse(MonedaYConfig[3].cotizacion) != 0) {
                ListasPrecio = new List<ListasPrecio>();
                ListasPrecio = db.VerListaPrecioId(int.Parse(MonedaYConfig[3].cotizacion));
                int itemSel = adapter.GetPosition(ListasPrecio[0].nombre);
                SeleccListaPrecios.SetSelection(itemSel);
            }
            //Toast.MakeText(this,"cantidad: " + adapter.Count+  " Categ defecto: " + ListasPrecio[0].nombre + " pos: " + itemSel, ToastLength.Long).Show();

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
                string ItemLIstaPrecios = SeleccListaPrecios.SelectedItem.ToString();
                ListasPrecio = new List<ListasPrecio>();
                ListasPrecio = db.ObtenerIDListaPrecios(ItemLIstaPrecios);

                double tempCotiza = 0;
                if (txtCotizacion.Text != "" & double.TryParse(txtCotizacion.Text, out tempCotiza) && listaOK==true)
                {

                    CotizacionMoneda moneda = new CotizacionMoneda()
                    {
                        id = 2,
                        cotizacion = txtCotizacion.Text
                    };
                    CotizacionMoneda FormulaCalculo = new CotizacionMoneda
                    {
                        id = 3,
                        cotizacion = txtMetodoCalculo.Text
                    };
               
                    CotizacionMoneda listaDefecto = new CotizacionMoneda
                    {
                        id = 4,
                        cotizacion = ListasPrecio[0].id.ToString()
                    };
                
                    db.ActualizarMoneda(moneda);
                    db.ActualizarMoneda(FormulaCalculo);
                    db.ActualizarMoneda(listaDefecto);

                    VariablesGlobales.CotizacionDolar = double.Parse(txtCotizacion.Text);
                    VariablesGlobales.MetodoCalculo = int.Parse(txtMetodoCalculo.Text);
                    VariablesGlobales.ListaPrecioCliente = ListasPrecio[0].id;
                    
                    Toast.MakeText(this,"Se actualizo la configuracion " , ToastLength.Long).Show();
                }else
                {
                    Toast.MakeText(this, "No se puede guardar la configuracion debido a algun error en los datos seleccionados", ToastLength.Long).Show();
                }
                
            };
            btnVolver.Click += delegate
            {
                StartActivity(typeof(PaginaPrincipal)); 
            };
        }
        private void ListasPrecios_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner ListaPreciosSelecc = (Spinner)sender;

            string CategoriaBusq = ListaPreciosSelecc.GetItemAtPosition(e.Position).ToString();
            ClientesLista= new List<TablaClientes>();
            ClientesLista = db.BuscarClienteporLista(CategoriaBusq);
            if (ClientesLista.Count == 0)
            {
                listaOK = false;
                Toast.MakeText(this, "Esta lista no pertenece a sus clientes, no puede ser utilizada", ToastLength.Long).Show();
            }
            else
            {
                listaOK = true;
            }
            //    Toast.MakeText(this, "Se cargo la categoria: " + CategSelecc.GetItemAtPosition(e.Position), ToastLength.Long).Show();
        }

      

    }

}