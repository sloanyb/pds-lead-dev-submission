import { Component, OnInit } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-person-list',
  standalone: false,
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss']  // Note: plural 'styleUrls' is expected
})
export class PersonListComponent implements OnInit {
  people: PersonViewModel[] = [];

  constructor(private personService: PersonService) { }

  ngOnInit(): void {
    this.personService.getAll().subscribe(
      (data: PersonViewModel[]) => {
        this.people = data;
      },
      error => {
        console.error('Error fetching people:', error);
      }
    );
  }
}
