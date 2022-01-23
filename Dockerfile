# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder
# COPY . .
# RUN dotnet publish -c Release -o /app -r linux-arm64 --self-contained

# FROM --platform=arm64 alpine:latest AS runner
# COPY --from=builder /app .
# WORKDIR /app
# CMD ["./rig-watcher"] 

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder
COPY . .
RUN dotnet publish -c Release -o /app -r linux-arm64 --no-self-contained

FROM --platform=arm64 mcr.microsoft.com/dotnet/runtime:6.0-focal AS runner
WORKDIR /app
COPY --from=builder /app .
CMD ["./rig-watcher"] 