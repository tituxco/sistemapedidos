using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using SistemaPedidos.Resources;
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Interface;
using SistemaPedidos.Resources.Model;

namespace SistemaPedidos
{
    [Activity(Label = "PedidoActual")]
    public class PedidoActual : Activity
    {
        ISubirPedidoMaster interfazPedidoMaster;
        ISubirPedidoDetalle interfazPedidoDetalle;

        ConsultasTablas dbUser;
        List<PedidosMaster> pedidosMaster = new List<PedidosMaster>();
        
        List<TablaClientes> datosCliente = new List<TablaClientes>();
        List<PedidosDetalle> calcularTotalPedido = new List<PedidosDetalle>();


        ListView lstDatosProductos;
        List<PedidosDetalle> lstOrigenProductos = new List<PedidosDetalle>();
        List<PedidoDetalleCant> cantProductos = new List<PedidoDetalleCant>();
        FuncionesGlobales funcionesGlobales = new FuncionesGlobales();

        int IdPoductoSel;
        TextView pedidoCant; 
        TextView pedidoTot;

        int IdPedido = VariablesGlobales.IdPedidoenCurso;
        int IdCliente = VariablesGlobales.IdCliente;
        int IdVendedor = VariablesGlobales.Idvendedor;
        int IdListaPrecio = VariablesGlobales.ListaPrecioCliente;

        double subtotal = 0;
        double iva = 0;
        double TotalFinal = 0;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PedidoActual);
            dbUser = new ConsultasTablas();
           

            lstDatosProductos = FindViewById<ListView>(Resource.Id.lstPedidoActProductos);

            var pedCliente = FindViewById<TextView>(Resource.Id.txtPedidoCliente);
            var btnAddProducto = FindViewById<Button>(Resource.Id.btnPedidoActAgregar);
            var btnFinalizarPedido = FindViewById<Button>(Resource.Id.btnPedidoActFinalizar);
            var btnEnviarPedido = FindViewById<Button>(Resource.Id.btnEnviarPedido);
            var btnVolver = FindViewById<Button>(Resource.Id.btnPedidoVolver);

            if (IdPedido != 0 && VariablesGlobales.PedidoEnCurso == false)
            {

                pedidosMaster = dbUser.VerPedidoMaster(IdPedido);
                IdCliente = pedidosMaster[0].id_cliente;

                datosCliente = dbUser.VerDetalleClienteMain(IdCliente);
                IdVendedor = datosCliente[0].vendedor;
                IdListaPrecio = datosCliente[0].lista_precios;

                
                VariablesGlobales.Idvendedor = IdVendedor;
                VariablesGlobales.ListaPrecioCliente = IdListaPrecio;
                VariablesGlobales.IdCliente = IdCliente;
            }
            else
            {
                pedidosMaster = dbUser.VerPedidoMaster(IdPedido);
                datosCliente = dbUser.VerDetalleClienteMain(pedidosMaster[0].id_cliente);
            }

            if (pedidosMaster[0].finalizado == 1 && pedidosMaster[0].enviado == 0)
            {
                VariablesGlobales.PedidoEnCurso = false;
                btnAddProducto.Enabled = false;
                btnFinalizarPedido.Enabled = false;
                btnEnviarPedido.Enabled = true;
            }
            else if (pedidosMaster[0].finalizado == 0 && pedidosMaster[0].enviado == 0)
            {
                VariablesGlobales.PedidoEnCurso = true;
                btnEnviarPedido.Enabled = false;
                btnAddProducto.Enabled = true;
                btnFinalizarPedido.Enabled = true;
            }
            else if (pedidosMaster[0].finalizado == 1 && pedidosMaster[0].enviado == 1)
            {
                VariablesGlobales.PedidoEnCurso = false;
                btnEnviarPedido.Enabled = false;
                btnAddProducto.Enabled = false;
                btnFinalizarPedido.Enabled = false;
            }

