# Create Environment

Copy the template for environment variables:

`copy template-env-variables.bat env-variables-myserver.bat`

Edit `env-variables-myserver.bat` adding the configuration for the new enviroment and launch it.

`env-variables-myserver.bat`

Create the environment in the AKS cluster:

```bat
kubectl create namespace %NAMESPACE%
envsubst -i iparking-config-map.yaml | kubectl apply -f - -n %NAMESPACE%
envsubst -i iparking-deploy.yaml | kubectl apply -f - -n %NAMESPACE%
envsubst -i iparking-service.yaml | kubectl apply -f - -n %NAMESPACE%
```