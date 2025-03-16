// Disclaimer: ChatGPT used to assist me to generate this test, as I am not very experienced with frontend testing frameworks.

import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PersonService } from './person.service';
import { PersonGetViewModel } from '../models/person-view-model';
import { PersonUpdateViewModel } from '../models/person-view-model';

describe('PersonService', () => {
  let service: PersonService;
  let httpMock: HttpTestingController;
  const baseUrl = 'http://localhost/';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        PersonService,
        { provide: 'BASE_URL', useValue: baseUrl }
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
    const dummyPerson: PersonGetViewModel = {
      id: 1,
      firstName: 'John',
      lastName: 'Doe',
      dateOfBirth: '1990-01-01',
      department: { id: 1, departmentName: 'Sales' }
    };

    service.getById(1).subscribe(person => {
      expect(person).toEqual(dummyPerson);
    });

    const req = httpMock.expectOne(`${baseUrl}api/person/1`);
    expect(req.request.method).toBe('GET');
    req.flush(dummyPerson);
  });

  it('getAll should retrieve an array of persons (GET)', () => {
    const dummyPersons: PersonGetViewModel[] = [
      {
        id: 1,
        firstName: 'John',
        lastName: 'Doe',
        dateOfBirth: '1990-01-01',
        department: { id: 1, departmentName: 'Sales' }
      },
      {
        id: 2,
        firstName: 'Jane',
        lastName: 'Doe',
        dateOfBirth: '1985-05-05',
        department: { id: 2, departmentName: 'Marketing' }
      }
    ];

    service.getAll().subscribe(persons => {
      expect(persons).toEqual(dummyPersons);
    });

    const req = httpMock.expectOne(`${baseUrl}api/person`);
    expect(req.request.method).toBe('GET');
    req.flush(dummyPersons);
  });

  it('addPerson should add a new person via POST', () => {
    const newPerson: PersonUpdateViewModel = {
      id: 0,
      firstName: 'Alice',
      lastName: 'Smith',
      dateOfBirth: '1992-02-02',
      departmentId: 1
    };

    const returnedPerson: PersonGetViewModel = {
      id: 3,
      firstName: 'Alice',
      lastName: 'Smith',
      dateOfBirth: '1992-02-02',
      department: { id: 1, departmentName: 'Sales' }
    };

    service.addPerson(newPerson).subscribe(person => {
      expect(person).toEqual(returnedPerson);
    });

    const req = httpMock.expectOne(`${baseUrl}api/person`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newPerson);
    req.flush(returnedPerson);
  });

  it('updatePerson should update an existing person via PUT', () => {
    const updatedPerson: PersonUpdateViewModel = {
      id: 1,
      firstName: 'John',
      lastName: 'DoeUpdated',
      dateOfBirth: '1990-01-01',
      departmentId: 1
    };

    const returnedPerson: PersonGetViewModel = {
      id: 1,
      firstName: 'John',
      lastName: 'DoeUpdated',
      dateOfBirth: '1990-01-01',
      department: { id: 1, departmentName: 'Sales' }
    };

    service.updatePerson(updatedPerson).subscribe(person => {
      expect(person).toEqual(returnedPerson);
    });

    const req = httpMock.expectOne(`${baseUrl}api/person/${updatedPerson.id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedPerson);
    req.flush(returnedPerson);
  });
});
