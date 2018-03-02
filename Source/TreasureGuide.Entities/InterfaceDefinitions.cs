using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Entities
{
    public partial class Unit : IIdItem<int>, IEditedDateItem { }

    public partial class UnitAlias : IEditedDateItem { }

    public partial class UnitEvolution : IEditedDateItem { }

    public partial class Team : IIdItem<int>, IEditedDateItem { }

    public partial class TeamVideo : IIdItem<int> { }

    public partial class Stage : IIdItem<int>, IEditedDateItem { }

    public partial class StageAlias : IEditedDateItem { }

    public partial class Ship : IIdItem<int>, IEditedDateItem { }

    public partial class UserProfile : IIdItem<string> { }

    public partial class Box : IIdItem<int> { }

    public partial class TeamUnit : ISubItem { }

    public partial class TeamGenericSlot : ISubItem { }

    public partial class Donation : IIdItem<int> { }
}
