using Microsoft.AspNetCore.Mvc;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class GenericRestfulApiController<TKey, TEditorModel> : Controller
        where TKey : struct
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

        #region PostEndpoints
        [HttpGet]
        [ActionName("")]
        [Route("{id?}")]
        public IActionResult ActionPost(TEditorModel model, TKey? id = null)
        {
            return Post(model, id);
        }

        [HttpGet]
        [ActionName("Post")]
        [Route("{id}/[action]")]
        public IActionResult ActionIdPost(TEditorModel model, TKey? id = null)
        {
            return Post(model, id);
        }

        [HttpGet]
        [ActionName("Post")]
        [Route("[action]/{id?}")]
        public IActionResult ActionPostId(TEditorModel model, TKey? id = null)
        {
            return Post(model, id);
        }
        #endregion

        #region PutEndpoints
        [HttpGet]
        [ActionName("")]
        [Route("{id?}")]
        public IActionResult ActionPut(TEditorModel model, TKey? id = null)
        {
            return Put(model, id);
        }

        [HttpGet]
        [ActionName("Put")]
        [Route("{id}/[action]")]
        public IActionResult ActionIdPut(TEditorModel model, TKey? id = null)
        {
            return Put(model, id);
        }

        [HttpGet]
        [ActionName("Put")]
        [Route("[action]/{id?}")]
        public IActionResult ActionPutId(TEditorModel model, TKey? id = null)
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

        protected abstract IActionResult Get(TKey? id = null);
        protected abstract IActionResult Post(TEditorModel model, TKey? id = null);

        protected virtual IActionResult Put(TEditorModel model, TKey? id = null)
        {
            return Post(model, id);
        }

        protected abstract IActionResult Delete(TKey? id = null);
    }
}
