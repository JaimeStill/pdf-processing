import { Routes } from '@angular/router';

import {
    HomeRoute    
} from './routes';

export const routes: Routes = [
    { path: '', component: HomeRoute },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
