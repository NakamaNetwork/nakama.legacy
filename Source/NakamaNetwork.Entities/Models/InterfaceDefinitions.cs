using NakamaNetwork.Entities.Interfaces;

namespace NakamaNetwork.Entities.Models
{
    public partial class Unit : IIdItem<int> { }

    public partial class UnitAlias { }

    public partial class UnitEvolution { }

    public partial class Team : IIdItem<int>, IEditedDateItem { }

    public partial class TeamComment : IIdItem<int>, IEditedDateItem { }

    public partial class TeamVideo : IIdItem<int> { }

    public partial class Stage : IIdItem<int> { }

    public partial class StageAlias { }

    public partial class Ship : IIdItem<int> { }

    public partial class UserProfile : IIdItem<string> { }

    public partial class Box : IIdItem<int> { }

    public partial class TeamUnit : ISubItem { }

    public partial class TeamGenericSlot : ISubItem { }

    public partial class Donation : IIdItem<int> { }

    public partial class CacheSet : IEditedDateItem { }
}
