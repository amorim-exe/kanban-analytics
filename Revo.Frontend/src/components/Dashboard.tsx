import { Card, CardContent, CardHeader, CardTitle } from './ui/card';
import type { Task, Project } from '@/services/api';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';
import { CheckCircle, Clock, AlertTriangle, TrendingUp } from 'lucide-react';

interface DashboardProps {
  tasks: Task[];
  projects: Project[];
}

const COLORS = ['#3b82f6', '#f59e0b', '#8b5cf6', '#10b981'];

export function Dashboard({ tasks, projects }: DashboardProps) {
  const totalTasks = tasks.length;
  const completedTasks = tasks.filter(t => t.status === 'Done').length;
  const inProgressTasks = tasks.filter(t => t.status === 'InProgress').length;
  const overdueTasks = tasks.filter(t => 
    t.dueDate && new Date(t.dueDate) < new Date() && t.status !== 'Done'
  ).length;
  const completionRate = totalTasks > 0 ? (completedTasks / totalTasks) * 100 : 0;

  const tasksByStatus = [
    { name: 'A Fazer', value: tasks.filter(t => t.status === 'Todo').length, color: COLORS[0] },
    { name: 'Em Progresso', value: tasks.filter(t => t.status === 'InProgress').length, color: COLORS[1] },
    { name: 'Em Revisão', value: tasks.filter(t => t.status === 'Review').length, color: COLORS[2] },
    { name: 'Concluído', value: tasks.filter(t => t.status === 'Done').length, color: COLORS[3] },
  ];

  const tasksByPriority = [
    { name: 'Baixa', value: tasks.filter(t => t.priority === 'Low').length },
    { name: 'Média', value: tasks.filter(t => t.priority === 'Medium').length },
    { name: 'Alta', value: tasks.filter(t => t.priority === 'High').length },
    { name: 'Crítica', value: tasks.filter(t => t.priority === 'Critical').length },
  ];

  const projectStats = projects.map(p => ({
    name: p.name,
    completed: p.completedTaskCount,
    total: p.taskCount,
    rate: p.taskCount > 0 ? (p.completedTaskCount / p.taskCount) * 100 : 0,
  }));

  return (
    <div className="space-y-6 p-6 animate-fade-in">
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <Card className="bg-gradient-to-br from-blue-500 to-blue-600 text-white border-0">
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-blue-100">Total de Tarefas</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center justify-between">
              <div className="text-3xl font-bold">{totalTasks}</div>
              <TrendingUp className="w-8 h-8 opacity-50" />
            </div>
          </CardContent>
        </Card>

        <Card className="bg-gradient-to-br from-green-500 to-green-600 text-white border-0">
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-green-100">Concluídas</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center justify-between">
              <div className="text-3xl font-bold">{completedTasks}</div>
              <CheckCircle className="w-8 h-8 opacity-50" />
            </div>
            <p className="text-xs text-green-100 mt-2">{completionRate.toFixed(1)}% de conclusão</p>
          </CardContent>
        </Card>

        <Card className="bg-gradient-to-br from-yellow-500 to-yellow-600 text-white border-0">
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-yellow-100">Em Progresso</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center justify-between">
              <div className="text-3xl font-bold">{inProgressTasks}</div>
              <Clock className="w-8 h-8 opacity-50" />
            </div>
          </CardContent>
        </Card>

        <Card className="bg-gradient-to-br from-red-500 to-red-600 text-white border-0">
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium text-red-100">Atrasadas</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center justify-between">
              <div className="text-3xl font-bold">{overdueTasks}</div>
              <AlertTriangle className="w-8 h-8 opacity-50" />
            </div>
          </CardContent>
        </Card>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card>
          <CardHeader>
            <CardTitle>Tarefas por Status</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={tasksByStatus}
                  cx="50%"
                  cy="50%"
                  labelLine={false}
label={({ name, percent }) =>
  `${name} ${((percent ?? 0) * 100).toFixed(0)}%`
}                  outerRadius={80}
                  fill="#8884d8"
                  dataKey="value"
                >
                  {tasksByStatus.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Tarefas por Prioridade</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={tasksByPriority}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip />
                <Bar dataKey="value" fill="#3b82f6" radius={[8, 8, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Progresso por Projeto</CardTitle>
        </CardHeader>
        <CardContent>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={projectStats}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="name" />
              <YAxis />
              <Tooltip />
              <Bar dataKey="completed" fill="#10b981" name="Concluídas" radius={[8, 8, 0, 0]} />
              <Bar dataKey="total" fill="#3b82f6" name="Total" radius={[8, 8, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </CardContent>
      </Card>
    </div>
  );
}