            LoadDataPedido(IdPedido);
            LoadDataProductos(IdPedido);
            lstDatosProductos.ItemClick += (s, e) =>
            {
                LayoutInflater inputModificar = LayoutInflater.From(Application.Context);
                View viewModificar = inputModificar.Inflate(Resource.Layout.inputBoxCantProd, null);

                AlertDialog.Builder constrModifica = new AlertDialog.Builder(this);
                EditText cantProd = viewModificar.FindViewById<EditText>(Resource.Id.txtInputCantProd);
                EditText PrecioProd = viewModificar.FindViewById<EditText>(Resource.Id.txtInputPrecioProd);

                TextView mensajeInput = viewModificar.FindViewById<TextView>(Resource.Id.txtInputCantMensaje);
                constrModifica.SetView(viewModificar);
                
                string cantidad = "";
                string precioUnit = "";
                double precioTotal = 0;
                
                AlertDialog alertModificar = constrModifica.Create();
                alertModificar.SetCanceledOnTouchOutside(true);
                alertModificar.SetTitle("Modificar producto");

                
                alertModificar.SetButton("Modificar cantidad", (ss, ee) =>
                {

                    List<PedidosDetalle> consProdPedido = new List<PedidosDetalle>();
                    consProdPedido = dbUser.VerPedidoDetalleID(IdPoductoSel);
                    //              
                    if (cantProd.Text != "" & cantProd.Text != "0")
                    {
                        cantidad = cantProd.Text;
                    }
                    else
                    {
                        cantidad = consProdPedido[0].cantidad;
                    }

                    if (PrecioProd.Text !="" & PrecioProd.Text != "0")
                    {
                        precioUnit = PrecioProd.Text;
                    }
                    else
                    {
                        precioUnit = consProdPedido[0].punit;
                    }

                    precioTotal = double.Parse(precioUnit) * double.Parse(cantidad) ;
                                               
                    PedidosDetalle productoDetalle = new PedidosDetalle()
                    {                           
                        id =IdPoductoSel ,                            
                        cantidad = cantidad,
                        punit = precioUnit,
                        ptotal = precioTotal.ToString() 
                    };

                    dbUser.ModificarProductoPedido(productoDetalle);
                    Toast.MakeText(this, "Producto modificado!", ToastLength.Short).Show();
                    LoadDataPedido(IdPedido);
                    LoadDataProductos(IdPedido);                                      
                });                
                alertModificar.SetButton2("Eliminar producto", (sss, eee) =>
                {
                    PedidosDetalle productoDetalle = new PedidosDetalle()
                    {
                        id = int.Parse(e.Id.ToString()),
                    };
                    if (dbUser.EliminarProductoPedido(productoDetalle)){
                        Toast.MakeText(this, "Producto Eliminado!", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "No se pudo eliminar el producto!", ToastLength.Short).Show();
                    }
                    LoadDataPedido(IdPedido);
                    LoadDataProductos(IdPedido);                    
                });
                IdPoductoSel = int.Parse(e.Id.ToString());
                
                List<PedidosDetalle> consProdPedido = new List<PedidosDetalle>();
                consProdPedido = dbUser.VerPedidoDetalleID (IdPoductoSel);

                PrecioProd.Hint = "Precio: $" + consProdPedido[0].punit;
                precioUnit = consProdPedido[0].punit;
                
                cantProd.Hint = consProdPedido[0].cantidad;
                cantidad = consProdPedido[0].cantidad;

                alertModificar.Show();
            };
          
            Toast.MakeText(this, "Pedido cliente:" + VariablesGlobales.IdCliente+" | Pedido N°:" + VariablesGlobales.IdPedidoenCurso + " | Lista de precios:" +
                + VariablesGlobales.ListaPrecioCliente, ToastLength.Long).Show();

            pedCliente.Text = datosCliente[0].nomapell_razon;

            btnAddProducto.Click += delegate
            {
                StartActivity(typeof(VerProductos));
            };
            btnFinalizarPedido.Click += delegate
            {
                LayoutInflater layoutInflater = LayoutInflater.From(Application.Context);
                View dialogo = layoutInflater.Inflate(Resource.Layout.inputBoxFinalizarPedido, null);

                AlertDialog.Builder constrFinaliza = new AlertDialog.Builder(this);
                TextView txtsubtotal = dialogo.FindViewById<TextView>(Resource.Id.txtFinalizarPedSubtotal);
                TextView txtiva = dialogo.FindViewById<TextView>(Resource.Id.txtFinalizarPedIVA);
                TextView txtptotal = dialogo.FindViewById<TextView>(Resource.Id.txtFinalizarPedPtotal);
                EditText txtObservaciones = dialogo.FindViewById<EditText>(Resource.Id.txtFinalizarPedObservaciones);

                SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy-MM-dd");
                string fecha = simpleDateFormat.Format(new Date());

                constrFinaliza.SetView(dialogo);
                AlertDialog alertFinalizar = constrFinaliza.Create();
                alertFinalizar.SetCanceledOnTouchOutside(true);
                alertFinalizar.SetTitle("Finalizar pedido");
                alertFinalizar.SetButton("Finalizar", (ss, ee) =>
                {
                    PedidosMaster pedidosMaster = new PedidosMaster()
                    {
                        id = IdPedido,
                        id_cliente = IdCliente,
                        fecha = fecha,
                        finalizado = 1,
                        subtotal = subtotal.ToString(),
                        iva105 = "0",
                        iva21 = iva.ToString(),
                        total = TotalFinal.ToString(),
                        vendedor = IdVendedor.ToString(),
                        observaciones = txtObservaciones.Text
                    };
                    if (dbUser.ActualizaPedido(pedidosMaster))
                    {
                        Toast.MakeText(this, "Pedido finalizado!", ToastLength.Short).Show();
                        StartActivity(typeof(VerPedidos));
                        VariablesGlobales.IdCliente = 0;
                        VariablesGlobales.IdPedidoenCurso = 0;
                        VariablesGlobales.ListaPrecioCliente = 0;
                    }
                });
                alertFinalizar.SetButton2("Cancelar", (ss, ee) =>
                {
                    Toast.MakeText(this, "Finalizacion cancelada!", ToastLength.Short).Show();
                });

                txtsubtotal.Text = "Subtotal: " + subtotal.ToString();// Math.Round(subtotal,2).ToString();
                txtiva.Text = "IVA: " + iva.ToString(); //Math.Round(iva,2).ToString();
                txtptotal.Text = "Total pedido: " + TotalFinal.ToString();// Math.Round(TotalFinal,2).ToString();
                alertFinalizar.Show();          
            };
            btnVolver.Click += delegate
            {
                StartActivity(typeof(VerPedidos));
            };
            
