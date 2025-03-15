import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-person-add',
  templateUrl: './person-add.component.html',
  styleUrls: ['./person-add.component.scss']
})
export class PersonAddComponent implements OnInit {
  personForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private personService: PersonService
  ) {
    this.personForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required]
    });
  }

  ngOnInit(): void {

  }

  onSubmit(): void {
    if (this.personForm.valid) {
      this.personService.addPerson(this.personForm.value).subscribe(
        (newPerson: PersonViewModel) => {
          this.router.navigate(['/']);
        },
        error => {
          console.error('Error adding person', error);
        }
      );
    }
  }
}
