
export interface IAccessTokenModel {
    token: string;
    expiration: Date;
    
}


export class BoxConstants {
    public static BoxLimit: number = 2;
    public static DonorBoxLimit: number = 6;
    
}


export interface IBoxStubModel {
    id: number;
    name: string;
    friendId: number;
    global: boolean;
    public: boolean;
    blacklist: boolean;
    
}

export interface IBoxDetailModel extends IBoxStubModel{
    boxUnits: IBoxUnitDetailModel[];
    userId: string;
    userName: string;
    userIsDonor: boolean;
    userUnitId: number;
    
}

export interface IBoxEditorModel {
    id: number;
    name: string;
    friendId: number;
    global: boolean;
    public: boolean;
    blacklist: boolean;
    
}


export interface IBoxSearchModel extends ISearchModel{
    userId: string;
    blacklist: boolean;
    
}


export interface IBoxUnitStubModel {
    unitId: number;
    flags: IndividualUnitFlags;
    
}

export interface IBoxUnitDetailModel extends IBoxUnitStubModel{
    
}

export interface IBoxUnitEditorModel {
    unitId: number;
    flags: number;
    
}


export interface IBoxUpdateModel {
    updated: IBoxUnitUpdateModel[];
    id: number;
    added: number[];
    removed: number[];
    
}

export interface IBoxUnitUpdateModel {
    id: number;
    flags: IndividualUnitFlags;
    
}


export enum CacheItemType { 
    FullReset = 0,
    Unit = 1,
    Stage = 2,
    Ship = 3
}


export interface IDonationStubModel {
    id: number;
    userId: string;
    userName: string;
    userUnitId: number;
    userIsDonor: boolean;
    amount: number;
    date: Date;
    public: boolean;
    message: string;
    
}

export interface IDonationDetailModel extends IDonationStubModel{
    
}

export interface IDonationEditorModel {
    id: number;
    
}


export interface IDonationResultModel {
    id: number;
    userId: string;
    payerId: string;
    paymentId: string;
    tokenId: string;
    state: PaymentState;
    paymentType: PaymentType;
    error: string;
    redirectUrl: string;
    hasError: boolean;
    
}


export interface IDonationSearchModel extends ISearchModel{
    user: string;
    startDate: Date;
    endDate: Date;
    minAmount: number;
    maxAmount: number;
    complex: boolean;
    
}


export interface IDonationSubmissionModel {
    amount: number;
    paymentType: PaymentType;
    message: string;
    public: boolean;
    
}


export interface IDonationVerificationModel {
    paymentId: string;
    tokenId: string;
    id: number;
    
}


export interface IExternalLoginConfirmationViewModel {
    userName: string;
    email: string;
    toS: boolean;
    privacy: boolean;
    
}


export interface IGCREditorModel {
    units: IGCRUnitEditModel[];
    stages: IGCRStageEditModel[];
    
}

export interface IGCRResultModel {
    units: IGCRDataModel[];
    stages: IGCRDataModel[];
    teams: IGCRTableModel[];
    
}

export interface IGCRTableModel {
    id: number;
    leaderId: number;
    stageId: number;
    f2P: boolean;
    global: boolean;
    video: boolean;
    
}

export interface IGCRDataModel {
    id: number;
    thumbnail: number;
    name: string;
    color: UnitType;
    
}

export interface IGCRAbstractEditModel {
    order: number;
    name: string;
    
}

export interface IGCRUnitEditModel extends IGCRAbstractEditModel{
    unitId: number;
    
}

export interface IGCRStageEditModel extends IGCRAbstractEditModel{
    stageId: number;
    
}


export enum IndividualUnitFlags { 
    Unknown = 0,
    Favorite = 32,
    LevelMaxed = 1,
    SkillLevelMaxed = 2,
    SocketsMaxed = 4,
    CottonCandyMaxed = 8,
    LimitBroken = 16
}


export interface IMetaResultModel {
    title: string;
    description: string;
    
}


export enum PaymentState { 
    Unknown = 0,
    Initialized = 1,
    Processing = 2,
    Complete = 3,
    Failed = 4,
    Cancelled = 5,
    Chargeback = 6
}


export enum PaymentType { 
    Unknown = 0,
    Paypal = 1
}


export interface IProfileStubModel {
    id: string;
    userName: string;
    unitId: number;
    teamCount: number;
    
}

export interface IProfileDetailModel extends IProfileStubModel{
    website: string;
    userRoles: string[];
    canEdit: boolean;
    
}

