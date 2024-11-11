import { useState, useEffect } from "react";
import { CreateTodo } from "./CreateTodo";
import { getTodos } from "./api";
import { TodoList, TodoState } from "./TodoList";

interface TodosGrouped {
	pending: TodoDetailsState[];
	overdue: TodoDetailsState[];
}

async function fetchTodosGrouped(): Promise<TodosGrouped> {
	const todos = (await getTodos()).map(t => ({ ...t, dueDate: t.dueDate ? new Date(t.dueDate) : null }));
	const now = new Date();
	const pending = todos.filter(t => !t.dueDate || t.dueDate >= now).map(t => ({...t, status: TodoState.Pending }));
	const overdue = todos.filter(t => t.dueDate && t.dueDate < now).map(t => ({...t, status: TodoState.Overdue }));

	return { pending, overdue };
}

export function Todos() {
	const [todosGrouped, setTodosGrouped] = useState<TodosGrouped | null>(null);

	useEffect(() => { getTodosGrouped(); }, []);

	function getTodosGrouped() { fetchTodosGrouped().then(setTodosGrouped); }

	return (
		<div className="d-flex justify-content-center flex-column">
			<h1 className="mb-5">TODO List</h1>

			<div className="w-100 my-2">
				<h2>Create</h2>
				<hr />
				<CreateTodo onTodoCreated={getTodosGrouped} />
			</div>
			<div className="w-100 my-2">
				<h2>Overdue</h2>
				<hr />
				<TodoList todos={todosGrouped?.overdue} onTodoUpdated={getTodosGrouped} />
			</div>
			<div className="w-100 my-2">
				<h2>Pending</h2>
				<hr />
				<TodoList todos={todosGrouped?.pending} onTodoUpdated={getTodosGrouped} />
			</div>
		</div>
	);
}