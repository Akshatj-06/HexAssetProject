import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { customInterceptor } from './interceptor/custom.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {ToastrModule} from 'ngx-toastr';
import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
export const appConfig: ApplicationConfig = {
  providers: [ importProvidersFrom(ToastrModule.forRoot(), BrowserAnimationsModule),
    provideHttpClient(withInterceptors([customInterceptor])) ,provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes)]
};
