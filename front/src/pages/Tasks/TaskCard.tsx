import { Task } from './types'

interface TaskCardProps {
  task: Task
  onClick?: (taskId: string) => void
}

function TaskCard({ task, onClick }: TaskCardProps) {
  return (
    <div
      key={task.id}
      onClick={() => task.isUnlocked && onClick?.(task.id)}
      className={`flex-shrink-0 w-64 p-4 rounded-lg transition-colors cursor-pointer
        ${task.isUnlocked ? 'bg-gray-800 hover:bg-gray-700' : 'bg-gray-700 opacity-50 cursor-not-allowed'}`}
    >
      <h2 className='text-white font-semibold mb-2'>{task.title}</h2>
      <p className='text-gray-300 text-sm mb-2'>{task.text}</p>

      {task.isCompleted && (
        <span className='inline-block mt-2 text-xs text-green-400 font-semibold'>
          ✔ Пройдено
        </span>
      )}
    </div>
  )
}

export default TaskCard
