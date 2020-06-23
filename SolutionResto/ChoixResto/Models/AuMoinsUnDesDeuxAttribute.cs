using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ChoixResto.Models
{
    public class AuMoinsUnDesDeuxAttribute : ValidationAttribute
    {
        public string Parametre1 { get; set; }
        public string Parametre2 { get; set; }

        public AuMoinsUnDesDeuxAttribute(): base("AAA Vous devez saisir au moins un moyen de contacter le restaurant")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo[] proprietes = validationContext.ObjectType.GetProperties();
            PropertyInfo info1 = proprietes.FirstOrDefault(p => p.Name == Parametre1);
            PropertyInfo info2 = proprietes.FirstOrDefault(p => p.Name == Parametre2);

            string valeur1 = info1.GetValue(validationContext.ObjectInstance) as string;
            string valeur2 = info2.GetValue(validationContext.ObjectInstance) as string;

            if (string.IsNullOrWhiteSpace(valeur1) && string.IsNullOrWhiteSpace(valeur2))
                return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;
        }
    }
}