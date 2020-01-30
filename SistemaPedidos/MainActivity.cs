using Android.App;
using Android.Widget;
using Android.OS;
using SistemaPedidos.Resources.DataHelper;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using SistemaPedidos.Resources.Model;
using System.Globalization;

namespace SistemaPedidos
{
    [Activity(Label = "Sistema de pedidos - Login de usuario", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {       
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
                        dtosUsuario = dbPrinc.DatosUsuario(LoginUsuario.Text, LoginContraseña.Text);                        
                        VariablesGlobales.Idvendedor = dtosUsuario[0].vendedor;                        
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
        public static IEnumerable<ConsultasTablas> LoguearUsuario(SQLiteConnection db, string usuario, string contraseña)
        {
            {
                return db.Query<ConsultasTablas>("SELECT * FROM Usuarios where usuario=? and contraseña=?", usuario, contraseña);
            }
        }       
    }
}

