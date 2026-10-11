using System;
using System.Collections.Generic;
using System.Text;

namespace DepoStokTakip
{
    internal class Urun
    {
        public string Ad { get; set; } = "";
        public string Kod { get; set; } = "";
        public string Kategori { get; set; } = "";
        public int Stok { get; set; } = -1;
    }
}
