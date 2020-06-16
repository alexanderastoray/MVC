using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChoixResto.Models
{
    /*
     Les annotations [Table("Restos")] [Required, MaxLength(80)] 
     * 
     */
    [Table("Resto")]
    public class Resto 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du restaurant doit être saisi")]
        public string Nom { get; set; }

        [Display(Name = "Téléphone")]
        [RegularExpression(@"^0[0-9]{9}$",ErrorMessage="Le numéro de téléphone est incorrect")]
        [AuMoinsUnDesDeux(Parametre1 = "Telephone", Parametre2 = "Email", ErrorMessage = "Vous devez saisir au moins un moyen de contacter le restaurant")]
        public string Telephone { get; set; }

        [AuMoinsUnDesDeux(Parametre1 = "Telephone", Parametre2 = "Email", ErrorMessage = "Vous devez saisir au moins un moyen de contacter le restaurant")]
        public string Email { get; set; }

        // previament faudra faire Resto : IValidatableObject
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.IsNullOrWhiteSpace(Telephone) && string.IsNullOrWhiteSpace(Email))
        //        yield return new ValidationResult("Vous devez saisir au moins un moyen de contacter le restaurant", new[] { "Telephone", "Email" });
        //}
    }
}