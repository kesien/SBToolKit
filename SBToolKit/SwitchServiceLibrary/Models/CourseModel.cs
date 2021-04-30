using System;
using System.Collections.Generic;

namespace SwitchServiceLibrary.Models
{
    public class CourseModel
    {
        public string Coursenumber { get; set; }
        public string Language { get; set; }
        public string Level { get; set; }
        public List<ParticipantModel> Participants { get; set; }

        public override string ToString()
        {
            return $"{Coursenumber} - {Language} / {Level}\nParticipants: {Participants.Count}";
        }
    }
}
