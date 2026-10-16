import { useState, useEffect } from 'react';
import { KanbanBoard } from './components/KanbanBoard';
import { Dashboard } from './components/Dashboard';
import { projectService, taskService } from './services/api';
import type { Project, Task } from './services/api';
import { Button } from './components/ui/button';
import { LayoutDashboard, KanbanSquare, Plus, Settings } from 'lucide-react';

function App() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [view, setView] = useState<'dashboard' | 'kanban'>('dashboard');
  const [selectedProject, setSelectedProject] = useState<string | null>(null);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [projectsData, tasksData] = await Promise.all([
        projectService.getAll(),
        taskService.getAll(),
      ]);
      setProjects(projectsData);
      setTasks(tasksData);
    } catch (error) {
      console.error('Error loading data:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleTaskMove = async (taskId: string, newStatus: string, newOrder: number) => {
    try {
      await taskService.move(taskId, newStatus, newOrder);
      const updatedTasks = await taskService.getAll(selectedProject || undefined);
      setTasks(updatedTasks);
    } catch (error) {
      console.error('Error moving task:', error);
    }
  };

  const filteredTasks = selectedProject
    ? tasks.filter(t => t.projectId === selectedProject)
    : tasks;

  if (loading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Carregando...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100">
      {/* Header */}
      <header className="bg-white border-b shadow-sm sticky top-0 z-10">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between h-16">
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 bg-gradient-to-br from-primary-500 to-primary-600 rounded-lg flex items-center justify-center">
                <KanbanSquare className="w-6 h-6 text-white" />
              </div>
              <h1 className="text-2xl font-bold bg-gradient-to-r from-primary-600 to-primary-800 bg-clip-text text-transparent">
                Revo
              </h1>
            </div>
            
            <div className="flex items-center gap-2">
              <Button
                variant={view === 'dashboard' ? 'default' : 'ghost'}
                size="sm"
                onClick={() => setView('dashboard')}
              >
                <LayoutDashboard className="w-4 h-4 mr-2" />
                Dashboard
              </Button>
              <Button
                variant={view === 'kanban' ? 'default' : 'ghost'}
                size="sm"
                onClick={() => setView('kanban')}
              >
                <KanbanSquare className="w-4 h-4 mr-2" />
                Kanban
              </Button>
              <Button variant="ghost" size="sm">
                <Settings className="w-4 h-4" />
              </Button>
            </div>
          </div>
        </div>
      </header>

      {/* Project Selector */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
        <div className="flex items-center gap-2 overflow-x-auto pb-2">
          <Button
            variant={selectedProject === null ? 'default' : 'outline'}
            size="sm"
            onClick={() => setSelectedProject(null)}
          >
            Todos os Projetos
          </Button>
          {projects.map((project) => (
            <Button
              key={project.id}
              variant={selectedProject === project.id ? 'default' : 'outline'}
              size="sm"
              onClick={() => setSelectedProject(project.id)}
              className="flex items-center gap-2"
            >
              <div
                className="w-3 h-3 rounded-full"
                style={{ backgroundColor: project.color }}
              />
              {project.name}
            </Button>
          ))}
          <Button variant="outline" size="sm">
            <Plus className="w-4 h-4 mr-2" />
            Novo Projeto
          </Button>
        </div>
      </div>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto">
        {view === 'dashboard' ? (
          <Dashboard tasks={filteredTasks} projects={projects} />
        ) : (
          <KanbanBoard tasks={filteredTasks} onTaskMove={handleTaskMove} />
        )}
      </main>
    </div>
  );
}

export default App;
