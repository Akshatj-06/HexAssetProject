import { Route, Routes } from "@angular/router";
import { AssetAllocationComponent } from "./components/asset-allocation/asset-allocation.component";
import { AssetRequestComponent } from "./components/asset-request/asset-request.component";
import { AssetComponent } from "./components/asset/asset.component";
import { AuditRequestComponent } from "./components/audit-request/audit-request.component";
import { ServiceRequestComponent } from "./components/service-request/service-request.component";
import { UserComponent } from "./components/user/user.component";
import { HeaderComponent } from "./header/header.component";
import { LoginComponent } from "./login/login.component";
import { ForgotPasswordComponent } from "./login/forgot-password/forgot-password.component";
import { authGuard } from "./guard/auth.guard";
import { HomeComponent } from "./home/home.component";

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'forgot-password',
        component: ForgotPasswordComponent
    },
    {
        path: 'header',
        component: HeaderComponent,
        children: [ 
            {
                path: '',
                redirectTo: 'Home',
                pathMatch: 'full'
              },
            {
                path: 'User',
                component: UserComponent
            },
            {
                path: 'Home',
                component: HomeComponent
            },
            {
                path: 'Asset',
                component: AssetComponent
             
            },
            {
                path: 'Asset-Request',
                component: AssetRequestComponent
            },
            {
                path: 'Service-Request',
                component: ServiceRequestComponent
            },
            {
                path: 'Asset-Allocation',
                component: AssetAllocationComponent
            },
            {
                path: 'Audit-Request',
                component: AuditRequestComponent
            }
        ]
    },
    {
        path: '**', 
        redirectTo: 'login'
    }
];
