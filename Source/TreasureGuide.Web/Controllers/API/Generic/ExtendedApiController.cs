using Microsoft.AspNetCore.Mvc;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class ExtendedApiController<TKey, TStubModel, TDetailModel, TEditorModel> : GenericRestfulApiController<TKey, TEditorModel>
        where TKey : struct
    {
        #region StubEndpoints
        [HttpGet]
        [ActionName("")]
        [Route("{id?}")]
        public IActionResult ActionStub(TKey? id = null)
        {
            return Stub(id);
        }

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
        [ActionName("")]
        [Route("{id?}")]
        public IActionResult ActionDetail(TKey? id = null)
        {
            return Detail(id);
        }

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
        [ActionName("")]
        [Route("{id?}")]
        public IActionResult ActionEditor(TKey? id = null)
        {
            return Editor(id);
        }

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

        protected override IActionResult Get(TKey? id = null)
        {
            return Stub(id);
        }

        private IActionResult Stub(TKey? id = null)
        {
            return Get<TStubModel>(id);
        }

        private IActionResult Detail(TKey? id = null)
        {
            return Get<TDetailModel>(id);
        }

        private IActionResult Editor(TKey? id = null)
        {
            return Get<TEditorModel>(id);
        }

        protected abstract IActionResult Get<TModel>(TKey? id = null);
    }
}
