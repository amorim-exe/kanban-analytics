@echo off
echo ========================================
echo Revo - Sistema de Gestao de Projetos
echo ========================================
echo.

echo [1/4] Iniciando infraestrutura (Docker)...
docker-compose up -d
if %errorlevel% neq 0 (
    echo Erro ao iniciar Docker. Verifique se o Docker esta rodando.
    pause
    exit /b 1
)
echo Infraestrutura iniciada com sucesso!
echo.

echo [2/4] Restaurando pacotes .NET...
cd Revo.API
dotnet restore
if %errorlevel% neq 0 (
    echo Erro ao restaurar pacotes .NET.
    pause
    exit /b 1
)
echo Pacotes .NET restaurados com sucesso!
echo.

echo [3/4] Instalando dependencias Node.js...
cd ..\Revo.Frontend
call npm install
if %errorlevel% neq 0 (
    echo Erro ao instalar dependencias Node.js.
    pause
    exit /b 1
)
echo Dependencias Node.js instaladas com sucesso!
echo.

echo [4/4] Build do projeto...
cd ..\Revo.API
dotnet build
if %errorlevel% neq 0 (
    echo Erro ao buildar projeto .NET.
    pause
    exit /b 1
)
echo Build concluido com sucesso!
echo.

echo ========================================
echo Setup concluido com sucesso!
echo ========================================
echo.
echo Para iniciar o backend:
echo   cd Revo.API && dotnet run
echo.
echo Para iniciar o frontend:
echo   cd Revo.Frontend && npm run dev
echo.
echo API: http://localhost:5000
echo Frontend: http://localhost:5173
echo Swagger: http://localhost:5000/swagger
echo.
pause
