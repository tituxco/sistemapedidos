using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text.Format;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Interface;
using SistemaPedidos.Resources.Model;
//using SistemaPedidos.Resources;

namespace SistemaPedidos
{
    [Activity(Label = "DetalleCliente")]
   
    public class DetalleCliente : Activity
    {
        public static int IdCliente;
        ConsultasTablas dbUser;        
        List<TablaClientes> dtosCliente = new List<TablaClientes>();
        List<PedidoMax> nuevoPedido = new List<PedidoMax>();
        int IdClienteSel = IdCliente;
        IModificarClienteServer interfazModificarCliente;
        IEliminarClienteServer interfazEliminarCliente;
        //string PrecioProdSel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (VariablesGlobales.Idvendedor == 0) {
                Intent i = new Intent(this.ApplicationContext, typeof(MainActivity));
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
                StartActivity(i);
                this.Finish();
            }

            SetContentView(Resource.Layout.DetalleCliente);

            dbUser = new ConsultasTablas();
            dtosCliente = dbUser.VerDetalleCliente(IdCliente);
            
            var txtnomapell_razon = FindViewById<TextView>(Resource.Id.txtDetalleClieNomRazon);
            var txtdireccion = FindViewById<TextView>(Resource.Id.txtDetalleClieDireccion);
            var txttelefono_celular = FindViewById<TextView>(Resource.Id.txtDetalleClieTelefonoCelular);
            var txtcontacto = FindViewById<TextView>(Resource.Id.txtDetalleClieContacto);
            var txtobservaciones = FindViewById<TextView>(Resource.Id.txtDetalleClieObservaciones);           
            var btnnvopedido = FindViewById<Button>(Resource.Id.btnDetalleClieNvoPedido);
            var btnvolver = FindViewById<Button>(Resource.Id.btnDetalleClieVolver);
            var btnModificar = FindViewById<Button>(Resource.Id.btnDetalleClieModificar);

            txtnomapell_razon.Text = dtosCliente[0].codclieMain+" - "+ dtosCliente[0].nomapell_razon;
            txtdireccion.Text = "DIRECCION: " + dtosCliente[0].domicilio;
            txttelefono_celular.Text = "TELEFONO/CELULAR:"+ dtosCliente[0].telefono + "/" + dtosCliente[0].celular;
            txtcontacto.Text = "PERSONA DE CONTACTO:"+dtosCliente[0].contacto;
            txtobservaciones.Text = "OBSERVACIONES:" + dtosCliente[0].observaciones + "\n" + "E-Mail: " + dtosCliente[0].email;
            SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy-MM-dd");
            string fecha = simpleDateFormat.Format(new Date());

