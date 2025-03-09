using System;
using System.Linq;
using ContextLib;
using Entities;
using StubbedContextLib;
using Microsoft.EntityFrameworkCore;

namespace tutoRapideEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WtaContext context = new StubbedContext())
            {
                // Création de la base et application des migrations
                try
                {
                    // context.Database.EnsureDeleted(); // Supprime la base existante
                    // context.Database.Migrate();         // Applique toutes les migrations et recrée la base
                    Console.WriteLine("Base de données recréée et migrations appliquées.");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'application des migrations : {ex.Message}");
                }

                // ======================================================
                // Partie 2a : Gestion des résultats et associations
                // ======================================================

                // 1. Afficher toutes les joueuses avec leurs résultats
                Console.WriteLine("\n--- Liste des joueuses avec leurs résultats ---");
                var playersWithResults = context.Players
                                                .Include(p => p.Results)
                                                    .ThenInclude(r => r.Tournaments)
                                                .ToList();
                foreach (var player in playersWithResults)
                {
                    Console.WriteLine($"ID: {player.Id}, Nom: {player.FirstName} {player.LastName}");
                    if (player.Results.Any())
                    {
                        foreach (var res in player.Results)
                        {
                            Console.WriteLine($"\tRésultat ID: {res.Id}, Statut: {res.Result}");
                            if (res.Tournaments.Any())
                            {
                                Console.WriteLine("\t\tTournois associés:");
                                foreach (var tournament in res.Tournaments)
                                {
                                    Console.WriteLine($"\t\t\t- {tournament.Name} ({tournament.Year})");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tAucun résultat associé.");
                    }
                }

                // 2. Afficher tous les résultats avec les joueuses et tournois associés
                Console.WriteLine("\n--- Liste des résultats avec joueuses et tournois ---");
                var resultsWithDetails = context.Results
                                                .Include(r => r.Players)
                                                .Include(r => r.Tournaments)
                                                .ToList();
                foreach (var res in resultsWithDetails)
                {
                    Console.WriteLine($"Résultat ID: {res.Id}, Statut: {res.Result}");
                    Console.WriteLine("   Joueurs associés:");
                    foreach (var player in res.Players)
                    {
                        Console.WriteLine($"\tID: {player.Id}, {player.FirstName} {player.LastName}");
                    }
                    Console.WriteLine("   Tournois associés:");
                    foreach (var tournament in res.Tournaments)
                    {
                        Console.WriteLine($"\tID: {tournament.Id}, {tournament.Name} ({tournament.Year})");
                    }
                }

                // 3. Ajouter un nouveau résultat et l'associer à une joueuse et un tournoi
                Console.WriteLine("\n--- Ajout d'un nouveau résultat et association ---");
                ResultEntity newResult = new ResultEntity { Result = Result.Semifinal };

                var player42 = context.Players.FirstOrDefault(p => p.Id == 42);
                var tournament42 = context.Tournaments.FirstOrDefault(t => t.Id == 42);
                if (player42 != null && tournament42 != null)
                {
                    newResult.Players.Add(player42);
                    newResult.Tournaments.Add(tournament42);
                    context.Results.Add(newResult);
                    try
                    {
                        context.SaveChanges();
                        Console.WriteLine($"Nouveau résultat ajouté avec l'ID {newResult.Id} et associé avec succès.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'ajout du nouveau résultat : {ex.Message}");
                        Console.WriteLine($"Details: {ex}");
                    }
                }
                else
                {
                    Console.WriteLine("Impossible de trouver la joueuse ou le tournoi pour l'association.");
                }

                // 4. Modification d'un résultat existant
                Console.WriteLine("\n--- Modification d'un résultat existant ---");
                var resultToUpdate = context.Results.FirstOrDefault(r => r.Id == 42);
                if (resultToUpdate != null)
                {
                    resultToUpdate.Result = Result.Final;
                    try
                    {
                        context.SaveChanges();
                        Console.WriteLine($"Résultat avec ID {resultToUpdate.Id} modifié avec succès.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la modification du résultat : {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun résultat trouvé avec l'ID 42 pour modification.");
                }

                // 5. Suppression d'un résultat et dissociation des relations
                Console.WriteLine("\n--- Suppression d'un résultat ---");
                var resultToDelete = context.Results
                                            .Include(r => r.Players)
                                            .Include(r => r.Tournaments)
                                            .FirstOrDefault(r => r.Id == 43);
                if (resultToDelete != null)
                {
                    // Dissociation des relations (No need to clear collections with Many-to-Many and cascade delete disabled by default)
                    // resultToDelete.Players.Clear(); // Not needed for implicit many-to-many
                    // resultToDelete.Tournaments.Clear(); // Not needed for implicit many-to-many

                    context.Results.Remove(resultToDelete);
                    try
                    {
                        context.SaveChanges();
                        Console.WriteLine("Résultat supprimé avec succès.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la suppression du résultat : {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun résultat trouvé avec l'ID 43 pour suppression.");
                }
            }

            Console.WriteLine("\nToutes les opérations ont été effectuées. Appuyez sur une touche pour fermer l'application.");
            Console.ReadKey();
        }
    }
}