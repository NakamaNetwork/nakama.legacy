
export interface AccessTokenModel { 
    token: string;
    expiration: Date;
}


export interface ExternalLoginConfirmationViewModel { 
    userName: string;
    email: string;
}


export interface ProfileModel { 
    userName: string;
    roles: string[];
}


export interface SearchModel { 
    page: number;
    pageSize: number;
}


export interface ShipStubModel { 
    id: number;
    name: string;
}

export interface ShipDetailModel extends ShipStubModel{ 
    description: string;
}

export interface ShipEditorModel { 
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


export interface StageStubModel { 
    id: number;
    stamina: number;
    name: string;
    global: boolean;
    type: StageType;
    teamCount: number;
}

export interface StageDetailModel { 
    id: number;
    stamina: number;
    name: string;
    global: boolean;
    type: StageType;
    teams: TeamStubModel[];
}

export interface StageEditorModel { 
    id: number;
    stamina: number;
    name: string;
    global: boolean;
    type: StageType;
}


export interface StageSearchModel extends SearchModel{ 
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


export interface TeamStubModel { 
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

export interface TeamDetailModel { 
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

export interface TeamEditorModel { 
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


export interface TeamSearchModel extends SearchModel{ 
    term: string;
    leaderId: number;
    stageId: number;
    myBox: boolean;
    global: boolean;
    freeToPlay: boolean;
}


export interface TeamSocketModel { 
    socketType: SocketType;
    level: number;
}

export interface TeamSocketStubModel extends TeamSocketModel{ 
}

export interface TeamSocketDetailModel extends TeamSocketModel{ 
}

export interface TeamSocketEditorModel extends TeamSocketModel{ 
}


export interface TeamUnitStubModel { 
    unitId: number;
    position: number;
}

export interface TeamUnitDetailModel { 
    unitId: number;
    position: number;
    specialLevel: number;
    sub: boolean;
}

export interface TeamUnitEditorModel { 
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


export interface UnitModel { 
    id: number;
    name: string;
    class: UnitClass;
    type: UnitType;
    flags: UnitFlag;
}

export interface UnitStubModel extends UnitModel{ 
}

export interface UnitDetailModel extends UnitModel{ 
}

export interface UnitEditorModel { 
    id: number;
}


export interface UnitSearchModel extends SearchModel{ 
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

