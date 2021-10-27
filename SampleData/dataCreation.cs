using Jayride_Challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jayride_Challenge.SampleData
{
    public class dataCreation
    {
        //Creating a sample Candidate Test of Models/Candidate Type
        public Candidate CandidateTest()
        {
            return 
                new Candidate
                {
                    name = "test",
                    phone = "test"
                };
        }
    }
}
