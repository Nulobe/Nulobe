import { NULOBE_ENV_SETTINGS } from '../../../app.settings';
import { SourceType, ApaSourceType } from '../../api';

export const SourcePropertyHelper = {

    hasProperty(sourceType: SourceType, propName: string): boolean {
        return this.getProperties(sourceType).indexOf(propName) > -1;
    },

    getProperties(sourceType: SourceType): string[] {
        if (!sourceType) {
            throw new Error('Unknown source type');
        }
        else {
            return NULOBE_ENV_SETTINGS.sourceTypeFields[SourceType[sourceType]];
        }
    }
};