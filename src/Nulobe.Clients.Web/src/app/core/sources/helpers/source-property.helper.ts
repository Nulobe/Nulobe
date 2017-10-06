import { NULOBE_ENV_SETTINGS } from '../../../app.settings';
import { SourceType, ApaSourceType } from '../../api';

export const SourcePropertyHelper = {

    hasProperty(sourceType: SourceType, apaSourceType: ApaSourceType, propName: string): boolean {
        return this.getProperties(sourceType, apaSourceType).indexOf(propName) > -1;
    },

    getProperties(sourceType: SourceType, apaSourceType: ApaSourceType = null): string[] {
        if (!sourceType) {
            throw new Error('Unknown source type');
        }
        else if (sourceType === SourceType.Apa) {
            if (!apaSourceType) {
                throw new Error('Unknown apa source type');
            }

            return NULOBE_ENV_SETTINGS.apaSourceTypeFields[ApaSourceType[apaSourceType]];
        }
        else {
            return NULOBE_ENV_SETTINGS.sourceTypeFields[SourceType[sourceType]];
        }
    }
};