using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    [Route("api/[controller]")]
    public abstract class GenericApiController<TKey, TEntity, TStubModel, TDetailModel, TEditorModel> : Controller
        where TKey : struct
        where TEntity : IIdItem<TKey>
        where TEditorModel : IIdItem<TKey?>
    {
        #region GetEndpoints
        [HttpGet]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> ActionGet(TKey? id = null)
        {
            return await Get(id);
        }

        [HttpGet]
        [ActionName("Get")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdGet(TKey? id = null)
        {
            return await Get(id);
        }

        [HttpGet]
        [ActionName("Get")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionGetId(TKey? id = null)
        {
            return await Get(id);
        }
        #endregion

        #region StubEndpoints
        [HttpGet]
        [ActionName("Stub")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdStub(TKey? id = null)
        {
            return await Stub(id);
        }

        [HttpGet]
        [ActionName("Stub")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionStubId(TKey? id = null)
        {
            return await Stub(id);
        }
        #endregion

        #region DetailEndpoints
        [HttpGet]
        [ActionName("Detail")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdDetail(TKey? id = null)
        {
            return await Detail(id);
        }

        [HttpGet]
        [ActionName("Detail")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionDetailId(TKey? id = null)
        {
            return await Detail(id);
        }
        #endregion

        #region EditorEndpoints
        [HttpGet]
        [ActionName("Editor")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdEditor(TKey? id = null)
        {
            return await Editor(id);
        }

        [HttpGet]
        [ActionName("Editor")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionEditorId(TKey? id = null)
        {
            return await Editor(id);
        }
        #endregion

        #region PostEndpoints
        [HttpPost]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> ActionPost([FromBody]TEditorModel model, TKey? id = null)
        {
            return await Post(model, id);
        }

        [HttpPost]
        [ActionName("Post")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdPost([FromBody]TEditorModel model, TKey? id = null)
        {
            return await Post(model, id);
        }

        [HttpPost]
        [ActionName("Post")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionPostId([FromBody]TEditorModel model, TKey? id = null)
        {
            return await Post(model, id);
        }
        #endregion

        #region PutEndpoints
        [HttpPut]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> ActionPut([FromBody]TEditorModel model, TKey? id = null)
        {
            return await Put(model, id);
        }

        [HttpPut]
        [ActionName("Put")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdPut([FromBody]TEditorModel model, TKey? id = null)
        {
            return await Put(model, id);
        }

        [HttpPut]
        [ActionName("Put")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionPutId([FromBody]TEditorModel model, TKey? id = null)
        {
            return await Put(model, id);
        }
        #endregion

        #region DeleteEndpoints
        [HttpDelete]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> ActionDelete(TKey? id = null)
        {
            return await Delete(id);
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdDelete(TKey? id = null)
        {
            return await Delete(id);
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionDeleteId(TKey? id = null)
        {
            return await Delete(id);
        }
        #endregion

        protected virtual async Task<IActionResult> Get(TKey? id = null)
        {
            return await Stub(id);
        }

        protected virtual async Task<IActionResult> Stub(TKey? id = null)
        {
            return await Get<TStubModel>(id);
        }

        protected virtual async Task<IActionResult> Detail(TKey? id = null)
        {
            if (!id.HasValue)
            {
                return BadRequest("Must specify an Id.");
            }
            return await Get<TDetailModel>(id);
        }

        protected virtual async Task<IActionResult> Editor(TKey? id = null)
        {
            if (!id.HasValue)
            {
                return BadRequest("Must specify an Id.");
            }
            return await Get<TEditorModel>(id);
        }

        protected virtual async Task<IActionResult> Put(TEditorModel model, TKey? id = null)
        {
            return await Post(model, id);
        }

        protected abstract Task<IActionResult> Get<TModel>(TKey? id = null);
        protected abstract Task<IActionResult> Post(TEditorModel model, TKey? id = null);
        protected abstract Task<IActionResult> Delete(TKey? id = null);
    }
}
