using Microsoft.AspNetCore.Mvc;

namespace CPWorkout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProgramController : ControllerBase
    {
        private readonly ILogger<ProgramController> _logger;
        private readonly ICosmosService _cosmosService;
        public ProgramController(ICosmosService cosmosService, ILogger<ProgramController> logger)
        {
            _cosmosService = cosmosService;
            _logger = logger;
        }

        [HttpGet(Name = "Program")]
        public async Task<ProgramDetailsResponse> Get([FromQuery] ProgramIdRequest programIdRequest)
        {
            try
            {
                var program = await _cosmosService.GetItem<Program>(programIdRequest.Id, programIdRequest.Country);
                if (program == null)
                {
                    return new ProgramDetailsResponse() { ErrorCode = 1000, Message = "Failed" };
                }
                var questions = program.Questions ?? null;
                var programDetails = new ProgramDetails()
                {
                    Description = program.Description,
                    Title = program.Title
                };
                programDetails.Questions = Common.GetQuestionsResponse(program);
                return new ProgramDetailsResponse() { Message = "Success", ErrorCode = 0, ProgramDetails = programDetails };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ProgramDetailsResponse() { ErrorCode = 1000, Message = "Failed" };
            }
        }

        [HttpPost(Name = "Program")]
        public async Task<ProgramResponse> Post(ProgramRequest programRequest)
        {
            try
            {
                var questionsArray = Common.GetQuestions(programRequest);
                var program = (new Program(Guid.NewGuid().ToString())
                {
                    Title = programRequest.Title,
                    Description = programRequest.Description,
                    Questions = questionsArray
                });
                var response = await _cosmosService.AddItem<Program>(program, program.Country);
                if (response != null)
                {
                    return new ProgramResponse() { Message = "Success", Id = response.Id.Value, ErrorCode = 0 };
                }
                else
                {
                    return new ProgramResponse() { Message = "Failed", Id = Guid.Empty.ToString(), ErrorCode = 1000 };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ProgramResponse() { Message = "Failed", Id = Guid.Empty.ToString(), ErrorCode = 1000 };
            }
        }

        [HttpPut(Name = "Program")]
        public async Task<ProgramResponse> Put(ProgramUpdateRequest programUpdateRequest)
        {
            try
            {
                var questionsArray = Common.GetQuestions(programUpdateRequest);
                var program = (new Program(programUpdateRequest.Id)
                {
                    Title = programUpdateRequest.Title,
                    Description = programUpdateRequest.Description,
                    Questions = questionsArray
                });
                var response = await _cosmosService.UpdateItem<Program>(program, program.Country);
                if (response != null)
                {
                    return new ProgramResponse() { Message = "Success", Id = response.Id.Value, ErrorCode = 0 };
                }
                else
                {
                    return new ProgramResponse() { Message = "Failed", Id = Guid.Empty.ToString(), ErrorCode = 1000 };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ProgramResponse() { Message = "Failed", Id = Guid.Empty.ToString(), ErrorCode = 1000 };
            }
        }

        [HttpDelete(Name = "Program")]
        public async Task<Response> Delete([FromQuery] ProgramIdRequest programIdRequest)
        {
            try
            {
                var program = await _cosmosService.DeleteItem<Program>(programIdRequest.Id, programIdRequest.Country);
                if (program == null)
                {
                    return new Response() { ErrorCode = 1000, Message = "Failed" };
                }
                return new Response() { Message = "Success", ErrorCode = 0 };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response() { ErrorCode = 1000, Message = "Failed" };
            }
        }
    }
}
