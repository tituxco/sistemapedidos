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
using Java.Util;
using SistemaPedidos.Resources;
using SistemaPedidos.Resources.DataHelper;
using SistemaPedidos.Resources.Model;
using static Android.Widget.RadioGroup;

namespace SistemaPedidos
{
    [Activity(Label = "Sistema de pedidos - Ver pedidos")]
    public class VerPedidos : Activity
    {
        ListView lstDatos;
        List<PedidosMaster> lstOrigen = new List<PedidosMaster>();
        ConsultasTablas dbUser;

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.ListViewPedidos);
            dbUser = new ConsultasTablas();
            lstDatos = FindViewById<ListView>(Resource.Id.lstPedidosLista);
            RadioGroup tiposPed = FindViewById<RadioGroup>(Resource.Id.rdgTiposPedidos);
            RadioButton fin = FindViewById<RadioButton>(Resource.Id.rd0);
            RadioButton enviar = FindViewById<RadioButton>(Resource.Id.rd1);
            RadioButton todos = FindViewById<RadioButton>(Resource.Id.rd2);
            var btnVolver = FindViewById<Button>(Resource.Id.btnPedidosListaVolver);
            LoadData(1);
            tiposPed.CheckedChange += delegate
            {
                if (fin.Checked==true)
                {
                    LoadData(0);
                };
                if (enviar.Checked==true)
                {
                    LoadData(1);
                };
                if(todos.Checked==true)
                {
                    LoadData(2);
                };
            };
            btnVolver.Click += delegate
            {
                StartActivity(typeof(PaginaPrincipal));
            };
            lstDatos.ItemClick += (s, e) =>
            {
                VariablesGlobales.IdPedidoenCurso = int.Parse(e.Id.ToString());
                StartActivity(typeof(PedidoActual));
            };
        }
        public void LoadData(int tipo)
        {
            lstOrigen = dbUser.verListaPedidos(tipo);
            var adapter = new PedidosViewAdapter(this, lstOrigen);
            lstDatos.Adapter = adapter;
        }
    }
}