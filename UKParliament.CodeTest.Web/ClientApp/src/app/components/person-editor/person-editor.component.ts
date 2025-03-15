import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.scss']
})

export class PersonEditorComponent implements OnInit {
  personForm: FormGroup;
  personId!: number;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private personService: PersonService
  ) {
    this.personForm = this.fb.group({
      id: [''],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.personId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadPerson();
  }

  loadPerson(): void {
    this.personService.getById(this.personId).subscribe(
      (person: PersonViewModel) => {
        this.personForm.patchValue(person);
      },
      error => {
        console.error('Error loading person', error);
      }
    );
  }

  onSubmit(): void {
    if (this.personForm.valid) {
      this.personService.updatePerson(this.personForm.value).subscribe(
        (updatedPerson: PersonViewModel) => {
          this.router.navigate(['/']);
        },
        error => {
          console.error('Error updating person', error);
        }
      );
    }
  }
}
