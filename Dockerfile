# ==============================================================================
# Etapa 1: Compilación de Frontend (Tailwind CSS v4 con pnpm)
# ==============================================================================
FROM node:22-alpine AS frontend
WORKDIR /src/frontend

# Habilitar pnpm versión fijada
RUN npm install -g pnpm@12.11.2

# Copiar manifiestos de dependencias para aprovechar caché
COPY FinanceHorizon/package.json FinanceHorizon/pnpm-lock.yaml FinanceHorizon/pnpm-workspace.yaml* ./
RUN pnpm config set dangerously-allow-all-builds true && pnpm install --frozen-lockfile

# Copiar estilos y vistas para que el motor de Tailwind analice las clases CSS
COPY FinanceHorizon/Styles ./Styles
COPY FinanceHorizon/Views ./Views
COPY FinanceHorizon/Areas ./Areas
COPY FinanceHorizon/ViewComponents ./ViewComponents
COPY FinanceHorizon/wwwroot ./wwwroot

# Compilar CSS minificado para producción
RUN pnpm run build:css

# ==============================================================================
# Etapa 2: Compilación y Publicación de .NET 10
# ==============================================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias NuGet (cacheable)
COPY ["Domine/Domine.csproj", "Domine/"]
COPY ["Aplication/Aplication.csproj", "Aplication/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["FinanceHorizon/FinanceHorizon.csproj", "FinanceHorizon/"]

RUN dotnet restore "FinanceHorizon/FinanceHorizon.csproj" -r linux-x64

# Copiar el resto del código fuente
COPY Domine/ Domine/
COPY Aplication/ Aplication/
COPY Infrastructure/ Infrastructure/
COPY FinanceHorizon/ FinanceHorizon/

# Inyectar el CSS compilado en la etapa frontend
COPY --from=frontend /src/frontend/wwwroot/css/tailwind.css ./FinanceHorizon/wwwroot/css/tailwind.css

# Publicar los binarios de la aplicación optimizados para producción
RUN dotnet publish "FinanceHorizon/FinanceHorizon.csproj" \
    -c Release \
    -o /app/publish \
    -r linux-x64 \
    --self-contained false \
    -p:SkipTailwind=true \
    --no-restore

# ==============================================================================
# Etapa 3: Runtime de Producción
# ==============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Puerto estándar de ASP.NET Core en .NET 8/9/10
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

# Copiar la aplicación publicada
COPY --from=build /app/publish .

# Crear directorios para almacenamiento local de imágenes y documentos
RUN mkdir -p /app/wwwroot/uploads /app/storage/documents

ENTRYPOINT ["dotnet", "FinanceHorizon.dll"]
