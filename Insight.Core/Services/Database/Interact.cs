using Insight.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            using (InsightContext insightContext = new InsightContext())
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

        public static Person GetPersonsByName(string firstName, string lastName)
        {
           List<Person> persons = new List<Person>();
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                    //TODO put AEF formatting stuff in DigestAEF

                    var folks = insightContext.Persons.Select(x => x).ToList();

                    Debug.WriteLine(insightContext.Persons.Count());

                    //persons = insightContext.Persons.Where(x => x.FirstName.ToUpper() == firstName && x.LastName.ToUpper() == lastName).ToList();
                    persons = (from x in insightContext.Persons
                              where x.FirstName.ToUpper() == firstName && x.LastName.ToUpper() == lastName
                              select x).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Insight.db access error");
            }

            if (persons.Count != 1)
            {
                throw new Exception("sdoihdsoighskldjglkghsd");
            }

            return persons[0];
        }
        public static void UpdatePerson(Person person)
        {
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                    _ = insightContext.Persons.Update(person);
                    _ = insightContext.SaveChanges();
                }

            }
            catch (Exception)
            {
                throw new Exception("Insight.db access error");
            }
        }

        public static void AddMedical(Medical medical)
        {
            using (InsightContext insightContext = new InsightContext())
            {
                _ = insightContext.Medicals.Add(medical);

                _ = insightContext.SaveChanges();
            }
        }

        public static void AddTraining(Training training)
        {
            using (InsightContext insightContext = new InsightContext())
            {
                _ = insightContext.Trainings.Add(training);

                _ = insightContext.SaveChanges();
            }
        }
        public static void AddPersonnel(Personnel personnel)
        {
            using (InsightContext insightContext = new InsightContext())
            {
                _ = insightContext.Personnels.Add(personnel);

                _ = insightContext.SaveChanges();
            }
        }

    }
}