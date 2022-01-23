# rig-watcher

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=rig-watcher&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=rig-watcher)
[![Build Status](https://dev.azure.com/jacksonmiller7855/Personal%20Public/_apis/build/status/Jacksondr5.rig-watcher?branchName=main)](https://dev.azure.com/jacksonmiller7855/Personal%20Public/_build/latest?definitionId=27&branchName=main)
[![Docker Hub](https://img.shields.io/docker/v/jacksondr5/rig-watcher)](https://hub.docker.com/repository/docker/jacksondr5/rig-watcher)

A small utility to watch for inactive mining rigs on NiceHash and wake them up. The idea is that this is supposed to be run with some CRON like utility whenever you want. The docker image is currently build for arm64 since I'm running it on a Raspberry Pi based k8s cluster.

## Local Development

This project makes use of [dotnet user-secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows) to store sensitive information. Make sure you follow the [Generating an API key instructions on NiceHash](https://www.nicehash.com/docs/) to generate a key. Once you do that, use `dotnet user-secrets` to add the config values as the [appsettings.json](./appsettings.json) file expects them.
