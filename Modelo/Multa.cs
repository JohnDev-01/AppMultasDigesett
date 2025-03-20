using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMultasDigesett.Modelo
{
    public class Multa
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CodigoMarbete { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public int Anio { get; set; }
        public string Placa { get; set; }
        public string TipoInfraccion { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }
        public string FotoPath { get; set; }
        public string AudioPath { get; set; }
    }

}
