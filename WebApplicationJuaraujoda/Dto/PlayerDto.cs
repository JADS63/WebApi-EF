using System;
using System.ComponentModel.DataAnnotations;
using Entities;

namespace Dto
{
    public class PlayerDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Le nom de famille est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom de famille ne peut pas dépasser 50 caractères.")]
        public string? LastName { get; set; }

        [Range(0, 3, ErrorMessage = "La valeur de HandPlay doit être comprise entre 0 et 3.")]
        public HandPlay HandPlay { get; set; } 

        public double Height { get; set; }

        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "La nationalité est obligatoire.")]
        [StringLength(50, ErrorMessage = "La nationalité ne peut pas dépasser 50 caractères.")]
        public string? Nationality { get; set; }
    }
}