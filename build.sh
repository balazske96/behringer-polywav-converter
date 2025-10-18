#!/bin/bash

OUT="./build"
mkdir -p "$OUT"

# Windows x64
dotnet publish ./BehringerPolywavConverter.csproj -c Release -r win-x64 --self-contained true \
  /p:PublishSingleFile=true /p:PublishDir="$OUT/" /p:AssemblyName="WingSplitter-win-x64"

# macOS Apple Silicon
dotnet publish ./BehringerPolywavConverter.csproj -c Release -r osx-arm64 --self-contained true \
  /p:PublishSingleFile=true /p:PublishDir="$OUT/" /p:AssemblyName="WingSplitter-osx-arm64"

# macOS Intel
dotnet publish ./BehringerPolywavConverter.csproj -c Release -r osx-x64 --self-contained true \
  /p:PublishSingleFile=true /p:PublishDir="$OUT/" /p:AssemblyName="WingSplitter-osx-x64"

# Linux x64
dotnet publish ./BehringerPolywavConverter.csproj -c Release -r linux-x64 --self-contained true \
  /p:PublishSingleFile=true /p:PublishDir="$OUT/" /p:AssemblyName="WingSplitter-linux-x64"
