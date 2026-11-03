FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Exam.Api.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_HTTP_PORTS=8080
ENV EvidencePath=/data/evidence
USER root
RUN mkdir -p /data/evidence && chown -R app:app /data
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "Exam.Api.dll"]
