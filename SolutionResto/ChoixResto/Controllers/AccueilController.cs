using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ChoixResto.Models;
using ChoixResto.ViewModels;
using Microsoft.Extensions.Logging;

namespace ChoixResto.Controllers
{
    public class AccueilController : Controller
    {
        private IDal dal;
        private readonly ILogger<AccueilController> _logger;

        // le principe d’injection de dépendances : Le principe est de créer une factory responsable de l’instanciation des contrôleurs utilisant un conteur IOC.
        public AccueilController(IDal dalIoc, ILogger<AccueilController> logger)
        {
            dal = dalIoc;
            _logger = logger;
        }


        //
        // GET: /Accueil/
        public IActionResult Index()
        {
            return View();

            //ViewData["message"] = "Bonjour depuis le contrôleur";
            //ViewData["date"] = DateTime.Now;
            ////ViewData["resto"] = new Resto { Nom = "Choucroute party", Telephone = "1234" };

            //dynamic resto = new Resto();
            //resto.Nom = "Resto dynam-hic";
            //resto.Telephone = "1234";

            //ViewData["resto"] = resto;

            //return View("Index");

            //Resto r = new Resto { Nom = "La bonne fourchette", Telephone = "1234" };
            //return View(r);

            //AccueilViewModel vm = new AccueilViewModel
            //{
            //    Message = "Bonjour depuis le contrôleur",
            //    Date = DateTime.Now,
            //    Login = "Alexander",
            //    Resto = new Resto { Nom = "La bonne fourchette", Telephone = "1234" }
            //};
            //return View(vm);
            //return new HttpUnauthorizedResult(); --> OK HTTP Error 401.0 - Unauthorized
            //return HttpNotFound();  --> OK HTTP Error 404.0 - Not Found
            //return RedirectToRoute(new { controller = "Restaurant", action = "Index" }); ---> OK
        }

        [HttpPost]
        public ActionResult Index(Utilisateur user)
        {
            int IdSOndage = dal.NouveauSondage();

            /*
            if (!dal.ADejaVote(IdSOndage,Request.Headers["User-Agent"].ToString()))
            {
                //_logger.LogInformation("Voici le log: " + uaString);
                return RedirectToRoute(new { controller = "Vote", action = "Index", id = IdSOndage });
            }
            */
            return RedirectToRoute(new { controller = "Vote", action = "Index", id = IdSOndage });
            //return StatusCode(404); 
           
        }
        
        //[ChildActionOnly] On peut marquer la méthode de notre contrôleur comme n’étant appelable qu’à partir d’une vue mère
        //public ActionResult AfficheListeRestaurant()
        //{
        //    List<Models.Resto> listeDesRestos = new List<Resto>
        //    {
        //        new Resto { Id = 1, Nom = "Resto pinambour", Telephone = "1234" },
        //        new Resto { Id = 2, Nom = "Resto tologie", Telephone = "1234" },
        //        new Resto { Id = 5, Nom = "Resto ride", Telephone = "5678" },
        //        new Resto { Id = 9, Nom = "Resto toro", Telephone = "555" },
        //    };
        //    return PartialView(listeDesRestos);
        //}
	}
}