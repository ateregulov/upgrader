import { Task } from './types'

interface TaskCardProps {
  task: Task
}

function TaskCard({ task }: TaskCardProps) {
  return (
    <div
      key={task.id}
      className={`flex-shrink-0 w-64 p-4 rounded-lg transition-colors cursor-pointer 
        ${task.isUnlocked ? 'bg-gray-800 hover:bg-gray-700' : 'bg-gray-700 opacity-50 cursor-not-allowed'}`}
    >
      <h2 className='font-semibold mb-2'>{task.title}</h2>
      <p className='text-gray-300 text-sm'>{task.text}</p>
    </div>
  )
}

export default TaskCard
