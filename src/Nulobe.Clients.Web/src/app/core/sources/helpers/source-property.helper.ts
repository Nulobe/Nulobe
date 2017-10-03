import { SourceType, ApaSourceType } from '../source-type';

const PROPERTIES_BY_TYPE = {
    [SourceType.CitationNeeded]: [],
    [SourceType.Nulobe]: ['factId'],
    [SourceType.Legacy]: ['url', 'description'],
    [SourceType.Apa]: ['apaType']
};

const APA_PROPERTIES_BY_TYPE = {
    [ApaSourceType.JournalArticle]: ['authors', 'date', 'title'],
    [ApaSourceType.Book]: ['authors', 'date', 'title']
}

export const SourcePropertyHelper = {

    hasProperty(type: SourceType, apaType: ApaSourceType, propName: string): boolean {
        return this.getProperties(type, apaType).indexOf(propName) > -1;
    },

    getProperties(type: SourceType, apaType: ApaSourceType): string[] {
        let result = [...PROPERTIES_BY_TYPE[type]];

        if (type === SourceType.Apa && ApaSourceType[apaType]) {
            result.push(...APA_PROPERTIES_BY_TYPE[apaType])
        }

        return result;
    }
};