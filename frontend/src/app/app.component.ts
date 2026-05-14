import { Component } from '@angular/core';
import { TodoListComponent } from './features/todos/components/todo-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TodoListComponent],
  template: `<app-todo-list />`
})
export class AppComponent {}
