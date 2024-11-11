import { useState } from "react";
import { EditTodo, EditTodoModel } from "./EditTodo";
import { TodoDetails, UpdateTodo, markAsDone, updateTodo } from "./api";
import { getHumanTime } from "./utils";

export enum TodoState {
	Pending,
	Overdue
}
export interface TodoDetailsState extends TodoDetails {
	status: TodoState
}

export interface TodoListProps {
	todos: TodoDetailsState[];
	onTodoUpdated: () => void;
}

interface EditTodoWithId extends EditTodoModel {
	id: number;
}

export function TodoList({ todos, onTodoUpdated }: TodoListProps) {
	const [editingTodoId, setEditingTodoId] = useState<number>(-1);
	const now = new Date();

	const onUpdateTodo = async (todo: EditTodoModel) => {
		try {
			const edit = todo as EditTodoWithId;
			const updateTodoRequest: UpdateTodo = {
				id: edit.id,
				title: edit.title,
				dueDate: edit.dueDate
			};

			const result = await updateTodo(updateTodoRequest);
			if (result)
				onTodoUpdated();

			else
				alert('Failed to update todo');
		} catch (error) {
			console.error(error);
			alert('Failed to update todo');
		}

		setEditingTodoId(-1);
	};

	const handleCheckboxChange = async (id: number) => {
		try {
			const result = await markAsDone(id);
			if (result)
				onTodoUpdated();

			else
				alert('Failed to mark todo as done');
		} catch (error) {
			console.error(error);
			alert('Failed to mark todo as done');
		}
	};

	if (!todos)
		return <div className="spinner-border"></div>;

	if (!todos.length)
		return <h4 className="my-4">No todos</h4>;

	return (
		<div className="list-group">
			{todos.map(todo => (
				<div key={todo.id} className="list-group-item d-flex">
					<input
						className="me-3"
						type="checkbox"
						onChange={() => handleCheckboxChange(todo.id)} />
					<div className="w-100">
						{editingTodoId === todo.id
							? (
								<EditTodo
									initialState={todo}
									onTodoEdited={onUpdateTodo}
									submitText="Update" />
							) : (
								<div style={{ cursor: 'pointer' }} className="w-100" onClick={() => setEditingTodoId(todo.id)}>
									<h4 className="w-100 py-4">{todo.title}</h4>
									{todo.status === TodoState.Overdue
										? <span className="text-danger">{getHumanTime(now, todo.dueDate!)}</span>
										: <small className="text-muted">{!todo.dueDate ? null : getHumanTime(now, todo.dueDate)}</small>}
								</div>
							)}
					</div>
				</div>
			))}
		</div>
	);
}