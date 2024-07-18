import { Component } from '@angular/core';
import { GeneratorService } from '../services';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'home-route',
    standalone: true,
    templateUrl: 'home.route.html',
    providers: [
        GeneratorService
    ],
    imports: [
        MatButtonModule
    ]
})
export class HomeRoute {
    constructor(
        private generator: GeneratorService
    ) { }

    async generate(): Promise<void> {
        const person = await this.generator.generatePerson();
        await this.generator.generatePdf(person);
    }
}
