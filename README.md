# Kindxt

[![main](https://github.com/sergioprates/kindxt/actions/workflows/pipeline.yml/badge.svg?branch=main)](https://github.com/sergioprates/kindxt/actions/workflows/pipeline.yml)

Kindxt is a wrapper for Kind to easily install common applications on Kind. This is only for local environments and you need to consider some conventions for configurations.

When I was configuring Kind with some applications, I felt some pain to organize all applications that I needed, because Kind on windows asks us to configure the port maps between the host and container. It's easy to do if you have only one application and only need to do this once. I had to do this configuration in many situations and I was tired to do this. So I created this wrapper to do this configuration with the command line so that I don't have to worry about conflicts on the Kind node ports because the configuration will stay centralized and can be shared between projects.

## Getting Started

1. Download the Kindxt from [releases on github](https://github.com/sergioprates/kindxt/releases).
2. Put the executable on PATH environment variable.
3. Run `kindxt --help` to see all options.

# Tools

* Kubernetes Version: v1.26.0
* Kind Version: 0.17.0
* Helm Version: 3.9.1
* Docker Api Version: 1.41

## All options accepted

|  Options | Description | Host Port | NodePort | Username | Password |
|---|---|---|---|---|---|
| -c, --create-cluster  | Delete the cluster and create a new one  | - | - | - | - |
| --sqlserver  | Install sqlserver chart on kind | localhost:1433 | 30000 | sa | P@ssword123 |
| --mongodb | Install mongodb chart on kind  | localhost:27017 | 30008 | desenv | P@ssword123 |
| --postgres | Install postgres chart on kind  | localhost:5432 | 30001 | desenv | P@ssword123 |
| --citus | Install citus chart with 2 nodes on kind | localhost:5433 | 30007 | desenv | P@ssword123 |
| --istio-ingress | Install istio-ingress chart on kind, this create the namespaces `istio-ingress` and `istio-system` | localhost:8081 http, localhost:8082 https, localhost:8083 status-port | 30004, 30005, 30006 | - | - |
| --pg-admin  | Install pgadmin chart on kind | http://localhost:9000 | 30002 | desenv@local.com | P@ssword123 |
| --nginx-ingress | Install nginx-ingress chart on kind | http://localhost:8080 | 30003 | - | - |
| --keda | Install Keda chart on kind, this creates the namespace `keda` | - | - | - | - |
| --rabbitmq | Install RabbitMQ chart on kind | http://localhost:5672 amqp, http://localhost:15672 manager | 30009, 30010 | desenv | P@ssword123 |
| --version | Show version information | - | - | - | - |
| -?, -h, --help | Show help and usage information | - | - | - | - |

## And if I want reinstall some package?

Run the command without the `--create-cluster` parameter and the reinstall will happen.

## How to add new chart?

1. Create a new folder inside the directory `charts`
2. Create a class that inherits `IHelmChart` and `HelmChartBase`
3. Implement the methods and create a config file with the default configurations
4. Create a port mapping in the class `Ports`. The number of the node port should be incremental
5. Update the readme with the parameters, default URL, user, and password if apply
6. Open a pull request

## Roadmap

1. Log the ports opened in the install action
2. Version the package
3. Generate changelog.md
4. Generate a chocolatey package
5. Install Kafka
