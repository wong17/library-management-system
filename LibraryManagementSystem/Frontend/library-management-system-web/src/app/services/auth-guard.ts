import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateFn, RouterStateSnapshot } from "@angular/router";
import { PermissionsService } from "./permissions.service";

export const AuthGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean => {
    return inject(PermissionsService).canActivate(route, state);
}

export const AdminGuard: CanActivateFn = (route, state) => {
    const permissionsService = inject(PermissionsService);
    route.data = { roles: ['ADMIN'] };
    return permissionsService.canActivate(route, state);
};

export const LibrarianGuard: CanActivateFn = (route, state) => {
    const permissionsService = inject(PermissionsService);
    route.data = { roles: ['LIBRARIAN'] };
    return permissionsService.canActivate(route, state);
};

export const StudentGuard: CanActivateFn = (route, state) => {
    const permissionsService = inject(PermissionsService);
    route.data = { roles: ['STUDENT'] };
    return permissionsService.canActivate(route, state);
};