            btnnvopedido.Click += delegate
            {
                    PedidosMaster pedidosMaster = new PedidosMaster()
                    {
                        id_cliente = dtosCliente[0].codclieMain,
                        fecha = fecha,
                        vendedor = VariablesGlobales.Idvendedor.ToString(),
                        finalizado = 0,
                        enviado = 0
                    };
                    dbUser.InsertarPedido(pedidosMaster);
                    nuevoPedido = dbUser.ObtenerUltimoIdPedido();
                    int idpedido = int.Parse(nuevoPedido[0].id.ToString());
                    VariablesGlobales.IdPedidoenCurso = int.Parse(idpedido.ToString());
                    VariablesGlobales.IdCliente = dtosCliente[0].codclieMain;
                    VariablesGlobales.ListaPrecioCliente = dtosCliente[0].lista_precios;
                    VariablesGlobales.PedidoEnCurso = true;
                    StartActivity(typeof(PedidoActual));            
            };
            btnvolver.Click += delegate
            {
                StartActivity(typeof(VerClientes));
            };
            btnModificar.Click += (s,e)=>
            {
                LayoutInflater layoutInflater = LayoutInflater.From(Application.Context);

                View dialogo = layoutInflater.Inflate(Resource.Layout.inputBoxModificaClie, null);

                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                EditText NomApellRazon = dialogo.FindViewById<EditText>(Resource.Id.txtModClieNomaApelRazon);
                EditText Domicilio = dialogo.FindViewById<EditText>(Resource.Id.txtModClieDomicilio);
                EditText Celular = dialogo.FindViewById<EditText>(Resource.Id.txtModClieCelular);
                EditText Email = dialogo.FindViewById<EditText>(Resource.Id.txtModClieEMail);
                EditText Observaciones = dialogo.FindViewById<EditText>(Resource.Id.txtModClieObservaciones);

                builder.SetView(dialogo);
                AlertDialog alertDialog = builder.Create();

                alertDialog.SetCanceledOnTouchOutside(true);
                alertDialog.SetTitle("Operaciones sobre Cliente");
                alertDialog.SetButton("Modificar", (ss, ee) =>

                {                    
                    List<TablaClientes> clienteDatos= new List<TablaClientes>();
                    clienteDatos   = dbUser.VerDetalleCliente(IdClienteSel);

                    string ClieNombreApellido = NomApellRazon.Text;
                    string ClieDomicilio = Domicilio.Text;
                    string ClieCelular = Celular.Text;
                    string ClieEmail = Email.Text;
                    string ClieObservaciones = Observaciones.Text;

                    if (ClieNombreApellido=="") { ClieNombreApellido = clienteDatos[0].nomapell_razon;  }
                    if (ClieDomicilio  == "") { ClieDomicilio  = clienteDatos[0].domicilio ; }
                    if (ClieCelular  == "") { ClieCelular  = clienteDatos[0].celular ; }
                    if (ClieEmail  == "") { ClieEmail  = clienteDatos[0].email ; }
                    if (ClieObservaciones  == "") { ClieObservaciones  = clienteDatos[0].observaciones; }

                    TablaClientes clienteModifica = new TablaClientes ()
                    {
                        idclientes=IdClienteSel,
                        nomapell_razon=ClieNombreApellido,
                        domicilio=ClieDomicilio,
                        celular=ClieCelular,
                        email=ClieEmail,
                        observaciones=ClieObservaciones
                    };

                    txtnomapell_razon.Text = ClieNombreApellido;
                    txtdireccion.Text = "DIRECCION: " + ClieDomicilio ;
                    txttelefono_celular.Text = "TELEFONO/CELULAR: " + ClieCelular ;
                    //txtcontacto.Text = "PERSONA DE CONTACTO:" +;
                    txtobservaciones.Text = "OBSERVACIONES: " + ClieObservaciones + "\n" +"E-Mail: " + ClieEmail;
                    dbUser.ActualizarClienteLocal(clienteModifica);
                    ModificarClienteServer();
                    Toast.MakeText(this, "Cliente modificado y actualizado en el servidor remoto!", ToastLength.Short).Show();
                    
                });
                alertDialog.SetButton2("Cancelar",(sss, eee) =>
                {

                });
                alertDialog.SetButton3("Eliminar", (sssss, eeeee) =>
                {
                    EliminarClienteServer();
                    dbUser.EliminarCliente(IdClienteSel);
                    Toast.MakeText(this, "Cliente eliminado del la base local y remota", ToastLength.Short).Show();
                    StartActivity(typeof(VerClientes));
                });
                
                List<TablaClientes> clienteDetalle = new List<TablaClientes >();
                clienteDetalle  = dbUser.VerDetalleCliente (IdClienteSel);
                
                NomApellRazon.Hint = "Nombre y Apellido / Razon: \n" + clienteDetalle[0].nomapell_razon;
                Domicilio.Hint = "Domicilio: \n" +clienteDetalle[0].domicilio;
                Celular.Hint = "Celular: \n" + clienteDetalle[0].celular;
                Email.Hint = "E-Mail: \n" + clienteDetalle[0].email;
                Observaciones.Hint = "Observaciones: \n" + clienteDetalle[0].observaciones;

                alertDialog.Show();
            };
        }
        private async void ModificarClienteServer()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };
            interfazModificarCliente = RestService.For<IModificarClienteServer>(VariablesGlobales.DireccWebService + VariablesGlobales.NombWebService);           
            List<TablaClientes> datosCliente = new List<TablaClientes>();            
            datosCliente = dbUser.VerDetalleCliente(IdClienteSel);            
            ClienteServer clienteServer = new ClienteServer 
            {
                idclientes=datosCliente[0].codclieMain,
                nomapell_razon= datosCliente[0].nomapell_razon,
                direccion= datosCliente[0].domicilio,
                celular= datosCliente[0].celular,
                email= datosCliente[0].email,
                observaciones= datosCliente[0].observaciones
            };
            await interfazModificarCliente.ModificarClienteServer(clienteServer);
            //Toast.MakeText(this, interfazModificarCliente.ModificarClienteServer, ToastLength.Short).Show();
        }
        private async void EliminarClienteServer()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };
            interfazEliminarCliente = RestService.For<IEliminarClienteServer>(VariablesGlobales.DireccWebService + VariablesGlobales.NombWebService);
            List<TablaClientes> datosCliente = new List<TablaClientes>();
            datosCliente = dbUser.VerDetalleCliente(IdClienteSel);
            ClienteServer clienteServer = new ClienteServer
            {
                idclientes = datosCliente[0].codclieMain,};
            await interfazEliminarCliente.EliminarClienteServer(clienteServer);
        }
    }
}