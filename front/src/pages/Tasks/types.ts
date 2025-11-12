export interface Task {
    id: string;
    courseId: string;
    order: number;
    title: string;
    text: string;
    isUnlocked: boolean;
    minListItemsCount?: number;
    maxListItemsCount?: number;
    type: TaskType;
    isCompleted: boolean;
    results: TaskResult[] | null;
}

export interface TaskResult {
    id: string;
    taskId: string;
    text: string;
    listItems: string[];
}

export interface CreateTaskResultDto {
    taskId: string;
    text?: string;
    listItems?: string[];
}

export enum TaskType
{
    Text = 10,
    TextList = 20,
}