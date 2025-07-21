FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY CourseDemo.sln ./
COPY CourseDemo/CourseDemo.Domain.csproj ./CourseDemo/
COPY CourseDemo.Application/CourseDemo.Application.csproj ./CourseDemo.Application/
COPY CourseDemo.Infrastructure/CourseDemo.Infrastructure.csproj ./CourseDemo.Infrastructure/
COPY CourseDemo.WebAPI/CourseDemo.WebAPI.csproj ./CourseDemo.WebAPI/
COPY CourseDemo.Tests/CourseDemo.Tests.csproj ./CourseDemo.Tests/

RUN dotnet restore

COPY . .

RUN dotnet build CourseDemo.WebAPI/CourseDemo.WebAPI.csproj -c Release --no-restore
RUN dotnet publish CourseDemo.WebAPI/CourseDemo.WebAPI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CourseDemo.WebAPI.dll"] 