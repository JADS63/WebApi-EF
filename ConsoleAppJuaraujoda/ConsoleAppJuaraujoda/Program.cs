using System;
using System.Linq;
using ContextLib;
using Entities;
using StubbedContextLib;
using System.Linq;
namespace tutoRapideEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WtaContext context = new WtaContext())
            {
                Console.WriteLine("Accéder à toutes les joueuses");
                var ePlayers1 = context.Players;
                foreach (var player in ePlayers1)
                {
                    Console.WriteLine($"Prenom: {player.FirstName}");
                }

                Console.WriteLine(" Récupérer les joueuses dont la taille est strictement supérieure à 1.75m");
                var ePlayers2 = context.Players
                    .Where(n => n.Height > 1.75);
                foreach (var player in ePlayers2)
                {
                    Console.WriteLine($"Prenom: {player.FirstName}");
                }

                Console.WriteLine(" Récupérer les joueuses dont la taille est strictement supérieure à 1.75m et triées de la plus jeune à la plus âgée");
                var ePlayers3 = context.Players
                    .Where(n => n.Height>1.75);
                ePlayers3.OrderBy(n => n.BirthDate).ToList();
                foreach (var player in ePlayers3) {
                    Console.WriteLine($"Prenom: {player.FirstName}");
                }


                Console.WriteLine("Ajouter une joueuse");

                PlayerEntity nouvelleJouese = new PlayerEntity
                {
                    Id = 100,
                    FirstName = "Nouvelle Joueuse",
                    LastName = "Nouvelle",
                    BirthDate = new DateTime(1999, 5, 5),
                    Height = 1.89f,
                    Nationality = "Belarus",
                    Handplay = HandplayEntity.LeftAndRight
                };
                context.Add(nouvelleJouese);


                context.SaveChanges();
            }
        }
    }
}
