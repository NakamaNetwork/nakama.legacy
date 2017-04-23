using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.UnitModels;
using TreasureGuide.Web.Services.API.Generic;

namespace TreasureGuide.Web.Controllers.API
{
    public class UnitController : EntityApiController<int, Unit, UnitStubModel, UnitDetailModel, UnitDetailModel>
    {
        public UnitController(IDataService<int, Unit> dataService) : base(dataService)
        {
        }
    }
}
