using System;
using System.Collections.Generic;

namespace contactoEFAPI.Models
{
    public partial class Contacto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Empresa { get; set; }
        public string? Email { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? ImagenPerfil { get; set; }
    }
}
