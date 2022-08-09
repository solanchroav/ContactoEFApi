namespace contactoEFAPI.Models.Dto
{
    public class ContactoDto
    {
        public string Nombre { get; set; } = null!;
        public string? Empresa { get; set; }
        public string? Email { get; set; }
        public string? FechaNacimiento { get; set; }
        public int? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? ImagenPerfil { get; set; }
    }
}
