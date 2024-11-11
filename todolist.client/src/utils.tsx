export const nameof = (name: keyof T) => name;

export const toISODateTimeLocal = (date: Date | null) => date?.toISOString().slice(0, 16);

const hourFormat: Intl.DateTimeFormatOptions = { hour: '2-digit', minute: '2-digit' };

export const getHumanTime = (now: Date, date: Date) => {
	const isToday = now.getFullYear() === date.getFullYear() &&
					now.getMonth() === date.getMonth() &&
					now.getDate() === date.getDate();

	const localTime = date.toLocaleTimeString([], hourFormat);

	if (isToday)
		return localTime;

	const localDate = date.toLocaleDateString();
	return `${localDate} ${localTime}`;
}