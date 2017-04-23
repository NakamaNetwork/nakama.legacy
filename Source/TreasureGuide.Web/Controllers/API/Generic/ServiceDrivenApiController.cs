using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Web.Services.API.Generic;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public class ServiceDrivenApiController<TKey, TStubModel, TDetailModel, TEditorModel> : ExtendedApiController<TKey, TStubModel, TDetailModel, TEditorModel>
        where TKey : struct
    {
        protected readonly IDataService<TKey> DataService;

        public ServiceDrivenApiController(IDataService<TKey> dataService)
        {
            DataService = dataService;
        }

        protected override IActionResult Get<TModel>(TKey? id = null)
        {
            var result = DataService.Get<TModel>(id);
            return GetResult(result);
        }

        protected override IActionResult Post(TEditorModel model, TKey? id = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = DataService.Post<TEditorModel>(model, id);
            return GetResult(result);
        }

        protected override IActionResult Delete(TKey? id = null)
        {
            var result = DataService.Delete(id);
            return GetResult(result);
        }

        protected virtual IActionResult GetResult<TResult>(DataResult<TResult> result)
        {
            switch (result.ErrorType)
            {
                case ErrorType.NotFound:
                    return NotFound(result.ErrorData);
                case ErrorType.NoContent:
                    return NoContent();
                case ErrorType.BadRequest:
                    return BadRequest(result.ErrorData);
                case ErrorType.Forbidden:
                    return Forbid();
                default:
                    return Ok(result.Result);
            }
        }
    }
}
