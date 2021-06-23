﻿using System;
using System.Collections.Generic;
using System.Text;
using Insight.Core.Models;

namespace Insight.Core.Services
{
    class AEFDigest
    {
        public List<AEF> processedData = new List<AEF>();
        
        /// <summary>
        /// loops AEF generated string list of lines and processes them
        /// </summary>
        /// <param name="File"></param>
        public void Digest(List<string> File)
        {
            for (int i = 0; i < File.Count; i++)
            {
                if (i > 3)
                {

                }
                else if (i == File.Count - 1)
                {

                }
                else
                {
                    string[] data = File[i].Split(',');
                    //processedData.Add(new AEF(data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11], data[12], data[13], data[14], data[15], data[16]));
                    AEF newAEF = new AEF();
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
                    newAEF.ModeTip = data[16];
                }
            }
        }
    }
}
