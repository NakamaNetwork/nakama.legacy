export class StringHelper {
    static prettifyEnum(input: string): string {
        return input.replace(/([A-Z]*)([A-Z])/g, '$1 $2')
            .replace(/^./, str => str.toUpperCase())
            .trim();
    }

    static truncate(text: string, length: number) {
        if (text && text.length > length) {
            text = text.substr(0, length - 3) + '...';
        }
        return text;
    }
}