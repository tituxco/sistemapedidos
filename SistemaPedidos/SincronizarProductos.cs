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
    [Activity(Label = "Sincronizar Productos")]
    public class SincronizarProductos : Activity
    {
        IObtenerProductosServer interfazProductos;
        IObtenerCategoriaProductos interfazCatprod;
        
        List<ProductosServer> Productos = new List<ProductosServer>();
        
        List<CategoriaProductosServer> categoriaProductos = new List<CategoriaProductosServer>();
        
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

            interfazProductos = RestService.For<IObtenerProductosServer>("http://66.97.35.86");
            interfazCatprod = RestService.For<IObtenerCategoriaProductos>("http://66.97.35.86");
            
            SetContentView(Resource.Layout.ListaProductosServer);

            var btnSincronizar = FindViewById<Button>(Resource.Id.btnProductosServerSincronizar);
            var btnVolver = FindViewById<Button>(Resource.Id.btnProductosServerVer);
            var mensaje = FindViewById<TextView>(Resource.Id.txtProductosServerMensaje);

            //progreso = FindViewById<ProgressBar>(Resource.Id.pgbProductosProgreso);
           // progreso.Min = 0;

            //listaProd = FindViewById<ListView>(Resource.Id.lstProductosServer);
            btnSincronizar.Click += btnSincronizar_Click;            
            btnVolver.Click += delegate
            {
                StartActivity(typeof(VerProductos));
            };

        }
        private void btnSincronizar_Click(object sender, EventArgs e)
        {
            var btnSincronizar = FindViewById<Button>(Resource.Id.btnProductosServerSincronizar);
            btnSincronizar.Text = "Sincronizando, por favor espere...";
            btnSincronizar.Enabled = false;
            
            ObtenerCatProd();
            ObtenerProductos();

        }


        private async void ObtenerProductos()
        {
            try
            {
                var databasepath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "kigest_sltosAriel.db");
                var db = new SQLiteConnection(databasepath);
                RespuestaServerProductos respuestaServerProductos = await interfazProductos.GetServerProductos();                
                Productos = respuestaServerProductos.ProductosLista;

                var mensaje = FindViewById<TextView>(Resource.Id.txtProductosServerMensaje);
                int contadoradd = 0;
                int contadormod = 0;
                int contadortot = 0;
                int CantProd = Productos.Count;


                foreach (ProductosServer producto in Productos)
                {
                    IEnumerable<ConsultasTablas> resultado = BuscarProductos(db, producto.id);
                    if (resultado.Count() == 0)
                    {

                        Productos ProductoLocal = new Productos()
                        {

                            codProdMain = producto.id,
                            descripcion = producto.descripcion,
                            precio = producto.precio,
                            ganancia = producto.ganancia,
                            iva = producto.iva,
                            cod_bar = producto.cod_bar,
                            utilidad1 = producto.utilidad1,
                            utilidad2 = producto.utilidad2,
                            codigo = producto.codigo,
                            calcular_precio = producto.calcular_precio,
                            categoria = producto.categoria,
                            presentacion = producto.presentacion
                        };
                        contadoradd++;
                        contadortot++;
                        dbUser.InsertarProducto(ProductoLocal);

                    }
                    else
                    {
                        Productos ProductoLocal = new Productos()
                        {
                            codProdMain = producto.id,
                            precio = producto.precio,
                            ganancia = producto.ganancia,
                            iva = producto.iva,
                            //bonif = producto.bonif,
                            utilidad1 = producto.utilidad1,
                            utilidad2 = producto.utilidad2,
                            calcular_precio=producto.calcular_precio
                        };
                        contadormod++;
                        contadortot++;
                        dbUser.ActualizarProducto(ProductoLocal);
                    }
                }
                //progreso.SetProgress((contadortot / CantProd) * 100, true);
                mensaje.Text = "Se han agregado " + contadoradd + " y se han modificado " + contadormod + " productos obtenidos del servidor";
                var btnSincronizar = FindViewById<Button>(Resource.Id.btnProductosServerSincronizar);
                btnSincronizar.Text = "Sincronizar productos";
                btnSincronizar.Enabled = true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message + "---"+ ex.StackTrace, ToastLength.Long).Show();
                var btnSincronizar = FindViewById<Button>(Resource.Id.btnProductosServerSincronizar);
                btnSincronizar.Text = "Sincronizar productos";
                btnSincronizar.Enabled = true;
            }
        }
        private async void ObtenerCatProd()
        {
            try
            {
                var databasepath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "kigest_sltosAriel.db");
                var db = new SQLiteConnection(databasepath);
                
                RespuestaServerCategProductos respuestCatprod = await interfazCatprod.GetServerCategProductos();
                categoriaProductos = respuestCatprod.ListaCategoriaProductos;

                foreach (CategoriaProductosServer categoria in categoriaProductos)
                {
                    IEnumerable<ConsultasTablas> resultadocatprod = BuscarCatProd(db, categoria.id);
                    if (resultadocatprod.Count() == 0)
                    {
                        CategoriaProductos catprodLocal = new CategoriaProductos()
                        {
                            id = categoria.id,
                            nombre = categoria.nombre
                        };
                        dbUser.InsertarCateogoriaProd(catprodLocal);
                    }
                    else
                    {
                        CategoriaProductos catprodLocal = new CategoriaProductos()
                        {
                            id = categoria.id,
                            nombre = categoria.nombre
                        };
                        dbUser.ActualizarCategoriaProd(catprodLocal);

                    }
                }
            }
            catch(Exception ex)
            {
                Toast.MakeText(this, ex.Message + "-" + ex.StackTrace, ToastLength.Long).Show();
            }
        }     
        public static IEnumerable<ConsultasTablas> BuscarProductos(SQLiteConnection db, int idproductoMain)
        {
            {
                return db.Query<ConsultasTablas>("SELECT * FROM Productos where codProdMain=?", idproductoMain);
            }
        }
        public static IEnumerable<ConsultasTablas> BuscarCatProd(SQLiteConnection db, int idCatProd)
        {
            {
                return db.Query<ConsultasTablas>("SELECT * FROM CategoriaProductos where id=?", idCatProd);
            }
        }
    }
}