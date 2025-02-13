using System;
using System.Linq;
using ContextLib;
using Entities;
using StubbedContextLib;
namespace tutoRapideEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WtaContext context = new WtaContext())
            {
                context.SaveChanges();
            }
        }
    }
}
