using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChoixResto.ViewModels
{
    public class RestaurantVoteViewModel : IValidatableObject
    {
        public List<RestaurantCheckBoxViewModel> ListeDesResto { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if(ListeDesResto.Where(o=>o.EstSelectionne).Count().Equals(0))
            {
            results.Add(new ValidationResult("Vous devez choisir au moins un restaurant"));
            }

            return results;
        }
    }
}