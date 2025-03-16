import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonGetViewModel } from '../models/person-view-model';
import { PersonUpdateViewModel } from '../models/person-view-model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getById(id: number): Observable<PersonGetViewModel> {
    return this.http.get<PersonGetViewModel>(`${this.baseUrl}api/person/${id}`);
  }

  getAll(): Observable<PersonGetViewModel[]> {
    return this.http.get<PersonGetViewModel[]>(`${this.baseUrl}api/person`);
  }

  addPerson(person: PersonUpdateViewModel): Observable<PersonGetViewModel> {
    return this.http.post<PersonGetViewModel>(`${this.baseUrl}api/person`, person);
  }

  updatePerson(person: PersonUpdateViewModel): Observable<PersonGetViewModel> {
    return this.http.put<PersonGetViewModel>(`${this.baseUrl}api/person/${person.id}`, person);
  }
}
