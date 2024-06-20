# cpworkouts Unit Test


using CPWorkout;
using CPWorkout.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CPWorkoutTest
{
    public class TestFixure
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public IOptions<AppConfig> Config;

        public TestFixure() 
        {
            var serviceCollection = new ServiceCollection();
            ServiceProvider = serviceCollection.BuildServiceProvider();
            serviceCollection.AddScoped<ICosmosService, CosmosService>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.development.json", false)
            .Build();

            Config = Options.Create(configuration.GetSection("AppConfig").Get<AppConfig>());
        }
    }
    
    public class ProgramControllerTest: IClassFixture<TestFixure>
    {
        private ServiceProvider _serviceProvider;
        private ProgramController _programController;
        private ICosmosService _cosmosService;
        private IOptions<AppConfig> _config;

        public ProgramControllerTest(TestFixure testFixure) 
        {
            _serviceProvider = testFixure.ServiceProvider;
            _config = testFixure.Config;
            _cosmosService = new CosmosService(_config);
            _programController = new ProgramController(_cosmosService, default);
        }

        [Fact]
        public async void GetProgramNegativeTest()
        {
            var program = await _programController.Get(new ProgramIdRequest() { Id = "1", Country = "India" });
            Assert.NotNull(program);
            Assert.Equal("Failed", program.Message);
        }

        [Fact]
        public async void PostProgramPositiveTest()
        {
            var program = await _programController.Post(new ProgramRequest() { Title = "Test Title", Description = "Test Desc"});
            Assert.NotNull(program);
            Assert.Equal("Success", program.Message);
            Assert.NotEqual(Guid.Empty.ToString(), program.Id);
        }

        [Fact]
        public async void PutProgramPositiveTest()
        {
            var program = await _programController.Post(new ProgramRequest() { Title = "Test Title", Description = "Test Desc" });
            await _programController.Put(new ProgramUpdateRequest() { Id = program.Id, Title = "Updated title", Description = "Test Desc" });
            var updatedProgram = await _programController.Get(new ProgramIdRequest() { Id = program.Id, Country = "India" });
            Assert.NotNull(updatedProgram);
            Assert.NotNull(updatedProgram.ProgramDetails);
            Assert.Equal("Updated title", updatedProgram.ProgramDetails.Title);
        }

        [Fact]
        public async void DeleteProgramPositiveTest()
        {
            var program = await _programController.Post(new ProgramRequest() { Title = "Test Title", Description = "Test Desc" });
            await _programController.Delete(new ProgramIdRequest() { Id = program.Id, Country = "India"});
            var updatedProgram = await _programController.Get(new ProgramIdRequest() { Id = program.Id, Country = "India" });
            Assert.NotNull(updatedProgram);
            Assert.Equal("Failed", updatedProgram.Message);

        }
    }
}
