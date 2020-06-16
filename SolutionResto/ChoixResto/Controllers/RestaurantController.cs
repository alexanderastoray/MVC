using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ChoixResto.Models;
using ChoixResto.ViewModels;

namespace ChoixResto.Controllers
{
    public class RestaurantController : Controller
    {

        private IDal dal;

        // le principe d’injection de dépendances : Le principe est de créer une factory responsable de l’instanciation des contrôleurs utilisant un conteur IOC.
        public RestaurantController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        //
        // GET: /Restaurant/
        // [ActionName("Index")] il est possible de faire en sorte que l’action d’un contrôleur ne porte pas le même nom qu’une méthode de la classe
        public IActionResult Index()
        {
            //using (IDal dal = new Dal())
            //{
            //    List<Resto> listeDesRestaurants = dal.ObtientTousLesRestaurants();
            //    return View(listeDesRestaurants);
            //}

            List<Resto> listeDesRestaurants = dal.ObtientTousLesRestaurants();
            return View(listeDesRestaurants);
        }

        public IActionResult ModifierRestaurant(int? id)
        {
            if (id.HasValue)
            {
                //using (IDal dal = new Dal())
                //{
                //    Resto restaurant = dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
                //    if (restaurant == null)
                //        return View("Error");
                //    return View(restaurant);
                //}
                Resto restaurant = dal.ObtientTousLesRestaurants().FirstOrDefault(r => r.Id == id.Value);
                if (restaurant == null)
                    return View("Error");
                return View(restaurant);
            }
            else
                return StatusCode(404);
                //return View("Error");
        }

        //[HttpPost] --> plus simple et typé l'example en bas
        //public ActionResult ModifierRestaurant(int? id, string nom, string telephone)
        //{
        //    if (id.HasValue)
        //    {
        //        using (IDal dal = new Dal())
        //        {
        //            dal.ModifierRestaurant(id.Value, nom, telephone);
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    else
        //        return View("Error");
        //}


        // ASP.NET MVC reconnaît bien que les propriétés ont les mêmes noms que les champs de formulaire.
        [HttpPost]
        public IActionResult ModifierRestaurant(Resto resto)
        {
            if (!ModelState.IsValid)
                return View(resto);

            dal.ModifierRestaurant(resto.Id, resto.Nom, resto.Telephone);
            return RedirectToAction("Index");

        //    using (IDal dal = new Dal())
        //    {
        //        dal.ModifierRestaurant(resto.Id, resto.Nom, resto.Telephone);
        //        return RedirectToAction("Index");
        //    }
        }

        public IActionResult CreerRestaurant()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreerRestaurant(Resto resto)
        {
            //using (IDal dal = new Dal())
            //{
            //    if (dal.RestaurantExiste(resto.Nom))
            //    {
            //        ModelState.AddModelError("Nom", "Ce nom de restaurant existe déjà");
            //        return View(resto);
            //    }
            //    if (!ModelState.IsValid)
            //        return View(resto);
            //    dal.CreerRestaurant(resto.Nom, resto.Telephone);
            //    return RedirectToAction("Index");
            //}

            if (dal.RestaurantExiste(resto.Nom))
            {
                ModelState.AddModelError("Nom", "Ce nom de restaurant existe déjà");
                return View(resto);
            }

            if (!ModelState.IsValid)
                return View(resto);
            
            dal.CreerRestaurant(resto.Nom, resto.Telephone);

            return RedirectToAction("Index");
        }

	}
}