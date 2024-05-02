FROM mcr.microsoft.com/dotnet/aspnet:8.0.4-bookworm-slim

COPY ./output /app
WORKDIR /app

ARG git_commit
ENV git_commit=$git_commit
ENV DD_VERSION=$git_commit

EXPOSE 5000

ENTRYPOINT ["dotnet", "BottleSplitter.dll"]
