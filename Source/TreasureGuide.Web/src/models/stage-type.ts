export class StageType {
    id: number;
    name: string;

    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }

    static unknown = new StageType(0, "Unknown");
}