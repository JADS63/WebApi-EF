using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public enum HandPlay
    {
        None = 0,
        Left = 1,
        Right = 2,
        LeftAndRight = 3
    }
    public enum SortCriteria
    {
        ByNameThenFirstName = 1,
        ByNameThenFirstNameDesc,
        ByNationality,
        ByNationalityDesc,
        ByBirthDate,
        ByBirthDateDesc
    }
    public class Player
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }
        public double Height { get; set; }
        public DateTime BirthDate { get; set; }
        public HandPlay HandPlay { get; set; }
        [Required]
        [StringLength(50)]
        public string? Nationality { get; set; }
    }
}