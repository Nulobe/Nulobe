import { SourceType, ApaSourceType } from '../api';

export interface Source {
    trackById?: number;
    type: SourceType;
    apaType?: ApaSourceType;
    organisation?: string;
    date?: SourceDate;
    title?: string;
    pages?: SourcePages;
    doi?: string;
    url?: string;
    description?: string;
    factId?: string;
    authors?: string[];
}

export interface SourceDate {
    year: number;
    month?: number;
    day: number;
}

export interface SourcePages {
    pageStart?: number;
    pageEnd?: number;
}