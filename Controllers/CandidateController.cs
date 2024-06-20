using Microsoft.AspNetCore.Mvc;

namespace CPWorkout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly ILogger<CandidateController> _logger;
        private readonly ICosmosService _cosmosService;
        public CandidateController(ICosmosService cosmosService, ILogger<CandidateController> logger)
        {
            _cosmosService = cosmosService;
            _logger = logger;
        }


        [HttpPost(Name = "Candidate")]
        public async Task<CandidateResponse> Post(CandidateRequest candidateRequest)
        {
            try
            {
                var applicationsArray = Common.GetApplications(candidateRequest);
                var candidate = (new Candidate(Guid.NewGuid().ToString())
                {
                    FirstName = candidateRequest.FirstName,
                    LastName = candidateRequest.LastName,
                    EmailId = candidateRequest.EmailId,
                    Phone  = candidateRequest.Phone,
                    Applications = applicationsArray,
                    ProgramId = candidateRequest.ProgramId,
                });
                var response = await _cosmosService.AddItem<Candidate>(candidate, candidate.Country);
                if (response != null)
                {
                    return new CandidateResponse() { Message = "Success", CandidateId = response.Id.Value, ErrorCode = 0 };
                }
                else
                {
                    return new CandidateResponse() { Message = "Failed", CandidateId = Guid.Empty.ToString(), ErrorCode = 1000 };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CandidateResponse() { Message = "Failed", CandidateId = Guid.Empty.ToString(), ErrorCode = 1000 };
            }
        }
    }
}
