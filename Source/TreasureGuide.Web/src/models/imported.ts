
export interface IAccessTokenModel {
    token: string;
    expiration: Date;
    
}


export interface IExternalLoginConfirmationViewModel {
    userName: string;
    email: string;
    
}


export interface IProfileAuthExportModel {
    userName: string;
    roles: string[];
    
}

export interface IProfileStubModel {
    id: string;
    userName: string;
    unitId: number;
    
}

export interface IProfileDetailModel extends IProfileStubModel{
    friendId: number;
    website: string;
    userRoles: string[];
    global: boolean;
    canEdit: boolean;
    
}

export interface IProfileEditorModel {
    id: string;
    userName: string;
    unitId: number;
    friendId: number;
    website: string;
    userRoles: string[];
    global: boolean;
    
}

export interface IProfileSearchModel extends ISearchModel{
    term: string;
    roles: string[];
    
}


export class RoleConstants {
    public static Administrator: string = "Administrator";
    public static Moderator: string = "Moderator";
    public static Contributor: string = "Contributor";
    
}


export interface ISearchModel {
    page: number;
    pageSize: number;
    
}


export interface IShipStubModel {
    id: number;
    name: string;
    
}

export interface IShipDetailModel extends IShipStubModel{
    description: string;
    
}

export interface IShipEditorModel {
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


export interface IStageStubModel {
    id: number;
    stamina: number;
    name: string;
    global: boolean;
    type: StageType;
    teamCount: number;
    
}

export interface IStageDetailModel {
    teams: ITeamStubModel[];
    id: number;
    stamina: number;
    name: string;
    global: boolean;
    type: StageType;
    
}

export interface IStageEditorModel {
    id: number;
    stamina: number;
    name: string;
    global: boolean;
    type: StageType;
    
}


export interface IStageSearchModel extends ISearchModel{
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
    TrainingForest = 7,
    TreasureMap = 8
}


export interface ITeamStubModel {
    teamUnits: ITeamUnitStubModel[];
    id: number;
    name: string;
    submittedById: string;
    submittedByName: string;
    score: number;
    global: boolean;
    shipId: number;
    stageId: number;
    
}

export interface ITeamDetailModel {
    teamUnits: ITeamUnitDetailModel[];
    teamSockets: ITeamSocketStubModel[];
    id: number;
    name: string;
    submittedById: string;
    submittedByName: string;
    submittedDate: Date;
    editedById: string;
    editedByName: string;
    editedDate: Date;
    score: number;
    guide: string;
    credits: string;
    global: boolean;
    shipId: number;
    stageId: number;
    canEdit: boolean;
    
}

export interface ITeamEditorModel {
    teamSockets: ITeamSocketEditorModel[];
    teamUnits: ITeamUnitEditorModel[];
    id: number;
    name: string;
    credits: string;
    guide: string;
    shipId: number;
    stageId: number;
    
}


export interface ITeamSearchModel extends ISearchModel{
    term: string;
    leaderId: number;
    stageId: number;
    myBox: boolean;
    global: boolean;
    freeToPlay: boolean;
    
}


export interface ITeamSocketModel {
    socketType: SocketType;
    level: number;
    
}

export interface ITeamSocketStubModel extends ITeamSocketModel{
    
}

export interface ITeamSocketDetailModel extends ITeamSocketModel{
    
}

export interface ITeamSocketEditorModel extends ITeamSocketModel{
    
}


export interface ITeamUnitStubModel {
    unitId: number;
    position: number;
    
}

export interface ITeamUnitDetailModel {
    unitId: number;
    position: number;
    specialLevel: number;
    sub: boolean;
    
}

export interface ITeamUnitEditorModel {
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


export interface IUnitModel {
    id: number;
    name: string;
    class: UnitClass;
    type: UnitType;
    flags: UnitFlag;
    
}

export interface IUnitStubModel extends IUnitModel{
    
}

export interface IUnitDetailModel extends IUnitModel{
    
}

export interface IUnitEditorModel {
    id: number;
    
}


export interface IUnitSearchModel extends ISearchModel{
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

