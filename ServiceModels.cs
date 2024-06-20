using System.ComponentModel.DataAnnotations;

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

    public class ProgramUpdateRequest: ProgramRequest
    {
        public required string Id { get; set; }
    }

    public class QuestionUpdateRequest: QuestionRequest
    {
        public required string Id { get; set; }
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

    public class ApplicationRequest
    {
        public required string QuestionId { get; set; }
        public string[]? Responses { get; set; }
    }


    public class CandidateRequest
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        [EmailAddress(ErrorMessage = "Not a valid email id")]
        public required string EmailId { get; set; }
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid phone number")]
        public required string Phone { get; set; }
        public required string ProgramId { get; set; }
        public ApplicationRequest[]? Applications { get; set; }
    }

    public class CandidateResponse:Response
    {
        public required string CandidateId { get; set; }
    }

 }
