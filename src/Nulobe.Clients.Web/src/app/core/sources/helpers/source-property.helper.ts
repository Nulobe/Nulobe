import { SourceType } from '../source-type';

const PROPERTIES_BY_TYPE = {
    [SourceType.CitationNeeded]: [],
    [SourceType.Nulobe]: ['factId'],
    [SourceType.Legacy]: ['url', 'description']
};

export const SourcePropertyHelper = {

    hasProperty(type: SourceType, propName: string): boolean {
        return PROPERTIES_BY_TYPE[type].indexOf(propName) > -1;
    },

    getProperties(type: SourceType): string[] {
        return [...PROPERTIES_BY_TYPE[type]];
    }
};