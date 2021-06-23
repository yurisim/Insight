using Insight.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insight.Core.Services.Database
{
   public static class Interact
   {
      public static void EnsureDB()
      {
         using (InsightContext insightContext = new InsightContext())
         {
            //Ensure database is created
            _ = insightContext.Database.EnsureCreated();
         }
      }

      public static void AddPerson(Person person)
      {
         using (InsightContext insightContext = new InsightContext())
         {
            _ = insightContext.Persons.Add(person);

            _ = insightContext.SaveChanges();
         }
      }

      /// <summary>
      /// Returns all Person objects from database
      /// </summary>
      /// <returns></returns>
      public static List<Person> GetAllPersons()
      {
         List<Person> persons = new List<Person>();
         try
         {
            using (InsightContext insightContext = new InsightContext())
            {
               persons = insightContext.Persons.Select(x => x).ToList();
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