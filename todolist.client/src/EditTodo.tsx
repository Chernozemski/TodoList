import { useState, useMemo } from "react";
import { nameof, toISODateTimeLocal } from "./utils";


interface EditTodoProps {
	initialState: EditTodoModel;
	onTodoEdited: (todo: EditTodoModel) => Promise<void>;
	submitText: string;
}

export interface EditTodoModel {
	title: string;
	dueDate?: Date | null;
}

function getMinMaxDates()
{
	const yesterday = new Date();
	yesterday.setDate(yesterday.getDate() - 1);
	const yearFromNow = new Date();
	yearFromNow.setFullYear(yearFromNow.getFullYear() + 1);

	return [toISODateTimeLocal(yesterday), toISODateTimeLocal(yearFromNow)];
}

export function EditTodo({initialState, onTodoEdited, submitText}: EditTodoProps) {
	const copiedState = useMemo(() => ({...initialState}), [initialState]);
	const [todo, setTodo] = useState<EditTodoModel>(copiedState);
	const [isSending, setIsSending] = useState<boolean>(false);
	const [minDate, maxDate] = useMemo(getMinMaxDates, []);

	const handleChange = (e: React.OnChangeEvent<HTMLInputElement> | React.OnBlurEvent<HTMLInputElement>) => {
		const {name, value} = e.target;
		const isChange = e.type === 'change';
		const isDueDate = name === nameof<EditTodoModel>('dueDate');

		if (isDueDate && isNaN(new Date(value)))
			return;

		const actualValue = isDueDate
						? new Date(value)
						: isChange ? value : value.trim();

		setTodo((prev) => ({ ...prev, [name]: actualValue }));
	};

	const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
		e.preventDefault();
		setIsSending(true);

		await onTodoEdited(todo);

		setIsSending(false);
	};

	return (
		<form onSubmit={handleSubmit}>
			<div className="mb-3">
				<label htmlFor="todo-title" className="form-label">Title</label>
				<input
					className="form-control"
					id="todo-title"
					name={nameof<EditTodoModel>('title')}
					value={todo.title}
					onChange={handleChange}
					onBlur={handleChange}
					placeholder="Title"
					minLength={2}
					maxLength={200}
					required
				/>
			</div>
			<div className="mb-3">
				<label htmlFor="todo-date" className="form-label">Due date</label>
				<input
					className="form-control"
					type="datetime-local"
					id="todo-date"
					name={nameof<EditTodoModel>('dueDate')}
					value={toISODateTimeLocal(todo.dueDate) ?? ''}
					onChange={handleChange}
					onBlur={handleChange}
					placeholder="Due date"
					min={minDate}
					max={maxDate}
				/>
			</div>

			<div className="w-100 text-end">
				<button className="btn btn-primary" type="submit" disabled={isSending}>{submitText}</button>
			</div>
		</form>
	);
}