import { SourceType, ApaSourceType } from './source-type';

export interface Source {
    trackById?: number;
    type: SourceType;
    apaType?: ApaSourceType;
    url?: string;
    description?: string;
    factId?: string;
    authors?: string[];
}