#!/bin/bash

BASEDIR=$(pwd)
TAG=$(git describe --exact-match)

if [[ ! ${TAG} =~ ^v[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
   echo "The tag ${TAG} isn't a valid version tag."
   exit
fi

VERSION=${TAG#v}

cd ${BASEDIR}/authorization-server/docker
docker build -t "openmhealth/omh-dsu-authorization-server:latest" .
docker build -t "openmhealth/omh-dsu-authorization-server:${VERSION}" .
docker push "openmhealth/omh-dsu-authorization-server:latest"
docker push "openmhealth/omh-dsu-authorization-server:${VERSION}"

cd ${BASEDIR}/resource-server/docker
docker build -t "openmhealth/omh-dsu-resource-server:latest" .
docker build -t "openmhealth/omh-dsu-resource-server:${VERSION}" .
docker push "openmhealth/omh-dsu-resource-server:latest"
docker push "openmhealth/omh-dsu-resource-server:${VERSION}"
