using Insight.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Insight.Core.Services.Database
{
   public static class Interact
   {
      public static void EnsureDatabase()
      {
         using (InsightContext insightContext = new InsightContext())
         {
            //Ensure database is created
            _ = insightContext.Database.EnsureCreated();
         }
      }

      public static async void AddPerson(Person person)
      {
         using (InsightContext insightContext = new InsightContext())
         {
            _ = insightContext.Persons.Add(person);

            _ = await insightContext.SaveChangesAsync();
         }
      }

      /// <summary>
      /// Returns all Person objects from database
      /// </summary>
      /// <returns></returns>
      public static async Task<List<Person>> GetAllPersons()
      {
         List<Person> persons;

         try
         {
            using (var insightContext = new InsightContext())
            {
               persons = await insightContext.Persons.Select(x => x).ToListAsync();
            }
         }
         catch (Exception)
         {
            throw new Exception("Insight.db access error");
         }

         return persons;
      }

      public static async Task<List<Person>> GetAllPersonsWithShortName(string ShortName, Org org)
      {
         List<Person> persons;

         try
         {
            using (var insightContext = new InsightContext())
            {
               persons = await insightContext.Persons.Select(x => x).ToListAsync();
            }
         }
         catch (Exception)
         {
            throw new Exception("Insight.db access error");
         }

         return persons;
      }
   }
}