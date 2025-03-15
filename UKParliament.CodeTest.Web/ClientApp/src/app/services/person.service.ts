import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonViewModel } from '../models/person-view-model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getById(id: number): Observable<PersonViewModel> {
    return this.http.get<PersonViewModel>(this.baseUrl + `api/person/${id}`)
  }

  getAll(): Observable<PersonViewModel[]> {
    return this.http.get<PersonViewModel[]>(this.baseUrl + 'api/person');
  }

  addPerson(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.post<PersonViewModel>(`${this.baseUrl}api/person`, person);
  }

  updatePerson(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.put<PersonViewModel>(`${this.baseUrl}api/person/${person.id}`, person);
  }
}
