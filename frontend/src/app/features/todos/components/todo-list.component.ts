import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TodoService } from '../../../core/services/todo.service';
import { Todo } from '../models/todo.model';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent implements OnInit {
  // full list of todos — signal so the template re-renders on every change
  todos = signal<Todo[]>([]);

  // bound to the "add task" input via [(ngModel)]
  newTitle = '';

  // controls the loading spinner visibility
  isLoading = signal(false);

  // holds the current error message shown in the banner (empty = no banner)
  errorMessage = signal('');

  // id of the todo currently being edited (null = none in edit mode)
  editingId = signal<string | null>(null);

  // temporary text held while the user edits — discarded on cancel
  editingTitle = '';

  constructor(private todoService: TodoService) {}

  // lifecycle hook — runs once after the component is created
  ngOnInit(): void {
    this.loadTodos();
  }

  // fetches all todos from the API and populates the signal
  loadTodos(): void {
    this.isLoading.set(true);
    this.todoService.getAll().subscribe({
      next: (todos) => {
        this.todos.set(todos);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Error loading tasks. Make sure the API is running.');
        this.isLoading.set(false);
      }
    });
  }

  // creates a new todo and prepends it to the list without reloading
  addTodo(): void {
    const title = this.newTitle.trim();
    if (!title) return;

    this.todoService.create(title).subscribe({
      next: (todo) => {
        this.todos.update(todos => [todo, ...todos]);
        this.newTitle = '';
      },
      error: () => this.errorMessage.set('Error creating task.')
    });
  }

  // flips isCompleted and updates only the affected item in the signal
  toggleComplete(todo: Todo): void {
    this.todoService.update(todo.id, {
      task: todo.task,
      isCompleted: !todo.isCompleted
    }).subscribe({
      next: (updated) => {
        this.todos.update(todos =>
          todos.map(t => t.id === updated.id ? updated : t)
        );
      },
      error: () => this.errorMessage.set('Error updating task.')
    });
  }

  // enters edit mode for the given todo and copies its current title
  startEdit(todo: Todo): void {
    this.editingId.set(todo.id);
    this.editingTitle = todo.task;
  }

  // saves the edit only if the title actually changed; cancels otherwise
  saveEdit(todo: Todo): void {
    const title = this.editingTitle.trim();

    // skip API call if title is empty or unchanged
    if (!title || title === todo.task) {
      this.cancelEdit();
      return;
    }

    this.todoService.update(todo.id, {
      task: title,
      isCompleted: todo.isCompleted
    }).subscribe({
      next: (updated) => {
        this.todos.update(todos =>
          todos.map(t => t.id === updated.id ? updated : t)
        );
        this.cancelEdit();
      },
      error: () => this.errorMessage.set('Error updating task.')
    });
  }

  // exits edit mode and discards any unsaved changes
  cancelEdit(): void {
    this.editingId.set(null);
    this.editingTitle = '';
  }

  // keyboard shortcuts while editing: Enter = save, Escape = cancel
  handleEditKeydown(event: KeyboardEvent, todo: Todo): void {
    if (event.key === 'Enter') this.saveEdit(todo);
    if (event.key === 'Escape') this.cancelEdit();
  }

  // removes the todo from the API then filters it out of the local signal
  deleteTodo(id: string): void {
    this.todoService.delete(id).subscribe({
      next: () => {
        this.todos.update(todos => todos.filter(t => t.id !== id));
      },
      error: () => this.errorMessage.set('Error deleting task.')
    });
  }

  // computed counts used in the header stats
  get pendingCount(): number {
    return this.todos().filter(t => !t.isCompleted).length;
  }

  get completedCount(): number {
    return this.todos().filter(t => t.isCompleted).length;
  }

  // triggers addTodo when the user presses Enter in the main input
  handleKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter') this.addTodo();
  }

  // clears the error banner
  dismissError(): void {
    this.errorMessage.set('');
  }
}
