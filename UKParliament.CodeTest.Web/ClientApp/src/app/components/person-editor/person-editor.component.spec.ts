// Disclaimer: ChatGPT used to assist me to generate this test, as I am not very experienced with frontend testing frameworks.

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { PersonEditorComponent } from './person-editor.component';
import { PersonService } from '../../services/person.service';
import { PersonGetViewModel } from '../../models/person-view-model';
import { PersonUpdateViewModel } from '../../models/person-view-model';

describe('PersonEditorComponent', () => {
  let component: PersonEditorComponent;
  let fixture: ComponentFixture<PersonEditorComponent>;
  let personServiceSpy: jasmine.SpyObj<PersonService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    personServiceSpy = jasmine.createSpyObj('PersonService', ['getById', 'updatePerson']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const activatedRouteStub = { snapshot: { paramMap: { get: () => '1' } } };

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [PersonEditorComponent],
      providers: [
        { provide: PersonService, useValue: personServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: activatedRouteStub }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonEditorComponent);
    component = fixture.componentInstance;
  });

  it('should create the component and initialize the form with default values', () => {
    // Return an object with empty values to simulate defaults
    const defaultPerson: PersonGetViewModel = {
      id: 1,
      firstName: '',
      lastName: '',
      dateOfBirth: '',
      department: { id: null as any, departmentName: '' }
    };
    personServiceSpy.getById.and.returnValue(of(defaultPerson));
    fixture.detectChanges();
    expect(component).toBeTruthy();
    expect(component.personForm.get('firstName')?.value).toBe('');
    expect(component.personForm.get('lastName')?.value).toBe('');
    expect(component.personForm.get('dateOfBirth')?.value).toBe('');
    expect(component.personForm.get('departmentId')?.value).toBe(null);
  });

  it('should fetch person data and populate the form (loadPerson)', () => {
    const mockPerson: PersonGetViewModel = {
      id: 1,
      firstName: 'Alice',
      lastName: 'Smith',
      dateOfBirth: '1990-01-01',
      department: { id: 1, departmentName: 'Sales' }
    };
    personServiceSpy.getById.and.returnValue(of(mockPerson));
    fixture.detectChanges();
    expect(personServiceSpy.getById).toHaveBeenCalledWith(1);
    expect(component.personForm.get('firstName')?.value).toBe(mockPerson.firstName);
    expect(component.personForm.get('lastName')?.value).toBe(mockPerson.lastName);
    expect(component.personForm.get('dateOfBirth')?.value).toBe(mockPerson.dateOfBirth);
    expect(component.personForm.get('departmentId')?.value).toBe(mockPerson.department.id);
  });

  it('should call updatePerson and navigate after form submit (onSubmit)', () => {
    // Simulate initial empty person returned by getById
    personServiceSpy.getById.and.returnValue(
      of({
        id: 1,
        firstName: '',
        lastName: '',
        dateOfBirth: '',
        department: { id: null as any, departmentName: '' }
      } as PersonGetViewModel)
    );
    fixture.detectChanges();
    // Set the form values for update
    component.personForm.setValue({
      id: 1,
      firstName: 'Bob',
      lastName: 'Johnson',
      dateOfBirth: '1985-05-05',
      departmentId: 2
    });
    const expectedUpdate: PersonUpdateViewModel = {
      id: 1,
      firstName: 'Bob',
      lastName: 'Johnson',
      dateOfBirth: '1985-05-05',
      departmentId: 2
    };
    const updatedPerson: PersonGetViewModel = {
      id: 1,
      firstName: 'Bob',
      lastName: 'Johnson',
      dateOfBirth: '1985-05-05',
      department: { id: 2, departmentName: 'Marketing' }
    };
    personServiceSpy.updatePerson.and.returnValue(of(updatedPerson));
    component.onSubmit();
    expect(personServiceSpy.updatePerson).toHaveBeenCalledWith(expectedUpdate);
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/']);
  });
});
