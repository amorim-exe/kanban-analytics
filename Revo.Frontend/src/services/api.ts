import axios from 'axios';

const API_BASE_URL = 'http://localhost:5149/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export interface Project {
  id: string;
  name: string;
  description: string;
  color: string;
  isArchived: boolean;
  order: number;
  createdAt: string;
  updatedAt: string;
  taskCount: number;
  completedTaskCount: number;
}

export interface Task {
  id: string;
  title: string;
  description: string;
  status: 'Todo' | 'InProgress' | 'Review' | 'Done';
  priority: 'Low' | 'Medium' | 'High' | 'Critical';
  order: number;
  dueDate: string | null;
  completedAt: string | null;
  projectId: string;
  assignedToId: string | null;
  estimatedHours: number;
  actualHours: number;
  createdAt: string;
  updatedAt: string;
  commentCount: number;
  attachmentCount: number;
}

export const projectService = {
  getAll: async () => {
    const response = await api.get<Project[]>('/projects');
    return response.data;
  },
  
  getById: async (id: string) => {
    const response = await api.get<Project>(`/projects/${id}`);
    return response.data;
  },
  
  create: async (data: { name: string; description: string; color: string }) => {
    const response = await api.post<Project>('/projects', data);
    return response.data;
  },
  
  update: async (id: string, data: { name: string; description: string; color: string }) => {
    const response = await api.put<Project>(`/projects/${id}`, data);
    return response.data;
  },
  
  delete: async (id: string) => {
    await api.delete(`/projects/${id}`);
  },
};

export const taskService = {
  getAll: async (projectId?: string) => {
    const params = projectId ? { projectId } : {};
    const response = await api.get<Task[]>('/tasks', { params });
    return response.data;
  },
  
  getById: async (id: string) => {
    const response = await api.get<Task>(`/tasks/${id}`);
    return response.data;
  },
  
  create: async (data: {
    title: string;
    description: string;
    priority: string;
    dueDate: string | null;
    estimatedHours: number;
    projectId: string;
    assignedToId: string | null;
  }) => {
    const response = await api.post<Task>('/tasks', data);
    return response.data;
  },
  
  update: async (id: string, data: {
    title: string;
    description: string;
    status: string;
    priority: string;
    dueDate: string | null;
    estimatedHours: number;
  }) => {
    const response = await api.put<Task>(`/tasks/${id}`, data);
    return response.data;
  },
  
  move: async (id: string, newStatus: string, newOrder: number) => {
    const response = await api.post<Task>(`/tasks/${id}/move`, { newStatus, newOrder });
    return response.data;
  },
  
  logHours: async (id: string, hours: number) => {
    const response = await api.post<Task>(`/tasks/${id}/log-hours`, { hours });
    return response.data;
  },
  
  delete: async (id: string) => {
    await api.delete(`/tasks/${id}`);
  },
  
  getOverdue: async () => {
    const response = await api.get<Task[]>('/tasks/overdue');
    return response.data;
  },
};

export default api;
