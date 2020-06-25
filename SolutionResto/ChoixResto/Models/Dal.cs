using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ChoixResto.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChoixResto.Models
{
    public class Dal : IDal
    {
        private ChoixRestoContext bdd;
        private readonly ILogger<Dal> _logger;

        #region DAL 

        public Dal(ChoixRestoContext context, ILogger<Dal> logger)
        {
           bdd = context;
           _logger = logger;
        }

        public void Dispose()
        {
            bdd.Dispose();
        }

        private string EncodeMD5(string motDePasse)
        {
            /*
             * vous allez utiliser un système de salage MD5 pour enregistrer le mot de passe. 
             */
            string motDePasseSel = "ChoixResto" + motDePasse + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(motDePasseSel)));
        }

        #endregion

        #region Restaurant

        public void CreerRestaurant(string nom, string telephone)
        {
            bdd.Restos.Add(new Resto { Nom = nom, Telephone = telephone });
            bdd.SaveChanges();
        }

        public void ModifierRestaurant(int id, string nom, string telephone)
        {
            Resto restoTrouve = bdd.Restos.FirstOrDefault(resto => resto.Id == id);
            if (restoTrouve != null)
            {
                restoTrouve.Nom = nom;
                restoTrouve.Telephone = telephone;
                bdd.SaveChanges();
            }
        } 

        public List<Resto> ObtientTousLesRestaurants()
        {
            return bdd.Restos.ToList();
        }


        public bool RestaurantExiste(string nom)
        {
            return bdd.Restos.Any(resto => string.Compare(resto.Nom, nom, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        #endregion

        #region utilisateur

        public int AjouterUtilisateur(string prenom, string motdepasse)
        {
            String motdePasseMD5 = EncodeMD5(motdepasse);
            var NouveauUser = new Utilisateur { Prenom = prenom, MotdePasse = motdePasseMD5 };
            bdd.Utilisateurs.Add(NouveauUser);
            bdd.SaveChanges();

           return NouveauUser.Id; 
        }

        public Utilisateur Authentifier(string login, string motdepasse)
        {
            String motdePasseMD5 = EncodeMD5(motdepasse);
            Utilisateur userTrouve = bdd.Utilisateurs.FirstOrDefault(user => user.Prenom == login && user.MotdePasse == motdePasseMD5);

            return userTrouve;

        }

       
       public Utilisateur ObtenirUtilisateur(int id)
       {
           Utilisateur userTrouve = bdd.Utilisateurs.Find(id);

           return userTrouve;

       }

       
      public Utilisateur ObtenirUtilisateur(string idString)
      {
          int idUser;
          if (Int32.TryParse(idString, out idUser))
          {
              return bdd.Utilisateurs.FirstOrDefault(user => user.Id == idUser);
          }
          return null;
      }
      

        public bool UtilisateurtExiste(int id)
        {
            bool reponse = false;
            Utilisateur userTrouve = bdd.Utilisateurs.FirstOrDefault(user => user.Id == id);
            if (userTrouve != null)
            {
                reponse = true;
            }

            return reponse;
        }

        public bool UtilisateurtExiste(string prenom)
        {
            bool reponse = false;
            Utilisateur userTrouve = bdd.Utilisateurs.FirstOrDefault(user => user.Prenom == prenom);
            if (userTrouve != null)
            {
                reponse = true;
            }

            return reponse;
        }

        /*
        public Utilisateur ObtenirUtilisateur(string idStr)
        {
            switch (idStr)
            {
                case "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36":
                    return CreeOuRecupere("Nico", "1234");
                case "Mozilla/5.0 (iPhone; CPU iPhone OS 13_5_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.1.1 Mobile/15E148 Safari/604.1":
                    return CreeOuRecupere("Jérémie", "1234");
                case "Firefox":
                    return CreeOuRecupere("Delphine", "1234");
                default:
                    return CreeOuRecupere("Timéo", "1234");
            }
        }
        

        private Utilisateur CreeOuRecupere(string nom, string motDePasse)
        {
            Utilisateur utilisateur = Authentifier(nom, motDePasse);
            if (utilisateur == null)
            {
                int id = AjouterUtilisateur(nom, motDePasse);
                return ObtenirUtilisateur(id);
            }
            return utilisateur;
        }
        */

        #endregion
        
        #region Sondage 

        public int NouveauSondage()
        {
            int idSondage = 0;

            if (!bdd.Sondages.Count().Equals(0) && bdd.Sondages.Where(o => o.Votes.Count().Equals(0)).Count()>0)
            {
                idSondage = bdd.Sondages.Where(o => o.Votes.Count().Equals(0)).FirstOrDefault().Id;
            }
            else 
            {
                idSondage = bdd.Sondages.Where(o => o.Votes.Count()>0).FirstOrDefault().Id;
            }
           

            if (idSondage.Equals(0))
            {
                var NouveauSondage = new Sondage { Date = DateTime.Now.Date };
                bdd.Sondages.Add(NouveauSondage);
                bdd.SaveChanges();

                idSondage = NouveauSondage.Id;
            }

            return idSondage; 
        }

        public void AjouterVote(int idSondage, int idRestaurant, int idUtilisateur)
        {
            Utilisateur user = bdd.Utilisateurs.Find(idUtilisateur);
            Resto resto = bdd.Restos.Find(idRestaurant);

            Vote NouveaVote = new Vote { Resto = resto, Utilisateur = user };
            bdd.Votes.Add(NouveaVote);
            bdd.SaveChanges();

            Sondage sondage = bdd.Sondages.Find(idSondage);

            if (sondage != null)
            {
                if (sondage.Votes == null)
                    sondage.Votes = new List<Vote>();

                sondage.Votes.Add(NouveaVote);
                bdd.SaveChanges();
            }
        }
       
        public bool ADejaVote(int idSondage, string idUtilisateur)
        {
            bool result = false;
            int idUser;
            bool success = Int32.TryParse(idUtilisateur, out idUser);
             if (success)
             {
                 Utilisateur user = bdd.Utilisateurs.Find(idUser);
                 if (user != null)
                 {
                     if (bdd.Sondages.Find(idSondage).Votes != null)
                        result = bdd.Sondages.Find(idSondage).Votes.Exists(rst => rst.Utilisateur == user);
                 }
             }

            return result;

        }
       
        /*
        public bool ADejaVote(int idSondage, string idStr)
        {
            Utilisateur utilisateur = ObtenirUtilisateur(idStr);
            //_logger.LogInformation("Uti Id : " + utilisateur.Id);
            
            if (utilisateur != null)
            {
                Sondage sondage = bdd.Sondages.Where(s => s.Id == idSondage).Include("Votes.Utilisateur").FirstOrDefault();
                // _logger.LogInformation("AjouterVote  : " + sondage.Votes.Count());
                if (sondage.Votes == null)
                    return false;

                return sondage.Votes.Any(v => v.Utilisateur != null && v.Utilisateur.Id == utilisateur.Id);
            }
            return false;
        }
       */

        #endregion

        #region Resultat 
        public List<Resultats> ObtenirLesResultats(int idSondage)
        { 
            List<Resultats> lstResultat = new List<Resultats>();
            Sondage sondage = bdd.Sondages.Where(s => s.Id == idSondage)
                                                .Include("Votes.Resto")
                                                .FirstOrDefault();
            List<Vote> lstVote = sondage.Votes;

            var resultat = lstVote.GroupBy(vt => vt.Resto)
                            .Select(group => new { Resto = group.Key, Items = group.ToList() })
                            .ToList() ;

            foreach (var item in resultat)
            {
                lstResultat.Add(new Resultats { Nom = item.Resto.Nom, Telephone = item.Resto.Telephone, NombreDeVotes = item.Items.Count });
            }

            return lstResultat;
        }
        #endregion

    }
}