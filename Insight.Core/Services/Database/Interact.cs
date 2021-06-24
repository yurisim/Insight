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
         using (var insightContext = new InsightContext())
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

        public static Person GetPersonsByName(string name)
        {
            List<Person> persons = new List<Person>();
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                    persons = insightContext.Persons.Where(x => x.FirstName + ' ' + x.LastName == name).ToList();
                }
            }
            catch (Exception)
            {
                throw new Exception("Insight.db access error");
            }
            if (persons.Count > 1)
            {
                throw new Exception("too many results");
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