export class StringHelper {
    static prettifyEnum(input: string): string {
        return input.replace(/([A-Z]*)([A-Z])/g, '$1 $2')
            .replace(/^./, str => str.toUpperCase())
            .trim();
    }
}