using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Model;
using System;
using System.Collections.Generic;

namespace SistemaPedidos
{
    [Activity(Label = "Configuracion")]
    public class Configuracion : Activity
    {
        ConsultasTablas db = new ConsultasTablas();
        Spinner SeleccListaPrecios;
        List<ListasPrecio> ListasPrecio;
        ArrayAdapter adapter;
        List<ConfiguracionesVarias> MonedaYConfig = new List<ConfiguracionesVarias>();
        List<TablaClientes> ClientesLista;
        List<ListasPrecio> ListaVendedor;

        Boolean listaOK = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
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
                //var btnVaciarCategoriasProductos = FindViewById<Button>(Resource.Id.btnVaciarCategorias);
                var btnVaciarListasPrecio = FindViewById<Button>(Resource.Id.btnVaciarListasPrecio);
                var btnVaciarClientes = FindViewById<Button>(Resource.Id.btnVaciarClientes);
                var btnVaciarPedidos = FindViewById<Button>(Resource.Id.btnVaciarPedidos);
                //var btnVaciarPedidosDetalle = FindViewById<Button>(Resource.Id.btnVariarDetallePed);
                var btnGuardarCotizacionMoneda = FindViewById<Button>(Resource.Id.btnCotizacionDolar);
                var btnVolver = FindViewById<Button>(Resource.Id.btnConfVolverInicio);
                var txtCotizacion = FindViewById<EditText>(Resource.Id.txtCotizacionDolar);
                var txtMetodoCalculo = FindViewById<EditText>(Resource.Id.txtMetodoCalculo);
                var txtDireccionWebService = FindViewById<EditText>(Resource.Id.txtDireccionWebService);
                var txtNomWebService = FindViewById<EditText>(Resource.Id.txtNombWebService);
                SeleccListaPrecios = (Spinner)FindViewById(Resource.Id.spnListaPrecios);
                //dbUser = new ConsultasTablas();

                ListasPrecio = db.VerListaPrecio();
                ListaVendedor = db.VerListaPreciosVendedor(VariablesGlobales.Idvendedor);
                adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, ListasPrecio);
                SeleccListaPrecios.Adapter = adapter;

                int posicion = 0;
                for (int i = 0; i < SeleccListaPrecios.Count; i++)
                {
                    if (SeleccListaPrecios.GetItemAtPosition(i).ToString() == ListaVendedor[0].nombre)
                    {

                        posicion = i;
                    }
                }
                //Toast.MakeText(this, "posicion " + posicion, ToastLength.Long).Show();
                SeleccListaPrecios.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ListasPrecios_ItemSelected);
                MonedaYConfig = db.VerListaConfiguraciones();
                
                txtCotizacion.Text = MonedaYConfig[1].valor;
                txtMetodoCalculo.Text = MonedaYConfig[2].valor;
                txtDireccionWebService.Text = MonedaYConfig[4].valor;
                txtNomWebService.Text = MonedaYConfig[5].valor;

                SeleccListaPrecios.SetSelection(posicion);
                if (int.Parse(MonedaYConfig[3].valor) != 0)
                {
                    ListasPrecio = new List<ListasPrecio>();
                    ListasPrecio = db.VerListaPrecioId(int.Parse(MonedaYConfig[3].valor));
                    int itemSel = adapter.GetPosition(ListasPrecio[0].nombre);                
                }
                
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
                    if (listaOK == true)
                    {
                        string ItemLIstaPrecios = SeleccListaPrecios.SelectedItem.ToString();
                        ListasPrecio = new List<ListasPrecio>();
                        ListasPrecio = db.ObtenerIDListaPrecios(ItemLIstaPrecios);
                    }
                    double tempCotiza = 0;
                    if (txtCotizacion.Text != "" & double.TryParse(txtCotizacion.Text, out tempCotiza))
                        
                    {
                        string  valLista = "0";
                        if (listaOK == true)
                        {
                            valLista = ListasPrecio[0].id.ToString();
                        }else
                        {
                            valLista="0";
                        }

                        ConfiguracionesVarias moneda = new ConfiguracionesVarias()
                        {
                            id = 2,
                            valor = txtCotizacion.Text
                        };
                        ConfiguracionesVarias FormulaCalculo = new ConfiguracionesVarias
                        {
                            id = 3,
                            valor = txtMetodoCalculo.Text
                        };

                        ConfiguracionesVarias listaDefecto = new ConfiguracionesVarias
                        {
                            id = 4,
                            valor = valLista 
                        };
                        db.ActualizarConfiguracion(moneda);
                        db.ActualizarConfiguracion(FormulaCalculo);
                        db.ActualizarConfiguracion(listaDefecto);

                        VariablesGlobales.CotizacionDolar = double.Parse(txtCotizacion.Text);
                        VariablesGlobales.MetodoCalculo = int.Parse(txtMetodoCalculo.Text);
                        VariablesGlobales.ListaPrecioCliente = int.Parse(valLista); // ListasPrecio[0].id;

                        Toast.MakeText(this, "Se actualizo configuraciones varias", ToastLength.Long).Show();
                    }
                    else 
                    {
                        Toast.MakeText(this, "NO SE PUEDEN GUARDAR LAS CONFIGURACIONES VARIAS", ToastLength.Long).Show();
                    }
                    if (txtDireccionWebService.Text  != "" & txtDireccionWebService.Text.Contains("http://")==true & txtNomWebService.Text !="" & txtNomWebService.Text.Contains("/")==false ) 
                    { 
                        ConfiguracionesVarias DireccWebService = new ConfiguracionesVarias
                        {
                            id = 5,
                            nombre = "DireccWebService",
                            valor = txtDireccionWebService.Text 
                        };
                        ConfiguracionesVarias NombWebService = new ConfiguracionesVarias
                        {
                            id = 6,
                            nombre = "NombWebService",
                            valor = txtNomWebService.Text 
                        };
                       
                        db.ActualizarConfiguracion(DireccWebService );
                        db.ActualizarConfiguracion(NombWebService );

                        VariablesGlobales.DireccWebService = txtDireccionWebService.Text ;
                        VariablesGlobales.NombWebService = txtNomWebService.Text ;

                        Toast.MakeText(this, "Se actualizo la configuracion de web service", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "NO SE PUEDEN GUARDAR LAS CONFIGURACIONES DE WEB SERVICE", ToastLength.Long).Show();
                    }
                };
                btnVolver.Click += delegate
                {
                    StartActivity(typeof(PaginaPrincipal));
                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();

            }

        }
        private void ListasPrecios_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                Spinner ListaPreciosSelecc = (Spinner)sender;

                string CategoriaBusq = ListaPreciosSelecc.GetItemAtPosition(e.Position).ToString();
                ClientesLista = new List<TablaClientes>();
                ClientesLista = db.BuscarClienteporLista(CategoriaBusq);
                if (ClientesLista.Count == 0)
                {
                    listaOK = false;
                    Toast.MakeText(this, "Esta lista no pertenece a sus clientes, no puede ser utilizada", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "lista aplicada", ToastLength.Long).Show();
                    listaOK = true;
                }
                 //   Toast.MakeText(this, "Se cargo la categoria: " + CategSelecc.GetItemAtPosition(e.Position), ToastLength.Long).Show();
            }catch(Exception ex)
            {
                listaOK = false;
                Toast.MakeText(this, ex.Message , ToastLength.Long).Show();
            }
        }

      

    }

}