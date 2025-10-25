using ErrorOr;
using Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Extensions.AspNetCore
{
    public class ControllerBaseExtensionsTests
    {
        private class TestController : ControllerBase
        {
        }

        [Fact]
        public void ConvertToActionResult_ReturnsOkObjectResult_WhenValue()
        {
            var controller = new TestController();
            var result = Success(42);

            var actionResult = controller.ConvertToActionResult(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(42, okResult.Value);
        }

        [Fact]
        public void ConvertToActionResult_ReturnsProblemDetails_WhenError()
        {
            var controller = new TestController();
            var result = Failure<int>("ERR01", "Something went wrong");

            var actionResult = controller.ConvertToActionResult(result);

            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal("ERR01", problem.Extensions["code"]);
            Assert.Equal(400, problem.Status);
        }

        [Fact]
        public void ConvertToActionResultWithoutData_ReturnsOk_WhenValue()
        {
            var controller = new TestController();
            var result = Success("ok");

            var actionResult = controller.ConvertToActionResultWithoutData(result);

            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public void ConvertToActionResultWithoutData_ReturnsProblem_WhenError()
        {
            var controller = new TestController();
            var result = Failure<string>();

            var actionResult = controller.ConvertToActionResultWithoutData(result);

            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal("ERR", problem.Extensions["code"]);
        }

        [Fact]
        public void ConvertToActionResult_WithMapping_ReturnsMappedValue()
        {
            var controller = new TestController();
            var result = Success(5);

            var actionResult = controller.ConvertToActionResult<int, string>(result, v => $"Number {v}");

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal("Number 5", okResult.Value);
        }

        [Fact]
        public async Task ConvertToActionResultAsync_ReturnsOk_WhenValue()
        {
            var controller = new TestController();
            var result = Success(10);

            var actionResult = await controller.ConvertToActionResultAsync<int, string>(result, async v =>
            {
                await Task.Delay(1);
                return new OkObjectResult($"Val {v}");
            });

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal("Val 10", okResult.Value);
        }

        [Fact]
        public async Task ConvertToActionResultAsync_ReturnsProblem_WhenError()
        {
            var controller = new TestController();
            var result = Failure<int>();

            var actionResult = await controller.ConvertToActionResultAsync<int, string>(result, async v =>
            {
                await Task.Delay(1);
                return new OkObjectResult(v.ToString());
            });

            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
            Assert.Equal("ERR", problem.Extensions["code"]);
        }

        [Fact]
        public void ConvertToIActionResult_ReturnsOk_WhenValue()
        {
            var controller = new TestController();
            var result = Success("hello");

            var actionResult = controller.ConvertToIActionResult(result, v => v.ToUpper());

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal("HELLO", okResult.Value);
        }

        [Fact]
        public async Task ConvertToIActionResultAsync_ReturnsOk_WhenValue()
        {
            var controller = new TestController();
            var result = Success(7);

            var actionResult = await controller.ConvertToIActionResultAsync<int, int>(result, async v =>
            {
                await Task.Delay(1);
                return v * 2;
            });

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(14, okResult.Value);
        }

        private static ErrorOr<T> Success<T>(T value)
        {
            var ctor = typeof(ErrorOr<T>).GetConstructor(
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new Type[] { typeof(T) },
                null);

            if (ctor == null)
            {
                throw new InvalidOperationException("Private constructor for ErrorOr<T> not found.");
            }

            return (ErrorOr<T>)ctor.Invoke(new object[] { value! });
        }

        private static ErrorOr<T> Failure<T>(string code = "ERR", string description = "Error") =>
            ErrorOr<T>.From(new List<Error> { Error.Validation(code, description) });
    }
}
