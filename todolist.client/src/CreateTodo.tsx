import { useState } from "react";
import { createTodo } from "./api";
import { EditTodo, EditTodoModel } from "./EditTodo";

interface CreateTodoProps {
	onTodoCreated: () => void;
}

const initialState: EditTodoModel = {title: '', dueDate: null};

export function CreateTodo({ onTodoCreated }: CreateTodoProps) {
	const [todo, _] = useState(initialState);
	async function onCreateTodo(todo: EditTodoModel) {
		try {
			const result = await createTodo(todo);

			if (!result)
				alert('Failed to add todo');
			else
				onTodoCreated();
		} catch (error) {
			console.error(error);
			alert('Failed to add todo');
		}
	}

	return (
		<EditTodo
			initialState={todo}
			onTodoEdited={onCreateTodo}
			submitText="Add"
		/>
	);
}