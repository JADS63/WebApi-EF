using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Tournament
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public int Year { get; set; }


        public Tournament(int id, string name, int year)
        {
            Id = id;
            Name = name ?? "";
            Year = year;
        }

        public override bool Equals(object? obj)
        {
            return obj is Tournament tournament &&
                   Id == tournament.Id &&
                   Name == tournament.Name &&
                   Year == tournament.Year;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Year);
        }

        public override string ToString()
        {
            return $"Tournament: {Name} ({Year})";
        }
    }
}