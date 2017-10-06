import { SourceType, ApaSourceType } from '../api';

export interface Source {
    trackById?: number;
    type: SourceType;
    title?: string;
    apaType?: ApaSourceType;
    url?: string;
    description?: string;
    factId?: string;
    authors?: string[];
    date?: SourceDate;
}

export interface SourceDate {
    year: number;
    month?: number;
    day: number;
}