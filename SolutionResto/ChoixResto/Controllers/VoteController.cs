using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ChoixResto.Models;
using ChoixResto.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ChoixResto.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private IDal dal;
        private readonly ILogger<VoteController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        // le principe d’injection de dépendances : Le principe est de créer une factory responsable de l’instanciation des contrôleurs utilisant un conteur IOC.
        public VoteController(IDal dalIoc, SignInManager<IdentityUser> signInManager, ILogger<VoteController> logger )
        {
            dal = dalIoc;
            _signInManager = signInManager;
            this._logger = logger;
        }

        //
        // GET: /Vote/
        public IActionResult Index(int id)
        {
            //_logger.LogInformation("Index Vote:" + dal.ADejaVote(id, Request.Headers["User-Agent"].ToString()));
            if (dal.ADejaVote(id,User.Identity.Name))
            {
                return RedirectToAction("AfficheResultat", new { id = id });
            }

            List<Resto> listeDesRestaurants = dal.ObtientTousLesRestaurants();
            List<RestaurantCheckBoxViewModel> listRestoVm = new List<RestaurantCheckBoxViewModel>();

            RestaurantVoteViewModel restoVoteVm = new RestaurantVoteViewModel();
            foreach (Resto resto in listeDesRestaurants)
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

        [HttpPost]
        public IActionResult Index(RestaurantVoteViewModel restoVoteVm, int id)
        {
            if (!ModelState.IsValid)
                return View(restoVoteVm);

            Utilisateur uti = dal.ObtenirUtilisateur(User.Identity.Name);

            if (uti == null)
                return StatusCode(401);

            foreach (RestaurantCheckBoxViewModel resto in restoVoteVm.ListeDesResto)
            {
                if (resto.EstSelectionne)
                    dal.AjouterVote(id, resto.Id, uti.Id);
            }

            return RedirectToAction("AfficheResultat", new { id = id });

        }

        public IActionResult AfficheResultat(int id)
        {
            if (!dal.ADejaVote(id, User.Identity.Name))
            {
                return RedirectToAction("Index", new { id = id });
            }

            List<Resultats> lstResultat = dal.ObtenirLesResultats(id);

            if (lstResultat == null)
                return View("Error");

            return View(lstResultat);
        }

    }
}