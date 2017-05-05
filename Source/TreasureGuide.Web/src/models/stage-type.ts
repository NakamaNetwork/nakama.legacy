export class StageType {
    id: number;
    name: string;

    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }

    static unknown = new StageType(0, 'Unknown');
    static story = new StageType(1, 'Story');
    static fortnight = new StageType(2, 'Fortnight');
    static weekly = new StageType(3, 'Weekly');
    static raid = new StageType(4, 'Raid');
    static coliseum = new StageType(5, 'Coliseum');
    static special = new StageType(6, 'Special');
    static trainingForest = new StageType(7, 'TrainingForest');

    static all = [
        StageType.unknown,
        StageType.story,
        StageType.fortnight,
        StageType.weekly,
        StageType.raid,
        StageType.coliseum,
        StageType.special,
        StageType.trainingForest
    ];
}