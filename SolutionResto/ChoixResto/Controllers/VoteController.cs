using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ChoixResto.Models;
using ChoixResto.ViewModels;

namespace ChoixResto.Controllers
{
    public class VoteController : Controller
    {
        private IDal dal;
        private int IdSondage;

        // le principe d’injection de dépendances : Le principe est de créer une factory responsable de l’instanciation des contrôleurs utilisant un conteur IOC.
        public VoteController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        //
        // GET: /Vote/
        public IActionResult Index(int? idSondage)
        {
            if (idSondage.HasValue)
            {
                List<Resto> listeDesRestaurants = dal.ObtientTousLesRestaurants();
                List<RestaurantCheckBoxViewModel> listRestoVm = new List<RestaurantCheckBoxViewModel>();
                RestaurantVoteViewModel restoVoteVm = new RestaurantVoteViewModel();
                this.IdSondage = idSondage ?? default(int);
                foreach(Resto resto in listeDesRestaurants)
                {
                    RestaurantCheckBoxViewModel vm = new RestaurantCheckBoxViewModel
                    {
                        Id = resto.Id,
                        NomEtTelephone = resto.Nom + " - (" + resto.Telephone + ")",
                        EstSelectionne = false
                    };
                    listRestoVm.Add(vm);
                }
                
                restoVoteVm.ListeDesResto = listRestoVm;

                return View(restoVoteVm);
            }
             else
                return StatusCode(404);
            
        }
        
        [HttpPost]    
        public IActionResult Index(RestaurantVoteViewModel restoVoteVm)
        {
           if (!ModelState.IsValid)
                return View(restoVoteVm);

           Utilisateur uti = dal.ObtenirUtilisateur(Request.Headers["User-Agent"].ToString());

           foreach(RestaurantCheckBoxViewModel resto in restoVoteVm.ListeDesResto)
           {
               dal.AjouterVote(IdSondage, resto.Id, uti.Id);
           }

           return RedirectToAction("AfficheResultat", IdSondage);
        }

        public IActionResult AfficheResultat(int? id)
        {
            if (id.HasValue)
            {
                List<Resultats> lstResultat = dal.ObtenirLesResultats(id ?? default(int));
                    if (lstResultat == null)
                        return View("Error");
                    return View(lstResultat);
            }
            else
                return StatusCode(404);
        }

	}
}