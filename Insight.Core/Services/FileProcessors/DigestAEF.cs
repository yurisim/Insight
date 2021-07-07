using System;
using System.Collections.Generic;
using System.Text;
using Insight.Core.Helpers;
using Insight.Core.Models;
using Insight.Core.Services.Database;

namespace Insight.Core.Services.FileProcessors
{
    public class DigestAEF : IDigest
    {
        private readonly IList<string> File = new List<string>();

        public DigestAEF(IList<string> input)
        {
            this.File = input;
        }

        /// <summary>
        /// loops AEF generated string list of lines and processes them
        /// </summary>
        /// <param name="File"></param>
        public void DigestLines()
        {
          for (int i = 3; i < File.Count - 1; i++)
          {
            string[] data = File[i].Split(',');

            //TODO refact to better handle format changes
            //Check variables
            var name = data[0].Split(' ');
            string unit = data[5];
            string AFSC = data[7];

            Person person = Interact.GetPersonByName(firstName: name[1].Trim(), lastName: name[0].Trim());

            //TODO handle user existing in AEF but not in alpha roster
            if (person == null)
            {
              continue;
            }

            //MEDICAL
            if (person.Medical == null)
            {
              Medical medical = new Medical()
              {
                OverallStatus = StringManipulation.StatusReader(data[11])
              };
              person.Medical = medical;
            }
            else //entity already exists
            {
              Medical medical = person.Medical;
              medical.OverallStatus = StringManipulation.StatusReader(data[11]);
            }

            //TRAINING
            if (person.Training == null)
            {
              Training training = new Training()
              {
                OverallStatus = StringManipulation.StatusReader(data[11])
              };
              person.Training = training;
            }
            else //entity already exists
            {
              Training training = person.Training;
              training.OverallStatus = StringManipulation.StatusReader(data[12]);
            }

            //PERSONNEL
            if (person.Personnel == null)
            {
              Personnel personnel = new Personnel()
              {
                OverallStatus = StringManipulation.StatusReader(data[11])
              };
              person.Personnel = personnel;
            }
            else //entity already exists
            {
              Personnel personnel = person.Personnel;
              personnel.OverallStatus = StringManipulation.StatusReader(data[10]);
            }

            Interact.UpdatePerson(person);

          }
        }
    }
}
