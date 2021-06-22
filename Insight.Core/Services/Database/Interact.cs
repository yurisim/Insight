using Insight.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insight.Core.Services.Database
{
   public static class Interact
   {
      public static void EnsureDB()
      {
         using (var insightContext = new InsightContext())
         {
            //Ensure database is created
            insightContext.Database.EnsureCreated();
         }
      }

      public static void AddPerson(Person person)
      {
         using (var insightContext = new InsightContext())
         {
            insightContext.Persons.Add(person);

            insightContext.SaveChanges();
         }
      }
   }
}