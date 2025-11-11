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