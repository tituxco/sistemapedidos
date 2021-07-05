using Android.App;
using Android.Widget;
using Android.OS;
using SistemaPedidos.Resources.DataHelper;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using SistemaPedidos.Resources.Model;
using System.Globalization;
using Plugin.Connectivity;
using SistemaPedidos.Resources.Interface;
using System;
using Refit;

namespace SistemaPedidos
{
    [Activity(Label = "Sistema de pedidos - Login de usuario", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        IVerConfiguraciones  interfazConfiguraciones;
        ConsultasTablas dbPrinc;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //string cultureName = "es-AR";
            //var locale = new Java.Util.Locale(cultureName);
            //Java.Util.Locale.Default = locale;

            //CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("es_AR");

            SetContentView(Resource.Layout.Login);
            dbPrinc = new ConsultasTablas();
            dbPrinc.CrearTablasGrales();
            var LoginUsuario = FindViewById<EditText>(Resource.Id.txtLoginUsuario);
            var LoginContraseña = FindViewById<EditText>(Resource.Id.txtLoginPassword);

            var LoginMensaje = FindViewById<TextView>(Resource.Id.txtLoginMensaje);

            var BotonLogin = FindViewById<Button>(Resource.Id.btnLoginIngresar);
            var BotonRegistro = FindViewById<Button>(Resource.Id.btnLoginRegistrar);

            BotonLogin.Click += delegate
            {
                try
                {

                    var databasepath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "kigest_sltosAriel.db");
                    var db = new SQLiteConnection(databasepath);
                    IEnumerable<ConsultasTablas> resultado = LoguearUsuario(db, LoginUsuario.Text, LoginContraseña.Text);
                    if (resultado.Count() > 0)
                    {
                        Toast.MakeText(this, "Login Correcto!", ToastLength.Long).Show();
                        List<Usuarios> dtosUsuario = new List<Usuarios>();
                        List<ConfiguracionesVarias> configuracionesVarias = new List<ConfiguracionesVarias>();
                        dtosUsuario = dbPrinc.DatosUsuario(LoginUsuario.Text, LoginContraseña.Text);
                        configuracionesVarias = dbPrinc.VerListaConfiguraciones();

                        if (configuracionesVarias.Count == 0 || configuracionesVarias.Count < 6)
                        {
                            ConfiguracionesVarias monedaPeso = new ConfiguracionesVarias
                            {
                                id = 1,
                                nombre = "PESO",
                                valor = "1"
                            };
                            ConfiguracionesVarias monedaDolar = new ConfiguracionesVarias
                            {
                                id = 2,
                                nombre = "DOLAR",
                                valor = "1"
                            };
                            ConfiguracionesVarias FormulaCalculo = new ConfiguracionesVarias
                            {
                                id = 3,
                                nombre = "FormulaCalculo",
                                valor = "1"
                            };
                            ConfiguracionesVarias ListaDefecto = new ConfiguracionesVarias
                            {
                                id = 4,
                                nombre = "ListaDefecto",
                                valor = "0"
                            };
                            ConfiguracionesVarias DireccWebService = new ConfiguracionesVarias
                            {
                                id = 5,
                                nombre = "DireccWebService",
                                valor = "http://00.00.00.00/"
                            };
                            ConfiguracionesVarias NombWebService = new ConfiguracionesVarias
                            {
                                id = 6,
                                nombre = "NombWebService",
                                valor = "nombreWebService"
                            };
                            
                            dbPrinc.InsertarNvaConfiguracion(monedaPeso);
                            dbPrinc.InsertarNvaConfiguracion(monedaDolar);
                            dbPrinc.InsertarNvaConfiguracion(FormulaCalculo);
                            dbPrinc.InsertarNvaConfiguracion(ListaDefecto);
                            dbPrinc.InsertarNvaConfiguracion(DireccWebService);
                            dbPrinc.InsertarNvaConfiguracion(NombWebService);

                            VariablesGlobales.CotizacionDolar = 1;
                            VariablesGlobales.MetodoCalculo = 1;
                            VariablesGlobales.ListaPrecioCliente = 0;
                            VariablesGlobales.DireccWebService = "http://00.00.00.00/";
                            VariablesGlobales.NombWebService = "nombreWebService";

                        }
                        else
                        {
                            VariablesGlobales.CotizacionDolar = double.Parse(configuracionesVarias[1].valor);
                            VariablesGlobales.MetodoCalculo = int.Parse(configuracionesVarias[2].valor);
                            VariablesGlobales.ListaPrecioCliente = int.Parse(configuracionesVarias[3].valor);
                            VariablesGlobales.DireccWebService = configuracionesVarias[4].valor;
                            VariablesGlobales.NombWebService = configuracionesVarias[5].valor;
                        }

                        VariablesGlobales.Idvendedor = dtosUsuario[0].vendedor;

                        if (CrossConnectivity.Current.IsConnected == true)
                        {
                            interfazConfiguraciones  = RestService.For<IVerConfiguraciones>(VariablesGlobales.DireccWebService + VariablesGlobales.NombWebService);
                            ObtenerConfiguraciones();                            

                            Toast.MakeText(this, "ESTA CONECTADO A INTERNET!", ToastLength.Long).Show();
                        }
                        else
                        {
                            Toast.MakeText(this, "NO ESTA CONECTADO A INTERNET!", ToastLength.Long).Show();
                        }
                        StartActivity(typeof(PaginaPrincipal));
                        Finish();
                    }
                    else
                    {
                        //LoginMensaje.Text = "Nombre de usuario o contraseña incorrectos, asegúrese que su usuario este activado!";
                        Toast.MakeText(this, "Nombre de usuario o contraseña incorrectos, asegúrese que su usuario este activado!", ToastLength.Long).Show();
                        
                    }                                        
                }
                catch (SQLiteException ex)
                {
                    LoginMensaje.Text = ex.Message;
                }
            };

            BotonRegistro.Click += delegate
            {
                StartActivity(typeof(Registro));
            };
        }

        private async void ObtenerConfiguraciones()
        {
            try
            {
                List<ConfiguracionesVariasServer > configuracionesVariasServer  = new List<ConfiguracionesVariasServer>();
                RespuestaServerConfiguraciones response = await interfazConfiguraciones.verConfiguraciones(VariablesGlobales.Idvendedor);
                configuracionesVariasServer  = response.ConfiguracionesServer;                

                VariablesGlobales.activo = int.Parse(configuracionesVariasServer[0].activo);
                VariablesGlobales.NombreVendedor = configuracionesVariasServer[0].vendedor;
                VariablesGlobales.version = configuracionesVariasServer[0].version;

                Toast.MakeText(this, "bienvenido " + VariablesGlobales.NombreVendedor  , ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message + "-" + ex.StackTrace, ToastLength.Long).Show();
            }
        }
        public static IEnumerable<ConsultasTablas> LoguearUsuario(SQLiteConnection db, string usuario, string contraseña)
        {
            {
                return db.Query<ConsultasTablas>("SELECT * FROM Usuarios where usuario=? and contraseña=?", usuario, contraseña);
            }
        }       
    }
}

