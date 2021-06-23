﻿using Insight.Core.Models;
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

        public static Medical GetOrCreate(Person person)
        {

            using (InsightContext insightContext = new InsightContext())
            {
                _ = insightContext.Medicals.Find();
                _ = insightContext.SaveChanges();
            }

            return default;
        }
    }
}