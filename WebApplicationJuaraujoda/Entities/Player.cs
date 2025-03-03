using System;

namespace Entities
{
    /// <summary>
    /// Énumération définissant le type de jeu de main.
    /// </summary>
    public enum HandPlay
    {
        None = 0,
        Left = 1,
        Right = 2,
        LeftAndRight = 3
    }

    /// <summary>
    /// Représente un joueur ou une joueuse dans le domaine métier.
    /// </summary>
    public class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Height { get; set; }
        public DateTime BirthDate { get; set; }
        public HandPlay HandPlay { get; set; }
        public string Nationality { get; set; }
    }
}