export interface IMyProfileModel extends IProfileDetailModel{
    userPreferences: { [key: number]: string; };
    boxCount: number;
    
}

export interface IProfileEditorModel {
    id: string;
    userName: string;
    unitId: number;
    website: string;
    userRoles: string[];
    
}

export interface IProfileSearchModel extends ISearchModel{
    term: string;
    roles: string[];
    
}


export class RoleConstants {
    public static Administrator: string = "Administrator";
    public static Moderator: string = "Moderator";
    public static BetaTester: string = "BetaTester";
    public static Contributor: string = "Contributor";
    public static BoxUser: string = "BoxUser";
    public static Donor: string = "Donor";
    public static GCRViewer: string = "GCR";
    public static GCRAdmin: string = "GCRAdmin";
    
}


export interface IScheduleModel {
    live: IScheduleSubModel;
    upcoming: IScheduleSubModel;
    
}

export interface IScheduleSubModel {
    global: number[];
    japan: number[];
    
}


export class SearchConstants {
    public static SortId: string = "Id";
    public static SortName: string = "Name";
    public static SortType: string = "Type";
    public static SortStage: string = "Stage";
    public static SortClass: string = "Class";
    public static SortStars: string = "Stars";
    public static SortCount: string = "Count";
    public static SortScore: string = "Score";
    public static SortLeader: string = "Leader";
    public static SortDate: string = "Date";
    public static SortUser: string = "User";
    public static SortAmount: string = "Amount";
    
}


export interface ISearchModel {
    page: number;
    pageSize: number;
    sortBy: string;
    sortDesc: boolean;
    
}


export interface IShipStubModel {
    id: number;
    name: string;
    eventShip: boolean;
    
}


export enum SocketType { 
    Unknown = 0,
    DamageReduction = 1,
    RCVBoost = 2,
    ChargeSpecials = 3,
    SlotRateBoost = 4,
    BindResistance = 5,
    PoisonResistance = 6,
    DespairResistance = 7,
    MapDamageResistance = 8,
    AutoHeal = 9,
    Resilience = 10
}


export interface IStageSearchModel extends ISearchModel{
    term: string;
    type: StageType;
    global: boolean;
    
}


