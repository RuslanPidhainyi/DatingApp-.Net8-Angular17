import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import {provideAnimations} from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { errorInterceptor } from './_interceptors/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),//router
    provideHttpClient(withInterceptors([errorInterceptor])), //wstrzykiwania zaleznosci (injection)
    provideAnimations(), //sluzy dla animacji
    provideToastr({
      positionClass: 'toast-bottom-right'
    }), //sluzy dla powiadomienia
  ]
};
