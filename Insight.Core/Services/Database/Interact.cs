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

        public static async Task<List<Person>> GetAllPersons(Org org)
        {
            List<Person> persons;
             
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                    persons = await insightContext.Persons.Where(x => x.Organization == org).Select(x => x).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw new Exception("Insight.db access error");
            }

            return persons;
        }

        /// <summary>
        /// Returns person that matches First/Last name or null if none exist
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static Person GetPersonsByName(string firstName, string lastName)
        {
           List<Person> persons = new List<Person>();
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                    persons = insightContext.Persons.Where(x => x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower()).ToList();
                }
            }
            //TODO implement exception
            catch (Exception e)
            {
                throw new Exception("Insight.db access error");
            }
            //TODO implement exception
            if (persons.Count != 1)
            {
                throw new Exception("sdoihdsoighskldjglkghsd");
            }
            //returns person or null if none exist
            return persons.FirstOrDefault();
        }

        /// <summary>
        /// Returns person that matches First, Last, SSN or null if none exist
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="SSN"></param>
        /// <returns></returns>
        public static Person GetPersonsByNameSSN(string firstName, string lastName, string SSN)
        {
            List<Person> persons = new List<Person>();
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                    persons = insightContext.Persons.Where(x => x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower() && x.SSN == SSN).ToList();
                }
            }
            //TODO implement exception
            catch (Exception e)
            {
                throw new Exception("Insight.db access error");
            }
            //TODO implement exception
            //Throws if more than one person is found
            if (persons.Count > 1)
            {
                throw new Exception("sdoihdsoighskldjglkghsd");
            }
            //returns person or null if none exist
            return persons.FirstOrDefault();
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
            //TODO implement exception
            catch (Exception e)
            {
                throw new Exception("Insight.db access error");
            }
        }

        public static void AddMedical(Medical medical, Person person)
        {
            using (InsightContext insightContext = new InsightContext())
            {
                _ = insightContext.Medicals.Add(medical);
                _ = insightContext.Persons.Attach(person);
                _ = insightContext.SaveChanges();
            }
        }

        public static void UpdateEntity<T>(T entity) 
        {
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                   // _ = insightContext.T.Update(e);
                    _ = insightContext.SaveChanges();
                }

            }
            //TODO implement exception
            catch (Exception e)
            {
                throw new Exception("Insight.db access error");
            }
        }

        public static void UpdateMedical(Medical medical)
        {
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                    _ = insightContext.Medicals.Update(medical);
                    _ = insightContext.SaveChanges();
                }

            }
            //TODO implement exception
            catch (Exception e)
            {
                throw new Exception("Insight.db access error");
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

        public static void UpdateTraining(Training training)
        {
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                    _ = insightContext.Trainings.Update(training);
                    _ = insightContext.SaveChanges();
                }

            }
            //TODO implement exception
            catch (Exception e)
            {
                throw new Exception("Insight.db access error");
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

        public static void UpdatePersonnel(Personnel personnel)
        {
            try
            {
                using (InsightContext insightContext = new InsightContext())
                {
                    _ = insightContext.Personnels.Update(personnel);
                    _ = insightContext.SaveChanges();
                }

            }
            //TODO implement exception
            catch (Exception e)
            {
                throw new Exception("Insight.db access error");
            }
        }
    }
}