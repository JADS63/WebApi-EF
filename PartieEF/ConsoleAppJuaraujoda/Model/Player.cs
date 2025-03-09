// Model/Player.cs
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Player
    {
        public int Id { get; init; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public float Height { get; set; } // <-- VERIFY: Is this float?
        public string Nationality { get; set; }
        public HandplayEntity Handplay { get; set; }


        public Player(int id, string firstName, string lastName, DateTime birthDate, float height, string nationality, HandplayEntity handplay)
        {
            Id = id;
            FirstName = firstName ?? "";
            LastName = lastName ?? "";
            BirthDate = birthDate;
            Height = height;
            Nationality = nationality ?? "";
            Handplay = handplay;
        }

        public override bool Equals(object? obj)
        {
            return obj is Player player &&
                   Id == player.Id &&
                   FirstName == player.FirstName &&
                   LastName == player.LastName &&
                   BirthDate == player.BirthDate &&
                   Height == player.Height &&
                   Nationality == player.Nationality &&
                   Handplay == player.Handplay;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, BirthDate, Height, Nationality, Handplay);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Player ID: {Id}");
            builder.AppendLine($"  Name: {FirstName} {LastName}");
            builder.AppendLine($"  Born: {BirthDate.ToShortDateString()}");
            builder.AppendLine($"  Height: {Height}m");
            builder.AppendLine($"  Nationality: {Nationality}");
            builder.AppendLine($"  Handplay: {Handplay}");
            return builder.ToString();
        }
    }
}