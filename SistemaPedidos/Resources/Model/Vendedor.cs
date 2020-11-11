﻿using System;
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
    class Vendedor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        
    }
}