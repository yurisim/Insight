using System;
using System.Collections.Generic;
using System.Text;
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
                    //processedData.Add(new AEF(data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11], data[12], data[13], data[14], data[15], data[16]));
                    /*AEF newAEF = new AEF();
                    newAEF.Name = data[0];
                    newAEF.CourseCount = data[1];
                    newAEF.DRCStatusForEmail = data[2];
                    newAEF.PayGrade = data[3];
                    newAEF.AEFI = data[4];
                    newAEF.Unit = data[5];
                    newAEF.PASCode = data[6];
                    newAEF.AFSC = data[7];
                    newAEF.Gender = data[8];
                    newAEF.DutyStatus = data[9];
                    newAEF.Personnel = data[10];
                    newAEF.Medical = data[11];
                    newAEF.Training = data[12];
                    newAEF.HasAEFAccount = data[13];
                    newAEF.VisitedAEFOnline = data[14];
                    newAEF.ChecklistStatus = data[15];
                    newAEF.ModeTip = data[16];*/

                    //Check variables
                    var name = data[0].Split(' ');
                    string unit = data[5];
                    string AFSC = data[7];

                    Person person = Interact.GetPersonsByName(firstName: name[1].Trim(), lastName: name[0].Trim());

                    //TODO handle user existing in AEF but not in alpha roster
                    if (person == null)
                    {
                      continue;
                    }

                    //MEDICAL
                    if (person.Medical == null)
                    {
                        Medical medical = new Medical() {
                            OverallStatus = StatusReader(data[11])/*, Person = person*/ };
                        person.Medical = medical;
                    }
                    else //entity already exists
                    {
                        Medical medical = person.Medical;
                        medical.OverallStatus = StatusReader(data[11]);
                        //Interact.UpdateMedical(medical);
                    }

                    //TRAINING
                    if (person.Training == null)
                    {
                        Training training = new Training()
                        {
                            OverallStatus = StatusReader(data[11])/*, Person = person */
                        };
                        person.Training = training;
                    }
                    else //entity already exists
                    {
                        Training training = person.Training;
                        training.OverallStatus = StatusReader(data[12]);
                        //Interact.UpdateTraining(training);
                    }

                    //PERSONNEL
                    if (person.Personnel == null)
                    {
                        Personnel personnel = new Personnel()
                        {
                            OverallStatus = StatusReader(data[11])/*, Person = person */
                        };
                        person.Personnel = personnel;
                    }
                    else //entity already exists
                    {
                        Personnel personnel = person.Personnel;
                        personnel.OverallStatus = StatusReader(data[10]);
                        //Interact.UpdatePersonnel(personnel);
                    }

                    Interact.UpdatePerson(person);
                
            }
        }

        //TODO move to helper
        private Status StatusReader(string input)
        {
	        Status status;
	        switch (input.ToLower())
	        {
                case "g":
	                status = Status.Current;
                    break;
                case "y": 
	                status = Status.Upcoming;
	                break;
                case "r":
	                status = Status.Overdue;
	                break;
                default:
                  status = Status.Unknown;
                  break;
	        }
            return status;
        }
    }
}
