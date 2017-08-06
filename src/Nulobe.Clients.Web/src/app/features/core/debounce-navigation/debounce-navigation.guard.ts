import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { Observable } from 'rxjs';

@Injectable()
export class DebounceNavigationGuard<TComponent> implements CanDeactivate<TComponent> {
    
    canDeactivate(
        component: TComponent, currentRoute: ActivatedRouteSnapshot,
        currentState: RouterStateSnapshot, nextState?: RouterStateSnapshot)
        : boolean | Observable<boolean> | Promise<boolean>
    {
        let debounceNavigationTimeData = currentRoute.data['debounceNavigationTime'];
        let debounceNavigationTime = debounceNavigationTimeData ? parseInt(debounceNavigationTimeData) : 0;
        return Observable.of(true).debounceTime(debounceNavigationTime);
    }
}