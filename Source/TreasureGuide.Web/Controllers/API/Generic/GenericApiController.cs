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
        public IActionResult ActionGet(TKey? id = null)
        {
            return Get(id);
        }

        [HttpGet]
        [ActionName("Get")]
        [Route("{id}/[action]")]
        public IActionResult ActionIdGet(TKey? id = null)
        {
            return Get(id);
        }

        [HttpGet]
        [ActionName("Get")]
        [Route("[action]/{id?}")]
        public IActionResult ActionGetId(TKey? id = null)
        {
            return Get(id);
        }
        #endregion

        #region StubEndpoints
        [HttpGet]
        [ActionName("Stub")]
        [Route("{id}/[action]")]
        public IActionResult ActionIdStub(TKey? id = null)
        {
            return Stub(id);
        }

        [HttpGet]
        [ActionName("Stub")]
        [Route("[action]/{id?}")]
        public IActionResult ActionStubId(TKey? id = null)
        {
            return Stub(id);
        }
        #endregion

        #region DetailEndpoints
        [HttpGet]
        [ActionName("Detail")]
        [Route("{id}/[action]")]
        public IActionResult ActionIdDetail(TKey? id = null)
        {
            return Detail(id);
        }

        [HttpGet]
        [ActionName("Detail")]
        [Route("[action]/{id?}")]
        public IActionResult ActionDetailId(TKey? id = null)
        {
            return Detail(id);
        }
        #endregion

        #region EditorEndpoints
        [HttpGet]
        [ActionName("Editor")]
        [Route("{id}/[action]")]
        public IActionResult ActionIdEditor(TKey? id = null)
        {
            return Editor(id);
        }

        [HttpGet]
        [ActionName("Editor")]
        [Route("[action]/{id?}")]
        public IActionResult ActionEditorId(TKey? id = null)
        {
            return Editor(id);
        }
        #endregion

        #region PostEndpoints
        [HttpPost]
        [ActionName("")]
        [Route("{id?}")]
        public IActionResult ActionPost([FromBody]TEditorModel model, TKey? id = null)
        {
            return Post(model, id);
        }

        [HttpPost]
        [ActionName("Post")]
        [Route("{id}/[action]")]
        public IActionResult ActionIdPost([FromBody]TEditorModel model, TKey? id = null)
        {
            return Post(model, id);
        }

        [HttpPost]
        [ActionName("Post")]
        [Route("[action]/{id?}")]
        public IActionResult ActionPostId([FromBody]TEditorModel model, TKey? id = null)
        {
            return Post(model, id);
        }
        #endregion

        #region PutEndpoints
        [HttpPut]
        [ActionName("")]
        [Route("{id?}")]
        public IActionResult ActionPut([FromBody]TEditorModel model, TKey? id = null)
        {
            return Put(model, id);
        }

        [HttpPut]
        [ActionName("Put")]
        [Route("{id}/[action]")]
        public IActionResult ActionIdPut([FromBody]TEditorModel model, TKey? id = null)
        {
            return Put(model, id);
        }

        [HttpPut]
        [ActionName("Put")]
        [Route("[action]/{id?}")]
        public IActionResult ActionPutId([FromBody]TEditorModel model, TKey? id = null)
        {
            return Put(model, id);
        }
        #endregion

        #region DeleteEndpoints
        [HttpDelete]
        [ActionName("")]
        [Route("{id?}")]
        public IActionResult ActionDelete(TKey? id = null)
        {
            return Delete(id);
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Route("{id}/[action]")]
        public IActionResult ActionIdDelete(TKey? id = null)
        {
            return Delete(id);
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Route("[action]/{id?}")]
        public IActionResult ActionDeleteId(TKey? id = null)
        {
            return Delete(id);
        }
        #endregion

        protected virtual IActionResult Get(TKey? id = null)
        {
            return Stub(id);
        }

        protected virtual IActionResult Stub(TKey? id = null)
        {
            return Get<TStubModel>(id);
        }

        protected virtual IActionResult Detail(TKey? id = null)
        {
            if (!id.HasValue)
            {
                return BadRequest("Must specify an Id.");
            }
            return Get<TDetailModel>(id);
        }

        protected virtual IActionResult Editor(TKey? id = null)
        {
            if (!id.HasValue)
            {
                return BadRequest("Must specify an Id.");
            }
            return Get<TEditorModel>(id);
        }

        protected virtual IActionResult Put(TEditorModel model, TKey? id = null)
        {
            return Post(model, id);
        }

        protected abstract IActionResult Get<TModel>(TKey? id = null);
        protected abstract IActionResult Post(TEditorModel model, TKey? id = null);
        protected abstract IActionResult Delete(TKey? id = null);
    }
}
