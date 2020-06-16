using System;
using System.Collections.Generic;
using System.Text;

namespace ChoixResto.Models
{
    public interface IDal : IDisposable
    {
        void CreerRestaurant(string nom, string telephone);
        void ModifierRestaurant(int id, string nom, string telephone);
        bool RestaurantExiste(string nom);

        Utilisateur ObtenirUtilisateur(int id);
        Utilisateur ObtenirUtilisateur(string Prenom);
        int AjouterUtilisateur(string prenom, string motdepasse);
        Utilisateur Authentifier(string login, string motdepasse);

        int NouveauSondage();
        void AjouterVote(int idSondage, int idRestaurant, int idUtilisateur);
        bool ADejaVote(int idSondage, string idUtilisateur);

        List<Resto> ObtientTousLesRestaurants();

        List<Resultats> ObtenirLesResultats(int idSondage);
    }
}
