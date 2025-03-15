// Disclaimer: ChatGPT used to assist me to generate this test, as I am not very experienced with frontend testing frameworks.

import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PersonService } from './person.service';
import { PersonViewModel } from '../models/person-view-model';

describe('PersonService', () => {
  let service: PersonService;
  let httpMock: HttpTestingController;
  const baseUrl = 'http://localhost/';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        PersonService,
        { provide: 'BASE_URL', useValue: "http://localhost/" },
      ]
  });
    service = TestBed.inject(PersonService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getById should fetch the person with the given ID (GET)', () => {
    const dummyPerson: PersonViewModel = { id: 1, firstName: 'John', lastName: 'Doe' };

    service.getById(1).subscribe(person => {
      expect(person).toEqual(dummyPerson);
    });

    const req = httpMock.expectOne(`${baseUrl}api/person/1`);
    expect(req.request.method).toBe('GET');

    req.flush(dummyPerson);
  });

  it('getAll should retrieve an array of persons (GET)', () => {
    const dummyPersons: PersonViewModel[] = [
      { id: 1, firstName: 'John', lastName: 'Doe' },
      { id: 2, firstName: 'Jane', lastName: 'Doe' }
    ];

    service.getAll().subscribe(persons => {
      expect(persons).toEqual(dummyPersons);
    });

    const req = httpMock.expectOne(baseUrl + "api/person");
    expect(req.request.method).toBe('GET');

    req.flush(dummyPersons);
  });

  it('addPerson should add a new person via POST', () => {
    const newPerson: PersonViewModel = { id: 3, firstName: 'Alice', lastName: 'Smith' };

    service.addPerson(newPerson).subscribe(person => {
      expect(person).toEqual(newPerson);
    });

    const req = httpMock.expectOne(baseUrl + "api/person");
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newPerson);

    req.flush(newPerson);
  });

  it('updatePerson should update an existing person via PUT', () => {
    const updatedPerson: PersonViewModel = { id: 1, firstName: 'John', lastName: 'DoeUpdated' };

    service.updatePerson(updatedPerson).subscribe(person => {
      expect(person).toEqual(updatedPerson);
    });

    const req = httpMock.expectOne(`${baseUrl}api/person/${updatedPerson.id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedPerson);

    req.flush(updatedPerson);
  });
});
