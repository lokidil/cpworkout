﻿namespace CPWorkout
{
    public static class Common
    {
        public static Question[]? GetQuestions(ProgramRequest programRequest)
        {
            if (programRequest.Questions != null && programRequest.Questions.Length > 0)
            {
                var questions = new List<Question>();
                foreach (var quest in programRequest.Questions)
                {
                    questions.Add(new Question(Guid.NewGuid().ToString()) { Title = quest.Title, QuestionType = quest.QuestionType, Options = quest.Options });
                }
                return questions.ToArray();
            }
            return null;
        }

        public static QuestionResponse[]? GetQuestionsResponse(Program program)
        {
            if (program.Questions != null && program.Questions.Length > 0)
            {
                var questions = new List<QuestionResponse>();
                foreach (var quest in program.Questions)
                {
                    questions.Add(new QuestionResponse() { Id = quest.Id, Title = quest.Title, QuestionType = quest.QuestionType, Options = quest.Options });
                }
                return questions.ToArray();
            }
            return null;
        }

        public static Application[]? GetApplications(CandidateRequest candidateRequest)
        {
            if (candidateRequest.Applications != null && candidateRequest.Applications.Length > 0)
            {
                var applications = new List<Application>();
                foreach (var application in candidateRequest.Applications)
                {
                    applications.Add(new Application() { QuestionId = application.QuestionId, Responses = application.Responses });
                }
                return applications.ToArray();
            }
            return null;
        }
    }
}
