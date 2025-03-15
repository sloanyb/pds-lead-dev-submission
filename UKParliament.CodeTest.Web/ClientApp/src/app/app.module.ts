import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { InstructionsComponent } from './components/instructions/instructions.component';
import { PersonListComponent } from './components/person-list/person-list.component';
import { PersonEditorComponent } from './components/person-editor/person-editor.component';

@NgModule({ declarations: [
        AppComponent,
        InstructionsComponent,
        PersonListComponent,
        PersonEditorComponent
    ],
    bootstrap: [AppComponent], imports: [BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: PersonListComponent, pathMatch: 'full' },
            { path: 'person/edit/:id', component: PersonEditorComponent },
            { path: 'instructions', component: InstructionsComponent, pathMatch: 'full' }
        ])], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
