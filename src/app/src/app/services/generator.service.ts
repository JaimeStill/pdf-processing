import {
    HttpClient,
    HttpResponse
} from '@angular/common/http';

import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';
import { Person } from '../models';

@Injectable()
export class GeneratorService {
    private api: string = `${environment.api}generator/`;

    constructor(
        private http: HttpClient
    ) { }

    generatePerson(): Promise<Person> {
        return firstValueFrom(
            this.http.get<Person>(`${this.api}generatePerson`)
        );
    }

    generatePdf(person: Person): Promise<boolean> {
        return new Promise((resolve, reject) => {
            try {
                this.http.post<HttpResponse<Blob>>(
                    `${this.api}generatePdf`,
                    person,
                    <Object>{
                        responseType: 'blob',
                        observe: 'response'
                    }
                )
                .subscribe({
                    next: (data: HttpResponse<Blob>) => {
                        const content: string | null = data
                            .headers
                            .get('content-disposition');

                        if (content && data.body) {
                            const filename = content
                                .split(';')
                                .filter(value => value.includes('filename='))[0]
                                .split('=')[1];

                            const link = document.createElement('a');
                            link.href = window.URL.createObjectURL(data.body);
                            link.download = filename;
                            link.click();

                            resolve(true);
                        } else
                            resolve(false);
                    },
                    error: (err: any) => reject(err)
                });
            } catch (err) {
                reject(err);
            }
        });
    }
}
