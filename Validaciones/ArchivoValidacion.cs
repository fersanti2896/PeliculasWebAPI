using System.ComponentModel.DataAnnotations;

namespace SPeliculasAPI.Validaciones {
    public class ArchivoValidacion : ValidationAttribute {
        private readonly int pesoMax;

        public ArchivoValidacion(int pesoMax) {
            this.pesoMax = pesoMax;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) { 
            if(value == null) { return ValidationResult.Success; }

            IFormFile formFile = value as IFormFile;

            if (formFile == null) { return ValidationResult.Success; }

            if (formFile.Length > pesoMax * 1024 * 1024) { return new ValidationResult($"El peso del archivo no debe ser mayor a {pesoMax} MB"); }

            return ValidationResult.Success;
        }
    }
}
