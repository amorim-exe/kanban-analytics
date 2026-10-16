
import { useState, useEffect } from 'react';
import {
  DndContext,
  DragOverlay,
} from '@dnd-kit/core';
import type {
  DragEndEvent,
  DragStartEvent,
} from '@dnd-kit/core';
import {
  SortableContext,
  verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import { SortableTask } from './SortableTask';
import type { Task } from '@/services/api';
import { useSignalR } from '@/hooks/useSignalR';

interface KanbanBoardProps {
  tasks: Task[];
  onTaskMove: (
    taskId: string,
    newStatus: Task['status'],
    newOrder: number
  ) => void | Promise<void>;
}

const COLUMNS = [
  { id: 'Todo', title: 'A Fazer', color: 'bg-blue-500' },
  { id: 'InProgress', title: 'Em Progresso', color: 'bg-yellow-500' },
  { id: 'Review', title: 'Em Revisão', color: 'bg-purple-500' },
  { id: 'Done', title: 'Concluído', color: 'bg-green-500' },
] satisfies { id: Task['status']; title: string; color: string }[];

export function KanbanBoard({ tasks, onTaskMove }: KanbanBoardProps) {
  const [activeTask, setActiveTask] = useState<Task | null>(null);
  const { connection } = useSignalR();

  const tasksByStatus: Record<Task['status'], Task[]> = {
    Todo: tasks.filter(t => t.status === 'Todo').sort((a, b) => a.order - b.order),
    InProgress: tasks.filter(t => t.status === 'InProgress').sort((a, b) => a.order - b.order),
    Review: tasks.filter(t => t.status === 'Review').sort((a, b) => a.order - b.order),
    Done: tasks.filter(t => t.status === 'Done').sort((a, b) => a.order - b.order),
  };

  useEffect(() => {
    if (!connection) return;

    const handleTaskMoved = (taskId: string, newStatus: string) => {
      console.log('Task moved:', taskId, newStatus);
    };

    connection.on('TaskMoved', handleTaskMoved);

    return () => {
      connection.off('TaskMoved', handleTaskMoved);
    };
  }, [connection]);

  const handleDragStart = (event: DragStartEvent) => {
    const task = tasks.find(t => t.id === String(event.active.id));
    setActiveTask(task ?? null);
  };

  const handleDragEnd = async (event: DragEndEvent) => {
    const { active, over } = event;
    setActiveTask(null);

    if (!over) return;

    const activeId = String(active.id);
    const overId = String(over.id);

    const draggedTask = tasks.find(t => t.id === activeId);
    if (!draggedTask) return;

    const overColumn = COLUMNS.find(col => col.id === overId);
    const overTask = tasks.find(t => t.id === overId);

    let newStatus: Task['status'] = draggedTask.status;
    let newOrder = draggedTask.order;

    if (overColumn) {
      newStatus = overColumn.id;
      const columnTasks = tasksByStatus[newStatus].filter(
        task => task.id !== activeId
      );
      newOrder = columnTasks.length;
    } else if (overTask) {
      newStatus = overTask.status;
      newOrder = overTask.order;
    }

    if (
      newStatus !== draggedTask.status ||
      newOrder !== draggedTask.order
    ) {
      await onTaskMove(activeId, newStatus, newOrder);
    }
  };

  return (
    <DndContext onDragStart={handleDragStart} onDragEnd={handleDragEnd}>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 p-6">
        {COLUMNS.map(column => (
          <div
            key={column.id}
            className="bg-gray-100 rounded-lg p-4 min-h-[500px]"
          >
            <div className="flex items-center gap-2 mb-4">
              <div className={`w-3 h-3 rounded-full ${column.color}`} />
              <h3 className="font-semibold text-gray-700">
                {column.title}
              </h3>
              <span className="ml-auto text-sm text-gray-500">
                {tasksByStatus[column.id].length}
              </span>
            </div>

            <SortableContext
              items={tasksByStatus[column.id].map(task => task.id)}
              strategy={verticalListSortingStrategy}
            >
              <div className="space-y-3">
                {tasksByStatus[column.id].map(task => (
                  <SortableTask key={task.id} task={task} />
                ))}
              </div>
            </SortableContext>
          </div>
        ))}
      </div>

      <DragOverlay>
        {activeTask ? (
          <div className="bg-white p-4 rounded-lg shadow-lg border-2 border-primary opacity-90">
            <p className="font-medium">{activeTask.title}</p>
          </div>
        ) : null}
      </DragOverlay>
    </DndContext>
  );
}
