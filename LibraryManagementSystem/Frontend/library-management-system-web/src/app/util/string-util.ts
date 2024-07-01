export class StringUtil {
    public static removeAccents(str: string): string {
        return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
    }
}