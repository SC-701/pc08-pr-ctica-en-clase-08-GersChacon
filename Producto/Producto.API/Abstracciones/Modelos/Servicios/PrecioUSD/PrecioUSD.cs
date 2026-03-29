using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos.Servicios.Registro
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Dato
    {
        public string titulo { get; set; }
        public string periodicidad { get; set; }
        public List<Indicadore> indicadores { get; set; }
    }

    public class Indicadore
    {
        public string codigoIndicador { get; set; }
        public string nombreIndicador { get; set; }
        public List<Series> series { get; set; }
    }

    public class Root
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public List<Dato> datos { get; set; }
    }

    public class Series
    {
        public string fecha { get; set; }
        public double valorDatoPorPeriodo { get; set; }
    }




}
