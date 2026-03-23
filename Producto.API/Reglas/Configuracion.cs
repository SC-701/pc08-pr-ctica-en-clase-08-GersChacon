using Abstracciones.Interfaces.Reglas;
using Microsoft.Extensions.Configuration;
using System;

namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private readonly IConfiguration _configuration;

        public Configuracion(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ObtenerMetodo(string seccion, string clave)
        {
            try
            {
                var ruta = $"{seccion}:{clave}";
                var valor = _configuration[ruta];

                if (string.IsNullOrEmpty(valor))
                {
                    throw new Exception($"No se encontró configuración para {seccion}.{clave}");
                }

                return valor;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener configuración {seccion}.{clave}: {ex.Message}", ex);
            }
        }
    }
}