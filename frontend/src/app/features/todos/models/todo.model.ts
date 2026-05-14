export interface Todo {
  id: string;
  task: string;
  isCompleted: boolean;
  createdAt: string;
}

export interface CreateTodoDto {
  title: string;
}

export interface UpdateTodoDto {
  task: string;
  isCompleted: boolean;
}
