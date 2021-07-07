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
        _ = insightContext.Trainings.Add(person.Training);
        _ = insightContext.Medicals.Add(person.Medical);
        _ = insightContext.Personnels.Add(person.Personnel);

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

          foreach (Person person in persons)
          {
            person.Medical = GetMedicalByPersonId(person, insightContext);
            person.Personnel = GetPersonnelByPersonId(person, insightContext);
            person.Training = GetTrainingByPersonId(person, insightContext);
          }
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
          //TODO implement better exceptions
          if (persons.Count > 1)
          {
            throw new Exception("Too many Persons found, should be null or 1");
          }

          Person person = persons.FirstOrDefault();

          if (person != null)
          {
            person.Medical = GetMedicalByPersonId(person, insightContext);
            person.Personnel = GetPersonnelByPersonId(person, insightContext);
            person.Training = GetTrainingByPersonId(person, insightContext);
          }
        }

      }
      //TODO implement exception
      catch (Exception e)
      {
        throw new Exception("Insight.db access error");
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
      //TODO refactor to reuse code more and have better methods
      List<Person> persons = new List<Person>();
      try
      {
        using (InsightContext insightContext = new InsightContext())
        {
          persons = insightContext.Persons.Where(x => x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower() && x.SSN == SSN).ToList();
          //TODO implement better exceptions
          if (persons.Count > 1)
          {
            throw new Exception("Too many Persons found, should be null or 1");
          }

          Person person = persons.FirstOrDefault();

          if (person != null)
          {
            person.Medical = GetMedicalByPersonId(person, insightContext);
            person.Personnel = GetPersonnelByPersonId(person, insightContext);
            person.Training = GetTrainingByPersonId(person, insightContext);
          }
        }

      }
      //TODO implement exception
      catch (Exception e)
      {
        throw new Exception("Insight.db access error");
      }
      //returns person or null if none exist
      return persons.FirstOrDefault();
    }

    public static async void UpdatePerson(Person person)
    {
      try
      {
        using (InsightContext insightContext = new InsightContext())
        {
          _ = insightContext.Persons.Update(person);
          _ = insightContext.Trainings.Update(person.Training);
          _ = insightContext.Medicals.Update(person.Medical);
          _ = insightContext.Personnels.Update(person.Personnel);
          _ = await insightContext.SaveChangesAsync();
        }

      }
      //TODO implement exception
      catch (Exception e)
      {
        throw new Exception("Insight.db access error");
      }
    }

    private static Medical GetMedicalByPersonId(Person person, InsightContext insightContext)
    {
      if (insightContext != null)
        return insightContext.Medicals.Where(x => x.PersonId == person.PersonId).ToList().FirstOrDefault();

      return null;
    }

    private static Training GetTrainingByPersonId(Person person, InsightContext insightContext)
    {
      if (insightContext != null)
        return insightContext.Trainings.Where(x => x.PersonId == person.PersonId).ToList().FirstOrDefault();

      return null;
    }

    private static Personnel GetPersonnelByPersonId(Person person, InsightContext insightContext)
    {
      if (insightContext != null)
        return insightContext.Personnels.Where(x => x.PersonId == person.PersonId).ToList().FirstOrDefault();

      return null;
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