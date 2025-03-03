using System;

namespace Dto
{

    public enum HandPlay
    {
        None = 0,
        Left = 1,
        Right = 2,
        LeftAndRight = 3
    }


    public class PlayerDto
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
