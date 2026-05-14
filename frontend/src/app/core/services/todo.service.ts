import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Todo, CreateTodoDto, UpdateTodoDto } from '../../features/todos/models/todo.model';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private readonly apiUrl = `${environment.apiUrl}/todo`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Todo[]> {
    return this.http.get<Todo[]>(this.apiUrl);
  }

  create(task: string): Observable<Todo> {
  return this.http.post<Todo>(`${this.apiUrl}?Task=${task}`, null);
  }

  update(id: string, dto: UpdateTodoDto): Observable<Todo> {
    return this.http.put<Todo>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
