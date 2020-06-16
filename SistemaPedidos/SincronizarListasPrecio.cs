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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Interface;
using SistemaPedidos.Resources.Model;
using SQLite;

namespace SistemaPedidos
{
    [Activity(Label = "Sincronizar Listas de Precios")]
    public class SincronizarListasPrecio : Activity
    {
        IObtenerListaPrecio interfaz;
        List<ListasPrecioServer> ListasPrecio = new List<ListasPrecioServer>();
        List<string> ListasPrecioString = new List<string>();

        //IObtenerPromocionesDescuentos interfaz2;
        //List<PromocionesDescuentosServer> PromocionesDescuentos=new List<PromocionesDescuentosServer>();


        IListAdapter ListAdapter;
        ListView listaListasPrec;
        ConsultasTablas dbUser;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (VariablesGlobales.Idvendedor == 0)
            {
                Intent i = new Intent(this.ApplicationContext, typeof(MainActivity));
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
                StartActivity(i);
                this.Finish();
            }
            dbUser = new ConsultasTablas();
            base.OnCreate(savedInstanceState);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };

            interfaz = RestService.For<IObtenerListaPrecio>("http://66.97.35.86");
            //interfaz2 = RestService.For<IObtenerPromocionesDescuentos>("http://66.97.35.86");

            SetContentView(Resource.Layout.ListasPrecioServer);
            var btnSincronizar = FindViewById<Button>(Resource.Id.btnListasServerSincronizar);
            var btnVolver = FindViewById<Button>(Resource.Id.btnListasServerVer);
            var mensaje = FindViewById<TextView>(Resource.Id.txtListasServerMensaje);
            listaListasPrec = FindViewById<ListView>(Resource.Id.lstListasServerListas);
            btnSincronizar.Click += btnSincronizar_Click;
            btnVolver.Click += delegate
            {
                StartActivity(typeof(VerListasPrecio));
            };

        }
        private void btnSincronizar_Click(object sender, EventArgs e)
        {
            var btnSincronizar = FindViewById<Button>(Resource.Id.btnListasServerSincronizar);
            btnSincronizar.Text = "Sincronizando, por favor espere...";
            btnSincronizar.Enabled = false;
            ObtenerListasPrecio();
            //ObtenerPromocionesDescuentos();
        }
        private async void ObtenerListasPrecio()
        {
            try
            {
                RespuestaServerListasPrecio response = await interfaz.GetServerListasPrecio();
                ListasPrecio = response.ListasPrecio;
                var databasepath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "kigest_sltosAriel.db");
                var db = new SQLiteConnection(databasepath);
                var mensaje = FindViewById<TextView>(Resource.Id.txtListasServerMensaje);
                int contadoradd = 0;
                int contadormod = 0;
                foreach (ListasPrecioServer listasPrecio in ListasPrecio)
                {
                    IEnumerable<ConsultasTablas> resultado = BuscarListasPrecio(db, listasPrecio.id);
                    if (resultado.Count() == 0)
                    {
                        ListasPrecio ListasPrecioLocal = new ListasPrecio()
                        {
                            id = listasPrecio.id,
                            nombre = listasPrecio.nombre,
                            utilidad = listasPrecio.utilidad,
                            auxcol = listasPrecio.auxcol

                        };
                        contadoradd++;
                        dbUser.InsertarListaPrecio(ListasPrecioLocal);

                    }
                    else
                    {
                        ListasPrecio ListasPrecioLocal = new ListasPrecio()
                        {
                            id = listasPrecio.id,
                            nombre = listasPrecio.nombre,
                            utilidad = listasPrecio.utilidad,
                            auxcol = listasPrecio.auxcol
                        };
                        contadormod++;
                        dbUser.ActualizarListasPrecios(ListasPrecioLocal);
                    }
                    ListasPrecioString.Add(listasPrecio.ToString());
                    mensaje.Text = "Se han agregado " + contadoradd + " y se han modificado " + contadormod + " Listas obtenidos del servidor /n";
                    //var btnSincronizar = FindViewById<Button>(Resource.Id.btnListasServerSincronizar);
                    //btnSincronizar.Text = "Sincronizar listas";
                    //btnSincronizar.Enabled = true;
                }
                ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListasPrecioString);
                listaListasPrec.Adapter = ListAdapter;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message + "-" + ex.StackTrace, ToastLength.Long).Show();
            }
        }
        //    private async void ObtenerPromocionesDescuentos()
        //    {
        //        try
        //        {
        //            RespuestaServerPromocionesDescuentos response = await interfaz2.GetServerPromocionesDescuentos();
        //            PromocionesDescuentos = response.PromocionesDescuentos;
        //            var databasepath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "kigest_sltosAriel.db");
        //            var mensaje = FindViewById<TextView>(Resource.Id.txtListasServerMensaje);
        //            var db = new SQLiteConnection(databasepath);
        //            int contadoradd = 0;
        //            int contadormod = 0;
        //            foreach (PromocionesDescuentosServer promocionesDescuentos in PromocionesDescuentos)
        //            {
        //                IEnumerable<ConsultasTablas> resultado = BuscarPromocionesDescuentos (db, promocionesDescuentos.id);
        //                if (resultado.Count() == 0)
        //                {
        //                    PromocionesDescuentos  promocionesDescuentosLocal = new PromocionesDescuentos ()
        //                    {
        //                        id = promocionesDescuentos.id,
        //                        nombrepromo=promocionesDescuentos.nombrepromo,
        //                        idproducto=promocionesDescuentos.idproducto,
        //                        idcategoria=promocionesDescuentos.idcategoria,
        //                        compra_min=promocionesDescuentos.compra_min,
        //                    };
        //                    contadoradd++;
        //                    dbUser.InsertarPromocionesDescuentos(promocionesDescuentosLocal);

        //                }
        //                else
        //                {
        //                    PromocionesDescuentos promocionesDescuentosLocal = new PromocionesDescuentos()
        //                    {
        //                        id = promocionesDescuentos.id,
        //                        nombrepromo = promocionesDescuentos.nombrepromo,
        //                        idproducto=promocionesDescuentos.idproducto,
        //                        idcategoria=promocionesDescuentos.idcategoria,
        //                        compra_min=promocionesDescuentos.compra_min,
        //                        descuento_porc =promocionesDescuentos.descuento_porc
        //                    };
        //                    contadormod++;
        //                    dbUser.ActualizarPromocionesDescuentos(promocionesDescuentosLocal);
        //                }
        //                //ListasPrecioString.Add(listasPrecio.ToString());
        //                mensaje.Text = "Sincrinizacion exitosa";
        //                var btnSincronizar = FindViewById<Button>(Resource.Id.btnListasServerSincronizar);
        //                btnSincronizar.Text = "Sincronizar listas";
        //                btnSincronizar.Enabled = true;
        //            }
        //            //ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListasPrecioString);
        //            //listaListasPrec.Adapter = ListAdapter;
        //        }
        //        catch (Exception ex)
        //        {
        //            Toast.MakeText(this, ex.Message + "-" + ex.StackTrace, ToastLength.Long).Show();
        //        }
        //    }
        public static IEnumerable<ConsultasTablas> BuscarListasPrecio(SQLiteConnection db, int id)
        {
            {
                return db.Query<ConsultasTablas>("SELECT * FROM ListasPrecio where id=?", id);
            }
        }
        //    public static IEnumerable<ConsultasTablas> BuscarPromocionesDescuentos(SQLiteConnection db, int id)
        //    {
        //        {
        //            return db.Query<ConsultasTablas>("SELECT * FROM PromocionesDescuentos where id=?", id);
        //        }
        //    }
        //}
    }
}