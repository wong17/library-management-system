import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";
import { UserService } from "./security/user.service";
import { UserDto } from "../entities/dtos/security/user-dto";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class PermissionsService {

    constructor(private router: Router, private userService: UserService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        const currentUser = this.userService.currentUser;
        // Verifica si el usuario ha iniciado sesión
        if (!currentUser) {
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
            return false;
        }

        // Verifica si el usuario tiene el rol adecuado
        const expectedRoles = route.data['roles'] as string[];
        if (expectedRoles && !this.userHasRole(currentUser, expectedRoles)) {
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
            return false;
        }

        return true;
    }

    private userHasRole(user: UserDto, roles: string[]): boolean {
        return user.roles?.some(role => roles.includes(role.name!)) ?? false;
    }
}