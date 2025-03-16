import { Component, OnInit } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { PersonGetViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-person-list',
  standalone: false,
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss']
})
export class PersonListComponent implements OnInit {
  people: PersonGetViewModel[] = [];

  constructor(private personService: PersonService) { }

  ngOnInit(): void {
    this.personService.getAll().subscribe(
      (data: PersonGetViewModel[]) => {
        this.people = data;
      },
      error => {
        console.error('Error fetching people:', error);
      }
    );
  }
}
