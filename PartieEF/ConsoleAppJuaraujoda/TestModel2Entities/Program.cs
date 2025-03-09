using ContextLib;
using Entities;
using Microsoft.EntityFrameworkCore;
using Model;
using Model2Entities;
using Shared;
using StubbedContextLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestModel2Entities
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<WtaContext>()
                .Options;

            using (var context = new StubbedContext(options))
            {

                // Création des services (couche entité)
                using var dbPlayersManager = new DbPlayersManager(context);
                using var dbTournamentsManager = new DbTournamentsManager(context);
                using var dbResultsManager = new DbResultsManager(context, dbPlayersManager, dbTournamentsManager);

                // Création des services (couche modèle)
                var playersManager = new PlayersManager(dbPlayersManager);
                var tournamentsManager = new TournamentsManager(dbTournamentsManager);
                var resultsManager = new ResultsManager(dbResultsManager, playersManager, tournamentsManager);


                // --- Tests (Exemples) ---

                // 1. Lister tous les joueurs
                Console.WriteLine("---- Liste de tous les joueurs ----");
                var allPlayers = await playersManager.GetAllAsync();
                foreach (var player in allPlayers)
                {
                    Console.WriteLine(player);
                }

                // 2. Lister tous les tournois
                Console.WriteLine("\n---- Liste de tous les tournois ----");
                var allTournaments = await tournamentsManager.GetAllAsync();
                foreach (var tournament in allTournaments)
                {
                    Console.WriteLine(tournament);
                }

                // 3. Ajouter un joueur
                Console.WriteLine("\n---- Ajout d'un joueur ----");
                var newPlayer = new Player(99, "Roger", "Federer", new DateTime(1981, 8, 8), 1.85f, "Swiss", HandplayEntity.Right);
                var addedPlayerModel = await playersManager.AddAsync(newPlayer);
                var addedPlayer = await playersManager.GetByIdAsync(addedPlayerModel.Id);
                Console.WriteLine($"Joueur ajouté : {addedPlayer}");


                // TEST MODIFIÉ : Filtre simplifié (ID > 0) - Pour éviter l'erreur de traduction LINQ
                Console.WriteLine("\n---- Liste de tous les joueurs (avec filtre ID > 0) ----");
                var filteredPlayers = await playersManager.GetAllAsync(p => p.Id > 0); 
                foreach (var player in filteredPlayers)
                {
                    Console.WriteLine(player);
                }

                Console.WriteLine("\n---- Liste des joueuses de plus de 1.75m, triées par âge (jeune à vieux) ----");
                var tallPlayersSortedByAge = (await playersManager.GetAllAsync(p => p.Height > 1.75f))
                                                    .OrderBy(p => p.BirthDate)
                                                    .ToList();
                foreach (var player in tallPlayersSortedByAge)
                {
                    Console.WriteLine(player);
                }



                // 4. Ajouter un tournoi
                Console.WriteLine("\n---- Ajout d'un tournoi ----");
                var newTournament = new Tournament(99, "Wimbledon", 2024);
                var addedTournamentModel = await tournamentsManager.AddAsync(newTournament);
                var addedTournament = await tournamentsManager.GetByIdAsync(addedTournamentModel.Id);
                Console.WriteLine($"Tournoi ajouté : {addedTournament}");

                // 5. Ajouter un résultat
                Console.WriteLine("\n---- Ajout d'un résultat ----");
                var newResult = new ResultEntity { Result = Result.Win };
                var addedResultEntity = await resultsManager.AddAsync(newResult);
                var addedResult = await resultsManager.GetByIdAsync(addedResultEntity.Id);
                Console.WriteLine($"Resultat ajouté : {addedResult.Result}");

                // 6. Associer un résultat à un joueur et un tournoi
                Console.WriteLine("\n---- Association d'un résultat ----");
                var playerEntity = await dbPlayersManager.GetByIdAsync(addedPlayerModel.Id);
                var tournamentEntity = await dbTournamentsManager.GetByIdAsync(addedTournamentModel.Id);
                var resultEntity = await dbResultsManager.GetByIdAsync(addedResultEntity.Id);

                if (playerEntity != null && tournamentEntity != null && resultEntity != null)
                {
                    resultEntity.Players.Add(playerEntity);
                    resultEntity.Tournaments.Add(tournamentEntity);
                    await dbResultsManager.UpdateAsync(resultEntity);
                    Console.WriteLine($"Resultat {addedResultEntity.Id} associé au joueur {addedPlayerModel.Id} et tournoi {addedTournamentModel.Id}");
                }
                else
                {
                    Console.WriteLine("Erreur lors de l'association du résultat : Joueur, Tournoi ou Resultat non trouvé.");
                }


                // 7. Récupérer les résultats d'un joueur (MODIFIÉ pour afficher les détails du tournoi)
                Console.WriteLine("\n---- Résultats d'un joueur (avec détails du tournoi) ----");
                var playerResults = await resultsManager.GetResultsByPlayerIdAsync(addedPlayerModel.Id);
                foreach (var result in playerResults)
                {
                    var tournament = result.Tournaments.FirstOrDefault();
                    Console.WriteLine($"Resultat du joueur {addedPlayer.FirstName} au tournoi {tournament?.Name ?? "N/A"} : {result.Result}");
                }

                // 8. Récupérer les résultats d'un tournoi (MODIFIÉ pour afficher les détails des joueurs)
                Console.WriteLine("\n---- Résultats d'un tournoi (avec détails des joueurs) ----");
                var tournamentResults = await resultsManager.GetResultsByTournamentIdAsync(addedTournamentModel.Id);
                foreach (var result in tournamentResults)
                {
                    var player = result.Players.FirstOrDefault();
                    Console.WriteLine($"Resultat du joueur {player?.FirstName ?? "N/A"} au tournoi {addedTournament.Name} : {result.Result}");
                }

                // NOUVEAU TEST POUR LA PARTIE 3B : Résultats complets par tournoi
                Console.WriteLine("\n---- Résultats COMPLETS d'un tournoi (avec détails joueurs et tournois) ----");
                var fullTournamentResults = await resultsManager.GetFullResultsByTournamentIdAsync(addedTournamentModel.Id);
                foreach (var result in fullTournamentResults)
                {
                    Console.WriteLine($"Résultat ID: {result.Id}, Statut: {result.Result}");
                    Console.WriteLine($"  Tournoi: {result.Tournaments.FirstOrDefault()?.Name ?? "N/A"} ({result.Tournaments.FirstOrDefault()?.Year})");
                    Console.WriteLine($"  Joueur(s):");
                    foreach (var player in result.Players)
                    {
                        Console.WriteLine($"    - {player.FirstName} {player.LastName}");
                    }
                }


                // 9. Supprimer un résultat d'un joueur dans un tournoi
                Console.WriteLine("\n---- Suppression d'un résultat ----");
                if (addedResultEntity != null)
                {
                    await resultsManager.DeleteAsync(addedResultEntity.Id);
                    Console.WriteLine($"Resultat {addedResultEntity.Id} supprimé.");
                }


                // 10. Vérifier la suppression
                playerResults = await resultsManager.GetResultsByPlayerIdAsync(addedPlayerModel.Id);
                Console.WriteLine($"Nombre de résultats pour le joueur après suppression: {playerResults.Count}");

                // 11. Modifier un joueur
                Console.WriteLine("\n---- Modification d'un joueur ----");
                if (addedPlayer != null)
                {
                    addedPlayer.Height = 1.86f;
                    await playersManager.UpdateAsync(addedPlayer);
                    var updatedPlayer = await playersManager.GetByIdAsync(addedPlayer.Id);
                    Console.WriteLine($"Joueur modifié : {updatedPlayer}");
                }

                // 12. Supprimer un joueur
                Console.WriteLine("\n---- Suppression d'un joueur ----");
                if (addedPlayer != null)
                {
                    await playersManager.DeleteAsync(addedPlayer.Id);
                    var deletedPlayer = await playersManager.GetByIdAsync(addedPlayer.Id);
                    Console.WriteLine(deletedPlayer == null ? "Joueur supprimé avec succès." : "Erreur lors de la suppression.");
                }


                // 13. Supprimer un tournoi
                Console.WriteLine("\n---- Suppression d'un tournoi ----");
                if (addedTournament != null)
                {
                    await tournamentsManager.DeleteAsync(addedTournament.Id);
                    var deletedTournament = await tournamentsManager.GetByIdAsync(addedTournamentModel.Id);
                    Console.WriteLine(deletedTournament == null ? "Tournoi supprimé avec succès." : "Erreur lors de la suppression.");
                }
            }

            Console.ReadKey();
        }
    }
}