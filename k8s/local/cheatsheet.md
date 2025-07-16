# Cheat Sheet for local testing and usage
### Open a service tunnel for local access
`minkikube service <service-name>`

### Support for LoadBalancer service type
`minikube tunnel`

### Use the docker daemon inside minikube
`& minikube -p minikube docker-env (--unset) --shell powershell | Invoke-Expression`

### Docker compose
`docker compose -p Bison up (--build)`
`docker compose build`
`docker compose -p Bison down`

### Kustomize
`kubectl kustomize <path to overlay folder>`
`kubectl apply -k <path to overlay folder>`

### Dashboard
- Add kubernetes-dashboard repository
`helm repo add kubernetes-dashboard https://kubernetes.github.io/dashboard/`
- Deploy a Helm Release named "kubernetes-dashboard" using the kubernetes-dashboard chart
`helm upgrade --install kubernetes-dashboard kubernetes-dashboard/kubernetes-dashboard --create-namespace --namespace kubernetes-dashboard`
- Create user
`kubectl apply -f dashboard.yaml`
- Get token
`kubectl -n kubernetes-dashboard create token admin-user`