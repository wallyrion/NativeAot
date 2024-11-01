# This repository is example of performance benchmark setup for 2 web applications.

## How to run this repository locally
    docker-compose up --build


## The following components are included:

For .net apps new Chiseled images is introduced that allows to reduce image size dramatically.

### WebAppNoAot 
Simple web app with single endpoint /todos.
Compiled in .NET 9 for Chiseled amd64 noble image. Image is 120MB
### WebAppAot
Simple web app with single endpoint /todos. Compiled in .NET 9 AOT for Chiseled amd64 noble image. Image is only 30MB
### K6
Used for simulating concurrent work. Real time k6 metrics are sent to Prometheus server using [remote-write](https://grafana.com/docs/k6/latest/results-output/real-time/prometheus-remote-write/) feature (**_in preview_**)
### Grafana
Have single Prometheus data source and multiple dashboards:
- [ASP.NET Core](https://grafana.com/grafana/dashboards/19924-asp-net-core/) official dashboard by .NET team 
- [K6 Prometheus](https://grafana.com/grafana/dashboards/19665-k6-prometheus/) official dashboard
- [Docker cAdvisor](https://grafana.com/grafana/dashboards/13946-docker-cadvisor/) official dashboard
All dashboards are provisioned with docker-compose
### Prometheus
- Collects metrics from cAdvisor
- Collects metrics from .NET web app1 both apps
### CAdvisor
Allows to collect metrics about docker containers (cpu usage, memory usage, etc.)
### Aspire dashboard
Allows to see default .net [build-in](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/built-in-metrics) metrics that are send to standalone dashboard via OpenTelemetry protocol.