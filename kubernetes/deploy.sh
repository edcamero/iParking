#!/bin/bash

# Check if required environment variables are set
if [ -z "$NAMESPACE" ]; then
  echo "Error: NAMESPACE environment variable is not set."
  exit 1
fi

if [ -z "$MyServiceConfiguration__Url" ]; then
  echo "Error: MyServiceConfiguration__Url environment variable is not set."
  exit 1
fi

if [ -z "$DOCKER_IMAGE" ]; then
  echo "Error: DOCKER_IMAGE environment variable is not set."
  exit 1
fi

if [ -z "$ConnectionStrings__iParkingConnection" ]; then
  echo "Error: ConnectionStrings__iParkingConnection environment variable is not set."
  exit 1
fi

# Create namespace if it doesn't exist
kubectl create namespace $NAMESPACE --dry-run=client -o yaml | kubectl apply -f -

# Apply ConfigMap
envsubst < kubernetes/iparking-config-map.yaml | kubectl apply -n $NAMESPACE -f -

# Apply Deployment
envsubst < kubernetes/iparking-deploy.yaml | kubectl apply -n $NAMESPACE -f -

# Apply Service
kubectl apply -n $NAMESPACE -f kubernetes/iparking-services.yaml

echo "Deployment completed successfully in namespace $NAMESPACE."
