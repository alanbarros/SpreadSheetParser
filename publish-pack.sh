#!/bin/bash

#Example: sh publish-pack.sh -l alanbarros -k $(cat ../key.txt) -v 0.0.4

PROJECT_NAME=SpreadSheetParser
VERSION=0.0.5
DIR=src/bin/Release

for x in ${@} # Organiza os argumentos 
do
    case ${x} in
    -l | --login)
        LOGIN=${2}
        shift
        shift
        ;;
    -k | --key)
        KEY=${2}
        shift
        shift
        ;;
    -d | --diretory)
        DIR=${2}
        shift
        shift
        ;;
    -v | --version)
        VERSION=${2}
        shift
        shift
        ;;
    --) shift; 
        break 
        ;;
    esac
done

PACKAGE=${DIR}/${PROJECT_NAME}.${VERSION}.nupkg

# echo ${LOGIN}
# echo ${KEY}
# echo ${DIR}
# echo ${VERSION}
echo ${PACKAGE}

sed "s/{{PAT}}/${KEY}/g" templateNuget.config | sed "s/{{USER}}/${USER}/g" > nuget.config

dotnet clean src/${PROJECT_NAME}.csproj
dotnet restore src/${PROJECT_NAME}.csproj
dotnet build src/${PROJECT_NAME}.csproj
dotnet pack src/${PROJECT_NAME}.csproj --configuration Release

dotnet nuget push "${PACKAGE}" --api-key ${KEY} --source "github"

rm -f nuget.config