            btnEnviarPedido.Click += delegate
            {
                SubirPedido();
            };
        }
        private async void SubirPedido()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };
            interfazPedidoMaster = RestService.For<ISubirPedidoMaster>("http://66.97.35.86");
            interfazPedidoDetalle=RestService.For<ISubirPedidoDetalle>("http://66.97.35.86");

            List<PedidosMaster> masterPed = new List<PedidosMaster>();
            List<PedidosDetalle> detallePed = new List<PedidosDetalle>();

            masterPed = dbUser.VerPedidoMaster(IdPedido);
            detallePed = dbUser.verDetallePedido(IdPedido);

            PedidosMasterServer pedidosMasterServer = new PedidosMasterServer
            {
                fecha = masterPed[0].fecha,
                id_cliente = masterPed[0].id_cliente,
                iva105 = "0",
                iva21 = masterPed[0].iva21,
                subtotal = masterPed[0].subtotal,
                total = masterPed[0].total,
                vendedor = masterPed[0].vendedor,
                observaciones = masterPed[0].observaciones,
                observaciones2 = IdVendedor + "-" + masterPed[0].id
            };
            await interfazPedidoMaster.SubirPedidoMaster(pedidosMasterServer);

            for(int i = 0; i<= detallePed.Count() - 1; i++)
            {
                PedidosDetalleServer pedidosDetalleServer = new PedidosDetalleServer
                {
                    cantidad = detallePed[i].cantidad,
                    codProdMain=detallePed[i].codProdMain,
                    descripcion=detallePed[i].descripcion,
                    id_master=IdVendedor + "-" + detallePed[i].id_master,
                    iva=detallePed[i].iva,
                    plu=detallePed[i].plu,
                    ptotal=detallePed[i].ptotal,
                    punit=detallePed[i].punit
                };
                await interfazPedidoDetalle.SubirPedidoDetalle(pedidosDetalleServer);                
            }

            PedidosMaster pedidosMaster = new PedidosMaster
            {
                id=IdPedido,
                enviado=1,
                finalizado=1,
                fecha=masterPed[0].fecha,
                id_cliente=masterPed[0].id_cliente,
                iva105="0",
                iva21=masterPed[0].iva21,
                observaciones=masterPed[0].observaciones,
                subtotal=masterPed[0].subtotal,
                total=masterPed[0].total,
                vendedor=masterPed[0].vendedor                
            };
            dbUser.ActualizaPedido(pedidosMaster);
            Toast.MakeText(this, "Pedido enviado correctamente!", ToastLength.Short).Show();
            StartActivity(typeof(VerPedidos));
        }

        public void LoadDataProductos(int idPedido)
        {
            lstOrigenProductos = dbUser.verDetallePedido(idPedido);
            var adapter = new PedidoDetalleViewAdapter(this, lstOrigenProductos);
            lstDatosProductos.Adapter = adapter;
        }
        public void LoadDataPedido(int idPedido)
        {
            pedidoCant = FindViewById<TextView>(Resource.Id.txtPedidoCantProd);
            pedidoTot = FindViewById<TextView>(Resource.Id.txtPedidoTotal);

            cantProductos = dbUser.ObtenerCantProdPedido(IdPedido);
            pedidoCant.Text = "Productos: " + cantProductos[0].cantidad;
            
            double Ptotal = 0;
            calcularTotalPedido = dbUser.verDetallePedido(IdPedido);
            for (int i = 0; i <= calcularTotalPedido.Count() - 1; i++)
            {
                Ptotal = Ptotal + double.Parse(calcularTotalPedido[i].ptotal);
            }
            pedidoTot.Text = "Total de pedido: $" + Ptotal.ToString();

            subtotal = Math.Round(Ptotal / (1.21), 2);
            iva = Ptotal - Math.Round(subtotal,2);
            TotalFinal = Math.Round(Ptotal,2);
        }
    }
}