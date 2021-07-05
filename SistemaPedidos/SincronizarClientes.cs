using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;
using Java.IO;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Json;
using SistemaPedidos.Resources.Model;
using SistemaPedidos.Resources;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using SistemaPedidos.Resources.Interface;
using Refit;
using SistemaPedidos.Resources.DataHelper;
using SQLite;

namespace SistemaPedidos
{

    [Activity(Label = "Sincronizar Clientes")]
    public class SincronizarClientes : Activity
    {
        IObtenerClientesServer interfazClientes;
        List<ClienteServer> Clientes = new List<ClienteServer>();
        List<string> ClientesString = new List<string>();
        IListAdapter ListAdapter;
        ListView listaClie;
        ConsultasTablas dbUser;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            dbUser = new ConsultasTablas();            
            base.OnCreate(savedInstanceState);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };

            interfazClientes = RestService.For<IObtenerClientesServer>(VariablesGlobales.DireccWebService + VariablesGlobales.NombWebService);            
            
            SetContentView(Resource.Layout.ListaClientesServer);
            
            var btnSincronizar = FindViewById<Button>(Resource.Id.btnClieServerSincro);
            var btnVolver = FindViewById<Button>(Resource.Id.btnClieServerVolver);
            var Mensaje = FindViewById<TextView>(Resource.Id.txtClieServerMensaje);

            listaClie = FindViewById<ListView>(Resource.Id.lstClieServLista);
            btnSincronizar.Click += btnSincronizar_Click;
            Mensaje.Text = "Vendedor: " + VariablesGlobales.Idvendedor;
            btnVolver.Click += delegate
            {
                StartActivity(typeof(VerClientes));
            };
        }
        private void btnSincronizar_Click(object sender, EventArgs e)
        {
            var btnSincronizar = FindViewById<Button>(Resource.Id.btnClieServerSincro);
            btnSincronizar.Text = "Sincronizando, por favor espere...";
            btnSincronizar.Enabled = false;
            ObtenerClientes();
        }
        private async void ObtenerClientes()
        {
            try
            {
                RespuestaServerClientes response = await interfazClientes.GetServerClientes();
                Clientes = response.ClientesLista;
                var databasepath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "kigest_sltosAriel.db");
                var db = new SQLiteConnection(databasepath);
                var Mensaje = FindViewById<TextView>(Resource.Id.txtClieServerMensaje);               
                int contadorADD = 0;
                int contadorMOD = 0;

                foreach (ClienteServer cliente in Clientes)
                {
                    IEnumerable<ConsultasTablas> resultado = BuscarCliente(db, cliente.idclientes);
                    if (resultado.Count() == 0 && cliente.vendedor==VariablesGlobales.Idvendedor)
                    {
                        TablaClientes ClieLocal = new TablaClientes()
                        {
                            codclieMain = cliente.idclientes,
                            nomapell_razon = cliente.nomapell_razon,
                            domicilio = cliente.direccion,
                            localidad = cliente.localidad,
                            iva_tipo = cliente.iva_tipo,
                            cuit = cliente.cuit,
                            telefono = cliente.telefono,
                            contacto = cliente.contacto,
                            celular = cliente.celular,
                            email = cliente.email,
                            observaciones = cliente.observaciones,
                            lista_precios = cliente.lista_precios,
                            vendedor = cliente.vendedor
                        };
                        contadorADD++;
                        dbUser.InsterarCliente(ClieLocal);
                        ClientesString.Add(" (!) " + cliente.ToString());
                    }
                    else if (resultado.Count() != 0)
                    {
                        TablaClientes ClieLocal = new TablaClientes()
                        {
                            codclieMain = cliente.idclientes,
                            nomapell_razon = cliente.nomapell_razon,
                            domicilio = cliente.direccion,
                            localidad = cliente.localidad,
                            iva_tipo = cliente.iva_tipo,
                            cuit = cliente.cuit,
                            telefono = cliente.telefono,
                            contacto = cliente.contacto,
                            celular = cliente.celular,
                            email = cliente.email,
                            observaciones = cliente.observaciones,
                            lista_precios = cliente.lista_precios,
                            vendedor = cliente.vendedor
                        };
                        contadorMOD++;
                        dbUser.ActualizarCliente(ClieLocal);
                        ClientesString.Add(" (*) " + cliente.ToString());
                    }
                    
                    
                    Mensaje.Text = "Se han agregado(!) " + contadorADD + " y se han modificado(*) " + contadorMOD + " de " + ClientesString.Count() + " clientes obtenidos del servidor";
                    var btnSincronizar = FindViewById<Button>(Resource.Id.btnClieServerSincro);
                    btnSincronizar.Text = "Sincronizar clientes";
                    btnSincronizar.Enabled = true ;
                }
                ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ClientesString);
                listaClie.Adapter = ListAdapter;
            }
            catch(Exception ex)
            {
                Toast.MakeText(this,ex.Message +"-"+ ex.StackTrace, ToastLength.Long).Show();
            }
        }
        public static IEnumerable<ConsultasTablas> BuscarCliente(SQLiteConnection db, int idclienteMain)
        {
            {
                return db.Query<ConsultasTablas>("SELECT * FROM TablaClientes where codclieMain=?", idclienteMain);
            }
        }
    }
}