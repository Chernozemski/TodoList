export interface CreateTodo {
	title: string;
	dueDate?: Date | null;
}

export interface UpdateTodo extends CreateTodo {
    id: number;
}

export interface TodoDetails {
	id: number;
	title: string;
	dueDate?: Date | null;
}

export const createTodo = (todo: CreateTodo) =>
    fetch('/todos', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(todo)
    })
	.then(r => r.ok);

export const updateTodo = (todo: UpdateTodo) =>
    fetch('/todos/' + todo.id, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(todo)
    })
	.then(r => r.ok);

export const getTodos = (): Promise<TodoDetails[]> => fetch('/todos').then(r => r.json());

export const markAsDone = (id: number) =>
    fetch('/todos/' + id + '/done', { method: 'PATCH' })
	.then(r => r.ok);
