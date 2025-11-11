export interface Task {
    id: string;
    courseId: string;
    order: number;
    title: string;
    text: string;
    isUnlocked: boolean;
}