import {
    FlexModule,
    ThemeService
} from './core';

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule, MatIconRegistry } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [
        CommonModule,
        FlexModule,
        MatButtonModule,
        MatIconModule,
        MatToolbarModule,
        RouterOutlet
    ],
    templateUrl: 'app.component.html',
    styleUrl: 'app.component.scss'
})
export class AppComponent {
    constructor(
        iconRegistry: MatIconRegistry,
        public themer: ThemeService
    ) {
        iconRegistry.setDefaultFontSetClass('material-symbols-outlined');
    }
}
