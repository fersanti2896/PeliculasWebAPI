using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.Validaciones {
    public class TipoArchivoValidacion : ValidationAttribute {
        private readonly string[] tiposValidos;

        public TipoArchivoValidacion(TipoArchivo tipoArchivo) {
            if (tipoArchivo == TipoArchivo.Imagen) {
                tiposValidos = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        public TipoArchivoValidacion(string[] tiposValidos) {
            this.tiposValidos = tiposValidos;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value == null) { return ValidationResult.Success; }

            IFormFile formFile = value as IFormFile;

            if (formFile == null) { return ValidationResult.Success; }

            if (!tiposValidos.Contains(formFile.ContentType)) { 
                return new ValidationResult($"El tipo de archivos aceptados: {string.Join(", ", tiposValidos)}"); 
            }

            return ValidationResult.Success;
        }
    }
}
