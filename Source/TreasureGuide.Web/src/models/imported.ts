
export class AccessTokenModel { 
    token: string;
    expiration: Date;
}


export class ExternalLoginConfirmationViewModel { 
    userName: string;
    email: string;
}


export class ProfileModel { 
    userName: string;
    roles: string[];
}


export class SearchModel { 
    page: number;
    pageSize: number;
}


export class ShipStubModel { 
    id: number;
    name: string;
}

export class ShipDetailModel extends ShipStubModel{ 
    description: string;
}

export class ShipEditorModel { 
    id: number;
}


export enum SocketType { 
    Unknown = 0,
    DamageReduction = 1,
    ChargeSpecials = 2,
    BindResistance = 3,
    DespairResistance = 4,
    AutoHeal = 5,
    RCVBoost = 6,
    SlotRateBoost = 7,
    PoisonResistance = 8,
    MapDamageResistance = 9,
    Resilience = 10
}


export class StageStubModel { 
    id: number;
    stamina: number;
    name: string;
    global: boolean;
    type: StageType;
    teamCount: number;
}

export class StageDetailModel { 
    id: number;
    stamina: number;
    name: string;
    global: boolean;
    type: StageType;
    teams: TeamStubModel[];
}

export class StageEditorModel { 
    id: number;
    stamina: number;
    name: string;
    global: boolean;
    type: StageType;
}


export class StageSearchModel extends SearchModel{ 
    term: string;
    type: StageType;
    global: boolean;
}


export enum StageType { 
    Unknown = 0,
    Story = 1,
    Fortnight = 2,
    Weekly = 3,
    Raid = 4,
    Coliseum = 5,
    Special = 6,
    TrainingForest = 7
}


export class TeamStubModel { 
    id: number;
    name: string;
    submittedById: string;
    submittedByName: string;
    score: number;
    global: boolean;
    shipId: number;
    stageId: number;
    teamUnits: TeamUnitStubModel[];
}

export class TeamDetailModel { 
    id: number;
    name: string;
    submittedById: string;
    submittedByName: string;
    score: number;
    description: string;
    guide: string;
    credits: string;
    global: boolean;
    shipId: number;
    stageId: number;
    teamUnits: TeamUnitDetailModel[];
    teamSockets: TeamSocketStubModel[];
}

export class TeamEditorModel { 
    id: number;
    name: string;
    description: string;
    credits: string;
    guide: string;
    shipId: number;
    stageId: number;
    teamSockets: TeamSocketEditorModel[];
    teamUnits: TeamUnitEditorModel[];
}


export class TeamSearchModel extends SearchModel{ 
    term: string;
    leaderId: number;
    stageId: number;
    myBox: boolean;
    global: boolean;
    freeToPlay: boolean;
}


export class TeamSocketModel { 
    socketType: SocketType;
    level: number;
}

export class TeamSocketStubModel extends TeamSocketModel{ 
}

export class TeamSocketDetailModel extends TeamSocketModel{ 
}

export class TeamSocketEditorModel extends TeamSocketModel{ 
}


export class TeamUnitStubModel { 
    unitId: number;
    position: number;
}

export class TeamUnitDetailModel { 
    unitId: number;
    position: number;
    specialLevel: number;
    sub: boolean;
}

export class TeamUnitEditorModel { 
    unitId: number;
    position: number;
    specialLevel: number;
    sub: boolean;
}


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


export enum UnitFlag { 
    Unknown = 0,
    Global = 1,
    RareRecruit = 2,
    RareRecruitExclusive = 4,
    RareRecruitLimited = 8,
    Promotional = 16,
    Shop = 32
}


export class UnitModel { 
    id: number;
    name: string;
    class: UnitClass;
    type: UnitType;
    flags: UnitFlag;
}

export class UnitStubModel extends UnitModel{ 
}

export class UnitDetailModel extends UnitModel{ 
}

export class UnitEditorModel { 
    id: number;
}


export class UnitSearchModel extends SearchModel{ 
    term: string;
    classes: UnitClass[];
    types: UnitType[];
    forceTypes: boolean;
    freeToPlay: boolean;
    global: boolean;
    myBox: boolean;
}


export enum UnitType { 
    Unknown = 0,
    STR = 1,
    DEX = 2,
    QCK = 4,
    INT = 8,
    PSY = 16
}

