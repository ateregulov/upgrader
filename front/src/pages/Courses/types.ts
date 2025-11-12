export interface Course {
    id: string;
    title: string;
    shortDescription: string;
    longDescription: string;
    price: number;
    isBought: boolean;
    tasksCount: number;
    finishedTasksCount: number
}

export interface CoursePaymentInfo {
    price: number;
    balance: number;
}