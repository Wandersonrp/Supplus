# 🚀 Supplus [🚧 Em desenvolvimento]


**Sistema de Tickets de Suporte em Tempo Real**

O **Sup+** é um projeto de um sistema de Help Desk moderno, focado em comunicação em tempo real entre o usuário final (Cliente) e o agente de suporte. Desenvolvido com as tecnologias mais recentes do ecossistema .NET.

## ✨ Tecnologias Utilizadas

| Categoria | Tecnologia | Função |
| :--- | :--- | :--- |
| **Frontend** | **Blazor WebAssembly** | Interface do Usuário (Admin,Cliente e Agente) e Lógica de Apresentação. |
| **Mobile Android** | **Blazor Hybrid** | Interface do Usuário (Admin,Cliente e Agente) e Lógica de Apresentação. |
| **Backend** | **ASP.NET Core Web API** | Lógica de Negócio, Autenticação (JWT), Persistência de Dados. |
| **Comunicação** | **SignalR** | Comunicação em tempo real (Chat) entre Cliente e Agente. |
| **Persistência** | **Entity Framework Core** | ORM para comunicação com o Banco de Dados. |
| **UI/UX** | **MudBlazor** | Biblioteca de componentes UI baseada em Material Design para o Blazor. |

## 🔑 Funcionalidades Principais

O projeto implementa as funcionalidades essenciais de um sistema de tickets:

*   **Autenticação Segura:** Login e Registro para Clientes e Agentes.
*   **Criação de Tickets:** Clientes podem abrir novos chamados, definindo Título, Descrição, Categoria e Prioridade.
*   **Chat em Tempo Real:** Comunicação instantânea via SignalR dentro do ticket.
*   **Gestão de Tickets:** Agentes podem visualizar a fila de tickets, assumir chamados, alterar status (Aberto, Em Atendimento, Resolvido, Fechado) e editar prioridades.
*   **Perfis de Acesso:** Distinção clara entre **Cliente**, **Agente de Suporte** e **Administrador**.

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas, inspirada no **Clean Architecture**, para garantir a manutenibilidade e a escalabilidade.

*   **Domínio:** Contém as entidades e regras de negócio puras.
*   **Aplicação:** Contém a lógica de negócio (Use Cases/Services).
*   **Infraestrutura:** Responsável pela persistência de dados (EF Core) e serviços externos.
*   **API:** Camada de apresentação do Backend (Controllers e SignalR Hubs).
*   **Client:** Camada de apresentação do Frontend (Blazor Components).

---


