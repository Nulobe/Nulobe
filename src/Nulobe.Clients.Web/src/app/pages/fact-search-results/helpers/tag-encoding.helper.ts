export const QUERY_STRING_SEPARATOR = '-';

export class TagEncodingHelper {

    static encode(tags: string[]): string {
        return tags
            .map(t => t.toLowerCase())
            .join(QUERY_STRING_SEPARATOR);
    }

    static decode(tagsQuery: string): string[] {
        return tagsQuery.split(QUERY_STRING_SEPARATOR);
    }
}