import { SourceType } from './source-type';

export interface Source {
    trackById: number;
    type: SourceType;
    url?: string;
    description?: string;
}