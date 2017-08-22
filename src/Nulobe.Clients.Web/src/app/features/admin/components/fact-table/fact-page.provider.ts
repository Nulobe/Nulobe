import { Observable } from 'rxjs';
import { PageModel } from '../../../../core/abstractions';
import { Fact } from '../../../../core/api';

export interface FactPageOptions {
    tags: string;
}

export interface FactPageProvider {
    getFactPage: (pageIndex: number, pageSize: number, options?: FactPageOptions) => Observable<PageModel<Fact>>;
}