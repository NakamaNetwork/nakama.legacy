#API Controllers

These are the API controllers on Nakama Network:

```
/api/unit
/api/stage
/api/team
/api/box
/api/profile
/api/ship
```

## Endpoints

These are the endpoints they implement:

```
/api/[controller]/[id]/get
/api/[controller]/[id]/stub
/api/[controller]/[id]/detail
/api/[controller]/search
```

Get and Stub are the same endpoint.

### Searching 

All search endpoint can take in a series of query strings dictated by the following interfaces:

```
export interface ISearchModel {
    page: number;
    pageSize: number;
    sortBy: string;
    sortDesc: boolean;    
}
```

#### Units

```
export interface IUnitSearchModel extends ISearchModel{
    term: string;
    classes: UnitClass;
    types: UnitType;
    forceClass: boolean;
    freeToPlay: boolean;
    global: boolean;
    boxId: number;
    blacklist: boolean;
}
```

#### Teams

```
export interface ITeamSearchModel extends ISearchModel{
    term: string;
    submittedBy: string;
    leaderId: number;
    noHelp: boolean;
    stageId: number;
    boxId: number;
    blacklist: boolean;
    global: boolean;
    freeToPlay: FreeToPlayStatus;
    classes: UnitClass;
    types: UnitType;
    deleted: boolean;
    draft: boolean;
    reported: boolean;
    bookmark: boolean;  
}
```

#### Stages

```
export interface IStageSearchModel extends ISearchModel{
    term: string;
    type: StageType;
    global: boolean;
}
```

#### Profiles

```
export interface IProfileSearchModel extends ISearchModel{
    term: string;
    roles: string[];  
}
```

#### Boxes

```
export interface IBoxSearchModel extends ISearchModel{
    userId: string;
    blacklist: boolean;
}
```

### Flags

Most search models use enum flags to specify things like types. These for the most part operate on bitwise operations so if you need to specify multiple flags you'll want to set the value to the bitwise sum of the two flags.

```
export enum FreeToPlayStatus { 
    None = 0,
    All = 1,
    Crew = 2
}
```

```
export enum StageType { 
    Unknown = 0,
    Story = 1,
    Fortnight = 2,
    Weekly = 3,
    Raid = 4,
    Coliseum = 5,
    Special = 6,
    TrainingForest = 7,
    TreasureMap = 8
}
```

```
export enum UnitClass { 
    Unknown = 0,
    Shooter = 1,
    Fighter = 2,
    Striker = 4,
    Slasher = 8,
    Cerebral = 16,
    Driven = 32,
    Powerhouse = 64,
    FreeSpirit = 128,
    Evolver = 256,
    Booster = 512
}
```

```
export enum UnitType { 
    Unknown = 0,
    STR = 1,
    DEX = 2,
    QCK = 4,
    INT = 8,
    PSY = 16
}
```

# GitHub Reminder

Don't forget this is all open source so feel free to sniff around the source if it'll be helpful:
https://github.com/RoboCafaz/TreasureGuide/tree/develop/Source/TreasureGuide.Web/Controllers/API
https://github.com/RoboCafaz/TreasureGuide/tree/master/Source/TreasureGuide.Web/Models