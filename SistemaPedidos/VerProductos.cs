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
using Android;
using SistemaPedidos.Resources.Model;
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources;
using Android.Database;

namespace SistemaPedidos
{
     [Activity(Label = "KIGEST - Productos")]
    public class VerProductos : Activity
    {
        
        ListView lstDatosProductos;
        List<Productos> lstOrigenProductos = new List<Productos>();
        ConsultasTablas dbUser;

        Spinner SeleccCategoria;
        List<CategoriaProductos> CategoriasProd;       
        ArrayAdapter adapter;
        SearchView BusquedaProducto;
        FuncionesGlobales funcionesGlobales = new FuncionesGlobales();
        int IdPoductoSel;
        string PrecioProdSel;
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuProductos, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.btnMnuProdVolver:
                    StartActivity(typeof(PaginaPrincipal));
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (VariablesGlobales.Idvendedor == 0)
            {
                Intent i = new Intent(this.ApplicationContext, typeof(MainActivity));
                i.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
                StartActivity(i);
                this.Finish();
            }
            SetContentView(Resource.Layout.ListViewProductos);
            lstDatosProductos = FindViewById<ListView>(Resource.Id.lstProductosLista);
            dbUser = new ConsultasTablas();
            
            LoadData();
            var btnVolverAlPedido = FindViewById<Button>(Resource.Id.btnProductosPedidoActual);

            SeleccCategoria = (Spinner)FindViewById(Resource.Id.spnProductosCategoria);
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            
            CategoriasProd = dbUser.verCategoriaProductos();
            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, CategoriasProd);            
            SeleccCategoria.Adapter = adapter;
            SeleccCategoria.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ListaCategorias_ItemSelected);
            

            var btnVolver = FindViewById<Button>(Resource.Id.btnProductosVolver);
            BusquedaProducto = (SearchView)FindViewById(Resource.Id.srcProductosBusqueda);
            BusquedaProducto.QueryTextSubmit += BusquedaProducto_QuerySubmit;
            BusquedaProducto.QueryTextChange += BusquedaProducto_TextChange;

            btnVolver.Click += delegate
            {
                StartActivity(typeof(PaginaPrincipal));
            };
            if (VariablesGlobales.PedidoEnCurso == true)
            {               
                lstDatosProductos.ItemClick += (s, e) =>
                {                                       
                    LayoutInflater layoutInflater = LayoutInflater.From(Application.Context);
                    
                    View dialogo = layoutInflater.Inflate(Resource.Layout.inputBoxCantProd, null);
                    
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    EditText cantProd = dialogo.FindViewById<EditText>(Resource.Id.txtInputCantProd);
                    EditText PrecioProd = dialogo.FindViewById<EditText>(Resource.Id.txtInputPrecioProd);                   
                    TextView mensajeInput = dialogo.FindViewById<TextView>(Resource.Id.txtInputCantMensaje);

                    builder.SetView(dialogo);
                    AlertDialog alertDialog = builder.Create();

                    alertDialog.SetCanceledOnTouchOutside(true);
                    alertDialog.SetTitle("Agregar producto");
                    
                    alertDialog.SetButton("Agregar", (ss, ee) =>

                    {
                        if(cantProd.Text=="" || cantProd.Text == "0")
                        {
                            mensajeInput.Text = "La cantidad debes ser un numero mayor que 0";
                        }
                        else
                        {
                            //agregamos el producto
                            List<Productos> agregaProd = new List<Productos>();
                            agregaProd = dbUser.VerListaProductosBusquedaID(IdPoductoSel);                            

                            string cant = cantProd.Text;
                            string pUnit = funcionesGlobales.CalcularPrecioLista(agregaProd[0].precio, agregaProd[0].ganancia, agregaProd[0].utilidad1 , 
                                agregaProd[0].utilidad2 , agregaProd[0].iva, VariablesGlobales.ListaPrecioCliente, agregaProd[0].calcular_precio );                                                 
                            if (PrecioProd.Text !="" & PrecioProd.Text  != "0")
                            {
                                pUnit = PrecioProd.Text;
                            }
                            string pTotal = (double.Parse(pUnit) * double.Parse(cantProd.Text)).ToString();
                            PedidosDetalle productoDetalle = new PedidosDetalle()
                            {
                                id_master = VariablesGlobales.IdPedidoenCurso,
                                cod = agregaProd[0].codigo,
                                plu = agregaProd[0].codigo,
                                codProdMain = agregaProd[0].codProdMain,
                                descripcion = agregaProd[0].descripcion,
                                iva = agregaProd[0].iva,
                                cantidad = cantProd.Text,
                                punit = pUnit,
                                ptotal = pTotal                                                               
                            };
                            dbUser.InsertarProductoPedido(productoDetalle);
                            Toast.MakeText(this, "Producto agregado!", ToastLength.Short).Show();
                        }
                    });
                    alertDialog.SetButton2("Cancelar", (sss, eee) =>
                    {
                        Toast.MakeText(this, "No se agrego el producto!", ToastLength.Short).Show();
                    });

                    IdPoductoSel = int.Parse(e.Id.ToString());
                    List<Productos> selectPrecio = new List<Productos>();                    
                    selectPrecio = dbUser.VerListaProductosBusquedaID(IdPoductoSel);
                    PrecioProdSel = funcionesGlobales.CalcularPrecioLista(selectPrecio[0].precio, selectPrecio[0].ganancia, selectPrecio[0].utilidad1,
                                selectPrecio[0].utilidad2, selectPrecio[0].iva, VariablesGlobales.ListaPrecioCliente, selectPrecio[0].calcular_precio );
                    
                    PrecioProd.Hint = "Precio: $" + PrecioProdSel;
                    alertDialog.Show();
                };
            }            
            if (VariablesGlobales.PedidoEnCurso == false){
                btnVolverAlPedido.Enabled = false;
            }
            else
            {
                btnVolverAlPedido.Enabled = true;
            }

            btnVolverAlPedido.Click += delegate
            {
                StartActivity(typeof(PedidoActual));
            };
        }
        void BusquedaProducto_QuerySubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            if (e.Query == "")
            {
                LoadData();
            }
            else
            {
                LoadDataBusq(e.Query);
            }
        }
        void BusquedaProducto_TextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (e.NewText == "")
            {
                LoadData();
            }
        }
        private void ListaCategorias_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner CategSelecc = (Spinner)sender;

            string CategoriaBusq = CategSelecc.GetItemAtPosition(e.Position).ToString(); 
            lstOrigenProductos = dbUser.VerListaProductosCategoria(CategoriaBusq);
            var adapter = new ProductoViewAdapter(this, lstOrigenProductos);
            lstDatosProductos.Adapter = adapter;
        //    Toast.MakeText(this, "Se cargo la categoria: " + CategSelecc.GetItemAtPosition(e.Position), ToastLength.Long).Show();

        }

        public void LoadData()
        {
            lstOrigenProductos = dbUser.VerListaProductos();
            var adapter = new ProductoViewAdapter(this, lstOrigenProductos);
            lstDatosProductos.Adapter = adapter;
        }
        //public void LoadDataCateg(int categoria)
        //{
        //    lstOrigenProductos = dbUser.VerListaProductosCategoria(categoria);
        //    var adapter = new ProductoViewAdapter(this, lstOrigenProductos);
        //    lstDatosProductos.Adapter = adapter;
        //}
        public void LoadDataBusq(string producto)
        {
            lstOrigenProductos = dbUser.VerListaProductosBusqueda(producto);
            var adapter = new ProductoViewAdapter(this, lstOrigenProductos);
            lstDatosProductos.Adapter = adapter;
        }

    }
}