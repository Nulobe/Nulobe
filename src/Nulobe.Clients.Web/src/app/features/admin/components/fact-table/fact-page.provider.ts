import { Observable } from 'rxjs';
import { PageModel } from '../../../../core/abstractions';
import { Fact } from '../../../../core/api';

export interface FactPageProvider {
    getFactPage: (pageIndex: number, pageSize: number) => Observable<PageModel<Fact>>;
}