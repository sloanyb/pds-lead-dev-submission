// Disclaimer: ChatGPT used to assist me to generate this test, as I am not very experienced with frontend testing frameworks.

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { PersonEditorComponent } from './person-editor.component';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';

describe('PersonEditorComponent', () => {
  let component: PersonEditorComponent;
  let fixture: ComponentFixture<PersonEditorComponent>;
  let personServiceSpy: jasmine.SpyObj<PersonService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    personServiceSpy = jasmine.createSpyObj('PersonService', ['getById', 'updatePerson']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    personServiceSpy.getById.and.returnValue(of({ id: 1, firstName: '', lastName: '' }));

    const activatedRouteStub = {
      snapshot: { paramMap: { get: () => '1' } }
    };

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

  it('should create the component', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    personServiceSpy.getById.and.returnValue(of({ id: 1, firstName: '', lastName: '' }));
    fixture.detectChanges();

    expect(component.personForm).toBeTruthy();
    expect(component.personForm.get('firstName')!.value).toBe('');
    expect(component.personForm.get('lastName')!.value).toBe('');
  });

  it('should fetch person data and populate the form (loadPerson)', () => {
    const mockPerson: PersonViewModel = { id: 1, firstName: 'Alice', lastName: 'Smith' };
    personServiceSpy.getById.and.returnValue(of(mockPerson));

    fixture.detectChanges();

    expect(personServiceSpy.getById).toHaveBeenCalledWith(1);
    expect(component.personForm.get('firstName')!.value).toBe(mockPerson.firstName);
    expect(component.personForm.get('lastName')!.value).toBe(mockPerson.lastName);
  });

  it('should call updatePerson and navigate after form submit (onSubmit)', () => {
    personServiceSpy.getById.and.returnValue(of({ id: 1, firstName: '', lastName: '' }));
    fixture.detectChanges();

    component.personForm.setValue({ id: 1, firstName: 'Bob', lastName: 'Johnson' });
    const expectedUpdate: PersonViewModel = { id: 1, firstName: 'Bob', lastName: 'Johnson' };

    personServiceSpy.updatePerson.and.returnValue(of(expectedUpdate));

    component.onSubmit();

    expect(personServiceSpy.updatePerson).toHaveBeenCalledWith(expectedUpdate);
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/']);
  });
});
