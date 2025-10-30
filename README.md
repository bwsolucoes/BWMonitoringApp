# BWMonitoringApp

Uma aplicação leve para a coleta, armazenamento local e envio para o Datadog de métricas de uso da máquina.

--- 

## Tabela de Conteúdo
- [Visão Geral](#visão--geral)
- [Características](#características)
- [Requerimentos](#requerimentos)
- [Instalação](#instalação)
- [Configuração](#configuração)

---

---

## Visão Geral

BWMonitoringApp é uma aplicação de monitoramento com o objetivo de coletar métricas de uso da máquina do usuário, salvá-las num banco de dados local, e enviá-las para o Datadog via requests HTTP, para a equipe poder monitorar a performance das máquinas. A aplicação tem objetivo de ser leve, e rodar em background, para não prejudicar o usuário.

---

## Características

* Coleta de diversas métricas de performance
  - Uso de CPU
  - RAM total, usada e disponível
  - Uso de Rede
  - Uso de Disco
  - Armazenamento total, ocupado e livre
  - Uptime da máquina
* Armazenamento das métricas coletadas com [LiteDB](https://www.litedb.org/)
* Envio das métricas para o Datadog, via requests HTTP para os endpoints da API do Datadog

---

## Requerimentos

- .NET SDK 8.0
- Sistema Operacional Windows

---

## Instalação

A aplicação pode ser instalada com a execução do instalador .exe.
Também pode ser instalada via terminal, fornecendo os dados de configuração. Rodar o comando no diretório do instalador.
```sh
BW MonitorApp Instaler.exe /silent /APIKEY=YOUR-API-KEY /APPKEY=YOUR-APP-KEY /URL=YOUR-DATADOG-DOMAIN-URL
```

---

## Configuração

Para o funcionamento da aplicação, é necessário fornecer as seguintes informações do Datadog do usuário: 
- Chave de API
- Chave de Aplicação
- Url do site do Datadog
Estas informações podem ser fornecidas durante a instalação, em uma página do instalador.
