import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import type { Task } from '@/services/api';
import { Card } from './ui/card';
import { Badge } from './ui/badge';
import { Clock, AlertCircle } from 'lucide-react';

interface SortableTaskProps {
  task: Task;
}

const PRIORITY_COLORS = {
  Low: 'bg-gray-100 text-gray-700',
  Medium: 'bg-blue-100 text-blue-700',
  High: 'bg-orange-100 text-orange-700',
  Critical: 'bg-red-100 text-red-700',
};

export function SortableTask({ task }: SortableTaskProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: task.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  const isOverdue = task.dueDate && new Date(task.dueDate) < new Date() && task.status !== 'Done';

  return (
    <div ref={setNodeRef} style={style} {...attributes} {...listeners}>
      <Card className="p-4 cursor-grab active:cursor-grabbing hover:shadow-md transition-shadow">
        <div className="space-y-3">
          <div className="flex items-start justify-between gap-2">
            <h4 className="font-medium text-sm line-clamp-2">{task.title}</h4>
            <Badge className={`text-xs ${PRIORITY_COLORS[task.priority]}`}>
              {task.priority}
            </Badge>
          </div>
          
          {task.description && (
            <p className="text-xs text-gray-600 line-clamp-2">{task.description}</p>
          )}
          
          <div className="flex items-center gap-3 text-xs text-gray-500">
            {task.dueDate && (
              <div className={`flex items-center gap-1 ${isOverdue ? 'text-red-600' : ''}`}>
                {isOverdue ? <AlertCircle className="w-3 h-3" /> : <Clock className="w-3 h-3" />}
                <span>{new Date(task.dueDate).toLocaleDateString('pt-BR')}</span>
              </div>
            )}
            
            {task.estimatedHours > 0 && (
              <div className="flex items-center gap-1">
                <Clock className="w-3 h-3" />
                <span>{task.estimatedHours}h</span>
              </div>
            )}
          </div>

          <div className="flex items-center justify-between text-xs text-gray-500 pt-2 border-t">
            <div className="flex items-center gap-2">
              {task.commentCount > 0 && (
                <span className="flex items-center gap-1">
                  💬 {task.commentCount}
                </span>
              )}
              {task.attachmentCount > 0 && (
                <span className="flex items-center gap-1">
                  📎 {task.attachmentCount}
                </span>
              )}
            </div>
            {task.actualHours > 0 && (
              <span className="text-green-600 font-medium">{task.actualHours}h</span>
            )}
          </div>
        </div>
      </Card>
    </div>
  );
}
