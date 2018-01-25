using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Entities
{
    public partial class Unit : IIdItem<int> { }

    public partial class Team : IIdItem<int> { }

    public partial class TeamVideo : IIdItem<int> { }

    public partial class Stage : IIdItem<int> { }

    public partial class Ship : IIdItem<int> { }

    public partial class Round : IIdItem<int> { }

    public partial class UserProfile : IIdItem<string> { }

    public partial class TeamUnit : ISubItem { }

    public partial class TeamGenericSlot : ISubItem { }
}
