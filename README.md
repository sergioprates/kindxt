# kindxt
Kindxt is a wrapper for kind to easyly install common applications on kind. This is only for local environments and consider some convetions for configurations.

When I was configuring kind with some applications, I feel some pain to organize all applications that I need because kind on windows ask us to configure the port maps between host and container. It's to do if you has only one application and only need to do this once. I needed to make this in various situations and I'm tired to do this. So I created this wrapper to do this configuration with the command line and I don't have to worry about conflicts on the kind node ports.

## Getting Started

1. Download the kindxt from releases on github
2. Put the executable on PATH environment variable
3. Run kindxt --help to see all options

## All options accepted

|  Options | Description | Host Port | NodePort | Username | Password |
|---|---|---|---|---|---|
| -c, --create-cluster  | Delete the cluster and create a new one  | - | - | - | - |
| -sql, --sqlserver  | Install sqlserver chart on kind | localhost:1433 | 30000 | sa | P@ssword123 |
| -pssql, --postgres | Install postgres chart on kind  | localhost:5432 | 30001 | desenv | P@ssword123 |
| -pssql-admin, --pgAdmin  | Install pgadmin chart on kind | http://localhost:9000 | 30002 | desenv@local.com | P@ssword123 |
| -nginx, --nginx-ingress | Install nginx-ingress chart on kind | http://localhost:8080 | 30003 | - | - |
| --version | Show version information | - | - | - | - |
| -?, -h, --help | Show help and usage information | - | - | - | - |

## If I want reinstall some package?

Run the command without the --create-cluster parameter and the reinstall will happen.

## Roadmap

1. Log the ports opened in install action
2. Version the package
3. Generate changelog.md
4. Generate chocolatey package
5. Install kafka