export interface IStageStubModel {
    id: number;
    unitId: number;
    name: string;
    global: boolean;
    type: StageType;
    aliases: string[];
    
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


export interface ITeamCommentStubModel {
    id: number;
    teamId: number;
    text: string;
    deleted: boolean;
    reported: boolean;
    canEdit: boolean;
    myVote: number;
    score: number;
    submittedById: string;
    submittedByName: string;
    submittedByUnitId: number;
    submittedByIsDonor: boolean;
    editedDate: Date;
    
}

export interface ITeamCommentDetailModel extends ITeamCommentStubModel{
    
}

export interface ITeamCommentEditorModel {
    id: number;
    teamId: number;
    text: string;
    
}


export interface ITeamCommentSearchModel extends ISearchModel{
    teamId: number;
    deleted: boolean;
    reported: boolean;
    
}


export interface ITeamCommentVoteModel {
    teamCommentId: number;
    up: boolean;
    
}


export interface ITeamCreditModel {
    credit: string;
    type: TeamCreditType;
    
}


export enum TeamCreditType { 
    Unknown = 0,
    Reddit = 1,
    YouTube = 2,
    Twitter = 3,
    Slefty = 4,
    Irving = 5,
    GCR = 6
}


export interface ITeamGenericSlotStubModel {
    role: UnitRole;
    type: UnitType;
    class: UnitClass;
    position: number;
    
}

export interface ITeamGenericSlotDetailModel extends ITeamGenericSlotStubModel{
    sub: boolean;
    
}

export interface ITeamGenericSlotEditorModel extends ITeamGenericSlotDetailModel{
    
}


export interface ITeamImportModel {
    team: ITeamEditorModel;
    credit: ITeamCreditModel;
    videos: ITeamVideoModel[];
    
}


export interface ITeamStubModel {
    teamUnits: ITeamUnitStubModel[];
    teamGenericSlots: ITeamGenericSlotStubModel[];
    id: number;
    name: string;
    submittedById: string;
    submittedByName: string;
    submittedByUnitId: number;
    submittedByIsDonor: boolean;
    editedDate: Date;
    score: number;
    global: boolean;
    f2P: boolean;
    f2PC: boolean;
    hasVideos: boolean;
    shipId: number;
    stageId: number;
    invasionId: number;
    deleted: boolean;
    reported: boolean;
    draft: boolean;
    comments: number;
    
}

export interface ITeamDetailModel {
    teamUnits: ITeamUnitDetailModel[];
    teamGenericSlots: ITeamGenericSlotDetailModel[];
    teamSockets: ITeamSocketStubModel[];
    teamVideos: ITeamVideoModel[];
    id: number;
    name: string;
    submittedById: string;
    submittedByName: string;
    submittedByUnitId: number;
    submittedByIsDonor: boolean;
    editedDate: Date;
    score: number;
    myVote: number;
    myBookmark: boolean;
    guide: string;
    credits: string;
    global: boolean;
    f2P: boolean;
    f2PC: boolean;
    shipId: number;
    stageId: number;
    invasionId: number;
    canEdit: boolean;
    deleted: boolean;
    reported: boolean;
    draft: boolean;
    
}

export interface ITeamEditorModel {
    teamSockets: ITeamSocketEditorModel[];
    teamUnits: ITeamUnitEditorModel[];
    teamGenericSlots: ITeamGenericSlotEditorModel[];
    id: number;
    name: string;
    credits: string;
    guide: string;
    shipId: number;
    stageId: number;
    invasionId: number;
    deleted: boolean;
    draft: boolean;
    
}


export interface ITeamReportModel {
    teamId: number;
    reason: string;
    
}

export interface ITeamReportStubModel {
    id: number;
    teamId: number;
    reason: string;
    acknowledged: boolean;
    
}


export interface ITeamSearchModel extends ISearchModel{
    term: string;
    submittedBy: string;
    leaderId: number;
    noHelp: boolean;
    stageId: number;
    invasionId: number;
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
    excludeEventShips: boolean;
    
}

export enum FreeToPlayStatus { 
    None = 0,
    All = 1,
    Crew = 2
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

export interface ITeamUnitDetailModel extends ITeamUnitStubModel{
    flags: IndividualUnitFlags;
    sub: boolean;
    
}

export interface ITeamUnitEditorModel extends ITeamUnitStubModel{
    flags: IndividualUnitFlags;
    sub: boolean;
    
}


export interface ITeamVideoModel {
    id: number;
    teamId: number;
    videoLink: string;
    submittedDate: Date;
    deleted: boolean;
    userId: string;
    userName: string;
    userUnitId: number;
    userIsDonor: boolean;
    
}


export interface ITeamVoteModel {
    teamId: number;
    up: boolean;
    
}


export enum UnitClass { 
    Unknown = 0,
    Fighter = 1,
    Slasher = 2,
    Striker = 4,
    Shooter = 8,
    FreeSpirit = 16,
    Driven = 32,
    Cerebral = 64,
    Powerhouse = 128,
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


export enum UnitRole { 
    Unknown = 0,
    Beatstick = 1,
    DamageReducer = 2,
    DefenseReducer = 4,
    Delayer = 8,
    AttackBooster = 16,
    OrbBooster = 32,
    FixedDamage = 64,
    HealthCutter = 128,
    OrbShuffler = 256,
    Healer = 512,
    Zombie = 1024,
    Nuker = 2048
}


export interface IUnitSearchModel extends ISearchModel{
    term: string;
    classes: UnitClass;
    types: UnitType;
    forceClass: boolean;
    freeToPlay: boolean;
    global: boolean;
    myBox: boolean;
    blacklist: boolean;
    limitTo: number[];
    
}


export interface IUnitStubModel {
    id: number;
    name: string;
    maxLevel: number;
    stars: number;
    cost: number;
    class: UnitClass;
    type: UnitType;
    flags: UnitFlag;
    aliases: string[];
    evolvesTo: number[];
    evolvesFrom: number[];
    
}


export enum UnitType { 
    Unknown = 0,
    STR = 1,
    DEX = 2,
    QCK = 4,
    INT = 8,
    PSY = 16
}


export enum UserPreferenceType { 
    Unknown = 0,
    BoxId = 1
}


export interface IWikiSearchResultModel {
    teamUnits: IWikiSearchUnitStubModel[];
    id: number;
    name: string;
    submittedById: string;
    submittedByName: string;
    editedDate: Date;
    score: number;
    shipId: number;
    shipName: string;
    stageId: number;
    stageName: string;
    invasionId: number;
    invasionName: string;
    
}

export interface IWikiSearchUnitStubModel extends ITeamUnitStubModel{
    unitName: string;
    
}

