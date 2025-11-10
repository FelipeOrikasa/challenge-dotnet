using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using System.IO;

namespace Mottu.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MlController : ControllerBase
    {
        private readonly string _modelPath;
        private readonly MLContext _ml;

        public MlController()
        {
            _ml = new MLContext();
            _modelPath = Path.Combine(AppContext.BaseDirectory, "ml_model.zip");
        }

        [HttpGet("predict")]
        public IActionResult Predict([FromQuery] float feature1 = 0, [FromQuery] float feature2 = 0)
        {
            if (!System.IO.File.Exists(_modelPath))
                return BadRequest("Model not found on server.");

            var input = new ModelInput { Feature1 = feature1, Feature2 = feature2 };
            ITransformer mlModel;
            using (var stream = System.IO.File.OpenRead(_modelPath))
            {
                mlModel = _ml.Model.Load(stream, out _);
            }

            var predEngine = _ml.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            var prediction = predEngine.Predict(input);
            return Ok(new { Score = prediction.Score });
        }

        public class ModelInput
        {
            public float Feature1 { get; set; }
            public float Feature2 { get; set; }
        }

        public class ModelOutput
        {
            public float Score { get; set; }
        }
    }
}
