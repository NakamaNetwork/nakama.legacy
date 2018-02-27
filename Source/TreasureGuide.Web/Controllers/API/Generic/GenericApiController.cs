using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class GenericApiController<TKey, TStubModel, TDetailModel, TEditorModel> : Controller
        where TEditorModel : IIdItem<TKey>
    {
        #region GetEndpoints
        [HttpGet]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> ActionGet(TKey id = default(TKey))
        {
            return await Get(id);
        }

        [HttpGet]
        [ActionName("Get")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdGet(TKey id = default(TKey))
        {
            return await Get(id);
        }

        [HttpGet]
        [ActionName("Get")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionGetId(TKey id = default(TKey))
        {
            return await Get(id);
        }
        #endregion

        #region StubEndpoints
        [HttpGet]
        [ActionName("Stub")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdStub(TKey id = default(TKey))
        {
            return await Stub(id);
        }

        [HttpGet]
        [ActionName("Stub")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionStubId(TKey id = default(TKey))
        {
            return await Stub(id);
        }
        #endregion

        #region DetailEndpoints
        [HttpGet]
        [ActionName("Detail")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdDetail(TKey id = default(TKey))
        {
            return await Detail(id);
        }

        [HttpGet]
        [ActionName("Detail")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionDetailId(TKey id = default(TKey))
        {
            return await Detail(id);
        }
        #endregion

        #region EditorEndpoints
        [HttpGet]
        [ActionName("Editor")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdEditor(TKey id = default(TKey))
        {
            return await Editor(id);
        }

        [HttpGet]
        [ActionName("Editor")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionEditorId(TKey id = default(TKey))
        {
            return await Editor(id);
        }
        #endregion

        #region PostEndpoints
        [HttpPost]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> ActionPost([FromBody]TEditorModel model, TKey id = default(TKey))
        {
            return await Post(model, id);
        }

        [HttpPost]
        [ActionName("Post")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdPost([FromBody]TEditorModel model, TKey id = default(TKey))
        {
            return await Post(model, id);
        }

        [HttpPost]
        [ActionName("Post")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionPostId([FromBody]TEditorModel model, TKey id = default(TKey))
        {
            return await Post(model, id);
        }
        #endregion

        #region PutEndpoints
        [HttpPut]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> ActionPut([FromBody]TEditorModel model, TKey id = default(TKey))
        {
            return await Put(model, id);
        }

        [HttpPut]
        [ActionName("Put")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdPut([FromBody]TEditorModel model, TKey id = default(TKey))
        {
            return await Put(model, id);
        }

        [HttpPut]
        [ActionName("Put")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionPutId([FromBody]TEditorModel model, TKey id = default(TKey))
        {
            return await Put(model, id);
        }
        #endregion

        #region DeleteEndpoints
        [HttpDelete]
        [ActionName("")]
        [Route("{id?}")]
        public async Task<IActionResult> ActionDelete(TKey id = default(TKey))
        {
            return await Delete(id);
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Route("{id}/[action]")]
        public async Task<IActionResult> ActionIdDelete(TKey id = default(TKey))
        {
            return await Delete(id);
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> ActionDeleteId(TKey id = default(TKey))
        {
            return await Delete(id);
        }
        #endregion

        protected virtual async Task<IActionResult> Get(TKey id = default(TKey))
        {
            return await Stub(id);
        }

        protected virtual async Task<IActionResult> Stub(TKey id = default(TKey))
        {
            return await Get<TStubModel>(id, true);
        }

        protected virtual async Task<IActionResult> Detail(TKey id = default(TKey))
        {
            return await Get<TDetailModel>(id, true);
        }

        protected virtual async Task<IActionResult> Editor(TKey id = default(TKey))
        {
            return await Get<TEditorModel>(id, true);
        }

        protected virtual async Task<IActionResult> Put(TEditorModel model, TKey id = default(TKey))
        {
            return await Post(model, id);
        }

        protected abstract Task<IActionResult> Get<TModel>(TKey id = default(TKey), bool required = false);
        protected abstract Task<IActionResult> Post(TEditorModel model, TKey id = default(TKey));
        protected abstract Task<IActionResult> Delete(TKey id = default(TKey));

        protected static bool IsUnspecified(TKey id = default(TKey))
        {
            return EqualityComparer<TKey>.Default.Equals(id, default(TKey));
        }

        protected static TKey DefaultIfUnspecified(TKey value, TKey def = default(TKey))
        {
            return IsUnspecified(value) ? def : value;
        }
    }
}
