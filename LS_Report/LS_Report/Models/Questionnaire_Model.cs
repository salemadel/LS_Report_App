using System.Collections.Generic;

namespace LS_Report.Models
{
    public class Questionnaire_Model
    {
        public List<Question> questions { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string _id { get; set; }

        public Questionnaire_Model()
        {
            questions = new List<Question>();
        }
    }

    public class Question
    {
        public List<string> answers { get; set; }
        public string question { get; set; }
        public string _id { get; set; }

        public Question()
        {
            answers = new List<string>();
        }
    }

    public class Questionnaire_Responce_Model
    {
        public string client { get; set; }
        public string note { get; set; }
        public string questionnaire { get; set; }

        public class Questions
        {
            public string question { get; set; }
            public string answer { get; set; }
        }

        public List<Questions> questions { get; set; }

        public Questionnaire_Responce_Model()
        {
            questions = new List<Questions>();
        }
    }
}