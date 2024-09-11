using System.ComponentModel.DataAnnotations;

namespace peliculasWebApi.Validaciones
{
<<<<<<< HEAD
    public class PrimeraLetraMayuscula : ValidationAttribute
=======
    public class PrimeraLetraMayuscula: ValidationAttribute
>>>>>>> 2af95bc638e0cb8dfd888ba82555fb6ab1b34aea
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()[0].ToString();

            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }

            return ValidationResult.Success;

        }
    }
}
