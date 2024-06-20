namespace CPWorkout
{
    public class Response
    {
        public required string Message { get; set; }
        public int ErrorCode { get; set; }
    }

    public class ProgramResponse : Response
    {
        public required string Id { get; set; }
    }

    public class QuestionRequest
    {
        public required string Title { get; set; }
        public required string QuestionType { get; set; }
        public string[]? Options { get; set; }
    }

    public class ProgramRequest
    {
        public required string Title { get; set; }
        public required string Description { get; set; }

        public QuestionRequest[]? Questions { get; set; }
    }

    public class ProgramIdRequest
    {
        public required string Id { get; set; }

        public required string Country { get; set; } = "India";
    }

    public class QuestionResponse
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string QuestionType { get; set; }
        public string[]? Options { get; set; }
    }

    public class ProgramDetails
    {
        public required string Title { get; set; }
        public required string Description { get; set; }

        public QuestionResponse[]? Questions { get; set; }
    }

    public class ProgramDetailsResponse : Response
    {
        public ProgramDetails? ProgramDetails { get; set; }

    }

}
