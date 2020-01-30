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
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Model;

namespace SistemaPedidos
{
    [Activity(Label = "DetalleCliente")]
   
    public class DetalleCliente : Activity
    {
        public static int IdCliente;
        ConsultasTablas dbUser;        
        List<TablaClientes> dtosCliente = new List<TablaClientes>();
        List<PedidoMax> nuevoPedido = new List<PedidoMax>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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

            txtnomapell_razon.Text = dtosCliente[0].codclieMain+" - "+ dtosCliente[0].nomapell_razon;
            txtdireccion.Text = "DIRECCION: "+dtosCliente[0].domicilio;
            txttelefono_celular.Text = "TELEFONO/CELULAR:"+ dtosCliente[0].telefono + "/" + dtosCliente[0].celular;
            txtcontacto.Text = "PERSONA DE CONTACTO:"+dtosCliente[0].contacto;
            txtobservaciones.Text = "OBSERVACIONES:" + dtosCliente[0].observaciones;
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

        }
    }
}