#!/bin/bash

#Example: sh pkg-helper.sh -l alanbarros -k $(cat ../key.txt) -v 0.0.7

WORK_DIR=src
PATH_CSPROJ=$WORK_DIR/SpreadSheetParser.csproj
PROJECT_NAME=$(xmlstarlet sel -t -v '//PackageId' $PATH_CSPROJ)
VERSION=$(xmlstarlet sel -t -v '//Version' $PATH_CSPROJ)
CONFIG=Release
DIR=$WORK_DIR/bin/$CONFIG

for x in ${@} # Organiza os argumentos 
do
    case ${x} in
    -c | --configuration)
        CONFIG=${2}
        shift 2
        ;;
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
dotnet clean $PATH_CSPROJ
dotnet restore $PATH_CSPROJ
dotnet build $PATH_CSPROJ
dotnet pack $PATH_CSPROJ --configuration $CONFIG -o $DIR

if [ ! -z ${SHOULD_PUBLISH} ]; then # Publica o pacote
echo "Publicando ${PACKAGE} no Github Packages...";
dotnet nuget push "${PACKAGE}" --api-key ${KEY} --source "github"
fi 

echo "Removendo arquivo nuget.config"
rm -f nuget.config
