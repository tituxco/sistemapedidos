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
using SQLite;

namespace SistemaPedidos.Resources.Model
{
    class TablaSincronizaciones
    {
        [PrimaryKey, AutoIncrement]
        public int idSincro { get; set; }
        public string TablaSincro { get; set; }
        public DateTime ultimaSincro { get; set; }

    }
}