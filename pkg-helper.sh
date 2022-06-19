#!/bin/bash

#Example: sh pkg-helper.sh -l alanbarros -k $(cat ../key.txt) -v 0.0.7

PROJECT_NAME=SpreadSheetParser
VERSION=0.0.5
DIR=src/bin/Release

for x in ${@} # Organiza os argumentos 
do
    case ${x} in
    -l | --login)
        LOGIN=${2}
        shift 2
        ;;
    -k | --key)
        KEY=${2}
        shift 2
        ;;
    -p | --publish)
        SHOULD_PUBLISH=1
        shift
        ;;
    -d | --diretory)
        DIR=${2}
        shift 2
        ;;
    -v | --version)
        VERSION=${2}
        shift 2
        ;;
    --) shift; 
        break 
        ;;
    esac
done

PACKAGE=${DIR}/${PROJECT_NAME}.${VERSION}.nupkg

# echo ${LOGIN}
# echo ${KEY}
echo "Substituindo tokens e criando arquivo nuget.config"
sed "s/{{PAT}}/${KEY}/g" templateNuget.config | sed "s/{{USER}}/${USER}/g" > nuget.config

echo "Limpando, restaurando, compilando e empacotando o projeto"
dotnet clean src/${PROJECT_NAME}.csproj
dotnet restore src/${PROJECT_NAME}.csproj
dotnet build src/${PROJECT_NAME}.csproj
dotnet pack src/${PROJECT_NAME}.csproj --configuration Release

if [ ! -z ${SHOULD_PUBLISH} ]; then # Publica o pacote
echo "Publicando ${PACKAGE} no Github Packages...";
dotnet nuget push "${PACKAGE}" --api-key ${KEY} --source "github"
fi 

echo "Removendo arquivo nuget.config"
rm -f nuget.config
