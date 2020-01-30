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
using SistemaPedidos.Resources.Model;
using SistemaPedidos.Resources.DataHelper;
using SQLite;
using SistemaPedidos.Resources.Interface;
using Refit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace SistemaPedidos
{
    [Activity(Label = "Sistema de pedidos - Registro de vendedor")]
    public class Registro : Activity
    {
        IObtenerEstadoVendedor interfazVendedor;

        ConsultasTablas dbPrinc;
        protected override void OnCreate(Bundle bundle)
        {            
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Registro);
            dbPrinc = new ConsultasTablas();
            //dbPrinc.CrearTablasGrales();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };
            var RegistroNombre = FindViewById<EditText>(Resource.Id.txtRegistroNombre);
            var RegistroApellido = FindViewById<EditText>(Resource.Id.txtRegistroApellido);
            var RegistroUsuario = FindViewById<EditText>(Resource.Id.txtRegistroUsuario);
            var RegistroContraseña1 = FindViewById<EditText>(Resource.Id.txtRegistroContraseña1);
            var RegistroContraseña2 = FindViewById<EditText>(Resource.Id.txtRegistroContraseña2);
            var RegistroActivo = FindViewById<EditText>(Resource.Id.txtRegistroActivo);
            var RegistroVendedor = FindViewById<EditText>(Resource.Id.txtRegistroVendedor);

            var RegistroMensaje = FindViewById<TextView>(Resource.Id.txtRegistroMensaje);

            var BtnAceptar = FindViewById<Button>(Resource.Id.btnRegistroAceptar);
            var BtnCancelar = FindViewById<Button>(Resource.Id.btnRegistroCancelar);



            BtnAceptar.Click += delegate
            {
                if (RegistroContraseña1.Text == RegistroContraseña2.Text)
                {
                    try
                    {
                        Usuarios nvoUsuario = new Usuarios()
                        {
                            nombre = RegistroNombre.Text,
                            apellido = RegistroApellido.Text,
                            usuario = RegistroUsuario.Text,
                            contraseña = RegistroContraseña1.Text,
                            //activo = int.Parse(RegistroActivo.Text),
                            vendedor = int.Parse(RegistroVendedor.Text)
                        };

                        


                        dbPrinc.InstertarNuevoUsuario(nvoUsuario);
                        Toast.MakeText(this, "Se ingreso nuevo usuario!", ToastLength.Long).Show();
                        StartActivity(typeof(MainActivity));
                    }
                    catch (SQLiteException ex)
                    {
                        RegistroMensaje.Text = ex.Message;
                    }
                }
                else
                {
                    Toast.MakeText(this, "Error de login, coroobore sus datos y que su usario este activo!", ToastLength.Long).Show();
                }
            };


            BtnCancelar.Click += delegate
            {
                StartActivity(typeof(MainActivity));
            };


        }

        private async void ObtenerEstadoVendedor()
        {
            try
            {                
                //interfazVendedor = RestService.For<IObtenerEstadoVendedor>("http://66.97.35.86");
                //RespuestaServerUsuarios  re= await interfazVendedor.GetServerUsuarios();
                //nada mas

            }
            catch (Exception ex)
            {

            }
        }

    }
}